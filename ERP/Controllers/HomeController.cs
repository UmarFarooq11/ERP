using LIS.Models;
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Web.Mvc;
using System.Data.Entity; 
using System.Net; 
using System.Data;

namespace LIS.Controllers
{
    public class HomeController : Controller
    {
        #region Objects

        SMSEntities dbSms = new SMSEntities();
        SetupEntities dbSetup = new SetupEntities();
        SMS_User objUser = new SMS_User();
        SMS_UserRole ObjUserRole = new SMS_UserRole();

         
        #endregion

        public ActionResult Index()
        {

            if (Session["UserInfo"] == null)
            {
                return View("_LoginPartial");
            }
            else
            {
                return View();
            }

        }
        public ActionResult LogOut()
        {
            Session.Remove("UserInfo");
            Session.Remove("UserName");
            Session.Remove("Picture");
            return RedirectToAction("Index");
        }
        public ActionResult Login(SMS_User model)
        { 
            int UserId = model.UserId == null ? 0 : model.UserId;
            string Password = model.Password == null ? "" : model.Password;

            var UserInfo = dbSms.SMS_User.Where(x => x.UserId == UserId && x.Password == Password).ToList();
            if (UserInfo.Count != 0)
            {
                Session["UserInfo"] = UserInfo.First();
                Session["UserName"] = UserInfo.First().FirstName + " " + UserInfo.First().LastName;
                Session["Picture"] = UserInfo.First().Picture == "" ? "../Images/defaultImg.png" : UserInfo.First().Picture;
            }
            else
            {
                Session["UserInfo"] = null;
                Session["UserName"] = null;
                Session["Picture"] = null;
            }
            return RedirectToAction("Index"); 

            //ObjUser.UserId = model.UserId == null ? 0 : model.UserId;
            //ObjUser.Password = model.Password == null ? "" : model.Password;
            //DataSet dsUserInfo = ObjUser.CRUD_User(ObjUser);

            //if (dsUserInfo.Tables != null && dsUserInfo.Tables[0].Rows.Count > 0)
            //{
            //    Sms_User UserInfoList = (Sms_User)ObjUser.GetUserList(dsUserInfo.Tables[0]);
            //    Session["UserInfo"] = UserInfoList;
            //    Session["UserName"] = UserInfoList.FirstName + " " + UserInfoList.LastName;
            //    Session["Picture"] = UserInfoList.Picture == "" ? "../Images/defaultImg.png" : UserInfoList.Picture;
            //}
            //else
            //{
            //    Session["UserInfo"] = null;
            //    Session["UserName"] = null;
            //    Session["Picture"] = null;
            //}

        }

        #region SMS_Setup 

        // Get Tree Menu 
        public JsonResult GetTreeMenu()
        {
            objUser = (SMS_User)Session["UserInfo"];
            object List = new object();
             

            var _ParentList = dbSms.SMS_Menu.ToList();
            if (objUser != null && objUser.SMS_Group.Type == 1)
            {
                //var _ParentList = dbSms.SMS_Menu.ToList();
                List = _ParentList
                                .Where(c => c.ChildId == 0)
                                .Select(c => new SMS_Menu()
                                {
                                    MenuId = c.MenuId,
                                    ParentId = c.ParentId,
                                    ChildId = c.ChildId,
                                    MenuName = c.MenuName,
                                    Icon = c.Icon,
                                    Action = c.Action,
                                    Status = c.Status,
                                    CreatedBy = c.CreatedBy,
                                    CreatedDate = c.CreatedDate,
                                    UpdatedBy = c.UpdatedBy,
                                    UpdatedDate = c.UpdatedDate,
                                    MenuList = GetChildren(_ParentList, c.ParentId)
                                })
                                .ToList();
            }
            else
            {
                List<SMS_Menu> menu = new List<SMS_Menu>();
                var LIST = from m in dbSms.SMS_Menu
                           from d in dbSms.SMS_UserRole.Where(x => x.Status == true && x.GroupId == objUser.GroupId && x.MenuId == m.MenuId)
                           select new { m };

                foreach (var item in LIST)
                {
                    menu.Add(new SMS_Menu()
                    {
                        MenuId = item.m.MenuId,
                        ParentId = item.m.ParentId,
                        ChildId = item.m.ChildId,
                        Action = item.m.Action,
                        Status = item.m.Status,
                        MenuName = item.m.MenuName,
                        Icon = item.m.Icon,
                        CreatedBy = item.m.CreatedBy,
                        CreatedDate = item.m.CreatedDate,
                        UpdatedBy = item.m.UpdatedBy,
                        UpdatedDate = item.m.UpdatedDate
                    });
                }

                _ParentList = menu;
                List = _ParentList
                              .Where(c => c.ChildId == 0)
                              .Select(c => new SMS_Menu()
                              {
                                  MenuId = c.MenuId,
                                  ParentId = c.ParentId,
                                  ChildId = c.ChildId,
                                  MenuName = c.MenuName,
                                  Icon = c.Icon,
                                  Action = c.Action,
                                  Status = c.Status,
                                  CreatedBy = c.CreatedBy,
                                  CreatedDate = c.CreatedDate,
                                  UpdatedBy = c.UpdatedBy,
                                  UpdatedDate = c.UpdatedDate,
                                  MenuList = GetChildren(_ParentList, c.ParentId)
                              })
                              .ToList();


            }
            return Json(List);
        }

