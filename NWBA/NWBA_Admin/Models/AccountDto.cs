using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NWBA_Admin.Models
{
    public enum AccountType
    {
        Checking = 1,
        Saving = 2
    }

    public class AccountDto
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }

        [Required, Display(Name = "Account Type")]
        public AccountType Type { get; set; }

        [Range(-0.01, Int32.MaxValue, ErrorMessage = "Value can not be a negative number")]
        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }

        [Required, ForeignKey("Customer")]
        public int CustomerID { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Motify Time")]
        public DateTime ModifyTime { get; set; }
        public int FreeTransactions { get; set; }

        public virtual List<TransactionDto> Transactions { get; set; }

    }
}
