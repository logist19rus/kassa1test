using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;

namespace kassa1test.Models
{
    public class Credit
    {
        int Sum;// Размер кредита
        double SumRes; // Сумма кредита с процентами
        int CreditTime; // Срок кредита с формы
        int CreditTimeDay; // Срок кредита в днях
        double CreditRate; // Ставка в процентах
        int PayPeriod; // Периодичность выплат(30 = месяц)
        bool RateType;//периодичность начисления ставки по кредиту => false-ежегодно true-ежедневно
        bool PeriodType; // тип срока кредита => false-месяцев true-дней
        double PaySum; // размер выплат по кредиту в период (вычисляется)
        DateTime CreditDate; //дата взятия кредита(считается в момент генерации класса)
        List<DateTime> PayDay = new List<DateTime>(); //список всех дней в которые происходит выплата по кредиту
        List<string> ErrorInputs = new List<string>();
        
        public Credit(int _Sum, int _CreditTime, bool _PeriodType, double _CreditRate, bool _RateType, int _PayPeriod)
        {
            Regex regExNumeric = new Regex("^[0-9]+$");
            Regex regExDouble = new Regex("^[0-9]+(.[0-9]+)?$");
            DateTime StartPays = CreditDate = DateTime.Today;
            DateTime EndPays;
            PeriodType = _PeriodType;

            if (_Sum>0)
            {
                Sum = _Sum;
            }
            else
            {
                ErrorInputs.Add("Сумма кредита");
            }

            if (_CreditTime > 0)
            {
                if (_PeriodType)
                {
                    CreditTime = CreditTimeDay = _CreditTime;
                }
                else
                {
                    CreditTime = _CreditTime;
                    CreditTimeDay = 0;
                    DateTime month = DateTime.Today;
                    for (int i = 0; i < _CreditTime; i++)
                    {
                        CreditTimeDay += DateTime.DaysInMonth(month.AddMonths(i).Year, month.AddMonths(i).Month);
                    }
                }
            }
            else
            {
                ErrorInputs.Add("Срок кредита");
            }

            if (_CreditRate > 0)
            {
                CreditRate = _CreditRate;
            }
            else
            {
                ErrorInputs.Add("Ставка");
            }

            if (_PayPeriod == 30 || _PayPeriod == 15 || _PayPeriod == 10)
            {
                PayPeriod = _PayPeriod;
            }
            else
            {
                ErrorInputs.Add("Шаг платежа");
            }

            if (_PeriodType)
            {
                EndPays = StartPays.AddDays(_CreditTime);
            }
            else
            {
                EndPays = StartPays.AddMonths(_CreditTime);
            }

            RateType = _RateType;

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
            GetSumRez();
            if (ErrorInputs.Count > 0)
            {
                string errorMsg = "";
                errorMsg += "<ul class=\"list - group\"><li class=\"list-group-item list-group-item-warning\">В следующих полях были допущены ошибки:</li>";
                foreach(var x in ErrorInputs)
                {
                    errorMsg += "<li class=\"list-group-item list-group-item-danger\">" + x + "</li>";
                }
                errorMsg += "</ul>";
                throw new Exception(errorMsg);
            }
        }

        void GetSumRez()
        {
            //имена переменных взяты с формулы для простоты ассоциирования
            int n = PayDay.Count;//количество периодов 
            double i = CreditRate / 100;//ставка по кредиту за период
            double K;//коэффициент аннуитета

            //if (!PeriodType)
            //{
            //    n = CreditTime;
            //}
            //else
            //{
            //    n = CreditTimeDay / PayPeriod;
            //}

            if (!RateType)
            {
                i /= 12;
            }
            else
            {
                i *= PayPeriod;
            }

            //if(!RateType && !PeriodType)
            //{
            //    n = CreditTime;
            //    i = (CreditRate / 100) / 12;
            //}
            //else if (RateType && !PeriodType)
            //{
            //    n = CreditTime;
            //    i = (CreditRate / 100) * 30;
            //}
            //else if (!RateType && PeriodType)
            //{
            //    n = CreditTimeDay / PayPeriod;
            //    i = (CreditRate / 100) / (365 / PayPeriod);
            //}
            //else
            //{
            //    n = CreditTimeDay / PayPeriod;
            //    i = (CreditRate / 100) * PayPeriod;
            //}
            K = (i * Step((1 + i), n)) / (Step((1 + i), n) - 1);
            PaySum = Math.Round(Sum * K, 2);
            SumRes = PaySum * n;
        }

        public List<string> getPayTable()
        {
            List<string> rezTable = new List<string>();
            double CreditOst = SumRes;
            double BodyPay = Sum;

            for (int i = 0; i < PayDay.Count; i++)
            {
                CreditOst -= PaySum;
                string nowStr = "";
                nowStr += "<tr><th class=\"row\">";
                nowStr += (i + 1).ToString() + "</th><td>";
                nowStr += PayDay[i].ToString("dd.MM.yyyy") + "</td><td>";
                if (BodyPay >= PaySum)
                {
                    nowStr += PaySum + "</td><td>";
                    nowStr += "0</td><td>";
                    BodyPay -= Math.Round(PaySum, 2);
                }
                else
                {
                    nowStr += Math.Round(BodyPay, 2) + "</td><td>";
                    nowStr += Math.Round((PaySum - BodyPay), 2) + "</td><td>";
                    BodyPay -= BodyPay;
                }
                nowStr += Math.Round(CreditOst, 2) + "</td></tr>";
                rezTable.Add(nowStr);
            }
            rezTable.Add("<tr><th>Переплата по кредиту:</th><td></td><td></td><td></td><td>" + Math.Round((SumRes - Sum), 2) + "</td></tr>");
            return rezTable;
        }

        double Step(double num, int count)
        {
            double rez = 1;
            for(int i = 0; i < count; i++)
            {
                rez *= num;
            }
            return rez;
        }
    }
}
