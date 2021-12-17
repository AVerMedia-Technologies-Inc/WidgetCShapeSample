using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Websocket2CreatorCentralPlugin
{
    public partial class PluginForm : Form
    {
        public string g_URL = "";
        private Websocket2Client g_WebClient = null;
        private string g_ReceiveOrSendMsg = "";
        string g_UUID = "";
        string g_Port = "";
        TimeZoneInfo g_TimeZone;
        bool g_EnableClock = false;
        bool g_IsDigitalClock = true;
        string g_CurrentCity = "";
        string g_CurrentClockType = "";

        delegate void callbyUI();

        public PluginForm(string uuid, string port)
        {
            InitializeComponent();
            g_UUID = uuid;
            g_URL = "ws://localhost:" + port;
            g_Port = port;
        }

        ~PluginForm()
        {
            if (g_WebClient != null)
            {
                g_WebClient = null;
            }
        }

        private void PluginForm_Load(object sender, EventArgs e)
        {
            this.Text = "PluginC#.Exe_UUID:[ " + g_UUID + " ]_PORT:" + g_Port;
            listInfo.Items.Clear();
            txtURL.Text = g_URL;
            g_WebClient = new Websocket2Client(g_URL, g_UUID);
            g_WebClient.RecieveMessage += new Websocket2Client.DelegateRecieveMessage(ReceiveMessageByWebsocket);
            g_WebClient.SocketError += new Websocket2Client.DelegateSocketError(SocketError);
            g_WebClient.SocketClose += new Websocket2Client.DelegateSocketDisconnect(SocketClose);

            btnConnect_Click(sender, e);
            g_TimeZone = TimeZoneInfo.Local;
            var ALL = TimeZoneInfo.GetSystemTimeZones();
        }

        private void ReceiveMessageByWebsocket(string msg)
        {
            g_ReceiveOrSendMsg = msg;
            
            MethodInvoker mi = new MethodInvoker(this.ChangeUI);
            this.BeginInvoke(mi, null);

            try
            {
                JObject json = JObject.Parse(g_ReceiveOrSendMsg);
                if (json.ContainsKey("result"))
                {
                    string registerResult = json.SelectToken("result").ToString();
                    if (registerResult == "ax.register.widget")
                    {
                        GetPayload();
                        return;
                    }
                }

                string method = json.SelectToken("method").ToString();
                
                switch (method)
                {
                    case "ax.send.to.widget":
                        Send2Plugin(json);
                        SetPayload();
                        break;
                    case "ax.widget.key.down":
                        break;
                    case "ax.widget.key.up":
                        break;
                    case "ax.widget.triggered":
                        break;
                    case "ax.update.payload":
                        ShowPayload(json);
                        SetPayload();
                        SetPropertyUI();
                        break;
                    case "ax.property.connected":
                        SetPropertyUI();
                        break;
                    default:
                        break;
                }                

            }
            catch (Exception e)
            {
                //MessageBox.Show("Reason: " + e.Message + ";\nSource: " + e.StackTrace);
            }                       

        }

        private void Send2Plugin(JObject json)
        {
            try
            {
                string uuid = json.SelectToken("params").SelectToken("id").ToString();
                string action = json.SelectToken("params").SelectToken("payload").SelectToken("action").ToString();

                string runType = "";
                switch (action)
                {
                    case "set_city_val":
                        g_EnableClock = true;
                        runType = json.SelectToken("params").SelectToken("payload").SelectToken("city").ToString();
                        g_CurrentCity = runType;
                        break;
                    case "set_type_val":
                        g_EnableClock = true;
                        runType = json.SelectToken("params").SelectToken("payload").SelectToken("type").ToString();
                        g_CurrentClockType = runType;
                        if (runType == "analog")
                        {
                            g_IsDigitalClock = false;
                        }
                        else
                        {
                            g_IsDigitalClock = true;
                        }
                        break;
                    default:
                        break;
                }


                RunCommandByMessage(uuid, runType);
            }
            catch (Exception)
            {
            }
        }

        private void ShowPayload(JObject json)
        {
            try
            {
                g_EnableClock = true;
                string uuid = json.SelectToken("params").SelectToken("id").ToString();
                string city = json.SelectToken("params").SelectToken("payload").SelectToken("city").ToString();
                RunCommandByMessage(uuid, city);
                g_CurrentCity = city;

                string clockType = json.SelectToken("params").SelectToken("payload").SelectToken("type").ToString();
                if (clockType == "analog")
                {
                    g_IsDigitalClock = false;
                }
                else
                {
                    g_IsDigitalClock = true;
                }
                g_CurrentClockType = clockType;
            }
            catch (Exception)
            {

                //throw;
            }
        }

        private void RunCommandByMessage(string uuid, string runType)
        {
            switch (runType)
            {
                case "taipei":
                    g_TimeZone = TimeZoneInfo.GetSystemTimeZones().Where(x => x.Id.Contains("Taipei"))?.First();

                    break;
                case "new_york":
                    g_TimeZone = TimeZoneInfo.GetSystemTimeZones().Where(x => x.Id.Contains("Eastern Standard Time"))?.ElementAt(2);

                    break;
                case "california":
                    g_TimeZone = TimeZoneInfo.GetSystemTimeZones().Where(x => x.Id.Contains("Pacific Standard Time"))?.First();

                    break;
                case "australia":
                    g_TimeZone = TimeZoneInfo.GetSystemTimeZones().Where(x => x.Id.Contains("AUS Eastern Standard Time"))?.First();

                    break;
                default:
                    break;
            }
        }

        private void btnSendMsg_Click(object sender, EventArgs e)
        {
            string msg = txtSendMsg.Text;
            g_WebClient.SendMessage(msg);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            g_WebClient.Connect();
            btnConnect.Enabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            g_WebClient.DisConnect();
            btnConnect.Enabled = true;
            txtSendMsg.Enabled = true;
        }

        private void PluginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (g_WebClient != null)
            {
                g_WebClient.DisConnect();
                g_WebClient = null;
            }
        }

        private Image DrawingWorldClock(DateTime time)
        {
            Image clockImage = null;
            try
            {
                // Create image.
                clockImage = Properties.Resources.ClockJPG;
                pictureBox1.Image = clockImage;
                int imageWidth = clockImage.Width;
                int imageHeight = clockImage.Height;
                // Create graphics object for alteration.
                Graphics newGraphics = Graphics.FromImage(clockImage);

                #region Alter image.
                float radius;
                if (imageWidth > imageHeight)
                {
                    radius = (float)(imageHeight - 8) / 2;
                }
                else
                {
                    radius = (float)(imageWidth - 8) / 2;
                }

                Pen pen = new Pen(Color.Black, 2);
                //Get current time
                DateTime currentTime = DateTime.Now;
                DateTime targetTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(currentTime, TimeZoneInfo.Local.Id, g_TimeZone.Id);
                int second = targetTime.Second;
                int minute = targetTime.Minute;
                int hour = targetTime.Hour;
                string curTimeFormat = targetTime.ToString("HH:mm:ss");
                txtTime.Text = curTimeFormat;

                //Set the coordinate origin
                newGraphics.TranslateTransform((float)(imageWidth / 2.0), (float)(imageHeight / 2.0));

                //Draw the second hand
                pen.Color = Color.Red;
                newGraphics.RotateTransform(6 * second);
                newGraphics.DrawLine(pen, 0, 0, 0, (-1) * (float)(radius / 1.5));

                //Draw minute hand
                pen.Color = Color.Blue;
                newGraphics.RotateTransform(-6 * second);
                newGraphics.RotateTransform((float)(0.1 * second + 6 * minute));
                newGraphics.DrawLine(pen, 0, 0, 0, (-1) * (float)(radius / 2));

                //Draw hour hand
                pen.Color = Color.Green;
                pen.Width = 10;
                newGraphics.RotateTransform((float)(0.1 * second + 6 * minute) * (-1));
                newGraphics.RotateTransform((float)(30.0 / 3600.0 * second + 30.0 / 60.0 * minute + hour * 30.0));
                newGraphics.DrawLine(pen, 0, 0, 0, (-1) * (float)(radius / 2.5));

                #endregion

                SendWorldClockImageMessage(clockImage);

                newGraphics.Dispose();
            }
            catch (Exception)
            {
            }
            return clockImage;
        }

        private string Image2Base64(Image clockImage)
        {
            string base64Image = "";

            using (MemoryStream m = new MemoryStream())
            {
                clockImage.Save(m, clockImage.RawFormat);
                byte[] imageBytes = m.ToArray();

                // Convert byte[] to Base64 String
                base64Image = Convert.ToBase64String(imageBytes);
            }

            return base64Image;
        }

        private void SendWorldClockImageMessage(Image clockImage)
        {
            if (g_WebClient.IsAlive && g_EnableClock)
            {
                string imageMsg = Image2Base64(clockImage);

                JObject jsonWorldClock = new JObject();
                jsonWorldClock.Add(new JProperty("id", 0));
                jsonWorldClock.Add(new JProperty("jsonrpc", "2.0"));
                jsonWorldClock.Add(new JProperty("method", "ax.set.image"));
                JObject jsonParameters = new JObject();
                jsonParameters.Add(new JProperty("id", g_UUID));

                JObject jsonPayload = new JObject();
                jsonPayload.Add(new JProperty("image", imageMsg));
                jsonParameters.Add(new JProperty("payload", jsonPayload));
                jsonWorldClock.Add("params", jsonParameters);
                g_ReceiveOrSendMsg = "[Send] " + jsonWorldClock.ToString();
                g_WebClient.SendMessage(jsonWorldClock.ToString());

                MethodInvoker mi = new MethodInvoker(this.ChangeUI);
                this.BeginInvoke(mi, null);
            }
        }

        private void ChangeUI()
        {
            if (this.InvokeRequired)
            {
                callbyUI cb = new callbyUI(ChangeUI);

                this.Invoke(cb);
            }
            else
            {
                if (listInfo.Items.Count > 1000000)
                {
                    listInfo.Items.Clear();
                }
                if (g_ReceiveOrSendMsg.Contains("[Send]"))
                {
                    txtSendMsg.Text = g_ReceiveOrSendMsg.Replace("[Send]", "").Trim();
                }
                else
                {
                    listInfo.Items.Add(g_ReceiveOrSendMsg);
                }
            }
        }

        private void timerDrawClock_Tick(object sender, EventArgs e)
        {
            if (g_IsDigitalClock)
            {
                DigitalClockImage();
            }
            else
            {
                DrawingWorldClock(DateTime.Now);
            }
        }

        private void SocketClose(string reason)
        {
            listInfo.Items.Add("[Socket Error] " + reason);
        }

        private void SocketError(string message)
        {
            listInfo.Items.Add("[Socket Error] " + message);
        }

        private void PluginForm_Shown(object sender, EventArgs e)
        {
            Hide();
        }

        private void DigitalClockImage()
        {
            //Get Current time
            DateTime currentTime = DateTime.Now;
            DateTime targetTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(currentTime, TimeZoneInfo.Local.Id, g_TimeZone.Id);
            int second = targetTime.Second;
            int minute = targetTime.Minute;
            int hour = targetTime.Hour;
            string curTimeFormat = targetTime.ToString("HH:mm:ss");
            txtTime.Text = curTimeFormat;

            try
            {
                Image digitalClockImage = null;
                Graphics g;

                digitalClockImage = Properties.Resources.Blank;
                int imageWidth = digitalClockImage.Width;
                int imageHeight = digitalClockImage.Height;
                // Create graphics object for alteration.

                Rectangle rect = new Rectangle(0, imageHeight / 3, txtTime.Width, imageHeight);
                StringFormat format = new StringFormat(StringFormatFlags.NoClip);

                g = Graphics.FromImage(digitalClockImage);

                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                g.FillRectangle(new SolidBrush(txtTime.BackColor), rect);
                g.DrawString(curTimeFormat, txtTime.Font, Brushes.Black, rect, format);
                g.Dispose();

                pictureBox1.Image = digitalClockImage;
                SendWorldClockImageMessage(digitalClockImage);                
            }
            catch (Exception e)
            {
                
            }
        }

        private void GetPayload()
        {
            try
            {
                JObject jsonGetPayload = new JObject();
                jsonGetPayload.Add("jsonrpc","2.0");
                jsonGetPayload.Add(new JProperty("method", "ax.get.payload"));
                JObject jsonParameters = new JObject();
                jsonParameters.Add(new JProperty("id", g_UUID));
                jsonGetPayload.Add("params", jsonParameters);
                g_ReceiveOrSendMsg = "[Send] " + jsonGetPayload.ToString();
                g_WebClient.SendMessage(jsonGetPayload.ToString());
                //UpdateUI();
                MethodInvoker mi = new MethodInvoker(this.ChangeUI);
                this.BeginInvoke(mi, null);
            }
            catch (Exception)
            {

                //throw;
            }
        }

        private void SetPayload()
        {
            try
            {
                JObject jsonSetPayload = new JObject();
                jsonSetPayload.Add(new JProperty("jsonrpc", "2.0"));
                jsonSetPayload.Add(new JProperty("method", "ax.set.payload"));
                JObject jsonParameters = new JObject();
                jsonParameters.Add(new JProperty("id", g_UUID));

                JObject jsonPayload = new JObject();
                jsonPayload.Add(new JProperty("city", g_CurrentCity));
                jsonPayload.Add(new JProperty("type", g_CurrentClockType));
                jsonParameters.Add(new JProperty("payload", jsonPayload));

                jsonSetPayload.Add("params", jsonParameters);
                g_ReceiveOrSendMsg = "[Send] " + jsonSetPayload.ToString();
                g_WebClient.SendMessage(jsonSetPayload.ToString());
            }
            catch (Exception)
            {
                //throw;
            }
        }

        private void SetPropertyUI()
        {
            try
            {
                JObject jsonSetProperty = new JObject();
                jsonSetProperty.Add(new JProperty("jsonrpc", "2.0"));
                jsonSetProperty.Add(new JProperty("method", "ax.send.to.property"));                

                if (g_CurrentCity != "")
                {
                    JObject jsonParameters = new JObject();
                    jsonParameters.Add(new JProperty("id", g_UUID));
                    JObject jsonPayload = new JObject();
                    jsonPayload.Add("action", "send_city_val");
                    jsonPayload.Add("city", g_CurrentCity);
                    jsonParameters.Add("payload", jsonPayload);
                    jsonSetProperty.Add("params", jsonParameters);

                    g_ReceiveOrSendMsg = "[Send] " + jsonSetProperty.ToString();
                    g_WebClient.SendMessage(jsonSetProperty.ToString());
                }

                if (g_CurrentClockType != "")
                {
                    jsonSetProperty.Remove("params");
                    JObject jsonParameters = new JObject();
                    jsonParameters.Add(new JProperty("id", g_UUID));
                    JObject jsonPayload = new JObject();
                    jsonPayload.Add("action", "send_type_val");
                    jsonPayload.Add("type", g_CurrentClockType);
                    jsonParameters.Add("payload", jsonPayload);
                    jsonSetProperty.Add("params", jsonParameters);

                    g_ReceiveOrSendMsg = "[Send] " + jsonSetProperty.ToString();
                    g_WebClient.SendMessage(jsonSetProperty.ToString());
                }
            }
            catch (Exception)
            {
                //throw;
            }
        }
    }
}
