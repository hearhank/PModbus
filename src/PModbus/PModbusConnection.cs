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


        private readonly ConcurrentQueue<PModbusItem> writesQueue = new ConcurrentQueue<PModbusItem>();

        private readonly List<PModbusItem> readList = new List<PModbusItem>();

        public int WriteCount => WritesQueue.Count;

        public ConcurrentQueue<PModbusItem> WritesQueue { get => writesQueue; }
        public List<PModbusItem> ReadList { get => readList; }

        public void AddToRead(PModbusItem item)
        {
            readList.Add(item);
        }

        public void AddToWrite(PModbusItem item)
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
