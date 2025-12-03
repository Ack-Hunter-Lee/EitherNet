// See https://aka.ms/new-console-template for more information
using EitherNet.Results;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;

Console.WriteLine("Hello, World!");

List<string> errs=new List<string>();
if (!GetIpInfo().UnWrap(out string ip, err => errs.Add(err)))
    return;
if (!GetPortInfo().UnWrap(out int port, err => errs.Add(err)))
    return;
var tcpClient = new TcpClient();
await tcpClient.ConnectAsync(ip, port);




static IResult<string,string> GetIpInfo() {
    return Success<string, string>.Of("127.0.0.1");
}
static IResult<int, string> GetPortInfo()
{
    return Success<int, string>.Of(8080);
}


