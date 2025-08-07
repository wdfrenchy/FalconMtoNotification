using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Text;

namespace FalconMtoNotification
{
    public partial class _Default : System.Web.UI.Page
    {
        public static string MakeConnectionString(string server, string user, string pw, string db)
        {
            return "server='" + server + "'; user id='" + user + "'; password='" + pw + "'; "
            + (null != db ? "database='" + db + "' " : "");
        }
        string connectionString = MakeConnectionString("nexus1", "Nexus", "nexusnexus", "NovoNexusTrain");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtEta.Attributes["type"] = "date";
                txtDepartureDate.Attributes["type"] = "date";

#if DEBUG
                txtRefNumber.Text = "M83063-1";
                txtEta.Text = "7/7/2025";
                txtDepartureDate.Text = "6/26/2025";
                txtDepartureAddress.Text = "30 EAGLEVILLE RD";
                txtCarrier.Text = "FEDEX";
                txtShipmentType.Text = "INTL DEFERRED AIR";
                txtContactDetails.Text = "FALCON LOGISTICS";
                txtTrackingNumber.Text = "882301055529";
#endif

            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var errors = RunButtonHandler();

            if (errors.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                lblMessage.ForeColor = System.Drawing.Color.Red;
                foreach (string error in errors)
                {
                    sb.AppendFormat("{0}<br />", error);
                }
                lblMessage.Text = sb.ToString();
            }
        }
        List<string> RunButtonHandler()
        {
            List<string> errorList = new List<string>(); 
            if (string.IsNullOrEmpty(txtRefNumber.Text) ||
                string.IsNullOrEmpty(txtEta.Text) ||
                string.IsNullOrEmpty(txtDepartureDate.Text) ||
                string.IsNullOrEmpty(txtDepartureAddress.Text) ||
                string.IsNullOrEmpty(txtCarrier.Text) ||
                string.IsNullOrEmpty(txtShipmentType.Text))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "All fields except \"Contact Details\" and \"Tracking Number\" are required.";
                errorList.Add(lblMessage.Text);
            }

