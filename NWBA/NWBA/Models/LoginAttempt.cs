using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations.Schema;
using NWBA.Data;

namespace NWBA.Models
{
    public class LoginAttempt
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoginAttemptID { get; set; }

        [Required, ForeignKey("Customer")]
        public int CustomerID { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Login Time")]
        public DateTime LoginTime { get; set; }

        [Required]
        public bool Successfull { get; set; }
    }
}
