using System;
using Microsoft.AspNetCore.Http;
using VerticalViews.Response;
using VerticalViews.ResponseBehavior;
using VerticalViews.Results;

namespace VerticalViews.Request;

public class RequestPipeline<TRequest, TViewModel> : IRequestPipeline<TRequest, TViewModel>
    where TRequest : BaseRequest
{
    private readonly IEnumerable<IRequestBehavior<TRequest, TViewModel>> _requestBehaviors;
    private readonly IResponsePipeline<TRequest, TViewModel> _responsePipeline;

    public RequestPipeline(
        IEnumerable<IRequestBehavior<TRequest, TViewModel>> requestBehaviors,
        IResponsePipeline<TRequest, TViewModel> responsePipeline)
    {
        _requestBehaviors = requestBehaviors;
        _responsePipeline = responsePipeline;
    }

    public Task<IResult> Handle(IViewRequest<TViewModel> request, bool isPartailView, CancellationToken cancellationToken)
    {
        Task<IResult> Handler() =>
            _responsePipeline.Handle(request, isPartailView, cancellationToken);

        return _requestBehaviors
           .Reverse()
           .Aggregate((RequestHandlerDelegate)Handler,
               (next, behavior) => () => behavior.Handle(request, next, cancellationToken))();
    }
}

