using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using VerticalViews.ResponseBehavior;
using VerticalViews.Results;
using VerticalViews.ViewRenders;

namespace VerticalViews.Wrappers;

public abstract class RequestHandlerBase
{
    public abstract Task<IResult> Handle(object request, bool isPartailView, IServiceProvider serviceProvider, CancellationToken cancellationToken);

    protected async Task<IResult> RenderView(IServiceProvider serviceProvider, object viewModel, BaseRequest request, bool isPartailView)
    {
        var viewRender = serviceProvider.GetRequiredService<IViewStringRender>();

        var html = await viewRender.RenderRazorViewToString(viewModel, request, isPartailView);

        return new HtmlResult(html);
    }
}

public abstract class RequestHandlerWrapper<TViewModel> : RequestHandlerBase
{
    public abstract Task<IResult> Handle(IViewRequest<TViewModel> request, bool isPartailView, IServiceProvider serviceProvider, CancellationToken cancellationToken);
}

public abstract class RequestClassHandlerWrapper: RequestHandlerBase
{
    public abstract Task<IResult> Handle(ViewRequest request, bool isPartailView, IServiceProvider serviceProvider, CancellationToken cancellationToken);
}

public class RequestHandlerWrapperImpl<TRequest, TViewModel> : RequestHandlerWrapper<TViewModel>
    where TRequest : IViewRequest<TViewModel>
{
    public override async Task<IResult> Handle(object request, bool isPartailView, IServiceProvider serviceProvider, CancellationToken cancellationToken) =>
        await Handle((IViewRequest<TViewModel>)request, isPartailView, serviceProvider, cancellationToken).ConfigureAwait(false);

    public override Task<IResult> Handle(IViewRequest<TViewModel> request, bool isPartailView, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        async Task<IResult> Handler()
        {
            var mediator = serviceProvider.GetRequiredService<IMediator>();

            var viewModel = await mediator.Send(request.Request);

            return await RenderView(serviceProvider, viewModel, request, isPartailView);
        }

        return serviceProvider
            .GetServices<IResponseBehavior<TViewModel>>()
            .Reverse()
            .Aggregate((RequestHandlerDelegate)Handler,
                (next, pipeline) => () => pipeline.Handle((TRequest)request, next, cancellationToken))();
    }
}

public class RequestClassHandlerWrapperImpl<TRequest> : RequestClassHandlerWrapper
    where TRequest : ViewRequest
{
    public override async Task<IResult> Handle(object request, bool isPartailView, IServiceProvider serviceProvider, CancellationToken cancellationToken) =>
        await Handle((ViewRequest)request, isPartailView, serviceProvider, cancellationToken).ConfigureAwait(false);

    public override Task<IResult> Handle(ViewRequest request, bool isPartailView, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        Task<IResult> Handler() => RenderView(serviceProvider, request.ViewModel, request, isPartailView);

        return serviceProvider
            .GetServices<ResponseBehavior<TRequest>>()
            .Reverse()
            .Aggregate((RequestHandlerDelegate)Handler,
                (next, pipeline) => () => pipeline.Handle(request, next, cancellationToken))();
    }
}