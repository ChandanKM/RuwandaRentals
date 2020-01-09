using System;

namespace App.BusinessObject
{
    public class UserProfileBo
    {
        public int User_Id { get; set; }
        public int Authority_Id { get; set; }
        public string User_Name { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Pswd { get; set; }
        public string Activated_By { get; set; }
        public string Department { get; set; }
        public int UserType { get; set; }
    }

    public class UserProifleMasterBo
    {
        public int Authority_Id { get; set; }
        public string Code { get; set; }
    }

}
