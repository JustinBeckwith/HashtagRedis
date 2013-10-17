using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;

namespace HashtagRedis.ProcessMonitor
{
    public class ProcessMontitorHost : IDisposable
    {
        public ProcessMontitorHost()
        {
        }

        private List<IService> Services
        {
            get;
            set;
        }

        public void Start()
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

        public void Dispose()
        {
            foreach (var service in this.Services)
            {
                service.Dispose();
            }

            this.Services = null;
        }

        private class ProcessSubscription
        {
            public static ProcessSubscription Create(Process process)
            {
                return new ProcessSubscription(process);
            }

            public ProcessSubscription(Process process)
            {
                this.Process = process;
            }

            private Process Process
            {
                get;
                set;
            }
        }
    }
}
