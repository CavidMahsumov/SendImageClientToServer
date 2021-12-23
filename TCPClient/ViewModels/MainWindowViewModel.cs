using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using TCPClient.Command;
using TCPClient.Extentations;

namespace TCPClient.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {

        public RelayCommand SelectBtnCommand { get; set; }
        public RelayCommand SendBtnCommand { get; set; }


        byte[] b;


        public MainWindowViewModel(MainWindow mainWindow)
        {

            

            SelectBtnCommand = new RelayCommand((sender) =>
            {

                var open = new System.Windows.Forms.OpenFileDialog();

                open.Multiselect = false;
                open.Filter = "Image file (*.png)|*.png";

                open.ShowDialog();


               b=ImageHelper.GetBytesOfImage(open.FileName);






        });
            SendBtnCommand = new RelayCommand((sender) =>
            {
                Task.Run(() => {

                    var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    var ipAdress = IPAddress.Parse("192.168.1.103");
                    var port = 27002;

                    var ep = new IPEndPoint(ipAdress, port);

                    try
                    {
                        socket.Connect(ep);

                        if (socket.Connected)
                        {
                            Console.WriteLine("Connected to the Server . . . .");
                            MessageBox.Show("Connected");

                        }
                        else
                        {
                            Console.WriteLine("Can not Connected to the Server");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Can Not connect to the Server");
                        Console.WriteLine(ex.Message);
                    }

                    while (true)
                    {

                        socket.Send(b);
                    }



                });
              
            });

        }
    }
}
