using System;


namespace EitherNet.Results
{
    public interface IResult<T, TError>
    {
        bool Success();
        TResult UnWrap<TResult>(Func<T, TResult> onSuccess, Func<TError, TResult> onError);
        bool UnWrap(Action<T> onSuccess, Action<TError> onError);

        bool UnWrap(out T val, Action<TError> onError);

        bool UnWrap(out T val, out TError err);
        object UnWrap();
    }

    public class Success<T, TError> : IResult<T, TError>
    {
        private readonly T _value;
        protected Success(T value,bool breaked)
        {
            _value = value;
        }

        public static IResult<T, TError> Of(T value,bool isBreaked=false) => new Success<T, TError>(value, isBreaked);

        public TResult UnWrap<TResult>(Func<T, TResult> onSuccess, Func<TError, TResult> onError) => onSuccess(_value);

        public bool UnWrap(Action<T> onSuccess, Action<TError> onError) {
            onSuccess(_value);
            return true;
        }

        public bool UnWrap(out T val, Action<TError> onError)
        {
            val = _value;
            return true;
        }

        public bool UnWrap(out T val, out TError err)
        {
            val = _value;
            err = default;
            return true;
        }

        public object UnWrap() => _value;

        bool IResult<T, TError>.Success() => true;

    }

    public class Error<T, TError> : IResult<T, TError>
    {
        private readonly TError _error;
        protected Error(TError error)
        {
            _error = error;
        }
        public static IResult<T, TError> Of(TError error) => new Error<T, TError>(error);

        public TResult UnWrap<TResult>(Func<T, TResult> onSuccess, Func<TError, TResult> onError) => onError(_error);

        public bool UnWrap(Action<T> onSuccess, Action<TError> onError) {
            onError(_error);
            return false;
        }

        public bool UnWrap(out T val, Action<TError> onError)
        {
            val = default;
            onError(_error);
            return false;
        }

        public bool UnWrap(out T val, out TError err)
        {
            val = default;
            err= _error;
            return false;
        }

        public object UnWrap() => _error;

        bool IResult<T, TError>.Success() => false;
    }

    public interface IResult<TError> : IResult<Unit, TError>
    {
    }

    public class Unit
    {
        private Unit() { }
        public static readonly Unit Instance = new Unit();
    }

    public class Success<TError> : Success<Unit, TError>, IResult<TError>
    {
        private Success(Unit value,bool isBreaked) : base(value, isBreaked) { }
        public new static IResult<TError> Of(Unit value = null,bool isBreaked = false) => new Success<TError>(Unit.Instance, isBreaked);
    }

    public class Error<TError> : Error<Unit, TError>, IResult<TError>
    {
        private Error(TError error) : base(error) { }
        public new static IResult<TError> Of(TError error) => new Error<TError>(error);
    }


}
