using System.Collections.Generic;

namespace RevitTestModule.Model
{
    public class Grouped
    {
        public string CategoryName { get; set; }
        public Dictionary<string, double> ParameterSums { get; set; } = new Dictionary<string, double>();
        public int ElementCount { get; set; }
    }

}
