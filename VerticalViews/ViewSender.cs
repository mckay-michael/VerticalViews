using MediatR;
using Microsoft.AspNetCore.Http;
using VerticalViews.Request;

namespace VerticalViews;

public class ViewSender<TRequest, TViewModel, TMediatorReqeust> : IViewSender<TRequest, TViewModel, TMediatorReqeust>
    where TRequest : IViewRequest<TMediatorReqeust, TViewModel>, new()
    where TMediatorReqeust : IRequest<TViewModel>
{
    private readonly IRequestPipeline<TRequest, TViewModel, TMediatorReqeust> _requestPipeline;

    public ViewSender(IRequestPipeline<TRequest, TViewModel, TMediatorReqeust> requestPipeline)
    {
        _requestPipeline = requestPipeline;
    }

    public Task<IResult> PartailView(TMediatorReqeust request, CancellationToken cancellationToken = default)
    {
        var viewRequest = new TRequest();

        viewRequest.Request = request;          

        return _requestPipeline.Handle(viewRequest, true, cancellationToken);
    }

    public Task<IResult> View(TMediatorReqeust request, CancellationToken cancellationToken = default)
    {
        var viewRequest = new TRequest();

        viewRequest.Request = request;

        return _requestPipeline.Handle(viewRequest, false, cancellationToken);
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