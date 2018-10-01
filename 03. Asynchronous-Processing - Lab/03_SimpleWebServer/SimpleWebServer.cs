namespace _03_SimpleWebServer
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    public class SimpleWebServer
    {
        public static void Main(string[] args)
        {
            // Create a TcpListener object a start the connection task.

            IPAddress address = IPAddress.Parse("127.0.0.1");
            int port = 1300;
            TcpListener listener = new TcpListener(address, port);
            listener.Start();

            Console.WriteLine("Server started.");
            Console.WriteLine($"Listening on TCP clinets at 127.0.0.1:{port}");

            var task = Task.Run(() => ConnectWithTcpClientAsync(listener));
            task.Wait();
        }

        public static async Task ConnectWithTcpClientAsync(TcpListener listener)
        {
            while (true)
            {
                //Write a task to connect with the client

                Console.WriteLine("Waiting for client...");
                var client = await listener.AcceptTcpClientAsync();

                Console.WriteLine("Client connected.");

                // Read the request and print it on the console.

                var buffer = new byte[1024];
                client.GetStream().Read(buffer, 0, buffer.Length);

                var message = Encoding.ASCII.GetString(buffer);
                Console.WriteLine(message);

                // Send a greeting to the client.

                byte[] data = Encoding.ASCII.GetBytes("Hello from server!");
                client.GetStream().Write(data, 0, data.Length);

                // Close the connection.

                Console.WriteLine("Closing connection.");
                client.GetStream().Dispose();
            }
        }
    }
}
