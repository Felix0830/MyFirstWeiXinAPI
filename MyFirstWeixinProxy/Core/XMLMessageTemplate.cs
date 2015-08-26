using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFirstWeixinProxy.Core
{
    /// <summary>
    /// 回复消息XML模板
    /// </summary>
    public class XMLMessageTemplate
    {
        /// <summary>
        /// 回复文本消息
        /// </summary>
        public static string TextXML
        {
            get
            {
                return @" <xml>
                              <ToUserName><![CDATA[{0}]]></ToUserName>
                              <FromUserName><![CDATA[{1}]]></FromUserName> 
                              <CreateTime>{2}</CreateTime>
                              <MsgType><![CDATA[text]]></MsgType>
                              <Content><![CDATA[{3}]]></Content>
                              </xml>";
            }
           
        }

        /// <summary>
        /// 回复图片消息
        /// </summary>
        public static string ImageXML
        {
            get
            {
                return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[image]]></MsgType>
                            <Image>
                            <MediaId><![CDATA[{3}]]></MediaId>
                            </Image>
                            </xml>";
            }
        }

        /// <summary>
        /// 回复语音消息
        /// </summary>
        public static string VoiceXML
        {
            get
            {
                return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[voice]]></MsgType>
                            <Voice>
                            <MediaId><![CDATA[{3}]]></MediaId>
                            </Voice>
                            </xml>";
            }
        }

        /// <summary>
        /// 回复视频消息
        /// </summary>
        public static string VideoXML
        {
            get
            {
                return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[video]]></MsgType>
                            <Video>
                            <MediaId><![CDATA[{3}]]></MediaId>
                            <Title><![CDATA[{4}]]></Title>
                            <Description><![CDATA[{5}]]></Description>
                            </Video> 
                            </xml>";
            }
        }

        /// <summary>
        /// 回复音乐消息
        /// </summary>
        public static string MusicXML
        {
            get
            {
                return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[music]]></MsgType>
                            <Music>
                            <Title><![CDATA[{3}]]></Title>
                            <Description><![CDATA[{4}]]></Description>
                            <MusicUrl><![CDATA[{5}]]></MusicUrl>
                            <HQMusicUrl><![CDATA[{5}]]></HQMusicUrl>
                            </Music>
                            </xml>";
            }
        }

        /// <summary>
        /// 回复图文消息
        /// </summary>
        public static string ImageArticleXML
        {
            get
            {
                return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[news]]></MsgType>
                            <ArticleCount>{3}</ArticleCount>
                            <Articles>{4}</Articles>
                            </xml>";
            }
        }

        public static string ArticleXML
        {
            get
            {
                return @"<item>
                            <Title><![CDATA[{0}]]></Title> 
                            <Description><![CDATA[{1}]]></Description>
                            <PicUrl><![CDATA[{2}]]></PicUrl>
                            <Url><![CDATA[{3}]]></Url>
                            </item>";
            }
        }

    }
}