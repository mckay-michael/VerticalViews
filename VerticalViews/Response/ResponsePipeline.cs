using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using VerticalViews.ResponseBehavior;
using VerticalViews.Results;
using VerticalViews.ViewRenders;

namespace VerticalViews.Response;

public class ResponsePipeline<TRequest, TViewModel> : IResponsePipeline<TRequest, TViewModel>
    where TRequest : IBaseRequest
{
    private readonly IEnumerable<IResponseBehavior<TRequest, TViewModel>> _responseBehaviors;
    private readonly IViewStringRender _viewRender;
    private readonly IMediator _mediator;

    public ResponsePipeline(
        IMediator mediator,
        IViewStringRender viewRender,
        IEnumerable<IResponseBehavior<TRequest, TViewModel>> responseBehaviors)
    {
        _responseBehaviors = responseBehaviors;
        _viewRender = viewRender;
        _mediator = mediator;
    }

    public async Task<IResult> Handle(IViewRequest<TViewModel> request, bool isPartailView, CancellationToken cancellationToken)
    {
        var viewModel = await _mediator.Send(request.Request);

        async Task<IResult> Handler()
        {
            var html = await _viewRender.RenderRazorViewToString(viewModel, request, isPartailView);

            return new HtmlResult(html);
        }

        return await _responseBehaviors
            .Reverse()
            .Aggregate((ResponseHandlerDelegate)Handler,
                (next, behavior) => () => behavior.Handle(viewModel, next, cancellationToken))();
    }
}

