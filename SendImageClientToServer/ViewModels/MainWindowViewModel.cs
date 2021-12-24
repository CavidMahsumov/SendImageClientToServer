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
                    Task.Run(() =>
                    {
                        var ipAdress = IPAddress.Parse("10.2.27.31");
                        int port = 27002;
                        using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                        {
                            var ep = new IPEndPoint(ipAdress, port);
                            socket.Bind(ep);

                            socket.Listen(10);
                            string path = String.Empty;
                            byte[] bytes2 =new byte[1000000] ;
                            byte[] bytes=new byte[1024];

                            while (true)
                            {
                                var client = socket.Accept();
                                MessageBox.Show("Client Accapted");



                                do
                                {

                                    Task.Run(() => {


                                        bytes = new byte[1024];
                                        var lenght = 0;

                                        lenght = client.Receive(bytes);
                                        Array.Copy(bytes, 0, bytes2, 0, lenght);


                                    });

                                       
                                    


                                        //var msg = Encoding.UTF8.GetString(bytes, 0, lenght);
                                        //Image x = (Bitmap)((new ImageConverter()).ConvertFrom(bytes));
                                        //images.Add(x);
                                        //MessageBox.Show("salam");

                                        //images.Add(new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute)));

                                       


                                    
                                } while (bytes==null);
                                path = ImageHelper.GetImagePath(bytes2, 10);

                                images.Add(new Images { ImagePath = path });

                                Application.Current.Dispatcher.Invoke(() =>
                                {

                                    mainWindow.myListView.ItemsSource = images;

                                });










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
