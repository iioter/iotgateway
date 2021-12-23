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
    public class UAService : IHostedService
    {
        string applicationName = "ConsoleReferenceServer";
        string configSectionName = "Quickstarts.ReferenceServer";

        UAServer<ReferenceServer> server = null;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            server = new UAServer<ReferenceServer>(null)
            {
                AutoAccept = false,
                Password = null
            };

            server.LoadAsync(applicationName, configSectionName).ConfigureAwait(false);
            server.CheckCertificateAsync(false).ConfigureAwait(false);
            server.StartAsync().ConfigureAwait(false);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            server.StopAsync().ConfigureAwait(false);
            return Task.CompletedTask;
        }
    }
}
