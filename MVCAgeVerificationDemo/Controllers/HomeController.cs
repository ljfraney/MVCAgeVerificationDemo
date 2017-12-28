using System.Web.Mvc;

namespace MVCAgeVerificationDemo.Controllers
{
    public class HomeController : Controller
    {        
        // The VerifyAge attribute can be placed on an action, or a controller if you want to apply
        // it to all actions in that controller.
        [VerifyAge]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Page2()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetAgeVerification()
        {
            // Remove the session variable.
            if (Session["AgeVerified"] != null)
                Session.Remove("AgeVerified");

            // Redirect the user back to Home/Index. This will run the VerifyAge attribute code which
            // will send them to the VerifyAge/Index view.
            return RedirectToAction("Index");
        }


    }
}