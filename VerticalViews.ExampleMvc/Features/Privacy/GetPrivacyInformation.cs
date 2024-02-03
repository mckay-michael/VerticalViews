using System;
using MediatR;

namespace VerticalViews.ExampleMvc.Features.Privacy;

public class GetPrivacyInformation : IViewRequest<PrivacyViewModel>
{
    public string Feature => "Privacy";

    public string Group => null;

    public string ViewName => "Privacy";

    public IRequest<PrivacyViewModel> Request { get; set; }
}

