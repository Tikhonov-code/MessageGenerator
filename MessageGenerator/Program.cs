using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageGenerator
{
    class Program
    {
        static Random x;
        static long msgCount = 0;
        static void Main(string[] args)
        {
            x = new Random();
            do
            {
                SendMessage();
                if (msgCount == (long.MaxValue - 1))
                {
                    msgCount = 0;
                }
                else
                {
                    msgCount++;
                }

                Thread.Sleep(1000);//ms

            } while (true);

        }

        private static void SendMessage()
        {
            string result = string.Empty;
            string url = Properties.Settings.Default.BrokerURL;
            url += CreateMessage();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
           
            Console.WriteLine("#"+msgCount+"  "+DateTime.Now.ToShortTimeString() + " message was sent. "+result);
        }

        private static string CreateMessage()
        {
            int channel = x.Next(1,10);
            string text = "message :" + x.NextDouble() * 1000.0;
            return "/" + channel + "/" + text;
        }
    }
}
