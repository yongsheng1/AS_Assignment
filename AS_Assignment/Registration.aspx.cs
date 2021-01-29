using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace AS_Assignment
{
    
    public partial class Registration : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYACC"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        DateTime dt;
        int attempt = 0;
        string time = "a";
        DateTime minpass;
        DateTime maxpass;

        protected void Page_Load(object sender, EventArgs e)
        {
           

        }
        private int checkInput()
        {
            lbl_error.Text = "";
            lbl_error.Visible = false;
            int score = 0;
            //if (Regex.IsMatch(tb_userid.Text, "/^[a-z0-9][-a-z0-9._]+@([-a-z0-9])+[a-z]{2,5}$/"))
            //{
            //    score += 1;

            //}
            if(tb_userid.Text != null)
            {
                score += 1;
            }
            else
            {
                lbl_error.Text += "Please Enter a vaild email  <br/>";
            }
            if (Regex.IsMatch(tb_fname.Text, "[A-Za-z]"))
            {
                score += 1;
            }
            else
            {
                lbl_error.Text += "Please Enter a vaild First Name <br/>";
            }
            if (Regex.IsMatch(tb_lname.Text, "[A-Za-z]"))
            {
                score += 1;
            }
            else
            {
                lbl_error.Text += "Please Enter a vaild Last Name <br/>";
            }
            if (DateTime.TryParse(tb_dob.Text, out dt))
            {
                score += 1;
            }
            else
            {
                lbl_error.Text += "Please Enter a vaild BirthDate <br/>";
            }
            if (tb_card.Text.Length ==16)
            {
                score += 1;
            }
            else
            {
                lbl_error.Text += "Please Enter a vaild Card number <br/>";
            }
            return score;

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
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            int check = checkInput();
            if (check == 5)
            {
                lbl_state.Text = "";
                int scores = checkPassword(tb_pwd.Text);
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



                string pwd = tb_pwd.Text.ToString().Trim(); ;
                //Generate random "salt"
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] saltByte = new byte[8];
                //Fills array of bytes with a cryptographically strong sequence of random values.
                rng.GetBytes(saltByte);
                salt = Convert.ToBase64String(saltByte);
                SHA512Managed hashing = new SHA512Managed();
                string pwdWithSalt = pwd + salt;
                byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                finalHash = Convert.ToBase64String(hashWithSalt);
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.GenerateKey();
                Key = cipher.Key;
                IV = cipher.IV;
                minpass = DateTime.Now.AddMinutes(5);
                maxpass = DateTime.Now.AddMinutes(15);
                createAccount();
                Response.Redirect("Login.aspx", false);
            }
            else
            {
                lbl_error.Visible = true;
                lbl_error.ForeColor = Color.Red;
            }
        }

        protected void createAccount()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Registration(FName,LName , Card , PasswordHash,PasswordSalt,Dob,Email,Attempts,LockTime,Minpass,Maxpass) "+ "VALUES (@paraFName, @paraLName, @paraCard, @paraPasswordHash, @paraPasswordSalt, @paraDob, @paraEmail, @paraAttempts,@paraLocktime,@paraMinpass,@paraMaxpass)"))
                {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@paraEmail", tb_userid.Text.Trim());
                            cmd.Parameters.AddWithValue("@paraFName", tb_fname.Text.Trim());
                            cmd.Parameters.AddWithValue("@paraLName", tb_lname.Text);
                            cmd.Parameters.AddWithValue("@paraPasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@paraPasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@paraCard", encryptData(tb_card.Text));
                            cmd.Parameters.AddWithValue("@paraDob", tb_dob.Text);
                            cmd.Parameters.AddWithValue("@paraAttempts", attempt);
                            cmd.Parameters.AddWithValue("@paraLocktime", time);
                            cmd.Parameters.AddWithValue("@paraMinpass", minpass );
                            cmd.Parameters.AddWithValue("@paraMaxpass", maxpass);

                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
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
    }

}


