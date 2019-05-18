using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Practice.Models;

namespace Practice.Models
{
    public class CustomerDataHandler
    {
        private static SqlConnection con;
        public static Profile pobj;
        private static void Connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["LudoConn"].ToString();
            con = new SqlConnection(constring);
        }
        public static string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

        public static bool Verify(string uname, string password)
        {
            try
            {
                Connection();
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "select * from Data_CC where Username = '" + uname + "' and Password = '" + password + "'";
                SqlDataReader dr1 = cmd.ExecuteReader();
                while (dr1.Read())
                {
                    con.Close();
                    return true;
                }
                con.Close();
                return false;
            }
            catch (System.Exception)
            { return false; }
        
        }
        public static bool ChechUname(string uname)
        {
            try
            {
                Connection();
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "select * from Personal_Info where Username = '" + uname + "'";
                SqlDataReader dr1 = cmd.ExecuteReader();
                while (dr1.Read())
                {
                    con.Close();
                    return false;
                }
                con.Close();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }

        }
        public static bool Register(Profile obj)
        {
            try
            {
                obj.Gender = RemoveWhitespace(obj.Gender);
                Connection();
                SqlCommand cmd = new SqlCommand("RegisterUser", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@uname", Profile.Username);
                cmd.Parameters.AddWithValue("@name", obj.Full_Name);
                cmd.Parameters.AddWithValue("@dateofbirth", obj.Dateofbirth);
                cmd.Parameters.AddWithValue("@gender", obj.Gender);
                cmd.Parameters.AddWithValue("@password", Profile.Password);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (System.Exception)
            { return false; }

}
        public async System.Threading.Tasks.Task<bool> PasswordResetAsync(string newPassword)
        {
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
                UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(context);
                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(store);
                var user = new IdentityUser() { UserName = Models.Profile.Username };
                String userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                String hashedNewPassword = UserManager.PasswordHasher.HashPassword(newPassword);
                ApplicationUser cUser = await store.FindByIdAsync(userId);
                await store.SetPasswordHashAsync(cUser, hashedNewPassword);
                await store.UpdateAsync(cUser);
                return true;
            }
            catch(Exception)
            { return false; }
        }
        public async System.Threading.Tasks.Task<bool> SaveProfileAsync(Profile obj)
        {
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("UpdateProfile", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
               

                cmd.Parameters.AddWithValue("@Username", Profile.oldUsername);
                cmd.Parameters.AddWithValue("@uname", Profile.Username);
                cmd.Parameters.AddWithValue("@location", obj.Locations);
                cmd.Parameters.AddWithValue("@name", obj.Full_Name);
                cmd.Parameters.AddWithValue("@dateofbirth", obj.Dateofbirth);
                cmd.Parameters.AddWithValue("@mobile", obj.Mobile);
                cmd.Parameters.AddWithValue("@address", obj.Address);
                cmd.Parameters.AddWithValue("@gender", obj.Gender);
                cmd.Parameters.AddWithValue("@email", obj.Email);
                cmd.Parameters.AddWithValue("@password", Profile.Password);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (Profile.Password != obj.Passwords)
                    await PasswordResetAsync(Profile.Password);
                    if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (System.Exception)
            { return false; }
        }
        public static Profile GetProfile(string uname)
        {
            try
            {
                Profile Profiledata = new Profile();

                Connection();
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "select * from Personal_Info where Username = '" + uname + "'";
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Profiledata.Address = (dr["Address"]).ToString();
                    Profiledata.Locations = (dr["Location"]).ToString();
                    Profiledata.Mobile = (dr["Mobile"]).ToString();
                    Profiledata.Full_Name = (dr["FullName"]).ToString();
                    Profiledata.Dateofbirth = (dr["DateOfBirth"]).ToString();
                    Profiledata.Gender = (dr["Gender"]).ToString();
                    break;
                }
                con.Close();

                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = "select Email from Data_CC where Username = '" + uname + "'";
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Profiledata.Email = (dr["Email"]).ToString();
                    break;
                }
                con.Close();
                pobj = Profiledata;
                return Profiledata;
            }
            catch (System.Exception)
            { return null; }
        }
    }
}