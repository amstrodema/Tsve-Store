using App.Services;
using Microsoft.IdentityModel.Tokens;
using Store.Data.Interface;
using Store.Model;
using Store.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Business
{
    public class GroupBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly GenericBusiness _genericBusiness;
        public GroupBusiness(IUnitOfWork unitOfWork, GenericBusiness genericBusiness)
        {
            _unitOfWork = unitOfWork;
            _genericBusiness = genericBusiness;
        }
        public async Task<IEnumerable<GroupVM>> GetCatList()
        {
            return from grp in await _unitOfWork.Groups.GetByGroupsTag(_genericBusiness.StoreID)
                      join cat in await _unitOfWork.Categories.GetAll() on grp.ID equals cat.GroupID into cats
                      select new GroupVM()
                      {
                          Categories = cats,
                          Group = grp
                      };
        }
        public async Task<MainVM> GetVM(string t)
        {
            Group grp = new Group();
            try
            {
                grp = await _unitOfWork.Groups.Find(Guid.Parse(t));
            }
            catch (Exception)
            {
                grp = await _unitOfWork.Groups.GetByGroupTag(t, _genericBusiness.StoreID);
            }
            MainVM mainVM = new MainVM();
            mainVM.Group = grp;
            return mainVM;
        }
        public async Task<MainVM> GetVM()
        {
            MainVM mainVM = new MainVM();
            mainVM.Groups = await _unitOfWork.Groups.GetByStoreID(_genericBusiness.StoreID);
            mainVM.Groups = mainVM.Groups.OrderBy(p=> p.Name);
            return mainVM;
        }
        public async Task<ResponseMessage<string>> Create(MainVM mainVM, User user)
        {
            ResponseMessage<string> Tsve_ResponseMessage = new ResponseMessage<string>();

            try
            {
                if (string.IsNullOrEmpty(mainVM.Group.Name))
                {
                    Tsve_ResponseMessage.StatusCode = 201;
                    Tsve_ResponseMessage.Message = "Not Saved! Add a group name.";
                    return Tsve_ResponseMessage;
                }
                mainVM.Group.ID = Guid.NewGuid();
                mainVM.Group.DateCreated = DateTime.UtcNow.AddHours(1);
                mainVM.Group.CreatedBy = user.ID;
                mainVM.Group.IsActive = true;
                mainVM.Group.StoreID = _genericBusiness.StoreID;
                mainVM.Group.Tag = GenericService.GetTag(mainVM.Group.Name);

                var thisGroup = await _unitOfWork.Groups.GetByGroupTag(mainVM.Group.Tag.Trim(), _genericBusiness.StoreID);

                if (thisGroup != null)
                {
                    Tsve_ResponseMessage.StatusCode = 201;
                    Tsve_ResponseMessage.Message = "Group exists";
                    return Tsve_ResponseMessage;
                }
                await _unitOfWork.Groups.Create(mainVM.Group);


                if (await _unitOfWork.Commit() > 0)
                {
                    Tsve_ResponseMessage.StatusCode = 200;
                    Tsve_ResponseMessage.Message = "Group Saved!";
                }
                else
                {
                    Tsve_ResponseMessage.StatusCode = 201;
                    Tsve_ResponseMessage.Message = "Group Not Saved!";
                }
            }
            catch (Exception)
            {
                Tsve_ResponseMessage.StatusCode = 201;
                Tsve_ResponseMessage.Message = "Group Not Saved!";
            }
            return Tsve_ResponseMessage;
        }
        public async Task<ResponseMessage<string>> Modify(MainVM mainVM, User user)
        {
            ResponseMessage<string> Tsve_ResponseMessage = new ResponseMessage<string>();

            try
            {

                var grp = await _unitOfWork.Groups.GetByGroupTag(mainVM.Group.Tag, _genericBusiness.StoreID);
                if (grp == null)
                {
                    Tsve_ResponseMessage.StatusCode = 201;
                    Tsve_ResponseMessage.Message = "Group Not Found!";
                    return Tsve_ResponseMessage;
                }

                grp.Name = mainVM.Group.Name;

                if (string.IsNullOrEmpty(grp.Name))
                {
                    Tsve_ResponseMessage.StatusCode = 201;
                    Tsve_ResponseMessage.Message = "Not Saved! Add a group name.";
                    return Tsve_ResponseMessage;
                }

                grp.DateModified = DateTime.UtcNow.AddHours(1);
                grp.ModifiedBy = user.ID;
                grp.Tag = GenericService.GetTag(grp.Name);

                var thisGroup = await _unitOfWork.Groups.GetByGroupTag(grp.Tag.Trim(), _genericBusiness.StoreID);

                if (thisGroup != null)
                {
                    Tsve_ResponseMessage.StatusCode = 201;
                    Tsve_ResponseMessage.Message = "Group exists";
                    return Tsve_ResponseMessage;
                }

                _unitOfWork.Groups.Update(grp);

                if (await _unitOfWork.Commit() > 0)
                {
                    Tsve_ResponseMessage.StatusCode = 200;
                    Tsve_ResponseMessage.Message = "Group Modified!";
                }
                else
                {
                    Tsve_ResponseMessage.StatusCode = 201;
                    Tsve_ResponseMessage.Message = "Group Not Modified!";
                }
            }
            catch (Exception)
            {
                Tsve_ResponseMessage.StatusCode = 201;
                Tsve_ResponseMessage.Message = "Group Not Modified!";
            }
            return Tsve_ResponseMessage;
        }
    }
}
