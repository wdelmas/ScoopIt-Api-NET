using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoopIt.Api.Service.Model
{
    using Hammock.Authentication.OAuth;
    using ScoopIt.Api.OAuth;
    using ScoopIt.Api.OAuth.Model;

    public class AnonymousAccess
    {
        #region Proerties

        public ScoopItService Api { get; set; }
        public OAuthCredentials AccessCredentials
        {
            get
            {
                return new OAuthCredentials
                {
                    SignatureMethod = OAuthSignatureMethod.HmacSha1,
                    ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                    ConsumerKey = OauthUtil.consumerKey,
                    ConsumerSecret = OauthUtil.consumerKeySecret,

                };
            }
        }

        #endregion

        #region Constructor

        public AnonymousAccess()
        {
           
        }

        #endregion



    }
}

