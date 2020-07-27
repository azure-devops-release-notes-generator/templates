using Microsoft.AspNetCore.Mvc;

namespace Templates.Controllers
{
    public class TemplatesController: Controller
    {
        [HttpPost]
        public IActionResult Index(string id, [FromBody] dynamic model)
        {
            var tenantId = "b9ec5788-c06e-45cb-9c64-22606a0e6573";
            return View($"{tenantId}/{id}", model);
        }
    }
}
