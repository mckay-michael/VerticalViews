using Microsoft.AspNetCore.Http;
using VerticalViews.Request;

namespace VerticalViews;

public class ViewSender<TRequest, TViewModel> : IViewSender<TRequest, TViewModel>
    where TRequest : IViewRequest<TViewModel>
{
    private readonly IRequestPipeline<TRequest, TViewModel> _requestPipeline;

    public ViewSender(
        IRequestPipeline<TRequest, TViewModel> requestPipeline)
    {
        _requestPipeline = requestPipeline;
    }

    public Task<IResult> View(IViewRequest<TViewModel> request, CancellationToken cancellationToken = default)
    {
        return _requestPipeline.Handle(request, false, cancellationToken);
    }

    public Task<IResult> PartailView(IViewRequest<TViewModel> request, CancellationToken cancellationToken = default)
    {
        return _requestPipeline.Handle(request, true, cancellationToken);
    }
}

public class ViewSender<TRequest> : IViewSender<TRequest>
    where TRequest : ViewRequest, new()
{
    private readonly IRequestPipeline<TRequest> _requestPipeline;

    public ViewSender(IRequestPipeline<TRequest> requestPipeline)
    {
        _requestPipeline = requestPipeline;
    }

    public Task<IResult> View(CancellationToken cancellationToken = default)
    {
        return _requestPipeline.Handle(new TRequest(), false, cancellationToken);
    }

    public Task<IResult> PartailView(CancellationToken cancellationToken = default)
    {
        return _requestPipeline.Handle(new TRequest(), true, cancellationToken);
    }
}