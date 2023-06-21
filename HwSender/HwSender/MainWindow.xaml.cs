using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HwSender;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public void SendData(string data)
    {
        Uri uri = new Uri("tcp://4.tcp.eu.ngrok.io");
        data += '\0';
        byte[] requestBytes = Encoding.ASCII.GetBytes(data);

        using Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(uri.Host, 17403);
        //socket.Connect("localhost", 8889);

        // Send the request.
        // For the tiny amount of data in this example, the first call to Send() will likely deliver the buffer completely,
        // however this is not guaranteed to happen for larger real-life buffers.
        // The best practice is to iterate until all the data is sent.
        int bytesSent = 0;
        while (bytesSent < requestBytes.Length)
        {
            bytesSent += socket.Send(requestBytes, bytesSent, requestBytes.Length - bytesSent, SocketFlags.None);
        }
        socket.Disconnect(false);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        SendDataModel data = new SendDataModel
        {
            LocationId = 1,
            Signal = true
        };
        SendData(JsonSerializer.Serialize(data));
    }
}

