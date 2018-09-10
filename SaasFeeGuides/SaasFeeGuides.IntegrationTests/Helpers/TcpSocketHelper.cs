using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace SaasFeeGuides.IntegrationTests.Helpers
{
    public static class TcpSocketHelper
    {
        public static int GetAvailablePortInRange(int min, int max)
        {
            if (min <= 0 || min >= max)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "Invalid port range");
            }

            var properties = IPGlobalProperties.GetIPGlobalProperties();

            var availablePorts = Enumerable.Range(min, max - min)
                .Except(properties.GetActiveTcpConnections().Select(atc => atc.LocalEndPoint.Port)
                    .Concat(properties.GetActiveTcpListeners().Select(atl => atl.Port))
                    .Concat(properties.GetActiveUdpListeners().Select(aul => aul.Port)))
                .ToArray();

            if (availablePorts.Any())
            {
                return availablePorts.First();
            }
            throw new InvalidOperationException($"No ports available in range {min}:{max}");
        }

        public static string GetNextLocalhostUrl(int portMin, int portMax, string @interface = null)
        {
            return $"http://{@interface ?? "localhost"}:{GetAvailablePortInRange(portMin, portMax)}";
        }

        public static string GetDefaultRouteInterface(string lookup = "8.8.8.8")
        {
            var remoteIp = IPAddress.Parse(lookup);
            var remoteEndPoint = new IPEndPoint(remoteIp, 0);
            var socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Dgram,
                ProtocolType.Udp);
            var localEndPoint = QueryRoutingInterface(socket, remoteEndPoint);
            return localEndPoint.Address.ToString();
        }

        private static IPEndPoint QueryRoutingInterface(
            Socket socket,
            EndPoint remoteEndPoint)
        {
            var address = remoteEndPoint.Serialize();

            var remoteAddrBytes = new byte[address.Size];
            for (var i = 0; i < address.Size; i++)
            {
                remoteAddrBytes[i] = address[i];
            }

            var outBytes = new byte[remoteAddrBytes.Length];
            socket.IOControl(
                IOControlCode.RoutingInterfaceQuery,
                remoteAddrBytes,
                outBytes);
            for (var i = 0; i < address.Size; i++)
            {
                address[i] = outBytes[i];
            }

            var ep = remoteEndPoint.Create(address);
            return (IPEndPoint)ep;
        }

    }
}
