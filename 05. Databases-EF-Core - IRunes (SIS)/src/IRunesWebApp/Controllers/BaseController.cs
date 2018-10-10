namespace IRunesWebApp.Controllers
{
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Collections.Generic;
    //using SIS.HTTP.Contracts;
    using SIS.HTTP.Enums;
    using SIS.WebServer.Results;
    using Contracts;
    using Data;
    using Services;
    using SIS.HTTP.Requests;
    using SIS.HTTP.Responses;
    using SIS.HTTP.Exceptions;

    public abstract class BaseController
    {
        private const string ViewsFolderName = "Views";

        private const string ControllerDefaultName = "Controller";

        private const string HtmlFileExtension = ".html";

        private string GetControllerName => this.GetType().Name.Replace(ControllerDefaultName, string.Empty);

        protected IRunesDbContext Db { get; set; }

        protected IHashService HashService;

        protected IUserCookieService CookieService;

        protected bool IsUserAuthenticated { get; set; } = false;

        protected IDictionary<string, string> ViewBag { get; set; }


        public BaseController()
        {
            this.Db = new IRunesDbContext();
            this.HashService = new HashService();
            this.CookieService = new UserCookieService();
            this.ViewBag = new Dictionary<string, string>();
        }

        protected string GetUsername(IHttpRequest request)
        {

            if (!request.Cookies.ContainsCookie("auth-irunes"))
            {
                return null;
            }

            this.IsUserAuthenticated = true;

            var cookie = request.Cookies.GetCookie("auth-irunes");
            var cookieContent = cookie.Value;
            var userName = this.CookieService.GetUserData(cookieContent);
            return userName;
        }

        protected IHttpResponse View([CallerMemberName] string viewName = "")
        {
            var filePath =
                $"{ViewsFolderName}/{this.GetControllerName}/{viewName}{HtmlFileExtension}";

            if (!File.Exists(filePath))
            {
                throw new BadRequestException("View was not found.");
            }

            var fileContent = File.ReadAllText(filePath);


            foreach (var viewBagKey in ViewBag.Keys)
            {
                var dynamicDataPlaceholder = $"{{{{{viewBagKey}}}}}";

                if (fileContent.Contains(dynamicDataPlaceholder))
                {
                    fileContent = fileContent.Replace(dynamicDataPlaceholder, this.ViewBag[viewBagKey]);
                }
            }

            var response = new HtmlResult(fileContent, HttpResponseStatusCode.Ok);

            return response;
        }

        protected IHttpResponse BadRequestError(string errorMessage)
        {
            return new HtmlResult($"<h3>{errorMessage}</h3>", HttpResponseStatusCode.BadRequest);
        }

        protected IHttpResponse ServerError(string errorMessage)
        {
            return new HtmlResult($"<h3>{errorMessage}</h3>", HttpResponseStatusCode.InternalServerError);
        }
    }
}