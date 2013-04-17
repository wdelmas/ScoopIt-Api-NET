using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Scoopit.Api.Model;

using System.Configuration;

namespace ScoopIt.App.Models
{
    /// <summary>
    /// Topic View Model
    /// </summary>
    public class TopicViewModel
    {

        #region attributes
        public Topic Topic { get; set; }
        public int CuratedCount { get; set; }
        public int Page { get; set; }
        public Double TotalPostCount { get; set; }
        public Double PageCount { get; set; }
        #endregion

        #region constructor
        /// <summary>
        /// Get a {@link Topic} from its <code>id</code> (unique identifier).
        ///
        /// <p>
        /// http://www.scoop.it/dev/api/1/urls#topic
        /// </p>
        /// </summary>
        /// <param name="page"> page number of curated post to retrieve</param>
       public TopicViewModel(int page, Topic topic)
        {
            this.CuratedCount = int.Parse(ConfigurationManager.AppSettings["NumberPostPerPage"]);
            this.Topic = topic;
            this.Page = page;
            this.TotalPostCount = this.Topic.CuratedPostCount;
            this.PageCount = Math.Ceiling(this.TotalPostCount / this.CuratedCount);
        }

        #endregion
    }
}