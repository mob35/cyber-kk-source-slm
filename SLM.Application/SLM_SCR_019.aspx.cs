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
    public partial class SLM_SCR_019 : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(SLM_SCR_018));
        private string ss_staffid = "staffid";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ((Label)Page.Master.FindControl("lblTopic")).Text = "เพิ่มข้อมูลพนักงาน";
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
                    ScreenPrivilegeData priData = RoleBiz.GetScreenPrivilege(HttpContext.Current.User.Identity.Name, "SLM_SCR_019");
                    if (priData == null || priData.IsView != 1)
                    {
                        AppUtil.ClientAlertAndRedirect(Page, "คุณไม่มีสิทธิ์เข้าใช้หน้าจอนี้", "SLM_SCR_003.aspx");
                        return;
                    }

                    InitialControl();
                    //SetDept();
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

            //Head Staff Branch
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

        //private void SetDept()
        //{
        //    decimal? stafftype = SlmScr019Biz.GetStaffTypeData(HttpContext.Current.User.Identity.Name);
        //    if (stafftype != null)
        //    {
        //        if (stafftype == SLMConstant.StaffType.ITAdministrator)
        //            cmbDepartment.Enabled = true;
        //        else
        //        {
        //            cmbDepartment.Enabled = false;
        //            int? dept = SlmScr019Biz.GetDeptData(HttpContext.Current.User.Identity.Name);
        //            if (dept != null)
        //            {
        //                cmbDepartment.SelectedIndex =  cmbDepartment.Items.IndexOf(cmbDepartment.Items.FindByValue(dept.ToString()));
        //            }
        //        }
        //    }
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateData())
                {
                    StaffDataManagement data = new StaffDataManagement();
                    data.Username = txtUserName.Text.Trim();
                    data.EmpCode = txtEmpCode.Text.Trim();
                    data.MarketingCode = txtMarketingCode.Text.Trim();
                    data.StaffNameTH = txtStaffNameTH.Text.Trim();
                    data.TelNo = txtTellNo.Text.Trim();
                    data.StaffEmail = txtStaffEmail.Text.Trim();
                    data.PositionId = int.Parse(cmbPosition.SelectedItem.Value);
                    data.StaffTypeId = decimal.Parse(cmbStaffType.SelectedItem.Value);
                    data.Team = txtTeam.Text.Trim();
                    data.BranchCode = cmbBranchCode.SelectedItem.Value;
                    if (cmbHeadStaffId.Items.Count > 0 && !string.IsNullOrEmpty(cmbHeadStaffId.SelectedItem.Value))
                    {
                        data.HeadStaffId = int.Parse(cmbHeadStaffId.SelectedItem.Value);
                    }
                    if (cmbDepartment.Items.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(cmbDepartment.SelectedItem.Value)) { data.DepartmentId = int.Parse(cmbDepartment.SelectedItem.Value); }
                    }

                    string staffId = SlmScr019Biz.InsertStaff(data, HttpContext.Current.User.Identity.Name);
                    Session[ss_staffid] = staffId;
                    AppUtil.ClientAlertAndRedirect(Page, "บันทึกข้อมูลเจ้าหน้าที่สำเร็จ", "SLM_SCR_018.aspx");
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
            if (txtUserName.Text.Trim() == "")
            {
                vtxtUserName.Text = "กรุณาระบุ Windows Username";
                vtxtUserName.ForeColor = System.Drawing.Color.Red;
                i += 1;
            }
            else
            {
                vtxtUserName.Text = "";
                if (SlmScr019Biz.CheckUsernameExist(txtUserName.Text.Trim(),null))
                {
                    vtxtUserName.Text = "Windows Username นี้มีอยู่แล้วในระบบแล้ว";
                    vtxtUserName.ForeColor = System.Drawing.Color.Red;
                    i += 1;
                }
                else
                    vtxtUserName.Text = "";
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
                if (SlmScr019Biz.CheckEmpCodeExist(txtEmpCode.Text.Trim(),null))
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
                if (SlmScr019Biz.CheckMarketingCodeExist(txtMarketingCode.Text.Trim(),null))
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

            //************************************สาขา********************************************
            if (cmbBranchCode.SelectedItem.Value == "")
            {
                vcmbBranchCode.Text = "กรุณาระบุ สาขา";
                i += 1;
            }
            else
                vcmbBranchCode.Text = "";

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
                AppUtil.ClientAlert(Page, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        private bool ValidateEmail()
        {
            string pattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(pattern);
            return reg.IsMatch(txtStaffEmail.Text.Trim());
        }

        protected void cmbHeadBranchCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbHeadStaffId.DataSource = StaffBiz.GetHeadStaffList(cmbHeadBranchCode.SelectedItem.Value);
                cmbHeadStaffId.DataTextField = "TextField";
                cmbHeadStaffId.DataValueField = "ValueField";
                cmbHeadStaffId.DataBind();
                cmbHeadStaffId.Items.Insert(0, new ListItem("", ""));

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


        protected void btnCheckUsername_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUserName.Text.Trim() != string.Empty)
                {
                    vtxtUserName.Text = "";

                    if (SlmScr019Biz.CheckUsernameExist(txtUserName.Text.Trim(),null))
                    {
                        vtxtUserName.Text = "Windows Username นี้มีอยู่แล้วในระบบแล้ว";
                        vtxtUserName.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        vtxtUserName.Text = "Windows Username ใช้งานได้";
                        vtxtUserName.ForeColor = System.Drawing.Color.Green;
                    }
                }
                else
                    vtxtUserName.Text = "";
            }
            catch (Exception ex)
            {
                AppUtil.ClientAlert(Page, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("SLM_SCR_017.aspx");
            }
            catch (Exception ex)
            {
                AppUtil.ClientAlert(Page, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }
    }
}