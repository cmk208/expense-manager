using Swashbuckle.Swagger;
using System.Web.Http.Description;

namespace EM.API.Codes
{
    public class RequestContentTypeOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var requestAttributes = apiDescription.GetControllerAndActionAttributes<SwaggerRequestContentTypeAttribute>();

            foreach (var requestAttribute in requestAttributes)
            {
                if (requestAttribute.Exclusive)
                {
                    operation.consumes.Clear();
                }

                operation.consumes.Add(requestAttribute.RequestType);
            }
        }
    }
}