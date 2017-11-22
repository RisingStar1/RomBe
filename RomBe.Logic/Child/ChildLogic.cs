using RomBe.Entities;
using RomBe.Entities.Class.Common;
using RomBe.Entities.Class.Request;
using RomBe.Entities.Class.Respone;
using RomBe.Entities.Class.Response;
using RomBe.Entities.DAL;
using RomBe.Entities.Enums;
using RomBe.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Logic.ChildLogic
{
    public class ChildLogic
    {
        #region public methods
        public async Task<GetUserDetailsResponse> CreateChild(ChildObject request)
        {
            try
            {
                //get the userId from context
                int? _currentUserId = new AuthenticationHelper().GetCurrentUserId();
                if (_currentUserId.HasValue)
                {
                    await new ChildDAL().CreateChild(request, _currentUserId.Value);
                }

                return new UserLogic.UserLogic().GetUserDetails();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<GetUserDetailsResponse> UpdateChild(ChildObject request)
        {
            try
            {
                int? _currentUserId = new AuthenticationHelper().GetCurrentUserId();
                if (!request.IsNull() && _currentUserId.HasValue)
                {
                    await new ChildDAL().UpdateChild(request, _currentUserId.Value);
                }

                return new UserLogic.UserLogic().GetUserDetails();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<GetUserDetailsResponse> DeleteChild(DeleteChildRequest request)
        {
            int? _currentUserId = new AuthenticationHelper().GetCurrentUserId();
            if (!request.IsNull() && _currentUserId.HasValue)
            {
                await new ChildDAL().DeleteChild(request, _currentUserId.Value);
            }
            return new UserLogic.UserLogic().GetUserDetails();
        }
        public List<ChildObject> GetChildsList(int userId)
        {
            List<Child> childList = new ChildDAL().GetChildList(userId);
            List<ChildObject> childsObjectList = new List<ChildObject>();
            foreach (Child child in childList)
            {
                childsObjectList.Add(new ChildObject
                {
                    FirstName = child.FirstName,
                    BirthDate = child.BirthDate,
                    ChildId=child.ChildId,
                    Gender = (GenderTypeEnum)child.GenderId
                });
            }
            return childsObjectList;
        }

        #endregion public methods

    }
}
