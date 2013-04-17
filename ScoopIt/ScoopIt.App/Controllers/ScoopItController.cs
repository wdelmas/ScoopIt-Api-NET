using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScoopIt.App.Controllers
{
    using System.Configuration;
    using ScoopIt.Api.OAuth;
    using ScoopIt.Api.OAuth.Model;
    using ScoopIt.Api.Service;
    using ScoopIt.Api.Service.Model;
    using ScoopIt.Api.Service.Model;
    using ScoopIt.App.Models;


    public class ScoopItController : Controller
    {
        #region Attributes
        private AccessTokenModel _accessTokenModel;
        #endregion

        #region Properties
        public AccessTokenModel AccessTokenModel
        {
            get
            {
                if (this.Session["AccessTokenModel"] != null)
                {
                    _accessTokenModel = this.Session["AccessTokenModel"] as AccessTokenModel;
                }
                return _accessTokenModel;
            }
            set
            {
                this.Session["AccessTokenModel"] = value;
                _accessTokenModel = value;
            }
        }
        #endregion

        #region Action

        /// <summary>
        /// Authorize mode
        /// http://www.scoop.it/dev/api/1/intro
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(int page = 0)
        {
            if (this.AccessTokenModel == null)
            {
                return RedirectToAction("AuthenticateToScoopIt");
            }
            else
            {
                var authorizeaccess = new AuthorizeAccess(this.AccessTokenModel);
                var topic = new ScoopItService(authorizeaccess).GetTopic(
                                ConfigurationManager.AppSettings["TOPIC_Name"], page);

                //ViewModel
                var topicPage = new TopicViewModel(page, topic);

                return this.View(topicPage);
            }

        }

        /// <summary>
        /// Anonymous Mode
        /// http://www.scoop.it/dev/api/1/intro
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult AnonymousIndex(int page = 0)
        {
            var topic = new ScoopItService().GetTopic(
                               ConfigurationManager.AppSettings["TOPIC_Name"], page);

            //ViewModel
            var topicPage = new TopicViewModel(page, topic);

            return this.View("Index", topicPage);
        }

        public ActionResult AuthenticateToScoopIt()
        {
            var redirectTo = OauthAuthorizeService.AuthenticateToScoopIt();
            if (!string.IsNullOrEmpty(redirectTo.Callback))
            {
                Response.Redirect(redirectTo.Callback);
                return null;
            }
            else
            {
                return Content("Error");
            }
        }

        public ActionResult CallbackFromScoopIt()
        {
            var accessTokenModel = OauthAuthorizeService.CallBackFromScoopIt();
            if (accessTokenModel != null)
            {
                this.AccessTokenModel = accessTokenModel;
                return RedirectToAction("Index");
            }
            else
            {
                return Content("Error");
            }


        }

        #endregion
    }
}
