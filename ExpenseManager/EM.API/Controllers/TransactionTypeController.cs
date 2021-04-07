using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;
using EM.DB.Models;
using EM.API.Models;
using EM.API.Codes;

namespace EM.API.Controllers
{
    public class TransactionTypeController : ApiController
    {
        [SwaggerResponse(HttpStatusCode.OK, "Success", Type = typeof(APIResponse<List<Models.TransactionType>>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Internal Server Error", Type = null)]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [Route("api/TransactionType/GetTransactionTypes")]
        [HttpGet]
        public HttpResponseMessage GetTransactionTypes()
        {
            try
            {
                using (var context = new EMContext())
                {
                    context.Configuration.LazyLoadingEnabled = false;

                    List<Models.TransactionType> transactionTypes = context.TransactionTypes.Select(x => new Models.TransactionType()
                    {
                        TransactionTypeID = x.TransactionTypeID,
                        TransactionTypeName = x.TransactionTypeName
                    }).ToList();

                    APIResponse<List<Models.TransactionType>> apiResponse = new APIResponse<List<Models.TransactionType>>
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        StatusRemark = "",
                        Content = transactionTypes
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, apiResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);

                if (HttpContext.Current.IsDebuggingEnabled)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "");
                }
            }
        }
    }
}