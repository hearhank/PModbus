using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PModbus
{
    interface IPModbusClient
    {
        IPModbusConnection Connection { get; }
        Exception Exception { get; }
        void Start();
        void Stop();

        void Write(ushort offset, IEnumerable<ushort> datas);

        void ChangeDevice(IConnectDevice device);
    }
}
