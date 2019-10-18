using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PModbus.Datas
{
    public class DataNodeManager
    {
        public DataNodeManager(ref ushort[] inputs, ref ushort[] outputs)
        {
            this.Inputs = inputs;
            this.Outputs = outputs;
        }
        public ushort[] Inputs { get; private set; }
        public ushort[] Outputs { get; private set; }

        public DataNodeConverter convert;
        public void Read(IDataNode dd)
        {
            ushort[] temp = new ushort[dd.Length];
            if (dd.DataNodeStore == DataNodeStore.Input)
            {
                Array.Copy(this.Inputs, dd.StartAddress, temp, 0, temp.Length);
            }
            else
            {
                Array.Copy(this.Outputs, dd.StartAddress, temp, 0, temp.Length);
            }
            switch (dd.DataNodeType)
            {
                case DataNodeType.Boolean:
                    dd.Value = convert.ConvertTo<bool>(temp, DataNode.Reversal);
                    break;
                case DataNodeType.Bit:
                    ushort pval = (ushort)convert.ConvertTo<ushort>(temp, DataNode.Reversal);
                    dd.Value = GetBitValue(pval, dd.Offset, dd.SubLength);
                    break;
                case DataNodeType.Short:
                    dd.Value = convert.ConvertTo<Int16>(temp, DataNode.Reversal);
                    break;
                case DataNodeType.UShort:
                    dd.Value = convert.ConvertTo<UInt16>(temp, DataNode.Reversal);
                    break;
                case DataNodeType.Int:
                    dd.Value = convert.ConvertTo<Int32>(temp, DataNode.Reversal);
                    break;
                case DataNodeType.UInt:
                    dd.Value = convert.ConvertTo<UInt32>(temp, DataNode.Reversal);
                    break;
                case DataNodeType.Float:
                    dd.Value = convert.ConvertTo<float>(temp, DataNode.Reversal);
                    break;
                case DataNodeType.Long:
                    dd.Value = convert.ConvertTo<Int64>(temp, DataNode.Reversal);
                    break;
                case DataNodeType.ULong:
                    dd.Value = convert.ConvertTo<UInt64>(temp, DataNode.Reversal);
                    break;
                case DataNodeType.Double:
                    dd.Value = convert.ConvertTo<double>(temp, DataNode.Reversal);
                    break;
                case DataNodeType.String:
                    dd.Value = convert.ConvertTo<string>(temp, DataNode.Reversal);
                    break;
                default:
                    throw new NotImplementedException();
            }

        }

        private ushort GetBitValue(ushort data, int offset, int subLength)
        {
            var index = offset + subLength;
            var t1 = data & ((1 << index) - 1);
            return Convert.ToUInt16(t1 >> offset);
        }
        private ushort SetBitValue(ushort data, int offset, int length = 1, ushort val = 1)
        {
            if (val > Math.Pow(2, length))
                throw new Exception("value is too large.");

            var index = offset + length;
            var t1 = data >> index;
            t1 <<= index;
            var t2 = val << offset;
            var t3 = data & ((1 << offset) - 1);
            return Convert.ToUInt16(t1 | t2 | t3);
        }


        public void Write(DataNode dd, object val)
        {
            ushort[] temp = new ushort[dd.Length];
            switch (dd.DataNodeType)
            {
                case DataNodeType.Boolean:
                    temp[0] = (ushort)(Convert.ToBoolean(val) ? 1 : 0);
                    break;
                case DataNodeType.Bit:
                    temp[0] = (ushort)Convert.ToUInt16(val);
                    temp[0] = SetBitValue(this.Inputs[dd.StartAddress], dd.Offset, dd.SubLength, temp[0]);
                    break;
                case DataNodeType.Short:
                    temp[0] = (ushort)Convert.ToUInt16(val);
                    break;
                case DataNodeType.UShort:
                    temp[0] = (ushort)Convert.ToUInt16(val);
                    break;
                case DataNodeType.Int:
                     var ival = (int)Convert.ToInt32(val);

                    break;
                case DataNodeType.UInt:
                    break;
                case DataNodeType.Float:
                    break;
                case DataNodeType.Long:
                    break;
                case DataNodeType.ULong:
                    break;
                case DataNodeType.Double:
                    break;
                case DataNodeType.String:
                    break;
                default:
                    break;
            }
        }
    }
}
