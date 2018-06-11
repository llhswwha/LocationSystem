using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace LocationWCFServices
{
    public static class ServiceHostHelper
    {
        public static void SetProxyDataContractResolver(this ServiceHost host)
        {
            SetDataContractResolver<ProxyDataContractResolver>(host);
        }

        public static void SetDataContractResolver<T>(this ServiceHost host) where T : DataContractResolver, new()
        {
            foreach (ServiceEndpoint endpoint in host.Description.Endpoints)
            {
                foreach (OperationDescription operation in endpoint.Contract.Operations)
                {
                    DataContractSerializerOperationBehavior behavior =
                        operation.Behaviors.Find<DataContractSerializerOperationBehavior>();
                    if (behavior.DataContractResolver == null)
                    {
                        behavior.DataContractResolver = new T();
                    }
                }
            }
        }
    }
}
