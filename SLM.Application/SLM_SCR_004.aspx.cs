using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SLM.Biz;
using SLM.Resource.Data;
using SLM.Application.Utilities;
using log4net;
using SLM.Resource;

namespace SLM.Application
{
    public partial class SLM_SCR_004 : System.Web.UI.Page
    {
        private LeadData lead = null;
        private static readonly ILog _log = LogManager.GetLogger(typeof(SLM_SCR_004));
        public string CMTUserName = System.Configuration.ConfigurationManager.AppSettings["CMTUserName"];
        public string CMTPassword = System.Configuration.ConfigurationManager.AppSettings["CMTPassword"];
        public string CMTServiceName = System.Configuration.ConfigurationManager.AppSettings["CMTServiceName"];
        public string CMTSystemCode = System.Configuration.ConfigurationManager.AppSettings["CMTSystemCode"];
        public string CMTReferenceNo = System.Configuration.ConfigurationManager.AppSettings["CMTReferenceNo"];
        public int CMTCampaignNo = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CMTCampaignNo"]);
        private string CMTCampaignSession = "CMTCampaignSession";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ScreenPrivilegeData priData = RoleBiz.GetScreenPrivilege(HttpContext.Current.User.Identity.Name, "SLM_SCR_004");
                    if (priData == null || priData.IsView != 1)
                    {
                        AppUtil.ClientAlertAndRedirect(Page, "คุณไม่มีสิทธิ์เข้าใช้หน้าจอนี้", "SLM_SCR_003.aspx");
                        return;
                    }

                    if (!string.IsNullOrEmpty(Request["ticketid"]))
                    {
                        txtTicketID.Text = Request["ticketid"].ToString();

                        if (!CheckTicketIdPrivilege(txtTicketID.Text.Trim())) { return; }

                        ((Label)Page.Master.FindControl("lblTopic")).Text = "แสดงข้อมูล Lead: " + txtTicketID.Text.Trim() +" (View)";

                        //เก็บไว้ส่งไปที่ Adam, AolSummaryReport
                        StaffData staff = SlmScr003Biz.GetStaff(HttpContext.Current.User.Identity.Name);
                        txtLoginEmpCode.Text = staff.EmpCode;
                        txtLoginStaffTypeId.Text = staff.StaffTypeId != null ? staff.StaffTypeId.ToString() : "";
                        txtLoginStaffTypeDesc.Text = staff.StaffTypeDesc;
                        txtLoginNameTH.Text = staff.StaffNameTH;
                        txtUserLoginChannelId.Text = staff.ChannelId;
                        txtUserLoginChannelDesc.Text = staff.ChannelDesc;

                        //Check สิทธิ์ภัทรในการเข้าใช้งาน
                        ConfigBranchPrivilegeData data = ConfigBranchPrivilegeBiz.GetConfigBranchPrivilege(staff.BranchCode);
                        if (data != null)
                        {
                            if (data.IsView != null && data.IsView.Value == false)
                            {
                                AppUtil.ClientAlertAndRedirect(Page, "คุณไม่มีสิทธิ์เข้าใช้หน้าจอนี้", "SLM_SCR_003.aspx");
                                return;
                            }
                        }
                        //------------------------------------------------------------------------------------------------------

                        GetLeadData();

                        if (lead.ISCOC == "1" && lead.COCCurrentTeam != SLMConstant.COCTeam.Marketing)
                        {
                            btnAllCampaign.Visible = false;
                        }
                        tabExistingLead.GetExistingLeadList(txtCitizenId.Text.Trim(), txtTelNo1.Text.Trim(), txtTicketID.Text.Trim());
                        tabExistingProduct.GetExistingProductList(txtCitizenId.Text.Trim());
                        tabOwnerLogging.GetOwnerLogingList(txtTicketID.Text.Trim());
                        tabPhoneCallHistory.InitialControl(lead);
                        tabNoteHistory.InitialControl(lead);

                        if (!string.IsNullOrEmpty(Request["tab"]) && Request["tab"] == "008")
                            tabMain.ActiveTabIndex = 4;

                        upTabMain.Update();
                    }
                    else
                        AppUtil.ClientAlertAndRedirect(Page, "Ticket Id not found", "SLM_SCR_003.aspx");
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private bool CheckTicketIdPrivilege(string ticketId)
        {
            if (!RoleBiz.GetTicketIdPrivilege(ticketId, HttpContext.Current.User.Identity.Name, SlmScr003Biz.GetStaffType(HttpContext.Current.User.Identity.Name)))
            {
                string message = "ข้อมูลผู้มุ่งหวังรายนี้ ท่านไม่มีสิทธิในการมองเห็น";
                LeadOwnerDelegateData data = SlmScr011Biz.GetOwnerAndDelegateName(ticketId);
                if (data != null)
                {
                    if (!string.IsNullOrEmpty(data.OwnerName) && !string.IsNullOrEmpty(data.DelegateName))
                        message += " ณ ปัจจุบันผู้เป็นเจ้าของ คือ " + data.OwnerName.ToString().Trim() + " และ Delegate คือ " + data.DelegateName.ToString().Trim();
                    else if (!string.IsNullOrEmpty(data.OwnerName))
                        message += " ณ ปัจจุบันผู้เป็นเจ้าของ คือ " + data.OwnerName.ToString().Trim();
                    else if (!string.IsNullOrEmpty(data.DelegateName))
                        message += " ณ ปัจจุบัน Delegate คือ " + data.DelegateName.ToString().Trim();
                }
                else
                    message = "ไม่พบ Ticket Id " + Request["ticketid"].ToString() + " ในระบบ";

                AppUtil.ClientAlertAndRedirect(Page, message, "SLM_SCR_003.aspx");
                return false;
            }
            else
                return true;
        }

        public void UpdateStatusDesc(string statusDesc)
        {
            txtstatus.Text = statusDesc;
            GetLeadData();
            upMainData.Update();
            tabOwnerLogging.GetOwnerLogingList(txtTicketID.Text.Trim());
            upTabMain.Update();
        }

