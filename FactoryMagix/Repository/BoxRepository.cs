using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using FactoryMagix.Models;
using System.Data;
using Newtonsoft.Json;

namespace FactoryMagix.Repository
{
    public class BoxRepository
    {
        public static DataTable GetBoxReprintDetails(string serialNo, int flag)
        {
           
            List <SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@SerialNo", serialNo, ref sqlParameters);
            DBHelper.AddSqlParameter("@FlagforPalletorBox", flag, ref sqlParameters);


            var dtUsers = DBHelper.ExecuteProcedure("spGetBarcodeDataprint", sqlParameters);
                       
                      
            return dtUsers;
                    
        }
    }
}