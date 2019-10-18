using Modbus.Data;
using System;
using System.Collections.Generic;

namespace PModbus
{
    public interface IPModbusItem : IEnumerable<UInt16>
    {
        ushort Length { get; set; }
        ModbusDataType ModbusDataType { get;  }
        ushort StartAddress { get; set; }
    }

    public class ReadCountEventArgs:EventArgs
    {
        public ReadCountEventArgs(int val)
        {
            Value = val;
        }
        public int Value { get; set; }
    }

    public interface IPModbusReadItem : IPModbusItem
    {
        event EventHandler<ReadCountEventArgs> ReadCountChanged;
        int GroupID { get; set; }
        int ReadCount { get; set; }
        int DelayCount { get; set; }

        Action<ushort[]> StoreAction { get; set; }
        bool CountTo();
    }
    public interface IPModbusWriteItem : IPModbusItem
    {
        //new ModbusDataType ModbusDataType { get; }
    }
}