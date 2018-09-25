namespace _01_URL_Decode
{
    using System;
    using System.Net;

    public class UrlDecoder
    {
        public static void Main(string[] args)
        {
            var urlInput1 = "http://www.google.bg/search?q=C%23";
            var decodedUrl1 = WebUtility.UrlDecode(urlInput1);
            Console.WriteLine(decodedUrl1);

            var urlInput2 = "https://mysite.com/show?n%40m3= p3%24h0";
            var decodedUrl2 = WebUtility.UrlDecode(urlInput2);
            Console.WriteLine(decodedUrl2);

            var urlInput3 = "http://url-decoder.com/i%23de%25?id=23";
            var decodedUrl3 = WebUtility.UrlDecode(urlInput3);
            Console.WriteLine(decodedUrl3);
        }
    }
}
