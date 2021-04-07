using System;
using System.Collections.Generic;
using System.Linq;
using EM.DB.Models;

namespace EM.API.Codes.Initiates
{
    public class InitTransactionType
    {
        Dictionary<Guid, string> TransactionTypes = new Dictionary<Guid, string>();

        public InitTransactionType()
        {
            TransactionTypes.Add(Guid.NewGuid(), "Income");
            TransactionTypes.Add(Guid.NewGuid(), "Expense");
            TransactionTypes.Add(Guid.NewGuid(), "Transfer");
        }

        public void InitTransactionTypeTable()
        {
            using (var context = new EMContext())
            {
                foreach (KeyValuePair<Guid, string> entry in TransactionTypes)
                {
                    TransactionType foundTransactionType = context.TransactionTypes.Where(x => x.TransactionTypeName == entry.Value).FirstOrDefault();

                    if (foundTransactionType == null)
                    {
                        context.TransactionTypes.Add(new TransactionType()
                        {
                            TransactionTypeID = entry.Key,
                            TransactionTypeName = entry.Value
                        });

                        context.SaveChanges();
                    }
                }
            }
        }
    }
}