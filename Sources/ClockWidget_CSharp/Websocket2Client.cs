using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;
using Newtonsoft.Json.Linq;

namespace Websocket2CreatorCentralPlugin
{
    class Websocket2Client
    {
        private WebSocket webSocketClient = null;
        private Boolean isRecon = false;
        private string url = null;
        private string ID = null;
        private System.Timers.Timer t = new System.Timers.Timer(2000); //
        private bool g_IsAlive = false;
        public bool IsAlive
        {
            get { return g_IsAlive; }
            private set { g_IsAlive = value; }
        }

        #region Event

        public delegate void DelegateRecieveMessage(string message);
        public event DelegateRecieveMessage RecieveMessage;

        public delegate void DelegateSocketError(string message);
        public event DelegateSocketError SocketError;

        public delegate void DelegateSocketDisconnect(string message);
        public event DelegateSocketDisconnect SocketClose;

        #endregion

        public Websocket2Client(string url, string id)
        {
            this.url = url;
            ID = id;
        }

        ~Websocket2Client()
        {
            DisConnect();
            if (webSocketClient != null)
            {
                webSocketClient = null;
            }
        }

        public void Connect()
        {
            bool result = false;

            webSocketClient = new WebSocket(url);
            webSocketClient.OnError += new EventHandler<WebSocketSharp.ErrorEventArgs>(ConnectError);
            webSocketClient.OnOpen += new EventHandler(onConnected);
            webSocketClient.OnClose += new EventHandler<WebSocketSharp.CloseEventArgs>(Close);
            webSocketClient.OnMessage += new EventHandler<MessageEventArgs>(onTextMessageReceived);
            webSocketClient.ConnectAsync();
        }

        public bool SendMessage(string message)
        {
            bool result = false;

            webSocketClient.Send(message);

            return result;
        }

        public bool DisConnect()
        {
            bool result = false;

            if (webSocketClient.IsAlive)
            {
                webSocketClient.Close();
                
            }
            g_IsAlive = webSocketClient.IsAlive;
            return result;
        }

        private void onConnected(object sender, EventArgs e)
        {
            JObject jsonAppTriggered = new JObject();
            jsonAppTriggered.Add(new JProperty("method", "ax.register.widget"));
            JObject jsonParameters = new JObject();
            jsonParameters.Add(new JProperty("id", ID));
            jsonAppTriggered.Add("params", jsonParameters);
            SendMessage(jsonAppTriggered.ToString());
            g_IsAlive = webSocketClient.IsAlive;
        }

        private void ConnectError(object sender, WebSocketSharp.ErrorEventArgs e)
        {
            SocketError(e.Message);
        }

        private void Close(object sender, WebSocketSharp.CloseEventArgs e)
        {
            if (!webSocketClient.IsAlive)
            {
                isRecon = true;
                webSocketClient.ConnectAsync();
            }
            g_IsAlive = webSocketClient.IsAlive;
            SocketClose(e.Reason);
        }

        private void onTextMessageReceived(object sender, MessageEventArgs e)
        {
            //當收到server回傳的訊息
            RecieveMessage(e.Data);
        }
    }
}
