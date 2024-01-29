using System;
using Microsoft.Extensions.DependencyInjection;

namespace VerticalViews.Wrappers;

public abstract class RequestHandlerBase
{
    public abstract Task<object?> Handle(object request, IServiceProvider serviceProvider,
        CancellationToken cancellationToken);
}

public abstract class RequestHandlerWrapper<TViewModel> : RequestHandlerBase where TViewModel : class 
{
    public abstract Task<TViewModel> Handle(IViewRequest<TViewModel> request, IServiceProvider serviceProvider,
        CancellationToken cancellationToken);
}

public class RequestHandlerWrapperImpl<TRequest, TViewModel> : RequestHandlerWrapper<TViewModel>
    where TRequest : IViewRequest<TViewModel>
    where TViewModel : class
{
    public override async Task<object?> Handle(object request, IServiceProvider serviceProvider,
        CancellationToken cancellationToken) =>
        await Handle((IViewRequest<TViewModel>)request, serviceProvider, cancellationToken).ConfigureAwait(false);

    public override Task<TViewModel> Handle(IViewRequest<TViewModel> request, IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        return serviceProvider.GetRequiredService<IViewRequestHandler<TRequest, TViewModel>>()
            .Handle((TRequest)request, cancellationToken);
    }
}