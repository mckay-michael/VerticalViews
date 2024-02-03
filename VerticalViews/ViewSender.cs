using System.Collections.Concurrent;
using MediatR;
using Microsoft.AspNetCore.Http;
using VerticalViews.Wrappers;

namespace VerticalViews;

public class ViewSender : IViewSender
{
    private readonly IServiceProvider _serviceProvider;

    private static readonly ConcurrentDictionary<Type, RequestHandlerBase> _requestHandlers = new();

    public ViewSender(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<IResult> View<TViewModel>(IViewRequest<TViewModel> request, CancellationToken cancellationToken = default)
    {
        return SendRequest(request, false, cancellationToken);
    }

    public Task<IResult> PartailView<TViewModel>(IViewRequest<TViewModel> request, CancellationToken cancellationToken = default)
    {
        return SendRequest(request, false, cancellationToken);
    }

    public Task<IResult> View(ViewRequest request, CancellationToken cancellationToken = default)
    {
        return SendRequest(request, false, cancellationToken);
    }

    public Task<IResult> PartailView(ViewRequest request, CancellationToken cancellationToken = default)
    {
        return SendRequest(request, true, cancellationToken);
    }

    private Task<IResult> SendRequest<TViewModel>(IViewRequest<TViewModel> request, bool isPartailView, CancellationToken cancellationToken = default)
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

        return handler.Handle(request, isPartailView, _serviceProvider, cancellationToken);
    }

    private Task<IResult> SendRequest(ViewRequest request, bool isPartailView, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var handler = (RequestClassHandlerWrapper)_requestHandlers.GetOrAdd(request.GetType(), static requestType =>
        {
            var wrapperType = typeof(RequestClassHandlerWrapperImpl<>).MakeGenericType(requestType);
            var wrapper = Activator.CreateInstance(wrapperType) ?? throw new InvalidOperationException($"Could not create wrapper type for {requestType}");
            return (RequestHandlerBase)wrapper;
        });

        return handler.Handle(request, isPartailView, _serviceProvider, cancellationToken);
    }
}