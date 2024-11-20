using SimpleTCP;
#region Simple TCP
//This imports the SimpleTCP library, which provides a simplified wrapper for working with TCP connections.
//It helps establish communication between a client and a server using sockets.
#endregion
using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args) 
        {

            var client = new SimpleTcpClient();

            // Handle incoming data from the server
            client.DataReceived += (sender, e) =>
            {
                var msg = Encoding.UTF8.GetString(e.Data); // Convert server data from byte array to string
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(msg); // Display the message from the server
                Console.ResetColor();
            };

            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                // Connect to the server
                client.Connect("192.168.1.14", 5000); // Replace with your server's IP address
                Console.WriteLine("Connected to the server.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error connecting to the server: {ex.Message}");
                Console.ResetColor();
                return;
            }

            // Chat loop for client to send messages to the server
            while (true)
            {

                var input = Console.ReadLine(); // Read client input
                if (string.IsNullOrEmpty(input)) continue; // Skip empty input

                try
                {
                    client.Write(input); // Send message to the server
                    //! This is how it works explicitly -> client.Write(Encoding.UTF8.GetBytes(input));

                     // After sending, display an acknowledgment message on the client side
                    //Console.WriteLine("Message sent successfully!");
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error sending message: {ex.Message}");
                    Console.ResetColor();

                }
            }
        }
    }
}
