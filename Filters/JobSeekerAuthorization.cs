using System.Web.Mvc;

namespace JobsFinder_Main.Filters
{
    public class JobSeekerAuthorization : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.IsInRole("JobSeeker") == false) 
            { 
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}
