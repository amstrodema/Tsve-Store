using App.Logger.Business;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Omega_Store.Services;
using Store.Business;
using Store.Data.Interface;
using Store.Model;
using Store.Model.ViewModel;

namespace Omega_Store.Controllers
{
    public class ShopController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly StoreBusiness _storeBusiness;
        private readonly LoginValidator _loginValidator;
        private readonly LoggerBusiness _loggerBusiness;
        private readonly SearchBusiness _searchBusiness;                        
        //private static string? cartHoldings = "";

        public ShopController(IUnitOfWork unitOfWork, StoreBusiness storeBusiness, LoginValidator loginValidator, LoggerBusiness loggerBusiness, SearchBusiness searchBusiness)
        {
            _unitOfWork = unitOfWork;
            _storeBusiness = storeBusiness;
            _loginValidator = loginValidator;
            _loggerBusiness = loggerBusiness;
            _searchBusiness = searchBusiness;           
        }
        [Route("Shop")]
        public async Task<IActionResult> Index(string c, string orderID)
        {
            await _loggerBusiness.Traffic("Shop", _loginValidator.GetUserID());
            if (!string.IsNullOrEmpty(orderID))
            {
                return RedirectToAction("orders", "manager", new { Ref = orderID });
            }
          
            var res = await _storeBusiness.GetVMForShop();
            return View(res);
        }
        public async Task<IActionResult> Category(string t)
        {
            await _loggerBusiness.Traffic("Category", _loginValidator.GetUserID());
            try
            {
                if (string.IsNullOrEmpty(t))
                {
                    return RedirectToAction("Index");
                }
                var res = await _storeBusiness.GetFromCategory(t);
                return View(res);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }
        //public IActionResult Tracking()
        //{
        //    return View();
        //}
        public async Task<IActionResult> Item(string t)
        {
            await _loggerBusiness.Traffic("Item", _loginValidator.GetUserID(), default, t);
            if (string.IsNullOrEmpty(t))
            {
                return RedirectToAction("Index");
            }
            var res = await _storeBusiness.GetItem(t);
            return View(res);
        }
        public async Task<IActionResult> Cart()
        {
            await _loggerBusiness.Action("Cart", _loginValidator.GetUserID());
            var store = await _storeBusiness.GetStore();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] Review review)
        {
            try
            {
                var userID = _loginValidator.GetUserID();
                //var review = JsonConvert.DeserializeObject<Review>(reviewJSON);
                //if (review == null)
                //{
                //    throw new Exception();
                //}
                return PartialView("_review", await _storeBusiness.AddReview(review, userID));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> QuickView([FromBody] string tag)
        {
            try
            {
                return PartialView("_modal", await _storeBusiness.GetQuickView(tag));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetCart([FromBody] string orders)
        {
            try
            {
                var orderVM = JsonConvert.DeserializeObject<OrderVM[]>(orders);
                var res = await _storeBusiness.GetCart(orderVM);
                if (res.Orders.Count() < 1)
                {
                    return PartialView("_nocontent");
                }
                return PartialView("_cart", res.Orders);
            }
            catch (Exception)
            {
                return PartialView("_nocontent");
            }
        }
        [HttpPost]
        public async Task<IActionResult> QuickCart([FromBody] string orders)
        {
            try
            {
                var orderVM = JsonConvert.DeserializeObject<OrderVM[]>(orders);
                var res = await _storeBusiness.GetCart(orderVM);
                if (res.Orders.Count() < 1)
                {
                    return PartialView("_nocontent");
                }
                return PartialView("_navCartTemp", res.Orders);
            }
            catch (Exception)
            {
                return PartialView("_nocontent");
            }
        }
        //public async Task<IActionResult> Order(string orderID)
        //{
        //    try
        //    {
        //        var res = await _storeBusiness.Orders(orderID); return RedirectToAction("Index");
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return RedirectToAction("Index");
        //}
        public async Task<IActionResult> Confirmation()
        {
            await _loggerBusiness.Traffic("Confirmation", _loginValidator.GetUserID());
            try
            {
                if (TempData["CheckOutCart"].ToString() == "Confirmed")
                {
                    return View();
                }
            }
            catch (Exception)
            {
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Checkout()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CheckOutCart(CheckOutVM checkOutVM)
        {
            try
            {
                var ordas = checkOutVM.Cart; // _loginValidator.GetSession("cartHolder");
                if(ordas == null)
                {
                    TempData["MessageError"] = "Cart is empty!";
                    return RedirectToAction("Checkout");
                }

                var user = await _loginValidator.GetUserAuth();

                var hold = JsonConvert.DeserializeObject<OrderVM[]>(ordas);
                if (hold == null)
                {
                    TempData["MessageError"] = "Cart is unmatched!";
                    return RedirectToAction("Checkout");
                }
                checkOutVM.Orders = hold;
                var res = await _storeBusiness.CheckOutCart(checkOutVM, user);
                if (res.StatusCode != 200)
                {
                    TempData["MessageError"] = res.Message;
                    return RedirectToAction("Checkout");
                }
                TempData["OrderRef"] = res.Data;
                TempData["MessageSuccess"] = res.Message;
            }

            catch (Exception)
            {
                TempData["MessageError"] = "Not Completed";
                return RedirectToAction("Checkout");
            }

            TempData["CheckOutCart"] = "Confirmed";

            return RedirectToAction("Confirmation");
        }
        //[HttpPost]
        //public async Task<IActionResult> GetCheckOut([FromBody] string orders)
        //{
        //    await _loggerBusiness.Traffic("View CheckOut", _loginValidator.GetUserID());
        //    try
        //    {
        //        var orderVM = JsonConvert.DeserializeObject<OrderVM[]>(orders);
        //        var res = await _storeBusiness.GetCart(orderVM);
        //        if (res.Orders.Count() < 1)
        //        {
        //            return PartialView("_nocontent");
        //        }
        //        cartHoldings = orders;
        //        //_loginValidator.SetSession("cartHolder", JsonConvert.SerializeObject(orders));
        //        //_loginValidator.SetSession("cartHolder", orders);
        //      //  var ordas = _loginValidator.GetSession("cartHolder");
        //        return PartialView("_checkout", res);
        //    }
        //    catch (Exception)
        //    {
        //        return PartialView("_nocontent");
        //    }
        //}
        [HttpPost]
        public async Task<IActionResult> GetFave([FromBody] string faves)
        {
            try
            {
                var favorites = JsonConvert.DeserializeObject<Guid[]>(faves);
                var res = await _storeBusiness.GetVMForFave(favorites);
                return PartialView("_favourite", res);
            }
            catch (Exception)
            {
                return PartialView("_nocontent");
            }
        }
        [HttpPost]
        public async Task<IActionResult> ItemFeatures([FromBody] Guid itemID)
        {
            var res = from feature in await _unitOfWork.Features.GetByItemID(itemID)
                      select new FeaturePicker()
                      {
                          //ID = feature.ID,
                          Name = feature.Name,
                          Option = feature.Options
                      };
            return Ok(res);
        }
        [Route("Information")]
        public async Task<IActionResult> Information(string tag, int pageNo)
        {
            if (string.IsNullOrWhiteSpace(tag))
                return View(await _searchBusiness.GetQuestions(pageNo));
            else
                return View("InfoDetails", await _searchBusiness.GetQuestion(tag));
        }
        [Route("Search")]
        public async Task<IActionResult> Search(string query, int pageNo)
        {
            ViewData["Query"] = query;
            var val = await _searchBusiness.SearchQuestion(query, pageNo);
            return View(val.Data);
        }
        [Route("Tracking")]
        public async Task<IActionResult> Tracking(string orderID)
        {
            return View();
        }
        [HttpPost]
        public IActionResult Currency([FromBody] string val)
        {
            GenericBusiness.ShoppingCurrencySymbol = val == "NGN" ? "₦" : "$";
            GenericBusiness.ShoppingCurrency = val;
            return Ok();
        }

    }
}
