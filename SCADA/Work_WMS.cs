using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace SCADA
{
    class Work_WMS
    {
        private static readonly Lazy<Work_WMS> lazy = new Lazy<Work_WMS>(() => new Work_WMS());

        public static Work_WMS Instance { get { return lazy.Value; } }

        private Work_WMS()
        {
            RunScadaService();
        }


        /// <summary>
        /// 运行Scada服务（供WMS系统调用）
        /// </summary>
        private void RunScadaService()
        {
            IScadaService service = new ScadaService();
            WebServiceHost host = new WebServiceHost(service, new Uri(@"http://localhost:41150/ScadaService"));
            host.Open();
        }
    }
}
