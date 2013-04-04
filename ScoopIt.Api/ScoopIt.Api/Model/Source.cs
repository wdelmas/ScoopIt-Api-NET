using System;
using Newtonsoft.Json.Linq;

namespace Scoopit.Api.Model
{
    /// <summary>
    ///  Holds data related to a Source: something that suggests content to curate to users.
    ///
    ///<p>
    ///http://www.scoop.it/dev/api/1/types#source
    ///</p>
    /// </summary>
    public class Source
    {
        #region attributes
        /** id of the source */
        public long Id { get; set; }
        /** name of the source (human readable) */
        public String Name { get; set; }
        /** description of the source (human readable) */
        public String Description { get; set; }
        /** type of the source (developper readable identifier) */
        public String Type { get; set; }
        /** url of an icon representing this source */
        public String IconUrl { get; set; }
        /**
         * url of this source (may be a user profile url, a link to a youtube search...)
         */
        public String Url { get; set; }

        #endregion

        #region methodes

        public static Source GetFromJSON(JObject obj)
        {
            if (obj == null)
            {
                return null;
            }

            var source = new Source();

            source.Id = (long)obj["id"];
            source.Name = (string)obj["name"];
            source.Description = (string)obj["description"];
            source.Type = (string)obj["type"];
            source.IconUrl = (string)obj["iconUrl"];
            source.Url = (string)obj["url"];

            return source;
        }
        #endregion

    }
}
