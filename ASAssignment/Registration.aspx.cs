﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;

namespace ASAssignment
{
    public partial class Registration : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["userDB"].ConnectionString;
        static string hashFinal;
        static string salt;
        DateTime dt = new DateTime();
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void btn_submit_Click(object sender, EventArgs e)
        {
            
            if (tbFirstName.Text == "")
            {
                lbl_msg.Text += "First name cannot be empty <br/>";
            }
            if (tbLastName.Text == "")
            {
                lbl_msg.Text += "Last name cannot be empty <br/>";
            }
            if (tbCardNumber.Text == "")
            {
                lbl_msg.Text += "Card number cannot be empty <br/>";
            }
            if (tbExpDate.Text == "")
            {
                lbl_msg.Text += "Expiry date cannot be empty <br/>";
            }
            if (tbCVV.Text == "")
            {
                lbl_msg.Text += "CVV cannot be empty <br/>";
            }
            if (tbEmail.Text == "")
            {
                lbl_msg.Text += "Email cannot be empty <br/>";
            }
            if (tbPassword.Text == "")
            {
                lbl_msg.Text += "Password cannot be empty <br/>";
            }
            if (tbPasswordCfm.Text == "")
            {
                lbl_msg.Text += "Password Confirmation cannot be empty <br/>";
            }
            if (tbDoB.Text == "")
            {
                lbl_msg.Text += "Date of birth cannot be empty <br/>";
            }
            else
            {
                dt = Convert.ToDateTime(DateTime.ParseExact(tbDoB.Text.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture));
                string pword = tbPassword.Text.ToString().Trim();

                //Generate random "salt"
                RNGCryptoServiceProvider rngcsp = new RNGCryptoServiceProvider();
                byte[] saltByte = new byte[8];

                //Fills array of bytes with a cryptographically strong sequence of random values
                rngcsp.GetBytes(saltByte);
                salt = Convert.ToBase64String(saltByte);

                SHA512Managed hashing = new SHA512Managed();

                string pwordWithSalt = pword + salt;
                byte[] hashPlain = hashing.ComputeHash(Encoding.UTF8.GetBytes(pword));
                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwordWithSalt));

                hashFinal = Convert.ToBase64String(hashWithSalt);

                RijndaelManaged cipher = new RijndaelManaged();
                cipher.GenerateKey();


                createUser();
                Response.Redirect("Login.aspx", false);
            }
        }

        private int passwordCheck(string pass)
        {
            int score = 0;

            if (pass.Length < 8)
            {
                return 1;
            }
            else
            {
                score = 1;
            }

            if (Regex.IsMatch(pass, "[a-z]"))
            {
                score++;
            }

            if (Regex.IsMatch(pass, "[A-Z]"))
            {
                score++;
            }

            if (Regex.IsMatch(pass, "[0-9]"))
            {
                score++;
            }

            if (Regex.IsMatch(pass, "[^A-Za-z0-9]"))
            {
                score++;
            }

            return score;
        }
        

        private void createUser()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                     using (SqlCommand command = new SqlCommand("INSERT INTO user_info VALUES(@Email,@firstName,@lastName,@cardNumber,@expiryDate,@CVV,@password,@salt,@dateOfBirth)"))
                    {
                        using (SqlDataAdapter sqlData = new SqlDataAdapter())
                        {
                            command.CommandType = System.Data.CommandType.Text;
                            command.Parameters.AddWithValue("@Email", tbEmail.Text.Trim());
                            command.Parameters.AddWithValue("@firstName", tbFirstName.Text.Trim());
                            command.Parameters.AddWithValue("@lastName", tbLastName.Text.Trim());
                            command.Parameters.AddWithValue("@cardNumber", Convert.ToBase64String(dataEncrypt(tbCardNumber.Text.Trim())));
                            command.Parameters.AddWithValue("@expiryDate", Convert.ToBase64String(dataEncrypt(tbExpDate.Text.Trim())));
                            command.Parameters.AddWithValue("@CVV", Convert.ToBase64String(dataEncrypt(tbCVV.Text.Trim())));
                            command.Parameters.AddWithValue("@password", hashFinal);
                            command.Parameters.AddWithValue("@salt", salt);
                            command.Parameters.AddWithValue("@dateOfBirth", dt);
                            command.Connection = conn;
                            conn.Open();
                            command.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        protected byte[] dataEncrypt(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged ciph = new RijndaelManaged();
                ICryptoTransform encryptTransform = ciph.CreateEncryptor();
                //ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0,
               plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }

        protected void btn_Check_Click(object sender, EventArgs e)
        {
            int complexScore = passwordCheck(tbPassword.Text);
            string stat = "";
            switch (complexScore)
            {
                case 1:
                    stat = "Very Bad!";
                    break;
                case 2:
                    stat = "Not Good Enough!";
                    break;
                case 3:
                    stat = "Not Bad";
                    break;
                case 4:
                    stat = "Quite Good";
                    break;
                case 5:
                    stat = "Awesome!";
                    break;
                default:
                    break;
            }
            lbl_pwdchecker.Text = "Status : " + stat;
            if (complexScore < 4)
            {
                lbl_pwdchecker.ForeColor = Color.Red;
                return;
            }
            lbl_pwdchecker.ForeColor = Color.Green;
        }
    }
}