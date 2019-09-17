using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Modbus.Device;

namespace PModbus
{
    public sealed class PModbusConnection : IPModbusConnection
    {
        IConnectDevice Device;
        public PModbusConnection(IConnectDevice device)
        {
            this.Device = device;
        }
        public byte SlaveID => 1;

        public string Name { get => Device?.Name; }


        private readonly ConcurrentQueue<IPModbusWriteItem> writesQueue = new ConcurrentQueue<IPModbusWriteItem>();

        private readonly List<IPModbusReadItem> enableItems = new List<IPModbusReadItem>();

        public int WriteCount => WritesQueue.Count;

        public ConcurrentQueue<IPModbusWriteItem> WritesQueue { get => writesQueue; }
        public List<IPModbusReadItem> ReadList { get => enableItems; }

        public void AddToRead(IPModbusReadItem item)
        {
            item.ReadCountChanged += Item_ReadCountChanged;
            enableItems.Add(item);
        }

        private void Item_ReadCountChanged(object sender, ReadCountEventArgs e)
        {
            if (sender is PModbusReadItem ritem)
            {
                Debug.WriteLine(ritem + ";Read Count=" + e.Value);
            }
        }

        public void SetItem(int groupID, int count, Action<ushort[]> storeAction = null)
        {
            var items = enableItems.Where(x => x.GroupID == groupID);
            foreach (var item in items)
            {
                item.ReadCount = count;
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
