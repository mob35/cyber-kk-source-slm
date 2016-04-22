using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SLM.Application.Utilities;
using SLM.Resource.Data;
using SLM.Biz;
using log4net;

namespace SLM.Application.Shared.Tabs
{
    public partial class Tab005 : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Tab005));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }
        }

        public void GetExistingLeadList(string citizenId, string telNo1, string ticketId)
        {
            try
            {
                txtTelNo1.Text = telNo1;
                txtCitizenId.Text = citizenId;
                txtTicketId.Text = ticketId;
                List<SearchLeadResult> result = SlmScr005Biz.SearchExistingLead(txtCitizenId.Text.Trim(), txtTicketId.Text.Trim());
                BindGridview((SLM.Application.Shared.GridviewPageController)pcTop, result.ToArray(), 0);
                upResult.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void imbView_Click(object sender, EventArgs e)
        {
            Response.Redirect("SLM_SCR_004.aspx?ticketid=" + ((ImageButton)sender).CommandArgument + "&ReturnUrl=" + Server.UrlEncode(Request["ReturnUrl"]) );
        }

        #region Page Control

        private void BindGridview(SLM.Application.Shared.GridviewPageController pageControl, object[] items, int pageIndex)
        {
            pageControl.SetGridview(gvExistingLead);
            pageControl.Update(items, pageIndex);
            upResult.Update();
        }

        protected void PageSearchChange(object sender, EventArgs e)
        {
            try
            {
                List<SearchLeadResult> result = SlmScr005Biz.SearchExistingLead(txtCitizenId.Text.Trim(), txtTicketId.Text.Trim());
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
    }
}