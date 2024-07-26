using App.Services;
using Store.Data.Interface;
using Store.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Newtonsoft.Json;
using Store.Model.Hybrid;

namespace Store.Business
{
    public class UserBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly GenericBusiness _genericBusiness;
        public UserBusiness(IUnitOfWork unitOfWork, GenericBusiness genericBusiness)
        {
            _unitOfWork = unitOfWork;
            _genericBusiness = genericBusiness;
        }

        public async Task<ResponseMessage<string>> Setup()
        {
                var Tsve_ResponseMessage = new ResponseMessage<string>();
            try
            {
                var thisAdmin = await _unitOfWork.Users.GetActiveUserByUserName("administrator");
                if (thisAdmin ==null)
                {
                    thisAdmin = new User()
                    {
                        ID = Guid.NewGuid(),
                        Username = "administrator",
                        Password = EncryptionService.Encrypt("78JpUi#oi"),
                        IsAdmin = true,
                        IsActive = true,
                        Email = "salmatraglobal@gmail.com",
                        IsEmailVer = true
                    };
                    User user = new User()
                    {
                        ID = Guid.NewGuid(),
                        Username = "zynxx",
                        Password = EncryptionService.Encrypt("123QwePoi!!"),
                        IsDev = true,
                        IsActive =true,
                        Email = "yk4love38@gmail.com",
                        IsEmailVer = true
                    };

                    await _unitOfWork.Users.Create(thisAdmin);
                    await _unitOfWork.Users.Create(user);
                }
                else
                {
					Tsve_ResponseMessage.StatusCode = 201;
					Tsve_ResponseMessage.Message = "Setup Already Completed";
                    return Tsve_ResponseMessage;
				}
                _genericBusiness.StoreID = Guid.NewGuid();
                try
                {
                    FileService.DeleteFile("aexxj");
                }
                catch (Exception)
                {

                }
			//	FileService.WriteToFile(_genericBusiness.StoreID.ToString(), "aexxj");
				//var aexxj =  FileService.ReadFromFile("aexxj");

                var store = await _unitOfWork.Stores.Find(_genericBusiness.StoreID);
                if (store == null)
                {
                    store = new Model.Store()
                    {
                        ID = _genericBusiness.StoreID,
                        DateCreated = DateTime.UtcNow.AddHours(1),
                        CreatedBy = thisAdmin.ID,
                        Name = "Omega Beta Stores",
                        Logo = "Omega",
                        LogoSuffix = "Stores",
                        ExpiryDate = DateTime.UtcNow.AddHours(1).AddDays(365)
                    };

                    User user = new User()
                    {
                        ID = Guid.NewGuid(),
                        Username = "owner",
                        Password = EncryptionService.Encrypt("12345!!"),
                        StoreID = _genericBusiness.StoreID,
                        CreatedBy = thisAdmin.ID,
                        IsActive = true,
                        Email = "thisowner@gmail.com",
                        IsEmailVer = true,
                        IsOwner = true,
                        DateCreated = DateTime.UtcNow.AddHours(1)
                    };

                    Group group = new Group()
                    {
                        ID = Guid.NewGuid(),
                        DateCreated = DateTime.UtcNow.AddHours(1),
                        CreatedBy = user.ID,
                        IsActive =true,
                        StoreID = _genericBusiness.StoreID,
                        Name = "General",
                        Tag = GenericService.GetTag("General"),
                        IsDefault = true
                    };

                    await _unitOfWork.Groups.Create(group);
                    await _unitOfWork.Stores.Create(store);
                    await _unitOfWork.Users.Create(user);

                    if (await _unitOfWork.Commit() > 0)
                    {
                        Tsve_ResponseMessage.StatusCode = 200;
                        Tsve_ResponseMessage.Message = "Setup Completed";
                    }
                    else
                    {
                        Tsve_ResponseMessage.StatusCode = 201;
                        Tsve_ResponseMessage.Message = "Setup Not Completed";
                    }
                }
                else
                {
                    Tsve_ResponseMessage.StatusCode = 201;
                    Tsve_ResponseMessage.Message = "Setup Already Completed";
                }
            }
            catch (Exception e)
            {
                FileService.WriteToFile("\n\n" + e, "ErrorLogs");
                Tsve_ResponseMessage.StatusCode = 209;
                Tsve_ResponseMessage.Message = "Setup Failed";
            }

            return Tsve_ResponseMessage;
        }

