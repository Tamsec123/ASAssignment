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

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=" +"&response=" + captchaResponse);

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
                if (statCheck(email) == "1")
                {
                    if (timeDiff(email) < 15)
                    {



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

                                    Response.Redirect("AfterLogin.aspx?email=" + HttpUtility.HtmlEncode(email), false);


                                }
                                else
                                {
                                    Session["AttemptCount"] = Convert.ToInt32(Session["AttemptCount"]) + 1;
                                    if (Convert.ToInt32(Session["AttemptCount"]) >= 3)
                                    {
                                        AccountLockout(email);
                                    }
                                    else
                                    {
                                        Response.Redirect("Login.aspx?error=error", false);
                                    }

                                }
                            }
                        }
                        catch (Exception ex)
                        {

                            throw new Exception(ex.ToString());
                        }
                        finally { }

                    }
                    else
                    {
                        lblError.Text = "Your password has expired, please change your password now.";
                        lblError.ForeColor = System.Drawing.Color.Red;
                        btn_login.Visible = false;
                        btn_changePassword.Visible = true;
                    }
                }
                else
                {
                    lblError.Text = "Your account has been locked out";
                    lblError.ForeColor = System.Drawing.Color.Red;
                    btn_Recover.Visible = true;
                }
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

        private string AccountLockout(string email)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string sqlq = "UPDATE user_info SET accountStat=0 WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sqlq, conn);
            command.Parameters.AddWithValue("@Email", email);
            try
            {
                conn.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
            finally { conn.Close(); }
            return null;
        }

        private string statCheck(string email)
        {
            string result = "1";
            SqlConnection conn = new SqlConnection(connectionString);
            string sqlq = "SELECT accountStat FROM user_info WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sqlq, conn);
            command.Parameters.AddWithValue("@Email", email);
            try
            {
                conn.Open();
                using (SqlDataReader read = command.ExecuteReader())
                {
                    while (read.Read())
                    {
                        if (read["accountStat"] != null)
                        {
                            if (read["accountStat"] != DBNull.Value)
                            {
                                result = read["accountStat"].ToString();
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
            return result;
        }

        protected void btn_Recover_Click(object sender, EventArgs e)
        {
            string email = HttpUtility.HtmlEncode(tb_email.Text.ToString().Trim());
            Response.Redirect("RecoverAccount.aspx?email=" + email, false);
        }
        protected int timeDiff(string email)
        {
            DateTime resetTime = new DateTime();
            TimeSpan diff = new TimeSpan();
            SqlConnection conn = new SqlConnection(connectionString);
            string sqlq = "select pwResetTime FROM user_info WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sqlq, conn);
            command.Parameters.AddWithValue("@Email", email);
            try
            {
                conn.Open();
                using (SqlDataReader read = command.ExecuteReader())
                {
                    while (read.Read())
                    {
                        if (read["pwResetTime"] != null)
                        {
                            if (read["pwResetTime"] != DBNull.Value)
                            {
                                resetTime = Convert.ToDateTime(read["pwResetTime"].ToString());
                                diff = DateTime.Now.Subtract(resetTime);
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
            return diff.Minutes;

        }

        protected void btn_changePassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangePassword.aspx?email=" + HttpUtility.HtmlEncode(tb_email.Text), false);
        }
    }
}
