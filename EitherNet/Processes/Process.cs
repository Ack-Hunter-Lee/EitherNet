

using EitherNet.Results;

namespace EitherNet.Processes
{
    public interface IProcess<Data,Err>
    {
        bool IsContinue { get;}
        object Unwrap();
    }
    public class Process {
        public static IProcess<Unit, Err> Start<Err>() {
            return Continue<Unit, Err>.Of(Unit.Instance);
        }
        public static IProcess<T, Err> Start<T,Err>(T data)
        {
            return Continue<T, Err>.Of(data);
        }
    }
    public class Continue<Data, Err> : IProcess<Data, Err>
    {
        private Data _data;
        private Continue(Data data) { _data = data; }
        public bool IsContinue => true;
        

        public object Unwrap() => _data;
        public static IProcess<Data, Err> Of(Data data) => new Continue<Data, Err>(data);


    }
    public class Complete<Data, Err> : IProcess<Data, Err>
    {
        private Err _err;
        private Complete(Err err) { _err = err; }
        public bool IsContinue => false;
        public static IProcess<Data, Err> Of(Err err) => new Complete<Data, Err>(err);

        public object Unwrap() => _err;
    }
}
