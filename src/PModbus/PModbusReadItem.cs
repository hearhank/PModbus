using Modbus.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PModbus
{

    public class PModbusReadItem : IPModbusReadItem
    {
        public PModbusReadItem(ushort index, ushort length, PModbusType modbusData = PModbusType.Input)
        {
            StartAddress = index;
            Length = length;
            if (modbusData == PModbusType.Input)
                ModbusDataType = ModbusDataType.InputRegister;
            else
                ModbusDataType = ModbusDataType.HoldingRegister;
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

        public int GroupID { get => _groupID; set => _groupID = value; }
        public bool Enabled { get => _enabled; set => _enabled = value; }
        public int DelayCount { get => _delayCount; set => _delayCount = value; }

        public Action<ushort[]> StoreAction { get => _storeAction; set => _storeAction = value; }

        private int _number = 0;
        private Action<ushort[]> _storeAction = null;
        private int _delayCount = 0;
        private bool _enabled = true;
        private int _groupID = -1;

        public bool CountTo()
        {
            if (++_number > DelayCount)
            {
                _number = 0;
                return true;
            }
            return false;
        }
    }
}
