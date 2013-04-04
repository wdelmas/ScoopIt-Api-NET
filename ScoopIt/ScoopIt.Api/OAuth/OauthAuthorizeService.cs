namespace ScoopIt.Api.OAuth
{
    using System;
    using System.Linq;
    using System.Web;
    using Hammock;
    using Hammock.Authentication.OAuth;
    using Hammock.Web;
    using ScoopIt.Api.OAuth.Model;

    public class OauthAuthorizeService
    {
        #region Attributes

        static String Token { get; set; }
        static String TokenSecret { get; set; }

        #endregion

        #region Methodes

        public static CallbackModel AuthenticateToScoopIt()
        {
            var credentials = new OAuthCredentials
            {
                CallbackUrl = OauthUtil.CallbackUri,
                ConsumerKey = OauthUtil.consumerKey,
                ConsumerSecret = OauthUtil.consumerKeySecret,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                // Verifier = "true",
                Type = OAuthType.RequestToken
            };

            var client = new RestClient { Authority = OauthUtil.Base_url_ScoopIt, Credentials = credentials };
            var request = new RestRequest { Path = OauthUtil.RequestTokenUri };
            RestResponse response = client.Request(request);

            Token = GetTokenFromResponse(response);
            TokenSecret = GetTokenSecretFromResponse(response);

            return new CallbackModel()
                       {
                           OAuthCredentials = credentials,
                           Callback = OauthUtil.AuthorizeUri + "?oauth_token=" + Token
                       };
        }

        public static AccessTokenModel CallBackFromScoopIt()
        {
            var token = HttpContext.Current.Request["oauth_token"];
            var verifier = HttpContext.Current.Request["oauth_verifier"];

            var credentials = new OAuthCredentials
            {
                ConsumerKey = OauthUtil.consumerKey,
                ConsumerSecret = OauthUtil.consumerKeySecret,
                Token = token,
                TokenSecret = TokenSecret,
                Verifier = verifier,
                Type = OAuthType.AccessToken,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                Version = "1.0"
            };
            var client = new RestClient { Authority = OauthUtil.Base_url_ScoopIt, Credentials = credentials, Method = WebMethod.Post };
            var request = new RestRequest { Path = OauthUtil.AccessTokenUri };
            RestResponse response = client.Request(request);

            if (!string.IsNullOrEmpty(response.Content))
            {
                return new AccessTokenModel()
                       {
                           Token = GetTokenFromResponse(response),
                           TokenSecret = GetTokenSecretFromResponse(response)
                       };
            }
            else
            {
                return null;
            }


        }

        private static String GetTokenFromResponse(RestResponse resp)
        {
            string token = resp.Content.Split('&').Single(s => s.StartsWith("oauth_token=")).Split('=')[1];
            return token;
        }

        private static String GetTokenSecretFromResponse(RestResponse resp)
        {
            string tokenSecret = resp.Content.Split('&').Single(s => s.StartsWith("oauth_token_secret=")).Split('=')[1];
            return tokenSecret;
        }

        #endregion
    }
}
