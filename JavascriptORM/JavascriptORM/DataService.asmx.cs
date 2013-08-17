using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Web.SessionState;

using Shared;
using Controller;

namespace JavascriptORM
{
    /// <summary>
    /// Summary description for DataService
    /// </summary>
    [WebService(Namespace = "http://localhost/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class DataService : System.Web.Services.WebService
    {
        /// <summary>
        /// Execute operation based on type of operation
        /// </summary>
        /// <param name="operationType"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public Object ExecuteModelOperation(String operationType, String values)
        {
            List<Object> retVal = new List<Object>();
            DataController dataController = Session["DataController"] as DataController;

            retVal = dataController.ExecuteModelOperation(operationType, values);
            
            return retVal;
        }
    }
}
