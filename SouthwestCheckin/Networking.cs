using System;
using System.IO;
using System.Net;
using System.Text;

namespace SouthwestCheckin
{
    class Networking
    {
        CookieContainer cookies;
        String UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
        static Networking()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }
        public Networking()
        {
            cookies = new CookieContainer();
            cookies.Add(new CookieCollection());
        }
        public String GetData(String url)
        {
            return PostData(url, null);
        }
        public String PostData(String url, String data)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = cookies;
            request.UserAgent = UserAgent;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate, br";
            request.Headers[HttpRequestHeader.AcceptLanguage] = "en-US,en;q=0.8";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("DNT", "1");
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.KeepAlive = true;
            if (data != null)
            {
                request.Method = WebRequestMethods.Http.Post;
                byte[] byteArray = Encoding.ASCII.GetBytes(data);
                request.ContentLength = byteArray.Length;
                using (Stream requestStream = request.GetRequestStream())
                    requestStream.Write(byteArray, 0, byteArray.Length);
            }
            String html = "";
            using (StreamReader streamReader = new StreamReader((request.GetResponse() as HttpWebResponse).GetResponseStream()))
                html = streamReader.ReadToEnd();
            cookies = request.CookieContainer;
            return html;
        }
    }
}
