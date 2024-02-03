namespace VerticalViews.ViewRenders
{
    public interface IViewStringRender
    {
        Task<string> RenderRazorViewToString(object model, BaseRequest options, bool isPartailView);
    }
}