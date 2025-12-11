// See https://aka.ms/new-console-template for more information
using EitherNet.Results;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;

string errMsg = string.Empty;
try
{

    if (!GetIpInfo().UnWrap(out string ip, err => errMsg = err))
        return;
    if (!GetPortInfo().UnWrap(out int port, err => errMsg = err))
        return;
    if (!(await CreateTcpClient(ip, port))
        .UnWrap(out TcpClient tcpClient, err => errMsg = err))
        return;
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
finally
{
    Console.WriteLine(errMsg ?? "成功");
    Console.WriteLine("执行结束");
    Console.ReadLine();
}


static IResult<string, string> GetIpInfo()
{
    return Success<string, string>.Of("127.0.0.1");
}
static IResult<int, string> GetPortInfo()
{
    return Success<int, string>.Of(8080);
}
static async Task<IResult<TcpClient, string>> CreateTcpClient(string ip, int port)
{
    try
    {
        var tcpClient = new TcpClient();
        await tcpClient.ConnectAsync(ip, port);
        return Success<TcpClient, string>.Of(tcpClient);
    }
    catch (Exception ex)
    {
        return Error<TcpClient, string>.Of(ex.Message);
    }
}


