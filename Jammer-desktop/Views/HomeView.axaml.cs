using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using Jammer_desktop.ViewModels;
using Jammer.Core;
using Org.BouncyCastle.Asn1.TeleTrust;
using Serilog;

namespace Jammer_desktop.Views;

public partial class HomeView : UserControl
{
    private static string serverIp = "";

    private static int serverPort = 7777;

    private static Socket tcpSocket = null!;

    private static byte[] key = new byte[32];

    private static ITun _tun = null!;

    private static IRoute? _route;

    private static ILogger _clientTcpLogger = Log.ForContext("SourceContext", "Client-Tcp");

    private static readonly string ipFilePath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Jammer", "server-ip.txt");

    private static bool isConnected;
    

    static HomeView()
    {
        AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
        {
            _route?.Clean(serverIp, "JammerTun");
        };

        //закрытие окна
        AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) =>
        {
            _route?.Clean(serverIp, "JammerTun");
        };
    }

    public HomeView()
    {
        InitializeComponent();
        
        if (File.Exists(ipFilePath))
        {
            ServerIpTextBox.Text = File.ReadAllText(ipFilePath);
        }
        else
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ipFilePath));
        }
    }

    private async void ConnectButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (!isConnected)
        {
            serverIp = ServerIpTextBox.Text;
            File.WriteAllText(ipFilePath, serverIp);
        
            var homeviewmodel = (HomeViewModel)DataContext;
            homeviewmodel._logText = "";
            ChangeButtonToInactive();
        
            try
            {
                await Task.Run(ConnectAsync);
            }
            catch (Exception ex)
            {
                _clientTcpLogger.Error("Ошибка: {Ex}", ex.Message);
                isConnected = false;
                _route?.Clean(serverIp, "JammerTun");
                WinTun.Close();
                ConnectButton.IsEnabled = true;
            }
        }
        else
        {
            isConnected = false;
            tcpSocket.Close();
            _route?.Clean(serverIp, "JammerTun");
            WinTun.Close();
            ConnectButton.IsEnabled = true;
            ConnectButton.Content = "Connect";
            ConnectButton.Background = new SolidColorBrush(Color.Parse("#C9444A"));
        }
        
    }

    async Task ConnectAsync()
    {
        _route = IRoute.CreateRoute();

        await CreateTcpConnection();
        int clientId = await ReceiveClientIdAsync();

        string tunnelIp = $"172.16.0.{clientId}";

        _tun = ITun.CreateTun();

        _tun.InitializeTunnel();
        
        _tun.StartSession();

        _tun.ConfigureIpAddress(tunnelIp, 24);

        _route.Route(serverIp, tunnelIp, null, null);

        _route.DNS();

        isConnected = true;

        await Task.WhenAll(ReceiveMessageAsync(), SendMessageAsync());
    }

    async static Task CreateTcpConnection()
    {
        tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        tcpSocket.NoDelay = true;
        var serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);

        await tcpSocket.ConnectAsync(serverEndPoint);
        _clientTcpLogger.Information("============TCP соедение установлено=========");

        //вычисляем AES ключ по общему секрету
        Crypto.ECDH ecdh = new Crypto.ECDH();
        key = await ecdh.CreateAesKey(tcpSocket);
    }

    async static Task ReceiveMessageAsync()
    {
        while (isConnected)
        {
            byte[] buffer = await Frame.ReadFrameAsync(tcpSocket);

            var data = Crypto.AES.Decrypt(buffer, key);
            _tun.SendPacket(data);

            _clientTcpLogger.Debug("Получено {Bytes} байт", data.Length);
            isConnected = true;
        }
    }

    async Task<int> ReceiveClientIdAsync()
    {
        byte[] buffer = await Frame.ReadFrameAsync(tcpSocket);

        var data = Crypto.AES.Decrypt(buffer, key);

        _clientTcpLogger.Information("Получен идентификатор клиента: {id}", BitConverter.ToInt32(data));

        Dispatcher.UIThread.Post(() =>
        {
            ConnectButton.IsEnabled = true;
            ConnectButton.Background = new SolidColorBrush(Colors.Green);
            ConnectButton.Content = "Connected";
        });

        return BitConverter.ToInt32(data);
    }

    async static Task SendMessageAsync()
    {
        while (isConnected)
        {
            var input = _tun.ReceivePacket();

            //fix: пофиксить загрузку одного ядра в 100% через WintunGetReadWaitEvent (if (input == null) continue бесконечно по кругу крутиться и забивает весь поток)
            if (input == null) continue;

            var data = Crypto.AES.Encrypt(input, key);
            await Frame.WriteFrameAsync(tcpSocket, data);

            _clientTcpLogger.Debug("Отправлено {Bytes} байт", data.Length);
        }
    }

    private void ChangeButtonToInactive()
    {
        ConnectButton.IsEnabled = false;
        ConnectButton.Content = "Connecting...";
    }
    
}
