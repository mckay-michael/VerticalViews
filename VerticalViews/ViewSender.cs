using System.Collections.Concurrent;
using MediatR;
using Microsoft.AspNetCore.Http;
using VerticalViews.Strategies;

namespace VerticalViews;

public class ViewSender<TRequest, TViewModel> : IViewSender<TRequest, TViewModel>
    where TRequest : BaseRequest
{
    private readonly IViewRequestStrategy<TRequest, TViewModel> _requestStrategy;

    public ViewSender(IViewRequestStrategy<TRequest, TViewModel> requestStrategy)
    {
        _requestStrategy = requestStrategy;
    }

    public Task<IResult> View(BaseRequest request, CancellationToken cancellationToken = default)
    {
        return _requestStrategy.Execute(request, false, cancellationToken);
    }

    public Task<IResult> PartailView(BaseRequest request, CancellationToken cancellationToken = default)
    {
        return _requestStrategy.Execute(request, true, cancellationToken);
    }
}