using Microsoft.AspNetCore.Http;

namespace VerticalViews;

public interface IViewSender
{
    Task<IResult> PartailView<TViewModel>(IViewRequest<TViewModel> request, CancellationToken cancellationToken = default);

    Task<IResult> PartailView(ViewRequest request, CancellationToken cancellationToken = default);

    Task<IResult> View<TViewModel>(IViewRequest<TViewModel> request, CancellationToken cancellationToken = default);

    Task<IResult> View(ViewRequest request, CancellationToken cancellationToken = default);
}