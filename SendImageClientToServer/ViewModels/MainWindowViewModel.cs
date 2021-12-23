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
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TCPClient.Extentations;
using TCPClient.Model;

namespace SendImageClientToServer.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public RelayCommand a { get; set; }
        public List<Images> images { get; set; }
        public string Path { get; set; }
        public MainWindowViewModel(MainWindow mainWindow)
        {

            images = new List<Images>();

            a = new RelayCommand((sender) =>
            {

                try
                {
                    Task.Run(() => { 
                    var ipAdress = IPAddress.Parse("192.168.1.103");
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





                            var lenght = 0;
                            var bytes = new byte[2999];

                            
                                lenght = client.Receive(bytes);


                                var path = ImageHelper.GetImagePath(bytes, 10);


                                //var msg = Encoding.UTF8.GetString(bytes, 0, lenght);
                                //Image x = (Bitmap)((new ImageConverter()).ConvertFrom(bytes));
                                //images.Add(x);
                                //MessageBox.Show("salam");

                                //images.Add(new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute)));

                                images.Add(new Images {  ImagePath=path });

                                Application.Current.Dispatcher.Invoke(() => {

                                    mainWindow.myListView.ItemsSource = images;
                                
                                });

                                if (bytes == null)
                                {

                                    client.Shutdown(SocketShutdown.Both);
                                    client.Dispose();
                                    break;

                                }
                        }
                    }
                    });
                   
                }
                catch (Exception)
                {

                }

                   

             });














        }
    }
}
