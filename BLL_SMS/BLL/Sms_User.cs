using BLL_SMS.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BLL_SMS.BLL
{
    public class Sms_User
    {
        // Updated by Umar Farooque 
        DBBridge ObjDb = new DBBridge();
        public int RowId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public int DeptCode { get; set; }
        public int EmpCode { get; set; }
        public int LoginLimit { get; set; }
        public int GroupId { get; set; }
        public int UserChangePassword { get; set; }
        public int FirstLoginId { get; set; }

        public DateTime LoginTImeFrom { get; set; }
        public DateTime LoginTimeTo { get; set; }
        public int LogedNo { get; set; }
        public string Picture { get; set; }
        public string MakerWorkStationId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public int LoginId { get; set; }

        public DataSet CRUD_User(Sms_User ObjUser)
        {

            SqlParameter[] param = new SqlParameter[19];
            param[0] = new SqlParameter("@RowId", ObjUser.RowId);
            param[1] = new SqlParameter("@UserId", ObjUser.UserId);
            param[2] = new SqlParameter("@FirstName", ObjUser.FirstName);
            param[3] = new SqlParameter("@LastName", ObjUser.LastName);
            param[4] = new SqlParameter("@Password", ObjUser.Password);
            param[5] = new SqlParameter("@Email", ObjUser.Email);
            param[6] = new SqlParameter("@Status", ObjUser.Status);
            param[7] = new SqlParameter("@DeptCode", ObjUser.DeptCode);
            param[8] = new SqlParameter("@EmpCode", ObjUser.EmpCode);
            param[9] = new SqlParameter("@LoginLimit", ObjUser.LoginLimit);
            param[10] = new SqlParameter("@GroupId", ObjUser.GroupId);
            param[11] = new SqlParameter("@UserChangePassword", ObjUser.UserChangePassword);
            param[12] = new SqlParameter("@FirstLoginId", ObjUser.FirstLoginId);
            param[13] = new SqlParameter("@LoginTImeFrom", "");// ObjUser.LoginTImeFrom 
            param[14] = new SqlParameter("@LoginTimeTo", "");  //ObjUser.LoginTimeTo 
            param[15] = new SqlParameter("@LogedNo", ObjUser.LogedNo);
            param[16] = new SqlParameter("@Picture", ObjUser.Picture);
            param[17] = new SqlParameter("@MakerWorkStationId", ObjUser.MakerWorkStationId);
            param[18] = new SqlParameter("@LoginId", ObjUser.UserId);
            DataSet ds = ObjDb.ExecuteDataset("sp_sms_user", param);
            return ds;
        }


        public Sms_User GetUserList(DataTable ObjUser)
        {

            Sms_User UserInfo = new Sms_User();
            foreach (DataRow dr in ObjUser.Rows)
            {
                UserInfo = new Sms_User()
                {

                    RowId = Convert.ToInt32(dr["RowId"])
                    ,
                    UserId = Convert.ToInt32(dr["UserId"])
                    ,
                    FirstName = dr["FirstName"].ToString()
                    ,
                    LastName = dr["LastName"].ToString()
                    ,
                    Password = dr["Password"].ToString()
                    ,
                    Email = dr["Email"].ToString()
                    ,
                    Status = Convert.ToBoolean(dr["Status"])
                    ,
                    DeptCode = Convert.ToInt32(dr["DeptCode"].ToString())
                    ,
                    EmpCode = Convert.ToInt32(dr["EmpCode"])
                    ,
                    LoginLimit = Convert.ToInt32(dr["LoginLimit"])
                    ,
                    GroupId = Convert.ToInt32(dr["GroupId"])
                    ,
                    UserChangePassword = Convert.ToInt32(dr["UserChangePassword"])
                    ,
                    FirstLoginId = Convert.ToInt32(dr["FirstLoginId"])
                    ,
                    LoginTImeFrom = Convert.ToDateTime(dr["LoginTImeFrom"])
                    ,
                    LoginTimeTo = Convert.ToDateTime(dr["LoginTimeTo"])
                    ,
                    LogedNo = Convert.ToInt32(dr["LogedNo"])
                    ,
                    Picture = dr["Picture"].ToString()
                    ,
                    MakerWorkStationId = dr["MakerWorkStationId"].ToString()
                    ,
                    CreatedBy = Convert.ToInt32(dr["CreatedBy"])
                    ,
                    CreatedDate = dr["CreatedDate"].ToString()
                    ,
                    UpdatedBy = Convert.ToInt32(dr["UpdatedBy"])
                    ,
                    UpdatedDate = dr["UpdatedDate"].ToString()
                };

            }

            return UserInfo;
        }

    }
}
