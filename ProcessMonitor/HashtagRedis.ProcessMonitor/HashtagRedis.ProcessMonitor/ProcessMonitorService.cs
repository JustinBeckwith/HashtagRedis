using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition.Hosting;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace HashtagRedis.ProcessMonitor
{
    public partial class ProcessMonitorService : ServiceBase
    {
        public ProcessMonitorService()
        {
            this.InitializeComponent();
        }

        private List<IService> Services
        {
            get;
            set;
        }

        protected override void OnStart(string[] args)
        {
            var catalog = new DirectoryCatalog(typeof(ProcessMonitorService).Assembly.Location);
            var container = new CompositionContainer(catalog);

            var services = container.GetExportedValues<IService>();
            List<IService> initialized = new List<IService>();

            try
            {
                foreach (var service in services)
                {
                    service.Initialize();
                    initialized.Add(service);
                }

                this.Services = initialized;
            }
            catch
            {
                foreach (var service in initialized)
                {
                    service.Dispose();
                }

                throw;
            }
        }

        protected override void OnStop()
        {
            foreach (var service in this.Services)
            {
                service.Dispose();
            }

            this.Services = null;
        }
    }
}
