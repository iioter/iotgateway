using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automation.OPCClient
{
    class TagItem
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private int serverHandler;

        public int ServerHandler
        {
            get { return serverHandler; }
            set { serverHandler = value; }
        }
        private int clientHandler;

        public int ClientHandler
        {
            get { return clientHandler; }
            set { clientHandler = value; }
        }
    }
}
