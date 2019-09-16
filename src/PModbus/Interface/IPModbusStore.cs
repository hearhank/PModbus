using Modbus.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PModbus
{
    public interface IPModbusStore
    {
        void Store(ModbusDataType modbusData, ushort startAddress, IEnumerable<ushort> datas);
    }
}
