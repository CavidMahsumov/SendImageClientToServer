using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SendImageClientToServer.ViewModels
{
   public class MainWindowViewModel:BaseViewModel
    {

        public MainWindowViewModel(MainWindow mainWindow)
        {
            var ipAdress = IPAddress.Parse("10.1.18.42");
            int port = 27001;
            using (var socket=new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp))
            {
                var ep = new IPEndPoint(ipAdress, port);
                socket.Bind(ep);

                socket.Listen(10);
                Console.WriteLine($"Listening  on {socket.LocalEndPoint}");


                while (true)
                {
                    var client = socket.Accept();

                    Task.Run(() => {

                        Console.WriteLine($"{client.RemoteEndPoint} connected  . . . .");

                        var lenght = 0;
                        var bytes = new byte[1024];

                        do
                        {
                            lenght = client.Receive(bytes);
                            var msg = Encoding.UTF8.GetString(bytes, 0, lenght);
                            Console.WriteLine($"Client : {client.RemoteEndPoint} : {msg}");
                            if (msg == "ok")
                            {

                                client.Shutdown(SocketShutdown.Both);
                                client.Dispose();
                                break;

                            }
                        } while (true);



                    });


                }
            }
        }
    }
}
