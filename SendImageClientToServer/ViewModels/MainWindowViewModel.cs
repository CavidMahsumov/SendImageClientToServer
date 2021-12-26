using SendImageClientToServer.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
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
        static TcpListener listener = null;
        static BinaryWriter bw = null;
        static BinaryReader br = null;
        int count=0;
        public MainWindowViewModel(MainWindow mainWindow)
        {

            images = new List<Images>();

            a = new RelayCommand((sender) =>
            {
                Task.Run(() =>
                {
                    var ip = IPAddress.Parse("192.168.1.103");
                    var ep = new IPEndPoint(ip, 27001);
                    listener = new TcpListener(ep);
                    listener.Start();

                    while (true)
                    {
                        var client = listener.AcceptTcpClient();
                        MessageBox.Show($"{client.Client.RemoteEndPoint} connected");
                        byte[] bytes = new byte[50000000];
                        Task.Run(() =>
                        {
                            var reader = Task.Run(() =>
                            {
                                ++count;
                                var stream = client.GetStream();
                                br = new BinaryReader(stream);
                                bw = new BinaryWriter(stream);
                                while (true)
                                {
                                    bytes = br.ReadBytes(count: 5000000);
                                    var msg = br.ReadString();
                                    Path = ImageHelper.GetImagePath(buffer: bytes, counter: count);
                                    break;

                                }
                                App.Current.Dispatcher.Invoke(() =>
                                {
                                    mainWindow.myListView.Items.Add(new Images { ImagePath=Path });

                                });
                            });

                        });



                    }
                });

            });














        }
    }
}
