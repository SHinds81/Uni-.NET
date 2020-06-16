using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NWBA.Data;

namespace NWBA.Models
{
    public class Customer
    {
        
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CustomerID { get; set; }

        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Can only be letters, no numbers or characters allowed")]
        [Required, StringLength(50)]
        public string Name { get; set; }

        [Range(0, 99999999999, ErrorMessage ="Too many digits for a Tax File Number")]
        [Display(Name = "Tax File Number")]
        public int TaxFileNumber { get; set; }

        [RegularExpression("^[ a-zA-Z0-9]*$", ErrorMessage = "Address can only contain numbers, letters and the '-' symbol")]
        [StringLength(40)]
        public string Address { get; set; }

        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Can only be letters, no numbers or characters allowed")]
        [StringLength(40)]
        public string City { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Can only be numbers, no letters or characters allowed")]
        [StringLength(4)]
        [Display(Name = "Postcode")]
        public string PostCode { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number can only contain numbers and brackets")]
        public string Phone { get; set; }

        public virtual List<Account> Accounts { get; set; }

        public void UpdateDetails(Customer newDetails)
        {
            //updates details of the customer
            Name = newDetails.Name;
            Address = newDetails.Address;
            City = newDetails.City;
            PostCode = newDetails.PostCode;
            Phone = newDetails.Phone;
            TaxFileNumber = newDetails.TaxFileNumber;
        }

        //validates a customer instance
        public static void IsValid(Customer customer, out Dictionary<string, string> errors)
        {
            errors = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(customer.Name))
                errors.Add("Name", "Name is invalid");

            if (string.IsNullOrEmpty(customer.Address))
                errors.Add("Address", "Address is invalid");

            if (string.IsNullOrEmpty(customer.City))
                errors.Add("City", "City is invalid");

            if (string.IsNullOrEmpty(customer.PostCode))
                errors.Add("PostCode", "PostCode is invalid");

            if (string.IsNullOrEmpty(customer.Phone))
                errors.Add("Phone", "Phone is invalid");

            if (customer.TaxFileNumber.ToString().Length != 11 && customer.TaxFileNumber != 0)
                errors.Add("TaxFileNumber", "TaxFileNumber is invalid");

        }

    }
}
