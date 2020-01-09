using System.Collections.Generic;
using App.BusinessObject;
using App.Common;
using App.Domain;
using System.Data;

namespace App.UIServices.InterfaceServices
{
    public interface ILoginServices
    {
        List<string> AuthenticateUser(LoginBo loginBo);
        List<string> AuthenticateUserRole(string username);
        ForgotPassword FindUserByEmail(ForgotPasswordBo forgotBo);
        EmailMaster EmailCredentials();
        TransactionStatus ResetPassword(ResetPasswordBo resetpaswdBo);
       
    }
}
