using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGHangfireApp.Model.Settings
{
    public class Api
    {
        public string BaseURL
        {
            get; set;
        }

        public List<string> Endpoints
        {
            get;set;
        }
    }
}
