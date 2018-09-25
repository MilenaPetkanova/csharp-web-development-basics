namespace _03_Request_Parser
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class RequestParser
    {
        public static void Main()
        {
            var routesByMethod = new Dictionary<string, HashSet<string>>();

            var inputLine = Console.ReadLine();
            while (inputLine != "END")
            {
                var inputArgs = inputLine.ToLower()
                    .Split('/', StringSplitOptions.RemoveEmptyEntries);
                var path = inputArgs[0];
                var method = inputArgs[1];

                if (!routesByMethod.ContainsKey(method))
                {
                    routesByMethod.Add(method, new HashSet<string>());
                }

                routesByMethod[method].Add(path);

                inputLine = Console.ReadLine();
            }

            var httpRequest = Console.ReadLine().ToLower()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var httpRequestMethod = httpRequest[0];
            var httpRequestPath = httpRequest[1].Substring(1, httpRequest[1].Length - 1);
            var httpRequestprotocol = httpRequest[2];

            var statusCodeSuccess = 200;
            var respondTextSuccess = "OK";
            var statusCodeNotFound = 404;
            var respondTextNotFound = "NotFound";

            var response = new StringBuilder();

            if (routesByMethod[httpRequestMethod].Contains(httpRequestPath))
            {
                response.AppendLine($"{httpRequestprotocol.ToUpper()} {statusCodeSuccess + " " + respondTextSuccess}");
                response.AppendLine($"Content-Length: {respondTextSuccess.Length}");
                response.AppendLine("Content-Type: text/plain");
                response.AppendLine();
                response.AppendLine($"{respondTextSuccess}");
            }
            else
            {
                response.AppendLine($"{httpRequestprotocol.ToUpper()} {statusCodeNotFound + " " + respondTextNotFound}");
                response.AppendLine($"Content-Length: {respondTextNotFound.Length}");
                response.AppendLine("Content-Type: text/plain");
                response.AppendLine();
                response.AppendLine($"{respondTextNotFound}");
            }

            Console.WriteLine(response.ToString().Trim());

        }
    }
}
