namespace CookTheWeek.Web.Controllers
{    
    using Ganss.Xss;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;


    [Authorize]
    public abstract class BaseController : Controller
    {
        
        public BaseController()
        {
            
        }    
       
    }
}
