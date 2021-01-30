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


namespace AS_Assignment
{
    public partial class Login : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYACC"].ConnectionString;
        public class MyObject
        {
            public string success { get; set; }

            public List<string> ErrorMessage { get; set; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           

        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            if (tb_userid.Text != null)
            {


                string pwd = tb_pass.Text.ToString().Trim();
                string username = tb_userid.Text.ToString().Trim();
                SHA512Managed hashing = new SHA512Managed();
                string dbHash = getDBHash(username);
                string dbSalt = getDBSalt(username);
                int attempt = Convert.ToInt32(getAttempt(username));
                string timenow = getTime(username).ToString();
                DateTime maxtime = getMaxTime(username);
                System.Diagnostics.Debug.WriteLine(dbHash);
                try
                {
                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                    {
                        if (timenow == "a")
                        {
                            if (attempt < 3)
                            {
                                string pwdWithSalt = pwd + dbSalt;
                                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                                string userHash = Convert.ToBase64String(hashWithSalt);
                                if (userHash.Equals(dbHash))
                                {
                                    if (DateTime.Compare(DateTime.Now, maxtime) < 0)
                                    {
                                        string reset = "a";
                                        int time = UpdateTime(username, reset);
                                        System.Diagnostics.Debug.WriteLine("login");

                                        Session["LoggedIn"] = tb_userid.Text.Trim();
                                        string guid = Guid.NewGuid().ToString();
                                        Session["AuthToken"] = guid;
                                        Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                                        Response.Redirect("HomePage.aspx", false);
                                    }
                                    else
                                    {


                                        Session["LoggedIn"] = tb_userid.Text.Trim();
                                        string guid = Guid.NewGuid().ToString();
                                        Session["AuthToken"] = guid;
                                        Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                                        Response.Redirect("ChangePassword.aspx", false);
                                    }
                                }
                                else
                                {
                                    attempt += 1;
                                    int update;
                                    update = UpdateAttempt(username, attempt);
                                    if (update == 1)
                                    {
                                        LblMsg.Visible = true;
                                        LblMsg.Text = "Userid or password is not valid. Please try again.";
                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.WriteLine("Unable to update Attempts");
                                    }
                                }
                            }
                            else
                            {
                                LblMsg.Text = "Account has been locked out. Try again in 10 minutes.";

                                attempt = 0;
                                int update;
                                int time;
                                update = UpdateAttempt(username, attempt);
                                if (update == 1)
                                {
                                    string date = DateTime.Now.AddMinutes(10).ToString();
                                    time = UpdateTime(username, date);

                                    System.Diagnostics.Debug.WriteLine("Attempts updated", "time: ", date);
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine("Unable to update Attempts");
                                }
                            }
                        }
                        else if (DateTime.Compare(DateTime.Now, Convert.ToDateTime(timenow)) > 0)
                        {

                            if (attempt < 3)
                            {
                                string pwdWithSalt = pwd + dbSalt;
                                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                                string userHash = Convert.ToBase64String(hashWithSalt);
                                if (userHash.Equals(dbHash))
                                {
                                    if (DateTime.Compare(DateTime.Now, maxtime) < 0)
                                    {
                                        string reset = "";
                                        int time = UpdateTime(username, reset);
                                        System.Diagnostics.Debug.WriteLine("login");

                                        Session["LoggedIn"] = tb_userid.Text.Trim();
                                        string guid = Guid.NewGuid().ToString();
                                        Session["AuthToken"] = guid;
                                        Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                                        Response.Redirect("HomePage.aspx", false);
                                    }
                                    else
                                    {
                                        Session["LoggedIn"] = tb_userid.Text.Trim();
                                        string guid = Guid.NewGuid().ToString();
                                        Session["AuthToken"] = guid;
                                        Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                                        Response.Redirect("ChangePassword.aspx", false);
                                    }
                                }
                                else
                                {
                                    attempt += 1;
                                    int update;
                                    update = UpdateAttempt(username, attempt);
                                    if (update == 1)
                                    {
                                        LblMsg.Visible = true;
                                        LblMsg.Text = "Userid or password is not valid. Please try again.";
                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.WriteLine("Unable to update Attempts");
                                    }
                                }
                            }

                            else
                            {
                                LblMsg.Text = "Account has been locked out. Try again in 10 minutes.";

                                attempt = 0;
                                int update;
                                int time;
                                update = UpdateAttempt(username, attempt);
                                if (update == 1)
                                {
                                    string date = DateTime.Now.AddMinutes(10).ToString();
                                    time = UpdateTime(username, date);

                                    System.Diagnostics.Debug.WriteLine("Attempts updated", "time: ", date);
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine("Unable to update Attempts");
                                }
                            }
                        }
                        else
                        {
                            LblMsg.Text = "Please wait for your account to be unlocked";
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

            }
            //if (tb_userid.Text.Trim().Equals("u") && tb_pass.Text.Trim().Equals("p"))
            //{
            //    Session["LoggedIn"] = tb_userid.Text.Trim();
            //    string guid = Guid.NewGuid().ToString();
            //    Session["AuthToken"] = guid;
            //    Response.Cookies.Add(new HttpCookie("AuthToken", guid));
            //    Response.Redirect("HomePage.aspx", false);

            //}
            //else
            //{
            //    lbl_Message.Text = "Wrong username or password";

            //}
        }


        public bool validateCaptcha()
        {
            bool result = true;
            string captchaResponse = Request.Form["g-recaptcha-response"];
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
                (" https://www.google.com/recaptcha/api/siteverify?secret=6LdHhiUaAAAAAAU2LzudU3F596RM7viycpUgC5Dq &response=" + captchaResponse);
            //6Lf1e-QZAAAAAKl4rh7eojWK5zTD95W0MOVe3wsk secret v3
            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();

                        lblMessage.Text = jsonResponse.ToString();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

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
        protected string getAttempt(string username)
        {
            string a =null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select Attempts FROM Registration WHERE Email=@paraEmail";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@paraEmail", username);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Attempts"] != null)
                        {
                            if (reader["Attempts"] != DBNull.Value)
                            {
                                a = reader["Attempts"].ToString();
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
            return a;
        }
        protected int UpdateAttempt(string username, int attempt)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "Update Registration SET Attempts = @paraattempt WHERE Email=@paraEmail";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@paraEmail", username);
            command.Parameters.AddWithValue("@paraattempt", attempt);
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

        protected int UpdateTime(string username, string time)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "Update Registration SET Locktime = @paralocktime WHERE Email=@paraEmail";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@paraEmail", username);
            command.Parameters.AddWithValue("@paralocktime", time);
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

        protected string getTime(string username)
        {
            string a = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select Locktime FROM Registration WHERE Email=@paraEmail";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@paraEmail", username);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Locktime"] != null)
                        {
                            if (reader["Locktime"] != DBNull.Value)
                            {
                                a = reader["Locktime"].ToString();
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
            return a;
        }
        protected DateTime getMaxTime(string username)
        {
            DateTime h =  DateTime.Now;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select Maxpass FROM Registration WHERE Email=@paraEmail";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@paraEmail", username);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["Maxpass"] != null)
                        {
                            if (reader["Maxpass"] != DBNull.Value)
                            {
                                h = Convert.ToDateTime(reader["Maxpass"]);
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

        protected void btn_register(object sender, EventArgs e)
        {
            Response.Redirect("Registration.aspx");
        }
    }
}