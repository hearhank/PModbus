using Modbus.Device;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PModbus
{
    public interface IPModbusConnection : IDisposable
    {
        byte SlaveID { get; }
        bool Connect(ref IModbusMaster master);
        string Name { get; }

        ConcurrentQueue<PModbusItem> WritesQueue { get; }
        List<PModbusItem> ReadList { get; }

        void AddToRead(PModbusItem item);
        void AddToWrite(PModbusItem item);
        void Close();

        int WriteCount { get; }
    }
}
