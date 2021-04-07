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
    public class AccountController : ApiController
    {
        [SwaggerResponse(HttpStatusCode.OK, "Success", Type = typeof(APIResponse<List<Models.Account>>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Internal Server Error", Type = null)]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [Route("api/Account/GetAccounts")]
        [HttpGet]
        public HttpResponseMessage GetAccounts()
        {
            try
            {
                using (var context = new EMContext())
                {
                    context.Configuration.LazyLoadingEnabled = false;

                    List<Models.Account> accounts = context.Accounts.Include(x => x.AccountType).OrderBy(x => x.AccountType.AccountTypeName).ThenBy(x => x.AccountName).Select(x => new Models.Account()
                    {
                        AccountID = x.AccountID,
                        AccountTypeID = x.AccountTypeID,
                        AccountTypeName = x.AccountType.AccountTypeName,
                        AccountName = x.AccountName,
                        CreatedDate = x.CreatedDate
                    }).ToList();

                    APIResponse<List<Models.Account>> apiResponse = new APIResponse<List<Models.Account>>
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        StatusRemark = "",
                        Content = accounts
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

        [SwaggerResponse(HttpStatusCode.OK, "Success", Type = typeof(APIResponse<Models.Account>))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Empty Object Error", Type = null)]
        [SwaggerResponse(HttpStatusCode.ExpectationFailed, "Unable To Create", Type = null)]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Internal Server Error", Type = null)]
        [SwaggerRequestContentType(requestType: "application/json", Exclusive = true)]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [Route("api/Account/Create")]
        [HttpPost]
        public HttpResponseMessage Create([FromBody] Models.Account account)
        {
            try
            {
                if (account == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "");
                }

                using (var context = new EMContext())
                {
                    context.Configuration.LazyLoadingEnabled = false;

                    DB.Models.Account newAccount = new DB.Models.Account
                    {
                        AccountID = Guid.NewGuid(),
                        AccountTypeID = account.AccountTypeID,
                        AccountName = account.AccountName,
                        CreatedDate = DateTime.Now
                    };

                    context.Accounts.Add(newAccount);

                    context.SaveChanges();

                    Models.Account createdAccount = context.Accounts.Where(x => x.AccountID == newAccount.AccountID).Select(x => new Models.Account()
                    {
                        AccountID = x.AccountID,
                        AccountTypeID = x.AccountTypeID,
                        AccountName = x.AccountName,
                        CreatedDate = x.CreatedDate
                    }).FirstOrDefault();

                    APIResponse<Models.Account> apiResponse = new APIResponse<Models.Account>()
                    {
                        StatusCode = (int)(createdAccount == null ? HttpStatusCode.ExpectationFailed : HttpStatusCode.OK),
                        StatusRemark = "",
                        Content = (createdAccount ?? null),
                    };

                    return Request.CreateResponse((createdAccount == null ? HttpStatusCode.ExpectationFailed : HttpStatusCode.OK), apiResponse);
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

        [SwaggerResponse(HttpStatusCode.OK, "Success", Type = typeof(APIResponse<Models.Account>))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Empty Object Error", Type = null)]
        [SwaggerResponse(HttpStatusCode.ExpectationFailed, "Unable To Create", Type = null)]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Internal Server Error", Type = null)]
        [SwaggerRequestContentType(requestType: "application/json", Exclusive = true)]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [Route("api/Account/Edit")]
        [HttpPut]
        public HttpResponseMessage Edit([FromBody] Models.Account account)
        {
            try
            {
                if (account == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "");
                }

                using (var context = new EMContext())
                {
                    context.Configuration.LazyLoadingEnabled = false;

                    Models.Account updatedAccount = new Models.Account();

                    DB.Models.Account existingAccount = context.Accounts.Where(x => x.AccountID == account.AccountID).FirstOrDefault();

                    if (existingAccount != null)
                    {
                        existingAccount.AccountTypeID = account.AccountTypeID;
                        existingAccount.AccountName = account.AccountName;
                        existingAccount.CreatedDate = DateTime.Now;

                        context.SaveChanges();

                        updatedAccount = context.Accounts.Where(x => x.AccountID == account.AccountID).Select(x => new Models.Account()
                        {
                            AccountID = x.AccountID,
                            AccountTypeID = x.AccountTypeID,
                            AccountName = x.AccountName,
                            CreatedDate = x.CreatedDate
                        }).FirstOrDefault();
                    }

                    APIResponse<Models.Account> apiResponse = new APIResponse<Models.Account>()
                    {
                        StatusCode = (int)(updatedAccount == null ? HttpStatusCode.ExpectationFailed : HttpStatusCode.OK),
                        StatusRemark = "",
                        Content = (updatedAccount ?? null),
                    };

                    return Request.CreateResponse((updatedAccount == null ? HttpStatusCode.ExpectationFailed : HttpStatusCode.OK), apiResponse);
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
        [Route("api/Account/Delete/{id}")]
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

                    Guid accountID = Guid.Parse(id);
                    
                    DB.Models.Transaction foundTransaction = context.Transactions.Where(x => x.FromAccountID == accountID || x.ToAccountID == accountID).FirstOrDefault();

                    if (foundTransaction == null)
                    {

                        DB.Models.Account foundAccount = context.Accounts.Where(x => x.AccountID == accountID).FirstOrDefault();

                        if (foundAccount != null)
                        {
                            context.Accounts.Remove(foundAccount);

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
                        error = "Error, found that this account in used in transaction.";
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