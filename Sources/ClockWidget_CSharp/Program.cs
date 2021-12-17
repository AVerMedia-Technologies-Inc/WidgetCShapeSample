using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Websocket2CreatorCentralPlugin
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string pluginUUID = "";
            string port = "1234";
            if (args.Length >= 2)
            {
                pluginUUID = args[0];
                port = args[1];
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new PluginForm(pluginUUID, port));
        }
    }
}
