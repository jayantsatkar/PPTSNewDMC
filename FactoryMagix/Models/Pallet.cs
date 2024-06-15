using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace FactoryMagix.Models
{
    public class Pallet
    {
        public int PalletId { get; set; }

        public string PalletNumber { get; set; }

        public string InvoiceNumber { get; set; }

        public int PartId { get; set; }

        public string InvoiceDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int ModifiedBy { get; set; }


        public Pallet()
        {
            PalletNumber = string.Empty;
            InvoiceNumber = string.Empty;
            InvoiceDate = string.Empty;
            CreatedBy = string.Empty;
            CreatedOn = DateTime.Now;
        }
    }
}