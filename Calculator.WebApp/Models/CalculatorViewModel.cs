using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Calculator.WebApp.Models
{
    public class CalculatorViewModel
    {
        [Required, MinLength(3)]
        [RegularExpression(@"^\s*(\d+)(?:\s*([-+*\/])\s*(?:\d+)\s*)+$", ErrorMessage ="Invalid expression")]
        public string Expression { get; set; }
        public string Results { get; set; }
    }
}
