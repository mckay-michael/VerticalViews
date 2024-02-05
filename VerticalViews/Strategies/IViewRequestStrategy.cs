using Microsoft.AspNetCore.Http;

namespace VerticalViews.Strategies
{
    public interface IViewRequestStrategy<TRequest, TViewModel>
    {
        Task<IResult> Execute(BaseRequest request, bool isPartailView, CancellationToken cancellationToken);
    }
}