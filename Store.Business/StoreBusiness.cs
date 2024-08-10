﻿using App.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
    public class StoreBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly GenericBusiness _genericBusiness;
        private readonly GeneralBusiness _generalBusiness;
        private readonly CategoryBusiness _categoryBusiness;
        public static bool IsLive = true;
        public readonly static string secretKey = IsLive ? "sk_live_41810b5f8f5a1bbec3a7768e9d8df755d447b711" : "sk_test_a816f19ac9aa7cfe587048ff252bba9c6da56ebe";
        public readonly static string publicKey = IsLive ? "pk_live_f54d8d97e29f4919fc08ed54757279219fff4450" : "pk_test_6d6956d2424d5eab1ae4b31c2c55eda167913679";
        public readonly static string Site = IsLive ? "https://customseam-001-site1.jtempurl.com/" : "https://localhost:7029/";

        public StoreBusiness(IUnitOfWork unitOfWork, GenericBusiness genericBusiness, GeneralBusiness generalBusiness, CategoryBusiness categoryBusiness)
        {
            _unitOfWork = unitOfWork;
            _genericBusiness = genericBusiness;
            _generalBusiness = generalBusiness;
            _categoryBusiness = categoryBusiness;            
        }
        public async Task<IEnumerable<Item>> Get() => await _unitOfWork.Items.GetByStoreID(_genericBusiness.StoreID);
        public async Task<Model.Store> GetStore() => await _unitOfWork.Stores.Find(_genericBusiness.StoreID);
        public async Task<ResponseMessage<string>> Renewal()
        {
            ResponseMessage<string> responseMessage = new ResponseMessage<string>();
            try
            {
                var store = await GetStore();
                store.ExpiryDate = DateTime.Now.AddDays(365);
                if (await _unitOfWork.Commit() > 0)
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Store renewed!";
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Store not renewed!";
                }

            }
            catch (Exception e)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Store not renewed!";
                FileService.WriteToFile("\n\n" + e, "ErrorLogs");
            }

            return responseMessage;
        }
        public async Task<ResponseMessage<string>> UpdateSettings(Model.Store store, IFormFile logo)
        {
            ResponseMessage<string> responseMessage = new ResponseMessage<string>();
            try
            {
                store.ID = _genericBusiness.StoreID;
                store.DateModified = DateTime.UtcNow.AddHours(1);
                if (logo != null)
                {
                    store.LogoImage = await ImageService.SaveImageInFolder(logo, store.ID.ToString(), "Logo");
                }
                _unitOfWork.Stores.Update(store);

                if (await _unitOfWork.Commit() > 0)
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Settings Saved!";
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "No Settings Saved!";
                }

            }
            catch (Exception e)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Settings Not Saved!";
                FileService.WriteToFile("\n\n" + e, "ErrorLogs");
            }

            return responseMessage;
        }
        public async Task<MainVM> GetVM()
        {
            MainVM mainVM = new MainVM();
            mainVM.Stocks = await _unitOfWork.Items.GetByStoreID(_genericBusiness.StoreID);
            mainVM.Categories = await _unitOfWork.Categories.GetAll();
            //mainVM.Categories = await _unitOfWork.Categories.GetByStoreID(_genericBusiness.StoreID);
            return mainVM;
        }
        public async Task<MainVM> GetVM(string t)
        {
            Item item = new Item();
            try
            {
                item = await _unitOfWork.Items.Find(Guid.Parse(t));
            }
            catch (Exception)
            {
                item = await _unitOfWork.Items.GetByTag(t);
            }
            MainVM mainVM = new MainVM();
            mainVM.Stock = _generalBusiness.AttachImage(item);
            mainVM.Categories = await _unitOfWork.Categories.GetAll();
            return mainVM;
        }

        public async Task<ResponseMessage<string>> Create(ItemVM itemVM, User user, IFormFile img, IFormFile img1, IFormFile img2, IFormFile img3, IFormFile img4)
        {
            ResponseMessage<string> responseMessage = new ResponseMessage<string>();
            try
            {
                var items = await _unitOfWork.Items.GetByStoreID(_genericBusiness.StoreID);
                var store = await _unitOfWork.Stores.Find(_genericBusiness.StoreID);

                if (img == null || img1 == null)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Upload image 1 and 2";
                    return responseMessage;
                }

                var cat = await _unitOfWork.Categories.Find(itemVM.Category);

                if (cat == null) { responseMessage.StatusCode = 201; return responseMessage; }
                Guid thisID;

                Item item = new Item()
                {
                    Brief = itemVM.Brief,
                    Currency = itemVM.Currency,
                    Description = itemVM.Desc,
                    OldPrice = itemVM.OldPrice,
                    Price = itemVM.Price,
                    Title = itemVM.Title,
                    Tag = GenericService.GetTag(itemVM.Title),
                    StoreID = _genericBusiness.StoreID,
                    ID = thisID = Guid.NewGuid(),
                    CatID = itemVM.Category,
                    CreatedBy = user.ID,
                    CurrencySymbol = itemVM.Currency == "NGN" ? "₦" : "$",
                    IsActive = true,
                    GroupID = cat.GroupID,
                    DateCreated = DateTime.UtcNow.AddHours(1),
                    IsApproved = true,
                    Image = img != null ? await ImageService.SaveImageInFolder(img, thisID.ToString(), "ItemImage") : "",
                    Image1 = img1 != null ? await ImageService.SaveImageInFolder(img1, thisID.ToString() + "1", "ItemImage") : "",
                    Image2 = img2 != null ? await ImageService.SaveImageInFolder(img2, thisID.ToString() + "2", "ItemImage") : "",
                    Image3 = img3 != null ? await ImageService.SaveImageInFolder(img3, thisID.ToString() + "3", "ItemImage") : "",
                    Image4 = img4 != null ? await ImageService.SaveImageInFolder(img4, thisID.ToString() + "4", "ItemImage") : "",
                    IsRecent = true
                };
                var thisItem = await _unitOfWork.Items.GetByTag(item.Tag);
                if (thisItem != null)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Item Exists already!";
                    return responseMessage;
                }
                await _unitOfWork.Items.Create(item);

                if (itemVM.Features != null)
                    foreach (var featurePicker in itemVM.Features)
                    {
                        await _unitOfWork.Features.Create(new Feature()
                        {
                            Name = featurePicker.Name,
                            StoreID = _genericBusiness.StoreID,
                            ItemID = item.ID,
                            ID = Guid.NewGuid(),
                            CreatedBy = user.ID,
                            DateCreated = DateTime.UtcNow.AddHours(1),
                            IsActive = true,
                            IsApproved = true,
                            Options = featurePicker.Option
                        }
                            );

                    }

                if (await _unitOfWork.Commit() > 0)
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Item Saved!";
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Item Not Saved!";
                }

            }
            catch (Exception e)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Item Not Saved!";
                FileService.WriteToFile("\n\n" + e, "ErrorLogs");
            }
            return responseMessage;
        }
        public async Task<MainVM> GetFromCategory(string t)
        {
            MainVM mainVM = new MainVM();
            try
            {
                var cat = await _unitOfWork.Categories.GetByCategoryTag(t);
                mainVM.Category = cat;
                mainVM.Stocks = from item in await _unitOfWork.Items.GetByStoreID(_genericBusiness.StoreID)
                                where item.CatID == cat.ID && item.IsActive && item.Currency == GenericBusiness.ShoppingCurrency
                                join review in await _unitOfWork.Reviews.GetAll() on item.ID equals review.ItemID into reviews
                                select new Item()
                                {
                                    CatID = item.CatID,
                                    Tag = item.Tag,
                                    Image = ImageService.GetLargeImagePath(item.Image, "ItemImage"),
                                    Currency = item.Currency,
                                    CurrencySymbol = item.CurrencySymbol,
                                    Title = item.Title,
                                    OldPrice = item.OldPrice,
                                    Price = item.Price,
                                    ID = item.ID,
                                    Rating = reviews.Sum(p => p.Rating) / (reviews.Count() < 10 ? 10 : reviews.Count()),
                                    IsRecent = item.IsRecent,
                                    IsFeatured = item.IsFeatured,
                                    Reviews = reviews.Count()
                                };
                mainVM.CategoryHybrids = await _categoryBusiness.GetHybrids();
            }
            catch (Exception)
            {
                throw;
            }

            return mainVM;
        }
        public async Task<ResponseMessage<string>> Modify(ItemVM itemVM, User user, IFormFile img, IFormFile img1, IFormFile img2, IFormFile img3, IFormFile img4)
        {
            ResponseMessage<string> responseMessage = new ResponseMessage<string>();
            try
            {
                var cat = await _unitOfWork.Categories.Find(itemVM.Category);
                var item = await _unitOfWork.Items.Find(itemVM.ID);
                var itemOld = await _unitOfWork.Items.Find(itemVM.ID);

                if (cat == null) { responseMessage.StatusCode = 201; return responseMessage; }
                if (item == null) { responseMessage.StatusCode = 201; return responseMessage; }

                itemVM.Img2 = item.Image1 ?? "";
                itemVM.Img3 = item.Image2 ?? "";
                itemVM.Img4 = item.Image3 ?? "";
                itemVM.Img5 = item.Image4 ?? "";

                item.Brief = itemVM.Brief;
                item.Currency = itemVM.Currency;
                item.Description = itemVM.Desc;
                item.OldPrice = itemVM.OldPrice;
                item.Price = itemVM.Price;
                item.Title = itemVM.Title;
                item.StoreID = _genericBusiness.StoreID;
                item.CatID = itemVM.Category;
                item.ModifiedBy = user.ID;
                item.CurrencySymbol = itemVM.Currency == "NGN" ? "₦" : "$";
                item.GroupID = cat.GroupID;
                item.DateModified = DateTime.UtcNow.AddHours(1);
                item.Image = img != null ? await ImageService.SaveImageInFolder(img, item.ID.ToString(), "ItemImage") : item.Image;
                item.Image1 = img1 != null ? await ImageService.SaveImageInFolder(img1, item.ID.ToString() + "1", "ItemImage") : itemVM.Img2;
                item.Image2 = img2 != null ? await ImageService.SaveImageInFolder(img2, item.ID.ToString() + "2", "ItemImage") : itemVM.Img3;
                item.Image3 = img3 != null ? await ImageService.SaveImageInFolder(img3, item.ID.ToString() + "3", "ItemImage") : itemVM.Img4;
                item.Image4 = img4 != null ? await ImageService.SaveImageInFolder(img4, item.ID.ToString() + "4", "ItemImage") : itemVM.Img5;

                if (GenericService.GetTag(itemVM.Title) != item.Tag)
                {
                    item.Tag = GenericService.GetTag(itemVM.Title);

                    var thisItem = await _unitOfWork.Items.GetByTag(item.Tag);
                    if (thisItem != null)
                    {
                        responseMessage.StatusCode = 201;
                        responseMessage.Message = "Item Exists already!";
                        return responseMessage;
                    }
                }


                _unitOfWork.Items.Update(item);

                var features = await _unitOfWork.Features.GetByItemID(item.ID);
                foreach (var feature in features)
                {
                    _unitOfWork.Features.Delete(feature);
                }

                if (itemVM.Features != null)
                    foreach (var featurePicker in itemVM.Features)
                    {
                        await _unitOfWork.Features.Create(new Feature()
                        {
                            Name = featurePicker.Name,
                            StoreID = _genericBusiness.StoreID,
                            ItemID = item.ID,
                            ID = Guid.NewGuid(),
                            CreatedBy = user.ID,
                            DateCreated = DateTime.UtcNow.AddHours(1),
                            IsActive = true,
                            IsApproved = true,
                            Options = featurePicker.Option
                        }
                            );

                    }

                if (await _unitOfWork.Commit() > 0)
                {
                    if (string.IsNullOrEmpty(item.Image1))
                    {
                        ImageService.DeleteImage(itemOld.Image1, "ItemImage");
                    }
                    if (string.IsNullOrEmpty(item.Image2))
                    {
                        ImageService.DeleteImage(itemOld.Image2, "ItemImage");
                    }
                    if (string.IsNullOrEmpty(item.Image3))
                    {
                        ImageService.DeleteImage(itemOld.Image3, "ItemImage");
                    }
                    if (string.IsNullOrEmpty(item.Image4))
                    {
                         ImageService.DeleteImage(itemOld.Image4, "ItemImage");
                    }

                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Item Modifed!";
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Item Not Modified!";
                }

            }
            catch (Exception e)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Item Not Modified!";
                FileService.WriteToFile("\n\n" + e, "ErrorLogs");
            }
            return responseMessage;
        }

        public async Task<MainVM> GetItem(string t)
        {
            
            MainVM mainVM = new MainVM(); 
            mainVM.Stocks = _generalBusiness.AttachImage((await _unitOfWork.Items.GetAll()).Take(5));
            var item= mainVM.Stocks.FirstOrDefault(p=> p.Tag == t);
            mainVM.Stock = item ?? _generalBusiness.AttachImage(await _unitOfWork.Items.GetByTag(t));
           
            mainVM.Features = await _unitOfWork.Features.GetByItemID(item.ID);
            mainVM.Reviews =(from review in await _unitOfWork.Reviews.GetByItemID(item.ID)
                            join user in await _unitOfWork.Users.GetAll() on review.UserID equals user.ID into users
                            from thisUser in users.DefaultIfEmpty()
                            select new Review()
                            {
                                ID = review.ID,
                                Name = review.UserID != default ? thisUser.Username : review.Name,
                                Message = review.Message,
                                DateCreated = review.DateCreated,
                                Rating = review.Rating
                            }).OrderByDescending(o=> o.DateCreated);
            var traffic = mainVM.Reviews.Count() < 10 ? 10 : mainVM.Reviews.Count();
            mainVM.Ratings = mainVM.Reviews.Sum(p => p.Rating) / traffic;
            return mainVM;
        }
        public async Task<CheckOutVM> GetCart(OrderVM[] ? orderItems)
        {
            CheckOutVM checkOutVM = new CheckOutVM();
            if (orderItems == null)
            {
                return checkOutVM;
            }
           var orders = from orderItem in orderItems
                      join item in await _unitOfWork.Items.GetAll() on orderItem.ID equals item.ID
                      select new OrderVM()
                      {
                          ID = orderItem.ID,
                          Image = ImageService.GetSmallImagePath(item.Image, "ItemImage"),
                          ItemName = item.Title,
                          Qty = orderItem.Qty,
                          Features = orderItem.Features,
                          Price = item.Price,
                          CurrenySymbol = item.CurrencySymbol,
                          Tag = item.Tag
                      };
            checkOutVM.Orders = orders;
            return checkOutVM;
        }
        public async Task<ResponseMessage<MainVM>> Order(string Ref) {
            ResponseMessage<MainVM> responseMessage = new ResponseMessage<MainVM>();
            MainVM mainVM = new MainVM();
            responseMessage.Data = mainVM;
            try
            {
                var order = await _unitOfWork.Orders.GetByOrderRef( Ref);
                mainVM.OrderVMs = from orderItem in await _unitOfWork.OrderItems.GetByOrderID(order.ID)
                                  join item in await _unitOfWork.Items.GetAll() on orderItem.ItemID equals item.ID
                                  join feature in await _unitOfWork.ItemFeatures.GetByOrderID(order.ID) on orderItem.ID equals feature.OrderItemID into features
                                  from feat in features.DefaultIfEmpty()
                                  select new OrderVM()
                                  {
                                      ID = orderItem.ID,
                                      Image = ImageService.GetSmallImagePath(item.Image, "ItemImage"),
                                      ItemName = item.Title,
                                      Qty = orderItem.Qty,
                                      Features = features.ToArray(),
                                      Price = item.Price,
                                      CurrenySymbol = item.CurrencySymbol,
                                      Tag = item.Tag
                                  };

                mainVM.ShippingDetail = await _unitOfWork.ShippingDetails.GetByOrder(order.ID);
                mainVM.Order = order;
                responseMessage.StatusCode = 200;
            }
            catch (Exception)
            {
                responseMessage.Data = new MainVM();
            }
            
            return responseMessage;
        }
        public async Task<CheckOutVM> Orders() {
            CheckOutVM checkOutVM = new CheckOutVM();
            checkOutVM.Orders = (from order in await _unitOfWork.Orders.GetAll()
                            join shipping in await _unitOfWork.ShippingDetails.GetAll() on order.ID equals shipping.OrderID
                            select new OrderVM()
                            {
                                ID = order.ID,
                                Tel = shipping.Tel== null ? "" : shipping.Tel,
                                Mail = order.Email== null ? "" : order.Email,
                                Ref = order.Ref,
                                DateCreated = order.DateCreated,
                                IsFinished = order.IsFinished,
                                IsPaid = order.IsPaid
                            }).OrderByDescending(p=> p.DateCreated);
            
            return checkOutVM;
        }

        public async Task<ResponseMessage<CheckOutVM>> AddToGallery(IFormFile resource, Guid userID)
        {
            ResponseMessage<CheckOutVM> responseMessage = new ResponseMessage<CheckOutVM>();
            if(resource == null)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "No media found!";
                return responseMessage;
            }
            try {
                Model.File file = new Model.File()
                {
                    ID = Guid.NewGuid(),
                    CreatedBy = userID,
                    DateCreated = DateTime.UtcNow.AddHours(1),
                    IsActive = true
                };
                try
                {
                    file.Resource = await VideoServices.SaveVideoFile(resource, "Gallery", file.ID.ToString());
                    file.IsVideo = true;
                }
                catch
                {
                    try
                    {
                        file.Resource = await ImageService.SaveImageInFolder(resource, file.ID.ToString(), "Gallery", false);
                        file.IsVideo = false;
                    }
                    catch
                    {
                        responseMessage.StatusCode = 201;
                        responseMessage.Message = "Invalid media format!";
                        return responseMessage;
                    }
                }
                
                await _unitOfWork.Files.Create(file);

                if(await _unitOfWork.Commit() > 0)
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Media saved!";
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Media not saved!";
                }


            }
            catch {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Media not saved!";
            }

            responseMessage.Data = new CheckOutVM()
            {
                Files = await _unitOfWork.Files.GetAll()
            };

            return responseMessage;
        }

        public async Task<CheckOutVM> Gallery()
        {
            CheckOutVM responseMessage = new CheckOutVM();
            responseMessage.Files = new List<Model.File>();
            try {
                responseMessage = new CheckOutVM()
                {
                    Files = from file in await _unitOfWork.Files.GetAll()
                            select new Model.File()
                            {
                                ID = file.ID,
                                Resource = file.IsVideo ? VideoServices.GetVideoFromFolder(file.Resource, "Gallery") : ImageService.GetLargeImagePath(file.Resource, "Gallery"),
                                IsVideo = file.IsVideo,
                                DateCreated = file.DateCreated
                            }
                };
            }
            catch { }
            return responseMessage;
        }
    
        public async Task<ResponseMessage<CheckOutVM>> ConfirmOrder(string orderRef)
        {
            ResponseMessage<CheckOutVM> responseMessage = new ResponseMessage<CheckOutVM>();
            try
            {
                var order = await _unitOfWork.Orders.GetByOrderRef(orderRef);
                responseMessage.Data = new CheckOutVM()
                {
                    ShippingDetail = await _unitOfWork.ShippingDetails.GetByOrder(order.ID),
                    Order = order,
                    Orders = from orderItem in await _unitOfWork.OrderItems.GetByOrderID(order.ID)
                             join item in await _unitOfWork.Items.GetAll() on orderItem.ItemID equals item.ID
                             join feature in await _unitOfWork.ItemFeatures.GetByOrderID(order.ID) on orderItem.ID equals feature.OrderItemID into features
                             from feat in features.DefaultIfEmpty()
                             select new OrderVM()
                             {
                                 ID = orderItem.ID,
                                 Image = ImageService.GetSmallImagePath(item.Image, "ItemImage"),
                                 ItemName = item.Title,
                                 Qty = orderItem.Qty,
                                 Features = features.ToArray(),
                                 Price = item.Price,
                                 CurrenySymbol = item.CurrencySymbol,
                                 Tag = item.Tag
                             }
            };
            }
            catch { throw; }
            return responseMessage;
        }
        public async Task<ResponseMessage<string>> CheckOutCart(CheckOutVM checkOutVM, User? user)
        {
            ResponseMessage<string> responseMessage = new ResponseMessage<string>();
            try
            {
                // var store = await _unitOfWork.Stores.Find(_genericBusiness.StoreID);
                //checkOutVM.BillingDetail.StoreID = _genericBusiness.StoreID;
                //checkOutVM.BillingDetail.ID = Guid.NewGuid();
                //checkOutVM.BillingDetail.IsActive = true;
                //checkOutVM.BillingDetail.Addr2 = string.IsNullOrEmpty(checkOutVM.BillingDetail.Addr2) ? string.Empty : checkOutVM.BillingDetail.Addr2;
                //checkOutVM.BillingDetail.DateCreated = DateTime.UtcNow.AddHours(1);
                //await _unitOfWork.BillingDetails.Create(checkOutVM.BillingDetail);

                if (checkOutVM.Mail == null || checkOutVM.Tel == null)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Email not provided!";
                    return responseMessage;
                }

                Order order = new Order()
                {
                    ID = Guid.NewGuid(),
                    DateCreated = DateTime.UtcNow.AddHours(1),
                    Ref = "TSVE" + GenService.Gen10DigitNumericCode()+"X-NY",
                    IsActive = true,
                    IsApproved = false,
                    Email = user == null ? checkOutVM.Mail : user.Email,
                    Tel = user == null ? checkOutVM.Tel : user.Tel
                };

                var orderItems = from oder in checkOutVM.Orders
                                 join item in await _unitOfWork.Items.GetAll() on oder.ID equals item.ID
                                 select new OrderItem()
                                 {
                                     ID = Guid.NewGuid(),
                                     CurrenySymbol = item.CurrencySymbol,
                                     DateCreated = DateTime.UtcNow.AddHours(1),
                                     Image = item.Image,
                                     ItemName = item.Title,
                                     ItemID = oder.ID,
                                     IsActive = true,
                                     OrderID = order.ID,
                                     Price = item.Price,
                                     Qty = oder.Qty,
                                     Tag = item.Tag
                                 };

                order.Amount = orderItems.Sum(p => p.Qty * p.Price);


                if (order.Amount <= 0)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Invalid Purchase Total";
                    return responseMessage;
                }

                while (await _unitOfWork.Orders.GetByOrderRef(order.Ref) != null)
                {
                    order.Ref = "TSVE" + GenService.Gen10DigitNumericCode() + "X-NY";
                }
                
                var resp = await PaystackService.InitializePaymentAsync(secretKey, order.Email, order.Amount * 100, order.ID.ToString(), "Store", $"{Site}confirmation?order={order.Ref}");
                order.PaymentLink = resp.Data.AuthorizationUrl;

                await _unitOfWork.Orders.Create(order);

                checkOutVM.ShippingDetail.ID = Guid.NewGuid();
                checkOutVM.ShippingDetail.OrderID = order.ID;
                checkOutVM.ShippingDetail.IsActive = true;
                checkOutVM.ShippingDetail.DateCreated = DateTime.UtcNow.AddHours(1);

                await _unitOfWork.ShippingDetails.Create(checkOutVM.ShippingDetail);
                await _unitOfWork.OrderItems.CreateMultiple(orderItems.ToArray());

                foreach (var oder in checkOutVM.Orders)
                {
                    if (oder.Features != null)
                        foreach (var item in oder.Features)
                        {
                            item.OrderID = order.ID;
                            item.StoreID = _genericBusiness.StoreID;
                            item.OrderItemID = orderItems.FirstOrDefault(p => p.ItemID == oder.ID).ID;
                            item.ID = Guid.NewGuid();
                            await _unitOfWork.ItemFeatures.Create(item);
                        }
                }

                if (checkOutVM.IsCreateAccount && user == null)
                {
                    user = new User()
                    {
                        ID = Guid.NewGuid(),
                        Email = checkOutVM.Mail ?? "",
                        EmailVerCode = GenService.Gen10DigitCode(),
                        EmailVerExp = DateTime.UtcNow.AddMinutes(10),
                        StoreID = _genericBusiness.StoreID,
                        DateCreated = DateTime.UtcNow.AddHours(1),
                        IsActive = true,
                        Username = "Cust-" + GenService.Gen10DigitNumericCode(),
                        Tel = checkOutVM.Tel ?? "",
                        Password = EncryptionService.Encrypt(GenService.Gen10DigitCode()),
                    };
                    await _unitOfWork.Users.Create(user);
                }

                if (await _unitOfWork.Commit() > 0)
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Order Logged!";
                    responseMessage.Data = order.Ref;
                //    try
                //    {
                //        Email email = new Email()
                //        {
                //            DisplayName = $"{store.Name}",
                //            Message = $"<h2>ORDER ALERT</h2><br><b>You have a new order.</b> <br>Click the link to view: <a href='{store.Url}/manager/orders?ref={order.Ref}'>{store.Url}/manager/orders?ref={order.Ref}</a>",
                //            Subject = "New Order",
                //            Recipients = new List<string>()
                //{
                //                store.Email == null ? string.Empty : store.Email
                //}
                //        };

                //        EmailService.SendMail(email, "salmatraglobal@gmail.com", "Salm1!0#009");
                //        EmailService.FastMail("", $"New Order Alert - {order.Ref}", $"You have a new order. Click to view: {store.Url}/manager/orders?ref={order.Ref}");
                //    }
                //    catch (Exception)
                //    {

                //    }
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Not Completed.";
                }
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Not Completed!";
            }
            return responseMessage;
        }
        public async Task<int> ToggleFave(Guid itemID, Guid userID)
        {
            try
            {
                var fave = await _unitOfWork.Favourites.GetByUserAndItemID(itemID, userID);
                if (fave != null)
                    _unitOfWork.Favourites.Delete(fave);
                else
                    await _unitOfWork.Favourites.Create(new Favourite()
                    {
                        ID = Guid.NewGuid(),
                        DateCreated = DateTime.UtcNow.AddHours(1),
                        StoreID = _genericBusiness.StoreID,
                        ItemID = itemID,
                        CreatedBy = userID,
                        UserID = userID
                    });

                if (await _unitOfWork.Commit() > 0)
                {
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
            }
            return 0;
        }

        public async Task<MainVM> GetVMForFave(Guid[] ? faves, User? user)
        {
            MainVM mainVM = new MainVM();

            try
            {
                if (faves == null && user == null)
                {
                    throw new Exception();
                }

                if(user != null)
                {
                    faves = (from fav in await _unitOfWork.Favourites.GetByUserID(user.ID) 
                              select fav.ID).ToArray();
                }

                mainVM.Favourite = (from fave in faves
                                   join item in await _unitOfWork.Items.GetByStoreIDAndCurrency(GenericBusiness.ShoppingCurrency) on fave equals item.ID
                                   join review in await _unitOfWork.Reviews.GetAll() on item.ID equals review.ItemID into reviews
                                   select new Item()
                                   {
                                       CatID = item.CatID,
                                       Tag = item.Tag,
                                       Image = ImageService.GetLargeImagePath(item.Image, "ItemImage"),
                                       Image1 = ImageService.GetLargeImagePath(item.Image1, "ItemImage"),
                                       Currency = item.Currency,
                                       CurrencySymbol = item.CurrencySymbol,
                                       Title = item.Title,
                                       OldPrice = item.OldPrice,
                                       Price = item.Price,
                                       ID = item.ID,
                                       Rating = reviews.Sum(p => p.Rating) / (reviews.Count() < 10 ? 10 : reviews.Count()),
                                       IsRecent = item.IsRecent,
                                       IsFeatured = item.IsFeatured,
                                       Reviews = reviews.Count()
                                   }).OrderByDescending(o=> o.DateCreated);

              //  mainVM.Favourite = _generalBusiness.AttachImage(mainVM.Favourite);
                mainVM.Faves = JsonConvert.SerializeObject(faves);
            }
            catch (Exception)
            {
                throw;
            }

            return mainVM;
        }
        public async Task<Item> GetQuickView(string tag) {
            var item = await _unitOfWork.Items.GetByTag(tag);
            var reviews = await _unitOfWork.Reviews.GetByItemID(item.ID);
        return new Item()
        {
            CatID = item.CatID,
            Tag = item.Tag,
            Image = ImageService.GetLargeImagePath(item.Image, "ItemImage"),
            Currency = item.Currency,
            CurrencySymbol = item.CurrencySymbol,
            Title = item.Title,
            OldPrice = item.OldPrice,
            Price = item.Price,
            ID = item.ID,
            Rating = reviews.Sum(p => p.Rating) / (reviews.Count() < 10 ? 10 : reviews.Count()),
            IsRecent = item.IsRecent,
            IsFeatured = item.IsFeatured,
            Reviews = reviews.Count(),
            Image1 = string.IsNullOrWhiteSpace(item.Image1) ? "" : ImageService.GetLargeImagePath(item.Image1, "ItemImage"),
            Brief = item.Brief
        };
        }
        public async Task<MainVM> GetVMForHome()
        {
            MainVM mainVM = new MainVM();
            mainVM.CategoryHybrids = await _categoryBusiness.GetHybrids();
            mainVM.Featured = from item in await _unitOfWork.Items.GetAll()
                              where item.Currency == GenericBusiness.ShoppingCurrency
                              join review in await _unitOfWork.Reviews.GetAll() on item.ID equals review.ItemID into reviews
                              select new Item()
                              {
                                  CatID = item.CatID,
                                  Tag = item.Tag,
                                  Image = ImageService.GetLargeImagePath(item.Image, "ItemImage"),
                                  Currency = item.Currency,
                                  CurrencySymbol = item.CurrencySymbol,
                                  Title = item.Title,
                                  OldPrice = item.OldPrice,
                                  Price = item.Price,
                                  ID = item.ID,
                                  Rating = reviews.Sum(p => p.Rating) / (reviews.Count() < 10 ? 10 : reviews.Count()),
                                  IsRecent = item.IsRecent,
                                  IsFeatured = item.IsFeatured,
                                  Reviews = reviews.Count(),
                                  Image1 = string.IsNullOrWhiteSpace(item.Image1) ? "" : ImageService.GetLargeImagePath(item.Image1, "ItemImage"),
                                  Image2 = string.IsNullOrWhiteSpace(item.Image2) ? "" : ImageService.GetLargeImagePath(item.Image2, "ItemImage"),
                                  Image3 = string.IsNullOrWhiteSpace(item.Image3) ? "" : ImageService.GetLargeImagePath(item.Image3, "ItemImage"),
                                  Image4 = string.IsNullOrWhiteSpace(item.Image4) ? "" : ImageService.GetLargeImagePath(item.Image4, "ItemImage"),
                              };

            //mainVM.Latest = from item in await _unitOfWork.Items.GetLatest()
            //                where item.Currency == GenericBusiness.ShoppingCurrency
            //                join review in await _unitOfWork.Reviews.GetAll() on item.ID equals review.ItemID into reviews
            //                select new Item()
            //                {
            //                    CatID = item.CatID,
            //                    Tag = item.Tag,
            //                    Image = ImageService.GetLargeImagePath(item.Image, "ItemImage"),
            //                    Currency = item.Currency,
            //                    CurrencySymbol = item.CurrencySymbol,
            //                    Title = item.Title,
            //                    OldPrice = item.OldPrice,
            //                    Price = item.Price,
            //                    ID = item.ID,
            //                    Rating = reviews.Sum(p => p.Rating) / (reviews.Count() < 10 ? 10 : reviews.Count()),
            //                    IsRecent = item.IsRecent,
            //                    IsFeatured = item.IsFeatured,
            //                    Reviews = reviews.Count()
            //                };

            //mainVM.Slides = from slide in await _unitOfWork.Slides.GetAll()
            //                where slide.StoreID == _genericBusiness.StoreID
            //                select new Slide()
            //                {
            //                    ID = slide.ID,
            //                    Caption = slide.Caption,
            //                    StoreID = slide.StoreID,
            //                    Action = slide.Action,
            //                    Desc = slide.Desc,
            //                    Image = ImageService.GetLargeImagePath(slide.Image, "Slide"),
            //                    Link = slide.Link
            //                };

            //mainVM.Brands = from brand in await _unitOfWork.Brands.GetAll()
            //                where brand.StoreID == _genericBusiness.StoreID
            //                select new Brand()
            //                {
            //                    ID = brand.ID,
            //                    Tag = brand.Tag,
            //                    Logo = ImageService.GetLargeImagePath(brand.Logo,"Brand")
            //                };
            //var offers = await _unitOfWork.Offers.GetAll();
            //mainVM.Offers = (from offer in offers
            //                 where offer.StoreID == _genericBusiness.StoreID && offer.IsActive && offer.IsHomepage && !offer.IsAdmin
            //                 select new Offer()
            //                 {
            //                     ID = offer.ID,
            //                     Caption = offer.Caption,
            //                     DiscountCaption = offer.DiscountCaption,
            //                     StoreID = offer.StoreID,
            //                     Action = offer.Action,
            //                     Description = offer.Description,
            //                     Tag = offer.Tag,
            //                     Image = ImageService.GetLargeImagePath(offer.Image, "Offer"),
            //                     Link = offer.Link,
            //                     DateCreated = offer.DateCreated
            //                 }).Take(3).ToList();

            //mainVM.Offer = offers.FirstOrDefault(p => p.IsActive && p.IsAdmin);

            return mainVM;
        }
        public async Task<MainVM> GetVMForShop(string c = "", int? pageNumber = 1)
        {
            MainVM mainVM = new MainVM();
            mainVM.CategoryHybrids = await _categoryBusiness.GetHybrids();
            var stocks = await _unitOfWork.Items.GetAll();

            if (!string.IsNullOrEmpty(c))
            {
                var cat = await _unitOfWork.Categories.GetByCategoryTag(c);
                if(cat != null)
                {
                    stocks = stocks.Where(o => o.CatID == cat.ID).ToList();
                    mainVM.Category = cat;
                    mainVM.CTag = cat.Tag;
                }
                
            }

            mainVM.Stocks = (from item in stocks
                              where item.Currency == GenericBusiness.ShoppingCurrency
                              join review in await _unitOfWork.Reviews.GetAll() on item.ID equals review.ItemID into reviews
                              select new Item()
                              {
                                  CatID = item.CatID,
                                  Tag = item.Tag,
                                  Image = ImageService.GetLargeImagePath(item.Image, "ItemImage"),
                                  Currency = item.Currency,
                                  CurrencySymbol = item.CurrencySymbol,
                                  Title = item.Title,
                                  OldPrice = item.OldPrice,
                                  Price = item.Price,
                                  ID = item.ID,
                                  Rating = reviews.Sum(p => p.Rating) / (reviews.Count() < 10 ? 10 : reviews.Count()),
                                  IsRecent = item.IsRecent,
                                  IsFeatured = item.IsFeatured,
                                  Reviews = reviews.Count(),
                                  DateCreated = item.DateCreated,
                                  Image1 = string.IsNullOrWhiteSpace(item.Image1) ? "" : ImageService.GetLargeImagePath(item.Image1, "ItemImage"),
                                  Brief = item.Brief
                              }).OrderByDescending(o=> o.DateCreated);
            // mainVM.Features = await _unitOfWork.Features.GetAll();
            mainVM.ItemCount = mainVM.Stocks.Count();
            var val = PaginatedList<Item>.Create(mainVM.Stocks.ToList(), pageNumber ?? 1, 20);
            mainVM.Stocks = val.Items;
            mainVM.TotalPages = val.TotalPages;


            mainVM.PageIndex = pageNumber ?? 1;
            

            return mainVM;
        }

        public async Task<MainVM> AddReview(Review review, Guid userID)
        {
            MainVM mainVM = new MainVM();
            try
            {
                review.ID = Guid.NewGuid();
                review.CreatedBy = userID;
                review.UserID = userID;
                review.StoreID = _genericBusiness.StoreID;
                review.DateCreated = DateTime.UtcNow.AddHours(1);
                review.Email = string.IsNullOrEmpty(review.Email) ? string.Empty : review.Email;
                review.Name = string.IsNullOrEmpty(review.Name) ? string.Empty : review.Name;

                var oldReview = await _unitOfWork.Reviews.GetByItemIDAndUserID(review.ItemID, userID);

                if (userID == default)
                {
                    oldReview = await _unitOfWork.Reviews.GetByItemIDAndUserEmail(review.ItemID, review.Email);
                }
                else
                {
                    var user = await _unitOfWork.Users.Find(userID);
                    review.Name = user.Fname;
                }

                if (oldReview != null)
                {
                    _unitOfWork.Reviews.Delete(oldReview);
                }

                await _unitOfWork.Reviews.Create(review);
                await _unitOfWork.Commit();

            }
            catch (Exception)
            {
            }

            mainVM.Reviews = (from thisReview in await _unitOfWork.Reviews.GetByItemID(review.ItemID)
                             join user in await _unitOfWork.Users.GetAll() on thisReview.UserID equals user.ID into users
                             from thisUser in users.DefaultIfEmpty()
                             select new Review()
                             {
                                 ID = thisReview.ID,
                                 Name = thisReview.UserID != default ? thisUser.Username : thisReview.Name,
                                 Message = thisReview.Message,
                                 DateCreated = thisReview.DateCreated,
                                 Rating = thisReview.Rating
                             }).OrderByDescending(p=> p.DateCreated);
            var traffic = mainVM.Reviews.Count() < 10 ? 10 : mainVM.Reviews.Count();
            mainVM.Ratings = mainVM.Reviews.Sum(p => p.Rating) / traffic;

            return mainVM;
        }




        public async Task<ResponseMessage<string>> CheckOut(Guid itemID, Guid userID)
        {
            ResponseMessage<string> responseMessage = new ResponseMessage<string>();

            return responseMessage;
        }
        public async Task<ResponseMessage<string>> Delete(string t)
        {
            var responseMessage = new ResponseMessage<string>();
            try
            {
                var item = await _unitOfWork.Items.Find(Guid.Parse(t));
                _unitOfWork.Items.Delete(item);

                if (await _unitOfWork.Commit() > 0)
                {
                    ImageService.DeleteImage(item.Image, "CategoryIcons");
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Item Deleted!";
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Item Not Deleted!";
                }
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Item Not Deleted!";
            }

            return responseMessage;
        }
        public async Task<ResponseMessage<string>> DeleteMedia(Guid mID)
        {
            var responseMessage = new ResponseMessage<string>();
            try
            {
                var media = await _unitOfWork.Files.Find(mID);
                _unitOfWork.Files.Delete(media);

                if (await _unitOfWork.Commit() > 0)
                {
                    if (media.IsVideo)
                    {
                        VideoServices.Delete(media.Resource, "Gallery");
                    }
                    else
                    {
                        ImageService.DeleteImage(media.Resource, "Gallery");
                    }
                    
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Media Deleted!";
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Media Not Deleted!";
                }
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Media Not Deleted!";
            }

            return responseMessage;
        }
    }
}
