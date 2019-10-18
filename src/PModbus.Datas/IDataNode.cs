using System;

namespace PModbus.Datas
{
    public interface IDataNode
    {
        DataNodeStore DataNodeStore { get; set; }
        DataNodeType DataNodeType { get; set; }
        double Factory { get; set; }
        Func<double> FactoryAction { get; set; }
        int Length { get; set; }
        int Offset { get; set; }
        int StartAddress { get; set; }
        int SubLength { get; set; }
        object Value { get; set; }
    }
}