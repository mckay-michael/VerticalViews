namespace VerticalViews.ExampleMvc.Features.Home.ViewRequests;

public class HomeViewRequest : ViewRequest
{
    public override string Feature => "Home";

    public override string Group => null;

    public override string ViewName => "Index";
}

