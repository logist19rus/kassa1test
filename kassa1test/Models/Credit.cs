using System;
using System.Collections.Generic;
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
        
        public Credit(int _Sum, int _CreditTime, bool _PeriodType, double _CreditRate, bool _RateType, int _PayPeriod)
        {
            DateTime StartPays = CreditDate = DateTime.Today;
            DateTime EndPays;
            PeriodType = _PeriodType;
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
                CreditTime = CreditTimeDay = _CreditTime;
            }
            else
            {
                CreditTime = _CreditTime;
                CreditTimeDay = 0;
                DateTime month = DateTime.Today;
                for(int i = 0; i < _CreditTime; i++)
                {
                    CreditTimeDay += DateTime.DaysInMonth(month.AddMonths(i).Year, month.AddMonths(i).Month);
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
            GetSumRez();
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
                if (BodyPay > 0)
                {
                    nowStr += PaySum + "</td><td>";
                    nowStr += "0</td><td>";
                    BodyPay -= Math.Round(PaySum, 2);
                }
                else
                {
                    nowStr += Math.Abs(Math.Round(BodyPay, 2)) + "</td><td>";
                    nowStr += Math.Round((PaySum + BodyPay), 2) + "</td><td>";
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
