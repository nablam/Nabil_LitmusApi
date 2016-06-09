using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLitmusApp.Models
{
    public class AccountDataObject
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Creationdate { get; }
        public string TimeLeft { get; }

        public AccountDataObject(string fname, string lname, string created) {
            FirstName = fname;
            LastName = lname;
            Creationdate = created;
            TimeLeft = calculateTimeLeft(Creationdate);
        }

        string calculateTimeLeft(string dateStartedSTR) {
            string calculatedTimeLeft = "??";
            if (!string.IsNullOrEmpty(dateStartedSTR)) {
                DateTime now_DATE = DateTime.Now;

                string[] individualnumbers = dateStartedSTR.Split(new Char[] { '-', 'T', ':' });

                int year = Int32.Parse(individualnumbers[0]);
                int month = Int32.Parse(individualnumbers[1]);
                int day = Int32.Parse(individualnumbers[2]);
                int hour = Int32.Parse(individualnumbers[3]);
                int min = Int32.Parse(individualnumbers[4]);
              //  int sec = Int32.Parse(individualnumbers[5]);

                
                DateTime Started_DATE = new DateTime(year, month, day, hour, min,0);
                DateTime Enging_DATE = Started_DATE.AddDays(30);

                TimeSpan timeleft = Enging_DATE.Subtract(now_DATE);

        

                calculatedTimeLeft = timeleft.Days + " Days " + timeleft.Hours + " Hours";
            }
            

            return calculatedTimeLeft;
        }

    }
}