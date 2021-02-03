using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASAssignment
{
    public partial class Login : System.Web.UI.Page
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["userDB"].ConnectionString;
        public class MyObject
        {
            public string success { get; set; }
            public List<String> ErrorMessage { get; set; }
        }

        public bool CaptchaValidation()
        {
            bool result = true;

            string captchaResponse = Request.Form["g-recaptcha-response"];

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6LfTIkgaAAAAACo87RcpBEP0Z4cAxQbJIseowih0" +"&response=" + captchaResponse);

            try
            {
                using (WebResponse Responses = req.GetResponse())
                {
                    using (StreamReader streamRead = new StreamReader(Responses.GetResponseStream()))
                    {
                        string responseJson = streamRead.ReadToEnd();

                        JavaScriptSerializer jss = new JavaScriptSerializer();

                        MyObject jsonObject = jss.Deserialize<MyObject>(responseJson);

                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["error"] == "error")
            {
                lblError.Text = "Email or Password not valid. Please try again.";
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }


        protected void btn_login_Click(object sender, EventArgs e)
        {
            if (CaptchaValidation())
            {
                string pword = HttpUtility.HtmlEncode(tb_password.Text.ToString().Trim());
                string email = HttpUtility.HtmlEncode(tb_email.Text.ToString().Trim());
                SHA512Managed hashh = new SHA512Managed();
                string dbh = getDBH(email);
                string dbs = getDBS(email);

                Session["Error"] = "Email or password is invalid, try again";
                try
                {
                    if (dbs != null && dbs.Length > 0 && dbh != null && dbh.Length > 0)
                    {
                        string pwordWithSalt = pword + dbs;
                        byte[] hashWithSalt = hashh.ComputeHash(Encoding.UTF8.GetBytes(pwordWithSalt));
                        string emailHash = Convert.ToBase64String(hashWithSalt);

                        if (emailHash.Equals(dbh))
                        {
                            Session["IsLoggedIn"] = tb_email.Text.Trim();

                            string guid = Guid.NewGuid().ToString();
                            Session["AuthenticationToken"] = guid;

                            Response.Cookies.Add(new HttpCookie("AuthenticationToken", guid));

                            Response.Redirect("AfterLogin.aspx?email="+ HttpUtility.HtmlEncode(email), false);


                        }
                        else
                        {
                            Response.Redirect("Login.aspx?error=error", false);
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.ToString());
                }
                finally { }

            }
        }
        protected string getDBH(string email)
        {
            string hash = null;
            SqlConnection conn = new SqlConnection(connectionString);
            string sqlq = "select password FROM user_info WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sqlq, conn);
            command.Parameters.AddWithValue("@Email", email);
            try
            {
                conn.Open();
                using (SqlDataReader read = command.ExecuteReader())
                {
                    while (read.Read())
                    {
                        if (read["password"] != null)
                        {
                            if (read["password"] != DBNull.Value)
                            {
                                hash = read["password"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
            finally { conn.Close(); }
            return hash;
        }

        protected string getDBS(string email)
        {
            string salt = null;
            SqlConnection connection = new SqlConnection(connectionString);
            string sqlq = "select salt FROM user_info WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sqlq, connection);
            command.Parameters.AddWithValue("@Email", email);
            try
            {
                connection.Open();
                using (SqlDataReader read = command.ExecuteReader())
                {
                    while (read.Read())
                    {
                        if (read["salt"] != null)
                        {
                            if (read["salt"] != DBNull.Value)
                            {
                                salt = read["salt"].ToString();

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return salt;
        }
    }
}