using System.Collections.Generic;
using App.BusinessObject;
using App.Common;
using App.Domain;
using System.Data;

namespace App.UIServices
{
   public interface  IUserService
    {
       TransactionStatus CreateUser(UserBo userBo);
       TransactionStatus EditUser(UserBo1 userBo);
       List<object> Bind();
       List<object> Edit(int Id);
       TransactionStatus DeleteUser(UserBo1 userBo);

   
    }

}
