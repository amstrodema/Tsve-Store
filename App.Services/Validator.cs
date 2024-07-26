//namespace App.Services
//{
//    public class Validator
//    {
//        private readonly IHttpContextAccessor _context;
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly GroupBusiness _groupBusiness;
//        private readonly GenericBusiness _genericBusiness;

//        public LoginValidator(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, GroupBusiness groupBusiness, GenericBusiness genericBusiness)
//        {
//            _context = httpContextAccessor;
//            _unitOfWork = unitOfWork;
//            _groupBusiness = groupBusiness;
//            _genericBusiness = genericBusiness;
//        }

//        public IHttpContextAccessor Context() { return _context; }

//        public void SetSession(string key, string json)
//        {
//            _context.HttpContext.Session.SetString(key, json);
//        }

//        public async Task<bool> IsLoggedInAuth()
//        {
//            try
//            {
//                if (await GetUserAuth() != null)
//                {
//                    return true;
//                }
//                return false;
//            }
//            catch (Exception)
//            {
//                return false;
//            }
//        }
//        public bool IsLoggedIn()
//        {
//            try
//            {
//                if (GetUser() != null)
//                {
//                    return true;
//                }
//                return false;
//            }
//            catch (Exception)
//            {
//                return false;
//            }
//        }
//        public async Task<bool> IsAdminAuth()
//        {
//            try
//            {
//                var user = await GetUserAuth();
//                if (user != null && user.IsAdmin)
//                {
//                    return true;
//                }
//                return false;
//            }
//            catch (Exception)
//            {
//                return false;
//            }
//        }
//        public bool IsAdmin()
//        {
//            try
//            {
//                var user = GetUser();
//                if (user != null && user.IsAdmin)
//                {
//                    return true;
//                }
//                return false;
//            }
//            catch (Exception)
//            {
//                return false;
//            }
//        }
//        public bool IsDev()
//        {
//            try
//            {
//                var user = GetUser();
//                if (user != null && user.IsDev)
//                {
//                    return true;
//                }
//                return false;
//            }
//            catch (Exception)
//            {
//                return false;
//            }
//        }

//        public Guid GetUserID()
//        {
//            var user = GetUser();
//            if (user == null)
//            {
//                LogOut();
//                return default;
//            }
//            return user.ID;
//        }
//        public async Task<Guid> GetUserIDAuth()
//        {
//            var user = await GetUserAuth();
//            if (user == null)
//            {
//                LogOut();
//                return default;
//            }
//            return user.ID;
//        }

//        //public async Task<bool> isDetailCompleteAuth()
//        //{
//        //    var user = await GetUserAuth();
//        //    try
//        //    {
//        //        if (user == null)
//        //        {
//        //            return false;
//        //        }

//        //        if ((!string.IsNullOrEmpty(user.Fname) && !string.IsNullOrEmpty(user.Address) && user.InstitutionID != default && user.CountryID != default && user.IsEmailVer && !string.IsNullOrEmpty(user.ProfileImage)) || user.Username == "trendycampus")
//        //        {
//        //            return true;
//        //        }
//        //    }
//        //    catch (Exception)
//        //    {
//        //    }
//        //    return false;
//        //}
//        //public bool isDetailComplete()
//        //{
//        //    var user = GetUser();
//        //    try
//        //    {
//        //        if (user == null)
//        //        {
//        //            return false;
//        //        }

//        //        if ((!string.IsNullOrEmpty(user.Fname) && !string.IsNullOrEmpty(user.Address) && user.InstitutionID != default && user.CountryID != default && user.IsEmailVer && !string.IsNullOrEmpty(user.ProfileImage)) || user.Username == "trendycampus")
//        //        {
//        //            return true;
//        //        }
//        //    }
//        //    catch (Exception)
//        //    {
//        //    }
//        //    return false;
//        //}

//        public User? GetUser()
//        {
//            try
//            {
//                var val = _context.HttpContext.Session.GetString("user");
//                string str = val == null ? "" : val;
//                var user = JsonConvert.DeserializeObject<User>(str);

//                if (user == null || user.IsBanned || !user.IsActive || GenericBusiness.IsLocked && !user.IsDev)
//                {
//                    return null;
//                }
//                return user;
//            }
//            catch (Exception)
//            {
//                return null;
//            }
//        }

//        public async Task<User?> GetUserAuth()
//        {
//            try
//            {
//                var val = _context.HttpContext.Session.GetString("user");
//                string str = val == null ? "" : val;
//                User? user = JsonConvert.DeserializeObject<User>(str);

//                if (user == null || user.IsBanned || !user.IsActive || (GenericBusiness.IsLocked && !user.IsDev))
//                {
//                    return null;
//                }

//                if (await _unitOfWork.LoginMonitors.GetMonitorByUserID(user.ID, user.StoreID) != null)
//                {
//                    return user;
//                }

//                return null;
//            }
//            catch (Exception)
//            {
//                return null;
//            }
//        }

//        public async Task<int> CheckNotification(Guid userID)
//        {
//            try
//            {
//                return await _unitOfWork.Notifications.CheckForNotification(userID);

//            }
//            catch (Exception)
//            {
//                return 0;
//            }
//        }

//        public async Task<Store.Model.Store> GetShop()
//        {
//            return await _unitOfWork.Stores.Find(_genericBusiness.StoreID);
//        }
//        public string GetShoppingCurrency()
//        {
//            return GenericBusiness.ShoppingCurrency;
//        }
//        public string GetShoppingCurrencySymbol()
//        {
//            return GenericBusiness.ShoppingCurrencySymbol;
//        }
//        public async Task<IEnumerable<GroupVM>> GetCatList()
//        {
//            try
//            {
//                return await _groupBusiness.GetCatList();

//            }
//            catch (Exception)
//            {
//                return new List<GroupVM>();
//            }
//        }
//        public void LogOut()
//        {
//            _context.HttpContext.Session.Clear();
//        }
//    }
//}