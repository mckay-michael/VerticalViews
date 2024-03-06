namespace VerticalViews.ViewRenders
{
    public interface IViewStringRender
    {
        Task<string> RenderRazorViewToString(object model, IBaseRequest options, bool isPartailView);
    }
}