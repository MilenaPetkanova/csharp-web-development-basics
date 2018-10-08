namespace SIS.HTTP.Responses
{
    using System.Linq;
    using System.Text;
    using SIS.HTTP.Common;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Extensions;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Cookies;

    public class HttpResponse : IHttpResponse
    {
        public HttpResponse() { }

        public HttpResponse(HttpResponseStatusCode statusCode)
        {
            CoreValidator.ThrowIfNull(statusCode, nameof(statusCode));

            this.Headers = new HttpHeaderCollection();
            this.Content = new byte[0];
            this.StatusCode = statusCode;
            this.Cookies = new HttpCookieCollection();
        }

        public HttpResponseStatusCode StatusCode { get; set;  }

        public IHttpHeaderCollection Headers { get; }

        public IHttpCookieCollection Cookies { get; }

        public byte[] Content { get; set; }
        
        public void AddHeader(HttpHeader header)
        {
            CoreValidator.ThrowIfNull(header, nameof(header));
            this.Headers.Add(header);
        }

        public void AddCookie(HttpCookie cookie)
        {
            CoreValidator.ThrowIfNull(cookie, nameof(cookie));
            this.Cookies.Add(cookie);
        }

        public byte[] GetBytes()
        {
            return Encoding.UTF8.GetBytes(this.ToString()).Concat(this.Content).ToArray();
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result
                .Append($"{GlobalConstants.HttpOneProtocolFragment} {this.StatusCode.GetResponseLine()}").Append(GlobalConstants.HttpNewLine)
                .Append(this.Headers).Append(GlobalConstants.HttpNewLine);

            if (this.Cookies.HasCookies())
            {
                result.AppendLine($"Set-Cookie: {this.Cookies}");
            }

            result.Append(GlobalConstants.HttpNewLine);

            return result.ToString();
        }
    }
}