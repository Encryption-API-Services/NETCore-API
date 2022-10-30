using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Mongo.Entities
{
    public class FailedLoginAttempt
    {
        public string ID { get; set; }
        public string UserAccount { get; set; }
        public string Password { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastModifed { get; set; }
    }
}