            if (string.IsNullOrEmpty(txtRefNumber.Text))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "All fields except \"Reference Number\" are required.";
                errorList.Add(lblMessage.Text);
            }


            SqlLibrary.Instance.sql SQL = null;
            try
            {
                SQL = new SqlLibrary.Instance.sql(connectionString);
                GenericLibraries.ParamList pl = new GenericLibraries.ParamList();

            string referenceNumber = txtRefNumber.Text.Trim();
            string eta = txtEta.Text;
            string departureDate = txtDepartureDate.Text;
            string departureAddress = txtDepartureAddress.Text.Trim();
            string carrier = txtCarrier.Text.Trim();
            string shipmentType = txtShipmentType.Text.Trim();
            string contactDetails = txtContactDetails.Text.Trim();
            string trackingNumber = txtTrackingNumber.Text.Trim();


            string[] parts = referenceNumber.Split('-');

            string mtoNumber=null; // reassign to null
            string shippingNumber= null; // reassign to null
            

            if (parts.Length != 2)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = string.Format("The entered MTO number '{0}' is not in the proper format. Unable to upload. Time: {1}", referenceNumber, DateTime.Now.ToString("g"));
                errorList.Add(lblMessage.Text); // added for error list, removed return
            }
            else
            {
                mtoNumber = parts[0];
                shippingNumber = parts[1];
            }

            DateTime etaDateTime;

            if (DateTime.TryParse(eta, out etaDateTime) == false) {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = string.Format("The entered ETA '{0}' is not in the proper format. Unable to upload. Time: {1}", eta, DateTime.Now.ToString("g"));
                errorList.Add(lblMessage.Text); // added for error list, removed return
            }

            DateTime departureDateDateTime;

            if (DateTime.TryParse(departureDate, out departureDateDateTime) == false)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = string.Format("The entered departure date '{0}' is not in the proper format. Unable to upload. Time: {1}", departureDate, DateTime.Now.ToString("g"));
                errorList.Add(lblMessage.Text); // added for error list, removed return
            }
            if (errorList.Count > 0) return errorList;
            string checkQuery = "SELECT UploadDate FROM FalconMtohUploadLog WHERE ReferenceNumber = @Ref";
            pl.Add("ref", referenceNumber); // Changed
            DataTable dt = SQL.GetResultTable(checkQuery, pl);

            bool existsWithUploadDate = false;
            if (dt.Rows.Count > 0 && dt.Rows[0]["UploadDate"] != DBNull.Value)
            {
                existsWithUploadDate = true;
            }


            bool recordDoesNotExist = dt.Rows.Count == 0;

            if (existsWithUploadDate) {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = string.Format("MTO '{0}' has already been transmitted to Falcon. Unable to upload. Time: {1}", referenceNumber, DateTime.Now.ToString("g"));
                errorList.Add(lblMessage.Text); // added for error list, removed return
            }

            bool hasInTransitLines = false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT COUNT(*) FROM MTOD_SHIP_INFO
                                WHERE Mto_Nbr = @MTO AND Ship_Nbrs = @Ship AND Qtys_in_Transit > 0";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MTO", mtoNumber); // Changed
                cmd.Parameters.AddWithValue("@Ship", shippingNumber); // Changed

                conn.Open();
                int count = (int) cmd.ExecuteScalar();
                conn.Close();

                hasInTransitLines = count > 0;
            }

            if (!hasInTransitLines)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = string.Format("MTO '{0}' has no in-transit lines. Unable to upload.", referenceNumber);
                errorList.Add(lblMessage.Text); // added for error list, removed return
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                if (!existsWithUploadDate) // Changed
                {
                    SqlCommand deleteCmd1 = new SqlCommand("DELETE FROM FalconMtodUploadLog WHERE ReferenceNumber = @Ref", conn);
                    deleteCmd1.Parameters.AddWithValue("@Ref", referenceNumber); // Changed
                    deleteCmd1.ExecuteNonQuery();
                    SqlCommand deleteCmd2 = new SqlCommand("DELETE FROM FalconMtohUploadLog WHERE ReferenceNumber = @Ref", conn);
                    deleteCmd2.Parameters.AddWithValue("@Ref", referenceNumber); // Changed
                    deleteCmd2.ExecuteNonQuery();
                }

                SqlCommand insertCmd = new SqlCommand(@"INSERT INTO FalconMtohUploadLog(ReferenceNumber, ETA, DepartureDate, DepartureAddress, Carrier, ShipmentType, ContactDetails, TrackingNumber, UploadDate)
                                                        VALUES (@ReferenceNumber, @ETA, @DepartureDate, @DepartureAddress, @Carrier, @ShipmentType, @ContactDetails, @TrackingNumber, NULL)
                                                        INSERT INTO FalconMtodUploadLog(ReferenceNumber, SKU, QTY)
                                                        SELECT s.Mto_Nbr + '-' + s.Ship_Nbrs, m.Part_wo_gl, s.Qtys_in_Transit
                                                        FROM MTOD_SHIP_INFO s
                                                        INNER JOIN MTOD m ON s.Mto_Nbr = m.Mto_Nbr
                                                        WHERE s.Mto_Nbr + '-' + s.Ship_Nbrs = @ReferenceNumber AND s.Qtys_in_Transit > 0", conn);

                insertCmd.Parameters.AddWithValue("@ReferenceNumber", referenceNumber);
                insertCmd.Parameters.AddWithValue("@ETA", etaDateTime);
                insertCmd.Parameters.AddWithValue("@DepartureDate", departureDateDateTime);
                insertCmd.Parameters.AddWithValue("@DepartureAddress", departureAddress);
                insertCmd.Parameters.AddWithValue("@Carrier", carrier);
                insertCmd.Parameters.AddWithValue("@ShipmentType", shipmentType);
                insertCmd.Parameters.AddWithValue("@ContactDetails", contactDetails);
                insertCmd.Parameters.AddWithValue("@TrackingNumber", trackingNumber);

                insertCmd.ExecuteNonQuery();

                conn.Close();
            }

            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = string.Format("Shipment '{0}' uploaded successfully at {1}.", referenceNumber, DateTime.Now.ToString("g"));
            errorList.Add(lblMessage.Text); // added for error list
            }
            finally
            {
                SqlLibrary.Instance.sql.Close_Quietly(SQL);
            }
            return new List<string>();
        }
    }
}
