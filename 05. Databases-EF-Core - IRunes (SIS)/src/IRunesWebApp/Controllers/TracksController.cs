namespace IRunesWebApp.Controllers
{
    using System.Text;
    using System.Web;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using SIS.WebServer.Results;
    using Models;
    using SIS.HTTP.Responses;
    using SIS.HTTP.Requests;

    public class TracksController : BaseController
    {
        public IHttpResponse Create(IHttpRequest request)
        {
            var username = this.GetUsername(request);

            if (username == null)
            {
                return this.View("Login");
            }

            var albumId = request.QueryData["albumId"].ToString();
            this.ViewBag["albumId"] = albumId;

            return this.View();
        }

        public IHttpResponse CreatePost(IHttpRequest request)
        {
            var username = this.GetUsername(request);

            if (username == null)
            {
                return this.View("Login");
            }

            var albumId = request.QueryData["albumId"].ToString();

            var name = request.FormData["name"].ToString();
            var link = request.FormData["link"].ToString();
            var price = decimal.Parse(request.FormData["price"].ToString());

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(link) || price == 0)
            {
                return this.View("Create");
            }

            var album = this.Db.Albums.Include(x => x.Tracks).FirstOrDefault(x => x.Id == albumId);

            album.Tracks.Add(new Track { Name = name, Link = link, Price = price });
            this.Db.SaveChanges();

            var albumCover = HttpUtility.UrlDecode(album.Cover);

            var tracksPrice = album.Tracks.Sum(x => x.Price);
            var tracksPriceAfterDiscount = tracksPrice - (tracksPrice * 13 / 100);

            var albumData = new StringBuilder();
            albumData.Append($"<br/><img src=\"{albumCover}\" width=\"250\" height=\"250\"><br/>");
            albumData.Append($"<b>Name: {album.Name}</b><br/>");
            albumData.Append($"<b>Price: ${tracksPriceAfterDiscount}</b><br/>");

            var tracks = album.Tracks.ToArray();

            var sbTracks = new StringBuilder();

            this.ViewBag["tracks"] = "";

            if (tracks.Length > 0)
            {
                foreach (var track in tracks)
                {
                    sbTracks.Append(
                        $"<a href=\"/track/details?id={track.Id}&albumId={albumId}\">{track.Name}</a></li><br/>");
                }

                this.ViewBag["tracks"] = sbTracks.ToString();
            }

            this.ViewBag["albumId"] = album.Id;
            this.ViewBag["album"] = albumData.ToString();

            return new RedirectResult($"/albums/details?id={albumId}");
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            var username = this.GetUsername(request);

            if (username == null)
            {
                return this.View("Login");
            }

            var trackId = request.QueryData["id"].ToString();
            var albumId = request.QueryData["albumId"].ToString();

            var track = this.Db.Tracks.FirstOrDefault(x => x.Id == int.Parse(trackId));

            var trackLink = HttpUtility.UrlDecode(track.Link);

            var trackInfo = new StringBuilder();
            trackInfo.Append($"<b>Track Name: {track.Name}</b><br/>");
            trackInfo.Append($"<b>Track Price: ${track.Price}</b><br/>");

            var trackVideo = $"<iframe class=\"embed-responsive-item\" src=\"{trackLink}\"></iframe><br/>";

            this.ViewBag["trackVideo"] = trackVideo;
            this.ViewBag["trackInfo"] = trackInfo.ToString();

            this.ViewBag["albumId"] = albumId;

            return this.View();
        }
    }
}