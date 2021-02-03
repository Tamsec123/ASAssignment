using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace ASAssignment
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        static string hashFinal;
        static string salt;
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["userDB"].ConnectionString;
        string email;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["email"] != "")
            {
                email = Request.QueryString["email"];
            }
            if (Request.QueryString["error"] == "error")
            {
                lblMessage.Text = "Password not valid. Please try again.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            Session["Error"] = "Password is invalid, try again";
            string pword = HttpUtility.HtmlEncode(tbCurrPassword.Text.ToString().Trim());
            string newpword = HttpUtility.HtmlEncode(tbNewPassword.Text.ToString().Trim());
            SHA512Managed hashh = new SHA512Managed();
            string dbh = getDBH(email);
            string dbs = getDBS(email);

            try
            {
                if (dbs != null && dbs.Length > 0 && dbh != null && dbh.Length > 0)
                {
                    string pwordWithSalt = pword + dbs;
                    byte[] hashWithSalt = hashh.ComputeHash(Encoding.UTF8.GetBytes(pwordWithSalt));
                    string emailHash = Convert.ToBase64String(hashWithSalt);


                    //Generate random "salt"
                    RNGCryptoServiceProvider rngcsp = new RNGCryptoServiceProvider();
                    byte[] saltByte = new byte[8];

                    //Fills array of bytes with a cryptographically strong sequence of random values
                    rngcsp.GetBytes(saltByte);
                    salt = Convert.ToBase64String(saltByte);

                    SHA512Managed hashing = new SHA512Managed();

                    string newpwordWithSalt = newpword + salt;
                    byte[] hashPlain = hashing.ComputeHash(Encoding.UTF8.GetBytes(newpword));
                    byte[] hashingWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(newpwordWithSalt));

                    hashFinal = Convert.ToBase64String(hashingWithSalt);

                    if (emailHash.Equals(dbh))
                    {
                        SqlConnection connection = new SqlConnection(connectionString);
                        string sqlq = "UPDATE user_info SET password=@NewPassword, salt=@NewSalt WHERE Email=@Email";
                        SqlCommand command = new SqlCommand(sqlq, connection);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@NewPassword", hashFinal);
                        command.Parameters.AddWithValue("@NewSalt", salt);
                        try
                        {
                            //command.Connection = connection;
                            connection.Open();
                            command.ExecuteNonQuery();
                            connection.Close();
                            lblMessage.Text = "Password has been changed successfully";
                            lblMessage.ForeColor = System.Drawing.Color.Green;
                        }
                        catch (SqlException ex)
                        {

                            lblMessage.Text = "Something went wrong, please try again.";
                        }


                    }
                    else
                    {
                        Response.Redirect("ChangePassword.aspx?error=error", false);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
            finally { }

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

        protected void btn_BackToLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx", false);
        }
    }
}
