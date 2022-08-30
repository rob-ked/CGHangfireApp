using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGHangfireApp.Model.Page
{
    public class Page
    {
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int PerPageItems { get; set; }
        public int CurrentPage { get; set; }
    }
}
