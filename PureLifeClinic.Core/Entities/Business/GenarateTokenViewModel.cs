using PureLifeClinic.Core.Entities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureLifeClinic.Core.Entities.Business
{
    public class GenarateTokenViewModel
    {
        public string AccessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }

        public DateTime CreateOn { get; set; }
        public DateTime ExpireOn { get; set; }
        public string AccessTokenId { get; set; }
    }
}
