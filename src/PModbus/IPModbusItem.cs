using Modbus.Data;
using System;
using System.Collections.Generic;

namespace PModbus
{
    public enum PModbusType
    {
        Input,
        Hold
    }
    public interface IPModbusItem : IEnumerable<UInt16>
    {
        ushort Length { get; set; }
        ModbusDataType ModbusDataType { get;  }
        ushort StartAddress { get; set; }
    }

    public interface IPModbusReadItem : IPModbusItem
    {
        int GroupID { get; set; }
        bool Enabled { get; set; }
        int DelayCount { get; set; }

        Action<ushort[]> StoreAction { get; set; }
        bool CountTo();
    }
    public interface IPModbusWriteItem : IPModbusItem
    {
        //new ModbusDataType ModbusDataType { get; }
    }
}