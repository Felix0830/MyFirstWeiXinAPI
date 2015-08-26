/*
 * 单词说明：
 * WX：代表微信
 * GMT：代表格林威治时间
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Xml;

namespace MyFirstWeixinProxy.Core
{
    public class WXMessageModel
    {
        public string FromUserName { get; set; }   //消息发送方微信号
        public string ToUserName { get; set; }    //消息接收方微信号，一般为公众平台账号微信号
        public string MsgType { get; set; }     //信息类型
        public string EventName { get; set; }  //事件名
        public string Content { get; set; }   //信息内容
        public string EventKey { get; set; }
        public string CreateTime { get; set; }
    }

    public class WXMessageHelper
    {
        private const string TOKEN = "onewxdemo";
        private static HttpContext Current = HttpContext.Current;

        /// <summary>
        /// 验证GET请求
        /// </summary>
        /// <returns></returns>
        public static string ValidateWXGetRequest()
        {
            string signature = Current.Request.QueryString["signature"];
            string timestamp = Current.Request.QueryString["timestamp"];
            string nonce = Current.Request.QueryString["nonce"];
            string echostr = Current.Request.QueryString["echostr"];
            string[] array = { TOKEN, timestamp, nonce };
            Array.Sort(array);
            string tempStr = string.Join("", array);
            tempStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tempStr, "SHA1");
            tempStr = tempStr.ToLower();
            if (tempStr == signature)
            {
                return echostr;
            }

            //验证不通过则返回空串
            return string.Empty;
        }

        /// <summary>
        /// 处理微信Post过来的请求
        /// </summary>
        /// <returns>返回对应类型的消息</returns>
        public static string ProcessPostRequest()
        {
            Stream stream = System.Web.HttpContext.Current.Request.InputStream;
            XmlDocument xml = new XmlDocument();
            xml.Load(stream);
            XmlElement root = xml.DocumentElement;
            WXMessageModel wxModel = AnalysisRequestXML(root);

            string returnMsg = string.Empty;

            switch (wxModel.MsgType)
            {
                case "text":
                    returnMsg = ProcessTextTypeMsg(wxModel);
                    break;
                case "event":
                    returnMsg = ProcessEventTypeMsg(wxModel);
                    break;
                case "image":
                    break;
                case "voice":
                    break;
                case "vedio":
                    break;
                case "location":
                    break;
                case "link":
                    break;
                default:
                    break;
            }
            return returnMsg;
        }

        #region 业务处理工厂

        /// <summary>
        /// 获得格林威治时间
        /// </summary>
        /// <returns></returns>
        private static int GetGMT()
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(DateTime.Now - startTime).TotalSeconds;
        }

        /// <summary>
        /// 分析微信Post过来的XML
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private static WXMessageModel AnalysisRequestXML(XmlElement root)
        {
            WXMessageModel xmlMsg = new WXMessageModel()
            {
                FromUserName = root.SelectSingleNode("FromUserName").InnerText,
                ToUserName = root.SelectSingleNode("ToUserName").InnerText,
                MsgType = root.SelectSingleNode("MsgType").InnerText,
            };
            if (xmlMsg.MsgType.Trim().ToLower() == "text")
            {
                xmlMsg.Content = root.SelectSingleNode("Content").InnerText;
            }
            else if (xmlMsg.MsgType.Trim().ToLower() == "event")
            {
                xmlMsg.EventName = root.SelectSingleNode("Event").InnerText;
            }
            return xmlMsg;
        }

        /// <summary>
        /// 处理文本消息，并且返回消息
        /// </summary>
        /// <param name="xmlModel"></param>
        /// <returns></returns>
        private static string ProcessTextTypeMsg(WXMessageModel xmlModel)
        {
            string xmlTemplate = string.Empty;
            string returnMsg = string.Empty;
            string musicURL=@"http://qxw1590700051.my3w.com/Musics/xhn.mp3";
            string content = xmlModel.Content.Trim();

            if (content == "1")
            {
                xmlTemplate = XMLMessageTemplate.MusicXML;
                returnMsg = string.Format(xmlTemplate, xmlModel.FromUserName, xmlModel.ToUserName, GetGMT(), "喜欢你"
                    , "这是邓紫棋翻唱Beyond的喜欢你，很好听哦！", musicURL);
            }
            else if (content == "2")
            {
                string msg = "这是工号9527开发的第一个微信App ^-^";
                xmlTemplate = XMLMessageTemplate.TextXML;
                return string.Format(xmlTemplate, xmlModel.FromUserName, xmlModel.ToUserName, GetGMT(), msg);
            }
            else
            {
                string msg = "回复 1 获取音乐\n" + "回复 2 获取文本";
                xmlTemplate = XMLMessageTemplate.TextXML;
                return string.Format(xmlTemplate, xmlModel.FromUserName, xmlModel.ToUserName, GetGMT(), msg);
            }

            return returnMsg;
        }

        /// <summary>
        /// 处理订阅事件
        /// </summary>
        /// <param name="wxModel"></param>
        /// <returns></returns>
        private static string ProcessEventTypeMsg(WXMessageModel wxModel)
        {
            string returnMsg = string.Empty;
            if (!string.IsNullOrEmpty(wxModel.EventName) && wxModel.EventName.Trim() == "subscribe")
            {
                string xmlTemplate = XMLMessageTemplate.TextXML;
                string msg = "感谢您关注【每日一笑】公众号。每日一笑给您的生活加点料^-~！！！\n"
                              +"回复 1 获取音乐\n"
                              +"回复 2 获取文本";
                returnMsg = string.Format(xmlTemplate, wxModel.FromUserName, wxModel.ToUserName, GetGMT(), msg);           
            }
            return returnMsg;
        }

        private static string GetResponceMsg(WXMessageModel xmlMsg)
        {
            string content = xmlMsg.Content.Trim();

            System.Text.StringBuilder retsb = new StringBuilder(200);
            if (content == "1")
            {
                retsb.Append("今天是小雨转阴");
            }
            else if (content == "2")
            {
                retsb.Append("晚上请我吃饭呗^-^!!!");
            }
            else
            {
                string str = "回复 1 获取音乐\n"
                             + "回复 2 获取图片";
                retsb.Append(str);
            }

            return retsb.ToString();
        } 
        #endregion
    }
}