using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Controller;
namespace JavascriptORM
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Initialize DataController

            DataController dataController;
            if (Session.IsNewSession)
            {
                dataController = new DataController();
                Session.Add("DataController", dataController);
            }

            #endregion Initialize DataController
        }
    }
}