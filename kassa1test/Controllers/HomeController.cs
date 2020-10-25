using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using kassa1test.Models;

namespace kassa1test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index( int Sum = 0,
         int CreditTime = 0,
         bool PeriodType = false,
         double CreditRate = 0.0,
         bool RateType = false,
         int PayPeriod = 30,
         string errorMsg = "")
        {
            var cForm = new CreditForm(Sum, CreditTime, PeriodType, CreditRate, RateType, PayPeriod, errorMsg);
            return View(cForm);
        }

        [HttpPost]
        public IActionResult KreditResult(int Sum, int CreditTime, bool PeriodType, double CreditRate, bool RateType, int PayPeriod)
        {
            try
            {
                Credit credit = new Credit(Sum, CreditTime, PeriodType, CreditRate, RateType, PayPeriod);
                return View(credit);
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", "Home", new { Sum, CreditTime, PeriodType, CreditRate, RateType, PayPeriod, errorMsg = ex.Message });
            }
        }
        [HttpGet]
        public IActionResult KreditResult()
        {
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
