using System.Web.Mvc;
using WindowsAzure.ResourceProviderDataLayer.Entities;

namespace ResourceProvidR.Controllers
{
    public class SsoController : Controller
    {
        public ActionResult Index()
        {
            // TODO: aiuthorize here

            string token = Request.QueryString["token"];
            string cloudServiceName = Request.QueryString["cloudservicename"];
            string subscriptionId = Request.QueryString["subid"];
            string resourceName = Request.QueryString["resourceName"];
            string resourceType = Request.QueryString["resourceType"];
            string timeStamp = Request.QueryString["timestamp"];
               
            // Locate the resource entity here
            ResourceEntity resourceEntity = WebApiApplication.Storage.ResourceStorage.GetResource(subscriptionId, cloudServiceName, resourceType, resourceName);
            if (resourceEntity != null)
            {
                ViewBag.ResourceEntity = resourceEntity;

                return View();
            }
            else
            {
                // TODO - 404
                return View();
            }
        }
    }
}
