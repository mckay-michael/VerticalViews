using Microsoft.AspNetCore.Http;

namespace VerticalViews;

public interface IViewSender<TRequest, TViewModel>
    where TRequest : BaseRequest
{
    Task<IResult> PartailView(BaseRequest request, CancellationToken cancellationToken = default);

    Task<IResult> View(BaseRequest request, CancellationToken cancellationToken = default);
}