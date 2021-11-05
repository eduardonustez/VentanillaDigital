using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.Transversal.HandlingError
{
    public class ApiResponse
    {
        public int StatusCode { get; }
        public string Message { get; }
        public object Object { get; set; }
        public ApiResponse(int statusCode, string message = null,object obj=null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
            Object = obj;
        }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            switch (statusCode)
            {
            case 400:
                    return "An error ocurred";
            case 404:
                return "Resource not found";
            case 500:
                return "An unhandled error occurred";
            default:
                return null;
        }
    }
}
}
