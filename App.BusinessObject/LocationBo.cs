using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BusinessObject
{
   
    public class LocationBo
    {
        public string Location_desc { get; set; }
    }

    public class EditLocationBo
    {
        public string Location_Id { get; set; }
        public string Location_desc { get; set; }
    }
}
