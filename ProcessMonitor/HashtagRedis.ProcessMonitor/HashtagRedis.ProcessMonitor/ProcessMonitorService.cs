using System.ServiceProcess;

namespace HashtagRedis.ProcessMonitor
{
    public partial class ProcessMonitorService : ServiceBase
    {
        private ProcessMontitorHost Host
        {
            get;
            set;
        }

        protected override void OnStart(string[] args)
        {
            this.Host = new ProcessMontitorHost();

            try
            {
                this.Host.Start();
            }
            catch
            {
                this.Host.Dispose();
            }
        }

        protected override void OnStop()
        {
            this.Host.Dispose();
            this.Host = null;
        }
    }
}
