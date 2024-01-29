using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Concurrent;
using VerticalViews.Wrappers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using VerticalViews.Options;

namespace VerticalViews.ViewRenders;

public class ViewStringRender : IViewStringRender
{
    private const string _areaKey = "area";
    private const string _controllerKey = "controller";

    private readonly ICompositeViewEngine _viewEngine;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServiceProvider _serviceProvider;
    private readonly ITempDataDictionary? _tempData;

    private static readonly ConcurrentDictionary<Type, RequestHandlerBase> _requestHandlers = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Mediator"/> class.
    /// </summary>
    /// <param name="serviceProvider">Service provider. Can be a scoped or root provider</param>
    public ViewStringRender(
        ICompositeViewEngine viewEngine,
        IHttpContextAccessor httpContextAccessor,
        IServiceProvider serviceProvider)
    {
        _viewEngine = viewEngine;
        _httpContextAccessor = httpContextAccessor;
        _serviceProvider = serviceProvider;

        var factory = httpContextAccessor.HttpContext?.RequestServices?.GetRequiredService<ITempDataDictionaryFactory>();
        _tempData = factory?.GetTempData(httpContextAccessor.HttpContext);
    }

    public async Task<string> RenderRazorViewToString<TOptions>(string viewName, object model, bool IsPartailView) where TOptions : VerticalViewOptions
    {
        var actionContext = new ActionContext(
            _httpContextAccessor.HttpContext,
            _httpContextAccessor.HttpContext.GetRouteData(),
            new ControllerActionDescriptor());

        var options = (VerticalViewOptions)Activator.CreateInstance<TOptions>();

        actionContext.RouteData.Values.TryAdd(_controllerKey, options.Feature);
        actionContext.RouteData.Values.TryAdd(_areaKey, options.Group);

        var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());

        viewData.Model = model;

        using var writer = new StringWriter();

        var viewResult = _viewEngine.FindView(actionContext, viewName, !IsPartailView);

        if (viewResult.View is null)
        {
            viewResult = _viewEngine.GetView(viewName, viewName, !IsPartailView);
        }

        var viewContext = new ViewContext(
        actionContext,
        viewResult.View,
        viewData,
        _tempData,
        writer,
        new HtmlHelperOptions()
        );

        await viewResult.View.RenderAsync(viewContext);

        return writer.GetStringBuilder().ToString();
    }
}

