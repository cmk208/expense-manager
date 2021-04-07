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
    public class TransactionController : ApiController
    {
        [SwaggerResponse(HttpStatusCode.OK, "Success", Type = typeof(APIResponse<List<Models.Transaction>>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Internal Server Error", Type = null)]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [Route("api/Transaction/GetTransactionsByDate")]
        [HttpGet]
        public HttpResponseMessage GetTransactionsByDate(string startDate = "", string endDate = "", string transactionTypeID = "", string accountID = "", string categoryID = "", string min = "", string max = "")
        {
            try
            {
                using (var context = new EMContext())
                {
                    context.Configuration.LazyLoadingEnabled = false;

                    DateTime firstDate = new DateTime(int.Parse(startDate.Substring(0, 4)), int.Parse(startDate.Substring(5, 2)), int.Parse(startDate.Substring(8, 2)));
                    DateTime lastDate = new DateTime(int.Parse(endDate.Substring(0, 4)), int.Parse(endDate.Substring(5, 2)), int.Parse(endDate.Substring(8, 2)));
                    Guid gTransactionTypeID = Guid.Parse(transactionTypeID);
                    Guid gAccountID = Guid.Empty;

                    if (accountID != null && accountID.Trim().Length > 0)
                    {
                        gAccountID = Guid.Parse(accountID);
                    }

                    Guid gCategoryID = Guid.Empty;

                    if (categoryID != null && categoryID.Trim().Length > 0)
                    {
                        gCategoryID = Guid.Parse(categoryID);
                    }

                    decimal dMin = 0;

                    if (min != null && min.Trim().Length > 0)
                    {
                        dMin = decimal.Parse(min);
                    }

                    decimal dMax = 0;

                    if (max != null && max.Trim().Length > 0)
                    {
                        dMax = decimal.Parse(max);
                    }

                    List<Models.Transaction> transactions = context.Transactions
                        .Include(x => x.TransactionType)
                        .Include(x => x.FromAccount)
                        .Include(x => x.ToAccount)
                        .Include(x => x.Category)
                        .Where(x => (
                                        x.TransactionDate >= firstDate &&
                                        x.TransactionDate <= lastDate
                                    ) &&
                                    (
                                        accountID == null ||
                                        accountID.Trim().Length == 0 ||
                                        gAccountID == Guid.Empty ||
                                        x.FromAccountID == gAccountID
                                    ) &&
                                    (
                                        categoryID == null ||
                                        categoryID.Trim().Length == 0 ||
                                        gCategoryID == Guid.Empty ||
                                        x.CategoryID == gCategoryID
                                    ) &&
                                    (
                                        transactionTypeID == null ||
                                        transactionTypeID.Trim().Length == 0 ||
                                        gTransactionTypeID == Guid.Empty ||
                                        x.TransactionTypeID == gTransactionTypeID
                                    ) &&
                                    (
                                        min == null ||
                                        min.Trim().Length == 0 ||
                                        x.Amount >= dMin
                                    ) &&
                                    (
                                        max == null ||
                                        max.Trim().Length == 0 ||
                                        x.Amount <= dMax
                                    )
                        )
                        .OrderByDescending(x => x.TransactionDate).ThenBy(x => x.TransactionTime)
                        .Select(x => new Models.Transaction()
                        {
                            TransactionID = x.TransactionID,
                            TransactionTypeID = x.TransactionTypeID,
                            TransactionTypeName = x.TransactionType.TransactionTypeName,
                            FromAccountID = x.FromAccountID,
                            FromAccountName = x.FromAccount.AccountName,
                            ToAccountID = x.ToAccountID,
                            ToAccountName = x.ToAccount.AccountName,
                            CategoryID = x.CategoryID,
                            CategoryName = x.Category.CategoryName,
                            Operator = x.Operator,
                            TransactionDate = x.TransactionDate,
                            TransactionTime = x.TransactionTime,
                            Amount = x.Amount,
                            Note = x.Note,
                            CreatedDate = x.CreatedDate
                        }).ToList();

                    APIResponse<List<Models.Transaction>> apiResponse = new APIResponse<List<Models.Transaction>>
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        StatusRemark = "",
                        Content = transactions
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

        [SwaggerResponse(HttpStatusCode.OK, "Success", Type = typeof(APIResponse<List<Models.Transaction>>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Internal Server Error", Type = null)]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [Route("api/Transaction/GetTransactionsByMonth")]
        [HttpGet]
        public HttpResponseMessage GetTransactionsByMonth(string year = "", string month = "", string transactionTypeID = "", string accountID = "", string categoryID = "", string min = "", string max = "")
        {
            try
            {
                using (var context = new EMContext())
                {
                    context.Configuration.LazyLoadingEnabled = false;

                    DateTime firstDate = new DateTime(int.Parse(year), int.Parse(month), 1);
                    DateTime lastDate = new DateTime(firstDate.Year, firstDate.Month, 1).AddMonths(1).AddDays(-1);
                    Guid gTransactionTypeID = Guid.Parse(transactionTypeID);
                    Guid gAccountID = Guid.Empty;

                    if (accountID != null && accountID.Trim().Length > 0)
                    {
                        gAccountID = Guid.Parse(accountID);
                    }

                    Guid gCategoryID = Guid.Empty;

                    if (categoryID != null && categoryID.Trim().Length > 0)
                    {
                        gCategoryID = Guid.Parse(categoryID);
                    }

                    decimal dMin = 0;

                    if (min != null && min.Trim().Length > 0)
                    {
                        dMin = decimal.Parse(min);
                    }

                    decimal dMax = 0;

                    if (max != null && max.Trim().Length > 0)
                    {
                        dMax = decimal.Parse(max);
                    }

                    List<Models.Transaction> transactions = context.Transactions
                        .Include(x => x.TransactionType)
                        .Include(x => x.FromAccount)
                        .Include(x => x.ToAccount)
                        .Include(x => x.Category)
                        .Where(x => (
                                        x.TransactionDate >= firstDate &&
                                        x.TransactionDate <= lastDate
                                    ) &&
                                    (
                                        accountID == null ||
                                        accountID.Trim().Length == 0 ||
                                        gAccountID == Guid.Empty ||
                                        x.FromAccountID == gAccountID
                                    ) &&
                                    (
                                        categoryID == null ||
                                        categoryID.Trim().Length == 0 ||
                                        gCategoryID == Guid.Empty ||
                                        x.CategoryID == gCategoryID
                                    ) &&
                                    (
                                        transactionTypeID == null ||
                                        transactionTypeID.Trim().Length == 0 ||
                                        gTransactionTypeID == Guid.Empty ||
                                        x.TransactionTypeID == gTransactionTypeID
                                    ) &&
                                    (
                                        min == null ||
                                        min.Trim().Length == 0 ||
                                        x.Amount >= dMin
                                    ) &&
                                    (
                                        max == null ||
                                        max.Trim().Length == 0 ||
                                        x.Amount <= dMax
                                    )
                        )
                        .OrderByDescending(x => x.TransactionDate).ThenBy(x => x.TransactionTime)
                        .Select(x => new Models.Transaction()
                        {
                            TransactionID = x.TransactionID,
                            TransactionTypeID = x.TransactionTypeID,
                            TransactionTypeName = x.TransactionType.TransactionTypeName,
                            FromAccountID = x.FromAccountID,
                            FromAccountName = x.FromAccount.AccountName,
                            ToAccountID = x.ToAccountID,
                            ToAccountName = x.ToAccount.AccountName,
                            CategoryID = x.CategoryID,
                            CategoryName = x.Category.CategoryName,
                            Operator = x.Operator,
                            TransactionDate = x.TransactionDate,
                            TransactionTime = x.TransactionTime,
                            Amount = x.Amount,
                            Note = x.Note,
                            CreatedDate = x.CreatedDate
                        }).ToList();

                    APIResponse<List<Models.Transaction>> apiResponse = new APIResponse<List<Models.Transaction>>
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        StatusRemark = "",
                        Content = transactions
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

        [SwaggerResponse(HttpStatusCode.OK, "Success", Type = typeof(APIResponse<List<Models.Transaction>>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Internal Server Error", Type = null)]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [Route("api/Transaction/GetTransactionsByYear")]
        [HttpGet]
        public HttpResponseMessage GetTransactionsByYear(string year = "", string transactionTypeID = "", string accountID = "", string categoryID = "", string min = "", string max = "")
        {
            try
            {
                using (var context = new EMContext())
                {
                    context.Configuration.LazyLoadingEnabled = false;

                    DateTime firstDate = new DateTime(int.Parse(year), 1, 1);
                    DateTime lastDate = new DateTime(firstDate.Year, 1, 1).AddYears(1).AddDays(-1);
                    Guid gTransactionTypeID = Guid.Parse(transactionTypeID);
                    Guid gAccountID = Guid.Empty;

                    if (accountID != null && accountID.Trim().Length > 0)
                    {
                        gAccountID = Guid.Parse(accountID);
                    }

                    Guid gCategoryID = Guid.Empty;

                    if (categoryID != null && categoryID.Trim().Length > 0)
                    {
                        gCategoryID = Guid.Parse(categoryID);
                    }

                    decimal dMin = 0;

                    if (min != null && min.Trim().Length > 0)
                    {
                        dMin = decimal.Parse(min);
                    }

                    decimal dMax = 0;

                    if (max != null && max.Trim().Length > 0)
                    {
                        dMax = decimal.Parse(max);
                    }

                    List<Models.Transaction> transactions = context.Transactions
                        .Include(x => x.TransactionType)
                        .Include(x => x.FromAccount)
                        .Include(x => x.ToAccount)
                        .Include(x => x.Category)
                        .Where(x => (
                                        x.TransactionDate >= firstDate &&
                                        x.TransactionDate <= lastDate
                                    ) &&
                                    (
                                        accountID == null ||
                                        accountID.Trim().Length == 0 ||
                                        gAccountID == Guid.Empty ||
                                        x.FromAccountID == gAccountID
                                    ) &&
                                    (
                                        categoryID == null ||
                                        categoryID.Trim().Length == 0 ||
                                        gCategoryID == Guid.Empty ||
                                        x.CategoryID == gCategoryID
                                    ) &&
                                    (
                                        transactionTypeID == null ||
                                        transactionTypeID.Trim().Length == 0 ||
                                        gTransactionTypeID == Guid.Empty ||
                                        x.TransactionTypeID == gTransactionTypeID
                                    ) &&
                                    (
                                        min == null ||
                                        min.Trim().Length == 0 ||
                                        x.Amount >= dMin
                                    ) &&
                                    (
                                        max == null ||
                                        max.Trim().Length == 0 ||
                                        x.Amount <= dMax
                                    )
                        )
                        .OrderByDescending(x => x.TransactionDate).ThenBy(x => x.TransactionTime)
                        .Select(x => new Models.Transaction()
                        {
                            TransactionID = x.TransactionID,
                            TransactionTypeID = x.TransactionTypeID,
                            TransactionTypeName = x.TransactionType.TransactionTypeName,
                            FromAccountID = x.FromAccountID,
                            FromAccountName = x.FromAccount.AccountName,
                            ToAccountID = x.ToAccountID,
                            ToAccountName = x.ToAccount.AccountName,
                            CategoryID = x.CategoryID,
                            CategoryName = x.Category.CategoryName,
                            Operator = x.Operator,
                            TransactionDate = x.TransactionDate,
                            TransactionTime = x.TransactionTime,
                            Amount = x.Amount,
                            Note = x.Note,
                            CreatedDate = x.CreatedDate
                        }).ToList();

                    APIResponse<List<Models.Transaction>> apiResponse = new APIResponse<List<Models.Transaction>>
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        StatusRemark = "",
                        Content = transactions
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

        [SwaggerResponse(HttpStatusCode.OK, "Success", Type = typeof(APIResponse<Models.Transaction>))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Empty Object Error", Type = null)]
        [SwaggerResponse(HttpStatusCode.ExpectationFailed, "Unable To Create", Type = null)]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Internal Server Error", Type = null)]
        [SwaggerRequestContentType(requestType: "application/json", Exclusive = true)]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [Route("api/Transaction/Create")]
        [HttpPost]
        public HttpResponseMessage Create([FromBody] Models.Transaction transaction)
        {
            try
            {
                if (transaction == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "");
                }

                using (var context = new EMContext())
                {
                    context.Configuration.LazyLoadingEnabled = false;

                    DB.Models.TransactionType transactionType = context.TransactionTypes.Where(x => x.TransactionTypeID == transaction.TransactionTypeID).FirstOrDefault();

                    DB.Models.Transaction fromTransaction = new DB.Models.Transaction();
                    DB.Models.Transaction toTransaction = new DB.Models.Transaction();
                    DB.Models.Transaction foundTransaction = new DB.Models.Transaction();
                    Models.Transaction createdTransaction = new Models.Transaction();

                    if (transactionType != null)
                    {
                        switch (transactionType.TransactionTypeName)
                        {
                            case "Income":
                                fromTransaction = new DB.Models.Transaction
                                {
                                    TransactionID = Guid.NewGuid(),
                                    TransactionTypeID = transaction.TransactionTypeID,
                                    FromAccountID = transaction.FromAccountID,
                                    CategoryID = transaction.CategoryID,
                                    Operator = true,
                                    TransactionDate = transaction.TransactionDate,
                                    TransactionTime = transaction.TransactionTime,
                                    Amount = transaction.Amount,
                                    Note = transaction.Note,
                                    CreatedDate = DateTime.Now
                                };

                                if (transaction.SubCategoryID != null && transaction.SubCategoryID != Guid.Empty)
                                {
                                    fromTransaction.SubCategoryID = transaction.SubCategoryID;
                                }

                                context.Transactions.Add(fromTransaction);

                                context.SaveChanges();

                                foundTransaction = context.Transactions.Where(x => x.TransactionID == fromTransaction.TransactionID).FirstOrDefault();

                                context.Entry(foundTransaction).Reference(x => x.FromAccount).Load();
                                context.Entry(foundTransaction).Reference(x => x.Category).Load();

                                if (transaction.SubCategoryID != null && transaction.SubCategoryID != Guid.Empty)
                                {
                                    context.Entry(foundTransaction).Reference(x => x.SubCategory).Load();
                                }

                                createdTransaction = new Models.Transaction
                                {
                                    TransactionID = foundTransaction.TransactionID,
                                    TransactionTypeID = foundTransaction.TransactionTypeID,
                                    FromAccountID = foundTransaction.FromAccountID,
                                    CategoryID = foundTransaction.CategoryID,
                                    SubCategoryID = foundTransaction.SubCategoryID,
                                    Operator = foundTransaction.Operator,
                                    TransactionDate = foundTransaction.TransactionDate,
                                    TransactionTime = foundTransaction.TransactionTime,
                                    Amount = foundTransaction.Amount,
                                    Note = foundTransaction.Note,
                                    CreatedDate = foundTransaction.CreatedDate
                                };
                                break;
                            case "Expense":
                                fromTransaction = new DB.Models.Transaction
                                {
                                    TransactionID = Guid.NewGuid(),
                                    TransactionTypeID = transaction.TransactionTypeID,
                                    FromAccountID = transaction.FromAccountID,
                                    CategoryID = transaction.CategoryID,
                                    Operator = false,
                                    TransactionDate = transaction.TransactionDate,
                                    TransactionTime = transaction.TransactionTime,
                                    Amount = transaction.Amount,
                                    Note = transaction.Note,
                                    CreatedDate = DateTime.Now
                                };

                                if (transaction.SubCategoryID != null && transaction.SubCategoryID != Guid.Empty)
                                {
                                    fromTransaction.SubCategoryID = transaction.SubCategoryID;
                                }

                                context.Transactions.Add(fromTransaction);

                                context.SaveChanges();

                                foundTransaction = context.Transactions.Where(x => x.TransactionID == fromTransaction.TransactionID).FirstOrDefault();

                                context.Entry(foundTransaction).Reference(x => x.FromAccount).Load();
                                context.Entry(foundTransaction).Reference(x => x.Category).Load();

                                if (transaction.SubCategoryID != null && transaction.SubCategoryID != Guid.Empty)
                                {
                                    context.Entry(foundTransaction).Reference(x => x.SubCategory).Load();
                                }

                                createdTransaction = new Models.Transaction
                                {
                                    TransactionID = foundTransaction.TransactionID,
                                    TransactionTypeID = foundTransaction.TransactionTypeID,
                                    FromAccountID = foundTransaction.FromAccountID,
                                    CategoryID = foundTransaction.CategoryID,
                                    SubCategoryID = foundTransaction.SubCategoryID,
                                    Operator = foundTransaction.Operator,
                                    TransactionDate = foundTransaction.TransactionDate,
                                    TransactionTime = foundTransaction.TransactionTime,
                                    Amount = foundTransaction.Amount,
                                    Note = foundTransaction.Note,
                                    CreatedDate = foundTransaction.CreatedDate
                                };
                                break;
                            case "Transfer":
                                fromTransaction = new DB.Models.Transaction
                                {
                                    TransactionID = Guid.NewGuid(),
                                    TransactionTypeID = transaction.TransactionTypeID,
                                    FromAccountID = transaction.FromAccountID,
                                    ToAccountID = transaction.ToAccountID,
                                    Operator = false,
                                    TransactionDate = transaction.TransactionDate,
                                    TransactionTime = transaction.TransactionTime,
                                    Amount = transaction.Amount,
                                    Note = transaction.Note,
                                    CreatedDate = DateTime.Now
                                };

                                toTransaction = new DB.Models.Transaction
                                {
                                    TransactionID = Guid.NewGuid(),
                                    FromTransactionID = fromTransaction.TransactionID,
                                    TransactionTypeID = transaction.TransactionTypeID,
                                    FromAccountID = transaction.ToAccountID ?? Guid.Empty,
                                    ToAccountID = transaction.FromAccountID,
                                    Operator = true,
                                    TransactionDate = transaction.TransactionDate,
                                    TransactionTime = transaction.TransactionTime,
                                    Amount = transaction.Amount,
                                    Note = transaction.Note,
                                    CreatedDate = DateTime.Now
                                };

                                context.Transactions.Add(fromTransaction);
                                context.Transactions.Add(toTransaction);

                                context.SaveChanges();

                                foundTransaction = context.Transactions.Where(x => x.TransactionID == fromTransaction.TransactionID).FirstOrDefault();

                                context.Entry(foundTransaction).Reference(x => x.FromAccount).Load();
                                context.Entry(foundTransaction).Reference(x => x.ToAccount).Load();

                                createdTransaction = new Models.Transaction
                                {
                                    TransactionID = foundTransaction.TransactionID,
                                    TransactionTypeID = foundTransaction.TransactionTypeID,
                                    FromAccountID = foundTransaction.FromAccountID,
                                    ToAccountID = foundTransaction.ToAccountID,
                                    Operator = foundTransaction.Operator,
                                    TransactionDate = foundTransaction.TransactionDate,
                                    TransactionTime = foundTransaction.TransactionTime,
                                    Amount = foundTransaction.Amount,
                                    Note = foundTransaction.Note,
                                    CreatedDate = foundTransaction.CreatedDate
                                };
                                break;
                            default:
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "");
                        }
                    }

                    APIResponse<Models.Transaction> apiResponse = new APIResponse<Models.Transaction>()
                    {
                        StatusCode = (int)(createdTransaction == null ? HttpStatusCode.ExpectationFailed : HttpStatusCode.OK),
                        StatusRemark = "",
                        Content = (createdTransaction ?? null),
                    };

                    return Request.CreateResponse((createdTransaction == null ? HttpStatusCode.ExpectationFailed : HttpStatusCode.OK), apiResponse);
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

        [SwaggerResponse(HttpStatusCode.OK, "Success", Type = typeof(APIResponse<Models.Transaction>))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Empty Object Error", Type = null)]
        [SwaggerResponse(HttpStatusCode.ExpectationFailed, "Unable To Create", Type = null)]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Internal Server Error", Type = null)]
        [SwaggerRequestContentType(requestType: "application/json", Exclusive = true)]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [Route("api/Transaction/Edit")]
        [HttpPut]
        public HttpResponseMessage Update([FromBody] Models.Transaction transaction)
        {
            try
            {
                if (transaction == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "");
                }

                using (var context = new EMContext())
                {
                    context.Configuration.LazyLoadingEnabled = false;

                    DB.Models.Transaction existingTransaction = context.Transactions.Include(x => x.FromTransaction).Include(x => x.TransactionType).Where(x => x.TransactionID == transaction.TransactionID).FirstOrDefault();
                    DB.Models.TransactionType transactionType = context.TransactionTypes.Where(x => x.TransactionTypeID == transaction.TransactionTypeID).FirstOrDefault();

                    if (existingTransaction != null && transactionType != null)
                    {
                        DB.Models.Transaction foundTransaction = new DB.Models.Transaction();
                        Models.Transaction createdTransaction = new Models.Transaction();

                        switch (transactionType.TransactionTypeName)
                        {
                            case "Income":
                                existingTransaction.TransactionTypeID = transaction.TransactionTypeID;
                                existingTransaction.FromAccountID = transaction.FromAccountID;
                                existingTransaction.CategoryID = transaction.CategoryID;
                                existingTransaction.Operator = true;
                                existingTransaction.TransactionDate = transaction.TransactionDate;
                                existingTransaction.TransactionTime = transaction.TransactionTime;
                                existingTransaction.Amount = transaction.Amount;
                                existingTransaction.Note = transaction.Note;
                                existingTransaction.CreatedDate = DateTime.Now;

                                if (transaction.SubCategoryID != null && transaction.SubCategoryID != Guid.Empty)
                                {
                                    existingTransaction.SubCategoryID = transaction.SubCategoryID;
                                }
                                else
                                {
                                    existingTransaction.SubCategoryID = null;
                                }

                                context.SaveChanges();

                                foundTransaction = context.Transactions.Where(x => x.TransactionID == existingTransaction.TransactionID).FirstOrDefault();

                                context.Entry(foundTransaction).Reference(x => x.FromAccount).Load();
                                context.Entry(foundTransaction).Reference(x => x.Category).Load();

                                if (transaction.SubCategoryID != null && transaction.SubCategoryID != Guid.Empty)
                                {
                                    context.Entry(foundTransaction).Reference(x => x.SubCategory).Load();
                                }

                                createdTransaction = new Models.Transaction
                                {
                                    TransactionID = foundTransaction.TransactionID,
                                    TransactionTypeID = foundTransaction.TransactionTypeID,
                                    FromAccountID = foundTransaction.FromAccountID,
                                    CategoryID = foundTransaction.CategoryID,
                                    SubCategoryID = foundTransaction.SubCategoryID,
                                    Operator = foundTransaction.Operator,
                                    TransactionDate = foundTransaction.TransactionDate,
                                    TransactionTime = foundTransaction.TransactionTime,
                                    Amount = foundTransaction.Amount,
                                    Note = foundTransaction.Note,
                                    CreatedDate = foundTransaction.CreatedDate
                                };
                                break;
                            case "Expense":
                                existingTransaction.TransactionTypeID = transaction.TransactionTypeID;
                                existingTransaction.FromAccountID = transaction.FromAccountID;
                                existingTransaction.CategoryID = transaction.CategoryID;
                                existingTransaction.Operator = false;
                                existingTransaction.TransactionDate = transaction.TransactionDate;
                                existingTransaction.TransactionTime = transaction.TransactionTime;
                                existingTransaction.Amount = transaction.Amount;
                                existingTransaction.Note = transaction.Note;
                                existingTransaction.CreatedDate = DateTime.Now;

                                if (transaction.SubCategoryID != null && transaction.SubCategoryID != Guid.Empty)
                                {
                                    existingTransaction.SubCategoryID = transaction.SubCategoryID;
                                }
                                else
                                {
                                    existingTransaction.SubCategoryID = null;

                                }

                                context.SaveChanges();

                                foundTransaction = context.Transactions.Where(x => x.TransactionID == existingTransaction.TransactionID).FirstOrDefault();

                                context.Entry(foundTransaction).Reference(x => x.FromAccount).Load();
                                context.Entry(foundTransaction).Reference(x => x.Category).Load();

                                if (transaction.SubCategoryID != null && transaction.SubCategoryID != Guid.Empty)
                                {
                                    context.Entry(foundTransaction).Reference(x => x.SubCategory).Load();
                                }

                                createdTransaction = new Models.Transaction
                                {
                                    TransactionID = foundTransaction.TransactionID,
                                    TransactionTypeID = foundTransaction.TransactionTypeID,
                                    FromAccountID = foundTransaction.FromAccountID,
                                    CategoryID = foundTransaction.CategoryID,
                                    SubCategoryID = foundTransaction.SubCategoryID,
                                    Operator = foundTransaction.Operator,
                                    TransactionDate = foundTransaction.TransactionDate,
                                    TransactionTime = foundTransaction.TransactionTime,
                                    Amount = foundTransaction.Amount,
                                    Note = foundTransaction.Note,
                                    CreatedDate = foundTransaction.CreatedDate
                                };
                                break;
                            case "Transfer":
                                existingTransaction.TransactionTypeID = transaction.TransactionTypeID;
                                existingTransaction.FromAccountID = transaction.FromAccountID;
                                existingTransaction.ToAccountID = transaction.ToAccountID;
                                existingTransaction.Operator = false;
                                existingTransaction.TransactionDate = transaction.TransactionDate;
                                existingTransaction.TransactionTime = transaction.TransactionTime;
                                existingTransaction.Amount = transaction.Amount;
                                existingTransaction.Note = transaction.Note;
                                existingTransaction.CreatedDate = DateTime.Now;

                                context.SaveChanges();

                                DB.Models.Transaction existingToTransaction = context.Transactions.Where(x => x.FromTransactionID == existingTransaction.TransactionID).FirstOrDefault();

                                if (existingToTransaction != null)
                                {
                                    existingToTransaction.TransactionTypeID = transaction.TransactionTypeID;
                                    existingToTransaction.FromAccountID = transaction.ToAccountID ?? Guid.Empty;
                                    existingToTransaction.ToAccountID = transaction.FromAccountID;
                                    existingToTransaction.Operator = true;
                                    existingToTransaction.TransactionDate = transaction.TransactionDate;
                                    existingToTransaction.TransactionTime = transaction.TransactionTime;
                                    existingToTransaction.Amount = transaction.Amount;
                                    existingToTransaction.Note = transaction.Note;
                                    existingToTransaction.CreatedDate = DateTime.Now;

                                    context.SaveChanges();
                                }

                                foundTransaction = context.Transactions.Where(x => x.TransactionID == existingTransaction.TransactionID).FirstOrDefault();

                                context.Entry(foundTransaction).Reference(x => x.FromAccount).Load();
                                context.Entry(foundTransaction).Reference(x => x.ToAccount).Load();

                                createdTransaction = new Models.Transaction
                                {
                                    TransactionID = foundTransaction.TransactionID,
                                    TransactionTypeID = foundTransaction.TransactionTypeID,
                                    FromAccountID = foundTransaction.FromAccountID,
                                    ToAccountID = foundTransaction.ToAccountID,
                                    Operator = foundTransaction.Operator,
                                    TransactionDate = foundTransaction.TransactionDate,
                                    TransactionTime = foundTransaction.TransactionTime,
                                    Amount = foundTransaction.Amount,
                                    Note = foundTransaction.Note,
                                    CreatedDate = foundTransaction.CreatedDate
                                };
                                break;
                            default:
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "");
                        }

                        APIResponse<Models.Transaction> apiResponse = new APIResponse<Models.Transaction>()
                        {
                            StatusCode = (int)(createdTransaction == null ? HttpStatusCode.ExpectationFailed : HttpStatusCode.OK),
                            StatusRemark = "",
                            Content = (createdTransaction ?? null),
                        };

                        return Request.CreateResponse((createdTransaction == null ? HttpStatusCode.ExpectationFailed : HttpStatusCode.OK), apiResponse);
                    }

                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "");
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
        [Route("api/Transaction/Delete/{id}")]
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

                using (var context = new EMContext())
                {
                    context.Configuration.LazyLoadingEnabled = false;

                    Guid transactionID = Guid.Parse(id);

                    //Delete any transfer transaction
                    DB.Models.Transaction foundToTransaction = context.Transactions.Where(x => x.FromTransactionID == transactionID).FirstOrDefault();

                    if (foundToTransaction != null)
                    {
                        context.Transactions.Remove(foundToTransaction);

                        context.SaveChanges();
                    }

                    DB.Models.Transaction foundTransaction = context.Transactions.Where(x => x.TransactionID == transactionID).FirstOrDefault();

                    if (foundTransaction != null)
                    {
                        context.Transactions.Remove(foundTransaction);

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