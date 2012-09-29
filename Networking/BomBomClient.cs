namespace AirHockey.Recognition.Client.Networking
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    static class NetworkConstants
    {
        public const byte ClientType = 5;
    }

    static class ServerCommands
    {
        public const byte AccelerometerData = 0;

        public const byte Init = 1;

        public const byte Name = 2;

        public const byte Notification = 3;
    }

    public class InitCommand 
    {
        public byte[] Serialize()
        {
            var data = new byte[]
                {
                    ServerCommands.Init,
                    NetworkConstants.ClientType
                };

            return data;
        }
    }

    public class CommandRepository
    {
        public InitCommand GetInitCommand()
        {
            return new InitCommand();
        }
    }

    public class BomBomClient : IDisposable
    {
        private const string ServerIpAddress = "192.168.1.89";
        private const int ServerPort = 9595;

        private readonly TcpClient tcpClient;

        private readonly CommandRepository commandRepository;

        private NetworkStream networkStream;

        public BomBomClient()
        {
            this.commandRepository = new CommandRepository();
            this.tcpClient = new TcpClient();
        }

        public Task Connect()
        {
            var result = tcpClient.ConnectAsync(ServerIpAddress, ServerPort);

            result.ContinueWith(
                it =>
                    {
                        if (it.Status == TaskStatus.RanToCompletion)
                        {
                            networkStream = tcpClient.GetStream();
                        }
                    });

            return result;
        }

        public void Dispose()
        {
            if (networkStream != null)
            {
                networkStream.Dispose();
            }
        }

        public Task InitializeCommunication()
        {
            var initCommand = this.commandRepository.GetInitCommand();
            var data = initCommand.Serialize();
            return this.SendData(data);
        }

        public Task SendPolygons(IEnumerable<Polygon> polygons)
        {
            return null;
        }

        private Task SendData(byte[] data)
        {
            return this.networkStream.WriteAsync(data, 0, data.Length);
        }
    }

    public class Polygon
    {
        public Polygon(List<Point> points)
        {
            
        }
    }
}