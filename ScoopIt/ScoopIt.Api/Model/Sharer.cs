using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Scoopit.Api.Model
{
    /// <summary>
    /// Holds data related to a "sharer". A "sharer" is basically an account to a publish service the user registered in the dedicated website page(eg:
    /// twitter account, facebook account, tumblr account).
    /// 
    /// <p>
    /// http://www.scoop.it/dev/api/1/types#sharer
    /// </p>
    /// </summary>
    public class Sharer
    {

        #region attributes
        /** name of this sharer */
        public String SharerName { get; set; }
        /** internal id */
        public String SharerId { get; set; }
        /** internal id */
        public long CnxId { get; set; }
        /**
    * the name of the user on the external service represented by this sharer, for a facebook account it will be the facebook user name
    */
        public String Name { get; set; }
        /**
    * true if the external service needs a manually specified text to publish a post
    */

        #endregion

        #region methodes
        public Boolean mustSpecifyShareText { get; set; }

        public static Sharer getFromJSON(JObject obj)
        {
            if (obj == null)
            {
                return null;
            }
            Sharer sharer = new Sharer();
            sharer.CnxId = (long)obj["cnxId"];
            sharer.SharerName = (string)obj["sharerName"];
            sharer.SharerId = (string)obj["sharerId"];
            sharer.Name = (string)obj["name"];
            sharer.mustSpecifyShareText = (Boolean)obj["mustSpecifyShareText"];
            return sharer;
        }

        public String generateJsonFragment(String text)
        {
            String fragment = "{\"sharerId\": \"" + this.SharerId + "\", \"cnxId\": " + this.CnxId;
            if (text != null)
            {
                fragment += ", \"text\": \"" + text + "\"";
            }
            fragment += "}";
            return fragment;
        }

        #endregion

    }
}
