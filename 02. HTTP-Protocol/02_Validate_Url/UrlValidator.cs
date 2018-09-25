namespace _02_Validate_Url
{
    using System;
    using System.Net;
    using System.Text;

    public class UrlValidator
    {
        public static void Main(string[] args)
        {
            try
            {
                var decodedUrl = WebUtility.UrlDecode(Console.ReadLine());
                var url = new Uri(decodedUrl);

                var protocol = url.Scheme;
                var host = url.Host;
                var port = url.Port;
                var path = url.LocalPath;
                var query = url.Query;
                var fragment = url.Fragment;

                ValidateUrlParts(url, protocol, host, port, path, query, fragment);

                PrintResult(url, protocol, host, port, path, query, fragment);
            }
            catch (UriFormatException ufe)
            {
                Console.WriteLine("Invalid URL");
            }
        }

        private static void PrintResult(Uri url, string protocol, string host, int port, string path, string query, string fragment)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Protocol: " + protocol);
            sb.AppendLine("Host: " + host);
            sb.AppendLine("Port: " + port);
            sb.AppendLine("Path: " + path);

            if (!string.IsNullOrEmpty(query))
            {
                sb.AppendLine("Query: " + url.Query.Substring(1, url.Query.Length - 1));
            }
            if (!string.IsNullOrEmpty(fragment))
            {
                sb.AppendLine("Fragment: " + url.Fragment.Substring(1, url.Fragment.Length - 1));
            }

            Console.WriteLine(sb.ToString().Trim());
        }

        private static void ValidateUrlParts(Uri url, string protocol, string host, int port, string path, string query, string fragment)
        {
            if (!string.IsNullOrEmpty(protocol) || !string.IsNullOrEmpty(host) || !string.IsNullOrEmpty(path) || port == -1)
            {
                throw new UriFormatException();
            }
        }
    }
}
