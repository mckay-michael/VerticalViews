using System;
namespace VerticalViews.ExampleMvc.Features.Privacy;

public class GetPrivacyInformation : IViewRequest<PrivacyViewModel>
{
    public string Title { get; set; }
}

