using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_SMS.BLL
{
    class SMS_Group
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public string MakerWorkStationId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }

    }
}
