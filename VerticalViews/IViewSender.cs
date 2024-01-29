using System;
using Microsoft.AspNetCore.Http;
using VerticalViews.Options;

namespace VerticalViews;

public interface IViewSender
{
    Task<IResult> View<TViewModel, TOptions>(
        string viewName,
        IViewRequest<TViewModel> request,
        CancellationToken cancellationToken = default)
            where TViewModel : class
            where TOptions : VerticalViewOptions;

    Task<IResult> View<TViewModel, TOptions>(string viewName)
        where TViewModel : class
        where TOptions : VerticalViewOptions;


    Task<IResult> View<TOptions>(string viewName)
        where TOptions : VerticalViewOptions;

    Task<IResult> PartailView<TViewModel, TOptions>(
        string viewName,
        IViewRequest<TViewModel> request,
        CancellationToken cancellationToken = default)
            where TViewModel : class
            where TOptions : VerticalViewOptions;

    Task<IResult> PartailView<TViewModel, TOptions>(string viewName)
        where TViewModel : class
        where TOptions : VerticalViewOptions;

    Task<IResult> PartailView<TOptions>(string viewName)
        where TOptions : VerticalViewOptions;
}