using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace NWBA_Admin
//namespace CustomDataAnnotations
{

    //This class is to make that dates before the current date aren't entered
    public class CurrentDateAttribute : ValidationAttribute
    {
        public CurrentDateAttribute() { }
        public override bool IsValid(object value)
        {
            return ((DateTime)value >= DateTime.Now);

        }
    }
}
