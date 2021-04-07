using System;
using System.Collections.Generic;
using System.Linq;
using EM.DB.Models;

namespace EM.API.Codes.Initiates
{
    public class InitAccountType
    {
        Dictionary<Guid, string> AccountTypes = new Dictionary<Guid, string>();

        public InitAccountType()
        {
            AccountTypes.Add(Guid.NewGuid(), "Cash");
            AccountTypes.Add(Guid.NewGuid(), "Card");
            AccountTypes.Add(Guid.NewGuid(), "Account");
        }

        public void InitAccountTypeTable()
        {
            using (var context = new EMContext())
            {
                foreach (KeyValuePair<Guid, string> entry in AccountTypes)
                {
                    AccountType foundAccountType = context.AccountTypes.Where(x => x.AccountTypeName == entry.Value).FirstOrDefault();

                    if (foundAccountType == null)
                    {
                        context.AccountTypes.Add(new AccountType()
                        {
                            AccountTypeID = entry.Key,
                            AccountTypeName = entry.Value
                        });

                        context.SaveChanges();
                    }
                }
            }
        }
    }
}