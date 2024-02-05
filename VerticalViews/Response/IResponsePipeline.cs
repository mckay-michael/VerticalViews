using Microsoft.AspNetCore.Http;

namespace VerticalViews.Response;

public interface IResponsePipeline<TRequest, TViewModel>
    where TRequest : BaseRequest
{
    Task<IResult> Handle(IViewRequest<TViewModel> request, bool isPartailView, CancellationToken cancellationToken);
}