using Microsoft.AspNetCore.Http;

namespace VerticalViews.Request;

public interface IRequestPipeline<TRequest, TViewModel>
    where TRequest : BaseRequest
{
    Task<IResult> Handle(IViewRequest<TViewModel> request, bool isPartailView, CancellationToken cancellationToken);
}