using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kassa1test.Models
{
    public class Credit
    {
        int Sum;
        double SumRes;
        int CreditTime;
        double CreditRate;
        int PayPeriod;
        bool RateType;
        DateTime CreditDate;
        List<DateTime> PayDay = new List<DateTime>();
        
        public Credit(int _Sum, int _CreditTime, bool _PeriodType, int _CreditRate, bool _RateType, int _PayPeriod)
        {
            DateTime StartPays = CreditDate = DateTime.Today;
            DateTime EndPays;
            if (_PeriodType)
            {
                EndPays = StartPays.AddDays(_CreditTime);
            }
            else
            {
                EndPays = StartPays.AddMonths(_CreditTime);
            }
            Sum = _Sum;
            if (_PeriodType)
            {
                CreditTime = _CreditTime;
            }
            else
            {
                CreditTime = 0;
                DateTime month = DateTime.Today;
                for(int i = 0; i < _CreditTime; i++)
                {
                    CreditTime += DateTime.DaysInMonth(month.AddMonths(i).Year, month.AddMonths(i).Month);
                }
            }
            CreditRate = _CreditRate;
            RateType = _RateType;
            PayPeriod = _PayPeriod;

            while(EndPays > StartPays)
            {
                PayDay.Add(StartPays);
                if(_PayPeriod == 30)
                {
                    StartPays = StartPays.AddMonths(1);
                }
                else
                {
                    StartPays = StartPays.AddDays(_PayPeriod);
                }
            }
        }

        void GetSumRez()
        {

        }
    }
}
