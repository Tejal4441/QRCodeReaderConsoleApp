using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Configuration;

namespace QRCodeReaderConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter Path with File Name(PNG, GIF or JP(E)G) \n(E.g." + @"C:\Users\c-tejal.khandelwal\Downloads\TestTejal.png) : " );
            var path = @"" + Console.ReadLine();

            if (Directory.Exists(Path.GetDirectoryName(path)))
            {
                ReadQRCode(path).Wait();
            }
            Console.Write("\nPress any key to continue... ");
            Console.ReadKey();
        }

        /// <summary>
        /// Reading QR code
        /// </summary>
        /// <param name="path">The file path with the file name</param>
        /// <returns></returns>
        static async Task ReadQRCode(string path)
        {
            var url = "http://api.qrserver.com/v1/read-qr-code/";
            var baseURL = "http://api.qrserver.com/";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseURL);
            MultipartFormDataContent form = new MultipartFormDataContent();

            var stream = new FileStream(path, FileMode.Open);
            HttpContent content = new StreamContent(stream);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                FileName = Path.GetFileName(path),
                
            };

            form.Add(content);
            HttpResponseMessage response = null;

            try
            {
                response = (client.PostAsync(url, form)).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            var result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine("\nResult: " + result);
            
        }
    }
}