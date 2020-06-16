using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NWBA_API.Models
{
    public enum LoginStatus
    {
        OPEN = 1,
        LOCKED = 2,
        TEMP_LOCKED = 3,
    }

    public class Login
    {
        [Range(10000000, 99999999, ErrorMessage = "Login ID must be 8 digits long")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string LoginID { get; set; }

        [Range(1000, 9999, ErrorMessage = "Customer ID must be 4 digits long")]
        [Required, ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        public LoginStatus Status { get; set; }

        [Required, StringLength(64)]
        public string PasswordHash { get; set; }

        [DataType(DataType.Date)]
        public DateTime ModifyTime { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime LockedUntil { get; set; }
    }
}
