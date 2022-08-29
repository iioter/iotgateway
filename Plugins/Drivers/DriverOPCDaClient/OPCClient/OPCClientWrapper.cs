using System;
using System.Collections.Generic;
using OPCAutomation;

namespace Automation.OPCClient
{
    public class OPCClientWrapper
    {

        private OPCAutomation.OPCServer opcServer;
        private OPCGroups opcGroups;
        private OPCGroup opcAGroup;
        private OPCBrowser opcBrowser;

        private bool isConnect;
        private bool isDefaultGroupActive;
        private int defaultGroupUpdateRate;
        private int defaultGroupDeadband;
        private string opcServerName;
        private string serverIP;

        private Dictionary<string, TagItem> clientHandleDict = new Dictionary<string, TagItem>();

        private List<string> clientNameList = new List<string>();
        private object lockObj = new object();
        public event OPCDataChangedHandler OpcDataChangedEvent;
        /// <summary>
        /// OPCServer object
        /// </summary>
        public OPCAutomation.OPCServer OPCServer
        {
            get { return opcServer; }
            set { opcServer = value; }
        }
        /// <summary>
        /// OPCGroups object
        /// </summary>
        public OPCGroups OPCGroups
        {
            get { return opcGroups; }
            set { opcGroups = value; }
        }

        public OPCGroup OPCGroup
        {
            get { return opcAGroup; }
            set { opcAGroup = value; }
        }
        /// <summary>
        /// ServerIP
        /// </summary>
        public string ServerIP
        {
            get { return serverIP; }
            set { serverIP = value; }
        }
        /// <summary>
        /// OPCServerName
        /// </summary>
        public string OPCServerName
        {
            get { return opcServerName; }
            set { opcServerName = value; }
        }

        /// <summary>
        /// OPCGroup object 默认活动状态
        /// </summary>
        public bool IsDefaultGroupActive
        {
            get { return isDefaultGroupActive; }
            set { isDefaultGroupActive = value; }
        }
        /// <summary>
        /// OPCGroup object 默认更新频率
        /// </summary>
        public int DefaultGroupUpdateRate
        {
            get { return defaultGroupUpdateRate; }
            set { defaultGroupUpdateRate = value; }
        }

        public int DefaultGroupDeadband
        {
            get { return defaultGroupDeadband; }
            set { defaultGroupDeadband = value; }

        }

        public OPCClientWrapper()
        {
            opcServer = new OPCAutomation.OPCServer();
        }

        /// <summary>
        /// 连接远程机上的OPCServer服务器
        /// </summary>
        /// <param name="serverIP"></param>
        /// <param name="OPCServerName"></param>
        /// <returns></returns>
        private bool ConnectRemote()
        {
            try
            {
                this.opcServer.Connect( opcServerName, serverIP );
                this.opcServer.ServerShutDown += new DIOPCServerEvent_ServerShutDownEventHandler(opcServer_ServerShutDown);
            }
            catch (Exception ex)
            {
                return false;
            }
            isConnect = true;
            return true;
        }

