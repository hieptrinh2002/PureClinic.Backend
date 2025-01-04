using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureLifeClinic.Core.Entities.Business
{
    public class SendConfirmationEmailViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string ConfirmationLink { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
