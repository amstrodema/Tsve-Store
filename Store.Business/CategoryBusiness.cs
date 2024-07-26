using App.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Store.Data.Interface;
using Store.Model;
using Store.Model.Hybrid;
using Store.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Business
{
    public class CategoryBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly GenericBusiness _genericBusiness;
        public CategoryBusiness(IUnitOfWork unitOfWork, GenericBusiness genericBusiness)
        {
            _unitOfWork = unitOfWork;
            _genericBusiness = genericBusiness;
        }
        public async Task<Category> GetVM(string t)
        {
            Category categ = new Category();
            try
            {
                categ = await _unitOfWork.Categories.Find(Guid.Parse(t));
            }
            catch (Exception)
            {
               categ = await _unitOfWork.Categories.GetByCategoryTag(t);
            }
            categ.Image = categ.Image != "" ? ImageService.GetSmallImagePath(categ.Image, "CategoryIcons") : "";
            return categ;
        }

        public async Task<IEnumerable<Category>> Get()
        {
            return from cat in await _unitOfWork.Categories.GetAll()
                   select new Category()
                   {
                       ID = cat.ID,
                       Name = cat.Name,
                       Image = cat.Image != "" ? ImageService.GetSmallImagePath(cat.Image, "CategoryIcons") : "",
                       Tag = cat.Tag
                   };
        }

        public async Task<IEnumerable<CategoryHybrid>> GetHybrids()
        {
            return from cat in await _unitOfWork.Categories.GetAll()
                   join item in await _unitOfWork.Items.GetAll() on cat.ID equals item.CatID into catItems
                   select new CategoryHybrid()
                   {
                       ID = cat.ID,
                       Name = cat.Name,
                       Image = cat.Image != "" ? ImageService.GetLargeImagePath(cat.Image, "CategoryIcons") : "",
                       Tag = cat.Tag,
                       ItemsNo = catItems.Count()
                   };
        }
        public async Task<IEnumerable<CategoryHybrid>> GetSmallHybrids()
        {
            return from cat in await _unitOfWork.Categories.GetAll()
                   join item in await _unitOfWork.Items.GetAll() on cat.ID equals item.CatID into catItems
                   select new CategoryHybrid()
                   {
                       ID = cat.ID,
                       Name = cat.Name,
                       Image = cat.Image != "" ? ImageService.GetSmallImagePath(cat.Image, "CategoryIcons") : "",
                       Tag = cat.Tag,
                       ItemsNo = catItems.Count()
                   };
        }
        public async Task<MainVM> GetVM()
        {
            MainVM mainVM = new MainVM();
            mainVM.Categories = (from cat in await _unitOfWork.Categories.GetAll()
                                 select new Category()
                                 {
                                     ID = cat.ID,
                                     Name = cat.Name,
                                     Image = cat.Image != "" ? ImageService.GetSmallImagePath(cat.Image, "CategoryIcons") : "",
                                     Tag = cat.Tag,
                                     DateCreated = cat.DateModified == default ? cat.DateCreated : cat.DateModified
                                 }).OrderBy(p => p.Name);
           
            return mainVM;
        }
        public async Task<ResponseMessage<string>> Create(string name, User user, IFormFile image)
        {
            ResponseMessage<string> responseMessage = new ResponseMessage<string>();
            try
            {
                var categories = await _unitOfWork.Categories.GetAll();

                if(string.IsNullOrWhiteSpace(name))
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Not Saved! Name cannot be empty!";
                    return responseMessage;
                }
                Category category = new()
                {
                    Name = name,
                    Tag = GenericService.GetTag(name.Trim())
                };
                var thisCat = await _unitOfWork.Categories.GetByCategoryTag(category.Tag);

                if (thisCat != null)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Category exists";
                    return responseMessage;
                }

                if (image == null)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Not Saved! Add a category image.";
                    return responseMessage;
                }

                category.ID = Guid.NewGuid();
                category.Image = await ImageService.SaveImageInFolder(image, category.ID.ToString(), "CategoryIcons");
                category.CreatedBy = user.ID;
                category.DateCreated = DateTime.UtcNow.AddHours(1);
                category.StoreID = _genericBusiness.StoreID;
                category.IsActive = true;
                await _unitOfWork.Categories.Create(category);

                if (await _unitOfWork.Commit() > 0)
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Category Saved!";
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Category Not Saved!";
                }
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Category Not Saved!";
            }
            return responseMessage;
        }
        public async Task<ResponseMessage<Category>> Modify(Guid catID,string name, User user, IFormFile image)
        {
            ResponseMessage<Category> responseMessage = new();
            try
            {

                if (string.IsNullOrEmpty(name))
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Not Modified! Add a category name.";
                    return responseMessage;
                }

                Category theCategory = await _unitOfWork.Categories.Find(catID);
                //= await _unitOfWork.Categories.Find(thisCat.ID);

                if (theCategory == null)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Category does not exist";
                    return responseMessage;
                }

                theCategory.DateModified = DateTime.UtcNow.AddHours(1);
                theCategory.ModifiedBy = user.ID;

                if (theCategory.Tag != GenericService.GetTag(name))
                {
                    theCategory.Tag = GenericService.GetTag(name);
                   var thisCat = await _unitOfWork.Categories.GetByCategoryTag(theCategory.Tag);
                    if (thisCat != null)
                    {
                        responseMessage.StatusCode = 201;
                        responseMessage.Message = "Category exists!";
                        return responseMessage;
                    }
                }
                theCategory.Image = image != null ? await ImageService.SaveImageInFolder(image, theCategory.ID.ToString(), "CategoryIcons") : theCategory.Image;
                _unitOfWork.Categories.Update(theCategory);

                if (await _unitOfWork.Commit() > 0)
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Category Modified!";
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Category Not Modified!";
                }
            }
            catch (Exception e)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Category Not Modified!";
                FileService.WriteToFile("\n\n" + e, "ErrorLogs");
            }
            return responseMessage;
        }
        public async Task<ResponseMessage<string>> Delete(string t)
        {
            var responseMessage = new ResponseMessage<string>();
            try
            {
                var cat = await _unitOfWork.Categories.Find(Guid.Parse(t));
                _unitOfWork.Categories.Delete(cat);

                if (await _unitOfWork.Commit() > 0)
                {
                     ImageService.DeleteImage(cat.Image, "CategoryIcons");
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Category Deleted!";
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Category Not Deleted!";
                }
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Category Not Deleted!";
            }

            return responseMessage;
        }
    }
}
