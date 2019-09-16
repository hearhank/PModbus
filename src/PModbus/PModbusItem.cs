using Modbus.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PModbus
{
    public enum PModbusType
    {
        Input,
        Hold
    }
    public class PModbusItem : IEnumerable<UInt16>
    {
        public PModbusItem(ushort index, ushort length, PModbusType modbusData = PModbusType.Input)
        {
            StartAddress = index;
            Length = length;
            if (modbusData == PModbusType.Input)
                ModbusDataType = ModbusDataType.InputRegister;
            else
                ModbusDataType = ModbusDataType.HoldingRegister;
        }
        public PModbusItem(ushort index, IEnumerable<ushort> datas)
        {
            StartAddress = index;
            m_datas.AddRange(datas);
            Length = (ushort)m_datas.Count;
            ModbusDataType = ModbusDataType.InputRegister;//Only for Write
        }
        List<ushort> m_datas = new List<ushort>();
        public IEnumerator<ushort> GetEnumerator()
        {
            return m_datas.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public ushort StartAddress { get; set; }
        public ushort Length { get; set; }
        public ModbusDataType ModbusDataType { get; set; }
    }
}
