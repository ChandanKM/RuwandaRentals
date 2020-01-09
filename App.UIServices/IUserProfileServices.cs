using System.Collections.Generic;
using App.BusinessObject;
using App.Common;
using App.Domain;
using System.Data;

namespace App.UIServices
{
    public interface IUserProfileServices
    {
        DataSet AddUserProfile(UserProfileBo userprofileBo);
        TransactionStatus UpdateUserProfile(UserProfileBo userprofileBo);
        TransactionStatus SuspendUserProfile(int userprofile_Id);
        List<object> GetProfileMaster();
        DataSet GetUserProfile(string AuthId, string UserId,string PropId);

        DataSet GetUserPermission(int authId,int userId);
        DataSet GetSuperAdminsPermission(int authId, int userId);
        
        TransactionStatus UpdatePermissionFlag(int userId, int pageId, string flag);
        TransactionStatus UpdateSuperAdminPermissionFlag(int userId, int pageId, string flag);
    }
}
