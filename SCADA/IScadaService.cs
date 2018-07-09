using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Routing;

namespace SCADA
{
    [ServiceContract(Name = "ScadaService")]
    interface IScadaService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/", BodyStyle = WebMessageBodyStyle.Bare)]
        string TestService();
    }
}
