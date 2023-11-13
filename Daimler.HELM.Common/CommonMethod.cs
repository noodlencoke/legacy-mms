using Daimler.HELM.BizObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace Daimler.HELM.Common
{
    public static class CommonMethod
    {
        public static RequestLog GetRequestInfo(HttpRequest request)
        {
            string requestParams = "";
            foreach (var item in request.Headers.AllKeys)
            {
                requestParams += string.Format("{0}:{1} /n ", item, request.Headers[item]);
            }
            byte[] data = request.BinaryRead(request.TotalBytes);

            string strRequestParams = System.Text.Encoding.UTF8.GetString(data);


            RequestLog requestLog = new RequestLog();
            requestLog.Id = Guid.NewGuid();
            requestLog.RequestInfo = requestParams;
            requestLog.SendContent = System.Web.HttpUtility.UrlDecode(strRequestParams);
            requestLog.SendContentOfBase64 = strRequestParams;
            requestLog.CreatedDt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");

            return requestLog;
        }

        public static RequestLog GetRequestInfo(HttpRequest request, string dataSource)
        {
            string requestParams = "";
            foreach (var item in request.Headers.AllKeys)
            {
                requestParams += string.Format("{0}:{1} /n ", item, request.Headers[item]);
            }

            byte[] data = request.BinaryRead(request.TotalBytes);
            string strRequestParams = data.ToString();

            RequestLog requestLog = new RequestLog();
            requestLog.Id = Guid.NewGuid();
            requestLog.RequestInfo = requestParams;
            if (strRequestParams != null && strRequestParams != string.Empty)
            {
                requestLog.SendContent = System.Web.HttpUtility.UrlDecode(strRequestParams);
            }
            requestLog.DataSrouce = dataSource;
            requestLog.CreatedDt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");

            return requestLog;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// md5 method from lix
        public static string Md5(string str)
        {
            byte[] result = Encoding.Default.GetBytes(str);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            String md = BitConverter.ToString(output).Replace("-", "");
            md5.Clear();
            md5.Dispose();
            return md.ToLower();

        }

        public static string DoPost(string url, string postData)
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
            string returnValue = sr.ReadToEnd();
            sr.Close();
            response.Close();
            newStream.Close();
            return returnValue;
        }


        public static string DoPost(string url, string postData, string userName, string userPassword)
        {
            try
            {
                byte[] byteArray = Encoding.GetEncoding("gb2312").GetBytes(postData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(url));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded;charset=gb2312";
                webReq.Credentials = new NetworkCredential(userName, userPassword);
                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gb2312"));
                string returnMsg = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
                return returnMsg;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message+","+ex.StackTrace);
            }
        }


        /// <summary>
        /// 对象转换成json字符串
        /// </summary>
        /// <param name="accountList"></param>
        /// <returns></returns>
        public static string ConvertObjectToJson(Object obj)
        {
            StringBuilder sb = new StringBuilder();
            System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();
            json.Serialize(obj, sb);
            return sb.ToString();
        }

       

        /// <summary>
        /// string to base64
        /// </summary>
        /// <param name="code"> string </param>
        /// <returns>Encode string </returns>
        public static string EncodeBase64(string code)
        {
            string encode = "";
            byte[] bytes = Encoding.GetEncoding("UTF-8").GetBytes(code);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return encode;
        }

        /// <summary> 
        /// Base64 to string
        /// </summary> 
        /// <param name="code">Base64 string</param> 
        /// <returns>Decode string</returns> 
        public static string DecodeBase64(string code)
        {

            if (code == string.Empty)
                return string.Empty;
            string decode = "";
            byte[] bytes = Convert.FromBase64String(code);
            try
            {
                decode = Encoding.GetEncoding("UTF-8").GetString(bytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return decode;
        }

        /// <summary>
        /// get Node value
        /// </summary>
        /// <param name="xe"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static string GetXmlElementValue(System.Xml.Linq.XElement xe, string nodeName)
        {
            string result = "";
            try
            {
                if (xe != null && xe.Element(nodeName) != null)
                {
                    result = xe.Element(nodeName).Value;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// get Node value
        /// </summary>
        /// <param name="xe"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static string GetXmlElementValue(System.Xml.Linq.XElement xe, string nodeName, System.Xml.Linq.XNamespace nspace)
        {
            if (string.IsNullOrEmpty(nspace.NamespaceName))
                return GetXmlElementValue(xe, nodeName);
            string result = "";
            try
            {
                if (xe != null && xe.Element(nspace + nodeName) != null)
                {
                    result = xe.Element(nspace + nodeName).Value;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        /// <summary>
        /// byte[] to 16 string
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }



        public static string SerializerObjectToXML<T>(T obj)
        {
            MemoryStream stream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, obj);
            StreamReader sr = new StreamReader(stream);
            string xml = sr.ReadToEnd();
            stream.Close();
            stream.Dispose();
            sr.Close();
            sr.Dispose();

            return xml;
        }


        public static string EncryptString(string securitykey, string value)
        {
            RijndaelKey key = new RijndaelKey()
            {
                PassPhrase = securitykey
            };
            RijndaelCryptographyUtil rijnd = new RijndaelCryptographyUtil(key);
            return rijnd.Encrypt(value);
        }

        public static string DecryptString(string securitykey, string value)
        {
            try
            {
                RijndaelKey key = new RijndaelKey()
                {
                    PassPhrase = securitykey
                };
                RijndaelCryptographyUtil rijnd = new RijndaelCryptographyUtil(key);
                return rijnd.Decrypt(value);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// 分页控件
        /// </summary>
        /// <param name="pageSize">一页多少条</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="totalCount">总条数</param>
        /// <returns></returns>
        public static string ShowPageNavigate(int pageSize, int currentPage, int totalCount)
        {
            string redirectTo = "";
            pageSize = pageSize == 0 ? 3 : pageSize;
            var totalPages = Math.Max((totalCount + pageSize - 1) / pageSize, 1); //总页数
            var output = new StringBuilder();
            if (totalPages > 1)
            {
                if (currentPage != 1)
                {//处理首页连接
                    output.AppendFormat("<a class='pageLink' href='{0}?pageIndex=1&pageSize={1}'>首页</a> ", redirectTo, pageSize);
                }
                if (currentPage > 1)
                {//处理上一页的连接
                    output.AppendFormat("<a class='pageLink' href='{0}?pageIndex={1}&pageSize={2}'>上一页</a> ", redirectTo, currentPage - 1, pageSize);
                }
                else
                {
                    // output.Append("<span class='pageLink'>上一页</span>");
                }

                output.Append(" ");
                int currint = 5;
                for (int i = 0; i <= 10; i++)
                {//一共最多显示10个页码，前面5个，后面5个
                    if ((currentPage + i - currint) >= 1 && (currentPage + i - currint) <= totalPages)
                    {
                        if (currint == i)
                        {//当前页处理
                            //output.Append(string.Format("[{0}]", currentPage));
                            output.AppendFormat("<a class='cpb' href='{0}?pageIndex={1}&pageSize={2}'>{3}</a> ", redirectTo, currentPage, pageSize, currentPage);
                        }
                        else
                        {//一般页处理
                            output.AppendFormat("<a class='pageLink' href='{0}?pageIndex={1}&pageSize={2}'>{3}</a> ", redirectTo, currentPage + i - currint, pageSize, currentPage + i - currint);
                        }
                    }
                    output.Append(" ");
                }
                if (currentPage < totalPages)
                {//处理下一页的链接
                    output.AppendFormat("<a class='pageLink' href='{0}?pageIndex={1}&pageSize={2}'>下一页</a> ", redirectTo, currentPage + 1, pageSize);
                }
                else
                {
                    //output.Append("<span class='pageLink'>下一页</span>");
                }
                output.Append(" ");
                if (currentPage != totalPages)
                {
                    output.AppendFormat("<a class='pageLink' href='{0}?pageIndex={1}&pageSize={2}'>末页</a> ", redirectTo, totalPages, pageSize);
                }
                output.Append(" ");
            }
            output.AppendFormat("第{0}页 / 共{1}页", currentPage, totalPages);//这个统计加不加都行
            return output.ToString();
        }



        /// <summary>
        /// 切割批量信息
        /// </summary>
        /// <param name="msgList"></param>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public static List<Dictionary<string, List<MessageInfo>>> SplitBatchMessage(List<MessageInfo> msgList, int batchSize)
        {
            List<Dictionary<string, List<MessageInfo>>> pageDicList = new List<Dictionary<string, List<MessageInfo>>>();
            try
            {
                var query = from p in msgList
                            group p by p.RequestId into g
                            select g;

                var groupList = query.ToList();
                foreach (var group in groupList)
                {
                    var smsList = group.ToList();
                    if (smsList.Count >= batchSize)
                    {
                        int pageNumber = 1;
                        List<MessageInfo> pageList = new List<MessageInfo>();
                        for (var i = 0; i < smsList.Count; i++)
                        {
                            if (i == pageNumber * batchSize - 1 || i == smsList.Count - 1)
                            {
                                pageList.Add(smsList[i]);
                                Dictionary<string, List<MessageInfo>> dicList = new Dictionary<string, List<MessageInfo>>();
                                dicList.Add(pageNumber.ToString(), pageList);
                                pageDicList.Add(dicList);

                                pageList = new List<MessageInfo>();
                                pageNumber++;
                            }
                            else
                            {
                                pageList.Add(smsList[i]);
                            }
                        }
                    }
                    else
                    {
                        Dictionary<string, List<MessageInfo>> dicList = new Dictionary<string, List<MessageInfo>>();
                        dicList.Add(group.Key.ToString(), smsList);
                        pageDicList.Add(dicList);
                    }
                }
                return pageDicList;
            }
            catch (Exception ex)
            {
                throw new Exception("SplitBatchMessage error:"+ex.Message+","+ex.StackTrace);
            }
        }
    }
}
