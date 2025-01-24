using Microsoft.AspNetCore.Mvc;

namespace ReviewWebsite.Controllers
{
    public static class ControllerExtensions
    {

        public static IActionResult ResolveView(this Controller controller, string viewName, object? model)
        {
            if (controller.Request.IsAjaxRequest())
            {
                return controller.PartialView($"_{viewName}", model);
            }

            return controller.View(viewName, model);
        }

        private static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
