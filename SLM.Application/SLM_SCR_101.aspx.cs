using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SLM.Application.Utilities;
using SLM.Resource.Data;
using SLM.Biz;
using log4net;

namespace SLM.Application
{
    public partial class UploadLeadSearch : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(UploadLeadSearch));

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ((Label)Page.Master.FindControl("lblTopic")).Text = "ค้นหา Upload Lead";

            var statuss =  UploadLeadBiz.GetStatusList();
            cmbStatus.DataSource = statuss;
            cmbStatus.DataValueField = "ValueField";
            cmbStatus.DataTextField = "TextField";
            cmbStatus.DataBind();
            cmbStatus.Items.Insert(0, new ListItem("", "0"));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ScreenPrivilegeData priData = RoleBiz.GetScreenPrivilege(HttpContext.Current.User.Identity.Name, "SLM_SCR_101");
                    if (priData == null || priData.IsView != 1)
                    {
                        AppUtil.ClientAlertAndRedirect(Page, "คุณไม่มีสิทธิ์เข้าใช้หน้าจอนี้", "SLM_SCR_101.aspx");
                        return;
                    }

                    Page.Form.DefaultButton = btnSearch.UniqueID;
                    DoSearchUploadLead(0);
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DoSearchUploadLead(0);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtFileName.Text = "";
                cmbStatus.SelectedIndex = 0;
                tdmFormDatePopup.DateValue = DateTime.MinValue;
                tdmToDatePopup.DateValue = DateTime.MinValue;
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }
        

        private void DoSearchUploadLead(int pageIndex)
        {
            try
            {

                SearchUploadLeadCondition search = new SearchUploadLeadCondition();
                search.slm_FileName = txtFileName.Text.Trim();
                search.slm_Status = cmbStatus.SelectedItem.Value.Trim();
                search.UploadedDateForm = tdmFormDatePopup.DateValue.Year.ToString() + tdmFormDatePopup.DateValue.ToString("-MM-dd"); 
                search.UploadedDateTo = tdmToDatePopup.DateValue.Year.ToString() + tdmToDatePopup.DateValue.ToString("-MM-dd");

                List<SearchUploadLeadResult> list = UploadLeadBiz.SearchUploadLeadData(search);
                BindGridview(pcTop, list.ToArray(), pageIndex);
                //upResult.Update();
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
            //pageControl.GenerateRecordNumber(1, pageIndex);
            upResult.Update();
        }

        protected void PageSearchChange(object sender, EventArgs e)
        {
            try
            {
                var pageControl = (SLM.Application.Shared.GridviewPageController)sender;
                DoSearchUploadLead(pageControl.SelectedPageIndex);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }


        protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                SearchUploadLeadResult row = ((SearchUploadLeadResult)e.Row.DataItem);
                String status = row.slm_Status;
                if (status == "Submit")
                {
                    ((ImageButton)e.Row.FindControl("imbEdit")).Visible = true;
                }
                else
                    ((ImageButton)e.Row.FindControl("imbEdit")).Visible = false;
            }
        }
        #endregion  

        protected void imbEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //int index = Convert.ToInt32(((ImageButton)sender).CommandArgument);

                Response.Redirect("SLM_SCR_102.aspx?type=e&uploadleadid=" + ((ImageButton)sender).CommandArgument);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }
        protected void imbView_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //int index = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                Response.Redirect("SLM_SCR_102.aspx?type=v&uploadleadid=" + ((ImageButton)sender).CommandArgument);

            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e) {
            Response.Redirect("SLM_SCR_102.aspx");

        }
    }
}