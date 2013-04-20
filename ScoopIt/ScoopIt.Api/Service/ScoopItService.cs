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
    using ScoopIt.Api.Service.Model;
    using Scoopit.Api.Model;

    public class ScoopItService
    {

        #region API PATH
        private const String REQUEST_TOPIC = "api/1/topic";
        private const String REQUEST_POST = "api/1/post";
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
                    this._topicId = long.Parse(HttpContext.Current.Application["TopicId"] as string);
                }
                return this._topicId;
            }
            set
            {
                HttpContext.Current.Application["TopicId"] = value;
                this._topicId = value;

            }
        }
        #endregion

        #region Contructor

        /// <summary>
        /// Default Anonymous mode
        /// </summary>
        public ScoopItService()
        {
            var anonymousScoopItAccess = new AnonymousAccess();
            this.InitScoopItService(anonymousScoopItAccess.AccessCredentials);
        }

        /// <summary>
        /// Authorize Mode
        /// </summary>
        /// <param name="authorizeAccess"></param>
        public ScoopItService(AuthorizeAccess authorizeAccess)
        {
            this.InitScoopItService(authorizeAccess.AccessCredentials);
        }

        public void InitScoopItService(OAuthCredentials oAuthCredentials)
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
        public Topic GetTopic(string topicName, int page = 0, int curated = 10)
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


        public Post GetPost(long postId)
        {
            var request = new RestRequest { Path = REQUEST_POST, };

            request.AddParameter("id", postId.ToString());
            request.AddParameter("ncomments", "100");

            Post postToreturn = new Post();
            using (var reader = new StringReader(this.RestClient.Request(request).Content))
            {
                JObject obj = JObject.Load(new JsonTextReader(reader));


                postToreturn = this.GetPostFromJsonObject(obj, false);


            }

            return postToreturn;
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

            JToken topicId = null;
            if (post.TryGetValue("id", out topicId))
            {
                toReturn = Post.GetFromJSON(post);


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
                toReturn = Topic.GetFromJSON(topic);


            }
            return toReturn;
        }

        #endregion
    }
}
