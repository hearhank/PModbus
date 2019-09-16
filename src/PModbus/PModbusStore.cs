using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Modbus.Data;

namespace PModbus
{
    public class PModbusStore : IPModbusStore
    {
        public PModbusStore(ref ushort[] inputs, ref ushort[] outputs)
        {
            this.Inputs = inputs;
            this.Outputs = outputs;
        }
        public ushort[] Inputs;
        public ushort[] Outputs;
        public void Store(ModbusDataType modbusData, ushort startAddress, IEnumerable<ushort> datas)
        {
            if (modbusData == ModbusDataType.InputRegister)
            {
                Array.Copy(datas.ToArray(), 0, Inputs, startAddress, datas.Count());
            }
            else if (modbusData == ModbusDataType.HoldingRegister)
            {
                Array.Copy(datas.ToArray(), 0, Outputs, startAddress, datas.Count());
            }
            else
            {
                throw new NotFiniteNumberException();
            }
            //Debug.WriteLine(modbusData);
            //Debug.WriteLine(string.Format("{0}~:{1} >>{2}...", startAddress, datas.Count(), string.Join(" ", datas.Take(3))));
        }
    }
}
