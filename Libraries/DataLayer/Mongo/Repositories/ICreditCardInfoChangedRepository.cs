using DataLayer.Mongo.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Mongo.Repositories
{
    public interface ICreditCardInfoChangedRepository
    {
        Task InsertCreditCardInformationChanged(CreditCardInfoChanged changedInfo);
    }
}