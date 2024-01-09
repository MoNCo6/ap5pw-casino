using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Application.ViewModels
{
    public class DepositViewModel
    {
        [Required]
        [Range(1, 10000)] // Set appropriate range
        public decimal Amount { get; set; }

        // Add other properties if needed, like PaymentMethod
    }
}
