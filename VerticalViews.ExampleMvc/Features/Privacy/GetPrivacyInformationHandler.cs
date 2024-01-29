using System;

namespace VerticalViews.ExampleMvc.Features.Privacy
{
	public class GetPrivacyInformationHandler : IViewRequestHandler<GetPrivacyInformation, PrivacyViewModel>
	{
        public Task<PrivacyViewModel> Handle(GetPrivacyInformation request, CancellationToken cancellationToken) =>
            Task.FromResult(new PrivacyViewModel
            {
                Title = request.Title
            });
    }
}

