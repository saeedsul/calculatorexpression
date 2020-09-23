using Calculator.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Calculator.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new CalculatorViewModel());
        }

        [HttpPost]
        public IActionResult Index(CalculatorViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var result = Common.Calculator.Calculate(viewModel.Expression);
            
            viewModel.Results = result;

            return View(viewModel);
        }
    }
}
