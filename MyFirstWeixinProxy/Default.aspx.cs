using MyFirstWeixinProxy.BusinessLayer;
using MyFirstWeixinProxy.Core;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyFirstWeixinProxy
{
    public partial class Default : System.Web.UI.Page
    {
        public string returnMsg = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.HttpMethod.ToLower() == "get")
                {
                    returnMsg = HappyLogic.Instance.ValidateGetRequest();
                }
                else
                {
                    returnMsg = HappyLogic.Instance.ProcessPostRequest();
                }

                Response.Write(returnMsg);
                Response.End();
            }
            catch
            { }
        }
    }
}