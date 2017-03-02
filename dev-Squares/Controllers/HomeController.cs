using dev_Squares.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dev_Squares.Controllers
{
    public class HomeController: Controller
    {

        public const string UserSessionID = "UserSessionId";

        public HomeController()
        {
        
        }

        public ActionResult Index()
        {
            if (Request.Cookies[UserSessionID] == null)
            {
                var user = new HttpCookie(UserSessionID)
                {
                    Expires = DateTime.Now.AddMonths(1),
                    Value = Guid.NewGuid().ToString()
                };

                Response.SetCookie(user);
            }

            return View();
        }
    }
}
