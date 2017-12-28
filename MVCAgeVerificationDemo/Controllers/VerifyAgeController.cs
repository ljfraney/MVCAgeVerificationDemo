using MVCAgeVerificationDemo.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MVCAgeVerificationDemo.Controllers
{
    public class VerifyAgeController : Controller
    {
        // Set the minimum age here. This could be moved to the web.config file, or a settings table.
        const int MinimumAge = 21;

        [HttpGet]
        public ActionResult Index()
        {
            var model = new AgeVerificationViewModel { AvailableMonths = GetMonthSelectList() };
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(AgeVerificationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var age = CalculateAge(model.Year.Value, model.Month.Value, model.Day.Value);
                if (age > MinimumAge)
                {
                    // User is old enough. Set a session variable to remember this.
                    Session["AgeVerified"] = true;

                    // If the user went straight to the VerifyAge/Index view, there may not be a RedirectUrl. If they were
                    // sent there via the VerififyAgeAttribute, there will be a RedirectUrl. If there is one, send them
                    // to the view originally requested. Otherwise, send them to Home/Index.
                    if (!string.IsNullOrEmpty(model.RedirectUrl))
                        return Redirect(model.RedirectUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                {
                    // User is not old enough.
                    ModelState.AddModelError("MinimumAge", "Sorry, you do not meet the minimum age to use this site.");

                    // Re-add the AvailableMonths.
                    model.AvailableMonths = GetMonthSelectList();

                    return View(model);
                }
            }

            // Re-add the AvailableMonths.
            model.AvailableMonths = GetMonthSelectList();

            // The model is invalid. 
            return View(model);
        }

        private SelectList GetMonthSelectList()
        {
            var months = new[]
            {
                new { Value = "", Text = "Select a month..." },
                new { Value = "1", Text = "January" },
                new { Value = "2", Text = "February" },
                new { Value = "3", Text = "March" },
                new { Value = "4", Text = "April" },
                new { Value = "5", Text = "May" },
                new { Value = "6", Text = "June" },
                new { Value = "7", Text = "July" },
                new { Value = "8", Text = "August" },
                new { Value = "9", Text = "September" },
                new { Value = "10", Text = "October" },
                new { Value = "11", Text = "November" },
                new { Value = "12", Text = "December" }
            }.ToList();

            return new SelectList(months, "Value", "Text");
        }

        private int CalculateAge(int year, int month, int day)
        {
            var birthdate = new DateTime(year, month, day);
            var now = DateTime.Now;
            int years = now.Year - birthdate.Year;

            if ((birthdate.Month > now.Month) || (birthdate.Month == now.Month && birthdate.Day > now.Day))
                years--;

            return years;
        }
    }
}