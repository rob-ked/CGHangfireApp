using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGHangfireApp.Job
{
    internal interface IJob
    {
        string GetName();

        string GetDescription();

        void SetSchedule(string schedule);

        string GetSchedule();

        string Run(PerformContext context);
    }
}
