using System;
using MediatR;

namespace VerticalViews.Factories;

public static class ViewRequestFactory
{
	public static IViewRequest<TViewModel> Create<TViewRequest, TViewModel>(IRequest<TViewModel> request)
		where TViewRequest: IViewRequest<TViewModel>, new()
    {
		var viewRequest = new TViewRequest();

		viewRequest.Request = request;

		return viewRequest;
    }

    public static ViewRequest Create<TViewRequest>()
        where TViewRequest : ViewRequest, new()
    {
        return new TViewRequest();
    }
}