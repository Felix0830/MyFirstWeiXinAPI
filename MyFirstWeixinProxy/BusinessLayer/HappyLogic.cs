using MyFirstWeixinProxy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFirstWeixinProxy.BusinessLayer
{
    public class HappyLogic
    {
        private HappyLogic() { }
        private static HappyLogic instance = null;
        public static HappyLogic Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HappyLogic();
                }
                return instance;
            }
        }

        /// <summary>
        /// 验证GET请求
        /// </summary>
        /// <returns></returns>
        public string ValidateGetRequest()
        {
            return WXMessageHelper.ValidateWXGetRequest();
        }

        /// <summary>
        /// 处理POST请求
        /// </summary>
        /// <returns></returns>
        public string ProcessPostRequest()
        {
            return WXMessageHelper.ProcessPostRequest();
        }


    }
}