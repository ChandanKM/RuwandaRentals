
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Net.Http;
using App.UIServices.InterfaceServices;
using App.DataAccess;
using System.Data.SqlClient;
using System.Data;
namespace App.Web
{
    public class UserRoleProvider : RoleProvider
    {
        readonly ILoginServices _loginService;
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            try
            {
                
                    CemexDb con = new CemexDb();
                    SqlConnection conn = con.GetConnection();
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("[proc_AuthenticateUserRole]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userid", username);

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<string> lst = new List<string>();
                    string user_Id = string.Empty, Authority_Id = string.Empty, user_Code = string.Empty, user_type = string.Empty;
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            lst.Add(reader["Authority_Id"].ToString());

                        }
                        conn.Close();
                    }

                    if (lst.Count > 0)
                    {

                        Authority_Id = lst[0];
                        if (Authority_Id == "3")
                        {
                            string[] Result = { "Admin" };
                            return Result;

                        }
                        else if (Authority_Id == "4")
                        {
                            string[] Result = { "Admin" };
                            return Result;

                        }
                        else if (Authority_Id == "5")
                        {
                            string[] Result = { "Admin" };
                            return Result;

                        }
                        else if (Authority_Id == "6")
                        {
                            string[] Result = { "Corporate" };
                            return Result;

                        }
                        else if (Authority_Id == "1")
                        {
                            string[] Result = { "SuperAdmin" };
                            return Result;

                        }
                        else if (Authority_Id == "2")
                        {
                            string[] Result = { "SuperAdmin" };
                            return Result;

                        }
                        else
                        {
                            string[] Result = { "Consumer" };
                            return Result;

                        }

                    }
                    else
                    {
                        string[] Result = { "Consumer" };
                        return Result;

                    }
                    //  string user_Id = User.Identity.Name;
                
            }

            catch
            {
                string[] Result = { "Consumer" };
                return Result;
            }
        }
        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}