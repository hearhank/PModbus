using System.IO.Ports;
using Modbus.Device;

namespace PModbus
{
    public sealed class SerialPortDevice : IConnectDevice
    {
        public string ComPort { get; set; }
        public int BandRate { get; set; }
        public int Timeout { get; set; }
        SerialPort SerialPort;
        public string Name => "RS232";
        public void Close()
        {
            SerialPort?.Close();
        }

        public void Dispose()
        {
            SerialPort?.Dispose();
        }

        public bool Open(ref IModbusMaster master)
        {
            if (SerialPort == null || !SerialPort.IsOpen)
            {
                SerialPort = new SerialPort(ComPort, BandRate, Parity.None, 8, StopBits.One)
                {
                    RtsEnable = false,
                    Handshake = Handshake.None,
                    ReceivedBytesThreshold = 1,
                    DtrEnable = false,
                    ReadTimeout = Timeout,
                    WriteTimeout = Timeout
                };
                SerialPort.Open();
                SerialPort.DiscardInBuffer();
                SerialPort.DiscardOutBuffer();

                master = ModbusSerialMaster.CreateRtu(SerialPort);
            }

            return SerialPort.IsOpen;
        }

        public override string ToString()
        {
            return string.Join(AppNormalData.Split, new string[] { "Name=" + Name, "ComPort=" + ComPort, "BandRate=" + BandRate, "Timeout=" + Timeout.ToString() });
        }
    }

}