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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="count">-1表示一直读，0表示停止读，</param>
        /// <param name="storeAction"></param>
        void SetItem(int groupID, int count, Action<ushort[]> storeAction = null);
        void Close();

        int WriteCount { get; }
    }
}
