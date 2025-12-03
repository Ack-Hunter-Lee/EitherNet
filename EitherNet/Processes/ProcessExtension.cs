using EitherNet.Results;
using System;
using System.Threading.Tasks;


namespace EitherNet.Processes
{
    public static class ProcessExtension
    {
        public static IProcess<T2, TErr> Than<T1, T2, TErr>(this IProcess<T1, TErr> last, Func<T1, IProcess<T2, TErr>> current, Action onPassBy = null)
        {
            if (!last.IsContinue)
            {
                onPassBy?.Invoke();
                return Complete<T2, TErr>.Of((TErr)last.Unwrap());
            }
            return current.Invoke((T1)last.Unwrap());
        }


        public static async Task<IProcess<T2, TErr>> ThanAsync<T1, T2, TErr>(this IProcess<T1, TErr> last, Func<T1, Task<IProcess<T2, TErr>>> current, Action onPassBy = null)
        {
            if (!last.IsContinue)
            {
                onPassBy?.Invoke();
                return Complete<T2, TErr>.Of((TErr)last.Unwrap());
            }
            return await current.Invoke((T1)last.Unwrap());
        }

        public static async Task<IProcess<T2, TErr>> ThanAsync<T1, T2, TErr>(this Task<IProcess<T1, TErr>> currentTask, Func<T1, Task<IProcess<T2, TErr>>> current, Action onPassBy = null)
        {
            var last = await currentTask;
            if (!last.IsContinue)
            {
                onPassBy?.Invoke();
                return Complete<T2, TErr>.Of((TErr)last.Unwrap());
            }
            return await current.Invoke((T1)last.Unwrap());
        }

        public static async Task<IProcess<Unit, TErr>> ThanAsync<T,TErr>(this Task<IProcess<T,TErr>> currentTask,params Func<T, Task<IProcess<Unit, TErr>>>[]thans)
        {
            var current = await currentTask;
            if (current.IsContinue)
            {
                foreach (var than in thans)
                {
                    await than((T)current.Unwrap());
                }
                return Continue<Unit, TErr>.Of(Unit.Instance);
            }
            return Complete<Unit, TErr>.Of((TErr)current.Unwrap());
        }




        //public static async Task<IProcess<T2, TErr>> ExcuteIfAsync<T1, T2, TErr>(this Task<IProcess<T1, TErr>> currentTask,
        //    Func<bool> condition,
        //    Func<T1, Task<IProcess<T2, TErr>>> current,
        //    Action onPassBy = null)
        //{
        //    var last = await currentTask;
        //    if (!condition())//没有达到条件则不执行
        //    {
        //        onPassBy?.Invoke();
        //        return last.IsContinue?Continue<T2, TErr>.Of((T2)last.Unwrap()): Complete<T2, TErr>.Of((TErr)last.Unwrap());
        //    }
        //    return await current.Invoke((T1)last.Unwrap());
        //}
        public static async Task<IProcess<Unit, TErr>> ExcuteIfAsync<T,TErr>(this Task<IProcess<T, TErr>> currentTask,
           Func<bool> condition,
           Func<Unit, Task<IProcess<Unit, TErr>>> current,
           Action onPassBy = null)
        {
            var last = await currentTask;
            if (condition())//达到条件则执行
            {
                return await current.Invoke(Unit.Instance);
            }
            onPassBy?.Invoke();
            return last.IsContinue ? Continue<Unit, TErr>.Of(Unit.Instance) : Complete<Unit, TErr>.Of((TErr)last.Unwrap());
        }


        public static IProcess<Data, Res> CompleteIf<Data, Res>(this IProcess<Data, Res> process, Func<Data, bool> predict, Res res, Action onPredicted=null)
        {
            if (!process.IsContinue)
                return Complete<Data, Res>.Of((Res)process.Unwrap());
            if (predict((Data)process.Unwrap()))
            {
                onPredicted();
                return Complete<Data, Res>.Of(res);
            }
            return process;
        }

        public static async Task<IProcess<Data, Res>> CompleteIf<Data, Res>(this Task<IProcess<Data, Res>> processTask, Func<Data, bool> predict, Res res, Action onPredicted=null)
        {
            var process = await processTask;
            if (!process.IsContinue)
                return Complete<Data, Res>.Of((Res)process.Unwrap());
            if (predict((Data)process.Unwrap())) {
                onPredicted();
                return Complete<Data, Res>.Of(res);
            } 
            return process;
        }

       

        public static async Task Finally<T, TErr>(this Task<IProcess<T, TErr>> lastTask, Action<IProcess<T, TErr>> action)
        {
            var last = await lastTask;
            action.Invoke(last);
        }
        public static void Finally<T, TErr>(this IProcess<T, TErr> last, Action<IProcess<T, TErr>> action)
        {
            action.Invoke(last);
        }


    }
}
