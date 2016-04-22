using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SLM.Application.Utilities;
using SLM.Resource.Data;
using SLM.Resource;
using SLM.Biz;
using log4net;

namespace SLM.Application.Shared.Tabs
{
    public partial class Tab008 : System.Web.UI.UserControl
    {
        public delegate void UpdatedDataEvent(string statusDesc);
        public event UpdatedDataEvent UpdatedDataChanged;
        private static readonly ILog _log = LogManager.GetLogger(typeof(Tab008));
        private string _currentAssignedFlag = "";
        private string _currentDelegateFlag = "";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            AppUtil.SetIntTextBox(txtTicketID);
            AppUtil.SetIntTextBox(txtContactPhone);
            txtContactPhone.Attributes.Add("OnBlur", "ChkIntOnBlurClear(this)");
            AppUtil.SetMultilineMaxLength(txtContactDetail, vtxtContactDetail.ClientID, "500");
            AppUtil.SetIntTextBox(txtCitizenId);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
            }
        }

        public void InitialControl(LeadData data)
        {
            try
            {
                //For Search Gridview
                txtTicketIdSearch.Text = data.TicketId;
                txtCitizenIdSearch.Text = data.CitizenId;
                txtTelNo1Search.Text = data.TelNo_1;

                if (data.ISCOC == "1" && data.COCCurrentTeam != SLMConstant.COCTeam.Marketing)
                    btnAddResultContact.Visible = false;
                else
                    CheckActivityConfig(data.ProductId, data.Status);

                pcTop.SetVisible = false;
                DoBindGridview(0);

                HideDebugControl();     //ซ่อน Control สำหรับ Debug
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void HideDebugControl()
        {
            txtTicketIdSearch.Visible = false;
            txtCitizenIdSearch.Visible = false;
            txtTelNo1Search.Visible = false;
            txtAssignedFlag.Visible = false;
            txtDelegateFlag.Visible = false;
            txtCampaignId.Visible = false;
            txtProductId.Visible = false;
            txtOldOwnerBranch.Visible = false;
            txtOldOwner.Visible = false;
            txtOldDelegateBranch.Visible = false;
            txtOldDelegate.Visible = false;
            txtOldStatus.Visible = false;
        }

        private void CheckActivityConfig(string productId, string leadStatus)
        {
            try
            {
                List<ActivityConfigData> list = ActivityConfigBiz.GetActivityConfig(productId, leadStatus);
                if (list.Count > 0)
                {
                    bool? rightAdd = list.Select(p => p.HaveRightAdd).FirstOrDefault();
                    btnAddResultContact.Visible = (rightAdd == true ? true : false);
                }
                else
                    btnAddResultContact.Visible = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void chkthis_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                DoBindGridview(0);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void DoBindGridview(int pageIndex)
        {
            var result = SlmScr008Biz.SearchPhoneCallHistory(txtCitizenIdSearch.Text.Trim(), txtTicketIdSearch.Text.Trim(), cbThisLead.Checked);
            BindGridview((SLM.Application.Shared.GridviewPageController)pcTop, result.ToArray(), pageIndex);
            upResult.Update();
        }

        #region Page Control

        private void BindGridview(SLM.Application.Shared.GridviewPageController pageControl, object[] items, int pageIndex)
        {
            pageControl.SetGridview(gvPhoneCallHistoty);
            pageControl.Update(items, pageIndex);
            upResult.Update();
        }

        protected void PageSearchChange(object sender, EventArgs e)
        {
            try
            {
                var pageControl = (SLM.Application.Shared.GridviewPageController)sender;
                DoBindGridview(pageControl.SelectedPageIndex);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        #endregion

        private void InitialDropdownlist(LeadDataPhoneCallHistory data)
        {
            try
            {
                //ประเภทบุคคล
                cmbCardType.DataSource = CardTypeBiz.GetCardTypeList();
                cmbCardType.DataTextField = "TextField";
                cmbCardType.DataValueField = "ValueField";
                cmbCardType.DataBind();
                cmbCardType.Items.Insert(0, new ListItem("", ""));

                cmbOwnerBranch.DataSource = SlmScr010Biz.GetBranchListByAccessRight(SLMConstant.Branch.Active, data.CampaignId);
                cmbOwnerBranch.DataTextField = "TextField";
                cmbOwnerBranch.DataValueField = "ValueField";
                cmbOwnerBranch.DataBind();
                cmbOwnerBranch.Items.Insert(0, new ListItem("", ""));
                cmbOwnerBranchSelectedIndexChanged();       //Do not remove

                cmbDelegateBranch.DataSource = SlmScr010Biz.GetBranchListByAccessRight(SLMConstant.Branch.Active, data.CampaignId);
                cmbDelegateBranch.DataTextField = "TextField";
                cmbDelegateBranch.DataValueField = "ValueField";
                cmbDelegateBranch.DataBind();
                cmbDelegateBranch.Items.Insert(0, new ListItem("", ""));
                cmbDelegateBranchSelectedIndexChanged();    //Do not remove

                //Status
                cmbLeadStatus.DataSource = OptionBiz.GetStatusListByActivityConfig(data.ProductId, data.LeadStatus);
                cmbLeadStatus.DataTextField = "TextField";
                cmbLeadStatus.DataValueField = "ValueField";
                cmbLeadStatus.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnAddResultContact_Click(object sender, EventArgs e)
        {
            try
            {
                var data = LeadInfoBiz.GetLeadDataPhoneCallHistory(txtTicketIdSearch.Text.Trim());
                if (data != null)
                {
                    txtTicketID.Text = data.TicketId;
                    txtFirstname.Text = data.Name;
                    txtLastname.Text = data.LastName;
                    txtProductId.Text = data.ProductId;
                    txtCampaignId.Text = data.CampaignId;
                    txtCampaign.Text = data.CampaignName;
                    txtTelNo1.Text = data.TelNo1;                  
                    txtCitizenId.Text = data.CitizenId;
                    txtAssignedFlag.Text = data.AssignedFlag;
                    txtDelegateFlag.Text = data.DelegateFlag != null ? data.DelegateFlag.ToString() : "";

                    InitialDropdownlist(data);

                    if (data.CardType != null)
                    {
                        cmbCardType.SelectedIndex = cmbCardType.Items.IndexOf(cmbCardType.Items.FindByValue(data.CardType.Value.ToString()));
                        lblCitizenId.Text = "*";
                        txtCitizenId.Enabled = true;
                        txtCitizenId.Text = data.CitizenId;
                        AppUtil.SetCardTypeValidation(cmbCardType.SelectedItem.Value, txtCitizenId);
                    }
                    else
                        txtCitizenId.Text = data.CitizenId;

                    //Owner Branch
                    if (!string.IsNullOrEmpty(data.OwnerBranch))
                    {
                        txtOldOwnerBranch.Text = data.OwnerBranch;
                        ListItem item = cmbOwnerBranch.Items.FindByValue(data.OwnerBranch);
                        if (item != null)
                            cmbOwnerBranch.SelectedIndex = cmbOwnerBranch.Items.IndexOf(item);
                        else
                        {
                            //check ว่ามีการกำหนด Brach ใน Table kkslm_ms_Access_Right ไหม ถ้ามีจะเท่ากับเป็น Branch ที่ถูกปิด ถ้าไม่มีแปลว่าไม่มีการเซตการมองเห็น
                            if (SlmScr011Biz.CheckBranchAccessRightExist(SLMConstant.Branch.All, txtCampaignId.Text.Trim(), data.OwnerBranch))
                            {
                                //Branch ที่ถูกปิด
                                string branchName = BranchBiz.GetBranchName(data.OwnerBranch);
                                if (!string.IsNullOrEmpty(branchName))
                                {
                                    cmbOwnerBranch.Items.Insert(1, new ListItem(branchName, data.OwnerBranch));
                                    cmbOwnerBranch.SelectedIndex = 1;
                                }
                            }
                        }

                        cmbOwnerBranchSelectedIndexChanged();   //Bind Combo Owner
                    }

                    //Owner
                    if (!string.IsNullOrEmpty(data.Owner))
                    {
                        txtOldOwner.Text = data.Owner;
                        cmbOwner.SelectedIndex = cmbOwner.Items.IndexOf(cmbOwner.Items.FindByValue(data.Owner));
                    }

                    //Delegate Branch
                    if (!string.IsNullOrEmpty(data.DelegateBranch))
                    {
                        txtOldDelegateBranch.Text = data.DelegateBranch;
                        ListItem item = cmbDelegateBranch.Items.FindByValue(data.DelegateBranch);
                        if (item != null)
                            cmbDelegateBranch.SelectedIndex = cmbDelegateBranch.Items.IndexOf(item);
                        else
                        {
                            //check ว่ามีการกำหนด Brach ใน Table kkslm_ms_Access_Right ไหม ถ้ามีจะเท่ากับเป็น Branch ที่ถูกปิด ถ้าไม่มีแปลว่าไม่มีการเซตการมองเห็น
                            if (SlmScr011Biz.CheckBranchAccessRightExist(SLMConstant.Branch.All, txtCampaignId.Text.Trim(), data.DelegateBranch))
                            {
                                //Branch ที่ถูกปิด
                                string branchName = BranchBiz.GetBranchName(data.DelegateBranch);
                                if (!string.IsNullOrEmpty(branchName))
                                {
                                    cmbDelegateBranch.Items.Insert(1, new ListItem(branchName, data.DelegateBranch));
                                    cmbDelegateBranch.SelectedIndex = 1;
                                }
                            }
                        }

                        cmbDelegateBranchSelectedIndexChanged();    //Bind Combo Delegate
                    }

                    if (!string.IsNullOrEmpty(data.Delegate))
                    {
                        txtOldDelegate.Text = data.Delegate;
                        cmbDelegate.SelectedIndex = cmbDelegate.Items.IndexOf(cmbDelegate.Items.FindByValue(data.Delegate));
                    }

                    //Lead Status
                    if (cmbLeadStatus.Items.Count > 0)
                    {
                        cmbLeadStatus.SelectedIndex = cmbLeadStatus.Items.IndexOf(cmbLeadStatus.Items.FindByValue(data.LeadStatus));
                        txtOldStatus.Text = data.LeadStatus;
                    }

                    cmbLeadStatus.Enabled = data.AssignedFlag == "1" ? true : false;
                    if (data.AssignedFlag == "1" && data.LeadStatus != "00")
                    {
                        cmbLeadStatus.Items.Remove(cmbLeadStatus.Items.FindByValue("00"));  //ถ้าจ่ายงานแล้ว และสถานะปัจจุบันไม่ใช่สนใจ ให้เอาสถานะ สนใจ ออก
                    }

                    //เช็กสิทธิการแก้ไขข้อมูล
                    if (txtAssignedFlag.Text.Trim() == "0" || txtDelegateFlag.Text.Trim() == "1")   //ยังไม่จ่ายงาน assignedFlag = 0, delegateFlag = 1
                    {
                        cmbDelegateBranch.Enabled = false;
                        cmbDelegate.Enabled = false;
                        cmbOwnerBranch.Enabled = false;
                        cmbOwner.Enabled = false;
                        lblTab008Info.Text = "ไม่สามารถแก้ไข Owner และ Delegate ได้ เนื่องจากอยู่ระหว่างรอระบบจ่ายงาน กรุณารอ 1 นาที";
                    }
                    else
                        AppUtil.CheckOwnerPrivilege(data.Owner, data.Delegate, cmbOwnerBranch, cmbOwner, cmbDelegateBranch, cmbDelegate);

                    upPopup.Update();
                    mpePopup.Show();
                }
                else
                    AppUtil.ClientAlert(Page, "ไม่พบ Ticket Id " + txtTicketID.Text.Trim() + " ในระบบ");
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void BindOwnerLead()
        {
            cmbOwner.DataSource = StaffBiz.GetStaffList(cmbOwnerBranch.SelectedItem.Value);
            cmbOwner.DataTextField = "TextField";
            cmbOwner.DataValueField = "ValueField";
            cmbOwner.DataBind();
            cmbOwner.Items.Insert(0, new ListItem("", ""));
        }

        protected void cmbOwnerBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbOwnerBranchSelectedIndexChanged();
                if (cmbOwnerBranch.SelectedItem.Value != string.Empty && cmbOwner.SelectedItem.Value == string.Empty)
                {
                    vcmbOwner.Text = "กรุณาระบุ owner lead";
                }
                else
                    vcmbOwner.Text = "";

                mpePopup.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void cmbOwnerBranchSelectedIndexChanged()
        {
            try
            {
                //Owner Lead
                List<ControlListData> source = StaffBiz.GetStaffAllDataByAccessRight(txtCampaignId.Text.Trim(), cmbOwnerBranch.SelectedItem.Value);
                //คำนวณงานในมือ
                AppUtil.CalculateAmountJobOnHandForDropdownlist(cmbOwnerBranch.SelectedItem.Value, source);
                cmbOwner.DataSource = source;
                cmbOwner.DataTextField = "TextField";
                cmbOwner.DataValueField = "ValueField";
                cmbOwner.DataBind();
                cmbOwner.Items.Insert(0, new ListItem("", ""));

                if (cmbOwnerBranch.SelectedItem.Value != string.Empty)
                    cmbOwner.Enabled = true;
                else
                    cmbOwner.Enabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void cmbDelegateBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbDelegateBranchSelectedIndexChanged();
                if (cmbDelegateBranch.SelectedItem.Value != string.Empty && cmbDelegate.SelectedItem.Value == string.Empty)
                {
                    vcmbDelegate.Text = "กรุณาระบุ Delegate Lead";
                }
                else
                    vcmbDelegate.Text = "";

                mpePopup.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void cmbDelegateBranchSelectedIndexChanged()
        {
            try
            {
                //Delegate Lead
                List<ControlListData> source = StaffBiz.GetStaffAllDataByAccessRight(txtCampaignId.Text.Trim(), cmbDelegateBranch.SelectedItem.Value);
                AppUtil.CalculateAmountJobOnHandForDropdownlist(cmbDelegateBranch.SelectedItem.Value, source);
                cmbDelegate.DataSource = source;
                cmbDelegate.DataTextField = "TextField";
                cmbDelegate.DataValueField = "ValueField";
                cmbDelegate.DataBind();
                cmbDelegate.Items.Insert(0, new ListItem("", ""));

                if (cmbDelegateBranch.SelectedItem.Value != string.Empty)
                    cmbDelegate.Enabled = true;
                else
                    cmbDelegate.Enabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ClearData();
                mpePopup.Hide();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void ClearData()
        {
            txtTicketID.Text = "";
            txtAssignedFlag.Text = "";          //Hidden
            txtDelegateFlag.Text = "";          //Hidden
            txtFirstname.Text = "";
            txtLastname.Text = "";
            cmbCardType.SelectedIndex = -1;
            txtCitizenId.Text = "";
            txtCampaign.Text = "";
            txtCampaignId.Text = "";            //Hidden
            txtProductId.Text = "";             //Hidden
            cmbOwnerBranch.SelectedIndex = -1;
            txtOldOwnerBranch.Text = "";        //Hidden
            cmbOwner.SelectedIndex = -1;
            txtOldOwner.Text = "";              //Hidden
            cmbDelegateBranch.SelectedIndex = -1;
            txtOldDelegateBranch.Text = "";     //Hidden
            cmbDelegate.SelectedIndex = -1;
            txtOldDelegate.Text = "";           //Hidden
            txtTelNo1.Text = "";
            cmbLeadStatus.SelectedIndex = -1;
            txtOldStatus.Text = "";             //Hidden
            txtContactPhone.Text = "";
            txtContactDetail.Text = "";

            lblCitizenId.Text = "";
            vtxtCitizenId.Text = "";
            vcmbOwner.Text = "";
            vcmbOwnerBranch.Text = "";
            vcmbDelegate.Text = "";
            vcmbDelegateBranch.Text = "";
            vtxtContactDetail.Text = "";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> flagList = LeadInfoBiz.GetAssignedFlagAndDelegateFlag(txtTicketID.Text.Trim());
                _currentAssignedFlag = flagList[0];
                _currentDelegateFlag = flagList[1];

                if (cmbOwnerBranch.Items.Count > 0 && cmbOwner.Items.Count > 0)
                {
                    if (cmbOwnerBranch.SelectedItem.Value != txtOldOwnerBranch.Text.Trim() || cmbOwner.SelectedItem.Value != txtOldOwner.Text.Trim())
                    {
                        if (_currentAssignedFlag != txtAssignedFlag.Text.Trim())
                        {
                            AppUtil.ClientAlertAndRedirect(Page, "ไม่สามารถบันทึกผลการติดต่อได้ เนื่องจากมีคนเปลี่ยน Owner รบกวนรอ 1 นาที แล้วกลับมาบันทึกผลการติดต่อได้", "SLM_SCR_004.aspx?ticketid=" + txtTicketID.Text.Trim() + "&tab=008");
                            return;
                        }
                    }
                }

                if (cmbDelegateBranch.Items.Count > 0 && cmbDelegate.Items.Count > 0)
                {
                    if (cmbDelegateBranch.SelectedItem.Value != txtOldDelegateBranch.Text.Trim() || cmbDelegate.SelectedItem.Value != txtOldDelegate.Text.Trim())
                    {
                        if (_currentDelegateFlag != txtDelegateFlag.Text.Trim())
                        {
                            AppUtil.ClientAlertAndRedirect(Page, "ไม่สามารถบันทึกผลการติดต่อได้ เนื่องจากมีคนเปลี่ยน Delegate รบกวนรอ 1 นาที แล้วกลับมาบันทึกผลการติดต่อได้", "SLM_SCR_004.aspx?ticketid=" + txtTicketID.Text.Trim() + "&tab=008");
                            return;
                        }
                    }
                }

                if (ValidateData())
                {
                    SlmScr008Biz.InsertPhoneCallHistory(txtTicketID.Text.Trim(), cmbCardType.SelectedItem.Value, txtCitizenId.Text.Trim(), cmbLeadStatus.SelectedItem.Value, txtOldStatus.Text.Trim(), cmbOwnerBranch.SelectedItem.Value, cmbOwner.SelectedItem.Value, txtOldOwner.Text.Trim()
                        , cmbDelegateBranch.SelectedItem.Value, cmbDelegate.SelectedItem.Value, txtOldDelegate.Text.Trim(), txtContactPhone.Text.Trim(), txtContactDetail.Text.Trim(), HttpContext.Current.User.Identity.Name);

                    txtTicketIdSearch.Text = txtTicketID.Text.Trim();
                    txtCitizenIdSearch.Text = txtCitizenId.Text.Trim();
                    txtTelNo1Search.Text = txtTelNo1.Text.Trim();

                    DoBindGridview(0);
                    CheckActivityConfig(txtProductId.Text.Trim(), cmbLeadStatus.SelectedItem.Value);

                    if (UpdatedDataChanged != null) UpdatedDataChanged(cmbLeadStatus.SelectedItem.Text);

                    ClearData();
                    mpePopup.Hide();
                    AppUtil.ClientAlert(Page, "บันทึกข้อมูลเรียบร้อย");
                }
                else
                    mpePopup.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }


        protected void cmbCardType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCardType.SelectedItem.Value == "")
                {
                    vtxtCardType.Text = "";
                    lblCitizenId.Text = "";
                    vtxtCitizenId.Text = "";
                    txtCitizenId.Text = "";
                    txtCitizenId.Enabled = false;
                }
                else
                {
                    vtxtCardType.Text = "";
                    lblCitizenId.Text = "*";
                    vtxtCitizenId.Text = "";
                    txtCitizenId.Enabled = true;
                    AppUtil.SetCardTypeValidation(cmbCardType.SelectedItem.Value, txtCitizenId);
                    AppUtil.ValidateCardId(cmbCardType, txtCitizenId, vtxtCitizenId);
                }

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

        protected void txtCitizenId_TextChanged(object sender, EventArgs e)
        {
            try
            {
                AppUtil.ValidateCardId(cmbCardType, txtCitizenId, vtxtCitizenId);
                mpePopup.Show();
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
            try
            {
                int i = 0;

                //เช็กสถานะ require cardId
                if (LeadInfoBiz.CheckRequireCardId(cmbLeadStatus.SelectedItem.Value))
                {
                    if (cmbCardType.SelectedItem.Value == "")
                    {
                        i += 1;
                        vtxtCardType.Text = "กรุณาระบุประเภทบุคคล";
                    }
                    if (txtCitizenId.Text.Trim() == "")
                    {
                        i += 1;
                        vtxtCitizenId.Text = "กรุณาระบุเลขที่บัตร";
                    }

                    if (cmbCardType.SelectedItem.Value != "" && txtCitizenId.Text.Trim() != "")
                    {
                        if (!AppUtil.ValidateCardId(cmbCardType, txtCitizenId, vtxtCitizenId))
                            i += 1;
                    }
                }
                else
                {
                    //Validate เลขที่บัตร
                    if (cmbCardType.SelectedItem.Value != "")
                    {
                        if (txtCitizenId.Text.Trim() == "")
                        {
                            vtxtCitizenId.Text = "กรุณาระบุเลขที่บัตร";
                            i += 1;
                        }
                        else
                        {
                            if (!AppUtil.ValidateCardId(cmbCardType, txtCitizenId, vtxtCitizenId))
                                i += 1;
                        }
                    }
                    else if (cmbCardType.SelectedItem.Value == "" && txtCitizenId.Text.Trim() != "")
                    {
                        vtxtCardType.Text = "กรุณาระบุประเภทบุคคล";
                        i += 1;
                    }
                    else
                    {
                        vtxtCardType.Text = "";
                        vtxtCitizenId.Text = "";
                    }
                }

                //OwnerBranch, Owner
                string clearOwnerBranchText = "Y";
                if (cmbOwnerBranch.SelectedItem.Value != txtOldOwnerBranch.Text.Trim() || cmbOwner.SelectedItem.Value != txtOldOwner.Text.Trim())
                {
                    if (!AppUtil.ValidateOwner(_currentAssignedFlag, cmbOwnerBranch, vcmbOwnerBranch, cmbOwner, vcmbOwner, txtCampaignId.Text.Trim(), ref clearOwnerBranchText))
                        i += 1;

                    //Branch ที่ถูกปิด
                    if (cmbOwnerBranch.Items.Count > 0 && cmbOwnerBranch.SelectedItem.Value != "" && !BranchBiz.CheckBranchActive(cmbOwnerBranch.SelectedItem.Value))
                    {
                        vcmbOwnerBranch.Text = "สาขานี้ถูกปิดแล้ว";
                        i += 1;
                    }
                    else
                    {
                        if (clearOwnerBranchText == "Y")
                            vcmbOwnerBranch.Text = "";
                    }
                }
                else
                {
                    vcmbOwnerBranch.Text = "";
                    vcmbOwner.Text = "";
                }

                //DelegateBranch, Delegate
                if (cmbDelegateBranch.SelectedItem.Value != txtOldDelegateBranch.Text.Trim() || cmbDelegate.SelectedItem.Value != txtOldDelegate.Text.Trim())
                {
                    if (cmbDelegateBranch.SelectedItem.Value != string.Empty && cmbDelegate.SelectedItem.Value == string.Empty)
                    {
                        vcmbDelegate.Text = "กรุณาระบุ Delegate Lead";
                        i += 1;
                    }
                    else
                        vcmbDelegate.Text = "";

                    if (cmbDelegateBranch.Items.Count > 0 && cmbDelegateBranch.SelectedItem.Value != "" && !BranchBiz.CheckBranchActive(cmbDelegateBranch.SelectedItem.Value))
                    {
                        vcmbDelegateBranch.Text = "สาขานี้ถูกปิดแล้ว";
                        i += 1;
                    }
                    else
                        vcmbDelegateBranch.Text = "";
                }
                else
                {
                    vcmbDelegateBranch.Text = "";
                    vcmbDelegate.Text = "";
                }

                //รายละเอียดเพิ่มเติม
                if (txtContactDetail.Text.Trim() == "")
                {
                    vtxtContactDetail.Text = "กรุณากรอกข้อมูลรายละเอียดก่อนทำการบันทึก";
                    i += 1;
                }
                else
                    vtxtContactDetail.Text = "";

                upPopup.Update();

                return i > 0 ? false : true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void cmbOwner_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbOwnerBranch.SelectedItem.Value != string.Empty && cmbOwner.SelectedItem.Value == string.Empty)
                {
                    vcmbOwner.Text = "กรุณาระบุ owner lead";
                }
                else
                {
                    vcmbOwner.Text = "";
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }
        protected void cmbDelegateLead_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDelegateBranch.SelectedItem.Value != string.Empty && cmbDelegate.SelectedItem.Value == string.Empty)
                {
                    vcmbDelegate.Text = "กรุณาระบุ Delegate Lead";
                }
                else
                {
                    vcmbDelegate.Text = "";
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }
    }
}