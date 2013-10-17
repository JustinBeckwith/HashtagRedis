using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashtagRedis.ProcessMonitor
{
    [Export(typeof(IService))]
    internal class ProcessMonitorService : Service
    {
        public ProcessMonitorService()
        {
            this.Subscriptions = new Dictionary<int, ProcessSubscription>();
        }

        [ImportMany(typeof(IProcessTypeDescriptor))]
        private IEnumerable<IProcessTypeDescriptor> ProcessTypes
        {
            get;
            set;
        }

        private IDictionary<int, ProcessSubscription> Subscriptions
        {
            get;
            set;
        }

        public override void Initialize()
        {
            var processes = Process.GetProcesses();

            foreach (var process in processes)
            {
                foreach (var processType in ProcessTypes)
                {
                    if (processType.IsMatch(process))
                    {

                    }
                }
            }
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
