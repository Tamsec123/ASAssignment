using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASAssignment
{
    public partial class AfterLogin : System.Web.UI.Page
    {
        string email;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["email"] != "")
            {
                email = Request.QueryString["email"];
            }

            if (Session["IsLoggedIn"] != null && Session["AuthenticationToken"] != null && Request.Cookies["AuthenticationToken"] != null)
            {
                if (!Session["AuthenticationToken"].ToString().Equals(Request.Cookies["AuthenticationToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
                else
                {
                    lbl_message.Text = "You have successfully Logged in.";
                    lbl_message.ForeColor = System.Drawing.Color.Green;
                    btn_Logout.Visible = true;
                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }

        protected void LogMeOut(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            Response.Redirect("Login.aspx", false);

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }

            if (Request.Cookies["AuthenticationToken"] != null)
            {
                Response.Cookies["AuthenticationToken"].Value = string.Empty;
                Response.Cookies["AuthenticationToken"].Expires = DateTime.Now.AddMonths(-20);
            }
        }



        protected void btn_ChangePassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangePassword.aspx?email=" + HttpUtility.HtmlEncode(email), false);
        }
    }
}