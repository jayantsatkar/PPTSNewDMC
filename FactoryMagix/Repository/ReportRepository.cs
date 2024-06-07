using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FactoryMagix.Models;
namespace FactoryMagix.Repository
{
    public class ReportRepository
    {
        public static IList<PartSerialNoReport> GetPartSerialNoReport(string PartSrNo, Boolean Type)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@PartSrNoorBatchCode", PartSrNo, ref sqlParameters);
            DBHelper.AddSqlParameter("@PartSerialNowise", Type, ref sqlParameters);

            var dtPartSerialNoReport = DBHelper.ExecuteProcedure("spGetPartSerialNoReport", sqlParameters);
            List<PartSerialNoReport> partSerialNoReports = new List<PartSerialNoReport>();

            if (dtPartSerialNoReport.Rows.Count > 0)
            {
                foreach (DataRow dr in dtPartSerialNoReport.Rows)
                {
                    partSerialNoReports.Add(new PartSerialNoReport
                    {
                        SrNo = Convert.ToInt32(dr["SrNo"]),
                        InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                        PalletSerial_No = Convert.ToString(dr["PalletSerial_No"]),
                        PalletBatchCode = Convert.ToString(dr["PalletBatchCode"]),
                        BoxSerialNo = Convert.ToString(dr["BoxSerialNo"]),
                        BoxBatchCode = Convert.ToString(dr["BoxBatchCode"]),
                        Boschpart_No = Convert.ToString(dr["Boschpart_No"]),
                        PartSerialNo = Convert.ToString(dr["PartSerialNo"]),
                        PartBatchCode = Convert.ToString(dr["PartBatchCode"]),
                        Created_On = Convert.ToString(dr["Created_On"]),
                        Login_ID = Convert.ToString(dr["Login_ID"])
                    });
                }

            }

            return partSerialNoReports;
        }

        public static IList<PartSerialNoReport> CommonReport(string InvoiceNo, DateTime fromDate, DateTime todate, Boolean Datewisereport, string ProcName)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            if(ProcName == "spGetPalletWiseReport")
            {
                DBHelper.AddSqlParameter("@PalletNo", InvoiceNo, ref sqlParameters);
            }
            else if(ProcName == "spGetInvoiceReport")
            {
                DBHelper.AddSqlParameter("@InvoiceNo", InvoiceNo, ref sqlParameters);
            }

            else if(ProcName == "spGetBoxSerialNoReport")
            {
                DBHelper.AddSqlParameter("@BoSrNoNo", InvoiceNo, ref sqlParameters);
            }
            
            DBHelper.AddSqlParameter("@fromDate", fromDate, ref sqlParameters);
            DBHelper.AddSqlParameter("@Todate", todate, ref sqlParameters);
            DBHelper.AddSqlParameter("@Datewisereport", Datewisereport, ref sqlParameters);

            var dtPartSerialNoReport = DBHelper.ExecuteProcedure(ProcName, sqlParameters);
            List<PartSerialNoReport> partSerialNoReports = new List<PartSerialNoReport>();
            if (dtPartSerialNoReport.Rows.Count > 0)
            {
                foreach (DataRow dr in dtPartSerialNoReport.Rows)
                {
                    partSerialNoReports.Add(new PartSerialNoReport
                    {
                        SrNo = Convert.ToInt32(dr["SrNo"]),
                        InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                        PalletSerial_No = Convert.ToString(dr["PalletSerial_No"]),
                        PalletBatchCode = Convert.ToString(dr["PalletBatchCode"]),
                        BoxSerialNo = Convert.ToString(dr["BoxSerialNo"]),
                        BoxBatchCode = Convert.ToString(dr["BoxBatchCode"]),
                        Boschpart_No = Convert.ToString(dr["Boschpart_No"]),
                        PartSerialNo = Convert.ToString(dr["PartSerialNo"]),
                        PartBatchCode = Convert.ToString(dr["PartBatchCode"]),
                        Created_On = Convert.ToString(dr["Created_On"]),
                        Login_ID = Convert.ToString(dr["Login_ID"])
                    });
                }

            }

