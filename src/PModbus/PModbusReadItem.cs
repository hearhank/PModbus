using Modbus.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        public int ReadCount
        {
            get => _readcount;
            set
            {
                if (_readcount != value)
                {
                    if (_readcount == 0 || value == 0)
                    {
                        ReadCountChanged?.Invoke(this, new ReadCountEventArgs(value));
                    }
                    _readcount = value;
                }
            }
        }
        public int DelayCount { get => _delayCount; set => _delayCount = value; }

        public Action<ushort[]> StoreAction { get => _storeAction; set => _storeAction = value; }

        private int _number = 0;
        private Action<ushort[]> _storeAction = null;
        private int _delayCount = 0;
        private int _readcount = -1;
        private int _groupID = -1;

        public event EventHandler<ReadCountEventArgs> ReadCountChanged;

        public bool CountTo()
        {
            if (++_number > DelayCount)
            {
                _number = 0;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return string.Join(AppNormalData.Split, new string[] { "Start Address=" + StartAddress, "Length=" + Length, "GroupID=" + GroupID });
        }
    }
}
