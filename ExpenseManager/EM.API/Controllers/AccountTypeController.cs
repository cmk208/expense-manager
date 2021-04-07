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
    public class AccountTypeController : ApiController
    {
        [SwaggerResponse(HttpStatusCode.OK, "Success", Type = typeof(APIResponse<List<Models.AccountType>>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Internal Server Error", Type = null)]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [Route("api/AccountType/GetAccountTypes")]
        [HttpGet]
        public HttpResponseMessage GetAccountTypes()
        {
            try
            {
                using (var context = new EMContext())
                {
                    context.Configuration.LazyLoadingEnabled = false;

                    List<Models.AccountType> accountTypes = context.AccountTypes.Select(x => new Models.AccountType()
                    {
                        AccountTypeID = x.AccountTypeID,
                        AccountTypeName = x.AccountTypeName
                    }).ToList();

                    APIResponse<List<Models.AccountType>> apiResponse = new APIResponse<List<Models.AccountType>>
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        StatusRemark = "",
                        Content = accountTypes
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