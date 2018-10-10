namespace IRunesWebApp.Controllers
{
    using System;
    using System.Linq;
    using Models;
    using SIS.HTTP.Cookies;
    using SIS.WebServer.Results;
    using SIS.HTTP.Responses;
    using SIS.HTTP.Requests;

    public class UsersController : BaseController
    {
        public IHttpResponse Login(IHttpRequest request)
        {
            return this.View();
        }

        public IHttpResponse LoginPost(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();

            var hashedPassword = this.HashService.Hash(password);

            var user = this.Db.Users.FirstOrDefault(u =>
                u.Username.Equals(username) && u.Password.Equals(hashedPassword));

            if (user == null)
            {
                return this.BadRequestError("A user with the given username and password does not exist.");
            }

            request.Session.AddParameter("username", username);

            var userCookieValue = this.CookieService.GetUserCookie(username);

            this.ViewBag["username"] = username;

            this.IsUserAuthenticated = true;

            var response = this.View("IndexLoggedIn");
            var cookie = new HttpCookie("auth-irunes", userCookieValue, 7) { HttpOnly = true };

            response.Cookies.Add(cookie);

            return response;
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            return this.View();
        }

        public IHttpResponse RegisterPost(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();
            var confirmPassword = request.FormData["confirmPassword"].ToString();
            var email = request.FormData["email"].ToString();

            if (string.IsNullOrWhiteSpace(username) || username.Length < 4)
            {
                return this.BadRequestError("Please provide a valid username with 4 or more characters.");
            }

            if (this.Db.Users.Any(u => u.Username.Equals(username)))
            {
                return this.BadRequestError("A user with this username already exists.");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                return this.BadRequestError("Please provide a valid password with at least 6 or more characters.");
            }

            if (password != confirmPassword)
            {
                return this.BadRequestError("Passwords don't match");
            }

            var hashedPassword = this.HashService.Hash(password);

            var user = new User()
            {
                Username = username,
                Email = email,
                Password = hashedPassword
            };

            try
            {
                Db.Users.Add(user);

                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                return this.ServerError("Couldn't add entity to the database.");
            }

            return this.View("Login");
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            if (!request.Cookies.ContainsCookie("auth-irunes"))
            {
                return new RedirectResult("/");
            }

            var cookie = request.Cookies.GetCookie("auth-irunes");
            cookie.Delete();
            var response = new RedirectResult("/");
            response.Cookies.Add(cookie);

            return response;
        }
    }
}