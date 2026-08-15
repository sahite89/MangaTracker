using MangaTracker.Application.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Responses
{
    public class BaseResponse
    {
        public BaseResponse()
        {
            Success = true;
        }

        public BaseResponse(string message)
        {
            Success = true;
            Message = message;
        }

        public BaseResponse(string message, bool success)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string>? ValidationErrors { get; set; } = new List<string>();
        public ErrorCode ErrorCode { get; set; } = ErrorCode.None;
    }
}
