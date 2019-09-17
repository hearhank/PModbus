using System.Net.Sockets;
using Modbus.Device;

namespace PModbus
{
    public sealed class EthernetDevice : IConnectDevice
    {
        public EthernetDevice()
        {
            Port = 502;
        }
        public string Name => "Ethernet";

        public string IPAddr { get; set; }
        public int Port { get; set; }
        public int Timeout { get; set; }


        private TcpClient TcpClient;
        public bool Open(ref IModbusMaster master)
        {
            if (TcpClient == null || !TcpClient.Connected)
            {
                TcpClient = new TcpClient();
                TcpClient.SendTimeout = Timeout;
                TcpClient.ReceiveTimeout = Timeout;
                TcpClient.Connect(IPAddr, Port);

                master = ModbusIpMaster.CreateIp(TcpClient);
            }
            return TcpClient.Connected;
        }
        public void Dispose()
        {
            Close();
            TcpClient = null;
        }

        public void Close()
        {
            if (TcpClient != null && TcpClient.Connected)
            {
                TcpClient.Close();
            }
        }

        public override string ToString()
        {
            return string.Join(AppNormalData.Split, new string[] { "Name=" + Name, "IP Address=" + IPAddr, "Port=" + Port.ToString(), "Timeout=" + Timeout.ToString() });
        }
    }

}