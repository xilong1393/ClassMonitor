using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ClassMonitor3.Util
{
    public static class Helper
    {
        public static HttpClient CreateClient()
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30); //set your own timeout.
            client.BaseAddress = new Uri("http://localhost:9090/");
            return client;
        }
        public static ComboItems[] GetComboItems()
        {
            return new ComboItems[] {//new ComboItems("0"," "),
                    new ComboItems("1","CameraVideo"),
                    new ComboItems("2", "whiteboard1"),
                    new ComboItems("3","whiteboard2"),
                    new ComboItems("4","TeacherScreen")
            };
        }
        public static void SafeInvoke(this Control uiElement, Action updater, bool forceSynchronous)
        {
            if (uiElement == null)
            {
                throw new ArgumentNullException("uiElement");
            }

            if (uiElement.InvokeRequired)
            {
                if (forceSynchronous)
                {
                    uiElement.Invoke((Action)delegate { SafeInvoke(uiElement, updater, forceSynchronous); });
                }
                else
                {
                    uiElement.BeginInvoke((Action)delegate { SafeInvoke(uiElement, updater, forceSynchronous); });
                }
            }
            else
            {
                if (uiElement.IsDisposed)
                {
                    throw new ObjectDisposedException("Control is already disposed.");
                }

                updater();
            }
        }

        public static string GenerateSessionID() {
            String UUID = Guid.NewGuid().ToString();
            return UUID;
        }
        public static IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }

        public static T DeepCopy<T>(this T obj)
        {
            object result = null;
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                result = (T)formatter.Deserialize(ms);
                ms.Close();
            }
            return (T)result;
        }
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static string formatClassName(string originalString)
        {
            string formatString = originalString;
            try
            {
                Regex rex = new Regex(@"[^\w|\.|\[|\]|\(|\)|\-]+");
                formatString = rex.Replace(originalString, " ");
            }
            catch (Exception)
            {
                formatString = originalString;
            }
            return formatString;
        }

        public static int[] SixtyArray()
        {
            int[] result = new int[60];
            for (int i = 0; i < 60; i++)
            {
                result[i] = i;
            }
            return result;
        }

        public static int[] TwentyFourArray()
        {
            int[] result = new int[24];
            for (int i = 0; i < 24; i++)
            {
                result[i] = i;
            }
            return result;
        }
    }
}
