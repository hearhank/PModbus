using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PModbus.Datas
{
    public class DataNode : IDataNode
    {
        private string unit;
        private double factory = 1d;
        private object _value;

        public static bool Reversal { get; set; }

        public DataNodeStore DataNodeStore { get; set; }
        public string Name { get; set; }
        public DataNodeType DataNodeType { get; set; }
        public int StartAddress { get; set; }
        public int Length { get; set; }
        public int Offset { get; set; }
        public int SubLength { get; set; }
        public string Unit { get => UnitAction != null ? UnitAction() : unit; set => unit = value; }
        public double Factory { get => FactoryAction != null ? FactoryAction() : factory; set => factory = value; }
        public Func<double> FactoryAction { get; set; }
        public Func<string> UnitAction { get; set; }
        public string[] MatchValues { get; set; }
        public object Value
        {
            get
            {
                if (MatchValues != null && MatchValues.Length > 0 && this.DataNodeType.GetHashCode() > 0 && this.DataNodeType.GetHashCode() < 8)
                {
                    var i = Convert.ToInt32(_value);
                    if (i >= 0 && i < MatchValues.Length)
                    {
                        return MatchValues[i];
                    }
                }
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnValueChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        public event EventHandler OnValueChanged;
    }
}
