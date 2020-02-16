using System;

namespace ElasticSearchDotNet
{
    public class Api
    {
        public static ApiResponse<T> Created<T>(T value)
        {
            return new ApiResponse<T>(value);
        }

        public static ApiResponse<Object> CreateInvalidResource(string code, string message)
        {
            return new ApiResponse<Object>(code, message);
        }
    }

    public class ApiResponse<T>
    {
        public T Data { get; private set; }
        public string Code { get; private set; }
        public string Message { get; private set; }

        public ApiResponse(T data)
        {
            Data = data;
        }

        public ApiResponse(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}