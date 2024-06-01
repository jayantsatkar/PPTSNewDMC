//using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryMagix
{
    public static class DBHelper
    {
      //  private static readonly ILog Logger =
        //      LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //private static IConfiguration config;
        //public static IConfiguration Configuration
        //{
        //    get
        //    {
        //        var builder = new ConfigurationBuilder()
        //            .SetBasePath(Directory.GetCurrentDirectory())
        //            .AddJsonFile("appsettings.json");
        //        config = builder.Build();
        //        return config;
        //    }
        //}


        public static string DefaultConnectionString
        {
            get
            {
                // return Configuration.GetConnectionString("DBConnection");
                //return ConfigurationManager.AppSettings["apiUrl"].ToString();
                return ConfigurationManager.ConnectionStrings["Laser"].ToString();
            }
        }

        public static DataTable ExecuteProcedure(string PROC_NAME, List<SqlParameter> parameters)
        {
            string query = PROC_NAME;

            return Query(query, parameters);
        }

        public static DataTable ExecuteQuery(string query, List<SqlParameter> parameters)
        {
            string queryString = "EXEC " + query;
            return Query(queryString, parameters);
        }

        public static int ExecuteNonQuery(string query, List<SqlParameter> parameters)
        {
            return NonQuery(query, parameters);
        }
        public static DataSet ExecuteProcedureDataSet(string PROC_NAME, List<SqlParameter> parameters)
        {
            string query = PROC_NAME;

            return QueryDataSet(query, parameters);
        }

        public static object ExecuteScalar(string query, List<SqlParameter> parameters)
        {
            return Scalar(query, parameters);
        }

        public static object BulkUploadData(DataTable dt, string DestinationTable, bool IsKeepIdentiy = false)
        {
            return BulkUpload(dt, DestinationTable, IsKeepIdentiy);
        }


        #region Private Methods

        private static DataTable Query(String consult, IList<SqlParameter> parameters)
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(DefaultConnectionString);
            SqlCommand command = new SqlCommand();
            SqlDataAdapter da;
            try
            {
                command.Connection = connection;
                command.CommandText = consult;
                command.CommandType = CommandType.StoredProcedure;
                if (parameters != null && parameters.Count > 0)
                {

                    foreach (SqlParameter sqlParameter in parameters)
                    {
                        command.Parameters.Add(sqlParameter);
                    }
                }
                da = new SqlDataAdapter(command);
                da.Fill(dt);
            }

            catch (Exception ex)
            {
                //Logger.Error(ex);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
            return dt;

        }

        private static int NonQuery(string query, IList<SqlParameter> parameters)
        {

            DataSet dt = new DataSet();
            SqlConnection connection = new SqlConnection(DefaultConnectionString);
            SqlCommand command = new SqlCommand();

            try
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = query;
                command.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter sqlParameter in parameters)
                {
                    command.Parameters.Add(sqlParameter);
                }
                return command.ExecuteNonQuery();

            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }



        }

        private static object Scalar(string query, List<SqlParameter> parameters)
        {

            DataSet dt = new DataSet();
            SqlConnection connection = new SqlConnection(DefaultConnectionString);
            SqlCommand command = new SqlCommand();

            try
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = query;
                command.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter sqlParameter in parameters)
                {
                    command.Parameters.Add(sqlParameter);
                }
                return command.ExecuteScalar();

            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }


        }
        private static object BulkUpload(DataTable dt1, string DestinationTableName, bool isKeepIdentity)
        {

            SqlConnection connection = new SqlConnection(DefaultConnectionString);
            SqlCommand command = new SqlCommand();

            SqlBulkCopyOptions copyOptions = SqlBulkCopyOptions.KeepIdentity;
            try
            {
                using (var copy = new SqlBulkCopy(DefaultConnectionString, copyOptions))
                {
                    copy.DestinationTableName = DestinationTableName;
                    foreach (DataColumn column in dt1.Columns)
                    {
                        copy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }
                    copy.WriteToServer(dt1);
                }
                return null;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }


        }
        private static DataSet QueryDataSet(String consult, IList<SqlParameter> parameters)
        {

            DataSet dt = new DataSet();
            SqlConnection connection = new SqlConnection(DefaultConnectionString);
            SqlCommand command = new SqlCommand();
            SqlDataAdapter da;
            try
            {
                command.Connection = connection;
                command.CommandText = consult;
                command.CommandType = CommandType.StoredProcedure;
                if (parameters != null && parameters.Count > 0)
                {

                    foreach (SqlParameter sqlParameter in parameters)
                    {
                        command.Parameters.Add(sqlParameter);
                    }

                }

                da = new SqlDataAdapter(command);
                da.Fill(dt);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
            return dt;


        }

        public static DataSet QueryDS(String consult, IList<SqlParameter> parameters)
        {
            DataSet ds = new DataSet();
            SqlConnection connection = new SqlConnection(DefaultConnectionString);
            SqlCommand command = new SqlCommand();
            SqlDataAdapter da;
            try
            {
                command.Connection = connection;
                command.CommandText = consult;
                command.CommandType = CommandType.StoredProcedure;
                if (parameters != null && parameters.Count > 0)
                {

                    foreach (SqlParameter sqlParameter in parameters)
                    {
                        command.Parameters.Add(sqlParameter);
                    }

                }

                da = new SqlDataAdapter(command);
                da.Fill(ds);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
            return ds;


        }

        public static void AddSqlParameter(string PamaterName, object ParamValue, ref List<SqlParameter> sqlParameters )
        {
            SqlParameter param = new SqlParameter(PamaterName,ParamValue);
            sqlParameters.Add(param);
        }

        #endregion
    }
}
