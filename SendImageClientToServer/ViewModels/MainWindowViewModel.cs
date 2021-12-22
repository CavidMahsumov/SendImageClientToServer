using SendImageClientToServer.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SendImageClientToServer.ViewModels
{
   public class MainWindowViewModel:BaseViewModel
    {
        public RelayCommand a { get; set; }
        public ObservableCollection<Image> images { get; set; }
        public MainWindowViewModel(MainWindow mainWindow)
        {

            images = new ObservableCollection<Image>();

            a = new RelayCommand((sender) => {

                Task.Run(() =>
                {

                    var ipAdress = IPAddress.Parse("10.2.27.31");
                    int port = 27002;
                    using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                    {
                        var ep = new IPEndPoint(ipAdress, port);
                        socket.Bind(ep);

                        socket.Listen(10);


                        while (true)
                        {
                            var client = socket.Accept();
                            MessageBox.Show("Client Accapted");

                            Task.Run(() => {



                                var lenght = 0;
                                var bytes = new byte[1024];

                                do
                                {
                                    lenght = client.Receive(bytes);
                                    var msg = Encoding.UTF8.GetString(bytes, 0, lenght);
                                    Image x = (Bitmap)((new ImageConverter()).ConvertFrom(bytes));
                                    images.Add(x);
                                    MessageBox.Show("salam");
                                    mainWindow.Listbox.ItemsSource=images;
                                    
                                                                        

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

                });

             



            });
               




     

              
            
        }
    }
}
