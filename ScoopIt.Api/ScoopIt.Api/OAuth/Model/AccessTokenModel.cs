using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoopIt.Api.OAuth.Model
{
    public class AccessTokenModel
    {
        public  string Token { get; set; }
        public  string TokenSecret { get; set; }
    }
}
