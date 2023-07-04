using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicProject.Models
{
    public class Owner
    {
        public Guid OwnerId{ get; set; }
        public string Owner_Name { get; set; }
        public string Owner_Email { get; set; }
        public int Owner_PhoneNumber { get; set; }

        public double Owner_Amount { get; set; }

        [NotMapped]
        public string Onwer_Password { get; set; }
        public string Owner_Note { get; set; }
        public string Onwer_Technical { get; set; }
        
    }
}
