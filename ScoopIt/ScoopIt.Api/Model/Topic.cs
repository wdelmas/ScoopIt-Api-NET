using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Scoopit.Api.Model
{
    /// <summary>
    /// Holds data related to a topic.
    //
    // <p>
    // http://www.scoop.it/dev/api/1/types#topic
    // </p>
    /// </summary>
    public class Topic
    {
        #region attrbutes

        /** topic id */
        public long Id { get; set; }
        /** topic image url (16x16) */
        public String SmallImageUrl { get; set; }
        /** topic image url (48x48) */
        public String MediumImageUrl { get; set; }
        /** topic image url (192x192) */
        public String LargeImageUrl { get; set; }
        /** URL of the topic's background image, if set */
        public String BackgroundImage { get; set; }
        /** CSS background-repeat property to be applied to the topic's background image */
        public String BackgroundRepeat { get; set; }
        /** topic's background color expressed as a six character hexa code (eg. FFCC00) */
        public String BackgroundColor { get; set; }
        /** topic description */
        public String Description { get; set; }
        /** topic name */
        public String Name { get; set; }
        /** topic short name (used in website urls) */
        public String Shortname { get; set; }
        /** topic url */
        public String Url { get; set; }
        /** true if topic is curated by current user */
        public Boolean IsCurator { get; set; }
        /** number of curable posts in this topic */
        public int CurablePostCount { get; set; }
        /** number of curated posts in this topic */
        public int CuratedPostCount { get; set; }
        /** number of unread posts */
        public int UnreadPostCount { get; set; }
        /** creator of the topic */
        public User Creator { get; set; }
        /** post pinned at the top of the topic */
        public Post PinnedPost { get; set; }
        /** an array of posts to curate on this topic */
        public List<Post> CurablePosts { get; set; }
        /** an array of curated posts on this topic */
        public List<Post> CuratedPosts { get; set; }
        #endregion

        #region methodes

        public static ICollection<Topic> CreateTopicArrayFromJSONArray(JArray array)
        {
            if (array == null)
            {
                return null;
            }


            var topics = new List<Topic>();
            for (int i = 0; i < array.Count(); i++)
            {
                JObject jsonTopic;
                jsonTopic = (JObject)array[i];
                topics.Add(Topic.GetFromJSON(jsonTopic));
            }
            return topics;
        }

        public static Topic GetFromJSON(JObject obj)
        {
            if (obj == null)
            {
                return null;
            }


            var topic = new Topic();
            topic.Id = (long)obj["id"];
            topic.SmallImageUrl = (string)obj["smallImageUrl"];
            topic.Description = (string)obj["description"];
            topic.MediumImageUrl = (string)obj["mediumImageUrl"];
            topic.LargeImageUrl = (string)obj["largeImageUrl"];
            topic.BackgroundImage = (string)obj["backgroundImage"];
            topic.BackgroundRepeat = (string)obj["backgroundRepeat"];
            topic.BackgroundColor = (string)obj["backgroundColor"];
            topic.Name = (string)obj["name"];
            topic.Shortname = (string)obj["shortname"];
            topic.Url = (string)obj["url"];
            topic.IsCurator = (bool)obj["isCurator"];
            //topic.CurablePostCount = (int)obj["curablePostCount"];
            topic.CuratedPostCount = (int)obj["curatedPostCount"];
            //topic.UnreadPostCount = (int)obj["unreadPostCount"];

            topic.Creator = User.GetFromJSON((JObject)obj["creator"]);

            topic.PinnedPost = new Post();
            topic.PinnedPost = Post.GetFromJSON((JObject)obj["pinnedPost"]);
            topic.CurablePosts = Post.CreatePostArrayFromJsonArray((JArray)obj["curablePosts"]);

            topic.CuratedPosts = Post.CreatePostArrayFromJsonArray((JArray)obj["curatedPosts"]);

            return topic;
        }

        #endregion


    }
}
