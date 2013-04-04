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
    using ScoopIt.Api.Service.Mode;
    using ScoopIt.App.Models;


    public class ScoopItController : Controller
    {
        //
        // GET: /ScoopIt/
        private AccessTokenModel _accessTokenModel;

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


        public ActionResult Index(int page = 0)
        {
            if (this.AccessTokenModel == null)
            {
                return RedirectToAction("AuthenticateToScoopIt");
            }
            else
            {
                var topicPage = new TopicViewModel();
                topicPage.Topic = new AuthorizeScoopItAccess(this.AccessTokenModel).Api.GetTopic(ConfigurationManager.AppSettings["TOPIC_Name"], topicPage.Page);
                topicPage.InitModel(page);

                return this.View(topicPage);
            }
            
        }

        public ActionResult AnonymousIndex(int page = 0)
        {
            var topicPage = new TopicViewModel();
            topicPage.Topic = new AnonymousScoopItAccess().Api.GetTopic(ConfigurationManager.AppSettings["TOPIC_Name"], topicPage.Page);
            topicPage.InitModel(page);

            return this.View("Index",topicPage);
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
            }

            return RedirectToAction("Index");
        }

    }
}
