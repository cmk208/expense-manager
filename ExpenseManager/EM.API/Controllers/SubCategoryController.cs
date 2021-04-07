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
    public class SubCategoryController : ApiController
    {
        [SwaggerResponse(HttpStatusCode.OK, "Success", Type = typeof(APIResponse<List<Models.SubCategory>>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Internal Server Error", Type = null)]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [Route("api/SubCategory/GetSubCategories")]
        [HttpGet]
        public HttpResponseMessage GetSubCategories()
        {
            try
            {
                using (var context = new EMContext())
                {
                    context.Configuration.LazyLoadingEnabled = false;

                    List<Models.SubCategory> subCategories = context.SubCategories.Select(x => new Models.SubCategory()
                    {
                        SubCategoryID = x.SubCategoryID,
                        CategoryID = x.CategoryID,
                        SubCategoryName = x.SubCategoryName,
                        Sequence = x.Sequence,
                        CreatedDate = x.CreatedDate
                    }).OrderBy(x => x.Sequence).ToList();

                    APIResponse<List<Models.SubCategory>> apiResponse = new APIResponse<List<Models.SubCategory>>
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        StatusRemark = "",
                        Content = subCategories
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

        [SwaggerResponse(HttpStatusCode.OK, "Success", Type = typeof(APIResponse<Models.SubCategory>))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Empty Object Error", Type = null)]
        [SwaggerResponse(HttpStatusCode.ExpectationFailed, "Unable To Create", Type = null)]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Internal Server Error", Type = null)]
        [SwaggerRequestContentType(requestType: "application/json", Exclusive = true)]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [Route("api/SubCategory/Create")]
        [HttpPost]
        public HttpResponseMessage Create([FromBody] Models.SubCategory subCategory)
        {
            try
            {
                if (subCategory == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "");
                }

                using (var context = new EMContext())
                {
                    context.Configuration.LazyLoadingEnabled = false;

                    DB.Models.SubCategory newSubCategory = new DB.Models.SubCategory
                    {
                        SubCategoryID = subCategory.SubCategoryID,
                        CategoryID = subCategory.CategoryID,
                        SubCategoryName = subCategory.SubCategoryName,
                        Sequence = subCategory.Sequence,
                        CreatedDate = DateTime.Now
                    };

                    context.SubCategories.Add(newSubCategory);

                    context.SaveChanges();

                    Models.SubCategory createdSubCategory = context.SubCategories.Where(x => x.SubCategoryID == newSubCategory.SubCategoryID).Select(x => new Models.SubCategory()
                    {
                        SubCategoryID = x.SubCategoryID,
                        CategoryID = x.CategoryID,
                        SubCategoryName = x.SubCategoryName,
                        Sequence = x.Sequence,
                        CreatedDate = x.CreatedDate
                    }).FirstOrDefault();

                    APIResponse<Models.SubCategory> apiResponse = new APIResponse<Models.SubCategory>()
                    {
                        StatusCode = (int)(createdSubCategory == null ? HttpStatusCode.ExpectationFailed : HttpStatusCode.OK),
                        StatusRemark = "",
                        Content = (createdSubCategory ?? null),
                    };

                    return Request.CreateResponse((createdSubCategory == null ? HttpStatusCode.ExpectationFailed : HttpStatusCode.OK), apiResponse);
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

        [SwaggerResponse(HttpStatusCode.OK, "Success", Type = typeof(APIResponse<bool>))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Internal Server Error", Type = null)]
        [SwaggerResponse(HttpStatusCode.ExpectationFailed, "Unable To Delete", Type = null)]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Internal Server Error", Type = null)]
        [SwaggerRequestContentType(requestType: "application/json", Exclusive = true)]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [Route("api/SubCategory/Delete")]
        [HttpDelete]
        public HttpResponseMessage Delete([FromBody] Models.SubCategory subCategory)
        {
            try
            {
                if (subCategory == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "");
                }

                bool result = false;

                using (var context = new EMContext())
                {
                    context.Configuration.LazyLoadingEnabled = false;

                    DB.Models.SubCategory foundSubCategory = context.SubCategories.Where(x => x.SubCategoryID == subCategory.SubCategoryID).FirstOrDefault();

                    if (foundSubCategory != null)
                    {
                        context.SubCategories.Remove(foundSubCategory);

                        context.SaveChanges();

                        result = true;
                    }
                    else
                    {
                        result = false;
                    }

                    APIResponse<bool> apiResponse = new APIResponse<bool>()
                    {
                        StatusCode = (int)(!result ? HttpStatusCode.ExpectationFailed : HttpStatusCode.OK),
                        StatusRemark = "",
                        Content = result,
                    };

                    return Request.CreateResponse((!result ? HttpStatusCode.ExpectationFailed : HttpStatusCode.OK), apiResponse);
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