using EitherNet.Processes;
using System;
using System.Threading.Tasks;

namespace EitherNet.Results
{
    public static class ResultExtension
    { 

        public static IResult<T2, TErr> Than<T1,T2,TErr>(this IResult<T1, TErr> lastResult, Func<T1, IResult<T2, TErr>> next)
        {
            if (!lastResult.Success())
                return Error<T2, TErr>.Of( (TErr)lastResult.UnWrap() );
            return next.Invoke((T1)lastResult.UnWrap());
        }

        /// <summary>
        /// Result->Task<Result>
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TErr"></typeparam>
        /// <param name="lastResult"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public static async Task<IResult<T2, TErr>> AsyncThan<T1, T2, TErr>(this IResult<T1, TErr> lastResult, Func<T1,Task<IResult<T2, TErr>> > next)
        {
            if (!lastResult.Success())
                return  Error<T2, TErr>.Of((TErr)lastResult.UnWrap())  ;
            return await next.Invoke((T1)lastResult.UnWrap());
        }

        /// <summary>
        /// Task<Result> -> Task<Result>
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TErr"></typeparam>
        /// <param name="lastResultTask"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public static async Task<IResult<T2, TErr>> AsyncThan<T1, T2, TErr>(this Task<IResult<T1, TErr>>lastResultTask, Func<T1, Task<IResult<T2, TErr>>> next)
        {
            var lastResult= await lastResultTask;
            if (!lastResult.Success())
                return Error<T2, TErr>.Of((TErr)lastResult.UnWrap());
            return await next.Invoke((T1)lastResult.UnWrap());
        }


        public static IResult<T, TErr> ErrorIf<T, TErr>(this IResult<T, TErr> result,Func<T,bool> predict, TErr err) {
            if (!result.Success())
                return result;
            if (predict((T)result.UnWrap()))
                return Error<T, TErr>.Of(err);
            return result;
        }
        public static async Task<IResult<T, TErr>> ErrorIf<T, TErr>(this Task<IResult<T, TErr>> resultTask, Func<T, bool> predict, TErr err)
        {
            var result =await resultTask;
            if (!result.Success())
                return result;
            if (predict((T)result.UnWrap()))
                return Error<T, TErr>.Of(err);
            return result;
        }

        public static IProcess<T, TErr> AsProcess<T, TErr>(this IResult<T, TErr> result) {
            if (result.Success())
                return Continue<T, TErr>.Of((T)result.UnWrap());
            return Complete<T, TErr>.Of((TErr)result.UnWrap());
        }
        public static async Task<IProcess<T, TErr>> AsProcessAsync<T, TErr>(this Task<IResult<T, TErr>> resultTask)
        {
            var result = await resultTask;
            if (result.Success())
                return Continue<T, TErr>.Of((T)result.UnWrap());
            return Complete<T, TErr>.Of((TErr)result.UnWrap());
        }


        public static IResult<T, TErr> EndIf<T, TErr>(this IResult<T, TErr> result, Func<T, bool> predict, TErr err) {
            //result一定是成功的，所以不需要检查是否成功
            //if (!result.Success())
            //    return result;
            if (predict((T)result.UnWrap()))
                return Error<T, TErr>.Of(err);
            return result;

        }

    }
}
