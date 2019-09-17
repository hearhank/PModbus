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

        ConcurrentQueue<IPModbusWriteItem> WritesQueue { get; }
        List<IPModbusReadItem> ReadList { get; }

        void AddToRead(IPModbusReadItem item);
        void AddToWrite(IPModbusWriteItem item);
        void SetItem(int groupID, bool isEnabled, Action<ushort[]> storeAction = null);
        void Close();

        int WriteCount { get; }
    }
}
