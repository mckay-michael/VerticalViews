using System;
using MediatR;

namespace VerticalViews;

public interface IViewRequest<TViewModel> : IBaseRequest
{
    IRequest<TViewModel> Request { get; set; }
}

public abstract class ViewRequest<TViewModel> : ViewRequest, IBaseRequest
{
    public override object ViewModel => Activator.CreateInstance(typeof(TViewModel));
}

public abstract class ViewRequest : IBaseRequest
{
    public virtual object ViewModel => null;

    public abstract string Feature { get; }

    public abstract string Group { get; }

    public abstract string ViewName { get; }
}

public interface IBaseRequest
{
    public string Feature { get; }

    public string Group { get; }

    public string ViewName { get; }
}
