namespace SpaExample2
{
    using Microsoft.AspNetCore.Rewrite;
    using System.Linq;
    using System.Net;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.Net.Http.Headers;
    public class LanguageRedirectRule : IRule
    {
        const string key = "oe-lang";
        public void ApplyRule(RewriteContext context)
        {
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;
            var host = request.Host;
            var path = request.Path;

        
            // try to get the language from the default version of the browser.cd/
            var rqf = request.HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = rqf.RequestCulture.Culture.ToString();
           
            string[] excludeTokens = { "/api/", "/es", "/en" };


            foreach(var token in excludeTokens)
            {
                if (request.Path.Value.Contains(token))
                {
                    context.Result = RuleResult.ContinueRules;
                    return;
                }
            }
            

            var newPath = request.Scheme + "://" + host.Value + request.PathBase + $"/{culture}" + request.Path +
                          request.QueryString;

            response.StatusCode = (int)HttpStatusCode.Redirect;
            response.Headers[HeaderNames.Location] = newPath;
            context.Result = RuleResult.EndResponse; // Do not continue processing the request 
        }
    }
}