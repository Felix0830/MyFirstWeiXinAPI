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
                xmlMsg.EventName = root.SelectSingleNode("Event").InnerText.ToLower();
                if (xmlMsg.EventName.Equals("click"))
                {
                    xmlMsg.EventKey = root.SelectSingleNode("EventKey").InnerText;
                }
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
            string content = xmlModel.Content.Trim();

            if (content == "1")
            {
                returnMsg = GetTuWenMsg(xmlModel);
            }
            else if (content == "2")
            {
                returnMsg = GetMusic(xmlModel);
            }
            else
            {
                string msg = "IT资讯回复：1\n" + "随机播歌回复：2\n" + "我的简历回复：3";
                xmlTemplate = XMLMessageTemplate.TextXML;
                return string.Format(xmlTemplate, xmlModel.FromUserName, xmlModel.ToUserName, GetGMT(), msg);
            }

            return returnMsg;
        }

        /// <summary>
        /// 事件工厂
        /// </summary>
        /// <param name="wxModel"></param>
        /// <returns></returns>
        private static string ProcessEventTypeMsg(WXMessageModel wxModel)
        {
            string returnMsg = string.Empty;
            switch (wxModel.EventName.Trim())
            {
                case "subscribe":
                    returnMsg = ProcessSubscribeEvent(wxModel);
                    break;
                case "click":
                    returnMsg = ProcessClickEvent(wxModel);
                    break;
                case "scan":
                    break;
                case "location":
                    break;
                default:
                    break;
            }
            return returnMsg;
        }

        /// <summary>
        /// 处理关注事件
        /// </summary>
        /// <param name="wxModel"></param>
        /// <returns></returns>
        private static string ProcessSubscribeEvent(WXMessageModel wxModel)
        {
            string xmlTemplate = XMLMessageTemplate.TextXML;
            string msg = "感谢您关注【每日一笑】公众号。每日一笑给您的生活加点料^-~！！！\n"
                            + "IT资讯回复：1\n"
                            + "随机播歌回复：2\n"
                            + "我的简历回复：3";
            return string.Format(xmlTemplate, wxModel.FromUserName, wxModel.ToUserName, GetGMT(), msg);
        }

        /// <summary>
        /// 处理自定义菜单事件
        /// </summary>
        /// <param name="wxModel"></param>
        /// <returns></returns>
        private static string ProcessClickEvent(WXMessageModel wxModel)
        {
            string returnMsg = string.Empty;
            switch (wxModel.EventKey)
            {
                case "It_Info":
                    returnMsg = GetTuWenMsg(wxModel);
                    break;
                case "Random_Music":
                    returnMsg = GetMusic(wxModel);
                    break;
                default:
                    break;
            }
            return returnMsg;
        }

        private static string GetTuWenMsg(WXMessageModel wxModel)
        {
            string responceMsg = string.Empty;
            string articleObj = string.Format(XMLMessageTemplate.ArticleXML
                                            , "Tiny框架创始人悠然：好的软件设计是“品”出来的", "Tiny是基于Java开发的一款开源框架，主要技术领域为J2EE及应用开发平台领域。"
                                            , "http://images.csdn.net/20150831/4.jpg", "http://www.csdn.net/article/2015-08-31/2825576-Tiny");
            articleObj += string.Format(XMLMessageTemplate.ArticleXML
                                            , "调查：云计算已成企业标配，反之则很难生存", "哈佛商业评论的研究显示迁移到云平台为企业带来的优势已不再明显，因为云计算已成企业标配。"
                                            , "http://images.csdn.net/20150831/518-131030094253504.jpg", "http://www.csdn.net/article/2015-08-31/2825592");
            responceMsg = string.Format(XMLMessageTemplate.ImageArticleXML, wxModel.FromUserName, wxModel.ToUserName, GetGMT()
                                        , 2, articleObj);

            return responceMsg;
        }

        private static string GetMusic(WXMessageModel wxModel)
        {
            Random random = new Random();
            int num = random.Next(1, 4);

            string xmlTemplate = string.Empty;
            string returnMsg = string.Empty;
            string musicURL = string.Format("http://qxw1590700051.my3w.com/Musics/{0}.mp3", num);

            string[] mTitle = new string[] {"喜欢你", "浮夸", "克罗地亚狂想曲" };
            string[] js = new string[] { "这是邓紫棋翻唱Beyond的喜欢你，很好听哦！", "这是陈亦迅所唱哦，超好听！", "钢琴王子--马克西姆" };

            xmlTemplate = XMLMessageTemplate.MusicXML;
            int i = num - 1;
            returnMsg = string.Format(xmlTemplate, wxModel.FromUserName, wxModel.ToUserName, GetGMT(), mTitle[i], js[i], musicURL);

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