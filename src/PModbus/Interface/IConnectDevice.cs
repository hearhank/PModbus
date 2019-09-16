using System;

namespace PModbus
{
    public interface IConnectDevice : IDisposable
    {
        string Name { get; }
        int Timeout { get; set; }
        bool Open(ref Modbus.Device.IModbusMaster master);
        void Close();
    }
}