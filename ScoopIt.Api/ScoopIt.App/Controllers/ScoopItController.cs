using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScoopIt.App.Controllers
{
    using ScoopIt.Api.OAuth.Model;
    using ScoopIt.Api.OAuth.Service;
    using ScoopIt.Api.Service;

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


        public ActionResult Index()
        {
            if (this.AccessTokenModel == null)
            {
                return RedirectToAction("AuthenticateToScoopIt");
            }
            else
            {
                var scoopItService = new ScoopItService(this.AccessTokenModel);
                return Content(scoopItService.GetTopic("1").Content);
            }
            
        }

        public ActionResult AuthenticateToScoopIt()
        {
            var redirectTo = AuthorizeScoopItMode.AuthenticateToScoopIt();
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
            var accessTokenModel = AuthorizeScoopItMode.CallBackFromScoopIt();
            if (accessTokenModel != null)
            {
                this.AccessTokenModel = accessTokenModel;
            }

            return RedirectToAction("Index");
        }

    }
}
