using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace MyFirstWeixinProxy
{
    public partial class CreateMenus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        private string GetAccessToken()
        {
            string appID = "wxa237c84cbdc75795";
            string appsecret = "99cfc622b6b62785a35218a4b58ef7db";
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}"
                                        , appID, appsecret);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }

        protected void btnCreateMenus_Click(object sender, EventArgs e)
        {
           
            string s = GetMenuData();
            string tokenJson = GetAccessToken();
            TokenModel obj = (TokenModel)JsonConvert.DeserializeObject(tokenJson,typeof(TokenModel));

            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", obj.access_token);



            byte[] data = Encoding.UTF8.GetBytes(s);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            Stream newStream = request.GetRequestStream();

            // Send the data.
            newStream.Write(data, 0, data.Length);
            newStream.Close();

            // Get response
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string content = reader.ReadToEnd();
            Response.Write(content);

            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.Method = "POST";


            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //Stream myResponseStream = response.GetResponseStream();
            //StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            //string retString = myStreamReader.ReadToEnd();
            //myStreamReader.Close();
            //myResponseStream.Close();

            //Response.Write(retString);

        }

        public string GetMenuData()
        {
            string path = Server.MapPath("menus.txt");
            string text = File.ReadAllText(path);
            return text;
            //byte[] byData = new byte[100];
            //char[] charData = new char[1000];

            //string path = Server.MapPath("menus.txt");
            //FileStream fs = new FileStream(path, FileMode.Open);
            //fs.Seek(0, SeekOrigin.Begin);
            //fs.Read(byData, 0, 100); //byData传进来的字节数组,用以接受FileStream对象中的数据,第2个参数是字节数组中开始写入数据的位置,它通常是0,表示从数组的开端文件中向数组写数据,最后一个参数规定从文件读多少字符.
            //Decoder d = Encoding.Default.GetDecoder();
            //d.GetChars(byData, 0, byData.Length, charData, 0);
            //fs.Close();

            //return charData.ToString();
        }
    }

    public class TokenModel
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
    }
}