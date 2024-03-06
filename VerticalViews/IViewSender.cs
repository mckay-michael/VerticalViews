using Microsoft.AspNetCore.Http;

namespace VerticalViews;

public interface IViewSender<TRequest, TViewModel>
    where TRequest : IViewRequest<TViewModel>
{
    Task<IResult> PartailView(IViewRequest<TViewModel> request, CancellationToken cancellationToken = default);

    Task<IResult> View(IViewRequest<TViewModel> request, CancellationToken cancellationToken = default);
}

public interface IViewSender<TRequest>
    where TRequest : IBaseRequest
{
    Task<IResult> PartailView(CancellationToken cancellationToken = default);

    Task<IResult> View(CancellationToken cancellationToken = default);
}