using Quickstarts;
using Quickstarts.ReferenceServer;

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
