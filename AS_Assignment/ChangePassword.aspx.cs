using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AS_Assignment
{
    public partial class Password : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYACC"].ConnectionString;
        static string finalHash;
        static string salt;
        static string newfinalHash;
        static string newsalt;
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Session["LoggedIn"].ToString();
            lb_name.Text = "Change Password for " + username;
            
            
           



        }

        protected void Submit(object sender, EventArgs e)
        {
            string username = Session["LoggedIn"].ToString();
            string pwd = tb_oldpass.Text.ToString().Trim();
            string dbHash = getDBHash(username);
            string dbSalt = getDBSalt(username);
            SHA512Managed hashing = new SHA512Managed();
            DateTime mintine = getMinTime(username);
            
            try
            {
                if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                {
                    string pwdWithSalt = pwd + dbSalt;
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                    string userHash = Convert.ToBase64String(hashWithSalt);
                    if (userHash.Equals(dbHash))
                    {
                        if (DateTime.Compare(DateTime.Now, mintine) > 0)
                        {

                            if (tb_newpass.Text == tb_confirmpass.Text)
                            {
                                int scores = checkPassword(tb_newpass.Text);
                                lbl_state.Text = "";
                                string status = "";
                                switch (scores)
                                {
                                    case 1:
                                        status = "Very Weak";
                                        break;
                                    case 2:
                                        status = "Weak";
                                        break;
                                    case 3:
                                        status = "Medium";
                                        break;
                                    case 4:
                                        status = "Strong";
                                        break;
                                    case 5:
                                        status = "Excellent";
                                        break;
                                    default:
                                        break;
                                }
                                lbl_state.Text = "Status : " + status;
                                if (scores < 4)
                                {
                                    lbl_state.ForeColor = Color.Red;
                                    return;
                                }
                                lbl_state.ForeColor = Color.Green;

                                string newpwd = tb_newpass.Text.ToString().Trim();
                                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                                byte[] newsaltByte = new byte[8];
                                rng.GetBytes(newsaltByte);
                                newsalt = Convert.ToBase64String(newsaltByte);
                                SHA512Managed newhashing = new SHA512Managed();
                                string newpwdWithSalt = newpwd + newsalt;
                                byte[] plainHash = newhashing.ComputeHash(Encoding.UTF8.GetBytes(newpwd));
                                byte[] newhashWithSalt = newhashing.ComputeHash(Encoding.UTF8.GetBytes(newpwdWithSalt));
                                newfinalHash = Convert.ToBase64String(newhashWithSalt);
                                int pass = UpdatePass(username, newfinalHash, newsalt);
                                DateTime minpass = DateTime.Now.AddMinutes(5);
                                DateTime maxpass = DateTime.Now.AddMinutes(15);

                                int mintime = UpdateMinTime(username, minpass);
                                int maxtime = UpdateMaxTime(username, maxpass);
                                if (pass == 1 && mintime ==1 && maxtime ==1)
                                {
                                    Response.Redirect("Homepage.aspx", false);
                                }


                            }
                        }
                        else
                        {
                            lb_error.Text = "Please wait a while before changin password again.";
                        }
                    }
                    else
                    {
                        lb_error.Text = "Old password does not match";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


            protected string getDBHash(string username)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);

            string sql = "select PasswordHash FROM Registration WHERE Email=@paraEmail";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@paraEmail", username);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
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
            return h;
        }



        protected string getDBSalt(string username)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordSalt FROM Registration WHERE Email=@paraEmail";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@paraEmail", username);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PASSWORDSALT"] != null)
                        {
                            if (reader["PASSWORDSALT"] != DBNull.Value)
                            {
                                s = reader["PASSWORDSALT"].ToString();
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
            return s;
        }

        protected int UpdatePass(string username, string hash,string salt)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "Update Registration SET PasswordHash = @paraPasswordHash,PasswordSalt = @paraPasswordSalt WHERE Email=@paraEmail";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@paraEmail", username);
            command.Parameters.AddWithValue("@paraPasswordHash", hash);
            command.Parameters.AddWithValue("@paraPasswordSalt", salt);
            try
            {
                connection.Open();
                int result = command.ExecuteNonQuery();
                return result;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }

        }

        private int checkPassword(string password)
        {
            int score = 0;



            if (password.Length < 8)
            {
                lbl_pwdchecker.Text += "Password length needs to be at lease 8 characters ";
                return 1;


            }
            else
            {
                score += 1;
            }
            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }
            else
            {
                lbl_pwdchecker.Text += "Password needs a lower case character";
            }
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }
            else
            {
                lbl_pwdchecker.Text += "Password needs a Upper case character";
            }
            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }
            else
            {
                lbl_pwdchecker.Text += "Password needs a Number";
            }

            if (Regex.IsMatch(password, "[^A-Za-z0-9]"))
            {
                score++;
            }
            else
            {
                lbl_pwdchecker.Text += "Password needs a special character";
            }



            return score;

        }
        protected DateTime getMinTime(string username)
        {
            DateTime h = DateTime.Now;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select Minpass FROM Registration WHERE Email=@paraEmail";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@paraEmail", username);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["Minpass"] != null)
                        {
                            if (reader["Minpass"] != DBNull.Value)
                            {
                                h = Convert.ToDateTime(reader["Minpass"]);
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
            return h;
        }
        protected int UpdateMinTime(string username, DateTime mintime)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "Update Registration SET Minpass = @paraMinpass WHERE Email=@paraEmail";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@paraEmail", username);
            command.Parameters.AddWithValue("@paraMinpass", mintime);
            
            try
            {
                connection.Open();
                int result = command.ExecuteNonQuery();
                return result;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }

        }
        protected int UpdateMaxTime(string username, DateTime maxtime)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "Update Registration SET Maxpass = @paraMaxpass WHERE Email=@paraEmail";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@paraEmail", username);
            command.Parameters.AddWithValue("@paraMaxpass", maxtime);

            try
            {
                connection.Open();
                int result = command.ExecuteNonQuery();
                return result;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }

        }

    }
}