using Microsoft.AspNetCore.Http;
using VerticalViews.Request;
using VerticalViews.ResponseBehavior;

namespace VerticalViews.ExampleMvc.Features.Privacy;

public class PrivacyResponseBehavior : IRequestBehavior<GetPrivacyInformation, PrivacyViewModel>
{
    public async Task<IResult> Handle(GetPrivacyInformation request, RequestHandlerDelegate next, CancellationToken cancellationToken)
    {
        return TypedResults.Redirect("http://google.com/");
    }
}