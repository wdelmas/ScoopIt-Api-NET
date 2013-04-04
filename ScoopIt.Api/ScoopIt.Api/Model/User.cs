using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Scoopit.Api.Model
{
    /// <summary>
    /// olds data related to a user.
    //
    // <p>
    // http://www.scoop.it/dev/api/1/types#user
    // </p>
    // 
    //   /// </summary>
    public class User
    {

        #region attributes

        /** user id */
        public long Id { get; set; }
        /** user name */
        public String Name { get; set; }
        /** user shortName (used in website urls) */
        public String ShortName { get; set; }
        /** user bio */
        public String Bio { get; set; }
        /** user avatar url (16x16) */
        public String SmallAvatarUrl { get; set; }
        /** user avatar url (48x48) */
        public String MediumAvatarUrl { get; set; }
        /** user avatar url (192x192) */
        public String LargeAvatarUrl { get; set; }

        /** the list of topic this user is the curator */
        private ICollection<Topic> CuratedTopics { get; set; }
        /** the list of topics followed by this user */
        private ICollection<Topic> FollowedTopics { get; set; }
        #endregion

        #region constructor

        public User()
        {
        }
        #endregion

        #region methodes

        public static User GetFromJSON(JObject obj)
        {
            var user = new User();
            user.Id = (long)obj["id"];
            user.Name = (string)obj["name"];
            user.ShortName = (string)obj["shortName"];
            user.Bio = (string)obj["bio"];
            user.SmallAvatarUrl = (string)obj["smallAvatarUrl"];
            user.MediumAvatarUrl = (string)obj["mediumAvatarUrl"];
            user.LargeAvatarUrl = (string)obj["largeAvatarUrl"];



            user.CuratedTopics = Topic.CreateTopicArrayFromJSONArray((JArray)obj["curatedTopics"]);
            user.FollowedTopics = Topic.CreateTopicArrayFromJSONArray((JArray)obj["followedTopics"]);
            return user;
        }

        #endregion
    }
}
