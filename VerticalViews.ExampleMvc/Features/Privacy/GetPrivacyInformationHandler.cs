using System;
using MediatR;

namespace VerticalViews.ExampleMvc.Features.Privacy
{
	public class GetPrivacyInformationHandler : IRequestHandler<GetPrivacyInformationRequest, PrivacyViewModel>
	{
        public Task<PrivacyViewModel> Handle(GetPrivacyInformationRequest request, CancellationToken cancellationToken) =>
            Task.FromResult(new PrivacyViewModel
            {
                Title = request.Title
            });
    }
}

