using RomBe.Entities.Class.Common;
using RomBe.Entities.Class.Request;
using RomBe.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.DAL
{
    public class ChildDAL
    {
        #region public methods

        public async Task CreateChild(ChildObject request, int currentUserId)
        {
            try
            {
                using (RombeEntities context = new RombeEntities())
                {
                    User _currentUser = new UserDAL().GetUser(currentUserId);
                    bool isChildExist = context.Children.Where(c => c.UserId == _currentUser.UserId && c.FirstName == request.FirstName.ToLower().Trim()).Any();
                    if (!isChildExist)
                    {
                        Child newChild = new Child();
                        newChild.UserId = _currentUser.UserId;
                        newChild.BirthDate = request.BirthDate;
                        newChild.FirstName = request.FirstName;
                        newChild.GenderId = (int)request.Gender;


                        newChild.InsertDate = DateTime.Now;
                        newChild.UpdateDate = DateTime.Now;

                        context.Children.Add(newChild);
                        await context.SaveChangesAsync().ConfigureAwait(false);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<Child> GetChildList(int userId)
        {
            try
            {
                using (RombeEntities context = new RombeEntities())
                {
                    return (from child in context.Children
                            where child.UserId == userId
                            select child).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task UpdateChild(ChildObject request, int currentUserId)
        {
            ValidationHelper validationHelper = new ValidationHelper();
            using (RombeEntities context = new RombeEntities())
            {
                Child childToUpdate = context.Children.Where(c => c.ChildId == request.ChildId.Value && c.UserId == currentUserId).FirstOrDefault();
                if (!childToUpdate.IsNull())
                {
                    childToUpdate.FirstName = validationHelper.GetLatestValue(childToUpdate.FirstName, request.FirstName);
                    childToUpdate.BirthDate = validationHelper.GetLatestValue(childToUpdate.BirthDate, request.BirthDate);
                    childToUpdate.GenderId = validationHelper.GetLatestValue(childToUpdate.GenderId, (int)request.Gender);

                    await context.SaveChangesAsync().ConfigureAwait(false);

                }
            }
        }
        public async Task DeleteChild(DeleteChildRequest request, int currentUserId)
        {
            using (RombeEntities context = new RombeEntities())
            {
                Child childToDelete = context.Children.Where(c => c.ChildId == request.ChildId && c.UserId == currentUserId).FirstOrDefault();
                if (!childToDelete.IsNull())
                {
                    context.Children.Remove(childToDelete);
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
            }
        }
        public DateTime? GetChildBirthDate(int childId)
        {
            using (RombeEntities context = new RombeEntities())
            {
                Child child = context.Children.Where(c => c.ChildId == childId).FirstOrDefault();
                if (!child.IsNull())
                {
                    return child.BirthDate;
                }
                else
                {
                    return null;
                }
            }
        }
        
        #endregion public methods


    }
}