        public static List<SMS_Menu> GetChildren(List<SMS_Menu> menu, int parentId)
        {
            List<SMS_Menu> list = menu.Where(c => c.ChildId == parentId)
                    .Select(c => new SMS_Menu
                    {
                        MenuId = c.MenuId,
                        ParentId = c.ParentId,
                        ChildId = c.ChildId,
                        MenuName = c.MenuName,
                        Icon = c.Icon,
                        Action = c.Action,
                        Status = c.Status,
                        CreatedBy = c.CreatedBy,
                        CreatedDate = c.CreatedDate,
                        UpdatedBy = c.UpdatedBy,
                        UpdatedDate = c.UpdatedDate,
                        MenuList = GetChildren(menu, c.ParentId)

                    })
                    .ToList();
            return list;
        }

        // CRUD Operation For Menu  
        public JsonResult CRUDMenu(IList<SMS_Menu> objMenu, string _Mode)
        {
            SMS_Menu _Menu = new SMS_Menu();
            var _List = new object();
            var _UserInfo = Session["UserInfo"];
            if (objMenu != null)
            {
                if (objMenu.Count > 0)
                {
                    // Set Properties in Object  
                    _Menu = objMenu[0]; 
                    _Menu.MakerWorkStationId = Dns.GetHostName(); 
                    _Menu.MenuList = new List<SMS_Menu>();
                }

                if (_Mode == "I")
                {
                    _Menu.CreatedBy= ((SMS_User)_UserInfo).UserId;
                    _Menu.CreatedDate = DateTime.Now;
                    // For Insert 
                    _Menu.ChildId = _Menu.ChildId == 0 ? _Menu.ParentId : 0;// dbSms.SMS_Menu.Max(m => m.ChildId + 1);
                    _Menu.ParentId = dbSms.SMS_Menu.Max(m => m.ParentId + 1);
                    dbSms.SMS_Menu.Add(_Menu);
                    dbSms.SaveChanges();

                }
                else if (_Mode == "U")
                {
                    _Menu.UpdatedBy = ((SMS_User)_UserInfo).UserId;
                    _Menu.UpdatedDate = DateTime.Now;
                    // For Update
                    dbSms.Entry(_Menu).State = EntityState.Modified;
                    dbSms.SaveChanges();
                }
                else if (_Mode == "D")
                {

                    dbSms.SMS_UserRole.RemoveRange(dbSms.SMS_UserRole.Where(x => x.MenuId == _Menu.ParentId).ToList());
                    dbSms.SMS_Menu.Remove(dbSms.SMS_Menu.Find(_Menu.MenuId));
                    dbSms.SaveChanges();

                }

            }
            else
            {
                _List = GetTreeMenu();
            }

            return Json(_List, JsonRequestBehavior.AllowGet);
        }

        // Get partial view as html By View Name
        public ActionResult GetPatialViewByName(string ViewName)
        {
            objUser = (SMS_User)Session["UserInfo"];
            if (Session["UserInfo"] == null)
                return RedirectToAction("Index");
            else
            {
                try
                {
                    var _Menu = dbSms.SMS_Menu.Where(x => x.MenuName.Trim() == ViewName.Trim()).Select(x => new { x.MenuId }).Single();
                    var _RolesByGroup = dbSms.SMS_UserRole.Where(x => x.MenuId == _Menu.MenuId & x.GroupId == objUser.GroupId).Select(x => new { x.InsertAllow, x.EditAllow, x.ViewAllow, x.Status });
                    Session["UserRoleByForm"] = _RolesByGroup;
                    Session["InsertAllow"] = objUser.SMS_Group.Type == 1 ? "" : (_RolesByGroup.SingleOrDefault().InsertAllow == false ? "hide" : "");
                    return PartialView(ViewName);
                }
                catch (Exception)
                {
                    return PartialView(ViewName);
                    throw;
                }
                
            }
        }

