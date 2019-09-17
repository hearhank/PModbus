using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modbus.Device;

namespace PModbus
{
    public class PModbusConnection : IPModbusConnection
    {
        IConnectDevice Device;
        public PModbusConnection(IConnectDevice device)
        {
            this.Device = device;
        }
        public byte SlaveID => 1;

        public string Name { get => Device?.Name; }


        private readonly ConcurrentQueue<IPModbusWriteItem> writesQueue = new ConcurrentQueue<IPModbusWriteItem>();

        private readonly List<IPModbusReadItem> readList = new List<IPModbusReadItem>();

        public int WriteCount => WritesQueue.Count;

        public ConcurrentQueue<IPModbusWriteItem> WritesQueue { get => writesQueue; }
        public List<IPModbusReadItem> ReadList { get => readList; }

        public void AddToRead(IPModbusReadItem item)
        {
            readList.Add(item);
        }
        public void SetItem(int groupID, bool isEnabled, Action<ushort[]> storeAction = null)
        {
            var item = readList.FirstOrDefault(x => x.GroupID == groupID);
            if (item != null)
            {
                item.Enabled = isEnabled;
                item.StoreAction = storeAction;
            }
        }
        public void AddToWrite(IPModbusWriteItem item)
        {
            WritesQueue.Enqueue(item);
        }

        public bool Connect(ref IModbusMaster master)
        {
            return Device?.Open(ref master) == true;
        }

        public void Close()
        {
            Device?.Close();
        }

        public void Dispose()
        {
            Device?.Dispose();
        }

        public override string ToString()
        {
            return string.Join(AppNormalData.Split, new string[] { Device.ToString(), "SlaveID=" + SlaveID.ToString() });
        }
    }
}
