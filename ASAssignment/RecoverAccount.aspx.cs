using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASAssignment
{
    public partial class RecoverAccount : System.Web.UI.Page
    {
        string email;
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["userDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["email"] != "")
            {
                email = Request.QueryString["email"];
            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            if (tbVerification.Text.ToLower() == Session["CaptchaVerify"].ToString())
            {
                AccountUnlock(email);
                btn_goToLogin.Visible = true;
            }
            else
            {
                lblCaptchaMessage.Text = "You have entered the wrong Captcha. Please enter correct Captcha!";
                lblCaptchaMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        private string AccountUnlock(string email)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                string sqlq = "UPDATE user_info SET accountStat=1 WHERE Email=@Email";
                SqlCommand command = new SqlCommand(sqlq, conn);
                command.Parameters.AddWithValue("@Email", email);
                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                    lblError.Text = "Account has been successfully unlocked. Please log in in the login page.";
                    lblError.ForeColor = System.Drawing.Color.Green;
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.ToString());
                }
                finally { conn.Close(); }
            }
            catch (SqlException ex)
            {

                lblError.Text = "Something went wrong, please try again.";
                lblError.ForeColor = System.Drawing.Color.Red;
            }
            return null;

        }

        protected void btn_goToLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx", false);
        }
    }
}