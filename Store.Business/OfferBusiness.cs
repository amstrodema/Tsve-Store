using App.Services;
using Microsoft.AspNetCore.Http;
using Store.Data.Interface;
using Store.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Business
{
    public class OfferBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly GenericBusiness _genericBusiness;
        public OfferBusiness(IUnitOfWork unitOfWork, GenericBusiness genericBusiness)
        {
            _unitOfWork = unitOfWork;
            _genericBusiness = genericBusiness;
        }
        public async Task<IEnumerable<Offer>> Get()
        {
            return from offer in await _unitOfWork.Offers.GetAll()
                   where offer.StoreID == _genericBusiness.StoreID
                   select new Offer()
                   {
                       ID = offer.ID,
                       Caption = offer.Caption,
                       StoreID = offer.StoreID,
                       Action = offer.Action,
                       Image = ImageService.GetSmallImagePath(offer.Image, "Offer"),
                       Link = offer.Link,
                       Description = offer.Description,
                       IsCoupon = offer.IsCoupon,
                       Tag = offer.Tag
                       
                   };
        }
        public async Task<ResponseMessage<string>> Create(Offer offer, IFormFile image, Guid userID)
        {
            ResponseMessage<string> Tsve_ResponseMessage = new ResponseMessage<string>();
            try
            {
                offer.ID = Guid.NewGuid();
                offer.Image = await ImageService.SaveImageInFolder(image, offer.ID.ToString(), "Offer");
                offer.StoreID = _genericBusiness.StoreID;
                offer.CreatedBy = userID;
                offer.DateCreated = DateTime.UtcNow.AddHours(1);
                await _unitOfWork.Offers.Create(offer);
                offer.Caption = string.IsNullOrEmpty(offer.Caption) ? string.Empty : offer.Caption;
                offer.Link = string.IsNullOrEmpty(offer.Link) ? string.Empty : offer.Link;
                offer.Action = string.IsNullOrEmpty(offer.Action) ? string.Empty : offer.Action;
                offer.Description = string.IsNullOrEmpty(offer.Description) ? string.Empty : offer.Description;
                offer.IsActive = true;

                if (await _unitOfWork.Commit() > 0)
                {
                    Tsve_ResponseMessage.StatusCode = 200;
                    Tsve_ResponseMessage.Message = "Offer Added";
                }
                else
                {
                    Tsve_ResponseMessage.StatusCode = 201;
                    Tsve_ResponseMessage.Message = "Offer Not Added";
                }
            }
            catch (Exception)
            {
                Tsve_ResponseMessage.StatusCode = 201;
                Tsve_ResponseMessage.Message = "Offer Not Added";
            }

            return Tsve_ResponseMessage;
        }


        public async Task<ResponseMessage<string>> Delete(Guid offerID)
        {
            ResponseMessage<string> Tsve_ResponseMessage = new ResponseMessage<string>();
            try
            {
                var item = await _unitOfWork.Items.GetByOfferID(offerID);
                var offer = await _unitOfWork.Offers.Find(offerID);

                if (item != null)
                {
                    offer.IsActive = false;
                    _unitOfWork.Offers.Update(offer);
                    Tsve_ResponseMessage.Message = "Offer Hidden";
                }
                else
                {
                    _unitOfWork.Offers.Delete(offer);
                    Tsve_ResponseMessage.Message = "Offer Deleted";
                }
                

                if (await _unitOfWork.Commit() > 0)
                {
                    Tsve_ResponseMessage.StatusCode = 200;
                }
                else
                {
                    Tsve_ResponseMessage.StatusCode = 201;
                    Tsve_ResponseMessage.Message = "Offer Not Deleted";
                }
            }
            catch (Exception)
            {
                Tsve_ResponseMessage.StatusCode = 201;
                Tsve_ResponseMessage.Message = "Offer Not Deleted";
            }
            return Tsve_ResponseMessage;
        }

        public async Task<ResponseMessage<string>> ToggleHome(Guid offerID)
        {
            ResponseMessage<string> Tsve_ResponseMessage = new ResponseMessage<string>();
            try
            {
                var offer = await _unitOfWork.Offers.Find(offerID);

                offer.IsHomepage = !offer.IsHomepage;
                _unitOfWork.Offers.Update(offer);


                if (await _unitOfWork.Commit() > 0)
                {
                    Tsve_ResponseMessage.StatusCode = 200;
                    Tsve_ResponseMessage.Message = "Offer Modified";
                }
                else
                {
                    Tsve_ResponseMessage.StatusCode = 201;
                    Tsve_ResponseMessage.Message = "Offer Not Modified";
                }
            }
            catch (Exception)
            {
                Tsve_ResponseMessage.StatusCode = 201;
                Tsve_ResponseMessage.Message = "Offer Not Modified!";
            }
            return Tsve_ResponseMessage;
        }
    }
}