        //public async Task<User> GetUser(Guid userID)
        //{
        //    UserVM userVM = new UserVM();
        //    try
        //    {
        //        var user = await Task.Run(() => _unitOfWork.Users.Find(userID));
        //        userVM.Countries = await Task.Run(() => _unitOfWork.Categories.GetActiveCategoriesBySection("Country"));
        //        var country = await Task.Run(() => userVM.Countries.FirstOrDefault(p => p.ID == user.CountryID));
        //        var institution = await Task.Run(() => _unitOfWork.Institutions.Find(user.InstitutionID));
        //        userVM.Referrals = (await _unitOfWork.Users.GetReferrals(user.Username)).Count();
        //        userVM.IsLocked = SingletonBusiness.IsLocked;


        //        var followers = await Task.Run(() => _unitOfWork.Follows.GetAllFollowers(user.ID));
        //        var following = await Task.Run(() => _unitOfWork.Follows.GetAllIFollow(user.ID));

        //        var loggedUsersFollows = await Task.Run(() => _unitOfWork.Follows.GetFollows(userID));

        //        userVM.FollowersList = from follo in followers
        //                               join thisFollower in await Task.Run(() => _unitOfWork.Users.GetAll()) on follo.UserID equals thisFollower.ID
        //                               select new UserHybrid()
        //                               {
        //                                   Fname = thisFollower.Fname,
        //                                   LName = thisFollower.LName,
        //                                   Username = thisFollower.Username,
        //                                   ID = thisFollower.ID,
        //                                   ProfileImage = thisFollower.ProfileImage == "" ? "" : ImageService.GetSmallImagePath(thisFollower.ProfileImage, "Profile"),
        //                                   IsFollowedByMe = loggedUsersFollows.FirstOrDefault(p => p.FollowedUserID == thisFollower.ID) == default ? false : true,
        //                                   IsFollowMe = loggedUsersFollows.FirstOrDefault(p => p.UserID == thisFollower.ID) == default ? false : true
        //                               };

        //        userVM.FollowingsList = from follo in following
        //                                join thisFollower in await Task.Run(() => _unitOfWork.Users.GetAll()) on follo.FollowedUserID equals thisFollower.ID
        //                                select new UserHybrid()
        //                                {
        //                                    Fname = thisFollower.Fname,
        //                                    LName = thisFollower.LName,
        //                                    Username = thisFollower.Username,
        //                                    ID = thisFollower.ID,
        //                                    ProfileImage = thisFollower.ProfileImage == "" ? "" : ImageService.GetSmallImagePath(thisFollower.ProfileImage, "Profile"),
        //                                    IsFollowedByMe = loggedUsersFollows.FirstOrDefault(p => p.FollowedUserID == thisFollower.ID) == default ? false : true,
        //                                    IsFollowMe = loggedUsersFollows.FirstOrDefault(p => p.UserID == thisFollower.ID) == default ? false : true
        //                                };

        //        userVM.Followers = followers.Count();
        //        userVM.Followings = following.Count();

        //        var userHybrid = new UserHybrid()
        //        {
        //            Address = user.Address,
        //            Bio = user.Bio,
        //            Country = country == null ? "n/a" : country.Name,
        //            CountryID = user.CountryID,
        //            IsSubscribed = user.IsSubscribed,
        //            Name = user.Fname == "" || user.LName == "" ? "n/a" : $"{user.Fname} {user.LName}",
        //            Username = user.Username,
        //            ProfileImage = user.ProfileImage == "" ? "" : ImageService.GetLargeImagePath(user.ProfileImage, "Profile"),
        //            Email = user.Email,
        //            Tel = user.Tel,
        //            Gender = user.Gender,
        //            ID = user.ID,
        //            Fname = user.Fname,
        //            IsAdmin = user.IsAdmin,
        //            LName = user.LName,
        //            AppID = appID,
        //            IsBanned = user.IsBanned,
        //            IsApproved = user.IsApproved,
        //            IsActive = user.IsActive,
        //            IsDev = user.IsDev,
        //            IsEmailVer = user.IsEmailVer,
        //            InstitutionID = user.InstitutionID,
        //            InstitutionName = institution == null ? "" : institution.Name,
        //            InstitutionAddress = institution == null ? "" : institution.Address,
        //            InstitutionCountry = institution == null ? "" : (await Task.Run(() => userVM.Countries.FirstOrDefault(p => p.ID == institution.CountryID))).Name,
        //            InstitutionState = institution == null ? "" : (await Task.Run(() => _unitOfWork.SubCategories.Find(institution.StateID))).Name,
        //            CV = user.CV == "" ? "" : DocumentService.GetDocumentPath(user.CV, "CV")
        //        };

