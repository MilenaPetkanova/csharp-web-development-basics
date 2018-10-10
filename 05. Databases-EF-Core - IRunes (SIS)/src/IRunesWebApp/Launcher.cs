namespace IRunesWebApp
{
    using SIS.HTTP.Enums;
    using SIS.WebServer;
    using SIS.WebServer.Routing;
    using SIS.WebServer.Results;
    using Controllers;

    public class Launcher
    {
        public static void Main()
        {
            var serverRoutingTable = new ServerRoutingTable();

            // index

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/"] = request =>
                new HomeController().Index(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/home/index"] = request =>
                new RedirectResult("/");

            // login

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/users/login"] = request =>
                new UsersController().Login(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/users/login"] = request =>
                new UsersController().LoginPost(request);

            // register

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/users/register"] = request =>
                new UsersController().Register(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/users/register"] = request =>
                new UsersController().RegisterPost(request);

            // logout

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/logout"] = request =>
                new UsersController().Logout(request);

            // albums

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/albums/all"] =
                request => new AlbumsController().All(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/albums/create"] = request =>
                new AlbumsController().Create(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/albums/create"] = request =>
                new AlbumsController().CreatePost(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/albums/details"] = request => 
                new AlbumsController().Details(request);

            // tracks
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/tracks/create"] = request => 
                new TracksController().Create(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/tracks/create"] = request => 
                new TracksController().CreatePost(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/tracks/details"] = request => 
                new TracksController().Details(request);


            var server = new Server(3456, serverRoutingTable);

            server.Run();
        }
    }
}