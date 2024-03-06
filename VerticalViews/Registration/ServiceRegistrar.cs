using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using VerticalViews.Request;
using VerticalViews.Response;
using VerticalViews.ViewRenders;

namespace VerticalViews.Registration;

public static class ServiceRegistrar
{

    public static void AddVerticalViews(this IServiceCollection services, params Assembly[] assembly)
    {
        const string viewAreaLocation = "/Features/{2}/{1}/Views/{0}.cshtml";
        const string viewLocation = "/Features/{1}/Views/{0}.cshtml";
        const string featuresLocation = "/Features/{1}/{0}.cshtml";
        const string viewSharedLocation = "/Features/Shared/Views/{0}.cshtml";

        services
            .AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(assembly));

        services.AddHttpContextAccessor();

        services.AddScoped(typeof(IViewSender<,>), typeof(ViewSender<,>)); 
        services.AddScoped(typeof(IViewSender<>), typeof(ViewSender<>));
        services.AddScoped(typeof(IResponsePipeline<,>), typeof(ResponsePipeline<,>)); 
        services.AddScoped(typeof(IRequestPipeline<,>), typeof(RequestPipeline<,>)); 
        services.AddScoped(typeof(IViewStringRender), typeof(ViewStringRender));

        services.AddMvc().AddRazorOptions(options =>
        {
            options.ViewLocationFormats.Add(viewAreaLocation);
            options.PageViewLocationFormats.Add(viewAreaLocation);
            options.AreaViewLocationFormats.Add(viewAreaLocation);

            options.ViewLocationFormats.Add(viewLocation);
            options.PageViewLocationFormats.Add(viewLocation);
            options.AreaViewLocationFormats.Add(viewLocation);

            options.ViewLocationFormats.Add(featuresLocation);
            options.PageViewLocationFormats.Add(featuresLocation);
            options.AreaViewLocationFormats.Add(featuresLocation);

            options.ViewLocationFormats.Add(viewSharedLocation);
            options.PageViewLocationFormats.Add(viewSharedLocation);
            options.AreaViewLocationFormats.Add(viewSharedLocation);
        });
    }
}