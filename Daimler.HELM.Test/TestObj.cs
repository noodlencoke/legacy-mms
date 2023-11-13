using Daimler.HELM.PublicService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.Test
{
    public static class TestObj
    {

        public static string DoPost(string url, string postData)
        {
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(postData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(url));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                //sr.Read();
                var val =sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();

                Console.WriteLine("finished :"+val); 

                return val;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }




        }





    }
    public class BaseServiceProxy<TChannel> : ClientBase<TChannel> where TChannel : class
    {

    }

    public class MessageServiceProxy : BaseServiceProxy<ISendSMS>
    {
        public ISendSMS ServiceChannel
        {
            get { return Channel; }
        }
    }
}
