using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SLM.Application.Utilities;
using log4net;
using SLM.Biz;
using SLM.Resource.Data;
using SLM.Resource;

namespace SLM.Application
{
    public partial class SLM_SCR_018 : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(SLM_SCR_018));
        private string ss_staffid = "staffid";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ((Label)Page.Master.FindControl("lblTopic")).Text = "แก้ไขข้อมูลพนักงาน";
            Page.Form.DefaultButton = btnSave.UniqueID;
            AppUtil.SetIntTextBox(txtEmpCode);
            AppUtil.SetIntTextBox(txtMarketingCode);
            AppUtil.SetIntTextBox(txtTellNo);
            txtEmpCode.Attributes.Add("OnBlur", "ChkIntOnBlurClear(this)");
            txtMarketingCode.Attributes.Add("OnBlur", "ChkIntOnBlurClear(this)");
            txtTellNo.Attributes.Add("OnBlur", "ChkIntOnBlurClear(this)");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ScreenPrivilegeData priData = RoleBiz.GetScreenPrivilege(HttpContext.Current.User.Identity.Name, "SLM_SCR_018");
                    if (priData == null || priData.IsView != 1)
                    {
                        AppUtil.ClientAlertAndRedirect(Page, "คุณไม่มีสิทธิ์เข้าใช้หน้าจอนี้", "SLM_SCR_003.aspx");
                        return;
                    }

                    if (Session[ss_staffid] != null)
                    {
                        txtStaffId.Text = Session[ss_staffid].ToString();
                        InitialControl();
                        LoadStaffData();
                        //GetCampaignData();
                        tabOwner.Username = txtUsername.Text.Trim();
                        tabOwner.Update();

                        tabDelegate.Username = txtUsername.Text.Trim();
                        tabDelegate.Update();

                        tabOwner.GetOwnerList();
                        tabDelegate.GetDelegateList();
                        //SetDept();
                    }
                    else
                    {
                        if (txtStaffId.Text.Trim() == string.Empty)
                            AppUtil.ClientAlertAndRedirect(Page, "Staff Id not found", "SLM_SCR_017.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        //private void SetDept()
        //{
        //    //decimal? stafftype = SlmScr019Biz.GetStaffTypeData(HttpContext.Current.User.Identity.Name);
        //    //if (stafftype != null)
        //    //{
        //    //    if (stafftype == SLMConstant.StaffType.ITAdministrator)
        //    //        cmbDepartment.Enabled = true;
        //    //    else
        //    //    {
        //    cmbDepartment.Enabled = false;
        //        //}
        //    //}
        //}

        private void InitialControl()
        {
            //Role
            cmbStaffType.DataSource = SlmScr017Biz.GetStaffTyeData();
            cmbStaffType.DataTextField = "TextField";
            cmbStaffType.DataValueField = "ValueField";
            cmbStaffType.DataBind();
            cmbStaffType.Items.Insert(0, new ListItem("", ""));

            //Market Branch
            cmbBranchCode.DataSource = BranchBiz.GetBranchList(SLMConstant.Branch.Active);
            cmbBranchCode.DataTextField = "TextField";
            cmbBranchCode.DataValueField = "ValueField";
            cmbBranchCode.DataBind();
            cmbBranchCode.Items.Insert(0, new ListItem("", ""));

            //Market Branch
            cmbHeadBranchCode.DataSource = BranchBiz.GetBranchList(SLMConstant.Branch.Active);
            cmbHeadBranchCode.DataTextField = "TextField";
            cmbHeadBranchCode.DataValueField = "ValueField";
            cmbHeadBranchCode.DataBind();
            cmbHeadBranchCode.Items.Insert(0, new ListItem("", ""));

            //Department
            cmbDepartment.DataSource = SlmScr017Biz.GetDeptData();
            cmbDepartment.DataTextField = "TextField";
            cmbDepartment.DataValueField = "ValueField";
            cmbDepartment.DataBind();
            cmbDepartment.Items.Insert(0, new ListItem("", ""));

            //Position
            cmbPosition.DataSource = PositionBiz.GetPositionList(SLMConstant.Position.Active);
            cmbPosition.DataTextField = "TextField";
            cmbPosition.DataValueField = "ValueField";
            cmbPosition.DataBind();
            cmbPosition.Items.Insert(0, new ListItem("", ""));
        }

        private void LoadStaffData()
        {
            try
            {
                StaffDataManagement staff = new StaffDataManagement();
                if (txtStaffId.Text.Trim() != "")
                    staff = SlmScr018Biz.GetStaffData(int.Parse(txtStaffId.Text.Trim()));

                if (staff != null)
                {
                    txtUsername.Text = staff.Username;
                    lblUsername.Text = staff.Username;
                    txtEmpCode.Text = staff.EmpCode;
                    txtMarketingCode.Text = staff.MarketingCode;
                    txtStaffNameTH.Text = staff.StaffNameTH;
                    txtTellNo.Text = staff.TelNo;
                    txtStaffEmail.Text = staff.StaffEmail;
                    if (staff.PositionId != null)
                        cmbPosition.SelectedIndex = cmbPosition.Items.IndexOf(cmbPosition.Items.FindByValue(staff.PositionId.ToString()));
                    if (staff.StaffTypeId != null)
                        cmbStaffType.SelectedIndex = cmbStaffType.Items.IndexOf(cmbStaffType.Items.FindByValue(staff.StaffTypeId.ToString()));

                    txtTeam.Text = staff.Team;

                    if (!string.IsNullOrEmpty(staff.BranchCode))
                    {
                        ListItem item = cmbBranchCode.Items.FindByValue(staff.BranchCode);
                        if (item != null)
                            cmbBranchCode.SelectedIndex = cmbBranchCode.Items.IndexOf(item);
                        else
                        {
                            //Branch ที่ถูกปิด
                            string branchName = BranchBiz.GetBranchName(staff.BranchCode);
                            if (!string.IsNullOrEmpty(branchName))
                            {
                                cmbBranchCode.Items.Insert(1, new ListItem(branchName, staff.BranchCode));
                                cmbBranchCode.SelectedIndex = 1;
                            }
                        }
                    }

                    txtOldBranchCode.Text = staff.BranchCode;

                    if (staff.HeadStaffId != null)
                    {
                        string branchCode = StaffBiz.GetBranchCode(staff.HeadStaffId.Value);
                        if (!string.IsNullOrEmpty(branchCode))
                        {
                            ListItem item = cmbHeadBranchCode.Items.FindByValue(branchCode);
                            if (item != null)
                                cmbHeadBranchCode.SelectedIndex = cmbHeadBranchCode.Items.IndexOf(item);
                            else
                            {
                                //Branch ที่ถูกปิด
                                string branchName = BranchBiz.GetBranchName(branchCode);
                                if (!string.IsNullOrEmpty(branchName))
                                {
                                    cmbHeadBranchCode.Items.Insert(1, new ListItem(branchName, branchCode));
                                    cmbHeadBranchCode.SelectedIndex = 1;
                                }
                            }
                        }

                        cmbHeadBranchCode_SelectedIndexChanged();

                        cmbHeadStaffId.SelectedIndex = cmbHeadStaffId.Items.IndexOf(cmbHeadStaffId.Items.FindByValue(staff.HeadStaffId.Value.ToString()));
                        cmbHeadStaffId.Enabled = true;
                        lblHeadStaffId.Text = "*";
                    }

                    if (staff.DepartmentId != null)
                        cmbDepartment.SelectedIndex = cmbDepartment.Items.IndexOf(cmbDepartment.Items.FindByValue(staff.DepartmentId.ToString()));

                    if (staff.Is_Deleted != null)
                    {
                        txtOldIsDeleted.Text = staff.Is_Deleted.ToString();
                        txtNewIsDeleted.Text = staff.Is_Deleted.ToString();
                        if (staff.Is_Deleted == 0)
                        {
                            rdNormal.Checked = true;
                        }
                        else if (staff.Is_Deleted == 1)
                        {
                            rdRetire.Checked = true;
                        }
                        else
                        {
                            rdNormal.Checked = false;
                            rdRetire.Checked = false;
                        }
                    }
                }
                upInfo.Update();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void cmbHeadBranchCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbHeadBranchCode_SelectedIndexChanged();
                if (cmbHeadBranchCode.SelectedItem.Value != "")
                {
                    lblHeadStaffId.Text = "*";
                    cmbHeadStaffId.Enabled = true;
                }
                else
                {
                    vcmbHeadStaffId.Text = "";
                    lblHeadStaffId.Text = "";
                    cmbHeadStaffId.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void cmbHeadBranchCode_SelectedIndexChanged()
        {
            var list = StaffBiz.GetHeadStaffList(cmbHeadBranchCode.SelectedItem.Value);
            var editedStaff = list.Where(p => p.ValueField == txtStaffId.Text.Trim()).FirstOrDefault();
            list.Remove(editedStaff);

            cmbHeadStaffId.DataSource = list;
            cmbHeadStaffId.DataTextField = "TextField";
            cmbHeadStaffId.DataValueField = "ValueField";
            cmbHeadStaffId.DataBind();
            cmbHeadStaffId.Items.Insert(0, new ListItem("", ""));
        }


        protected void btnSavePopup_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateData())
                {
                }
                else
                {
                    AppUtil.ClientAlert(Page, "กรุณาระบุข้อมูลให้ครบถ้วน");
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }
        private bool ValidateData()
        {
            int i = 0;
            //************************************Windows Username********************************************
            if (txtUsername.Text.Trim() == "")
            {
                vtxtUsername.Text = "กรุณาระบุ Windows Username";
                vtxtUsername.ForeColor = System.Drawing.Color.Red;
                i += 1;
            }
            else
            {
                vtxtUsername.Text = "";
                if (SlmScr019Biz.CheckUsernameExist(txtUsername.Text.Trim(), int.Parse(txtStaffId.Text.Trim())))
                {
                    vtxtUsername.Text = "Windows Username นี้มีอยู่แล้วในระบบแล้ว";
                    vtxtUsername.ForeColor = System.Drawing.Color.Red;
                    i += 1;
                }
                else
                    vtxtUsername.Text = "";
            }

            //************************************รหัสพนักงานธนาคาร********************************************
            if (txtEmpCode.Text.Trim() == "")
            {
                vtxtEmpCode.Text = "กรุณาระบุรหัสพนักงานธนาคาร";
                i += 1;
            }
            else
            {
                vtxtEmpCode.Text = "";
                if (SlmScr019Biz.CheckEmpCodeExist(txtEmpCode.Text.Trim(), int.Parse(txtStaffId.Text.Trim())))
                {
                    vtxtEmpCode.Text = "รหัสพนักงานธนาคารนี้มีอยู่แล้วในระบบแล้ว";
                    i += 1;
                }
                else
                    vtxtEmpCode.Text = "";
            }

            //************************************รหัสเจ้าหน้าที่การตลาด********************************************
            if (txtMarketingCode.Text.Trim() == "")
            {
                //vtxtMarketingCode.Text = "กรุณาระบุรหัสเจ้าหน้าที่การตลาด";
                //i += 1;
            }
            else
            {
                vtxtMarketingCode.Text = "";
                if (SlmScr019Biz.CheckMarketingCodeExist(txtMarketingCode.Text.Trim(), int.Parse(txtStaffId.Text.Trim())))
                {
                    vtxtMarketingCode.Text = "รหัสเจ้าหน้าที่การตลาดนี้มีอยู่แล้วในระบบแล้ว";
                    i += 1;
                }
                else
                    vtxtMarketingCode.Text = "";
            }

            //************************************ชื่อ-นามสกุลพนักงาน********************************************
            if (txtStaffNameTH.Text.Trim() == "")
            {
                vtxtStaffNameTH.Text = "กรุณาระบุชื่อ-นามสกุลพนักงาน";
                i += 1;
            }
            else
                vtxtStaffNameTH.Text = "";

            //************************************E-mail********************************************
            if (txtStaffEmail.Text.Trim() == "")
            {
                vtxtStaffEmail.Text = "กรุณาระบุ E-mail";
                i += 1;
            }
            else
            {
                if (!ValidateEmail())
                {
                    vtxtStaffEmail.Text = "กรุณาระบุ E-mail ให้ถูกต้อง";
                    i += 1;
                }
                else
                    vtxtStaffEmail.Text = "";
            }

            //************************************ตำแหน่ง********************************************
            if (cmbPosition.SelectedItem.Value == "")
            {
                vtxtPositionName.Text = "กรุณาระบุ ตำแหน่ง";
                i += 1;
            }
            else
                vtxtPositionName.Text = "";

            //************************************Role********************************************
            if (cmbStaffType.SelectedItem.Value == "")
            {
                vcmbStaffType.Text = "กรุณาระบุ Role";
                i += 1;
            }
            else
                vcmbStaffType.Text = "";

            //************************************ทีมการตลาด********************************************
            //if (txtTeam.Text.Trim() == "")
            //{
            //    vtxtTeam.Text = "กรุณาระบุ ทีมการตลาด";
            //    i += 1;
            //}
            //else
            //    vtxtTeam.Text = "";

            //************************************สาขาพนักงาน********************************************

            if (cmbBranchCode.SelectedItem.Value == "")
            {
                vcmbBranchCode.Text = "กรุณาระบุ สาขา";
                i += 1;
            }
            else
            {
                if (cmbBranchCode.Items.Count > 0 && cmbBranchCode.SelectedItem.Value != "" && !BranchBiz.CheckBranchActive(cmbBranchCode.SelectedItem.Value))
                {
                    vcmbBranchCode.Text = "สาขานี้ถูกปิดแล้ว";
                    i += 1;
                }
                else
                    vcmbBranchCode.Text = "";
            }

            //************************************สาขาหัวหน้างาน********************************************

            if (cmbHeadBranchCode.Items.Count > 0 && cmbHeadBranchCode.SelectedItem.Value != "" && !BranchBiz.CheckBranchActive(cmbHeadBranchCode.SelectedItem.Value))
            {
                vcmbHeadBranchCode.Text = "สาขานี้ถูกปิดแล้ว";
                i += 1;
            }
            else
                vcmbHeadBranchCode.Text = "";

            //************************************หัวหน้างาน********************************************

            if (cmbHeadBranchCode.Items.Count > 0 && cmbHeadBranchCode.SelectedItem.Value != "" && cmbHeadStaffId.SelectedItem.Value == "")
            {
                vcmbHeadStaffId.Text = "กรุณาระบุ หัวหน้างาน";
                i += 1;
            }
            else
                vcmbHeadStaffId.Text = "";

            if (i > 0)
                return false;
            else
                return true;
        }

        protected void txtEmail_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (ValidateEmail() == false && txtStaffEmail.Text.Trim() != "")
                    vtxtStaffEmail.Text = "กรุณาระบุ E-mail ให้ถูกต้อง";
                else
                    vtxtStaffEmail.Text = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ValidateEmail()
        {
            string pattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(pattern);
            return reg.IsMatch(txtStaffEmail.Text.Trim());

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                vcmbBranchCode.Text = "";
                vcmbHeadBranchCode.Text = "";
                string desc;
                if (cmbBranchCode.SelectedItem.Value != txtOldBranchCode.Text.Trim())
                {
                    if (SlmScr018Biz.CheckExistLeadOnHand(txtUsername.Text.Trim()))
                    {
                        AppUtil.ClientAlert(Page, "ไม่สามารถเปลี่ยนข้อมูลสาขาได้ เนื่องจากยังมีงานค้างอยู่");
                        return;
                    }
                }
                else if (txtOldIsDeleted.Text.Trim() != txtNewIsDeleted.Text.Trim())
                {
                    if (SlmScr018Biz.CheckExistLeadOnHand(txtUsername.Text.Trim()))
                    {
                        AppUtil.ClientAlert(Page, "ไม่สามารถเปลี่ยนสถานะพนักงานได้ เนื่องจากยังมีงานค้างอยู่");
                        return;
                    }
                    if (ChannelBiz.CheckUserErrorInUse(txtUsername.Text.Trim()))
                    {
                        AppUtil.ClientAlert(Page, "ไม่สามารถเปลี่ยนสถานะพนักงานได้ เนื่องจากพนักงานนี้ถูกกำหนดเป็นผู้รับผิดชอบ Channel (Table: kkslm_ms_channel) กรุณาแจ้ง IT เพื่อทำการเซ็ทค่าออก");
                        return;
                    }
                }
                else if (ChannelBiz.CheckHeadStaff(txtStaffId.Text, cmbHeadStaffId.SelectedValue.Trim(), out desc))
                {
                    AppUtil.ClientAlert(Page, "ไม่สามารถเปลี่ยนหัวหน้างานได้เนื่องจาก " + txtStaffNameTH.Text + "เป็นหัวหน้างาน" + cmbHeadStaffId.Text.Trim());
                    return;
                }
                //else if (ChannelBiz.CheckUnderStaff(txtStaffId.Text, cmbHeadStaffId.SelectedValue.Trim(), out desc))
                //{
                //    AppUtil.ClientAlert(Page, "ไม่สามารถเปลี่ยนหัวหน้างานได้เนื่องจากหัวหน้างานเป็นผู้ใต้บังคับัญชา");
                //        return;
                //}

                SaveData();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void SaveData()
        {
            try
            {
                if (ValidateData())
                {
                    int flag = 0;
                    StaffDataManagement data = new StaffDataManagement();
                    data.Username = txtUsername.Text.Trim();
                    data.EmpCode = txtEmpCode.Text.Trim();
                    data.MarketingCode = txtMarketingCode.Text.Trim();
                    data.StaffNameTH = txtStaffNameTH.Text.Trim();
                    data.TelNo = txtTellNo.Text.Trim();
                    data.StaffEmail = txtStaffEmail.Text.Trim();
                    data.PositionId = int.Parse(cmbPosition.SelectedItem.Value);
                    data.StaffTypeId = decimal.Parse(cmbStaffType.SelectedItem.Value);
                    data.Team = txtTeam.Text.Trim();
                    data.BranchCode = cmbBranchCode.SelectedItem.Value;
                    data.StaffId = int.Parse(txtStaffId.Text.Trim());
                    if (rdNormal.Checked == true)
                        data.Is_Deleted = 0;
                    else if (rdRetire.Checked == true)
                        data.Is_Deleted = 1;

                    if (cmbHeadStaffId.Items.Count > 0 && cmbHeadStaffId.SelectedItem.Value != "")
                        data.HeadStaffId = int.Parse(cmbHeadStaffId.SelectedItem.Value);
                    else
                        data.HeadStaffId = null;

                    if (cmbDepartment.Items.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(cmbDepartment.SelectedItem.Value)) { data.DepartmentId = int.Parse(cmbDepartment.SelectedItem.Value); }
                    }

                    if (txtOldIsDeleted.Text.Trim() != txtNewIsDeleted.Text.Trim())
                    {
                        flag = 1;
                    }

                    string staffId = SlmScr019Biz.UpdateStaff(data, HttpContext.Current.User.Identity.Name, flag);

                    AppUtil.ClientAlert(Page, "บันทึกข้อมูลเจ้าหน้าที่สำเร็จ");
                    txtStaffId.Text = staffId;
                    InitialControl();
                    LoadStaffData();
                    //SetDept();
                    upInfo.Update();
                }
                else
                {
                    AppUtil.ClientAlert(Page, "กรุณาระบุข้อมูลให้ครบถ้วน");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                Session[ss_staffid] = null;
                Response.Redirect("~/SLM_SCR_017.aspx");
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                AppUtil.ClientAlert(Page, message);
            }
        }



        protected void rdNormal_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtNewIsDeleted.Text = "0";
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void rdRetire_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtNewIsDeleted.Text = "1";
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                AppUtil.ClientAlert(Page, message);
            }
        }

        //public void UpdateData(string tabName)
        //{
        //    if (tabName == "tabOwner")
        //        tabOwner.GetOwnerList();
        //    else
        //        tabDelegate.GetDelegateList();
        //}

        #region Backup

        //private void GetCampaignData()
        //{
        //    List<StaffGroupData> sList = SlmScr018Biz.GetStaffGroupData(txtStaffId.Text.Trim());
        //    gvCampaign.DataSource = sList;
        //    gvCampaign.DataBind();
        //    upCampaign.Update();
        //}
        //protected void btnAdd_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        mpePopup.Show();
        //        GetCampaignDataPopup(0);
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        //        AppUtil.ClientAlert(Page, message);
        //    }
        //}
        //private void GetCampaignDataPopup(int pageIndex)
        //{
        //    List<CampaignWSData> campaign = SlmScr018Biz.GetCampaignData(txtCampaignIdList.Text.Trim());
        //    BindGridview((SLM.Application.Shared.GridviewPageController)pcTop, campaign.ToArray(), pageIndex);
        //    upCampaignPopup.Update();
        //}

        //private void BindGridview(SLM.Application.Shared.GridviewPageController pageControl, object[] items, int pageIndex)
        //{
        //    pageControl.SetGridview(gvSearchCampaign);
        //    pageControl.Update(items, pageIndex);
        //    upCampaignPopup.Update();
        //}
        //protected void PageSearchChange(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var pageControl = (SLM.Application.Shared.GridviewPageController)sender;
        //        GetCampaignDataPopup(pageControl.SelectedPageIndex);
        //        mpePopup.Show();
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        //        _log.Debug(message);
        //        AppUtil.ClientAlert(Page, message);
        //    }
        //}

        //protected void gvCampaign_DataBound(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string camList = "";
        //        if (gvCampaign.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < gvCampaign.Rows.Count; i++)
        //            {
        //                Label lblCampaignId = (Label)gvCampaign.Rows[i].FindControl("lblCampaignId");
        //                if (lblCampaignId != null)
        //                {
        //                    camList = (camList == "" ? camList : camList + ",") + lblCampaignId.Text.Trim();
        //                }
        //            }
        //        }
        //        txtCampaignIdList.Text = camList;
        //        upCampaign.Update();
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        //        _log.Debug(message);
        //        AppUtil.ClientAlert(Page, message);
        //    }
        //}
        //protected void btnOK_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        List<StaffGroupData> ListStaffGroup = new List<StaffGroupData>();
        //        if (gvSearchCampaign.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < gvSearchCampaign.Rows.Count; i++)
        //            {
        //                CheckBox chkSelect = (CheckBox)gvSearchCampaign.Rows[i].FindControl("chkSelect");
        //                if (chkSelect != null)
        //                {
        //                    if (chkSelect.Checked == true)
        //                    {
        //                        StaffGroupData sData = new StaffGroupData();
        //                        sData.StaffId = int.Parse(txtStaffId.Text.Trim());
        //                        sData.CampaignId = gvSearchCampaign.Rows[i].Cells[1].Text;
        //                        ListStaffGroup.Add(sData);
        //                    }
        //                }
        //            }
        //            if (ListStaffGroup.Count == 0)
        //            {
        //                AppUtil.ClientAlert(Page, "กรุณาระบุแคมเปญอย่างน้อย 1 รายการ");
        //                mpePopup.Show();
        //                return;
        //            }
        //            else
        //            {
        //                SlmScr018Biz.InsertStaffGroup(ListStaffGroup, HttpContext.Current.User.Identity.Name);
        //                GetCampaignData();
        //                upCampaign.Update();
        //                AppUtil.ClientAlert(Page, "บันทึกข้อมูลแคมเปญเรียบร้อย");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        mpePopup.Show();
        //        string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        //        _log.Debug(message);
        //        AppUtil.ClientAlert(Page, message);
        //    }
        //}

        //protected void imbDelete_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        ImageButton imbDelete = (ImageButton)sender;
        //        SlmScr018Biz.DeleteStaffGroup(decimal.Parse(imbDelete.CommandArgument), HttpContext.Current.User.Identity.Name);
        //        GetCampaignData();
        //        upCampaign.Update();
        //        AppUtil.ClientAlert(Page, "ลบข้อมูลแคมเปญเรียบร้อย");
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        //        _log.Debug(message);
        //        AppUtil.ClientAlert(Page, message);
        //    }

        //}

        #endregion
    }
}