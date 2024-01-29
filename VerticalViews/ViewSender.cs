using System.Collections.Concurrent;
using System.Threading;
using Microsoft.AspNetCore.Http;
using VerticalViews.Options;
using VerticalViews.Results;
using VerticalViews.ViewRenders;
using VerticalViews.Wrappers;

namespace VerticalViews;

public class ViewSender : IViewSender
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IViewStringRender _viewRender;

    private static readonly ConcurrentDictionary<Type, RequestHandlerBase> _requestHandlers = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Mediator"/> class.
    /// </summary>
    /// <param name="serviceProvider">Service provider. Can be a scoped or root provider</param>
    public ViewSender(
        IViewStringRender viewRender,
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _viewRender = viewRender;
    }

    public async Task<IResult> View<TViewModel, TOptions>(string viewName, IViewRequest<TViewModel> request, CancellationToken cancellationToken = default)
        where TViewModel : class
        where TOptions : VerticalViewOptions =>
            await SendRequest<TViewModel, TOptions>(viewName, request, cancellationToken, false);

    public async Task<IResult> View<TViewModel, TOptions>(string viewName)
        where TViewModel : class
        where TOptions : VerticalViewOptions
    {
        var viewModel = Activator.CreateInstance(typeof(TViewModel));
        return await CreateHtmlResult<TOptions>(viewName, viewModel, false);
    }

    public async Task<IResult> View<TOptions>(string viewName)
        where TOptions : VerticalViewOptions =>
        await CreateHtmlResult<TOptions>(viewName, null, false);

    public async Task<IResult> PartailView<TViewModel, TOptions>(string viewName, IViewRequest<TViewModel> request, CancellationToken cancellationToken = default)
        where TViewModel : class
        where TOptions : VerticalViewOptions =>
            await SendRequest<TViewModel,TOptions>(viewName, request, cancellationToken, true);

    public async Task<IResult> PartailView<TViewModel, TOptions>(string viewName)
        where TViewModel : class
        where TOptions : VerticalViewOptions
    {
        var viewModel = Activator.CreateInstance(typeof(TViewModel));
        return await CreateHtmlResult<TOptions>(viewName, viewModel, true);
    }

    public async Task<IResult> PartailView<TOptions>(string viewName)
        where TOptions : VerticalViewOptions =>
            await CreateHtmlResult<TOptions>(viewName, null, true);

    private async Task<HtmlResult> CreateHtmlResult<TOptions>(string viewName, object model, bool IsPartailView)
        where TOptions : VerticalViewOptions =>
            new HtmlResult(await _viewRender.RenderRazorViewToString<TOptions>(viewName, model, IsPartailView));


    private async Task<IResult> SendRequest<TViewModel, TOptions>(
        string viewName,
        IViewRequest<TViewModel> request,
        CancellationToken cancellationToken,
            bool isPartailView) where TViewModel : class
            where TOptions : VerticalViewOptions
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var handler = (RequestHandlerWrapper<TViewModel>)_requestHandlers.GetOrAdd(request.GetType(), static requestType =>
        {
            var wrapperType = typeof(RequestHandlerWrapperImpl<,>).MakeGenericType(requestType, typeof(TViewModel));
            var wrapper = Activator.CreateInstance(wrapperType) ?? throw new InvalidOperationException($"Could not create wrapper type for {requestType}");
            return (RequestHandlerBase)wrapper;
        });

        var viewModel = await handler.Handle(request, _serviceProvider, cancellationToken);
        return await CreateHtmlResult<TOptions>(viewName, viewModel, isPartailView);
    }
}