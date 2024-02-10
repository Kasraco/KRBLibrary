using System.IO;
using System.Web.Mvc;


namespace KRB.Utilities.Infrastructure;
public class ViewConvertor : Controller
{
    public string RenderRazorViewToString(string viewName, ControllerContext context, object model)
    {
        ViewData.Model = model;
        using (var sw = new StringWriter())
        {
            ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
            var viewContext = new ViewContext(context, viewResult.View, ViewData, TempData, sw);
            viewResult.View.Render(viewContext, sw);
            viewResult.ViewEngine.ReleaseView(context, viewResult.View);
            return sw.GetStringBuilder().ToString();
        }
    }
}