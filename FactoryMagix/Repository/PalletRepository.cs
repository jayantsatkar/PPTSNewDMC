using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FactoryMagix.Repository
{
    public class PalletRepository
    {
        public static DataTable GetPalletReprintDetails(string serialNo, int flag)
        {

            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            DBHelper.AddSqlParameter("@SerialNo", serialNo, ref sqlParameters);
            DBHelper.AddSqlParameter("@FlagforPalletorBox", flag, ref sqlParameters);


            var dtUsers = DBHelper.ExecuteProcedure("spGetPalletBarcodeDataprint", sqlParameters);


            return dtUsers;

        }
    }
}