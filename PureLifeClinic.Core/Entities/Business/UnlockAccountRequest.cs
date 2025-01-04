using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureLifeClinic.Core.Entities.Business
{
    public class UnlockAccountRequest
    {
        [Required]
        public int UserId { get; set; }
    }
}
