namespace CookTheWeek.Web.Controllers
{    
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;


    [Authorize]
    public abstract class BaseController : Controller
    {
        
        public BaseController()
        {
            
        }    
       
    }
}
