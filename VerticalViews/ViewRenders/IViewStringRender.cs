using VerticalViews.Options;

namespace VerticalViews.ViewRenders
{
    public interface IViewStringRender
    {
        Task<string> RenderRazorViewToString<TOptions>(string viewName, object model, bool IsPartailView)
            where TOptions : VerticalViewOptions;
    }
}