
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.IO;
using MonoTorrent.Common;
using MonoTorrent.Client;
using System.Net;
using System.Diagnostics;
using System.Threading;
using MonoTorrent.BEncoding;
using MonoTorrent.Client.Encryption;
using MonoTorrent.Client.Tracker;
using MonoTorrent.Dht;
using MonoTorrent.Dht.Listeners;
using NetUtil;
using protocol;
using ProtoBuf;
using SampleClient;
using Newtonsoft.Json;

namespace MonoTorrent
{
    class main
    {
        static string dhtNodeFile;
        static string basePath;
        static string downloadsPath;
        static string fastResumeFile;
        static string torrentsPath;
        static ClientEngine engine;				// The engine used for downloading
        static List<TorrentManager> torrents;	// The list where all the torrentManagers will be stored that the engine gives us
        static Top10Listener listener;          // This is a subclass of TraceListener which remembers the last 20 statements sent to it

        public static string user = "苏跳跳";
        static Socket client = null;
        static TcpClient tcpClient = null;
        static NetworkStream ns;
        static BinaryReader br;
        static BinaryWriter bw;
        static byte[] initBuff;

        static void Main(string[] args)
        {
            /* Generate the paths to the folder we will save .torrent files to and where we download files to */
            basePath = Environment.CurrentDirectory;						// This is the directory we are currently in
            torrentsPath = Path.Combine(basePath, "Torrents");				// This is the directory we will save .torrents to
            downloadsPath = Path.Combine(basePath, "Downloads");			// This is the directory we will save downloads to
            fastResumeFile = Path.Combine(torrentsPath, "fastresume.data");
            dhtNodeFile = Path.Combine(basePath, "DhtNodes");
            torrents = new List<TorrentManager>();							// This is where we will store the torrentmanagers
            listener = new Top10Listener(10);

            // We need to cleanup correctly when the user closes the window by using ctrl-c
            // or an unhandled exception happens
            Console.CancelKeyPress += delegate { shutdown(); };
            AppDomain.CurrentDomain.ProcessExit += delegate { shutdown(); };
            AppDomain.CurrentDomain.UnhandledException += delegate(object sender, UnhandledExceptionEventArgs e) { Console.WriteLine(e.ExceptionObject); shutdown(); };
            Thread.GetDomain().UnhandledException += delegate(object sender, UnhandledExceptionEventArgs e) { Console.WriteLine(e.ExceptionObject); shutdown(); };

            StartEngine();
        }

