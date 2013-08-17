using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;

using Shared;

namespace DataAccessManager
{
    public class Manager
    {
        private String connectionString = ConfigurationManager.ConnectionStrings["ContactData"].ToString();

        #region Sql Methods

        /// <summary>
        /// Returns a new sql parameter with the given paramenters.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <param name="dbtype"></param>
        /// <returns></returns>
        private SqlParameter GetParameter(String parameterName, Object value, DbType dbtype)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = parameterName;
            param.Value = value;
            param.DbType = dbtype;
            return param;
        }
        
        /// <summary>
        /// Returns the connection string.
        /// </summary>
        /// <returns></returns>
        private String GetConnectionString()
        {
            return connectionString;
        }

        /// <summary>
        /// Returns a new sql connection.
        /// </summary>
        /// <returns></returns>
        private SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(GetConnectionString());
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Closes the provided sql connection.
        /// </summary>
        /// <param name="connection"></param>
        private void CloseConnection(SqlConnection connection)
        {
            connection.Close();
        }

        #endregion Sql Methods

        #region CRUD

        /// <summary>
        /// Read contacts
        /// </summary>
        /// <param name="operationType"></param>
        /// <returns></returns>
        public DataTable Read(String operationType)
        {
            DataTable dataTable = new DataTable();          

            SqlDataReader dataReader;
            String readDataQuery = OperationType.Map[operationType];
            SqlConnection connection = GetConnection();
            SqlCommand command = new SqlCommand(readDataQuery, connection);

            dataTable.Columns.Add("id");
            dataTable.Columns.Add("name");
            dataTable.Columns.Add("phone");
            dataTable.Columns.Add("address");

            dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                dataTable.Rows.Add(dataReader["id"].ToString(),
                                   dataReader["name"].ToString(),
                                   dataReader["phone"].ToString(),
                                   dataReader["address"].ToString());
            }

            CloseConnection(connection);
            return dataTable;
        }

        /// <summary>
        /// Inserts a new contact
        /// </summary>
        /// <param name="operationType"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Int32 InsertContact(String operationType, Dictionary<String, String> data)
        {
            Int32 retVal;
            String insertDataQuery = OperationType.Map[operationType];
            SqlConnection connection = GetConnection();
            SqlCommand command = new SqlCommand(insertDataQuery, connection);

            String name = data["name"];
            String phone = data["phone"];
            String address = data["address"];

            command.Parameters.Add(GetParameter("@name", name, DbType.String));
            command.Parameters.Add(GetParameter("@phone", phone, DbType.String));
            command.Parameters.Add(GetParameter("@address", address, DbType.String));
            
            retVal = command.ExecuteNonQuery();
            CloseConnection(connection);
            return retVal;
        }

        /// <summary>
        /// Update contact
        /// </summary>
        /// <param name="operationType"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Int32 UpdateContact(String operationType, Dictionary<String, String> data)
        {
            Int32 retVal;
            String updateDataQuery = OperationType.Map[operationType];
            SqlConnection connection = GetConnection();
            SqlCommand command = new SqlCommand(updateDataQuery, connection);

            String id = data["id"];
            String name = data["name"];
            String phone = data["phone"];
            String address = data["address"];

            command.Parameters.Add(GetParameter("@id", id, DbType.Int32));
            command.Parameters.Add(GetParameter("@name", name, DbType.String));
            command.Parameters.Add(GetParameter("@phone", phone, DbType.String));
            command.Parameters.Add(GetParameter("@address", address, DbType.String));

            retVal = command.ExecuteNonQuery();
            CloseConnection(connection);
            return retVal;
        }

        /// <summary>
        /// Delete Contact
        /// </summary>
        /// <param name="operationType"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Object Delete(String operationType, Dictionary<String, String> data)
        {
            Int32 retVal;
            String deleteDataQuery = OperationType.Map[operationType];
            SqlConnection connection = GetConnection();
            SqlCommand command = new SqlCommand(deleteDataQuery, connection);

            String id = data["id"];

            command.Parameters.Add(GetParameter("@id", id, DbType.Int32));

            retVal = command.ExecuteNonQuery();
            CloseConnection(connection);
            return retVal;
        }

        #endregion CRUD
    }
}
