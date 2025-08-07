//using System;
//using System.Collections;
//using System.Configuration;
//using System.Data;
//using System.Linq;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;

//namespace WebApplication1
//{
//    public partial class FalconMtoUploadReport : System.Web.UI.Page
//    {
//        public static string MakeConnectionString(string server, string user, string pw, string db)
//        {
//            return "server='" + server + "'; user id='" + user + "'; password='" + pw + "'; "
//            + (null != db ? "database='" + db + "' " : "");
//        }
//        string connectionString = MakeConnectionString("nexus1", "Nexus", "nexusnexus", "NovoNexusTrain");

//        protected void Page_Load(object sender, EventArgs e)
//        {
//            if (!IsPostBack)
//            {
//                txtStartDate.Attributes["type"] = "date";
//                txtEndDate.Attributes["type"] = "date";
                
//#if DEBUG
//#endif
//            }
//        }
//        protected void btnSubmit_Click(object sender, EventArgs e)
//        {
//            if (string.IsNullOrEmpty(txtStartDate.Text) ||
//                string.IsNullOrEmpty(txtEndDate.Text) ||
//                string.IsNullOrEmpty(txtCustNumber.Text) ||
//                string.IsNullOrEmpty(txtMtoNumber.Text))
//            {
//                lblMessage.ForeColor = System.Drawing.Color.Red;
//                lblMessage.Text = "All fields are required.";
//                return;
//            }
//        }

//        SqlLibrary.Instance.sql SQL = null;
//            try
//            {
//                SQL = new SqlLibrary.Instance.sql(connectionString);
//                GenericLibraries.ParamList pl = new GenericLibraries.ParamList();

//                string startDate = txtStartDate.Text;
//                string endDate = txtEndDate.Text;
//                string custNumber = txtCustNumber.Text.Trim();
//                string mtoNumber = txtMtoNumber.Text.Trim();

//                // add error list

//                // mtoNumber or referenceNumber?

//                DateTime startDateDateTime;

//                if (DateTime.TryParse(startDate, out startDateDateTime) == false) {
//                    lblMessage.ForeColor = System.Drawing.Color.Red;
//                    lblMessage.Text = string.Format("The entered start date '{0}' is not in the proper format. Unable to upload. Time: {1}", startDate, DateTime.Now.ToString("g"));
//                    return;
//                }

//                DateTime endDateDateTime;

//                if (DateTime.TryParse(endDate, out endDateDateTime) == false)
//                {
//                    lblMessage.ForeColor = System.Drawing.Color.Red;
//                    lblMessage.Text = string.Format("The entered end date '{0}' is not in the proper format. Unable to upload. Time: {1}", endDate, DateTime.Now.ToString("g"));
//                    return;
//                }
                
//                string checkQuery = "SELECT UploadDate FROM FalconMtohUploadLog WHERE ReferenceNumber = @Ref";
//                pl.Add("ref", referenceNumber); // Changed
//                DataTable dt = SQL.GetResultTable(checkQuery, pl);
//    }
//}
