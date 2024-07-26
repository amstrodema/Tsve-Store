using App.Services;
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
    public class GeneralBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CategoryBusiness _categoryBusiness;
        private readonly GenericBusiness _genericBusiness;
        public GeneralBusiness(IUnitOfWork unitOfWork, CategoryBusiness categoryBusiness, GenericBusiness genericBusiness)
        {
            _unitOfWork = unitOfWork;
            _categoryBusiness = categoryBusiness;
            _genericBusiness = genericBusiness;

        }
       
        public IEnumerable<Item> AttachImage(IEnumerable<Item> items)
        {
            //for (int i = 0; i < items.Count(); i++)
            //{
            //    items[i].
            //}
            foreach (var item in items)
            {
                item.Image = string.IsNullOrEmpty(item.Image) ? "" : ImageService.GetLargeImagePath(item.Image, "ItemImage");
                item.Image1 = string.IsNullOrEmpty(item.Image1) ? "" : ImageService.GetLargeImagePath(item.Image1, "ItemImage");
                item.Image2 = string.IsNullOrEmpty(item.Image2) ? "" : ImageService.GetLargeImagePath(item.Image2, "ItemImage");
                item.Image3 = string.IsNullOrEmpty(item.Image3) ? "" : ImageService.GetLargeImagePath(item.Image3, "ItemImage");
                item.Image4 = string.IsNullOrEmpty(item.Image4) ? "" : ImageService.GetLargeImagePath(item.Image4, "ItemImage");
            }

            return items;
        }
        public Item AttachImage(Item item)
        {
            item.Image = string.IsNullOrEmpty(item.Image) ? "" : ImageService.GetLargeImagePath(item.Image, "ItemImage");
            item.Image1 = string.IsNullOrEmpty(item.Image1) ? "" : ImageService.GetLargeImagePath(item.Image1, "ItemImage");
            item.Image2 = string.IsNullOrEmpty(item.Image2) ? "" : ImageService.GetLargeImagePath(item.Image2, "ItemImage");
            item.Image3 = string.IsNullOrEmpty(item.Image3) ? "" : ImageService.GetLargeImagePath(item.Image3, "ItemImage");
            item.Image4 = string.IsNullOrEmpty(item.Image4) ? "" : ImageService.GetLargeImagePath(item.Image4, "ItemImage");

            return item;
        }
    }
}