        private static void StartEngine()
        {
            int port = 6888;
            Torrent torrent = null;
            // Ask the user what port they want to use for incoming connections
            Console.Write(Environment.NewLine + "Choose a listen port:{0} \n"+port);
            //while (!Int32.TryParse(Console.ReadLine(), out port)) { }



            // Create the settings which the engine will use
            // downloadsPath - this is the path where we will save all the files to
            // port - this is the port we listen for connections on
            EngineSettings engineSettings = new EngineSettings(downloadsPath, port);
            engineSettings.PreferEncryption = false;
            engineSettings.AllowedEncryption = EncryptionTypes.All;

            //engineSettings.GlobalMaxUploadSpeed = 30 * 1024;
            //engineSettings.GlobalMaxDownloadSpeed = 100 * 1024;
            //engineSettings.MaxReadRate = 1 * 1024 * 1024;


            // Create the default settings which a torrent will have.
            // 4 Upload slots - a good ratio is one slot per 5kB of upload speed
            // 50 open connections - should never really need to be changed
            // Unlimited download speed - valid range from 0 -> int.Max
            // Unlimited upload speed - valid range from 0 -> int.Max
            TorrentSettings torrentDefaults = new TorrentSettings(4, 150, 0, 0);

            // Create an instance of the engine.
            engine = new ClientEngine(engineSettings);
            engine.ChangeListenEndpoint(new IPEndPoint(IPAddress.Any, port));
            byte[] nodes = null;
            try
            {
                nodes = File.ReadAllBytes(dhtNodeFile);
            }
            catch
            {
                Console.WriteLine("No existing dht nodes could be loaded");
            }

            DhtListener dhtListner = new DhtListener (new IPEndPoint (IPAddress.Any, port));
            DhtEngine dht = new DhtEngine (dhtListner);
            engine.RegisterDht(dht);
            dhtListner.Start();
            engine.DhtEngine.Start(nodes);
            
            // If the SavePath does not exist, we want to create it.
            if (!Directory.Exists(engine.Settings.SavePath))
                Directory.CreateDirectory(engine.Settings.SavePath);

            // If the torrentsPath does not exist, we want to create it
            if (!Directory.Exists(torrentsPath))
                Directory.CreateDirectory(torrentsPath);

            BEncodedDictionary fastResume;
            try
            {
                fastResume = BEncodedValue.Decode<BEncodedDictionary>(File.ReadAllBytes(fastResumeFile));
            }
            catch
            {
                fastResume = new BEncodedDictionary();
            }

            // For each file in the torrents path that is a .torrent file, load it into the engine.
            foreach (string file in Directory.GetFiles(torrentsPath))
            {
                if (file.EndsWith(".torrent"))
                {
                    try
                    {
                        // Load the .torrent from the file into a Torrent instance
                        // You can use this to do preprocessing should you need to
                        torrent = Torrent.Load(file);
                        Console.WriteLine(torrent.InfoHash.ToString());
                    }
                    catch (Exception e)
                    {
                        Console.Write("Couldn't decode {0}: ", file);
                        Console.WriteLine(e.Message);
                        continue;
                    }
                    // When any preprocessing has been completed, you create a TorrentManager
                    // which you then register with the engine.
                    TorrentManager manager = new TorrentManager(torrent, downloadsPath, torrentDefaults);
                    if (fastResume.ContainsKey(torrent.InfoHash.ToHex ()))
                        manager.LoadFastResume(new FastResume ((BEncodedDictionary)fastResume[torrent.infoHash.ToHex ()]));
                    engine.Register(manager);

                    // Store the torrent manager in our list so we can access it later
                    torrents.Add(manager);
                    manager.PeersFound += new EventHandler<PeersAddedEventArgs>(manager_PeersFound);
                }
            }

            // If we loaded no torrents, just exist. The user can put files in the torrents directory and start
            // the client again
            if (torrents.Count == 0)
            {
                Console.WriteLine("No torrents found in the Torrents directory");
                Console.WriteLine("Exiting...");
                engine.Dispose();
                return;
            }

            // For each torrent manager we loaded and stored in our list, hook into the events
            // in the torrent manager and start the engine.
            foreach (TorrentManager manager in torrents)
            {
                // Every time a piece is hashed, this is fired.
                manager.PieceHashed += delegate(object o, PieceHashedEventArgs e) {
                    lock (listener)
                        listener.WriteLine(string.Format("Piece Hashed: {0} - {1}", e.PieceIndex, e.HashPassed ? "Pass" : "Fail"));
                };

                // Every time the state changes (Stopped -> Seeding -> Downloading -> Hashing) this is fired
                manager.TorrentStateChanged += delegate (object o, TorrentStateChangedEventArgs e) {
                    lock (listener)
                        listener.WriteLine("OldState: " + e.OldState.ToString() + " NewState: " + e.NewState.ToString());
                };

                // Every time the tracker's state changes, this is fired
                foreach (TrackerTier tier in manager.TrackerManager)
                {
                    foreach (MonoTorrent.Client.Tracker.Tracker t in tier.Trackers)
                    {
                        t.AnnounceComplete += delegate(object sender, AnnounceResponseEventArgs e) {
                            listener.WriteLine(string.Format("{0}: {1}", e.Successful, e.Tracker.ToString()));
                        };
                    }
                }
                // Start the torrentmanager. The file will then hash (if required) and begin downloading/seeding
                manager.Start();
            }

            try
            {
                Thread thread = new Thread(connectServer);
                thread.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("连接线程中断，无法连接到服务器！");
            }

            // While the torrents are still running, print out some stats to the screen.
            // Details for all the loaded torrent managers are shown.
            int i = 0;
            bool running = true;
            StringBuilder sb = new StringBuilder(1024);
            while (running)
            {
                if ((i++) % 10 == 0)
                {
                    sb.Remove(0, sb.Length);
                    running = torrents.Exists(delegate(TorrentManager m) { return m.State != TorrentState.Stopped; });

                    AppendFormat(sb, "Total Download Rate: {0:0.00}kB/sec", engine.TotalDownloadSpeed / 1024.0);
                    AppendFormat(sb, "Total Upload Rate:   {0:0.00}kB/sec", engine.TotalUploadSpeed / 1024.0);
                    AppendFormat(sb, "Disk Read Rate:      {0:0.00} kB/s", engine.DiskManager.ReadRate / 1024.0);
                    AppendFormat(sb, "Disk Write Rate:     {0:0.00} kB/s", engine.DiskManager.WriteRate / 1024.0);
                    AppendFormat(sb, "Total Read:         {0:0.00} kB", engine.DiskManager.TotalRead / 1024.0);
                    AppendFormat(sb, "Total Written:      {0:0.00} kB", engine.DiskManager.TotalWritten / 1024.0);
                    AppendFormat(sb, "Open Connections:    {0}", engine.ConnectionManager.OpenConnections);
                    
                    foreach (TorrentManager manager in torrents)
                    {
                        AppendSeperator(sb);
                        AppendFormat(sb, "State:           {0}", manager.State);
                        AppendFormat(sb, "Name:            {0}", manager.Torrent == null ? "MetaDataMode" : manager.Torrent.Name);
                        AppendFormat(sb, "Progress:           {0:0.00}", manager.Progress);
                        AppendFormat(sb, "Download Speed:     {0:0.00} kB/s", manager.Monitor.DownloadSpeed / 1024.0);
                        AppendFormat(sb, "Upload Speed:       {0:0.00} kB/s", manager.Monitor.UploadSpeed / 1024.0);
                        AppendFormat(sb, "Total Downloaded:   {0:0.00} MB", manager.Monitor.DataBytesDownloaded / (1024.0 * 1024.0));
                        AppendFormat(sb, "Total Uploaded:     {0:0.00} MB", manager.Monitor.DataBytesUploaded / (1024.0 * 1024.0));
                        MonoTorrent.Client.Tracker.Tracker tracker = manager.TrackerManager.CurrentTracker;
                        //AppendFormat(sb, "Tracker Status:     {0}", tracker == null ? "<no tracker>" : tracker.State.ToString());
                        AppendFormat(sb, "Warning Message:    {0}", tracker == null ? "<no tracker>" : tracker.WarningMessage);
                        AppendFormat(sb, "Failure Message:    {0}", tracker == null ? "<no tracker>" : tracker.FailureMessage);
                        if (manager.PieceManager != null)
                            AppendFormat(sb, "Current Requests:   {0}", manager.PieceManager.CurrentRequestCount());
                        
                        foreach (PeerId p in manager.GetPeers())
                            AppendFormat(sb, "\t{2} - {1:0.00}/{3:0.00}kB/sec - {0}", p.Peer.ConnectionUri,
                                                                                      p.Monitor.DownloadSpeed / 1024.0,
                                                                                      p.AmRequestingPiecesCount,
                                                                                      p.Monitor.UploadSpeed/ 1024.0);
                       
                        AppendFormat(sb, "", null);
                        if (manager.Torrent != null)
                            foreach (TorrentFile file in manager.Torrent.Files)
                                AppendFormat(sb, "{1:0.00}% - {0}", file.Path, file.BitField.PercentComplete);
                    }
                    //Console.Clear();
                    Console.WriteLine(sb.ToString());
                    listener.ExportTo(Console.Out);
                }

                System.Threading.Thread.Sleep(500);
            }
        }