        //        userVM.UserHybrid = userHybrid;
        //        userVM.User = user;
        //        userVM.Institution = institution == null ? new InstitutionHybrid() : new InstitutionHybrid()
        //        {
        //            ID = institution.ID,
        //            StateID = institution.StateID,
        //            Country = (await Task.Run(() => userVM.Countries.FirstOrDefault(p => p.ID == institution.CountryID))).Name,
        //            State = (await Task.Run(() => _unitOfWork.SubCategories.Find(institution.StateID))).Name,
        //            CountryID = institution.CountryID,
        //            Name = institution.Name,
        //            Address = institution.Address
        //        };



        //        userVM.Institutions = await Task.Run(() => _unitOfWork.Institutions.GetAll());
        //    }
        //    catch (Exception e)
        //    {

        //        FileService.WriteToFile("\n\n" + e, "ErrorLogs");
        //    }

        //    return userVM;
        //}

        public async Task<ResponseMessage<User>> Login(string usernameOrPasswordorPhone, string password)
        {
            ResponseMessage<User> Tsve_ResponseMessage = new ResponseMessage<User>();
            try
            {
                //var pic = _genericBusiness.StoreID;
                User thisUser = await _unitOfWork.Users.GetUserByUserNameOrEmail(usernameOrPasswordorPhone);

                if (thisUser == null)
                {
                    Tsve_ResponseMessage.StatusCode = 201;
                    Tsve_ResponseMessage.Message = "User not found!";
                    return Tsve_ResponseMessage;
                }
                else if (thisUser.IsBanned || !thisUser.IsActive)
                {
                    Tsve_ResponseMessage.StatusCode = 201;
                    Tsve_ResponseMessage.Message = "User is deactivated or banned. Contact administrator";
                }
                else if (EncryptionService.Validate(password, thisUser.Password))
                {
                    var monitor = await Task.Run(() => _unitOfWork.LoginMonitors.GetMonitorByUserIDOnly(thisUser.ID));
                    //_genericBusiness.StoreID = Guid.Parse(FileService.ReadFromFile("aexxj"));

                    if (monitor != null)
                    {
                        _unitOfWork.LoginMonitors.Delete(monitor);
                    }

                    LoginMonitor loginMonitor = new LoginMonitor()
                    {
                        ID = Guid.NewGuid(),
                        ClientCode = Guid.NewGuid(),
                        StoreID = _genericBusiness.StoreID,
                        DateCreated = DateTime.UtcNow.AddHours(1),
                        IsActive = true,
                        UserID = thisUser.ID,
                        TimeLogged = DateTime.UtcNow.AddHours(1)
                    };

                    thisUser.AppID = loginMonitor.ClientCode;
                    await _unitOfWork.LoginMonitors.Create(loginMonitor);

                    if (await _unitOfWork.Commit() > 0)
                    {
                        Tsve_ResponseMessage.StatusCode = 200;
                        Tsve_ResponseMessage.Data = thisUser;
                        Tsve_ResponseMessage.Message = "Login Successful!";
                    }
                    else
                    {
                        Tsve_ResponseMessage.StatusCode = 201;
                        Tsve_ResponseMessage.Message = "Login Not Successful!";
                    }
                }
                else
                {
                    Tsve_ResponseMessage.StatusCode = 201;
                    Tsve_ResponseMessage.Message = "Invalid login details!";
                }
            }
            catch (Exception)
            {
                Tsve_ResponseMessage.StatusCode = 201;
                Tsve_ResponseMessage.Message = "Try again!";
            }

            return Tsve_ResponseMessage;
        }
        //public async Task<ResponseMessage<User>> Create(User user)
        //{
        //    ResponseMessage<User> Tsve_ResponseMessage = new ResponseMessage<User>();

