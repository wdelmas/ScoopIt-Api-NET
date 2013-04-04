using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ScoopIt.Api.OAuth
{
    using System.Web;

    public class OauthUtil
    {
        private static readonly String ApplicationPath = string.Format("{0}://{1}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority.TrimEnd('/'));

        public const String Base_url_ScoopIt = "http://www.scoop.it/";


        public static string RequestTokenUri = "oauth/request";
        public static string AuthorizeUri = "https://www.scoop.it/oauth/authorize";
        public static string AccessTokenUri = "oauth/access";
        public static string CallbackUri = ApplicationPath + "/Scoopit/CallbackFromScoopIt";


        public static string consumerKey = ConfigurationManager.AppSettings["CONSUMER_KEY"];
        public static string consumerKeySecret = ConfigurationManager.AppSettings["CONSUMER_SECRET"];

        public static string oAuthVersion = "1.0";
    }
}
