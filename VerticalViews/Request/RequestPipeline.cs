using MediatR;
using Microsoft.AspNetCore.Http;
using VerticalViews.Response;
using VerticalViews.Results;
using VerticalViews.ViewRenders;

namespace VerticalViews.Request;

public class RequestPipeline<TRequest, TViewModel, TMediatorReqeust> : IRequestPipeline<TRequest, TViewModel, TMediatorReqeust>
    where TRequest : IViewRequest<TMediatorReqeust, TViewModel>
    where TMediatorReqeust : IRequest<TViewModel>
{
    private readonly IEnumerable<IRequestBehavior<TRequest, TViewModel>> _requestBehaviors;
    private readonly IResponsePipeline<TRequest, TViewModel, TMediatorReqeust> _responsePipeline;

    public RequestPipeline(
        IEnumerable<IRequestBehavior<TRequest, TViewModel>> requestBehaviors,
        IResponsePipeline<TRequest, TViewModel, TMediatorReqeust> responsePipeline)
    {
        _requestBehaviors = requestBehaviors;
        _responsePipeline = responsePipeline;
    }

    public Task<IResult> Handle(TRequest request, bool isPartailView, CancellationToken cancellationToken)
    {
        Task<IResult> Handler() =>
            _responsePipeline.Handle(request, isPartailView, cancellationToken);

        return _requestBehaviors
           .Reverse()
           .Aggregate((RequestHandlerDelegate)Handler,
               (next, behavior) => () => behavior.Handle(request, next, cancellationToken))();
    }
}


public class RequestPipeline<TRequest> : IRequestPipeline<TRequest>
    where TRequest : ViewRequest
{
    private readonly IEnumerable<IRequestBehavior<TRequest>> _requestBehaviors;
    private readonly IViewStringRender _viewRender;

    public RequestPipeline(
        IEnumerable<IRequestBehavior<TRequest>> requestBehaviors,
        IViewStringRender viewRender)
    {
        _requestBehaviors = requestBehaviors;
        _viewRender = viewRender;
    }

    public Task<IResult> Handle(TRequest request, bool isPartailView, CancellationToken cancellationToken)
    {
        async Task<IResult> Handler()
        {
            var html = await _viewRender.RenderRazorViewToString(request.ViewModel, request, isPartailView);

            return new HtmlResult(html);
        }

        return _requestBehaviors
           .Reverse()
           .Aggregate((RequestHandlerDelegate)Handler,
               (next, behavior) => () => behavior.Handle(request, next, cancellationToken))();
    }
}