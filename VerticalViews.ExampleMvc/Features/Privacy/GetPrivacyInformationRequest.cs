using System;
using MediatR;

namespace VerticalViews.ExampleMvc.Features.Privacy;

public class GetPrivacyInformationRequest : IRequest<PrivacyViewModel>
{
    public string Title { get; set; }
}

