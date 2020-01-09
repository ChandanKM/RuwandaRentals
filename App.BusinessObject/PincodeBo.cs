using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BusinessObject
{
    public class PincodeBo
    {
        public string Pincode { get; set; }
    }

    public class EditPincodeBo
    {
        public int PincodeId { get; set; }
        public string Pincode { get; set; }
    }
}