        //    try
        //    {
        //        User thisUser = await _unitOfWork.Users.GetUserByUserNameOrEmail(user.Username, _genericBusiness.StoreID);

        //        if (thisUser != null)
        //        {
        //            Tsve_ResponseMessage.StatusCode = 201;
        //            Tsve_ResponseMessage.Message = "User exist already";
        //            return Tsve_ResponseMessage;
        //        }

        //        thisUser = await _unitOfWork.Users.GetUserByUserNameOrEmail(user.Email, _genericBusiness.StoreID);

        //        if (thisUser != null)
        //        {
        //            Tsve_ResponseMessage.StatusCode = 201;
        //            Tsve_ResponseMessage.Message = "User exist already";
        //            return Tsve_ResponseMessage;
        //        }
        //        thisUser = await _unitOfWork.Users.GetUserByUserNameOrEmail(user.Tel, _genericBusiness.StoreID);

        //        if (thisUser != null)
        //        {
        //            Tsve_ResponseMessage.StatusCode = 201;
        //            Tsve_ResponseMessage.Message = "User exist already";
        //            return Tsve_ResponseMessage;
        //        }

        //        thisUser.Password = EncryptionService.Encrypt(thisUser.Password);

        //        await _unitOfWork.Users.Create(user);

        //        if (await _unitOfWork.Commit() > 0)
        //        {
        //            Tsve_ResponseMessage.StatusCode = 200;
        //            Tsve_ResponseMessage.Message = "Registration Completed!";
        //        }
        //        else
        //        {
        //            Tsve_ResponseMessage.StatusCode = 201;
        //            Tsve_ResponseMessage.Message = "Registration Not Completed!";
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Tsve_ResponseMessage.StatusCode = 201;
        //        Tsve_ResponseMessage.Message = "Failed!";
        //        FileService.WriteToFile("\n\n" + e, "ErrorLogs");
        //    }


        //    return Tsve_ResponseMessage;
        //}

        public async Task<ResponseMessage<string>> ChangePassword(Guid userID, string password, string newPassword)
        {
            ResponseMessage<string> Tsve_ResponseMessage = new ResponseMessage<string>();
            User thisUser = await _unitOfWork.Users.Find(userID);

            try
            {
                if (EncryptionService.Validate(password, thisUser.Password))
                {
                    thisUser.Password = EncryptionService.Encrypt(newPassword);
                    thisUser.ModifiedBy = userID;
                    thisUser.DateModified = DateTime.UtcNow.AddHours(1);
                    _unitOfWork.Users.Update(thisUser);

                    if (await _unitOfWork.Commit() > 0)
                    {
                        Tsve_ResponseMessage.StatusCode = 200;
                        Tsve_ResponseMessage.Message = "Password modified!";
                    }
                    else
                    {
                        Tsve_ResponseMessage.StatusCode = 201;
                        Tsve_ResponseMessage.Message = "Password not modified";
                    }
                }
                else
                {
                    Tsve_ResponseMessage.StatusCode = 201;
                    Tsve_ResponseMessage.Message = "Invalid Password!";
                }               
            }
            catch (Exception e)
            {
                Tsve_ResponseMessage.StatusCode = 201;
                Tsve_ResponseMessage.Message = "Password not modified!";
                FileService.WriteToFile("\n\n" + e, "ErrorLogs");
            }

            return Tsve_ResponseMessage;
        }

