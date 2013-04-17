using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoopIt.Api.Service.Model
{
    using Hammock;
    using Hammock.Authentication.OAuth;
    using Hammock.Web;
    using ScoopIt.Api.OAuth;
    using ScoopIt.Api.OAuth.Model;

    public class AuthorizeAccess
    {
        #region Properties

        public AccessTokenModel AccessTokenModel { get; set; }
        public OAuthCredentials AccessCredentials
        {
            get
            {
                return new OAuthCredentials
                {
                    Type = OAuthType.AccessToken,
                    SignatureMethod = OAuthSignatureMethod.HmacSha1,
                    ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                    ConsumerKey = OauthUtil.consumerKey,
                    ConsumerSecret = OauthUtil.consumerKeySecret,
                    Token = this.AccessTokenModel.Token,
                    TokenSecret = this.AccessTokenModel.TokenSecret
                };
            }
        }

        #endregion

        #region Constructor

        public AuthorizeAccess(AccessTokenModel accessTokenModel)
        {
            this.AccessTokenModel = accessTokenModel;
        }

        #endregion

    }
}