        void opcServer_ServerShutDown(string Reason)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void Init(string serverIP, string opcServerName )
        {
            this.opcServerName = opcServerName;
            this.serverIP = serverIP;

            try
            {
                if (ConnectRemote())
                {
                    opcGroups = opcServer.OPCGroups;
                    opcBrowser = opcServer.CreateBrowser();
                    opcAGroup = opcGroups.Add("Group");
                    opcAGroup.IsActive = true;
                    opcAGroup.IsSubscribed = true;
                    //opcAGroup.DataChange += new DIOPCGroupEvent_DataChangeEventHandler( OnGroup_DataChange );
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 当监视的标签发生改变时激活
        /// </summary>  
        private void OnGroup_DataChange( int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps )
        {
            lock (lockObj)
            {
                List<OPCChangeModel> list = new List<OPCChangeModel>();
                for (int i = 1; i <= NumItems; i++)
                {
                    list.Add(new OPCChangeModel() 
                    {
                        Name = this.clientNameList[(int)ClientHandles.GetValue(i)],
                        Value = ItemValues.GetValue(i),
                        Quality = (TagQuality)Qualities.GetValue(i),
                        TimeStamp = (DateTime)TimeStamps.GetValue(i),
                    });
                }
                if (this.OpcDataChangedEvent != null)
                {
                    this.OpcDataChangedEvent(list);
                }
            }
        }

        /// <summary>
        /// 释放连接
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if (isConnect == true)
                {
                    clientHandleDict.Clear();
                    opcServer.Disconnect();
                    isConnect = false;
                }
            }
            catch (Exception)
            {
                //SCLog.Instance.LogError( "断开OPCServer连接时错误:" + err.Message );
            }
        }

        /// <summary>
        /// 显示节点树 不包含叶子节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public TagTreeNode GetTree(TagTreeNode node)
        {
            TagTreeNode subNode = null;
            opcBrowser.ShowBranches();
            
            foreach (var branch in opcBrowser)
            {
                if (node == null)
                {
                    node = new TagTreeNode(opcServer.ServerName);
                }
                //Console.WriteLine(branch);
                subNode = node.AddNode(branch.ToString());
                try
                {
                    opcBrowser.MoveDown(branch.ToString());
                    GetTree(subNode);
                    opcBrowser.MoveUp();
                }
                catch { }
            }
            return node;
        }

        public List<String> GetLeaf(string path)
        {
            List<string> list = new List<string>();
            Array branches = path.Substring(path.IndexOf("/") + 1, path.Length - path.IndexOf("/") - 1).Split('/');
            opcBrowser.MoveTo(branches);
            opcBrowser.ShowLeafs(false);
            foreach (var branch in opcBrowser)
            {
                list.Add(branch.ToString());
            }
            opcBrowser.MoveToRoot();
            return list;
        }

        public List<string> GetRootNodes()
        {
            opcBrowser.MoveToRoot();
            List<string> list = new List<string>();
            opcBrowser.ShowBranches();
            foreach (var branch in opcBrowser)
            {
                list.Add(branch.ToString());
            }

            return list;
        }

        public List<string> GetChildNodes(string path)
        {
            List<string> list = new List<string>();
            Array branches = path.Substring(path.IndexOf("/") + 1, path.Length - path.IndexOf("/") - 1).Split('/');
            opcBrowser.MoveTo(branches);
            opcBrowser.ShowBranches();
            foreach (var branch in opcBrowser)
            {
                list.Add(branch.ToString());
            }
            opcBrowser.MoveToRoot();
            return list;
        }

        public bool IsOPCServerConnected()
        {
            try
            {
                if (opcServer.ServerState == (int)OPCServerState.OPCRunning)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 监视标签
        /// </summary>
        /// <param name="itemName"></param>
        public void MonitorOPCItem(string itemName)
        {
            try
            {
                if (!this.clientHandleDict.ContainsKey(itemName))
                {
                    //var bro = this.opcServer.CreateBrowser();
                    //bro.ShowBranches();
                    //foreach (var branch in bro)
                    //{
                    //    Console.WriteLine(branch.ToString());
                    //}
                    
                    var index = this.clientNameList.Count;
                    OPCAutomation.OPCItem tempItem = opcAGroup.OPCItems.AddItem(itemName, index);
                    TagItem item = new TagItem()
                    {
                        Name = itemName,
                        ClientHandler = index,
                        ServerHandler = tempItem.ServerHandle,
                    };
                    this.clientNameList.Add(itemName);
                    this.clientHandleDict.Add(itemName, item);
                    object value, timeStamp, quality;
                    tempItem.Read(1, out  value, out quality, out timeStamp);
                }
            }
            catch
            {
                
            }
        }

        public OPCChangeModel ReadValue(string name)
        {
            if (this.clientHandleDict.ContainsKey(name))
            {
                OPCAutomation.OPCItem item = opcAGroup.OPCItems.GetOPCItem(this.clientHandleDict[name].ServerHandler);
                object value,quality,timestamp;
                item.Read(2, out value, out quality, out timestamp);
                
                return new OPCChangeModel()
                {
                     Name = name,
                     Quality = (TagQuality)((short)quality),
                     Value = item.Value,
                     TimeStamp = (DateTime)timestamp,
                };  
            }
            return null;
        }

        public string ReadNodeLabel(string name)
        {
            MonitorOPCItem(name);

            if (clientHandleDict.ContainsKey(name))
            {
                OPCItem item = opcAGroup.OPCItems.GetOPCItem(clientHandleDict[name].ServerHandler);
                if (item.Value != null)
                {
                    return item.Value.ToString();
                }
                else
                {
                    object value, quality, timestamp;
                    item.Read(1, out value, out quality, out timestamp);
                    return value.ToString();
                }
            }
            return string.Empty;
        }

        public void Write(string name, object value)
        {
            if (this.clientHandleDict.ContainsKey(name))
            {
                OPCAutomation.OPCItem item = opcAGroup.OPCItems.GetOPCItem(this.clientHandleDict[name].ServerHandler);
                item.Write(value);
            }
        }
    }
}