        // Get Group List
        public JsonResult GetGroupList()
        {
            var Result = from m in dbSms.SMS_Group
                         select new { m.GroupId, m.GroupName };
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        // Get permission role by Group
        public JsonResult GetPermissionRoleByGroup(int GroupId)
        {
            // Get Data in RolePermission By Group 
            // Linq Left Outer Join Query 
            var Result = from m in dbSms.SMS_Menu
                         from d in dbSms.SMS_UserRole.Where(x => x.GroupId == GroupId & x.MenuId == m.MenuId).DefaultIfEmpty()
                         select new
                         {
                             MenuID = m.MenuId,
                             m.ParentId,
                             m.ChildId,
                             m.MenuName,
                             m.Action,
                             m.Status,
                             RoleId = (d.RoleId == null ? 0 : d.RoleId),
                             GroupId = (d.GroupId == null ? 0 : d.GroupId),
                             RoleRowId = (d.RoleId == null ? 0 : d.RoleId),
                             InsertAllow = (d.InsertAllow == null ? false : d.InsertAllow),
                             EditAllow = (d.EditAllow == null ? false : d.EditAllow),
                             ViewAllow = (d.ViewAllow == null ? false : d.ViewAllow),
                             MenuActive = (d.Status == null ? false : d.Status)
                         };

            return Json(Result, JsonRequestBehavior.AllowGet);

            // Entity Query But Not working 
            //var joined = dbSms.SMS_Menu.GroupJoin(dbSms.SMS_UserRole, a => a.RowID, b => b.MenuId,
            //                                      (a, b) => new { a, b, hello = "Hello World!" }).ToList();

            //var Result1 = dbSms.SMS_Menu.GroupJoin(dbSms.SMS_UserRole, d => d.ParentId, m => m.MenuId,
            //              (m, d) => d.DefaultIfEmpty()
            //                .Select(s => new {
            //                    MenuID = m.RowID,
            //                    ParentId = m.ParentId,
            //                    ChildId = m.ChildId,
            //                    MenuName = m.MenuName,
            //                    Status = m.Status,
            //                    GroupId = (s.GroupId == null ? 0 : s.GroupId)
            //                    ,RoleRowId = (s.RowId == null ? 0 : s.RowId)
            //                    ,InsertAllow = (s.InsertAllow == null ? false : s.InsertAllow)
            //                    ,EditAllow = (s.EditAllow == null ? false : s.EditAllow)
            //                    ,ViewAllow = (s.ViewAllow == null ? false : s.ViewAllow)
            //                }).Where(x => x.GroupId == GroupId)).ToList();
        }


        // CRUD Operation For User Role  
        public JsonResult CRUDSmsUserRole(IList<SMS_UserRole> ParamPermission)
        {

            if (ParamPermission != null)
            {
                foreach (SMS_UserRole _UserRole in ParamPermission)
                {

                    if (_UserRole != null)
                    {
                        // Set Properties in Object   
                        _UserRole.MakerWorkStationId = Request.UserHostAddress;
                    }
                    if (_UserRole.RoleId > 0)
                    {

                        _UserRole.CreatedBy = 1;
                        _UserRole.CreatedDate = DateTime.Today;
                        // For Update
                        dbSms.Entry(_UserRole).State = EntityState.Modified;
                        dbSms.SaveChanges();
                    }
                    else
                    {
                        _UserRole.UpdatedBy = 1;
                        _UserRole.UpdatedDate = DateTime.Today;
                        // For Insert 
                        dbSms.SMS_UserRole.Add(_UserRole);
                        dbSms.SaveChanges();
                    }

                }
            }

            return Json("saved");
        }

        // CRUD Operation For User Sub Role
        public JsonResult CRUDSmsUserSubRole(IList<SMS_SubUserRole> ParamSubPermission)
        {
            var _UserRoles = Session["UserRoleByForm"];
            var _UserInfo = Session["UserInfo"];

            if (ParamSubPermission != null)
            {
                foreach (SMS_SubUserRole _UserSubRole in ParamSubPermission)
                { 
                    if (_UserSubRole != null)
                    {
                        // Set Properties in Object   
                        _UserSubRole.MakerWorkStationId = Dns.GetHostName();
                    }
                    if (_UserSubRole.SubRoleId > 0)
                    {
                        _UserSubRole.UpdatedBy = ((SMS_User)_UserInfo).UserId;
                        _UserSubRole.UpdatedDate = DateTime.Today;
                       
                        // For Update
                        dbSms.Entry(_UserSubRole).State = EntityState.Modified;
                        dbSms.SaveChanges();
                    }
                    else
                    {
                        _UserSubRole.CreatedBy = ((SMS_User)_UserInfo).UserId;
                        _UserSubRole.CreatedDate = DateTime.Today;
                        // For Insert 
                        dbSms.SMS_SubUserRole.Add(_UserSubRole);
                        dbSms.SaveChanges();
                    }

                }
            }

            return Json("saved");
        }

        #endregion

        #region Custom_Setup

        // CRUD Operation For Country  
        public JsonResult CRUDCountry(IList<Country> objCountry, string _Mode)
        {
            Country _Country = new Country();
            var _List = new object();

            var _UserRoles = Session["UserRoleByForm"];
            var _UserInfo = Session["UserInfo"];

            if (ObjUserRole != null)
            {
                if (objCountry != null)
                {
                    if (objCountry.Count > 0)
                    {
                        // Set Properties in Object  
                        _Country = objCountry[0];
                        _Country.MakerWorkStationId = Dns.GetHostName();
                    }
                        if (_Mode == "D")
                    {
                        // For Delete 
                        dbSetup.Countries.Remove(dbSetup.Countries.Find(_Country.CountryId));
                        dbSetup.SaveChanges();
                    }
                    else if (objCountry.Count > 0 && objCountry[0].CountryId > 0)
                    {
                        _Country.UpdatedBy = ((SMS_User)_UserInfo).UserId;
                        _Country.UpdatedDate = DateTime.Now;

                        // For Update
                        dbSetup.Entry(_Country).State = EntityState.Modified;
                        dbSetup.SaveChanges();
                    }
                    else if (_Mode == "I")
                    {
                        _Country.CreatedBy = ((SMS_User)_UserInfo).UserId;
                        _Country.CreatedDate = DateTime.Now;

                        // For Insert 
                        dbSetup.Countries.Add(_Country);
                        dbSetup.SaveChanges();
                    }
                }
                else
                {
                    _List = dbSetup.Countries.ToList();
                }
            }
            return Json(new { Country = _List, _UserRoles }, JsonRequestBehavior.AllowGet);
        }

        // CRUD Operation For Country  
        public JsonResult CRUDCity(IList<City> objCity, string _Mode)
        {
            City _City = new City();
            var _List = new object();

            var _UserRoles = Session["UserRoleByForm"];
            var _UserInfo = Session["UserInfo"];

            if (ObjUserRole != null)
            {
                if (objCity != null)
                {
                    if (objCity.Count > 0)
                    {
                        // Set Properties in Object  
                        _City = objCity[0];
                        _City.MakerWorkStationId = Dns.GetHostName();
                    }
                    if (_Mode == "D")
                    {
                        // For Delete 
                        dbSetup.Countries.Remove(dbSetup.Countries.Find(_City.CityId));
                        dbSetup.SaveChanges();
                    }
                    else if (objCity.Count > 0 && objCity[0].CityId > 0)
                    {
                        _City.UpdatedBy = ((SMS_User)_UserInfo).UserId;
                        _City.UpdatedDate = DateTime.Now;

                        // For Update
                        dbSetup.Entry(_City).State = EntityState.Modified;
                        dbSetup.SaveChanges();
                    }
                    else if (_Mode == "I")
                    {
                        _City.CreatedBy = ((SMS_User)_UserInfo).UserId;
                        _City.CreatedDate = DateTime.Now;

                        // For Insert 
                        dbSetup.Cities.Add(_City);
                        dbSetup.SaveChanges();
                    }
                }
                else
                {
                    _List = dbSetup.Cities.ToList();
                }
            }
            return Json(new { City = _List, _UserRoles }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Functional_Forms

        #endregion



    }
}