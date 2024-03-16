using MediatR;
using Microsoft.AspNetCore.Http;

namespace VerticalViews.Response;

public interface IResponsePipeline<TRequest, TViewModel, TMediatorReqeust>
    where TRequest : IViewRequest<TMediatorReqeust, TViewModel>
    where TMediatorReqeust : IRequest<TViewModel>
{
    Task<IResult> Handle(TRequest request, bool isPartailView, CancellationToken cancellationToken);
}