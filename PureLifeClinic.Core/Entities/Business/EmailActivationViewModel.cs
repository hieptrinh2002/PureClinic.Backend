using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureLifeClinic.Core.Entities.Business
{
    public class EmailActivationViewModel
    {
        public int UserId { get; set; }  
        public string ActivationToken { get; set; }

        public string ActivationUrl { get; set; }
    }
}
