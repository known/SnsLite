namespace Known
{
    /// <summary>
    /// 业务逻辑返回的结果信息。
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 构造函数，创建一个带消息的实例。
        /// </summary>
        /// <param name="isValid">业务逻辑是否验证通过。</param>
        /// <param name="message">业务逻辑返回的消息。</param>
        public Result(bool isValid, string message)
        {
            IsValid = isValid;
            Message = message;
        }

        /// <summary>
        /// 取得业务逻辑是否验证通过。
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// 取得业务逻辑返回的消息。
        /// </summary>
        public string Message { get; private set; }

        public static Result Error(string message)
        {
            return new Result(false, message);
        }

        public static Result<T> Error<T>(string message)
        {
            return new Result<T>(false, message);
        }

        public static Result<T> Error<T>(string message, T data)
        {
            return new Result<T>(false, message, data);
        }

        public static Result Success(string message)
        {
            return new Result(true, message);
        }

        public static Result<T> Success<T>(string message, T data)
        {
            return new Result<T>(true, message, data);
        }
    }

    /// <summary>
    /// 业务逻辑返回的结果信息，并返回指定类型的对象。
    /// </summary>
    /// <typeparam name="T">指定的对象类型。</typeparam>
    public class Result<T> : Result
    {
        //// <summary>
        /// 构造函数，创建一个带消息的实例。
        /// </summary>
        /// <param name="isValid">业务逻辑是否验证通过。</param>
        /// <param name="message">业务逻辑返回的消息。</param>
        public Result(bool isValid, string message) : base(isValid, message) { }

        /// <summary>
        /// 构造函数，创建一个带指定类型对象的实例。
        /// </summary>
        /// <param name="isValid">业务逻辑是否验证通过。</param>
        /// <param name="message">业务逻辑返回的消息。</param>
        /// <param name="data">指定类型的返回对象。</param>
        public Result(bool isValid, string message, T data) : base(isValid, message)
        {
            Data = data;
        }

        /// <summary>
        /// 取得业务逻辑返回的指定类型对象。
        /// </summary>
        public T Data { get; private set; }
    }
}
