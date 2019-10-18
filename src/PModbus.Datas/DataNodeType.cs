using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PModbus.Datas
{
    public enum DataNodeType
    {
        Boolean,
        Bit,
        Short,
        UShort,
        Int,
        UInt,
        Float,
        Long,
        ULong,
        Double,
        String,
    }
    public enum DataNodeStore
    {
        Input,
        Output
    }
}
