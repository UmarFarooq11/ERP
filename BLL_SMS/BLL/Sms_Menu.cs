using BLL_SMS.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient; 

namespace BLL_SMS.BLL
{
    public class Sms_Menu
    {
        DBBridge ObjDb = new DBBridge();
        public int MenuId { get; set; }
        public int ParentId { get; set; }
        public Nullable<int> ChildId { get; set; }
        public string MenuName { get; set; }
        public string Action { get; set; }
        public string Icon { get; set; }
        public Nullable<bool> Status { get; set; }
        public string MakerWorkStationId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public List<Sms_Menu> MenuList { get; set; }


        public DataTable GetMenuList(int ChildId) {

            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@childid", ChildId); 
            return ObjDb.ExecuteDataset("sp_get_menu", param).Tables[0]; ;
        }

    }
}
