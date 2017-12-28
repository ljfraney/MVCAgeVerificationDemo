# MVC Age Verification Demo

Some sites on the Internet require that a user be at least a certain age in order to use the site, or certain pages. This application demonstrates one way to facilitate this requirement. The goal was to implement the feature, and prevent the user from being able to bypass the requirement by turning off JavaScript, or navigating directly to a page within the site.

Since the site is an ASP.NET MVC 5 application that will be running on IIS, I chose to implement the feature by using a custom AuthorizeAttribute, redirecting the user to an age verification form, and storing the result in a session variable.

## Anatomy 101

- `VerifyAgeAttribute.cs` contains the custom AuthorizeAttribute. The attribute can be placed on any controller or action in the application.
- `Controllers/HomeController.cs` contains the following actions:
  - `Index (GET)`: This action is decorated with the `VerifyAge` attribute. If the user is authorized (i.e. they have verified their age and they meet the age requirement), they are sent to the Home/Index view.
  - `Page2 (GET)`: This action is NOT decorated with the `VerifyAge` attribute, and the user could bypass the requirement by navigating directly to Home/Page2.
  - `ResetAgeVerification (POST)`: This action removes the "AgeVerified" session variable and redirects the user back to Home/Index, triggering the 'VerifyAge' filter.
- `Controllers/VerifyAgeController.cs` contains the following actions:
  - `Index (GET)`: This action loads the VerifyAge/Index view. This is the view that allows the user to input their birth date.
  - `Index (POST)`: This action accepts the user input from the VerifyAge/Index view, validates it, stores a positive result in a session variable, and redirects the user to the page originally requested. That page was added as a querystring parameter by the VerifyAge filter. This allows the user to bookmark any page, and the requirement will still be enforced.

## How it all works

The user typically enters the site via the Home/Index view. Before the action is run, the VerifyAge attribute is triggered. The system checks for the existance of a session variable named "AgeRequirementMet". If this exists, and the value is `true`, they are sent on to Home/Index. If it does not exist, or the value is `false`, they are redirected to VerifyAge/Index with a querystring parameter named redirectUrl that indicates the resource originally requested. In the VerifyAge/Index view, the user enters their birth day/month/year and clicks the submit button. If the input was valid, and the user meets the age requirement, they are sent to the url originally requested. If the user's session cookie is expired or removed, they will have to fulfill the requirement again.