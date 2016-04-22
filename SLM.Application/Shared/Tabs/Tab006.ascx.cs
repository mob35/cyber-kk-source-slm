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
    public partial class Tab006 : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Tab006));

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void GetExistingProductList(string citizenId)
        {
            try
            {
                txtCitizenId.Text = citizenId;
                //First Load
                DoSearchExistingProductList(0, "", SortDirection.Ascending);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DoSearchExistingProductList(int pageIndex, string sortExpression, SortDirection sortDirection)
        {
            try
            {
                List<ExistingProductData> result = SlmScr006Biz.SearchExistingProduct(txtCitizenId.Text.Trim());

                if (sortExpression == "ProductGroup")
                {
                    if (sortDirection == SortDirection.Ascending)
                        result = result.OrderBy(p => p.ProductGroup).ToList();
                    else
                        result = result.OrderByDescending(p => p.ProductGroup).ToList();
                }
                else if (sortExpression == "ContactNo")
                {
                    if (sortDirection == SortDirection.Ascending)
                        result = result.OrderBy(p => p.ContactNo).ToList();
                    else
                        result = result.OrderByDescending(p => p.ContactNo).ToList();
                }
                else if (sortExpression == "StartDate")
                {
                    if (sortDirection == SortDirection.Ascending)
                        result = result.OrderBy(p => p.StartDate).ToList();
                    else
                        result = result.OrderByDescending(p => p.StartDate).ToList();
                }
                else if (sortExpression == "EndDate")
                {
                    if (sortDirection == SortDirection.Ascending)
                        result = result.OrderBy(p => p.EndDate).ToList();
                    else
                        result = result.OrderByDescending(p => p.EndDate).ToList();
                }
                else if (sortExpression == "Status")
                {
                    if (sortDirection == SortDirection.Ascending)
                        result = result.OrderBy(p => p.Status).ToList();
                    else
                        result = result.OrderByDescending(p => p.Status).ToList();
                }

                BindGridview((SLM.Application.Shared.GridviewPageController)pcTop, result.ToArray(), pageIndex);
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
            pageControl.SetGridview(gvExistProduct);
            pageControl.Update(items, pageIndex);
            pageControl.GenerateRecordNumber(0, pageIndex);
            upResult.Update();
        }

        protected void PageSearchChange(object sender, EventArgs e)
        {
            try
            {
                //List<ExistingProductData> result = SlmScr006Biz.SearchExistingProduct(txtCitizenId.Text.Trim());
                //var pageControl = (SLM.Application.Shared.GridviewPageController)sender;
                //BindGridview(pageControl, result.ToArray(), pageControl.SelectedPageIndex);

                var pageControl = (SLM.Application.Shared.GridviewPageController)sender;
                DoSearchExistingProductList(pageControl.SelectedPageIndex, SortExpressionProperty, SortDirectionProperty);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        #endregion

        #region Sorting

        protected void gvExistProduct_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (SortExpressionProperty != e.SortExpression)         //เมื่อเปลี่ยนคอลัมน์ในการ sort
                    SortDirectionProperty = SortDirection.Ascending;
                else
                {
                    if (SortDirectionProperty == SortDirection.Ascending)
                        SortDirectionProperty = SortDirection.Descending;
                    else
                        SortDirectionProperty = SortDirection.Ascending;
                }

                SortExpressionProperty = e.SortExpression;
                DoSearchExistingProductList(0, SortExpressionProperty, SortDirectionProperty);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        public SortDirection SortDirectionProperty
        {
            get
            {
                if (ViewState["SortingState"] == null)
                {
                    ViewState["SortingState"] = SortDirection.Ascending;
                }
                return (SortDirection)ViewState["SortingState"];
            }
            set
            {
                ViewState["SortingState"] = value;
            }
        }

        public string SortExpressionProperty
        {
            get
            {
                if (ViewState["ExpressionState"] == null)
                {
                    ViewState["ExpressionState"] = string.Empty;
                }
                return ViewState["ExpressionState"].ToString();
            }
            set
            {
                ViewState["ExpressionState"] = value;
            }
        }

        #endregion
    }
}