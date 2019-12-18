using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using _2C2P.Database;
using _2C2P.Models;
using System.Globalization;

namespace _2C2P.Controllers.APIController
{
    public class TransactionsController : ApiController
    {
        private Entities db = new Entities();

        // GET: api/Transactions?currency=USD
        [ResponseType(typeof(Transaction))]
        public List<TransactionViewModel> GetTransactionByCurrency(string currency)
        {
            List<TransactionViewModel> ListData = (from data in db.Transactions
                                                   where data.CurrencyCode == currency

                                                   select new TransactionViewModel
                                                   {
                                                       id = data.TransactionID,
                                                       payment = data.Amount + " " + data.CurrencyCode,
                                                       Status = data.Status
                                                   }).ToList();

            return ListData;
        }

        // GET: api/Transactions?daterange=ddMMyyyy-ddMMyyyy
        [ResponseType(typeof(Transaction))]
        public List<TransactionViewModel> GetTransactionByDate(string daterange)
        {
            DateTime StartDate = DateTime.ParseExact(daterange.Split('-').ToList<string>()[0], "ddMMyyyy", CultureInfo.InvariantCulture);
            DateTime EndDate = DateTime.ParseExact(daterange.Split('-').ToList<string>()[1], "ddMMyyyy", CultureInfo.InvariantCulture);

            List<TransactionViewModel> ListData = (from data in db.Transactions
                                                   where StartDate <= data.TransactionDate && data.TransactionDate <= EndDate

                                                   select new TransactionViewModel
                                                   {
                                                       id = data.TransactionID,
                                                       payment = data.Amount + " " + data.CurrencyCode,
                                                       Status = data.Status
                                                   }).ToList();
            return ListData;
        }

        // GET: api/Transactions?status=A
        public List<TransactionViewModel> GetTransactionByStatus(string status)
        {
            List<TransactionViewModel> ListData = (from data in db.Transactions
                                                   where data.Status == status

                                                   select new TransactionViewModel
                                                   {
                                                       id = data.TransactionID,
                                                       payment = data.Amount + " " + data.CurrencyCode,
                                                       Status = data.Status
                                                   }).ToList();
            return ListData;
        }
    }
}