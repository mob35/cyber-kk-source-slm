using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SLM.Application.Utilities;
using SLM.Biz;
using SLM.Resource.Data;
using log4net;

namespace SLM.Application
{
    public partial class SLM_SCR_028 : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(SLM_SCR_028));

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ((Label)Page.Master.FindControl("lblTopic")).Text = "ค้นหาข้อมูลสาขา";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ScreenPrivilegeData priData = RoleBiz.GetScreenPrivilege(HttpContext.Current.User.Identity.Name, "SLM_SCR_028");
                    if (priData == null || priData.IsView != 1)
                    {
                        AppUtil.ClientAlertAndRedirect(Page, "คุณไม่มีสิทธิ์เข้าใช้หน้าจอนี้", "SLM_SCR_003.aspx");
                        return;
                    }

                    Page.Form.DefaultButton = btnSearch.UniqueID;
                    InitialControl();
                    DoSearchBranch(0);
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void InitialControl()
        {
            AppUtil.SetIntTextBox(txtWorkStartHourPopup);
            AppUtil.SetIntTextBox(txtWorkStartMinPopup);
            AppUtil.SetIntTextBox(txtWorkEndHourPopup);
            AppUtil.SetIntTextBox(txtWorkEndMinPopup);
            AppUtil.SetTimeControlScript(txtWorkStartHourPopup, txtWorkStartMinPopup);
            AppUtil.SetTimeControlScript(txtWorkEndHourPopup, txtWorkEndMinPopup);

            //Search
            cmbChannelSearch.DataSource = ChannelBiz.GetChannelList();
            cmbChannelSearch.DataTextField = "TextField";
            cmbChannelSearch.DataValueField = "ValueField";
            cmbChannelSearch.DataBind();
            cmbChannelSearch.Items.Insert(0, new ListItem("", ""));

            //Popup
            cmbChannelPopup.DataSource = ChannelBiz.GetChannelList();
            cmbChannelPopup.DataTextField = "TextField";
            cmbChannelPopup.DataValueField = "ValueField";
            cmbChannelPopup.DataBind();
            cmbChannelPopup.Items.Insert(0, new ListItem("", ""));
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DoSearchBranch(0);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void DoSearchBranch(int pageIndex)
        {
            try
            {
                List<BranchData> list = BranchBiz.SearchBranch(txtBranchCodeSearch.Text.Trim(), txtBranchNameSearch.Text.Trim(), cmbChannelSearch.SelectedItem.Value, cbActiveSearch.Checked, cbInActiveSearch.Checked);
                BindGridview(pcTop, list.ToArray(), pageIndex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Page Control

        private void BindGridview(SLM.Application.Shared.GridviewPageController pageControl, object[] items, int pageIndex)
        {
            pageControl.SetGridview(gvResult);
            pageControl.Update(items, pageIndex);
            pageControl.GenerateRecordNumber(1, pageIndex);
            upResult.Update();
        }

        protected void PageSearchChange(object sender, EventArgs e)
        {
            try
            {
                var pageControl = (SLM.Application.Shared.GridviewPageController)sender;
                DoSearchBranch(pageControl.SelectedPageIndex);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        #endregion

        #region Popup

        protected void btnAddBranch_Click(object sender, EventArgs e)
        {
            try
            {
                cbEdit.Checked = false;
                txtBranchCodePopup.Text = "";
                txtBranchCodePopup.Enabled = true;
                txtBranchNamePopup.Text = "";
                cmbChannelPopup.SelectedIndex = -1;
                txtWorkStartHourPopup.Text = "";
                txtWorkStartMinPopup.Text = "";
                txtWorkEndHourPopup.Text = "";
                txtWorkEndMinPopup.Text = "";
                rbActive.Checked = true;
                rbInActive.Checked = false;

                upPopup.Update();
                mpePopup.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    if (cbEdit.Checked)
                        BranchBiz.UpdateData(txtBranchCodePopup.Text.Trim(), txtBranchNamePopup.Text.Trim(), txtWorkStartHourPopup.Text.Trim(), txtWorkStartMinPopup.Text.Trim(), txtWorkEndHourPopup.Text.Trim(), txtWorkEndMinPopup.Text.Trim(), cmbChannelPopup.SelectedItem.Value, rbActive.Checked, HttpContext.Current.User.Identity.Name);
                    else
                        BranchBiz.InsertData(txtBranchCodePopup.Text.Trim(), txtBranchNamePopup.Text.Trim(), txtWorkStartHourPopup.Text.Trim(), txtWorkStartMinPopup.Text.Trim(), txtWorkEndHourPopup.Text.Trim(), txtWorkEndMinPopup.Text.Trim(), cmbChannelPopup.SelectedItem.Value, rbActive.Checked, HttpContext.Current.User.Identity.Name);

                    AppUtil.ClientAlert(Page, "บันทึกข้อมูลเรียบร้อย");

                    ClearPopupControl();
                    mpePopup.Hide();

                    DoSearchBranch(0);
                }
                else
                    mpePopup.Show();
            }
            catch (Exception ex)
            {
                mpePopup.Show();
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        #endregion      

        protected void imbEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                var branch = BranchBiz.GetBranch(((ImageButton)sender).CommandArgument);
                if (branch != null)
                {
                    cbEdit.Checked = true;
                    txtBranchCodePopup.Text = branch.BranchCode;
                    txtBranchCodePopup.Enabled = false;
                    txtBranchNamePopup.Text = branch.BranchName;
                    cmbChannelPopup.SelectedIndex = cmbChannelPopup.Items.IndexOf(cmbChannelPopup.Items.FindByValue(branch.ChannelId));
                    txtWorkStartHourPopup.Text = branch.StartTimeHour;
                    txtWorkStartMinPopup.Text = branch.StartTimeMinute;
                    txtWorkEndHourPopup.Text = branch.EndTimeHour;
                    txtWorkEndMinPopup.Text = branch.EndTimeMinute;

                    rbActive.Checked = branch.Status == "Y" ? true : false;
                    rbInActive.Checked = branch.Status == "Y" ? false : true;

                    upPopup.Update();
                    mpePopup.Show();
                }
                else
                    throw new Exception("ไม่พบรหัสสาขา " + ((ImageButton)sender).CommandArgument + " ในระบบ");
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearPopupControl();
            mpePopup.Hide();
        }

        private void ClearPopupControl()
        {
            cbEdit.Checked = false;
            txtBranchCodePopup.Text = "";
            txtBranchCodePopup.Enabled = true;
            txtBranchNamePopup.Text = "";
            cmbChannelPopup.SelectedIndex = -1;
            txtWorkStartHourPopup.Text = "";
            txtWorkStartMinPopup.Text = "";
            txtWorkEndHourPopup.Text = "";
            txtWorkEndMinPopup.Text = "";
            rbActive.Checked = true;
            rbInActive.Checked = false;

            alertBranchCodePopup.Text = "";
            alertBranchNamePopup.Text = "";
            alertWorkStartTime.Text = "";
            alertWorkEndTime.Text = "";
            alertStatus.Text = "";
        }

        private bool ValidateInput()
        {
            int i = 0;
            int branchCodeMaxLength = 100;
            int branchNameMaxLength = 500;
            bool starttimeOK = false;
            bool endtimeOK = false;

            if (cbEdit.Checked == false && txtBranchCodePopup.Text.Trim() == "")
            {
                alertBranchCodePopup.Text = "กรุณาระบุ รหัสสาขา";
                i += 1;
            }
            else
            {
                if (txtBranchCodePopup.Text.Trim().Length > branchCodeMaxLength)
                {
                    alertBranchCodePopup.Text = "กรุณาระบุ รหัสสาขาไม่เกิน " + branchCodeMaxLength.ToString() + " ตัวอักษร";
                    i += 1;
                }
                else
                    alertBranchCodePopup.Text = "";
            }


            if (txtBranchNamePopup.Text.Trim() == "")
            {
                alertBranchNamePopup.Text = "กรุณาระบุ ชื่อสาขา";
                i += 1;
            }
            else
            {
                if (txtBranchNamePopup.Text.Trim().Length > branchNameMaxLength)
                {
                    alertBranchNamePopup.Text = "กรุณาระบุ ชื่อสาขาไม่เกิน " + branchNameMaxLength.ToString() + " ตัวอักษร";
                    i += 1;
                }
                else
                    alertBranchNamePopup.Text = "";
            }

            if (txtWorkStartHourPopup.Text.Trim() == "" || txtWorkStartMinPopup.Text.Trim() == "")
            {
                alertWorkStartTime.Text = "กรุณาระบุ เวลาทำการเริ่มต้นให้ครบ";
                i += 1;
            }
            else
            {
                starttimeOK = true;
                alertWorkStartTime.Text = "";
            }

            if (txtWorkEndHourPopup.Text.Trim() == "" || txtWorkEndMinPopup.Text.Trim() == "")
            {
                alertWorkEndTime.Text = "กรุณาระบุ เวลาทำการสิ้นสุดให้ครบ";
                i += 1;
            }
            else
            {
                endtimeOK = true;
                alertWorkEndTime.Text = "";
            }

            if (starttimeOK && endtimeOK)
            {
                int start = int.Parse(txtWorkStartHourPopup.Text.Trim() + txtWorkStartMinPopup.Text.Trim());
                int end = int.Parse(txtWorkEndHourPopup.Text.Trim() + txtWorkEndMinPopup.Text.Trim());

                if (start >= end)
                {
                    alertWorkStartTime.Text = "เวลาทำการเริ่มต้นต้องน้อยกว่าเวลาทำการสิ้นสุด";
                    i += 1;
                }
                else
                    alertWorkStartTime.Text = "";
            }

            if (cbEdit.Checked && rbInActive.Checked)
            {
                if (BranchBiz.CheckEmployeeInBranch(txtBranchCodePopup.Text.Trim()))
                {
                    alertStatus.Text = "ไม่สามารถปิดสาขาได้ เนื่องจากยังมีพนักงานอยู่ในสาขา";
                    i += 1;
                }
                else
                    alertStatus.Text = "";
            }

            return i > 0 ? false : true;
        }
    }
}