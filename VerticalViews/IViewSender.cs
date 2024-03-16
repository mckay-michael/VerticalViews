using MediatR;
using Microsoft.AspNetCore.Http;

namespace VerticalViews;

public interface IViewSender<TRequest, TViewModel, TMediatorReqeust>
    where TRequest : IViewRequest<TMediatorReqeust, TViewModel>,new()
    where TMediatorReqeust : IRequest<TViewModel>
{
    Task<IResult> PartailView(TMediatorReqeust request, CancellationToken cancellationToken = default);

    Task<IResult> View(TMediatorReqeust request, CancellationToken cancellationToken = default);
}

public interface IViewSender<TRequest>
    where TRequest : ViewRequest, new()
{
    Task<IResult> PartailView(CancellationToken cancellationToken = default);

    Task<IResult> View(CancellationToken cancellationToken = default);
}