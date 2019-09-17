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
        private List<IPModbusReadItem> disabledItems = new List<IPModbusReadItem>();

        public int WriteCount => WritesQueue.Count;

        public ConcurrentQueue<IPModbusWriteItem> WritesQueue { get => writesQueue; }
        public List<IPModbusReadItem> ReadList { get => enableItems; }

        public void AddToRead(IPModbusReadItem item)
        {
            item.EnabledChanged += Item_EnabledChanged;
            if (item.Enabled)
                enableItems.Add(item);
            else
                disabledItems.Add(item);
        }

        private void Item_EnabledChanged(object sender, EventArgs e)
        {
            if(sender is PModbusReadItem ritem)
            {
                Debug.WriteLine(ritem);
            }
        }

        public void SetItem(int groupID, bool isEnabled, Action<ushort[]> storeAction = null)
        {
            if (isEnabled)
            {
                var dItems = disabledItems.Where(x => x.GroupID == groupID);
                foreach (var item in dItems)
                {
                    item.Enabled = true;
                    item.StoreAction = storeAction;
                }
                enableItems.AddRange(dItems.ToArray());
                disabledItems.RemoveAll(x => x.Enabled);
            }
            else
            {
                var eItems = enableItems.Where(x => x.GroupID == groupID);
                foreach (var item in eItems)
                {
                    item.Enabled = false;
                }
                disabledItems.AddRange(eItems.ToArray());
                enableItems.RemoveAll(x => !x.Enabled);
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
