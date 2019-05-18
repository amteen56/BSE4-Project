using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Practice.Models
{
    public class GameDataHandler
    {
        static SqlConnection con;
        private static void Connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["LudoConn"].ToString();
            con = new SqlConnection(constring);
        }

        public static bool Report()
        {
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("AddFeedBack", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@username", Feedback.username);
                cmd.Parameters.AddWithValue("@againstunmae", Feedback.Againstusername);
                cmd.Parameters.AddWithValue("@data", Feedback.Report);
                cmd.Parameters.AddWithValue("@type", Feedback.type);
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
        public static bool addUsers(Users uname)
        {
            try
            {
                if (CheckUser(uname.Red_name, Profile.Username))
                    addsubuser(uname.Red_name, Profile.Username);
                if (CheckUser(uname.Green_name, Profile.Username))
                    addsubuser(uname.Green_name, Profile.Username);
                if (CheckUser(uname.Yellow_name, Profile.Username))
                    addsubuser(uname.Yellow_name, Profile.Username);
                if (CheckUser(uname.Blue_name, Profile.Username))
                    addsubuser(uname.Blue_name, Profile.Username);

                return true;
            }
            catch (Exception) { return false; }
        }
        public static bool addsubuser(string cname,string pname)
        {
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("AddSubUser", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@cname", cname);
                cmd.Parameters.AddWithValue("@puname", pname);

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
        public static bool CheckUser(string uname,string puname)
        {
            try
            {
                Connection();
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "select * from SubUserNames where Parent_Username = '" + uname + "' and Child_Username = '" + puname + "'";
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
        public static bool addPlay()
        {
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("AddPlay", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@rname", Users.rname);
                cmd.Parameters.AddWithValue("@bname", Users.bname);
                cmd.Parameters.AddWithValue("@yname", Users.yname);
                cmd.Parameters.AddWithValue("@gname", Users.gname);

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
        public static bool addWinner(string uname)
        {
            try
            {
                if (uname == "red") uname = Users.rname;
                else if (uname == "blue") uname = Users.bname;
                else if (uname == "yellow") uname = Users.yname;
                else if (uname == "green") uname = Users.gname;
                Connection();
                SqlCommand cmd = new SqlCommand("AddWinner", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@rname", uname);

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