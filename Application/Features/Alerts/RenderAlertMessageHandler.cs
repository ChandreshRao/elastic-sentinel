using ElasticSentinel.Application.Common.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ElasticSentinel.Application.Features.Alerts;

/// <summary>
/// Renders alert messages to HTML using Razor view engine.
/// Used for generating HTML email bodies from Razor templates.
/// </summary>
internal sealed class RenderAlertMessageHandler : IRenderAlertMessageHandler
{
    private readonly IRazorViewEngine _razorViewEngine;
    private readonly ITempDataProvider _tempDataProvider;
    private readonly IServiceProvider _serviceProvider;

    public RenderAlertMessageHandler(
        IRazorViewEngine razorViewEngine,
        ITempDataProvider tempDataProvider,
        IServiceProvider serviceProvider)
    {
        _razorViewEngine = razorViewEngine;
        _tempDataProvider = tempDataProvider;
        _serviceProvider = serviceProvider;
    }

    public async Task<string> HandleAsync(
        RenderAlertMessageRequest request,
        CancellationToken cancellationToken = default)
    {
        var actionContext = GetContext();
        var view = FindView(request.ViewName);

        using (var output = new StringWriter())
        {
            var viewContext = new ViewContext(
                actionContext: actionContext,
                view: view,
                viewData: new ViewDataDictionary(
                    metadataProvider: new EmptyModelMetadataProvider(),
                    modelState: new ModelStateDictionary())
                {
                    Model = request.Model
                },
                tempData: new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                writer: output,
                htmlHelperOptions: new HtmlHelperOptions()
            );

            await view.RenderAsync(viewContext);
            return output.ToString();
        }
    }

    private IView FindView(string viewName)
    {
        ViewEngineResult viewResult = _razorViewEngine.GetView(
            executingFilePath: null,
            viewPath: viewName,
            isMainPage: true);

        if (viewResult.Success)
        {
            return viewResult.View;
        }

        throw new Exception("Invalid View Path");
    }

    private ActionContext GetContext()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.RequestServices = _serviceProvider;
        return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
    }
}
