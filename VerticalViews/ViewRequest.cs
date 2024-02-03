using System;
using MediatR;

namespace VerticalViews;

public interface IViewRequest<TViewModel> : BaseRequest
{
    IRequest<TViewModel> Request { get; set; }
}

public abstract class ViewRequest<TViewModel> : ViewRequest, BaseRequest
{
    public override object ViewModel => Activator.CreateInstance(typeof(TViewModel));
}

public abstract class ViewRequest : BaseRequest
{
    public virtual object ViewModel => null;

    public abstract string Feature { get; }

    public abstract string Group { get; }

    public abstract string ViewName { get; }
}

public interface BaseRequest
{
    public string Feature { get; }

    public string Group { get; }

    public string ViewName { get; }
}
