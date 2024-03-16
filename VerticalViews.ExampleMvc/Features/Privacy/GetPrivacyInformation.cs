using System;
using MediatR;

namespace VerticalViews.ExampleMvc.Features.Privacy;

public class GetPrivacyInformation : IViewRequest<GetPrivacyInformationRequest, PrivacyViewModel>
{
    public string Feature => "Testing";

    public string Group => "Privacy";

    public string ViewName => "Privacy";

    public GetPrivacyInformationRequest Request { get; set; }
}

