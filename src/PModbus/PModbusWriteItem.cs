using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modbus.Data;

namespace PModbus
{
    public class PModbusWriteItem : IPModbusWriteItem
    {
        public PModbusWriteItem(ushort index, IEnumerable<ushort> datas)
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
        public ModbusDataType ModbusDataType { get; private set; }
    }
}
