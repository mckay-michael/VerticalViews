using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using VerticalViews.ExampleMvc.Features.Error;
using VerticalViews.ExampleMvc.Features.Home.ViewRequests;
using VerticalViews.ExampleMvc.Features.Privacy;
using VerticalViews.ExampleMvc.Models;
using VerticalViews.Factories;
using VerticalViews.Registration;
using VerticalViews.Request;
using VerticalViews.ResponseBehavior;

namespace VerticalViews.ExampleMvc;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddVerticalViews(Assembly.GetExecutingAssembly());

        builder.Services.AddTransient<IRequestBehavior<GetPrivacyInformation, PrivacyViewModel>, PrivacyResponseBehavior>();

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

        //app.MapGet("/", async ([FromServices] IViewSender viewSender) => await viewSender.View(ViewRequestFactory.Create<HomeViewRequest>()));
        //app.MapGet("/PartailViewIndex", async ([FromServices] IViewSender viewSender) => await viewSender.PartailView(ViewRequestFactory.Create<HomeViewRequest>()));

        app.MapGet("/Privacy", async ([FromServices] IViewSender<GetPrivacyInformation, PrivacyViewModel> viewSender) =>
            await viewSender.View(ViewRequestFactory.Create<GetPrivacyInformation, PrivacyViewModel>(
                new GetPrivacyInformationRequest
                {
                    Title = "Hello Mediator"
                })));

        //app.MapGet("/PartailViewPrivacy", async ([FromServices] IViewSender viewSender) =>
        //    await viewSender.PartailView(ViewRequestFactory.Create<GetPrivacyInformation, PrivacyViewModel>(
        //        new GetPrivacyInformationRequest
        //        {
        //            Title = "Hello Mediator PartailViewPrivacy"
        //        })));

        //app.MapGet("/Error", async ([FromServices] IViewSender viewSender) =>
        //    await viewSender.View(ViewRequestFactory.Create<ErrorViewOptions>()));

        //app.MapGet("/PartailViewError", async ([FromServices] IViewSender viewSender) =>
        //    await viewSender.PartailView(ViewRequestFactory.Create<ErrorViewOptions>()));

        app.Run();
    }
}

