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
            //if (Request.QueryString["error"] == "error")
            //{
            //    lblMessage.Text = "Password not valid. Please try again.";
            //    lblMessage.ForeColor = System.Drawing.Color.Red;
            //}
            
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            


            string pword = HttpUtility.HtmlEncode(tbCurrPassword.Text.ToString().Trim());
            string newpword = HttpUtility.HtmlEncode(tbNewPassword.Text.ToString().Trim());
            SHA512Managed hashh = new SHA512Managed();
            string dbh = getDBH(email);
            string dbs = getDBS(email);

            try
            {
                if (dbs != null && dbs.Length > 0 && dbh != null && dbh.Length > 0)
                {


                        DateTime resetTime = new DateTime();
                        int existOrNot = 1;
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

                        SqlConnection connect = new SqlConnection(connectionString);
                        string sql = "select pwResetTime FROM user_info WHERE Email=@Email";
                        SqlCommand comm = new SqlCommand(sql, connect);
                        comm.Parameters.AddWithValue("@Email", email);
                        try
                        {
                            connect.Open();
                            using (SqlDataReader read = comm.ExecuteReader())
                            {
                                while (read.Read())
                                {
                                    if (read["pwResetTime"] != null)
                                    {
                                        if (read["pwResetTime"] != DBNull.Value)
                                        {
                                            resetTime = Convert.ToDateTime(read["pwResetTime"].ToString());
                                        }
                                        else
                                        {
                                            existOrNot = 0;
                                        }
                                    }
                                    else
                                    {
                                        existOrNot = 0;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                            throw new Exception(ex.ToString());
                        }
                        finally { connect.Close(); }

                        if (timeDiff(email) >= 5 || existOrNot == 0)
                        {


                            if (emailHash.Equals(dbh))
                            {
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                                if (tbCurrPassword.Text == "")
                                {
                                    lblMessage.Text += "Please Fill in Current Password <br/>";
                                }
                                if (tbNewPassword.Text == "")
                                {
                                    lblMessage.Text += "Please Fill in New Password <br/>";
                                }
                                if (tbCfmNewPassword.Text == "")
                                {
                                    lblMessage.Text += "Please Fill in Confirm New Password <br/>";
                                }
                                if (tbNewPassword.Text == tbCurrPassword.Text)
                                {
                                    lblMessage.Text += "Old password cannot be same as new password <br/>";
                                }
                                if (tbNewPassword.Text != tbCfmNewPassword.Text)
                                {
                                    lblMessage.Text += "New password not the same as Confirm password <br/>";
                                }
                                else
                                {
                                    SqlConnection connection = new SqlConnection(connectionString);
                                    string sqlq = "UPDATE user_info SET password=@NewPassword, salt=@NewSalt,pwResetTime=@pwResetTime WHERE Email=@Email";
                                    SqlCommand command = new SqlCommand(sqlq, connection);
                                    command.Parameters.AddWithValue("@Email", email);
                                    command.Parameters.AddWithValue("@NewPassword", hashFinal);
                                    command.Parameters.AddWithValue("@NewSalt", salt);
                                    command.Parameters.AddWithValue("@pwResetTime", DateTime.Now);
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
                            }
                            else
                            {
                                lblMessage.Text = "Password not valid. Please try again.";
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                            }
                        }
                        else
                        {
                            lblMessage.Text = "You have changed password recently, please try again in " + (5 - timeDiff(email)) + " minutes";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
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
    }
}
