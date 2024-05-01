using System.Web.Mvc;

namespace JobsFinder_Main.Filters
{
    public class RecruiterAuthorization : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.IsInRole("Recruiter") == false)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}
