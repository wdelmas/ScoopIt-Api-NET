using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoopIt.Api.Service
{
    using System.Configuration;
    using System.IO;
    using System.Web;
    using Hammock;
    using Hammock.Authentication.OAuth;
    using Hammock.Web;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using ScoopIt.Api.OAuth;
    using ScoopIt.Api.OAuth.Model;
    using Scoopit.Api.Model;

    public class ScoopItService
    {
        #region API PATH
        private const String REQUEST_TOPIC = "api/1/topic";
        private const String REQUEST_RESOLVER = "api/1/resolver";
        #endregion

        #region Attribute

        private long _topicId;
        #endregion

        #region Properties
        public RestClient RestClient { get; set; }
        public long TopicId
        {
            get
            {
                if (this._topicId == 0)
                {
                    this._topicId = long.Parse(HttpContext.Current.Session["TopicId"] as string);
                }
                return this._topicId;
            }
            set
            {
                HttpContext.Current.Session["TopicId"] = value;
                this._topicId = value;

            }
        }
        #endregion

        #region Contructor

        public ScoopItService(OAuthCredentials oAuthCredentials)
        {
            this.RestClient = new RestClient()
            {
                Authority = OauthUtil.Base_url_ScoopIt,
                Credentials = oAuthCredentials,
                Method = WebMethod.Get
            };
        }
        #endregion

        #region Helper

        /// <summary>
        /// http://www.scoop.it/dev/api/1/urls#resolver
        /// </summary>
        /// <param name="shortName"></param>
        /// <returns></returns>
        private long ResolveTopicIdFromTopicName(String shortName)
        {
            var request = new RestRequest()
            {
                Path = REQUEST_RESOLVER,
            };

            request.AddParameter("shortName", shortName);
            request.AddParameter("type", "Topic");

            using (var reader = new StringReader(this.RestClient.Request(request).Content))
            {
                JObject obj = JObject.Load(new JsonTextReader(reader));

                try
                {
                    return (long)obj["id"];
                }
                catch (Exception e)
                {
                    throw new Exception("The Topic doesn't exist.Please, enter a valid name");
                }

            }
        }

        ///  <summary>
        ///  Get a {@link Topic} from its <code>id</code> (unique identifier).
        /// 
        ///  <p>
        ///  http://www.scoop.it/dev/api/1/urls#topic
        ///  </p>
        ///  </summary>
        /// <param name="topicName"></param>
        /// <param name="page"> page number of curated post to retrieve</param>
        ///  <param name="curated">curated number of post per page</param>
        ///  <returns>Topic</returns>
        public Topic GetTopic(string topicName, int page = 0, int curated = 20)
        {
            var request = new RestRequest { Path = REQUEST_TOPIC, };
            this.TopicId = this.ResolveTopicIdFromTopicName(topicName);

            request.AddParameter("id", this.TopicId.ToString());

            //optional, default to 0, page number of curated post to retrieve 
            request.AddParameter("page", page.ToString());
            //optional, default to 30, number of curated posts to retrieve for this topic
            request.AddParameter("curated", curated.ToString());

            Topic topicToreturn = new Topic();
            using (var reader = new StringReader(this.RestClient.Request(request).Content))
            {
                JObject obj = JObject.Load(new JsonTextReader(reader));
                JToken topic;
                if (obj.TryGetValue("topic", out topic))
                {
                    var topicObj = topic as JObject;
                    topicToreturn = GetTopicFromJsonObject(topicObj);

                }
            }

            return topicToreturn;
        }

        /// <summary>
        /// Create a Post From a Json object
        /// </summary>
        /// <param name="post"></param>
        /// <param name="curated">A boolean to know if it is a curated model</param>
        /// <returns></returns>
        private Post GetPostFromJsonObject(JObject post, bool curated)
        {
            Post toReturn = null;

            JToken title = null;
            if (post.TryGetValue("title", out title))
            {
                toReturn = new Post();

                //TITLE
                toReturn.Title = (String)title;

                //DATES
                JToken curationDate = null;
                if (post.TryGetValue("curationDate", out curationDate))
                {
                    toReturn.CurationDate = long.Parse(curationDate.ToString());
                }
                JToken publicationDate = null;
                if (post.TryGetValue("publicationDate", out publicationDate))
                {
                    toReturn.PublicationDate = long.Parse(publicationDate.ToString());
                }

                //images
                JToken jsonImageUrls = null;
                String imageUrl = null;
                List<String> imageUrls = new List<string>();
                if (post.TryGetValue("imageUrls", out jsonImageUrls))
                {
                    var imageUrlsArray = jsonImageUrls as JArray;
                    for (int j = 0; j < imageUrlsArray.Count; j++)
                    {
                        if (j == 0 && !curated)
                        {
                            imageUrl = (String)imageUrlsArray[j];
                            toReturn.ImageUrl = imageUrl;
                        }
                        imageUrls.Add((String)imageUrlsArray[j]);
                    }
                }
                toReturn.ImageUrls = imageUrls;
                toReturn.ImageUrl = imageUrl;


                if (curated)
                {
                    JToken imageUrlJson = null;
                    post.TryGetValue("imageUrl", out imageUrlJson);
                    toReturn.ImageUrl = (String)imageUrlJson;
                }

                //url
                JToken url = null;
                post.TryGetValue("url", out url);
                toReturn.Url = (String)url;

                //url in scoop.it
                JToken scoopUrl = null;
                post.TryGetValue("scoopUrl", out scoopUrl);
                toReturn.ScoopUrl = (String)scoopUrl;

                //CONTENT
                JToken content = null;
                post.TryGetValue("content", out content);
                toReturn.Content = (String)content;

                //TITLE
                post.TryGetValue("title", out title);
                toReturn.Title = (string)title;

                //ID
                JToken id = null;
                if (post.TryGetValue("id", out id))
                {
                    var realId = long.Parse(id.ToString());
                    toReturn.Id = realId;
                }

                //TOPIC
                JToken topic = null;
                if (post.TryGetValue("topic", out topic))
                {
                    var topicObject = topic as JObject;
                    toReturn.Topic = GetTopicFromJsonObject(topicObject);
                }

                //SOURCE

                JToken source = null;
                if (post.TryGetValue("source", out source))
                {
                    JObject sourceObject = source as JObject;
                    toReturn.Source = GetSourceFromJsonObject(sourceObject);
                }


            }
            return toReturn;
        }

        /// <summary>
        /// Create a Source from a Json Object
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public Source GetSourceFromJsonObject(JObject source)
        {
            Source toReturn = null;

            JToken topicId = null;
            if (source.TryGetValue("id", out topicId))
            {
                toReturn = new Source();

                var topicIdDouble = long.Parse(topicId.ToString());
                toReturn.Id = topicIdDouble;

                JToken topicName = null;
                source.TryGetValue("name", out topicName);
                if (topicName != null) toReturn.Name = topicName.ToString();

                JToken topicDescription = null;
                source.TryGetValue("description", out topicDescription);
                if (topicDescription != null) toReturn.Description = topicDescription.ToString();

                JToken topicType = null;
                source.TryGetValue("type", out topicType);
                if (topicType != null) toReturn.Type = topicType.ToString();

                JToken topicIconUrl = null;
                source.TryGetValue("iconUrl", out topicIconUrl);
                if (topicIconUrl != null) toReturn.IconUrl = topicIconUrl.ToString();

                JToken topicUrl = null;
                source.TryGetValue("url", out topicUrl);
                if (topicUrl != null) toReturn.Url = topicUrl.ToString();
            }
            return toReturn;
        }

        /// <summary>
        /// Create a Topic From a Json object
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        private Topic GetTopicFromJsonObject(JObject topic)
        {
            Topic toReturn = null;

            JToken topicId = null;
            if (topic.TryGetValue("id", out topicId))
            {
                toReturn = new Topic();

                var topicIdDouble = long.Parse(topicId.ToString());
                toReturn.Id = topicIdDouble;

                JToken topicName = null;
                topic.TryGetValue("name", out topicName);
                toReturn.Name = (String)topicName;

                JToken url = null;
                topic.TryGetValue("url", out url);
                toReturn.Url = (String)url;

                JToken curablePostCount = null;
                topic.TryGetValue("curatedPostCount", out curablePostCount);
                toReturn.CuratedPostCount = (int)curablePostCount;

                JToken topicImgUrl = null;
                topic.TryGetValue("imageUrl", out topicImgUrl);
                toReturn.MediumImageUrl = (String)topicImgUrl;

                //get posts 
                //images
                JToken jsonPosts = null;
                List<Post> posts = new List<Post>();
                if (topic.TryGetValue("curatedPosts", out jsonPosts))
                {
                    JArray postsArray = jsonPosts as JArray;
                    for (int j = 0; j < postsArray.Count; j++)
                    {
                        var onePost = postsArray[j] as JObject;
                        var post = GetPostFromJsonObject(onePost, true);
                        post.Topic = toReturn;
                        posts.Add(post);
                    }
                }
                toReturn.CuratedPosts = posts;
            }
            return toReturn;
        }
        
        #endregion
    }
}
