using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

namespace CoEX_HRMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
