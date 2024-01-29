using System;
using Microsoft.AspNetCore.Mvc.Razor;

namespace VerticalViews.ExampleMvc
{
	public class SubAreaViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            foreach (var loction in viewLocations)
            {
                yield return loction;
            }

            yield return "/Views/{1}/{0}.cshtml";
            yield return "/Views/Shared/{0}.cshtml";
        }

        public void PopulateValues(ViewLocationExpanderContext context) { }
    }
}

