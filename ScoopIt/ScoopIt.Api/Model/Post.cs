using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Scoopit.Api.Model
{
    /// <summary>
    /// /**
    /// Holds data related to a post
    /// <p>
    /// http://www.scoop.it/dev/api/1/types#post
    /// </p>
    /// <summary>
    /// </summary>
    /// </summary>
    public class Post
    {

        #region attributes
        /** the id of the post */
        public long Id { get; set; }
        /** post content in plain text */
        public String Content { get; set; }
        /** post content in HTML */
        public String HtmlFragment { get; set; }
        /** additional embedded HTML content if applicable (eg: embedded videos) */
        public String HtmlContent { get; set; }
        /** The post title */
        public String Title { get; set; }
        /** The number of times this post was thanked */
        public int ThanksCount { get; set; }
        /** The number reactions on this post */
        public int ReactionsCount { get; set; }
        /** The source of the post */
        public Source Source { get; set; }
        /** if source is a twitter search, the twitter user who wrote the original tweet */
        public String TwitterAuthor { get; set; }
        /** original url of the post */
        public String Url { get; set; }
        /** url of the post on the scoop platform */
        public String ScoopUrl { get; set; }
        /**
         * - url of the image chosen by the curator, referred below as "post image" (max width: 100px)
         */
        public String SmallImageUrl { get; set; }
        /**
         * url of the image chosen by the curator, referred below as "post image" (max width: 200px)
         */
        public String MediumImageUrl { get; set; }
        /**
         * url of the image chosen by the curator, referred below as "post image" (max width: 400px)
         */
        public String ImageUrl { get; set; }
        /**
         * url of the image chosen by the curator, referred below as "post image" (max width: 1024px)
         */
        public String LargeImageUrl { get; set; }
        /** width in pixel of the original post image */
        public int ImageWidth { get; set; }
        /** height in pixel of the original post image */
        public int ImageHeight { get; set; }
        /** size of the post image in the topic view */
        public int ImageSize { get; set; }
        /**
         * position of the post image in the topic view: "left" | "center" | "right"
         */
        public String ImagePosition { get; set; }
        /** array of urls of image selector in curation mode */
        public ICollection<String> ImageUrls { get; set; }
        /** array of tags */
        public ICollection<String> Tags { get; set; }
        /** the number of comments for this post */
        public int CommentsCount { get; set; }
        /** true if the post is a user suggestion */
        public Boolean IsUserSuggestion { get; set; }
        /** number of time this post has been viewed */
        public long PageViews { get; set; }
        /** true if the description of this post has been manually edited by the curator */
        public Boolean Edited { get; set; }
        /** the publication date of the original article */
        public long PublicationDate { get; set; }
        /**
         * the curation date of the post (aka the publication date on Scoop.it)
         */
        public long CurationDate { get; set; }
        /** the topic this post is belonging to */
        public Topic Topic { get; set; }

        #endregion

        #region constructor
        public static Post GetFromJSON(JObject obj)
        {
            if (obj == null)
            {
                return null;
            }

            var post = new Post();
            post.Id = (int)obj["id"];
            post.Content = (string)obj["content"];
            post.HtmlFragment = (string)obj["htmlFragment"];
            post.HtmlContent = (string)obj["htmlContent"];
            post.Title = (string)obj["title"];
            post.ThanksCount = (int)obj["thanksCount"];
            post.ReactionsCount = (int)obj["reactionsCount"];
            post.Source = Source.GetFromJSON((JObject)obj["source"]);
            post.TwitterAuthor = (string)obj["twitterAuthor"];
            post.Url = (string)obj["url"];
            post.ScoopUrl = (string)obj["scoopUrl"];
            post.SmallImageUrl = (string)obj["smallImageUrl"];
            post.MediumImageUrl = (string)obj["mediumImageUrl"];
            post.ImageUrl = (string)obj["imageUrl"];
            post.LargeImageUrl = (string)obj["largeImageUrl"];
            post.ImageWidth = (int)obj["imageWidth"];
            post.ImageHeight = (int)obj["imageHeight"];
            post.ImageSize = (int)obj["imageSize"];
            post.ImagePosition = (string)obj["imagePosition"];
            post.ImageUrls = new List<String>();
            var array = (JArray)obj["imageUrls"];
            if (array != null)
            {
                for (int i = 0; i < array.Count(); i++)
                {
                    post.ImageUrls.Add((string)array[i]);
                }
            }
            post.Tags = new List<String>();
            array = (JArray)obj["tags"];
            if (array != null)
            {
                for (int i = 0; i < array.Count(); i++)
                {
                    post.Tags.Add((string)array[i]);
                }
            }
            post.CommentsCount = (int)obj["commentsCount"];
            post.IsUserSuggestion = (Boolean)obj["isUserSuggestion"];
            post.PageViews = (long)obj["pageViews"];
            post.Edited = (Boolean)obj["edited"];
            post.PublicationDate = (long)obj["publicationDate"];



            post.CurationDate = (long)obj["curationDate"];


            post.Topic = Topic.GetFromJSON((JObject)obj["topic"]);

            return post;
        }
        #endregion

        #region methodes
        public static List<Post> CreatePostArrayFromJsonArray(JArray array)
        {
            if (array == null)
            {
                return null;
            }

            var posts = new List<Post>();
            for (int i = 0; i < array.Count(); i++)
            {
                JObject jsonPost;
                jsonPost = (JObject)array[i];

                posts.Add(Post.GetFromJSON(jsonPost));
            }
            return posts;
        }
        public override bool Equals(object obj)
        {
            if (obj is Post)
            {
                return ((Post)obj).Id.Equals(this.Id);
            }
            return false;
        }


        public String GenerateTweetText()
        {
            String suffix = " | @scoopit http://bit.ly/...";
            int textLengthMax = 140 - suffix.Count();
            String editTitle = this.Title;
            if (editTitle.Count() > textLengthMax)
            {
                editTitle = editTitle.Substring(0, textLengthMax - 1);
            }
            return editTitle + suffix;
        }

        /// <summary>
        /// Methods to convert Unix time stamp to DateTime
        /// </summary>
        /// <param name="_UnixTimeStamp">Unix time stamp to convert</param>
        /// <returns>Return DateTime</returns>
        public DateTime UnixTimestampToDateTime()
        {
            return (new DateTime(1970, 1, 1, 0, 0, 0)).AddSeconds(this.CurationDate / 1000);
        }

        #endregion
    }
}
