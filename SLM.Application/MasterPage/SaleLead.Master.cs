using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using SLM.Biz;
using SLM.Application.Utilities;
using SLM.Resource.Data;
using log4net;

namespace SLM.Application.MasterPage
{
    public partial class SaleLead : System.Web.UI.MasterPage
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(SaleLead));

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        DisplayUserFullname();
                        SetAvailableButton();
                        SetMenu();
                    }
                    else
                        Response.Redirect(FormsAuthentication.LoginUrl);
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void DisplayUserFullname()
        {
            FormsIdentity identity = (FormsIdentity)HttpContext.Current.User.Identity;
            FormsAuthenticationTicket ticket = identity.Ticket;
            string[] data = ticket.UserData.Split('|');
            lblUserFullname.Text = data[0];
            lblBranchName.Text = data[1];
            txtUsername.Text = ticket.Name;

            //HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
            //FormsAuthenticationTicket decTicket = FormsAuthentication.Decrypt(cookie.Value);
            //lblUserFullname.Text = decTicket.UserData;
        }

        private void SetMenu()
        {
            try
            {
                var staffTypeId = SlmMasterBiz.GetStaffTypeId(HttpContext.Current.User.Identity.Name);
                if (staffTypeId != null)
                {
                    List<ScreenPrivilegeData> list = SlmMasterBiz.GetScreenPrivillegeList(Convert.ToInt32(staffTypeId));

                    menuUserMonitoring.Visible = (list.Where(p => p.ScreenId == 4 && p.IsView == 1).Count() > 0);       //UserMonitoring
                    menuCampaignRecommend.Visible = (list.Where(p => p.ScreenId == 5 && p.IsView == 1).Count() > 0);    //แนะนำแคมเปญ
                    menuUserManagement.Visible = (list.Where(p => p.ScreenId == 6 && p.IsView == 1).Count() > 0);       //UserManagement
                    menuNotice.Visible = (list.Where(p => p.ScreenId == 20 && p.IsView == 1).Count() > 0);              //ข้อมูลประกาศ
                    menuPosition.Visible = (list.Where(p => p.ScreenId == 21 && p.IsView == 1).Count() > 0);            //ข้อมูลตำแหน่ง
                    menuSLA.Visible = (list.Where(p => p.ScreenId == 22 && p.IsView == 1).Count() > 0);                 //ข้อมูล SLA
                    menuAssign.Visible = (list.Where(p => p.ScreenId == 23 && p.IsView == 1).Count() > 0);              //ข้อมูล กำหนดค่าการจ่ายงาน
                    menuPrivilege.Visible = (list.Where(p => p.ScreenId == 24 && p.IsView == 1).Count() > 0);           //ข้อมูล สิทธิ์การเข้าถึงข้อมูล
                    menuActivityPrivilege.Visible = (list.Where(p => p.ScreenId == 25 && p.IsView == 1).Count() > 0);   //ข้อมูล กำหนดเงื่อนไขการบันทึกผลการติดต่อ
                    menuBranchHoliday.Visible = (list.Where(p => p.ScreenId == 26 && p.IsView == 1).Count() > 0);       //ข้อมูล กำหนดวันหยุดสาขา
                    menuBranch.Visible = (list.Where(p => p.ScreenId == 27 && p.IsView == 1).Count() > 0);              //ข้อมูล ข้อมูลสาขา
                    menuLead.Visible = (list.Where(p => p.ScreenId == 29 && p.IsView == 1).Count() > 0);                //ข้อมูล ค้นหา upload lead
                }
                else
                {
                    menuUserMonitoring.Visible = false;
                    menuCampaignRecommend.Visible = false;
                    menuUserManagement.Visible = false;
                    menuNotice.Visible = false;
                    menuPosition.Visible = false;
                    menuSLA.Visible = false;
                    menuAssign.Visible = false;
                    menuPrivilege.Visible = false;
                    menuActivityPrivilege.Visible = false;
                    menuBranchHoliday.Visible = false;
                    menuBranch.Visible = false;
                    menuLead.Visible = false;
                }
            }
            catch(Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
            }
        }

        protected void imbLogout_Click(object sender, ImageClickEventArgs e)
        {
            Session["searchcondition"] = null;
            Session["staffsearchcondition"] = null;
            FormsAuthentication.SignOut();
            Response.Redirect(FormsAuthentication.LoginUrl);
            //FormsAuthentication.RedirectToLoginPage();
        }

        protected void btnNotAvailable_Click(object sender, EventArgs e)
        {
            try
            {
                SetUserActiveStatus(0);
                SetUnavailable();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnAvailable_Click(object sender, EventArgs e)
        {
            try
            {
                SetUserActiveStatus(1);
                SetAvailable();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void lbSearchLead_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SLM_SCR_003.aspx");
        }

        protected void lbUserMonitoring_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SLM_SCR_015.aspx");
        }

        protected void lbCampaignRecommend_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SLM_SCR_016.aspx");
        }

        protected void lbUserManagement_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SLM_SCR_017.aspx");
        }

        protected void lbSuggesstiont_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SLM_SCR_020.aspx");
        }

        protected void lbNotice_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SLM_SCR_021.aspx");
        }

        protected void lbSLA_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SLM_SCR_022.aspx");
        }

        protected void lbAssign_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SLM_SCR_023.aspx");
        }

        protected void lbPrivilege_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SLM_SCR_024.aspx");
        }

        protected void lbActivityPrivilege_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SLM_SCR_025.aspx");
        }

        protected void lbPosition_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SLM_SCR_026.aspx");
        }

        protected void lbBranchHoliday_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SLM_SCR_027.aspx");
        }

        protected void lbBranch_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SLM_SCR_028.aspx");
        }
        protected void lbLead_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SLM_SCR_101.aspx");
        }
        private void SetAvailableButton()
        {
            string status = StaffBiz.GetActiveStatusByAvailableConfig(txtUsername.Text.Trim());
            if (status == "0")
                SetUnavailable();
            else if (status == "1")
                SetAvailable();
            else
                HideStatusDetail();          
        }

        private void HideStatusDetail()
        {
            imgAvailable.Visible = false;
            imgNotAvailable.Visible = false;
            lblStatusDesc.Text = string.Empty;
            btnAvailable.Visible = false;
            btnNotAvailable.Visible = false;
        }

        private void SetAvailable()
        {
            imgAvailable.Visible = true;
            imgNotAvailable.Visible = false;
            lblStatusDesc.Text = "<b>สถานะ : </b>พร้อมทำงาน (Available)";
            btnAvailable.Visible = false;
            btnNotAvailable.Visible = true;
        }

        private void SetUnavailable()
        {
            imgAvailable.Visible = false;
            imgNotAvailable.Visible = true;
            lblStatusDesc.Text = "<b>สถานะ : </b>ไม่พร้อมทำงาน (Unavailable)";
            btnAvailable.Visible = true;
            btnNotAvailable.Visible = false;
        }

        private void SetUserActiveStatus(int status)
        {
            StaffBiz.SetActiveStatus(txtUsername.Text.Trim(), status);
        }

        //private void SetCurrentStatus(int status)
        //{
        //    SlmMasterBiz.SetCurrentStatus(txtUsername.Text.Trim(), status);
        //}

        //protected void imbSuggesstion_Click(object sender, ImageClickEventArgs e)
        //{
        //    mpeSuggestion.Show();
        //}

        //protected void btnCancelSuggestion_Click(object sender, EventArgs e)
        //{
        //    mpeSuggestion.Hide();
        //}
    }
}