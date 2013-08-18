using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Data;
using System.Data.SqlClient;

using Shared;
using DataAccessManager;

namespace Controller
{
    public class DataController
    {
        private Manager manager = new Manager();

        /// <summary>
        /// Execute operation based on type of operation
        /// </summary>
        /// <param name="operationType"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public List<Object> ExecuteModelOperation(String operationType, String values)
        {
            List<Object> retVal = new List<Object>();            
            Dictionary<String, String> columnMap = new Dictionary<String, String>();

            //lets first deserialize the json string into a dicitonary containing pairs of column-value        
            ConvertJsonToObject(values, columnMap);

            //execute the operation
            return ExecuteOperation(operationType, columnMap);
        }

        /// <summary>
        /// Execute Operation
        /// </summary>
        /// <param name="operationType"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private List<Object> ExecuteOperation(String operationType, Dictionary<String, String> data)
        {
            Object retVal = new Object();
            List<Object> output = new List<Object>();

            //Implementing a global try-catch block, never use this for production version 
            try
            {
                switch (operationType)
                {
                    case OperationType.Read:
                        retVal = manager.Read(operationType);
                        ConvertDataTable((DataTable)retVal, output, operationType);
                        break;
                    case OperationType.Insert:
                        retVal = manager.InsertContact(operationType, data);
                        break;
                    case OperationType.Update:
                        retVal = manager.UpdateContact(operationType, data);
                        break;
                    case OperationType.Delete:
                        retVal = manager.Delete(operationType, data);
                        break;
                    default:
                        break;
                }
            }
            catch(Exception exception)
            {
                throw new Exception(String.Concat(Exceptions.Error, ": ", exception.Message));
            }
            return output;
        }

        /// <summary>
        /// Convert the dataTable to appropriate object so as to return that to the UI
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="output"></param>
        /// <param name="operationType"></param>
        private void ConvertDataTable(DataTable dataTable, List<Object> output, String operationType)
        {
            switch (operationType)
            {
                case OperationType.Read:
                    foreach (DataRow row in dataTable.Rows)
                    {
                        output.Add(new
                        {
                            id = row["id"].ToString(),
                            name = HtmlEscape(row["name"].ToString()),
                            phone = HtmlEscape(row["phone"].ToString()),
                            address = HtmlEscape(row["address"].ToString())
                        });                        
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Convert the json string passed to the columnMap key value pair
        /// </summary>
        /// <param name="jsonString">JSON String</param>
        /// <param name="columnMap"></param>
        private void ConvertJsonToObject(String jsonString, Dictionary<String, String> columnMap)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            List<ColumnValues> vals = new List<ColumnValues>();

            //lets first deserialize the json string into object of type provided     
            vals = jss.Deserialize<List<ColumnValues>>((String)jsonString);

            foreach (var item in vals)
            {
                if(!columnMap.ContainsKey(item.ColName))
                    columnMap.Add(item.ColName, HtmlUnEscape(item.ColValue));
            }                 
        }

        /// <summary>
        /// Encodes the string in html form
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private String HtmlEscape(String str)
        {
            return System.Net.WebUtility.HtmlEncode(str);
        }

        /// <summary>
        /// Decodes the string from html form
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private String HtmlUnEscape(String str)
        {
            return System.Net.WebUtility.HtmlDecode(str);
        }
    }
}