            return partSerialNoReports;
        }

        public static IList<PartSerialNoReport> GetInvoiceReport(string InvoiceNo, DateTime FromDate, DateTime ToDate, Boolean Flag)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@InvoiceNo", InvoiceNo, ref sqlParameters);
            DBHelper.AddSqlParameter("@fromDate", FromDate, ref sqlParameters);
            DBHelper.AddSqlParameter("@Todate", ToDate, ref sqlParameters);
            DBHelper.AddSqlParameter("@Datewisereport", Flag, ref sqlParameters);

            var dtPartSerialNoReport = DBHelper.ExecuteProcedure("spGetInvoiceReport", sqlParameters);
            List<PartSerialNoReport> partSerialNoReports = new List<PartSerialNoReport>();
            if (dtPartSerialNoReport.Rows.Count > 0)
            {
                foreach (DataRow dr in dtPartSerialNoReport.Rows)
                {
                    partSerialNoReports.Add(new PartSerialNoReport
                    {
                        SrNo = Convert.ToInt32(dr["SrNo"]),
                        InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                        PalletSerial_No = Convert.ToString(dr["PalletSerial_No"]),
                        PalletBatchCode = Convert.ToString(dr["PalletBatchCode"]),
                        BoxSerialNo = Convert.ToString(dr["BoxSerialNo"]),
                        BoxBatchCode = Convert.ToString(dr["BoxBatchCode"]),
                        Boschpart_No = Convert.ToString(dr["Boschpart_No"]),
                        PartSerialNo = Convert.ToString(dr["PartSerialNo"]),
                        PartBatchCode = Convert.ToString(dr["PartBatchCode"]),
                        Created_On = Convert.ToString(dr["Created_On"]),
                        Login_ID = Convert.ToString(dr["Login_ID"])
                    });
                }

            }
            return partSerialNoReports;
        }


        public static IList<ErrorLogReport> GetUserErrorLogReport(string LoginId)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@User_ID", LoginId, ref sqlParameters);


            var dataTable = DBHelper.ExecuteProcedure("spUserErrorLogReport", sqlParameters);
            List<ErrorLogReport> errorLogReports = new List<ErrorLogReport>();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    errorLogReports.Add(new ErrorLogReport
                    {
                        Login_ID = Convert.ToString(dr["Login ID"]),
                        Login_Name = Convert.ToString(dr["Login Name"]),
                        Error_Description = Convert.ToString(dr["Error Description"]),
                        Part_Config__No = Convert.ToInt32(dr["Part Config. No"]),
                        CustPart_No = Convert.ToString(dr["CustPart_No"]),
                        Approved_By = Convert.ToString(dr["Approved_By"]),
                        Approved_By_Name = Convert.ToString(dr["Approved_By Name"]),
                        Created_On = Convert.ToDateTime(dr["Created_On"])
                    });
                }

            }
            return errorLogReports;
        }

        public static IList<ErrorLogReport2> GetUserErrorLogReport2(string LoginId)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@User_ID", LoginId, ref sqlParameters);


            var dataTable = DBHelper.ExecuteProcedure("spUserErrorLogReport_2", sqlParameters);
            List<ErrorLogReport2> errorLogReports = new List<ErrorLogReport2>();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    errorLogReports.Add(new ErrorLogReport2
                    {
                        Login_ID = Convert.ToString(dr["Login ID"]),
                        Login_Name = Convert.ToString(dr["Login Name"]),
                        Error_Description = Convert.ToString(dr["Error Description"]),
                        Part_Config__No = Convert.ToInt32(dr["Part Config.No"]),
                        CustPart_No = Convert.ToString(dr["CustPart_No"]),
                        Approved_By = Convert.ToString(dr["Approved_By"]),
                        Approved_By_Name = Convert.ToString(dr["Approved_By Name"]),
                        Created_On = Convert.ToDateTime(dr["Created_On"]),
                        InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                        InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                        InvoiceQty = Convert.ToInt64(dr["InvoiceQty"])
                    });
                }

            }
            return errorLogReports;
        }


        public static IList<PalletReport> GetPalletWiseReport(string PalletNo, DateTime FromDate, DateTime ToDate, Boolean Flag)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@PalletNo", PalletNo, ref sqlParameters);
            DBHelper.AddSqlParameter("@fromDate", FromDate, ref sqlParameters);
            DBHelper.AddSqlParameter("@toDate", ToDate, ref sqlParameters);
            DBHelper.AddSqlParameter("@Datewisereport", Flag, ref sqlParameters);

            var dataTable = DBHelper.ExecuteProcedure("spGetPalletWiseReport", sqlParameters);
            List<PalletReport> palletReports = new List<PalletReport>();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    palletReports.Add(new PalletReport
                    {
                        SrNo = Convert.ToInt64(dr["SrNo"]),
                        InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                        PalletSerial_No = Convert.ToString(dr["PalletSerial_No"]),
                        PalletBatchCode = Convert.ToString(dr["PalletBatchCode"]),
                        BoxSerialNo = Convert.ToString(dr["BoxSerialNo"]),
                        BoxBatchCode = Convert.ToString(dr["BoxBatchCode"]),
                        Boschpart_No = Convert.ToString(dr["Boschpart_No"]),
                        PartSerialNo = Convert.ToString(dr["PartSerialNo"]),
                        PartBatchCode = Convert.ToString(dr["PartBatchCode"]),

                        Created_On = Convert.ToString(dr["Created_On"]),
                        Login_ID = Convert.ToString(dr["Login_ID"])
                    });
                }

            }
            return palletReports;
        }


        public static IList<BoxSerialNoReport> GetBoxSerialNoReport(string BoxSrNo, DateTime FromDate, DateTime ToDate, Boolean Flag)
        {
            // db.spGetBoxSerialNoReport(BoxSrNo, DateTime.Now, DateTime.Now, false).ToList();
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@BoSrNoNo", BoxSrNo, ref sqlParameters);
            DBHelper.AddSqlParameter("@fromDate", FromDate, ref sqlParameters);
            DBHelper.AddSqlParameter("@toDate", ToDate, ref sqlParameters);
            DBHelper.AddSqlParameter("@Datewisereport", Flag, ref sqlParameters);


            var dataTable = DBHelper.ExecuteProcedure("spGetBoxSerialNoReport", sqlParameters);
            List<BoxSerialNoReport> boxSerialNoReports = new List<BoxSerialNoReport>();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    boxSerialNoReports.Add(new BoxSerialNoReport
                    {
                        SrNo = Convert.ToInt64(dr["SrNo"]),
                        InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                        PalletSerial_No = Convert.ToString(dr["PalletSerial_No"]),
                        PalletBatchCode = Convert.ToString(dr["PalletBatchCode"]),
                        BoxSerialNo = Convert.ToString(dr["BoxSerialNo"]),
                        BoxBatchCode = Convert.ToString(dr["BoxBatchCode"]),
                        Boschpart_No = Convert.ToString(dr["Boschpart_No"]),
                        PartSerialNo = Convert.ToString(dr["PartSerialNo"]),
                        PartBatchCode = Convert.ToString(dr["PartBatchCode"]),
                        Created_On = Convert.ToString(dr["Created_On"]),
                        Login_ID = Convert.ToString(dr["Login_ID"])
                    });
                }

            }
            return boxSerialNoReports;
        }
    }
}