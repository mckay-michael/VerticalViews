using System;
namespace VerticalViews;

/// <summary>
/// Marker interface to represent a request with a response
/// </summary>
/// <typeparam name="TResponse">Response type</typeparam>
public interface IViewRequest<TViewModel> where TViewModel : class { }