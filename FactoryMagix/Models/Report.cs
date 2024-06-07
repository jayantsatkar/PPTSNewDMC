using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactoryMagix.Models
{
    public class Report
    {
    }

    public class PartSerialNoReport
    {
        public int SrNo { get; set; }
        public string InvoiceNo { get; set; }
        public string PalletSerial_No { get; set; }
        public string PalletBatchCode { get; set; }
        public string BoxSerialNo { get; set; }
        public string BoxBatchCode { get; set; }
        public string Boschpart_No { get; set; }
        public string PartSerialNo { get; set; }
        public string PartBatchCode { get; set; }
        public string Created_On { get; set; }
        public string Login_ID { get; set; }

        public PartSerialNoReport()
        {
            InvoiceNo = string.Empty;
            PalletSerial_No = string.Empty;
            PalletBatchCode = string.Empty;
            BoxSerialNo = string.Empty;
            BoxBatchCode = string.Empty;
            Boschpart_No = string.Empty;
            PartSerialNo = string.Empty;
            PartBatchCode = string.Empty;
            Created_On = string.Empty;
            Login_ID = string.Empty;

        }
    }

    //public class InvoiceReport
    //{
    //    public int SrNo { get; set; }
    //    public string InvoiceNo { get; set; }
    //    public string PalletSerial_No { get; set; }
    //    public string PalletBatchCode { get; set; }
    //    public string BoxSerialNo { get; set; }
    //    public string BoxBatchCode { get; set; }
    //    public string Boschpart_No { get; set; }
    //    public string PartSerialNo { get; set; }
    //    public string PartBatchCode { get; set; }
    //    public string Created_On { get; set; }
    //    public string Login_ID { get; set; }
    //}

    public class ErrorLogReport
    {
        public string Login_ID { get; set; }
        public string Login_Name { get; set; }
        public string Error_Description { get; set; }
        public int Part_Config__No { get; set; }
        public string CustPart_No { get; set; }
        public string Approved_By { get; set; }
        public string Approved_By_Name { get; set; }
        public Nullable<System.DateTime> Created_On { get; set; }
    }

    public class ErrorLogReport2
    {

        public string Login_ID { get; set; }
        public string Login_Name { get; set; }
        public string Error_Description { get; set; }
        public Nullable<long> Part_Config__No { get; set; }
        public string CustPart_No { get; set; }
        public string Approved_By { get; set; }
        public string Approved_By_Name { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public Nullable<long> InvoiceQty { get; set; }
        public Nullable<System.DateTime> Created_On { get; set; }
    }

    public class PalletReport
    {
        public Nullable<long> SrNo { get; set; }
        public string InvoiceNo { get; set; }
        public string PalletSerial_No { get; set; }
        public string PalletBatchCode { get; set; }
        public string BoxSerialNo { get; set; }
        public string BoxBatchCode { get; set; }
        public string Boschpart_No { get; set; }
        public string PartSerialNo { get; set; }
        public string PartBatchCode { get; set; }
        public string Created_On { get; set; }
        public string Login_ID { get; set; }
    }

    public class BoxSerialNoReport
    {
        public Nullable<long> SrNo { get; set; }
        public string InvoiceNo { get; set; }
        public string PalletSerial_No { get; set; }
        public string PalletBatchCode { get; set; }
        public string BoxSerialNo { get; set; }
        public string BoxBatchCode { get; set; }
        public string Boschpart_No { get; set; }
        public string PartSerialNo { get; set; }
        public string PartBatchCode { get; set; }
        public string Created_On { get; set; }
        public string Login_ID { get; set; }
    }
}