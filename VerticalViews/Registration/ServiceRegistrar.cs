﻿using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using VerticalViews.ViewRenders;

namespace VerticalViews.Registration;

public static class ServiceRegistrar
{

    public static void AddVerticalViews(this IServiceCollection services, Assembly assembly)
    {
        const string viewAreaLocation = "/Features/{2}/{1}/Views/{0}.cshtml";
        const string viewLocation = "/Features/{1}/Views/{0}.cshtml";
        const string featuresLocation = "/Features/{1}/{0}.cshtml";
        const string viewSharedLocation = "/Features/Shared/Views/{0}.cshtml";

        services.AddHttpContextAccessor();
        services.AddScoped<IViewSender, ViewSender>(); 
        services.AddScoped<IViewStringRender, ViewStringRender>();

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

        var iViewRequestHandlerType = typeof(IViewRequestHandler<,>);

        var modules = assembly.DefinedTypes
            .Where(type => type.GetInterfaces()
                .Any(interfaceType => IsIViewRequestHandler(interfaceType, iViewRequestHandlerType)));

        foreach (var module in modules)
        {
            var interfaceType = module.GetInterfaces().First(interfaceType =>
                IsIViewRequestHandler(interfaceType, iViewRequestHandlerType));

            services.AddTransient(interfaceType, module);
        }
    }
    public static bool IsIViewRequestHandler(Type interfaceType, Type iViewRequestHandlerType)
    {
        return interfaceType.IsGenericType
            && iViewRequestHandlerType.IsAssignableFrom(interfaceType.GetGenericTypeDefinition());
    }
}
