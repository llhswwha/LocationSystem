using Location.IModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace IModel
{
    public interface IDevMonitorNode : IId
    {
        string TagName { get; set; }

        string DbTagName { get; set; }
        
        string Describe { get; set; }
        
        string Value { get; set; }

        string Unit { get; set; }

        string DataType { get; set; }

        string KKS { get; set; }

        string ParentKKS { get; set; }

        int? ParseResult { get; set; }

        long Time { get; set; }
    }
}