        public async Task<ResponseMessage<string>> ResetPassword(Guid userID, string email)
        {
            ResponseMessage<string> Tsve_ResponseMessage = new ResponseMessage<string>();
            User thisUser;

            if (userID != default)
            {
                thisUser = await _unitOfWork.Users.Find(userID);
            }
            else
            {
                thisUser = await _unitOfWork.Users.GetUserByUserNameOrEmail(email);
            }

            try
            {
                if (thisUser == null)
                {
                    Tsve_ResponseMessage.StatusCode = 201;
                    Tsve_ResponseMessage.Message = "Password verification not sent!";
                    return Tsve_ResponseMessage;
                }

                thisUser.PasswordVer = GenService.RandomGen5Code();
                thisUser.ModifiedBy = thisUser.ID;
                thisUser.DateModified = DateTime.UtcNow.AddHours(1);
                thisUser.PasswordVerExp = DateTime.UtcNow.AddHours(1).AddMinutes(10);
                Tsve_ResponseMessage.Data = thisUser.ID.ToString();

                _unitOfWork.Users.Update(thisUser);

                if (await _unitOfWork.Commit() > 0)
                {
                    EmailTemplateService.ResetPassword(thisUser);
                    Tsve_ResponseMessage.StatusCode = 200;
                    Tsve_ResponseMessage.Message = "Password verification sent!";
                    return Tsve_ResponseMessage;
                }
            }
            catch (Exception e)
            {
                Tsve_ResponseMessage.StatusCode = 201;
                Tsve_ResponseMessage.Message = "Password verification not sent!";
                FileService.WriteToFile("\n\n" + e, "ErrorLogs");
            }

            return Tsve_ResponseMessage;
        }
        public async Task<ResponseMessage<string>> ResetEmailVerification(Guid userID)
        {
            ResponseMessage<string> Tsve_ResponseMessage = new ResponseMessage<string>();
            User thisUser = await _unitOfWork.Users.Find(userID);

            try
            {

                thisUser.EmailVerCode = GenService.RandomGen5Code();
                thisUser.ModifiedBy = userID;
                thisUser.DateModified = DateTime.UtcNow.AddHours(1);
                thisUser.EmailVerExp = DateTime.UtcNow.AddHours(1).AddMinutes(10);

                _unitOfWork.Users.Update(thisUser);

                if (await _unitOfWork.Commit() > 0)
                {
                    EmailTemplateService.VerifyEmail(thisUser);
                    Tsve_ResponseMessage.StatusCode = 200;
                    Tsve_ResponseMessage.Message = "Email verification sent!";
                    return Tsve_ResponseMessage;
                }
            }
            catch (Exception e)
            {
                Tsve_ResponseMessage.StatusCode = 201;
                Tsve_ResponseMessage.Message = "Email verification not sent!";
                FileService.WriteToFile("\n\n" + e, "ErrorLogs");
            }

            return Tsve_ResponseMessage;
        }

        public async Task<ResponseMessage<string>> EmailVerification(Guid userID, string verCode)
        {
            ResponseMessage<string> Tsve_ResponseMessage = new ResponseMessage<string>();
            User thisUser = await _unitOfWork.Users.Find(userID);

            try
            {
                if (thisUser.EmailVerCode != verCode)
                {
                    Tsve_ResponseMessage.StatusCode = 201;
                    Tsve_ResponseMessage.Message = "Invalid code!";
                    return Tsve_ResponseMessage;
                }
                else if (thisUser.EmailVerExp < DateTime.UtcNow.AddHours(1))
                {
                    Tsve_ResponseMessage.StatusCode = 201;
                    Tsve_ResponseMessage.Message = "Email verification code expired!";
                    return Tsve_ResponseMessage;
                }

                thisUser.IsEmailVer = true;
                thisUser.ModifiedBy = userID;
                thisUser.DateModified = DateTime.UtcNow.AddHours(1);

                _unitOfWork.Users.Update(thisUser);

                if (await _unitOfWork.Commit() > 0)
                {
                    Tsve_ResponseMessage.StatusCode = 200;
                    Tsve_ResponseMessage.Message = "Email verified!";
                    return Tsve_ResponseMessage;
                }
            }
            catch (Exception e)
            {
                FileService.WriteToFile("\n\n" + e, "ErrorLogs");
            }

            return Tsve_ResponseMessage;
        }
        public async Task<ResponseMessage<int>> LogOut(Guid userID, Guid appID)
        {
            ResponseMessage<int> Tsve_ResponseMessage = new ResponseMessage<int>();
            try
            {
                LoginMonitor loginMonitor = await _unitOfWork.LoginMonitors.GetMonitorByUserID(userID, appID);
                _unitOfWork.LoginMonitors.Delete(loginMonitor);

                if (await _unitOfWork.Commit() > 0)
                {
                    Tsve_ResponseMessage.StatusCode = 200;
                }
                else
                {
                    Tsve_ResponseMessage.StatusCode = 201;
                }
            }
            catch (Exception e)
            {
                FileService.WriteToFile("\n\n" + e, "ErrorLogs");
                Tsve_ResponseMessage.StatusCode = 209;
            }
            return Tsve_ResponseMessage;
        }



    }
}
