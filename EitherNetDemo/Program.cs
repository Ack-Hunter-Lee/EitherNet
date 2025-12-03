// See https://aka.ms/new-console-template for more information
using EitherNet.Results;
using System.Runtime.CompilerServices;

Console.WriteLine("Hello, World!");
List<string> errs=new List<string>();
if (!GetIpInfo().UnWrap(out string ip, err => errs.Add(err)))
    return;
if (!GetPortInfo().UnWrap(out int port, err => errs.Add(err)))
    return;


static IResult<string,string> GetIpInfo() {
    return Success<string, string>.Of("127.0.0.1");
}
static IResult<int, string> GetPortInfo()
{
    return Success<int, string>.Of(8080);
}


