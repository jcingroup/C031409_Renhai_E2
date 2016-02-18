
using System.Web.Mvc;


namespace DotWeb.Controllers
{
    public class LoginManageController : Controller
    {
        public RedirectResult Index()
        {
            return Redirect("~/Sys_Base/MNGLogin");
        }
    }
}
