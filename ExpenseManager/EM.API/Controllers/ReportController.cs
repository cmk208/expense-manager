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
    public class ReportController : ApiController
    {
        [SwaggerResponse(HttpStatusCode.OK, "Success", Type = typeof(APIResponse<List<Models.Report>>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Internal Server Error", Type = null)]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [Route("api/Report/GetReportsByDate")]
        [HttpGet]
        public HttpResponseMessage GetReportsByDate(string startDate = "", string endDate = "", string transactionTypeID = "")
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
                                        transactionTypeID == null ||
                                        transactionTypeID.Trim().Length == 0 ||
                                        gTransactionTypeID == Guid.Empty ||
                                        x.TransactionTypeID == gTransactionTypeID
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

                    List<Report> reports = transactions.GroupBy(x => new { x.CategoryID, x.CategoryName }).Select(x => new Report
                    {
                        CategoryID = x.Key.CategoryID ?? Guid.Empty,
                        CategoryName = x.Key.CategoryName,
                        Amount = x.Sum(y => y.Amount)
                    }).ToList();

                    decimal total = reports.Sum(x => x.Amount);

                    foreach (Report report in reports)
                    {
                        report.Percentage = report.Amount * 100 / total;
                        report.Transactions = transactions.Where(x => x.CategoryID == report.CategoryID).ToList();
                    }
                    
                    APIResponse<List<Models.Report>> apiResponse = new APIResponse<List<Models.Report>>
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        StatusRemark = "",
                        Content = reports
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

        [SwaggerResponse(HttpStatusCode.OK, "Success", Type = typeof(APIResponse<List<Models.Report>>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Internal Server Error", Type = null)]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [Route("api/Report/GetReportsByMonth")]
        [HttpGet]
        public HttpResponseMessage GetReportsByMonth(string year = "", string month = "", string transactionTypeID = "")
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
                                        transactionTypeID == null ||
                                        transactionTypeID.Trim().Length == 0 ||
                                        gTransactionTypeID == Guid.Empty ||
                                        x.TransactionTypeID == gTransactionTypeID
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

                    List<Report> reports = transactions.GroupBy(x => new { x.CategoryID, x.CategoryName }).Select(x => new Report
                    {
                        CategoryID = x.Key.CategoryID ?? Guid.Empty,
                        CategoryName = x.Key.CategoryName,
                        Amount = x.Sum(y => y.Amount)
                    }).ToList();

                    decimal total = reports.Sum(x => x.Amount);

                    foreach (Report report in reports)
                    {
                        report.Percentage = report.Amount * 100 / total;
                        report.Transactions = transactions.Where(x => x.CategoryID == report.CategoryID).ToList();
                    }

                    APIResponse<List<Models.Report>> apiResponse = new APIResponse<List<Models.Report>>
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        StatusRemark = "",
                        Content = reports
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

        [SwaggerResponse(HttpStatusCode.OK, "Success", Type = typeof(APIResponse<List<Models.Report>>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Internal Server Error", Type = null)]
        [SwaggerResponseContentType(responseType: "application/json", Exclusive = true)]
        [Route("api/Report/GetReportsByYear")]
        [HttpGet]
        public HttpResponseMessage GetReportsByYear(string year = "", string transactionTypeID = "")
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
                                        transactionTypeID == null ||
                                        transactionTypeID.Trim().Length == 0 ||
                                        gTransactionTypeID == Guid.Empty ||
                                        x.TransactionTypeID == gTransactionTypeID
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

                    List<Report> reports = transactions.GroupBy(x => new { x.CategoryID, x.CategoryName }).Select(x => new Report
                    {
                        CategoryID = x.Key.CategoryID ?? Guid.Empty,
                        CategoryName = x.Key.CategoryName,
                        Amount = x.Sum(y => y.Amount)
                    }).ToList();

                    decimal total = reports.Sum(x => x.Amount);

                    foreach (Report report in reports)
                    {
                        report.Percentage = report.Amount * 100 / total;
                        report.Transactions = transactions.Where(x => x.CategoryID == report.CategoryID).ToList();
                    }

                    APIResponse<List<Models.Report>> apiResponse = new APIResponse<List<Models.Report>>
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        StatusRemark = "",
                        Content = reports
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