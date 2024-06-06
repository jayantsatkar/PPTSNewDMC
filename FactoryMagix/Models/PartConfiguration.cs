using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactoryMagix.Models
{
    public class PartConfiguration : Customer
    {
        public int MST_PartConfiguration_ID { get; set; }
        public string BoschPart_No { get; set; }
        public new string Customer_Index { get; set; }
        public new int MST_Customer_ID { get; set; }
        public string BoschPart_Desc { get; set; }
        public string CustPart_No { get; set; }
        public int NoOfPartQy_Box { get; set; }
        public string Packed_In { get; set; }
        public string Line_Id { get; set; }
        public string Line_No { get; set; }
        public new int Created_By { get; set; }
        public new int? Modified_By { get; set; }

        public PartConfiguration()

        {

            BoschPart_No = string.Empty;

            Customer_Index = string.Empty;

            BoschPart_Desc = string.Empty;

            CustPart_No = string.Empty;

            Packed_In = string.Empty;

            Line_Id = string.Empty;

            Line_No = string.Empty;
        }
    }
}