using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoopIt.Api.Service
{
    using Hammock;
    using Hammock.Authentication.OAuth;
    using Hammock.Web;
    using ScoopIt.Api.OAuth;
    using ScoopIt.Api.OAuth.Model;

    public class ScoopItService
    {
        public static string ConsumerKey { get { return OauthUtil.consumerKey; } }
        public static string ConsumerKeySecret { get { return OauthUtil.consumerKeySecret; } }
        public AccessTokenModel AccessTokenModel { get; set; }

        #region API PATH
        private const String REQUEST_TOPIC = "api/1/topic";
        private const String REQUEST_RESOLVER = "api/1/resolver";
        #endregion
        public ScoopItService(AccessTokenModel accessTokenModel)
        {
            this.AccessTokenModel = accessTokenModel;
        }

        private OAuthCredentials AccessCredentials
        {
            get
            {
                return new OAuthCredentials
                {
                    Type = OAuthType.AccessToken,
                    SignatureMethod = OAuthSignatureMethod.HmacSha1,
                    ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                    ConsumerKey = ConsumerKey,
                    ConsumerSecret = ConsumerKeySecret,
                    Token = this.AccessTokenModel.Token,
                    TokenSecret = this.AccessTokenModel.TokenSecret
                };
            }
        }

        #region Helper

        public RestResponse GetTopic(string path)
        {
            var client = new RestClient()
            {
                Authority = OauthUtil.Base_url_ScoopIt,
                Credentials = AccessCredentials,
                Method = WebMethod.Get
            };

            var request = new RestRequest { Path = REQUEST_TOPIC, };
            request.AddParameter("id", path);

            return client.Request(request);
    
        }

   

        #endregion
    }
}
