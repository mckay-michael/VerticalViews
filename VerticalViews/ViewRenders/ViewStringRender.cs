using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;

namespace VerticalViews.ViewRenders;

public class ViewStringRender : IViewStringRender
{
    private const string _areaKey = "area";
    private const string _controllerKey = "controller";

    private readonly ICompositeViewEngine _viewEngine;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITempDataDictionary? _tempData;


    public ViewStringRender(
        ICompositeViewEngine viewEngine,
        IHttpContextAccessor httpContextAccessor)
    {
        _viewEngine = viewEngine;
        _httpContextAccessor = httpContextAccessor;

        var factory = httpContextAccessor.HttpContext?.RequestServices?.GetRequiredService<ITempDataDictionaryFactory>();
        _tempData = factory?.GetTempData(httpContextAccessor.HttpContext);
    }

    public async Task<string> RenderRazorViewToString(object model, BaseRequest options, bool isPartailView)
    {
        var actionContext = new ActionContext(
            _httpContextAccessor.HttpContext,
            _httpContextAccessor.HttpContext.GetRouteData(),
            new ControllerActionDescriptor());

        actionContext.RouteData.Values.TryAdd(_controllerKey, options.Feature);
        actionContext.RouteData.Values.TryAdd(_areaKey, options.Group);

        var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());

        viewData.Model = model;

        using var writer = new StringWriter();

        var viewResult = _viewEngine.FindView(actionContext, options.ViewName, !isPartailView);

        if (viewResult.View is null)
        {
            viewResult = _viewEngine.GetView(options.ViewName, options.ViewName, !isPartailView);
        }

        var viewContext = new ViewContext(
            actionContext,
            viewResult.View,
            viewData,
            _tempData,
            writer,
            new HtmlHelperOptions());

        await viewResult.View.RenderAsync(viewContext);

        return writer.GetStringBuilder().ToString();
    }
}

