using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SLM.Resource.Data;
using SLM.Biz;
using SLM.Application.Utilities;
using log4net;

namespace SLM.Application.Shared.Tabs
{
    public partial class Tab007 : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Tab007));

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void GetOwnerLogingList(string ticketId)
        {
            try
            {
                txtTicketId.Text = ticketId;
                List<OwnerLoggingData> result = SlmScr007Biz.SearchOwnerLogging(ticketId);
                BindGridview((SLM.Application.Shared.GridviewPageController)pcTop, result.ToArray(), 0);
                upResult.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Page Control

        private void BindGridview(SLM.Application.Shared.GridviewPageController pageControl, object[] items, int pageIndex)
        {
            pageControl.SetGridview(gvOwnerLogging);
            pageControl.Update(items, pageIndex);
            upResult.Update();
        }

        protected void PageSearchChange(object sender, EventArgs e)
        {
            try
            {
                List<OwnerLoggingData> result = SlmScr007Biz.SearchOwnerLogging(txtTicketId.Text.Trim());
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