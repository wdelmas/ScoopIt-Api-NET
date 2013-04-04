using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoopIt.Api.OAuth.Model
{
    using Hammock.Authentication.OAuth;

   public  class AuthorizeModel
    {
        public OAuthCredentials OAuthCredentials { get; set; }
        public String Callback { get; set; }
    }
}
