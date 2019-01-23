using Location.IModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace IModel
{
    public interface IDevMonitorNode : IId
    {
        string KKS { get; set; }
        string TagName { get; set; }
         string DataBaseName { get; set; }

         string DataBaseTagName { get; set; }

         string Describe { get; set; }

         string Unit { get; set; }

         string DataType { get; set; }

         string TagType { get; set; }
    }
}
