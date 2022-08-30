using CGHangfireApp.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGHangfireApp.Model.Page
{
    public class Comments : Page
    {
        public List<Comment> Data { get; set; }
    }
}
