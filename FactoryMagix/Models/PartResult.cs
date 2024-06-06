using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactoryMagix.Models
{
    public class PartResult
    {
        public int RESULT { get; set; }
        public string PartNo { get; set; }
        public string PartBatchCode { get; set; }
        public string PartSerialNo { get; set; }

        public int PartId { get; set; }

        public PartResult()
        {
            PartNo = string.Empty;
            PartBatchCode = string.Empty;
            PartSerialNo = string.Empty;
        }
    }
}