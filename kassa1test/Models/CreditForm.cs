using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kassa1test.Models
{
    public class CreditForm
    {
        public int Sum = 0;
        public int CreditTime = 0;
        public bool PeriodType = false;
        public double CreditRate = 0.0;
        public bool RateType = false;
        public int PayPeriod = 30;
        public string errorMsg = "";

        public CreditForm(int _Sum, int _CreditTime, bool _PeriodType, double _CreditRate, bool _RateType, int _PayPeriod, string _msg = "")
        {
            Sum = _Sum;
            CreditTime = _CreditTime;
            PeriodType = _PeriodType;
            CreditRate = _CreditRate;
            RateType = _RateType;
            PayPeriod = _PayPeriod;
            errorMsg = _msg;
        }
        public CreditForm()
        {

        }
    }
}
