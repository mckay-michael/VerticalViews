using VerticalViews.ExampleMvc.Models;

namespace VerticalViews.ExampleMvc.Features.Error;

public class ErrorViewOptions : ViewRequest<ErrorViewModel>
{
    public override string Feature => "Error";

    public override string Group => null;

    public override string ViewName => "Error";
}

