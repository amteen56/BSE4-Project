using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Practice.Models
{
    public class AdminDataHandler
    {
        static SqlConnection con;
        private static void Connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["LudoConn"].ToString();
            con = new SqlConnection(constring);
        }

        public static List<UserRecords> GerUsersRecords()
        {
            Connection();
            List<UserRecords> UserRecords = new List<UserRecords>();

            SqlCommand cmd = new SqlCommand("GetUsersRecords", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                UserRecords.Add(
                    new UserRecords
                    {
                        UserName = Convert.ToString(dr["Username"]),
                        Total_Played= Convert.ToInt32(dr["Total_Play"]),
                        Total_Wins = Convert.ToInt32(dr["Total_Wins"]),
                        
                    });
            }
            return UserRecords;
        }

            public static List<Feedback> GetFeedbackData()
            {
                Connection();
                List<Feedback> UserRecords = new List<Feedback>();

                SqlCommand cmd = new SqlCommand("GetFeedBack", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                con.Open();
                sd.Fill(dt);
                con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    UserRecords.Add(
                        new Feedback
                        {
                            Username = Convert.ToString(dr["UserName"]),
                            Against_Username = Convert.ToString(dr["Against_UserName"]),
                            Id = Convert.ToInt32(dr["Id"]),
                            Report_Data = Convert.ToString(dr["Report"]),
                        });
                }
                return UserRecords;
            }
        public static List<Feedback> GetBugFeedbackData()
        {
            Connection();
            List<Feedback> UserRecords = new List<Feedback>();

            SqlCommand cmd = new SqlCommand("GetBugFeedBack", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                UserRecords.Add(
                    new Feedback
                    {
                        Username = Convert.ToString(dr["UserName"]),
                        Id = Convert.ToInt32(dr["Id"]),
                        Report_Data = Convert.ToString(dr["Report"]),
                    });
            }
            return UserRecords;
        }

        public static List<Profile> GerUsersData()
        {
            Connection();
            List<Profile> UserData = new List<Profile>();

            SqlCommand cmd = new SqlCommand("GetUsersData", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                UserData.Add(
                    new Profile
                    {
                        UserName = Convert.ToString(dr["Username"]),
                        Full_Name = Convert.ToString(dr["FullName"]),
                        Email = Convert.ToString(dr["Email"]),
                        Locations = dr["Location"].ToString(),
                        Dateofbirth = dr["DateOfBirth"].ToString(),
                        Mobile = dr["Mobile"].ToString(),
                        Address = dr["Address"].ToString(),
                        Gender = dr["Gender"].ToString(),
                    });
            }
            return UserData;
        }
        public static List<Profile> GetAdminsData()
        {
            Connection();
            List<Profile> UserData = new List<Profile>();

            SqlCommand cmd = new SqlCommand("GetAdminData", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                UserData.Add(
                    new Profile
                    {
                        UserName = Convert.ToString(dr["Username"]),
                        Full_Name = Convert.ToString(dr["FullName"]),
                        Email = Convert.ToString(dr["Email"]),
                        Locations = dr["Location"].ToString(),
                        Dateofbirth = dr["DateOfBirth"].ToString(),
                        Mobile = dr["Mobile"].ToString(),
                        Address = dr["Address"].ToString(),
                        Gender = dr["Gender"].ToString(),
                    });
            }
            return UserData;
        }
        public static bool DeleteUser(string username)
        {
            Connection();
            try
            {
                SqlCommand cmd = new SqlCommand("DeleteUser", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@uname", username);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch(Exception)
            { return false; }
        }
        public static bool UpdateDetails(Profile smodel)
        {
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("UpdateDetails", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@loc", smodel.Locations);
                cmd.Parameters.AddWithValue("@name", smodel.Full_Name);
                cmd.Parameters.AddWithValue("@dob", smodel.Dateofbirth);
                cmd.Parameters.AddWithValue("@mob", smodel.Mobile);
                cmd.Parameters.AddWithValue("@address", smodel.Address);
                cmd.Parameters.AddWithValue("@gender", smodel.Gender);
                cmd.Parameters.AddWithValue("@email", smodel.Email);
                cmd.Parameters.AddWithValue("@uname", smodel.UserName);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception) { return false; }
        }
        public static bool UpdateFeedBack(int id)
        {
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("UpdateFeedBack", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception) { return false; }
        }

        public static bool DeleteFeedBack(int id)
        {
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("deleteFeedBack", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception) { return false; }
        }
        public static bool CreateUser(Profile smodel)
        {
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("CreateUser", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@location", smodel.Locations);
                cmd.Parameters.AddWithValue("@name", smodel.Full_Name);
                cmd.Parameters.AddWithValue("@dateofbirth", smodel.Dateofbirth);
                cmd.Parameters.AddWithValue("@mobile", smodel.Mobile);
                cmd.Parameters.AddWithValue("@address", smodel.Address);
                cmd.Parameters.AddWithValue("@gender", smodel.Gender);
                cmd.Parameters.AddWithValue("@email", smodel.Email);
                cmd.Parameters.AddWithValue("@uname", smodel.UserName);
                cmd.Parameters.AddWithValue("@password", smodel.Passwords);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception) { return false; }
        }

    }
}