using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using System.Data.Common;
using System.Collections;
//using System.Web.UI.WebControls;
//using System.Web.UI.HtmlControls;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Json;
using System.Xml;

namespace Daimler.HELM.WechatInterface
{
    public class JsonHelper
    {        
        /// <summary>
        /// List转成json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ListToJson<T>(IList<T> list,string JsonName)
        {
            string jsonName = JsonName;
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName))
            {
                jsonName = list[0].GetType().Name;
            }
            Json.Append("{\""+ jsonName + "\":[");
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    T obj = Activator.CreateInstance<T>();
                    PropertyInfo[] pi = obj.GetType().GetProperties();
                    Json.Append("{");
                    for (int j = 0; j < pi.Length; j++)
                    {
                        Type type;
                        object o = pi[j].GetValue(list[i], null);
                        string v = string.Empty;
                        if (o != null)
                        {
                            type = o.GetType();
                            v = o.ToString();
                        }
                        else
                        {
                            type = typeof(string);
                        }

                        Json.Append("\"" + pi[j].Name.ToLower().ToString() + "\":" + StringFormat(v, type));

                        if (j < pi.Length - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < list.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }


        /// <summary>
        /// 反序列化单个对象
        /// </summary>
        public static T JsonDeserializeBySingleData<T>(string JsonString)
        {
            string jsonString = JsonString;
            //将"yyyy-MM-dd HH:mm:ss"格式的字符串转为"\/Date(1294499956278+0800)\/"格式 
            string p = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateStringToJsonDate);
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }


        /// <summary> 
        /// 将时间字符串转为Json时间
        /// </summary>
        private static string ConvertDateStringToJsonDate(Match m)
        {
            string result = string.Empty;
            DateTime dt = DateTime.Parse(m.Groups[0].Value);
            dt = dt.ToUniversalTime();
            TimeSpan ts = dt - DateTime.Parse("1970-01-01");
            result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);
            return result;
        }

        /// <summary>
        /// 对象转换为Json字符串
        /// </summary>
        /// <param name="jsonObject">对象</param>
        /// <returns>Json字符串</returns>
        public static string ToJson(object jsonObject)
        {
            try
            {
                StringBuilder jsonString = new StringBuilder();
                jsonString.Append("{");
                PropertyInfo[] propertyInfo = jsonObject.GetType().GetProperties();
                for (int i = 0; i < propertyInfo.Length; i++)
                {
                    object objectValue = propertyInfo[i].GetGetMethod().Invoke(jsonObject, null);
                    if (objectValue == null)
                    {
                        continue;
                    }
                    StringBuilder value = new StringBuilder();
                    if (objectValue is DateTime || objectValue is Guid || objectValue is TimeSpan)
                    {
                        value.Append("\"" + objectValue.ToString() + "\"");
                    }
                    else if (objectValue is string)
                    {
                        value.Append("\"" + objectValue.ToString() + "\"");
                    }
                    else if (objectValue is IEnumerable)
                    {
                        value.Append(ToJson((IEnumerable)objectValue));
                    }
                    else
                    {
                        value.Append("\"" + objectValue.ToString() + "\"");
                    }
                    jsonString.Append("\"" + propertyInfo[i].Name + "\":" + value + ",");
                }
                return jsonString.ToString().TrimEnd(',') + "}";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 对象集合转换Json
        /// </summary>
        /// <param name="array">集合对象</param>
        /// <returns>Json字符串</returns>
        public static string ToJson(IEnumerable array)
        {
            string jsonString = "[";
            foreach (object item in array)
            {
                jsonString += ToJson(item) + ",";
            }
            if (jsonString.Length > 1)
            {              
               jsonString = jsonString.Remove(jsonString.Length - 1, 1);
            }
            else
            {
                jsonString = "[]";
            }
            return jsonString + "]";
        }

        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string String2Json(string s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                sb.Append(FormatChar(c));
            }
            return sb.ToString();
        }
        private static string FormatChar(char c)
        {
            Dictionary<char, string> charDic = new Dictionary<char, string>();
            charDic.Add('\"', "\\\"");
            charDic.Add('\\', "\\\\");
            charDic.Add('/',  "\\/");
            charDic.Add('\b', "\\b");
            charDic.Add('\f', "\\f");
            charDic.Add('\n', "\\n");
            charDic.Add('\r', "\\r");
            charDic.Add('\t', "\\t");
            charDic.Add('\v', "\\v");
            charDic.Add('\0', "\\0");
            return charDic.ContainsKey(c) ? charDic[c] : c.ToString();
            
        }
        /// <summary>
        /// 格式化字符型、日期型、布尔型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string StringFormat(string Str, Type type)
        {
            string str = Str;
            if (type != typeof(string) && string.IsNullOrEmpty(str))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(string))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            else if (type == typeof(byte[]))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(Guid))
            {
                str = "\"" + str + "\"";
            }
            return str;
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
        /// xml对象转换为json
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static string ConvertXmlToJson(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
           return Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
        }


    }    
}