        static void manager_PeersFound(object sender, PeersAddedEventArgs e)
        {
            lock (listener)
                listener.WriteLine(string.Format("Found {0} new peers and {1} existing peers", e.NewPeers, e.ExistingPeers ));//throw new Exception("The method or operation is not implemented.");
        }

        private static void AppendSeperator(StringBuilder sb)
        {
            AppendFormat(sb, "", null);
            AppendFormat(sb, "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - -", null);
            AppendFormat(sb, "", null);
        }
		private static void AppendFormat(StringBuilder sb, string str, params object[] formatting)
		{
            if (formatting != null)
                sb.AppendFormat(str, formatting);
            else
                sb.Append(str);
			sb.AppendLine();
		}

		private static void shutdown()
		{
            BEncodedDictionary fastResume = new BEncodedDictionary();
            for (int i = 0; i < torrents.Count; i++)
            {
                torrents[i].Stop(); ;
                while (torrents[i].State != TorrentState.Stopped)
                {
                    Console.WriteLine("{0} is {1}", torrents[i].Torrent.Name, torrents[i].State);
                    Thread.Sleep(250);
                }

                fastResume.Add(torrents[i].Torrent.InfoHash.ToHex (), torrents[i].SaveFastResume().Encode());
            }

#if !DISABLE_DHT
            File.WriteAllBytes(dhtNodeFile, engine.DhtEngine.SaveNodes());
#endif
            File.WriteAllBytes(fastResumeFile, fastResume.Encode());
            engine.Dispose();

			foreach (TraceListener lst in Debug.Listeners)
			{
				lst.Flush();
				lst.Close();
			}

            System.Threading.Thread.Sleep(2000);
		}
        /// <summary>
        /// 连接中转服务器
        /// </summary>
        private static void connectServer()
        {

            byte[] buf = new byte[1024];
            string input;
            IPAddress local = IPAddress.Parse("127.0.0.1");
            IPEndPoint iep = new IPEndPoint(local, 13333);
            try
            {
                tcpClient = new TcpClient();
                tcpClient.Connect("127.0.0.1", 13333);
                Console.WriteLine("开始连接服务器");
                ns = tcpClient.GetStream();
            }
            catch (SocketException)
            {
                Console.WriteLine("无法连接到服务器！");
                return;
            }
            bw = new BinaryWriter(ns);
            br = new BinaryReader(ns);
            //启动心跳发送线程
            Thread sendEcho = new Thread(new ThreadStart(socketSend));
            sendEcho.Start();
            sendEcho.IsBackground = true;
            //启动消息接收线程
            Thread receiveThread = new Thread(new ThreadStart(socketReceive));
            receiveThread.Start();
            receiveThread.IsBackground = true;
            //构造初始化请求报文
            P2PInitReq initReq = new P2PInitReq
            {
                msgFrom = P2PInitReq.MsgFrom.PC,
                userName = user,
            };
            MemoryStream ms = new MemoryStream();
            Serializer.Serialize(ms, initReq);
            byte[] data = ms.ToArray();
            initBuff = NetTcpBase.PackMessage((int)ENetworkMessage.P2P_INIT_REQ, LittleEndian.GetLittleEndianBytes(1), data);
            Thread.Sleep(5000);
            init(bw, br, initBuff);

        }
        /// <summary>
        /// 消息接收线程
        /// </summary>
        private static void socketReceive()
        {

            while (true)
            {
                byte[] data1 = NetTcpBase.ReadFromServer(br);
                int size = LittleEndian.LittleEndianToInt32(data1, 0);
                int type = LittleEndian.LittleEndianToInt32(data1, 4);
                byte[] data2 = new byte[size - 12];
                Array.Copy(data1, 12, data2, 0, size - 12);
                MemoryStream ms1 = new MemoryStream(data2);
                switch (type)
                {
                    case (int)ENetworkMessage.P2P_CONTROL_REQ:
                        P2PControlReq controlReq = Serializer.Deserialize<P2PControlReq>(ms1);
                        switch (controlReq.command)
                        {
                            case P2PControlReq.Command.START:
                                foreach (TorrentManager manager in torrents)
                                {
                                    if (manager.Torrent.Name == controlReq.filename)
                                    {
                                        manager.Start();
                                    }
                                }
                                break;
                            case P2PControlReq.Command.STARTALL:
                                engine.StartAll();
                                break;
                            case P2PControlReq.Command.STOP:
                                foreach (TorrentManager manager in torrents)
                                {
                                    if (manager.Torrent.Name == controlReq.filename)
                                    {
                                        manager.Stop();
                                    }
                                }
                                break;
                            case P2PControlReq.Command.STOPALL:
                                engine.PauseAll();
                                break;
                            default:
                                break;
                        }
                        break;
                    case (int)ENetworkMessage.P2P_INIT_RSP:
                        P2PInitRsp result = Serializer.Deserialize<P2PInitRsp>(ms1);
                        switch (result.status)
                        {
                            case P2PInitRsp.Status.SUCCESS:
                                Console.WriteLine("初始化连接服务器成功！");
                                break;
                            case P2PInitRsp.Status.FAIL:
                                init(bw, br, initBuff);
                                Thread.Sleep(10000);
                                break;
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// 心跳包发送线程
        /// </summary>
        public static void socketSend()
        {
            int i = 0;
            while (true)
            {
                Thread.Sleep(2000);
                try
                {
                    Console.WriteLine("发送心跳包");
                    List<DownloadFile> files = new List<DownloadFile>();
                    foreach (TorrentManager manager in torrents)
                    {
                        DownloadFile downloadFile = new DownloadFile(manager.State.ToString(), manager.Torrent == null ? "MetaDataMode" : manager.Torrent.Name, manager.Monitor.DownloadSpeed / 1024.0, manager.Monitor.UploadSpeed / 1024.0, manager.Progress);
                        files.Add(downloadFile);
                    }
                    PCKeepAlivePacket loginReq = new PCKeepAlivePacket
                    {
                        keepAliveType = PCKeepAlivePacket.KeepAliveType.SUCCESS,
                        data = JsonConvert.SerializeObject(files),
                    };
                    MemoryStream ms = new MemoryStream();
                    Serializer.Serialize(ms, loginReq);
                    byte[] data = ms.ToArray();
                    byte[] Buff = NetTcpBase.PackMessage((int)ENetworkMessage.PC_KEEP_ALIVE, LittleEndian.GetLittleEndianBytes(1), data);
                    bw.Write(Buff, 0, Buff.Length);
                    bw.Flush();
                }
                catch (SocketException)
                {
                    Thread.CurrentThread.Abort();
                    //throw ex;
                }
            }
        }

        /// <summary>
        /// 初始化消息请求
        /// </summary>
        /// <param name="bw"></param>
        /// <param name="br"></param>
        /// <param name="request"></param>
        private static void init(BinaryWriter bw, BinaryReader br, byte[] request)
        {
            bw.Write(request, 0, request.Length);
            bw.Flush();
        }

        static void doAnounce(MonoTorrent.Client.Tracker.Tracker t)
        {
            t.AnnounceComplete += delegate (object sender, AnnounceResponseEventArgs e) {
                listener.WriteLine(string.Format("{0}: {1}", e.Successful, e.Tracker.ToString()));
            };
        }

    }
}
