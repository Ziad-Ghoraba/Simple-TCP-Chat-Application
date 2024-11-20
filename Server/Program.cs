using SimpleTCP;
using System;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    internal class Program
    {
        static Socket connectedClient = null; // Store the single connected client as a Socket

        static void Main(string[] args)
        {
            var server = new SimpleTcpServer();

            // Start the server on port 5000
            server.Start(5000);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Server started on port 5000...");
            Console.WriteLine("Press Ctrl+C to stop the server.");
            Console.ResetColor();

            // Handle when a client connects
            server.ClientConnected += (sender, e) =>
            {
                connectedClient = e.Client; // Store the connected client Socket
                Console.WriteLine($"Client ({e.Client.RemoteEndPoint}) connected!");
                Console.WriteLine("-----------------------------------------------");
            };

            // Handle when a client disconnects
            server.ClientDisconnected += (sender, e) =>
            {
                if (e.Client == connectedClient)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    connectedClient = null; // Clear the connected client when disconnected
                    Console.WriteLine($"Client ({e.Client.RemoteEndPoint}) disconnected!");
                    Console.ResetColor();

                }
            };

            // Handle when data is received from the client
            server.DataReceived += (sender, e) =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                var msg = Encoding.UTF8.GetString(e.Data); // Convert received data to string
                Console.WriteLine($"Client: {msg}"); // Display message from client
                Console.ResetColor();

            };

            // Server input loop
            while (true)
            {
                var input = Console.ReadLine(); // Read server input
                if (string.IsNullOrEmpty(input)) continue; // Skip empty input

                if (connectedClient == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No clients connected.");
                    Console.ResetColor();
                    continue;
                }

                var data = Encoding.UTF8.GetBytes($"Server: {input}");

                try
                {
                    connectedClient.Send(data); // Send data to the single connected client
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error sending to client: {ex.Message}");
                    Console.ResetColor();
                }
            }
        }
    }
}
