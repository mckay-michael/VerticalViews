using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using VerticalViews.ExampleMvc.Features.Error;
using VerticalViews.ExampleMvc.Features.Home.Options;
using VerticalViews.ExampleMvc.Features.Privacy;
using VerticalViews.ExampleMvc.Models;
using VerticalViews.Registration;

namespace VerticalViews.ExampleMvc;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddVerticalViews(Assembly.GetExecutingAssembly());

        builder.Services.Configure<RazorViewEngineOptions>(o =>
        {
            o.ViewLocationExpanders.Add(new SubAreaViewLocationExpander());
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapGet("/", async ([FromServices] IViewSender viewSender) => await viewSender.View<HomeViewOptions>("Index"));
        app.MapGet("/PartailViewIndex", async ([FromServices] IViewSender viewSender) => await viewSender.PartailView<HomeViewOptions>("Index"));

        app.MapGet("/Privacy", async ([FromServices] IViewSender viewSender) =>
            await viewSender.View<PrivacyViewModel, PrivacyViewOptions>("Privacy", new GetPrivacyInformation
            {
                Title = "Hello Mediator"
            }));

        app.MapGet("/PartailViewPrivacy", async ([FromServices] IViewSender viewSender) =>
            await viewSender.PartailView<PrivacyViewModel, PrivacyViewOptions>("Privacy", new GetPrivacyInformation
            {
                Title = "Hello Mediator PartailViewPrivacy"
            }));

        app.MapGet("/Error", async ([FromServices] IViewSender viewSender) =>
            await viewSender.View<ErrorViewModel, ErrorViewOptions>("Error"));

        app.MapGet("/PartailViewError", async ([FromServices] IViewSender viewSender) =>
            await viewSender.PartailView<ErrorViewModel, ErrorViewOptions>("Error"));

        app.Run();
    }
}