        private void GetLeadData()
        {
            lead = SlmScr004Biz.SearchSCR004Data(txtTicketID.Text.Trim());
            if (lead != null)
            {
                tabLeadInfo.GetLeadData(lead);
                txtstatus.Text = lead.StatusName;
                txtExternalSubStatusDesc.Text = lead.ExternalSubStatusDesc;
                txtExternalSubStatusDesc.ToolTip = lead.ExternalSubStatusDesc;
                txtFirstname.Text = lead.Name;
                txtFirstname.ToolTip = lead.Name;
                txtLastname.Text = lead.LastName;
                txtLastname.ToolTip = lead.LastName;
                txtCampaignId.Text = lead.CampaignId;
                txtCampaignName.Text = lead.CampaignName;
                txtCampaignName.ToolTip = lead.CampaignName;
                txtInterestedProd.Text = lead.InterestedProd;
                txtInterestedProd.ToolTip = lead.InterestedProd;
                txtCitizenId.Text = lead.CitizenId;
                txtChannelId.Text = lead.ChannelId;
                txtTelNo1.Text = lead.TelNo_1;
                if (lead.ContactLatestDate != null)
                    txtContactLatestDate.Text = lead.ContactLatestDate.Value.ToString("dd/MM/") + lead.ContactLatestDate.Value.Year.ToString()+" "+ lead.ContactLatestDate.Value.ToString("HH:mm:ss");
                if (lead.AssignedDateView != null)
                    txtAssignDate.Text = lead.AssignedDateView.Value.ToString("dd/MM/") + lead.AssignedDateView.Value.Year.ToString() + " " + lead.AssignedDateView.Value.ToString("HH:mm:ss");
                if (lead.ContactFirstDate != null)
                    txtContactFirstDate.Text = lead.ContactFirstDate.Value.ToString("dd/MM/") + lead.ContactFirstDate.Value.Year.ToString() + " " + lead.ContactFirstDate.Value.ToString("HH:mm:ss");
                txtOwnerLead.Text = lead.OwnerName;
                txtOwnerLead.ToolTip = lead.OwnerName;
                txtDelegateLead.Text = lead.DelegateName;
                txtDelegateLead.ToolTip = lead.DelegateName;
                txtDelegateBranch.Text = lead.DelegatebranchName;
                txtDelegateBranch.ToolTip = lead.DelegatebranchName;
                txtOwnerBranch.Text = lead.OwnerBranchName;
                txtOwnerBranch.ToolTip = lead.OwnerBranchName;
                txtTelNo_1.Text = lead.TelNo_1;
                txtTelNo2.Text  = lead.TelNo_2;
                txtExt2.Text = lead.Ext_2;
                txtTelNo3.Text = lead.TelNo_3;
                txtExt3.Text = lead.Ext_3;
                txtIsCOC.Text = lead.ISCOC;

                //COC
                if (lead.CocAssignedDate != null)
                    txtCOCAssignDate.Text = lead.CocAssignedDate.Value.ToString("dd/MM/") + lead.CocAssignedDate.Value.Year.ToString() + " " + lead.CocAssignedDate.Value.ToString("HH:mm:ss");

                txtMarketingOwner.Text = lead.MarketingOwnerName;
                txtMarketingOwner.ToolTip = lead.MarketingOwnerName;
                txtLastOwner.Text = lead.LastOwnerName;
                txtLastOwner.ToolTip = lead.LastOwnerName;
                txtCocStatus.Text = lead.CocStatusDesc;
                txtCocStatus.ToolTip = lead.CocStatusDesc;
                txtCocTeam.Text = lead.COCCurrentTeam;
                txtCocTeam.ToolTip = lead.COCCurrentTeam;
                txtDetail.Text = lead.Detail;

                //Icons
                if (lead.HasAdamsUrl)
                {
                    imbDoc.Visible = true;
                    LeadDataForAdam leadData = SlmScr003Biz.GetLeadDataForAdam(txtTicketID.Text.Trim());
                    imbDoc.OnClientClick = AppUtil.GetCallAdamScript(leadData, HttpContext.Current.User.Identity.Name, txtLoginEmpCode.Text.Trim(), false);
                }
                else
                    imbDoc.Visible = false;

                if (!string.IsNullOrEmpty(lead.CalculatorUrl))
                {
                    imbCal.Visible = true;
                    imbCal.OnClientClick = AppUtil.GetCallCalculatorScript(txtTicketID.Text.Trim(), lead.CalculatorUrl);
                }
                else
                    imbCal.Visible = false;

                if (!string.IsNullOrEmpty(lead.AppNo))
                {
                    decimal staffTypeId = txtLoginStaffTypeId.Text.Trim() != "" ? Convert.ToDecimal(txtLoginStaffTypeId.Text.Trim()) : 0;
                    string privilegeNCB = SlmScr003Biz.GetPrivilegeNCB(lead.ProductId, staffTypeId);

                    if (privilegeNCB != "")
                    {
                        imbOthers.Visible = true;
                        imbOthers.OnClientClick = AppUtil.GetCallAolSummaryReportScript(lead.AppNo, txtLoginEmpCode.Text.Trim(), txtLoginStaffTypeDesc.Text.Trim(), privilegeNCB);
                    }
                    else
                        imbOthers.Visible = false;
                }
                else
                    imbOthers.Visible = false;

                GetCampaignList();
            }
            
        }
        private void GetCampaignList()
        {
            try
            {
                List<CampaignWSData>  cList = SlmScr004Biz.GetCampaignFinalData(txtTicketID.Text.Trim());
                lbSum.Text = "<font class='hilightGreen'><b>" + cList.Count.ToString("#,##0") + "</b></font>"; 
                gvCampaign.DataSource = cList;
                gvCampaign.DataBind();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SLM_SCR_003.aspx");
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                GetCampaignDataPopup(0);
                mpePopup.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        #region Page Control

        private void BindGridview(SLM.Application.Shared.GridviewPageController pageControl, object[] items, int pageIndex)
        {
            pageControl.SetGridview(gvSearchCampaign);
            pageControl.Update(items, pageIndex);
            upCampaignPopup.Update();
        }

        protected void PageSearchChange(object sender, EventArgs e)
        {
            try
            {
                var pageControl = (SLM.Application.Shared.GridviewPageController)sender;
                GetCampaignDataPopup(pageControl.SelectedPageIndex);
                mpePopup.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        #endregion
        private void GetCampaignDataPopup(int pageIndex)
        {
            List<CampaignWSData>  campaign = SlmScr010Biz.GetCampaignData();
            BindGridview((SLM.Application.Shared.GridviewPageController)pcTop, campaign.ToArray(), pageIndex);
            upCampaignPopup.Update();
        }
        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                List<CampaignWSData> ListCampaign = new List<CampaignWSData>();
                if (gvSearchCampaign.Rows.Count > 0)
                {
                    for (int i = 0; i < gvSearchCampaign.Rows.Count; i++)
                    {
                        CheckBox chkSelect = (CheckBox)gvSearchCampaign.Rows[i].FindControl("chkSelect");
                        if (chkSelect != null)
                        {
                            if (chkSelect.Checked == true)
                            {
                                CampaignWSData cData = new CampaignWSData();
                                cData.TicketId = txtTicketID.Text.Trim();
                                cData.CampaignId = gvSearchCampaign.Rows[i].Cells[1].Text;
                                cData.CampaignName = gvSearchCampaign.Rows[i].Cells[2].Text;
                                Label lblCampaignDetail =  (Label)gvSearchCampaign.Rows[i].FindControl("lblCampaignDetail");
                                if(lblCampaignDetail != null)
                                    cData.CampaignDetail = lblCampaignDetail.ToolTip;
                                //cData.CampaignDetail = gvSearchCampaign.Rows[i].Cells[3].Text.Trim().Replace("&nbsp;","");
                                ListCampaign.Add(cData);
                            }
                        }
                    }
                    if (ListCampaign.Count == 0)
                    {
                        AppUtil.ClientAlert(Page, "กรุณาระบุ Product/Campaign อย่างน้อย 1 รายการ");
                        mpePopup.Show();
                        return;
                    }
                    else
                    {
                        SlmScr004Biz.InsertCampaginFinalList(ListCampaign, HttpContext.Current.User.Identity.Name);
                        GetCampaignList();
                        upHistory.Update();
                        AppUtil.ClientAlert(Page, "บันทึกข้อมูล Product/Campaign เรียบร้อย");
                        //AppUtil.ClientAlertAndRedirect(Page, "บันทึกข้อมูล Product/Campaign เรียบร้อย", "SLM_SCR_004.aspx?ticketid=" +txtTicketID.Text.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                mpePopup.Show();
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnCancelCampaign_Click(object sender, EventArgs e)
        {
            mpePopup.Hide();
        }

        protected void btnOfferCampaign_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCitizenId.Text.Trim() != "" && txtUserLoginChannelId.Text.Trim() != "")
                {
                    List<CampaignWSData> LData = new List<CampaignWSData>();

                    //********************** Web Services*******************************
                    CmtServiceProxy.CampaignByCustomerRequest request = new CmtServiceProxy.CampaignByCustomerRequest();

                    CmtServiceProxy.Header2 header = new CmtServiceProxy.Header2();
                    header.transaction_date = DateTime.Now.Year.ToString() + DateTime.Now.ToString("MMdd");
                    header.user_name = CMTUserName;
                    header.password = CMTPassword;
                    header.service_name = CMTServiceName;
                    header.system_code = CMTSystemCode;
                    header.reference_no = CMTReferenceNo;
                    request.header = header;

                    CmtServiceProxy.ReqCampByIdEntity body = new CmtServiceProxy.ReqCampByIdEntity();
                    body.CitizenId = txtCitizenId.Text.Trim();
                    body.RequestDate = DateTime.Now.Year.ToString() + DateTime.Now.ToString("MMdd");
                    body.Channel = txtUserLoginChannelId.Text.Trim();
                    body.CampaignNum = CMTCampaignNo;
                    body.HasOffered = "N";
                    body.IsInterested = "N";
                    body.CustomerFlag = "AND";
                    body.Command = "CampaignByCustomer";
                    request.CampByCitizenIdBody = body;

                    CmtServiceProxy.ICmtService service = new CmtServiceProxy.CmtServiceClient();
                    CmtServiceProxy.CampaignByCustomerResponse response = service.CampaignByCustomer(request);

                    if (response.status.status == "SUCCESS")
                    {
                        foreach (CmtServiceProxy.CitizenId result in response.detail.CitizenIds)
                        {
                            CampaignWSData cData = new CampaignWSData();
                            cData.CampaignId = result.CampaignId;
                            cData.CampaignCode = result.CampaignId;
                            cData.CampaignName = result.CampaignName;
                            cData.CampaignDetail = result.CampaignDescription;
                            cData.StartDate = result.StartDate;
                            cData.ExpireDate = AppUtil.ConvertToDateTime(result.ExpireDate, "");
                            if (cData.ExpireDate.Value.Year == 1)
                                cData.ExpireDate = null;
                            cData.ChannelName = result.Channel;
                            cData.HasOffered = result.HasOffered;
                            cData.IsInterested = result.IsInterested;

                            LData.Add(cData);
                        }

                        gvOfferCampaign.DataSource = LData;
                        gvOfferCampaign.DataBind();

                        upOfferCampaign.Update();
                        mpeOfferCampaignPopup.Show();
                    }
                    else
                    {
                        if(response.status.error_code == "E008")
                            AppUtil.ClientAlert(Page, "ไม่พบข้อมูล");
                        else
                            AppUtil.ClientAlert(Page, response.status.error_code + " : " + response.status.description);
                    }
                }
                else
                {
                    AppUtil.ClientAlert(Page, "กรุณาระบุเลขที่บัตรประชาชนและช่องทาง");
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnSelectOfferCampaign_Click(object sender, EventArgs e)
        {
            try
            {
                List<CampaignWSData> ListCampaign = new List<CampaignWSData>();
                if (gvOfferCampaign.Rows.Count > 0)
                {
                    for (int i = 0; i < gvOfferCampaign.Rows.Count; i++)
                    {
                        CheckBox chkSelect = (CheckBox)gvOfferCampaign.Rows[i].FindControl("chkSelect");
                        if (chkSelect.Checked == true)
                        {
                            CampaignWSData cData = new CampaignWSData();
                            cData.TicketId = txtTicketID.Text.Trim();
                            cData.CampaignId = gvOfferCampaign.Rows[i].Cells[1].Text;
                            cData.CampaignName = gvOfferCampaign.Rows[i].Cells[2].Text;
                            cData.CampaignDetail = ((Label)gvOfferCampaign.Rows[i].FindControl("lbDetail")).ToolTip.Trim().Replace("&nbsp;", "");
                            ListCampaign.Add(cData);
                        }
                    }
                    if (ListCampaign.Count == 0)
                    {
                        AppUtil.ClientAlert(Page, "กรุณาระบุ Product/Campaign อย่างน้อย 1 รายการ");
                        mpeOfferCampaignPopup.Show();
                        return;
                    }
                    else
                    {
                        CmtServiceProxy.UpdateCustomerFlagsResponse response = CallCmtService();
                        if (response.status.status == "SUCCESS")
                        {
                            SlmScr004Biz.InsertCampaginFinalList(ListCampaign, HttpContext.Current.User.Identity.Name);
                            GetCampaignList();
                            upHistory.Update();
                            AppUtil.ClientAlert(Page, "บันทึกข้อมูล Product/Campaign เรียบร้อย");
                            //AppUtil.ClientAlertAndRedirect(Page, "บันทึกข้อมูล Product/Campaign เรียบร้อย", "SLM_SCR_004.aspx?ticketid=" +txtTicketID.Text.Trim());
                        }
                        else
                        {
                            AppUtil.ClientAlert(Page, response.status.error_code + " : " + response.status.description);
                            mpeOfferCampaignPopup.Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                mpeOfferCampaignPopup.Show();
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private CmtServiceProxy.UpdateCustomerFlagsResponse CallCmtService()
        {
            try
            {
                List<CmtServiceProxy.UpdInquiry> inquiries = new List<CmtServiceProxy.UpdInquiry>();

                foreach (GridViewRow row in gvOfferCampaign.Rows)
                {
                    if (((CheckBox)row.FindControl("chkSelect")).Checked)
                    {
                        CmtServiceProxy.UpdInquiry inq = new CmtServiceProxy.UpdInquiry();
                        inq.CampaignId = row.Cells[1].Text.Trim();
                        inq.IsInterested = "Y";
                        inq.HasOffered = "Y";
                        inq.UpdatedBy = HttpContext.Current.User.Identity.Name;
                        inq.Command = "UpdateCustomerFlags";

                        inquiries.Add(inq);
                    }
                }

                //********************** Web Services*******************************
                CmtServiceProxy.UpdateCustomerFlagsRequest request = new CmtServiceProxy.UpdateCustomerFlagsRequest();

                CmtServiceProxy.Header header = new CmtServiceProxy.Header();
                header.transaction_date = DateTime.Now.Year.ToString() + DateTime.Now.ToString("MMdd");
                header.user_name = CMTUserName;
                header.password = CMTPassword;
                header.service_name = CMTServiceName;
                header.system_code = CMTSystemCode;
                header.reference_no = CMTReferenceNo;
                request.header = header;

                CmtServiceProxy.ReqUpdFlagEntity body = new CmtServiceProxy.ReqUpdFlagEntity();
                body.CitizenId = txtCitizenId.Text.Trim();
                body.UpdInquiries = inquiries.ToArray();

                request.UpdateCustFlag = body;

                CmtServiceProxy.ICmtService service = new CmtServiceProxy.CmtServiceClient();
                CmtServiceProxy.UpdateCustomerFlagsResponse response = service.UpdateCustomerFlags(request);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnCancelOfferCampaign_Click(object sender, EventArgs e)
        {
            mpeOfferCampaignPopup.Hide();
        }

        protected void gvSearchCampaign_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblCampaignDetail = (Label)e.Row.FindControl("lblCampaignDetail");
                    if (lblCampaignDetail.Text.Trim().Length > 100)
                        lblCampaignDetail.Text = lblCampaignDetail.Text.Trim().Substring(0, 100) + "...";
                }
            }
            catch (Exception ex)
            {
                mpePopup.Show();
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void gvCampaign_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblCampaignDesc = (Label)e.Row.FindControl("lbCampaignDetail");
                    if (lblCampaignDesc.Text.Trim().Length > AppConstant.Campaign.DisplayCampaignDescMaxLength)
                    {
                        lblCampaignDesc.Text = lblCampaignDesc.Text.Trim().Substring(0, AppConstant.Campaign.DisplayCampaignDescMaxLength) + "...";
                        LinkButton lbShowCampaignDesc = (LinkButton)e.Row.FindControl("lbShowCampaignDesc");
                        lbShowCampaignDesc.Visible = true;
                        lbShowCampaignDesc.OnClientClick = AppUtil.GetShowCampaignDescScript(Page, lbShowCampaignDesc.CommandArgument, "004_campaign_campaigndesc_" + lbShowCampaignDesc.CommandArgument);
                    }
                }
            }
            catch (Exception ex)
            {
                mpePopup.Show();
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void gvOfferCampaign_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lbDetail = (Label)e.Row.FindControl("lbDetail");
                    if (lbDetail.Text.Trim().Length > 100)
                        lbDetail.Text = lbDetail.Text.Trim().Substring(0, 100) + "...";
                }
            }
            catch (Exception ex)
            {
                mpePopup.Show();
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnAllCampaign_Click(object sender, EventArgs e)
        {
            try
            {
                Session[CMTCampaignSession] = null;
                DoSearchBundleCampaign();

                rbSearchByCombo.Checked = true;
                rbSearchByText.Checked = false;
                SearchCampaignCheckChanged();
                BindComboProductGroup();
                cmbProduct.Items.Clear();
                cmbProduct.Items.Insert(0, new ListItem("", "0"));
                cmbCampaign.Items.Clear();
                cmbCampaign.Items.Insert(0, new ListItem("", ""));
                if (txtUserLoginChannelId.Text.Trim() == "")
                {
                    lblPopupInfo.Text = "ไม่สามารถสร้างข้อมูลผู้มุ่งหวังได้ เนื่องจากไม่พบข้อมูลช่องทางของผู้ใช้งานระบบ กรุณาติดต่อ Admin เพื่อทำการกำหนดช่องทาง";
                    btnSearchCampaign.Enabled = false;
                }
                else
                {
                    lblPopupInfo.Text = "";
                    btnSearchCampaign.Enabled = true;
                }

                pcGridCampaign.SetVisible = false;
                gvAllCampaign.DataSource = null;
                gvAllCampaign.DataBind();
                gvAllCampaign.Visible = false;

                if (txtScreenHeight.Text.Trim() != "" && txtScreenWidth.Text.Trim() != "")
                {
                    if (Convert.ToInt32(txtScreenHeight.Text.Trim()) > 0 && Convert.ToInt32(txtScreenHeight.Text.Trim()) < 700)
                    {
                        pnPopupSearchCampaign.Height = new Unit(0.6 * Convert.ToDouble(txtScreenHeight.Text.Trim()), UnitType.Pixel);
                        pnPopupSearchCampaign.Width = new Unit(0.8 * Convert.ToDouble(txtScreenWidth.Text.Trim()), UnitType.Pixel);
                    }
                    else if (Convert.ToInt32(txtScreenHeight.Text.Trim()) >= 700 && Convert.ToInt32(txtScreenHeight.Text.Trim()) < 950)
                    {
                        pnPopupSearchCampaign.Height = new Unit(0.75 * Convert.ToDouble(txtScreenHeight.Text.Trim()), UnitType.Pixel);
                        pnPopupSearchCampaign.Width = new Unit(0.75 * Convert.ToDouble(txtScreenWidth.Text.Trim()), UnitType.Pixel);
                    }
                }

                upPopupSearchCampaign.Update();
                mpePopupSearchCampaign.Show();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        //========================================= Search Campaign แบบ All และ Bundle ==================================================
        #region Popup Search All Campaign

        private void BindComboProductGroup()
        {
            cmbProductGroup.DataSource = SlmScr003Biz.GetProductGroupData();
            cmbProductGroup.DataTextField = "TextField";
            cmbProductGroup.DataValueField = "ValueField";
            cmbProductGroup.DataBind();
            cmbProductGroup.Items.Insert(0, new ListItem("", ""));
        }

        protected void cmbProductGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbProduct.DataSource = SlmScr003Biz.GetProductData(cmbProductGroup.SelectedItem.Value);
                cmbProduct.DataTextField = "TextField";
                cmbProduct.DataValueField = "ValueField";
                cmbProduct.DataBind();
                cmbProduct.Items.Insert(0, new ListItem("", "0"));  //value = 0 ป้องกันในกรณีส่งค่า ช่องว่างไป where ใน CMT_CAMPAIGN_PRODUCT แล้วค่า PR_ProductId บาง record เป็นช่องว่าง

                cmbCampaign.Items.Clear();
                cmbCampaign.Items.Insert(0, new ListItem("", ""));

                mpePopupSearchCampaign.Show();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            string campaignList = "";
            try
            {
                if (Session[CMTCampaignSession] == null)
                    GetCMTCampaign();

                List<CmtServiceProxy.CitizenId> cmtCampaignList = (List<CmtServiceProxy.CitizenId>)Session[CMTCampaignSession];
                foreach (CmtServiceProxy.CitizenId campaign in cmtCampaignList)
                {
                    campaignList += (campaignList != "" ? "," : "") + "'" + campaign.CampaignId + "'";
                }

                cmbCampaign.DataSource = SlmScr004Biz.GetCampaignDataViewPage(cmbProductGroup.SelectedItem.Value, cmbProduct.SelectedItem.Value, campaignList);
                cmbCampaign.DataTextField = "TextField";
                cmbCampaign.DataValueField = "ValueField";
                cmbCampaign.DataBind();
                cmbCampaign.Items.Insert(0, new ListItem("", ""));
                cmbCampaign.Items.Remove(cmbCampaign.Items.FindByValue(txtCampaignId.Text.Trim())); //เอาแคมเปญหลักของหน้าออก

                mpePopupSearchCampaign.Show();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            mpePopupSearchCampaign.Hide();
        }

        protected void rbSearchByCombo_CheckedChanged(object sender, EventArgs e)
        {
            SearchCampaignCheckChanged();
            upPopupSearchCampaignInner.Update();
            //upPopupSearchCampaign.Update();
            mpePopupSearchCampaign.Show();
        }

        protected void rbSearchByText_CheckedChanged(object sender, EventArgs e)
        {
            SearchCampaignCheckChanged();
            upPopupSearchCampaignInner.Update();
            //upPopupSearchCampaign.Update();
            mpePopupSearchCampaign.Show();
        }

        private void SearchCampaignCheckChanged()
        {
            cmbProductGroup.Enabled = rbSearchByCombo.Checked;
            cmbProduct.Enabled = rbSearchByCombo.Checked;
            cmbCampaign.Enabled = rbSearchByCombo.Checked;

            txtFullSearchCampaign.Enabled = rbSearchByText.Checked;
            txtFullSearchCampaign.Text = "";
            pcGridCampaign.SetVisible = false;
            gvAllCampaign.DataSource = null;
            gvAllCampaign.DataBind();
            gvAllCampaign.Visible = false;

            if (rbSearchByText.Checked)
            {
                cmbProductGroup.SelectedIndex = -1;
                cmbProduct.Items.Clear();
                cmbProduct.Items.Insert(0, new ListItem("", "0"));
                cmbCampaign.Items.Clear();
                cmbCampaign.Items.Insert(0, new ListItem("", ""));
            }
        }

        protected void imbSelect_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnSearchCampaign_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateInput())
                    DoSearchCampaign();

                mpePopupSearchCampaign.Show();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private bool ValidateInput()
        {
            if (rbSearchByCombo.Checked)
            {
                if (cmbProductGroup.SelectedItem.Value == "")
                {
                    AppUtil.ClientAlert(Page, "กรุณาเลือกข้อมูล กลุ่มผลิตภัณฑ์/บริการ");
                    return false;
                }
                //if (cmbProduct.SelectedItem.Value == "0")
                //{
                //    AppUtil.ClientAlert(Page, "กรุณาเลือกข้อมูล ผลิตภัณฑ์/บริการ");
                //    return false;
                //}
                //if (cmbCampaign.SelectedItem.Value == "")
                //{
                //    AppUtil.ClientAlert(Page, "กรุณาเลือกข้อมูล แคมเปญ");
                //    return false;
                //}
            }
            else
            {
                if (txtFullSearchCampaign.Text.Trim() == "")
                {
                    AppUtil.ClientAlert(Page, "กรุณาระบุคำที่ต้องการค้นหา");
                    return false;
                }
            }

            return true;
        }

        private void DoSearchCampaign()
        {
            //********* แก้ใน Method นี้ ให้ดูใน Method PageSearchChangeCampaign ด้วย *********
            try
            {
                List<ProductData> result = null;

                if (rbSearchByCombo.Checked)
                {
                    //string[] bundleCamIdList = txtBundleCampaignIdList.Text.Trim().Replace("'", "").Split(',');
                    //if (bundleCamIdList.Count(p => p == cmbCampaign.SelectedItem.Value) == 0)
                    //    result = SlmScr004Biz.SearchCampaignViewPage(cmbProductGroup.SelectedItem.Value, cmbProduct.SelectedItem.Value, cmbCampaign.SelectedItem.Value);
                    //else
                    //    result = new List<ProductData>();

                    //ถ้าเลือก 3 combo และ แคมเปญอยู่ในตาราง Bundle แล้ว ให้แสดง alert แจ้ง
                    if (cmbProductGroup.SelectedItem.Value != "" && cmbProduct.SelectedItem.Value != "0" && cmbCampaign.SelectedItem.Value != "")
                    {
                        if (txtBundleCampaignIdList.Text.Trim() != "")
                        {
                            string[] bundleCamIdList = txtBundleCampaignIdList.Text.Trim().Replace("'", "").Split(',');
                            if (bundleCamIdList.Count(p => p.Trim() == cmbCampaign.SelectedItem.Value) > 0)
                            {
                                result = new List<ProductData>();
                                BindGridviewAllCampaign((SLM.Application.Shared.GridviewPageController)pcGridCampaign, result.ToArray(), 0);
                                gvAllCampaign.Visible = true;
                                AppUtil.ClientAlert(Page, "อยู่ในแคมเปญร่วมแล้ว");
                                return;
                            }
                        }
                    }

                    result = SlmScr004Biz.SearchCampaignViewPage(cmbProductGroup.SelectedItem.Value, cmbProduct.SelectedItem.Value, cmbCampaign.SelectedItem.Value, txtBundleCampaignIdList.Text.Trim());
                }
                else
                    result = SlmScr004Biz.SearchCampaign(txtFullSearchCampaign.Text.Trim(), txtBundleCampaignIdList.Text.Trim());

                if (result.Count > 0)
                {
                    result = CheckCMTAllCampaign(result, rbSearchByCombo.Checked ? "combo" : "text");
                    result = FilterAccessRight(result);
                }

                BindGridviewAllCampaign((SLM.Application.Shared.GridviewPageController)pcGridCampaign, result.ToArray(), 0);
                gvAllCampaign.Visible = true;
                //upPopupSearchCampaignInner.Update();
                //upPopupSearchCampaign.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<ProductData> FilterAccessRight(List<ProductData> list)
        {
            try
            {
                //ส่งข้อมุลแคมเปญและ owner ไปเช็กสิทธิในการสร้าง (คน login = owner)
                List<ProductData> notPassList = new List<ProductData>();
                foreach (ProductData data in list)
                {
                    if (!SlmScr010Biz.PassPrivilegeCampaign(SLMConstant.Branch.Active, data.CampaignId, HttpContext.Current.User.Identity.Name))
                        notPassList.Add(data);
                }

                return list.Except<ProductData>(notPassList).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DoSearchBundleCampaign()
        {
            try
            {
                List<ProductData> result = CheckCMTBundle(gvBundleCampaign, SlmScr004Biz.GetBundleProduct(txtCampaignId.Text.Trim()));
                txtBundleCampaignIdList.Text = "'" + txtCampaignId.Text.Trim() + "'";
                foreach (ProductData product in result)
                {
                    //เก็บไว้เพื่อส่งไป Not In ใน Gridview All Campaign
                    txtBundleCampaignIdList.Text += (txtBundleCampaignIdList.Text.Trim() != "" ? "," : "") + "'" + product.CampaignId + "'";
                }

                BindGridviewBundleCampaign((SLM.Application.Shared.GridviewPageController)pcGridBundleCampaign, result.ToArray(), 0);
                
                upPopupSearchCampaign.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<ProductData> CheckCMTBundle(GridView gv, List<ProductData> productList)
        {
            try
            {
                if (Session[CMTCampaignSession] == null)
                    GetCMTCampaign();

                List<CmtServiceProxy.CitizenId> cmtCampaignList = (List<CmtServiceProxy.CitizenId>)Session[CMTCampaignSession];

                foreach (CmtServiceProxy.CitizenId campaign in cmtCampaignList)
                {
                    var product = productList.Where(p => p.CampaignId == campaign.CampaignId).FirstOrDefault();
                    if (product != null)
                        product.Recommend = "*";
                }

                //นำแคมเปญที่ไม่ใช่ Mass และไม่ได้ถูกแนะนำจาก CMT ออกจาก list
                List<ProductData> belowTheLineList = productList.Where(p => p.CampaignType != SLM.Resource.SLMConstant.CampaignType.Mass && p.Recommend != "*").ToList();
                if (belowTheLineList.Count > 0)
                {
                    foreach (ProductData product in belowTheLineList)
                    {
                        productList.Remove(product);
                    }
                }

                productList = productList.OrderByDescending(p => p.Recommend).ToList();

                //Logic เดิม
                //if (cmtCampaignList.Count > 0)
                //{
                //    foreach (ProductData product in productList)
                //    {
                //        if (cmtCampaignList.Count(p => p.CampaignId == product.CampaignId) > 0)
                //            product.Recommend = "*";
                //    }

                //    productList = productList.OrderByDescending(p => p.Recommend).ToList();
                //}

                return productList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<ProductData> CheckCMTAllCampaign(List<ProductData> productList, string searchFlag)
        {
            try
            {
                if (Session[CMTCampaignSession] == null)
                    GetCMTCampaign();

                List<CmtServiceProxy.CitizenId> cmtCampaignList = (List<CmtServiceProxy.CitizenId>)Session[CMTCampaignSession];

                //List ของ campaign id ที่อยู่ในตาราง Bundle
                string[] bundleCamIdList = txtBundleCampaignIdList.Text.Trim().Replace("'", "").Split(',');

                foreach (CmtServiceProxy.CitizenId campaign in cmtCampaignList)
                {
                    //campaign จาก cmt ต้องไม่อยู่ในตาราง bundle และไม่ใช่แคมเปญหลักของหน้า view
                    if (bundleCamIdList.Count(p => p.Trim() == campaign.CampaignId) == 0 && campaign.CampaignId != txtCampaignId.Text.Trim())
                    {
                        var product = productList.Where(p => p.CampaignId == campaign.CampaignId).FirstOrDefault();

                        //ถ้านำ campaign จาก cmt ไปหาใน result list(productList) ที่ได้จากการค้นหาจากฐานข้อมูล
                        //ถ้าเจอให้ใส่ *, ถ้าไม่เจอให้นำ campaign จาก cmt ไป add เพิ่มใน result list(productList) เพื่อนำไปแสดงผลบนหน้าจอ
                        if (product != null)
                            product.Recommend = "*";
                        else
                        {
                            if (searchFlag == "text")   //campaign ใน cmt add เพิ่มใน result list 
                            {
                                var data = SlmScr004Biz.GetProductCampaignDataForCmt(campaign.CampaignId);  //เอา Campaign จาก CMT ไปหาข้อมูลโดยไม่ต้องส่งใจ Mass
                                if (data.Count > 0)
                                {
                                    //นำ campaign จาก cmt ไปค้นหาข้อมูลในฐานข้อมูล 
                                    //เมื่อได้ข้อมูลมาแล้วให้เช็กด้วยว่า campaign นัั้นมี ProductGroupName, ProductName, CampaignName ที่มีคำ เหมือนคำที่อยู่ใน txtFullSearchCampaign.Text หรือไม่
                                    //ถ้ามีคำเหมือนให้ add เข้า result list(productList) เพื่อนำไปแสดงผลบนหน้าจอ
                                    if (data[0].ProductGroupName.Contains(txtFullSearchCampaign.Text.Trim()) || data[0].ProductName.Contains(txtFullSearchCampaign.Text.Trim())
                                    || data[0].CampaignName.Contains(txtFullSearchCampaign.Text.Trim()))
                                    {
                                        ProductData new_product = new ProductData()
                                        {
                                            CampaignId = campaign.CampaignId,
                                            CampaignName = data[0].CampaignName,
                                            ProductGroupId = data[0].ProductGroupId,
                                            ProductGroupName = data[0].ProductGroupName,
                                            ProductId = data[0].ProductId,
                                            ProductName = data[0].ProductName,
                                            CampaignDesc = data[0].CampaignDesc,
                                            StartDate = data[0].StartDate,
                                            EndDate = data[0].EndDate
                                        };

                                        productList.Add(new_product);
                                    }
                                }
                            }
                            else
                            {
                                //searchFlag == combo, เช็กว่าแคมเปญที่มาจาก Cmt มีค่า ProductGroupId, ProductId, CampaignId ตรงกับค่าใน Combo ที่ถูกเลือกไว้หรือไม่
                                bool isAdd = false;
                                var data = SlmScr004Biz.GetProductCampaignDataForCmt(campaign.CampaignId);  //เอา Campaign จาก CMT ไปหาข้อมูลโดยไม่ต้องส่งใจ Mass
                                if (data.Count > 0)
                                {
                                    //ถ้า Combo ถูกเลือกไว้สามตัว เช็กสามค่า
                                    if (cmbProductGroup.SelectedItem.Value != "" && cmbProduct.SelectedItem.Value != "0" && cmbCampaign.SelectedItem.Value != "")
                                    {
                                        if (cmbProductGroup.SelectedItem.Value == data[0].ProductGroupId && cmbProduct.SelectedItem.Value == data[0].ProductId && cmbCampaign.SelectedItem.Value == data[0].CampaignId)
                                            isAdd = true;
                                    }
                                    else if (cmbProductGroup.SelectedItem.Value != "" && cmbProduct.SelectedItem.Value != "0") //ถ้า Combo ถูกเลือกไว้สองตัว เช็กสองค่า
                                    {
                                        if (cmbProductGroup.SelectedItem.Value == data[0].ProductGroupId && cmbProduct.SelectedItem.Value == data[0].ProductId)
                                            isAdd = true;
                                    }
                                    else if (cmbProductGroup.SelectedItem.Value != "")  //ถ้า Combo ถูกเลือกไว้ตัวเดียว เช็กหนึ่งค่า
                                    {
                                        if (cmbProductGroup.SelectedItem.Value == data[0].ProductGroupId)   
                                            isAdd = true;
                                    }

                                    if (isAdd)
                                    {
                                        ProductData new_product = new ProductData()
                                        {
                                            CampaignId = campaign.CampaignId,
                                            CampaignName = data[0].CampaignName,
                                            ProductGroupId = data[0].ProductGroupId,
                                            ProductGroupName = data[0].ProductGroupName,
                                            ProductId = data[0].ProductId,
                                            ProductName = data[0].ProductName,
                                            CampaignDesc = data[0].CampaignDesc,
                                            StartDate = data[0].StartDate,
                                            EndDate = data[0].EndDate
                                        };

                                        productList.Add(new_product);
                                    }
                                }
                            }
                        }
                    }
                }

                //นำแคมเปญที่ไม่ใช่ Mass และไม่ได้ถูกแนะนำจาก CMT ออกจาก list
                List<ProductData> belowTheLineList = productList.Where(p => p.CampaignType != SLM.Resource.SLMConstant.CampaignType.Mass && p.Recommend != "*").ToList();
                if (belowTheLineList.Count > 0)
                {
                    foreach (ProductData product in belowTheLineList)
                    {
                        productList.Remove(product);
                    }
                }

                productList = productList.OrderByDescending(p => p.Recommend).ToList();

                return productList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetCMTCampaign()
        {
            try
            {
                if (txtCitizenId.Text.Trim() != "" && txtUserLoginChannelId.Text.Trim() != "")
                {
                    List<CampaignWSData> LData = new List<CampaignWSData>();

                    //********************** Web Services*******************************
                    //CmtServiceProxy.CampaignByCustomerRequest request = new CmtServiceProxy.CampaignByCustomerRequest();

                    CmtServiceProxy.CampaignByCustomersRequest request = new CmtServiceProxy.CampaignByCustomersRequest();
                    CmtServiceProxy.Header2 header = new CmtServiceProxy.Header2();
                    header.transaction_date = DateTime.Now.Year.ToString() + DateTime.Now.ToString("MMdd");
                    header.user_name = CMTUserName;
                    header.password = CMTPassword;
                    header.service_name = CMTServiceName;
                    header.system_code = CMTSystemCode;
                    header.reference_no = CMTReferenceNo;
                    request.header = header;

                    //CmtServiceProxy.ReqCampByIdEntity body = new CmtServiceProxy.ReqCampByIdEntity();
                    CmtServiceProxy.ReqCampByCusEntity body = new CmtServiceProxy.ReqCampByCusEntity();
                    body.CitizenId = txtCitizenId.Text.Trim();
                    body.RequestDate = DateTime.Now.Year.ToString() + DateTime.Now.ToString("MMdd");
                    body.Channel = txtUserLoginChannelId.Text.Trim();
                    body.CampaignNum = CMTCampaignNo;
                    body.HasOffered = "N";
                    body.IsInterested = "N";
                    body.CustomerFlag = "AND";
                    body.Command = "CampaignByCustomer";
                    request.CampByCitizenIdBody = body;

                    CmtServiceProxy.CmtServiceClient cmt_service_client = new CmtServiceProxy.CmtServiceClient();
                    cmt_service_client.InnerChannel.OperationTimeout = new TimeSpan(0, 0, AppConstant.CMTTimeout);

                    CmtServiceProxy.ICmtService service = cmt_service_client;
                    //CmtServiceProxy.CampaignByCustomerResponse response = service.CampaignByCustomer(request);
                    CmtServiceProxy.CampaignByCustomersResponse response = service.CampaignByCustomers(request);

                    if (response.status.status == "SUCCESS")
                    {
                        Session[CMTCampaignSession] = response.detail.CitizenIds.ToList();
                    }
                    else
                    {
                        Session[CMTCampaignSession] = new List<CmtServiceProxy.CitizenId>();

                        if (response.status.error_code != "E008")
                            _log.Debug("Call CMT: " + response.status.error_code + " : " + response.status.description);
                    }
                }
                else
                    Session[CMTCampaignSession] = new List<CmtServiceProxy.CitizenId>();
            }
            catch (Exception ex)
            {
                Session[CMTCampaignSession] = new List<CmtServiceProxy.CitizenId>();
                _log.Debug(ex.Message);
            }
        }

        #endregion

        #region Page Control Popup SearchCampaign

        private void BindGridviewAllCampaign(SLM.Application.Shared.GridviewPageController pageControl, object[] items, int pageIndex)
        {
            pageControl.SetGridview(gvAllCampaign);
            pageControl.Update(items, pageIndex, 5);
            upPopupSearchCampaignInner.Update();
            //upPopupSearchCampaign.Update();
            mpePopupSearchCampaign.Show();
        }

        protected void PageSearchChangeCampaign(object sender, EventArgs e)
        {
            try
            {
                List<ProductData> result = null;

                if (rbSearchByCombo.Checked)
                {
                    //string[] bundleCamIdList = txtBundleCampaignIdList.Text.Trim().Replace("'", "").Split(',');
                    //if (bundleCamIdList.Count(p => p == cmbCampaign.SelectedItem.Value) == 0)
                    //    result = SlmScr004Biz.SearchCampaignViewPage(cmbProductGroup.SelectedItem.Value, cmbProduct.SelectedItem.Value, cmbCampaign.SelectedItem.Value);
                    //else
                    //    result = new List<ProductData>();

                    result = SlmScr004Biz.SearchCampaignViewPage(cmbProductGroup.SelectedItem.Value, cmbProduct.SelectedItem.Value, cmbCampaign.SelectedItem.Value, txtBundleCampaignIdList.Text.Trim());
                }
                else
                    result = SlmScr004Biz.SearchCampaign(txtFullSearchCampaign.Text.Trim(), txtBundleCampaignIdList.Text.Trim());

                if (result.Count > 0)
                {
                    result = CheckCMTAllCampaign(result, rbSearchByCombo.Checked ? "combo" : "text");
                    result = FilterAccessRight(result);
                }

                var pageControl = (SLM.Application.Shared.GridviewPageController)sender;
                BindGridviewAllCampaign(pageControl, result.ToArray(), pageControl.SelectedPageIndex);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        #endregion

        #region Page Control Bundle Campaign

        private void BindGridviewBundleCampaign(SLM.Application.Shared.GridviewPageController pageControl, object[] items, int pageIndex)
        {
            pageControl.SetGridview(gvBundleCampaign);
            pageControl.Update(items, pageIndex, 5);
            upPopupSearchCampaign.Update();
            mpePopupSearchCampaign.Show();
        }

        protected void PageSearchChangeBundleCampaign(object sender, EventArgs e)
        {
            try
            {
                List<ProductData> result = CheckCMTBundle(gvBundleCampaign, SlmScr004Biz.GetBundleProduct(txtCampaignId.Text.Trim()));
                txtBundleCampaignIdList.Text = "'" + txtCampaignId.Text.Trim() + "'";
                foreach (ProductData product in result)
                {
                    //เก็บไว้เพื่อส่งไป Not In ใน Gridview All Campaign
                    txtBundleCampaignIdList.Text += (txtBundleCampaignIdList.Text.Trim() != "" ? "," : "") + "'" + product.CampaignId + "'";
                }
                var pageControl = (SLM.Application.Shared.GridviewPageController)sender;
                BindGridviewBundleCampaign(pageControl, result.ToArray(), pageControl.SelectedPageIndex);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        #endregion

        protected void gvBundleCampaign_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            { 
                if (((Label)e.Row.FindControl("lblCmt")).Text.Trim() == "*")
                    e.Row.ForeColor = System.Drawing.Color.Red;

                Label lblCampaignDesc = (Label)e.Row.FindControl("lblCampaignDesc");
                if (lblCampaignDesc.Text.Trim().Length > AppConstant.Campaign.DisplayCampaignDescMaxLength)
                {
                    lblCampaignDesc.Text = lblCampaignDesc.Text.Trim().Substring(0, AppConstant.Campaign.DisplayCampaignDescMaxLength) + "...";
                    LinkButton lbShowCampaignDesc = (LinkButton)e.Row.FindControl("lbShowCampaignDesc");
                    lbShowCampaignDesc.Visible = true;
                    lbShowCampaignDesc.OnClientClick = AppUtil.GetShowCampaignDescScript(Page, lbShowCampaignDesc.CommandArgument, "004_bundle_campaigndesc_" + lbShowCampaignDesc.CommandArgument);
                }
            }
        }

        protected void gvAllCampaign_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((Label)e.Row.FindControl("lblCmt")).Text.Trim() == "*")
                    e.Row.ForeColor = System.Drawing.Color.Red;

                Label lblCampaignDesc = (Label)e.Row.FindControl("lblCampaignDesc");
                if (lblCampaignDesc.Text.Trim().Length > AppConstant.Campaign.DisplayCampaignDescMaxLength)
                {
                    lblCampaignDesc.Text = lblCampaignDesc.Text.Trim().Substring(0, AppConstant.Campaign.DisplayCampaignDescMaxLength) + "...";
                    LinkButton lbShowCampaignDesc = (LinkButton)e.Row.FindControl("lbShowCampaignDesc");
                    lbShowCampaignDesc.Visible = true;
                    lbShowCampaignDesc.OnClientClick = AppUtil.GetShowCampaignDescScript(Page, lbShowCampaignDesc.CommandArgument, "004_all_campaigndesc_" + lbShowCampaignDesc.CommandArgument);
                }
            }
        }

        protected void btnSelectCampaign_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateData())
                {
                    AppUtil.ClientAlert(Page, "กรุณาเลือกอย่างน้อย 1 แคมเปญ");
                    mpePopupSearchCampaign.Show();
                    return;
                }

                List<string> selectedCmtCampaignIdList = GetSelectedCmtCampaignId();
                if (selectedCmtCampaignIdList.Count == 0)
                {
                    DoInsert();
                }
                else
                {
                    CmtServiceProxy.UpdateCustomerFlagsResponse response = UpdateToCmtService(selectedCmtCampaignIdList);
                    if (response.status.status == "SUCCESS")
                    {
                        DoInsert();
                    }
                    else
                    {
                        AppUtil.ClientAlert(Page, response.status.error_code + " : " + response.status.description);
                        mpePopupSearchCampaign.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private List<string> GetSelectedCmtCampaignId()
        {
            List<string> list = new List<string>();
            foreach (GridViewRow row in gvBundleCampaign.Rows)
            {
                if (((CheckBox)row.FindControl("cbSelectCampaign")).Checked && ((Label)row.FindControl("lblCmt")).Text.Trim() == "*")
                    list.Add(((Label)row.FindControl("lblCampaignId")).Text.Trim());
            }

            foreach (GridViewRow row in gvAllCampaign.Rows)
            {
                if (((CheckBox)row.FindControl("cbSelectCampaign")).Checked && ((Label)row.FindControl("lblCmt")).Text.Trim() == "*")
                    list.Add(((Label)row.FindControl("lblCampaignId")).Text.Trim());
            }
            return list;
        }

        private bool ValidateData()
        {
            foreach (GridViewRow row in gvBundleCampaign.Rows)
            {
                if (((CheckBox)row.FindControl("cbSelectCampaign")).Checked)
                    return true;
            }
            foreach (GridViewRow row in gvAllCampaign.Rows)
            {
                if (((CheckBox)row.FindControl("cbSelectCampaign")).Checked)
                    return true;
            }
            return false;
        }

        private void DoInsert()
        {
            try
            {
                //Insert Bundle
                List<ProductData> campList = GetCampaignData(gvBundleCampaign);
                if (campList.Count > 0)
                    SlmScr004Biz.InsertCampaginFinalList(campList, txtTicketID.Text.Trim(), HttpContext.Current.User.Identity.Name);

                //Insert New Lead
                campList = GetCampaignData(gvAllCampaign);
                if (campList.Count > 0)
                {
                    LeadInfoBiz biz = new LeadInfoBiz();
                    List<SaveResultData> resultList = biz.InsertNewLeads(txtTicketID.Text.Trim(), campList, HttpContext.Current.User.Identity.Name, txtLoginNameTH.Text.Trim(), txtUserLoginChannelId.Text.Trim(), txtUserLoginChannelDesc.Text.Trim());

                    rptPopupSaveResult.DataSource = resultList;
                    rptPopupSaveResult.DataBind();
                    upPopupSaveResult.Update();
                    mpePopupSaveResult.Show();

                    if (biz.ErrorList.Count > 0)
                    {
                        //ลง error log
                        _log.Debug(" ");
                        _log.Debug("Page: View Lead, Process: Create Lead by Campaign");
                        foreach (KeyValuePair<string, string> pair in biz.ErrorList)
                        {
                            _log.Debug("CampaignId: " + pair.Key + ", Error: " + pair.Value);
                        }
                    }
                }
                else
                    AppUtil.ClientAlert(Page, "บันทึกข้อมูลเรียบร้อย");

                GetCampaignList();
                upHistory.Update();
                mpePopupSearchCampaign.Hide();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnPopupSaveResultOK_Click(object sender, EventArgs e)
        {
            rptPopupSaveResult.DataSource = null;
            rptPopupSaveResult.DataBind();
            mpePopupSaveResult.Hide();
        }

        private List<ProductData> GetCampaignData(GridView gridview)
        {
            try
            {
                List<ProductData> camplist = new List<ProductData>();

                foreach (GridViewRow row in gridview.Rows)
                {
                    if (((CheckBox)row.FindControl("cbSelectCampaign")).Checked)
                    {
                        if (((Label)row.FindControl("lblCampaignDesc")).Text.Trim().Length > AppConstant.TextMaxLength)
                            throw new Exception("ไม่สามารถบันทึกรายละเอียดแคมเปญเกิน " + AppConstant.TextMaxLength.ToString() + " ตัวอักษรได้\\r\\nรบกวนติดต่อผู้ดูแลระบบ CMT เพื่อแก้ไขรายละเอียด");

                        ProductData data = new ProductData()
                        {
                            ProductGroupId = ((Label)row.FindControl("lblProductGroupId")).Text.Trim(),
                            ProductId = ((Label)row.FindControl("lblProductId")).Text.Trim(),
                            ProductName = ((Label)row.FindControl("lblProductName")).Text.Trim(),
                            CampaignId = ((Label)row.FindControl("lblCampaignId")).Text.Trim(),
                            CampaignName = ((Label)row.FindControl("lblCampaignName")).Text.Trim(),
                            CampaignDesc = ((Label)row.FindControl("lblCampaignDesc")).Text.Trim()
                        };
                        camplist.Add(data);
                    }
                }

                return camplist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private CmtServiceProxy.UpdateCustomerFlagsResponse UpdateToCmtService(List<string> selectedCmtCampaignIdList)
        {
            try
            {
                List<CmtServiceProxy.UpdInquiry> inquiries = new List<CmtServiceProxy.UpdInquiry>();

                foreach (string campaignId in selectedCmtCampaignIdList)
                {
                    CmtServiceProxy.UpdInquiry inq = new CmtServiceProxy.UpdInquiry();
                    inq.CampaignId = campaignId;
                    inq.IsInterested = "Y";
                    inq.HasOffered = "Y";
                    inq.UpdatedBy = HttpContext.Current.User.Identity.Name;
                    inq.Command = "UpdateCustomerFlags";

                    inquiries.Add(inq);
                }

                //********************** Web Services*******************************
                CmtServiceProxy.UpdateCustomerFlagsRequest request = new CmtServiceProxy.UpdateCustomerFlagsRequest();

                CmtServiceProxy.Header header = new CmtServiceProxy.Header();
                header.transaction_date = DateTime.Now.Year.ToString() + DateTime.Now.ToString("MMdd");
                header.user_name = CMTUserName;
                header.password = CMTPassword;
                header.service_name = CMTServiceName;
                header.system_code = CMTSystemCode;
                header.reference_no = CMTReferenceNo;
                request.header = header;

                CmtServiceProxy.ReqUpdFlagEntity body = new CmtServiceProxy.ReqUpdFlagEntity();
                body.CitizenId = txtCitizenId.Text.Trim();
                body.UpdInquiries = inquiries.ToArray();

                request.UpdateCustFlag = body;

                CmtServiceProxy.CmtServiceClient cmt_service_client = new CmtServiceProxy.CmtServiceClient();
                cmt_service_client.InnerChannel.OperationTimeout = new TimeSpan(0, 0, AppConstant.CMTTimeout);

                CmtServiceProxy.ICmtService service = cmt_service_client;
                CmtServiceProxy.UpdateCustomerFlagsResponse response = service.UpdateCustomerFlags(request);
                return response;
            }
            catch (Exception ex)
            {
                _log.Debug(" ");
                _log.Debug("Page: View Lead, Process: Update data back to cmt service");
                _log.Debug("Error: " + ex.InnerException != null ? ex.InnerException.Message : ex.Message);

                throw ex;
            }
        }

        protected void imbCopyLead_Click(object sender, ImageClickEventArgs e)
        {
            Session["ticket_id"] = txtTicketID.Text.Trim();
            Response.Redirect("~/SLM_SCR_010.aspx");
        }

//        private string GetPostToCreateLeadScript()
//        {
//            string script = @"var form = document.createElement('form');
//                                            var ticketid = document.createElement('input');
//            
//                                            form.action = '" + Page.ResolveUrl("~/SLM_SCR_010.aspx") + @"';
//                                            form.method = 'post';
//                                            form.setAttribute('target', '_parent');
//            
//                                            ticketid.name = 'ticket_id';
//                                            ticketid.value = '" + txtTicketID.Text.Trim() + @"';
//                                            form.appendChild(ticketid);
//            
//                                            document.body.appendChild(form);
//                                            form.submit();
//            
//                                            document.body.removeChild(form);";

//            return script;
//        }
    }
}