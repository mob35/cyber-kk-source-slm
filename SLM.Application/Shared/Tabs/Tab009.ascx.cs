using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SLM.Application.Utilities;
using SLM.Resource;
using SLM.Resource.Data;
using SLM.Biz;
using log4net;

namespace SLM.Application.Shared.Tabs
{
    public partial class Tab009 : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Tab009));

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            AppUtil.SetNotThaiCharacter(txtEmailTo);
            //AppUtil.SetMultilineMaxLength(txtNoteDetail, "lblMsg2", AppConstant.TextMaxLength.ToString());
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
                if (data.ISCOC == "1")
                {
                    btnAddNote.Visible = (data.COCCurrentTeam == SLMConstant.COCTeam.Marketing ? true : false);
                    cbNoteFlag.Visible = (data.COCCurrentTeam == SLMConstant.COCTeam.Marketing ? true : false);
                }
                else
                {
                    btnAddNote.Visible = true;
                    cbNoteFlag.Visible = true;
                }
                txtTicketID.Text = data.TicketId;
                txtFirstname.Text = data.Name;
                txtLastname.Text = data.LastName;
                txtOwnerLead.Text = data.OwnerName;
                txtCampaign.Text = data.CampaignName;
                txtTelNo1.Text = data.TelNo_1;
                txtExt1.Text = data.Ext_1;
                cbNoteFlag.Checked = data.NoteFlag != null ? (data.NoteFlag == "1" ? true : false) : false;

                pcTop.SetVisible = false;
                DoBindGridview();
                CheckEmailSubject();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DoBindGridview()
        {
            List<NoteHistoryData> result = SlmScr009Biz.SearchNoteHistory(txtTicketID.Text.Trim());
            BindGridview((SLM.Application.Shared.GridviewPageController)pcTop, result.ToArray(), 0);
            cbNoteFlag.Enabled = gvNoteHistory.Rows.Count > 0 ? true : false;
            upResult.Update();
        }

        private void CheckEmailSubject()
        {
            if (cbSendEmail.Checked)
            {
                txtEmailSubject.Enabled = true;
                txtEmailSubject.Text = "SLM: Ticket: " + txtTicketID.Text.Trim();
                trEmail.Visible = true;
                trEmailTo.Visible = true;
                trEmailSample.Visible = true;
            }
            else
            {
                txtEmailSubject.Text = string.Empty;
                txtEmailSubject.Enabled = false;
                txtEmailSubject.Text = "";
                alertEmailSubject.Text = "";
                txtEmailTo.Text = "";
                alertEmailTo.Text = "";
                trEmail.Visible = false;
                trEmailTo.Visible = false;
                trEmailSample.Visible = false;
            }
        }

        #region Page Control

        private void BindGridview(SLM.Application.Shared.GridviewPageController pageControl, object[] items, int pageIndex)
        {
            pageControl.SetGridview(gvNoteHistory);
            pageControl.Update(items, pageIndex);
            upResult.Update();
        }

        protected void PageSearchChange(object sender, EventArgs e)
        {
            try
            {
                List<NoteHistoryData> result = SlmScr009Biz.SearchNoteHistory(txtTicketID.Text.Trim());
                var pageControl = (SLM.Application.Shared.GridviewPageController)sender;
                BindGridview(pageControl, result.ToArray(), pageControl.SelectedPageIndex);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        #endregion

        protected void btnAddNote_Click(object sender, EventArgs e)
        {
            try
            {
                cbSendEmail.Enabled = SlmScr009Biz.HasOwnerOrDelegate(txtTicketID.Text.Trim());
                if (!cbSendEmail.Enabled)
                    lblInfo.Text = "ข้อมูลผู้มุ่งหวังนี้ ยังไม่ถูกจ่ายงานให้ Telesales ดังนั้นการบันทึก Note ไม่สามารถส่ง Email ได้";
                else
                {
                    lblInfo.Text = "";
                    CheckEmailSubject();
                }

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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearPopupControl();
        }

        private void ClearPopupControl()
        {
            txtNoteDetail.Text = "";
            cbSendEmail.Checked = false;
            CheckEmailSubject();
            lblInfo.Text = "";
            alertEmailSubject.Text = "";
            alertEmailTo.Text = "";
            lblError.Text = "";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateData())
                {
                    if (txtNoteDetail.Text.Trim().Length > AppConstant.TextMaxLength)
                        throw new Exception("ไม่สามารถบันทึกรายละเอียด Note เกิน " + AppConstant.TextMaxLength.ToString() + " ตัวอักษรได้");

                    SlmScr009Biz.InsertNoteHistory(txtTicketID.Text.Trim(), cbSendEmail.Checked, txtEmailSubject.Text.Trim(), txtNoteDetail.Text.Trim(), GetEmailList(), HttpContext.Current.User.Identity.Name);
                    txtNoteDetail.Text = "";
                    cbSendEmail.Checked = false;
                    CheckEmailSubject();
                    lblInfo.Text = "";
                    ClearPopupControl();

                    mpePopup.Hide();
                    cbNoteFlag.Checked = true;
                    DoBindGridview();
                    AppUtil.ClientAlert(Page, "บันทึกข้อมูลเรียบร้อย");
                }
                else
                {
                    mpePopup.Show();
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
                mpePopup.Show();
            }
        }

        protected void cbNoteFlag_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SlmScr009Biz.ChangeNoteFlag(txtTicketID.Text.Trim(), cbNoteFlag.Checked, HttpContext.Current.User.Identity.Name);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void cbSendEmail_CheckedChanged(object sender, EventArgs e)
        {
            CheckEmailSubject();
            mpePopup.Show();
        }

        private bool ValidateEmail(string email)
        {
            string pattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(pattern);
            return reg.IsMatch(email);
        }

        private List<string> GetEmailList()
        {
            try
            {
                List<string> emailList = new List<string>();

                if (cbSendEmail.Checked && txtEmailTo.Text.Trim() != "")
                {
                    string[] arr_email = txtEmailTo.Text.Trim().Split(',');
                    foreach (string email in arr_email)
                    {
                        if (ValidateEmail(email.Trim()))
                        {
                            if (!emailList.Contains(email.Trim()))
                                emailList.Add(email.Trim());
                        }
                        else
                            throw new Exception("กรุณาระบุ E-mail ให้ถูกต้อง");
                    }
                }

                return emailList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ValidateData()
        {
            int i = 0;
            int emailSubjectMaxLength = 500;

            if (cbSendEmail.Checked)
            {
                if (txtEmailTo.Text.Trim() != "")
                {
                    alertEmailTo.Text = "";
                    string[] arr_email = txtEmailTo.Text.Trim().Split(',');
                    foreach (string email in arr_email)
                    {
                        if (email.Trim() == "")
                        {
                            alertEmailTo.Text = "กรุณาระบุ E-mail ให้ถูกต้อง";
                            i += 1;
                            break;
                        }
                        else
                        {
                            if (!ValidateEmail(email.Trim()))
                            {
                                alertEmailTo.Text = "กรุณาระบุ E-mail ให้ถูกต้อง";
                                i += 1;
                                break;
                            }
                        }
                    }
                }
                else
                    alertEmailTo.Text = "";

                if (txtEmailSubject.Text.Trim() == "")
                {
                    alertEmailSubject.Text = "กรุณากรอกข้อมูล Email Subject ก่อนบันทึก";
                    i += 1;
                }
                else
                {
                    if (txtEmailSubject.Text.Trim().Length > emailSubjectMaxLength)
                    {
                        alertEmailSubject.Text = "กรุณากรอกข้อมูล Email Subject ไม่เกิน " + emailSubjectMaxLength.ToString() + " ตัวอักษร";
                        i += 1;
                    }
                    else
                        alertEmailSubject.Text = "";
                }
            }

            if (txtNoteDetail.Text.Trim() == "")
            {
                lblError.Text = "กรุณากรอกข้อมูลบันทึก Note ก่อนบันทึก";
                i += 1;
            }
            else
            {
                if (txtNoteDetail.Text.Trim().Length > AppConstant.TextMaxLength)
                {
                    lblError.Text = "ไม่สามารถบันทึกรายละเอียด Note เกิน " + AppConstant.TextMaxLength.ToString() + " ตัวอักษรได้";
                    i += 1;
                }
                else
                    lblError.Text = "";
            }

            return i > 0 ? false : true;
        }
    }
}