using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public class CategoryController : ApiController
    {
        [SwaggerResponse(HttpStatusCode.OK, "Success", Type = typeof(APIResponse<List<Models.Category>>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Internal Server Error", Type = null)]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [Route("api/Category/GetCategories")]
        [HttpGet]
        public HttpResponseMessage GetCategories(string transactionTypeID = "")
        {
            try
            {
                using (var context = new EMContext())
                {
                    context.Configuration.LazyLoadingEnabled = false;

                    Guid gTransactionTypeID = Guid.Empty;

                    if (transactionTypeID != null && transactionTypeID.Trim().Length > 0)
                    {
                        gTransactionTypeID = Guid.Parse(transactionTypeID);
                    }

                    List<Models.Category> categories = context.Categories.Include(x => x.TransactionType)
                    .Where(x =>
                        (
                            transactionTypeID == null ||
                            transactionTypeID.Trim().Length == 0 ||
                            gTransactionTypeID == Guid.Empty ||
                            x.TransactionTypeID == gTransactionTypeID
                        )
                    )
                    .OrderBy(x => x.Sequence).Select(x => new Models.Category()
                    {
                        CategoryID = x.CategoryID,
                        TransactionTypeID = x.TransactionTypeID,
                        TransactionTypeName = x.TransactionType.TransactionTypeName,
                        CategoryName = x.CategoryName,
                        Sequence = x.Sequence,
                        CreatedDate = x.CreatedDate
                    }).ToList();

                    APIResponse<List<Models.Category>> apiResponse = new APIResponse<List<Models.Category>>
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        StatusRemark = "",
                        Content = categories
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

        [SwaggerResponse(HttpStatusCode.OK, "Success", Type = typeof(APIResponse<Models.Category>))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Empty Object Error", Type = null)]
        [SwaggerResponse(HttpStatusCode.ExpectationFailed, "Unable To Create", Type = null)]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Internal Server Error", Type = null)]
        [SwaggerRequestContentType(requestType: "application/json", Exclusive = true)]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [Route("api/Category/Create")]
        [HttpPost]
        public HttpResponseMessage Create([FromBody] Models.Category category)
        {
            try
            {
                if (category == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "");
                }

                using (var context = new EMContext())
                {
                    context.Configuration.LazyLoadingEnabled = false;

                    DB.Models.Category newCategory = new DB.Models.Category
                    {
                        CategoryID = Guid.NewGuid(),
                        TransactionTypeID = category.TransactionTypeID,
                        CategoryName = category.CategoryName,
                        Sequence = category.Sequence,
                        CreatedDate = DateTime.Now
                    };

                    context.Categories.Add(newCategory);

                    context.SaveChanges();

                    Models.Category createdCategory = context.Categories.Where(x => x.CategoryID == newCategory.CategoryID).Select(x => new Models.Category()
                    {
                        CategoryID = x.CategoryID,
                        TransactionTypeID = x.TransactionTypeID,
                        CategoryName = x.CategoryName,
                        Sequence = x.Sequence,
                        CreatedDate = x.CreatedDate
                    }).FirstOrDefault();

                    APIResponse<Models.Category> apiResponse = new APIResponse<Models.Category>()
                    {
                        StatusCode = (int)(createdCategory == null ? HttpStatusCode.ExpectationFailed : HttpStatusCode.OK),
                        StatusRemark = "",
                        Content = (createdCategory ?? null),
                    };

                    return Request.CreateResponse((createdCategory == null ? HttpStatusCode.ExpectationFailed : HttpStatusCode.OK), apiResponse);
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

        [SwaggerResponse(HttpStatusCode.OK, "Success", Type = typeof(APIResponse<Models.Category>))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Empty Object Error", Type = null)]
        [SwaggerResponse(HttpStatusCode.ExpectationFailed, "Unable To Create", Type = null)]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Internal Server Error", Type = null)]
        [SwaggerRequestContentType(requestType: "application/json", Exclusive = true)]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [Route("api/Category/Edit")]
        [HttpPut]
        public HttpResponseMessage Edit([FromBody] Models.Category category)
        {
            try
            {
                if (category == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "");
                }

                using (var context = new EMContext())
                {
                    context.Configuration.LazyLoadingEnabled = false;

                    Models.Category updatedCategory = new Models.Category();

                    DB.Models.Category existingCategory = context.Categories.Where(x => x.CategoryID == category.CategoryID).FirstOrDefault();

                    if (existingCategory != null) {
                        existingCategory.TransactionTypeID = category.TransactionTypeID;
                        existingCategory.CategoryName = category.CategoryName;
                        existingCategory.Sequence = category.Sequence;
                        existingCategory.CreatedDate = DateTime.Now;
                       
                        context.SaveChanges();

                        updatedCategory = context.Categories.Where(x => x.CategoryID == category.CategoryID).Select(x => new Models.Category()
                        {
                            CategoryID = x.CategoryID,
                            TransactionTypeID = x.TransactionTypeID,
                            CategoryName = x.CategoryName,
                            Sequence = x.Sequence,
                            CreatedDate = x.CreatedDate
                        }).FirstOrDefault();
                    }

                    APIResponse<Models.Category> apiResponse = new APIResponse<Models.Category>()
                    {
                        StatusCode = (int)(updatedCategory == null ? HttpStatusCode.ExpectationFailed : HttpStatusCode.OK),
                        StatusRemark = "",
                        Content = (updatedCategory ?? null),
                    };

                    return Request.CreateResponse((updatedCategory == null ? HttpStatusCode.ExpectationFailed : HttpStatusCode.OK), apiResponse);
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
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [Route("api/Category/Delete/{id}")]
        [HttpDelete]
        public HttpResponseMessage Delete(string id)
        {
            try
            {
                if (id == null || (id != null && id.Trim().Length == 0))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "");
                }

                bool result = false;
                string error = string.Empty;

                using (var context = new EMContext())
                {
                    context.Configuration.LazyLoadingEnabled = false;

                    Guid categoryID = Guid.Parse(id);

                    DB.Models.Transaction foundTransaction = context.Transactions.Where(x => x.CategoryID == categoryID).FirstOrDefault();

                    if (foundTransaction == null)
                    {
                        DB.Models.Category foundCategory = context.Categories.Where(x => x.CategoryID == categoryID).FirstOrDefault();

                        if (foundCategory != null)
                        {
                            context.Categories.Remove(foundCategory);

                            context.SaveChanges();

                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                    else
                    {
                        result = false;
                        error = "Error, found that this category in used in transaction.";
                    }

                    APIResponse<bool> apiResponse = new APIResponse<bool>()
                    {
                        StatusCode = (int)(!result ? HttpStatusCode.ExpectationFailed : HttpStatusCode.OK),
                        StatusRemark = error,
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