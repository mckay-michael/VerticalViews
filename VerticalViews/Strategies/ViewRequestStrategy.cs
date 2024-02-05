using Microsoft.AspNetCore.Http;
using VerticalViews.Request;
using VerticalViews.Results;
using VerticalViews.ViewRenders;

namespace VerticalViews.Strategies;

public class ViewRequestStrategy<TRequest, TViewModel> : IViewRequestStrategy<TRequest, TViewModel>
    where TRequest : BaseRequest
{
    private readonly IViewStringRender _viewRender;
    private readonly IRequestPipeline<TRequest, TViewModel> _requestPipeline;

    public ViewRequestStrategy(
        IViewStringRender viewRender,
        IRequestPipeline<TRequest, TViewModel> requestPipeline)
    {
        _viewRender = viewRender;
        _requestPipeline = requestPipeline;
    }

    public Task<IResult> Execute(BaseRequest request, bool isPartailView, CancellationToken cancellationToken)
    {
        if (request is IViewRequest<TViewModel> viewRequest)
        {
            return _requestPipeline.Handle(viewRequest, isPartailView, cancellationToken);
        }
        else
        {
            return GetEmptyViewResult((ViewRequest)request, isPartailView);
        }
    }

    private async Task<IResult> GetEmptyViewResult(ViewRequest request, bool isPartailView)
    {
        var html = await _viewRender.RenderRazorViewToString(request.ViewModel, request, isPartailView);

        return new HtmlResult(html);
    }
}

