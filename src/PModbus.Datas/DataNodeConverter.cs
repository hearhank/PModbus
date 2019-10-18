using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PModbus.Datas
{
    public class DataNodeConverter
    {
        public object ConvertTo<T>(ushort[] datas, bool reversal) where T : IConvertible
        {
            List<byte> bList = new List<byte>();
            if (reversal)
            {
                foreach (var item in datas.Reverse())
                {
                    bList.AddRange(BitConverter.GetBytes(item).Reverse());
                }
            }
            else
            {
                Array.ForEach(datas, d =>
                {
                    bList.AddRange(BitConverter.GetBytes(d));
                });
            }

            var bytes = bList.ToArray();
            object result = null;
            T t = default(T);
            switch (t.GetTypeCode())
            {
                case TypeCode.Boolean:
                    result = BitConverter.ToBoolean(bytes, 0);
                    break;
                case TypeCode.Char:
                    result = BitConverter.ToChar(bytes, 0);
                    break;
                case TypeCode.Int16:
                    result = BitConverter.ToInt16(bytes, 0);
                    break;
                case TypeCode.UInt16:
                    result = BitConverter.ToUInt16(bytes, 0);
                    break;
                case TypeCode.Int32:
                    result = BitConverter.ToInt32(bytes, 0);
                    break;
                case TypeCode.UInt32:
                    result = BitConverter.ToUInt32(bytes, 0);
                    break;
                case TypeCode.Single:
                    result = BitConverter.ToSingle(bytes, 0);
                    break;
                case TypeCode.Int64:
                    result = BitConverter.ToInt64(bytes, 0);
                    break;
                case TypeCode.UInt64:
                    result = BitConverter.ToUInt64(bytes, 0);
                    break;
                case TypeCode.Double:
                    result = BitConverter.ToDouble(bytes, 0);
                    break;
                case TypeCode.String:
                    result = Encoding.Unicode.GetString(bytes);
                    break;
            }

            return result;
        }

        public ushort[] ConvertFrom<T>(object data, bool reversal) where T : IConvertible
        {
            List<byte> bList = new List<byte>();
            T t = default(T);
            switch (t.GetTypeCode())
            {
                case TypeCode.Boolean:
                    bList.AddRange(BitConverter.GetBytes(Convert.ToBoolean(data)));
                    break;
                case TypeCode.Char:
                    bList.AddRange(BitConverter.GetBytes(Convert.ToChar(data)));
                    break;
                case TypeCode.Int16:
                    bList.AddRange(BitConverter.GetBytes(Convert.ToInt16(data)));
                    break;
                case TypeCode.UInt16:
                    bList.AddRange(BitConverter.GetBytes(Convert.ToUInt16(data)));
                    break;
                case TypeCode.Int32:
                    bList.AddRange(BitConverter.GetBytes(Convert.ToInt32(data)));
                    break;
                case TypeCode.UInt32:
                    bList.AddRange(BitConverter.GetBytes(Convert.ToUInt32(data)));
                    break;
                case TypeCode.Single:
                    bList.AddRange(BitConverter.GetBytes(Convert.ToSingle(data)));
                    break;
                case TypeCode.Int64:
                    bList.AddRange(BitConverter.GetBytes(Convert.ToInt64(data)));
                    break;
                case TypeCode.UInt64:
                    bList.AddRange(BitConverter.GetBytes(Convert.ToUInt64(data)));
                    break;
                case TypeCode.Double:
                    bList.AddRange(BitConverter.GetBytes(Convert.ToDouble(data)));
                    break;
                case TypeCode.String:
                    bList.AddRange(Encoding.Unicode.GetBytes(data?.ToString()));
                    break;
            }
            int bytesLen = bList.Count;
            if (bList.Count % 2 == 1)
                bytesLen = bList.Count + 1;
            var bytes = new byte[bytesLen];
            var temp = bList.ToArray();
            if (reversal)
                temp = temp.Reverse().ToArray();
            Array.Copy(temp, bytes, temp.Length);

            List<ushort> result = new List<ushort>();
            for (int i = 0; i < bytes.Length; i += 2)
            {
                result.Add(BitConverter.ToUInt16(bytes, i));
            }

            return result.ToArray();
        }
    }
}
