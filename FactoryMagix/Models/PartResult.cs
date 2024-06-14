using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactoryMagix.Models
{
    public class PartResult
    {
        public int Result { get; set; }
        public string PartNo { get; set; }
        public string PartBatchCode { get; set; }
        public string PartSerialNo { get; set; }

        public int PartId { get; set; }

        public int MST_PartConfiguration_ID { get; set; }

        public int result { get; set; }
        public int TRN_BoxSerialNo_ID { get; set; }
        public string BoxSerial_No { get; set; }
        public string Created_On { get; set; }

        public PartResult()
        {
            PartNo = string.Empty;
            PartBatchCode = string.Empty;
            PartSerialNo = string.Empty;
            BoxSerial_No = string.Empty;
            Created_On = string.Empty;
        }
    }
    //public class PartResult
    //{
    //    public int Result { get; set; }
    //    public string PartNo { get; set; }
    //    public string PartBatchCode { get; set; }
    //    public string PartSerialNo { get; set; }

    //    public PartResult()
    //    {
    //        PartNo = string.Empty;
    //        PartBatchCode = string.Empty;
    //        PartSerialNo = string.Empty;
    //    }
    //}
}