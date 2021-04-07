using System;

namespace EM.API.Codes
{
    /// <summary>
    /// SwaggerRequestContentTypeAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SwaggerRequestContentTypeAttribute : Attribute
    {
        /// <summary>
        /// SwaggerRequestContentTypeAttribute
        /// </summary>
        /// <param name="requestType"></param>
        public SwaggerRequestContentTypeAttribute(string requestType)
        {
            RequestType = requestType;
        }
        /// <summary>
        /// Request Content Type
        /// </summary>
        public string RequestType { get; private set; }

        /// <summary>
        /// Remove all other Request Content Types
        /// </summary>
        public bool Exclusive { get; set; }
    }
}