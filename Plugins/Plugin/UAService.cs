using Microsoft.Extensions.Hosting;
using Opc.Ua;
using Quickstarts;
using Quickstarts.ReferenceServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Plugin
{
    public class UAService :IDisposable
    {
        string applicationName = "ConsoleReferenceServer";
        string configSectionName = "Quickstarts.ReferenceServer";

        public UAServer<ReferenceServer> server = null;

        public UAService()
        {
            server = new UAServer<ReferenceServer>(null)
            {
                AutoAccept = false,
                Password = null
            };

            server.LoadAsync(applicationName, configSectionName).ConfigureAwait(false);
            server.CheckCertificateAsync(false).ConfigureAwait(false);
            server.StartAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            server.StopAsync().ConfigureAwait(false);
        }
    }
}
