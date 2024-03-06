using Microsoft.AspNetCore.Http;

namespace VerticalViews.Request;

public interface IRequestPipeline<TRequest, TViewModel>
    where TRequest : IViewRequest<TViewModel>
{
    Task<IResult> Handle(IViewRequest<TViewModel> request, bool isPartailView, CancellationToken cancellationToken);
}

public interface IRequestPipeline<TRequest>
    where TRequest : ViewRequest
{
    Task<IResult> Handle(ViewRequest request, bool isPartailView, CancellationToken cancellationToken);
}