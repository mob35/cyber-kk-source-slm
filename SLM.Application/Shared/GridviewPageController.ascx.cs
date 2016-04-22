﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SLM.Resource.Data;

namespace SLM.Application.Shared
{
    public partial class GridviewPageController : System.Web.UI.UserControl
    {
        GridView _gvMain = null;
        public event EventHandler PageChange;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        public void SetGridview(GridView gv)
        {
            _gvMain = gv;
        }

        public int SelectedPageIndex
        {
            get { return cmbPage.SelectedIndex; }
        }

        public Unit Width
        {
            set { pnPageControl.Width = value; }
        }

        public bool SetVisible
        {
            set { pnPageControl.Visible = value; }
        }

        public void Update(object[] items, int page_index)
        {
            try
            {
                //ส่ง Default Page Size จาก Config File
                Update(items, page_index, int.Parse(System.Configuration.ConfigurationManager.AppSettings["GridviewPageSize"]));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(object[] items, int page_index, int page_size)
        {
            try
            {
                int num_of_page = 0;
                int upper_bound = 0;
                int lower_bound = 0;
                ArrayList source = new ArrayList();

                //prevent error ในกรณีหน้าจอ search ไม่พบข้อมูลใดๆ, เมื่อมีการกดไปหน้า Add จะมีการเก็บ pageindex = -1 (เป็น -1 เนื่องจาก search ไม่พบข้อมูลใดๆ จึงไม่มีการ Bind Item) ใน session
                //เมื่อมีการ Add ข้อมูลเช่น เพิ่ม Lead, เมื่อบันทึกแล้วกลับไปหน้าจอค้นหา ระบบจะนำ pageindex ที่เก็บไว้ใน session ไปทำการค้นหา ซึ่งถ้า pageindex = -1 จะทำให้ error ได้
                page_index = page_index > -1 ? page_index : 0;

                if (items.Count() > 0)
                {
                    num_of_page = (int)Math.Ceiling(Convert.ToDouble(items.Count()) / Convert.ToDouble(page_size));

                    if (page_index >= num_of_page)
                        page_index = 0;

                    lower_bound = page_size * page_index;
                    upper_bound = (page_size * (page_index + 1)) - 1;

                    for (int i = lower_bound; i <= upper_bound; i++)
                    {
                        if ((i + 1) > items.Count())
                        {
                            upper_bound = i - 1;
                            break;
                        }
                        source.Add(items[i]);
                    }
                }

                UpdatePageControl(num_of_page, page_index, lower_bound, upper_bound, items.Count());
                BindGridview(source);
                pnPageControl.Visible = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateMonitoring(object[] items, int page_index)
        {
            try
            {
                int page_size = int.Parse(System.Configuration.ConfigurationManager.AppSettings["GridviewPageSizeMonitoring"]);
                int num_of_page = 0;
                int upper_bound = 0;
                int lower_bound = 0;
                ArrayList source = new ArrayList();

                //prevent error ในกรณีหน้าจอ search ไม่พบข้อมูลใดๆ, เมื่อมีการกดไปหน้า Add จะมีการเก็บ pageindex = -1 (เป็น -1 เนื่องจาก search ไม่พบข้อมูลใดๆ จึงไม่มีการ Bind Item) ใน session
                //เมื่อมีการ Add ข้อมูลเช่น เพิ่ม Lead, เมื่อบันทึกแล้วกลับไปหน้าจอค้นหา ระบบจะนำ pageindex ที่เก็บไว้ใน session ไปทำการค้นหา ซึ่งถ้า pageindex = -1 จะทำให้ error ได้
                page_index = page_index > -1 ? page_index : 0;

                if (items.Count() > 0)
                {
                    num_of_page = (int)Math.Ceiling(Convert.ToDouble(items.Count()) / Convert.ToDouble(page_size));

                    if (page_index >= num_of_page)
                        page_index = 0;

                    lower_bound = page_size * page_index;
                    upper_bound = (page_size * (page_index + 1)) - 1;

                    for (int i = lower_bound; i <= upper_bound; i++)
                    {
                        if ((i + 1) > items.Count())
                        {
                            upper_bound = i - 1;
                            break;
                        }
                        source.Add(items[i]);
                    }
                }

                UpdatePageControl(num_of_page, page_index, lower_bound, upper_bound, items.Count());
                BindGridview(source);
                pnPageControl.Visible = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Unit GetGridviewWidth()
        {
            double width = 0;
            foreach (DataControlField df in _gvMain.Columns)
            {
                width += df.ItemStyle.Width.Value;
            }
            return Unit.Pixel(Convert.ToInt32(width));
        }

        //public void Update<T>(List<T> items, int page_index)
        //{
        //    try
        //    {
        //        int page_size = int.Parse(System.Configuration.ConfigurationManager.AppSettings["GridviewPageSize"]);
        //        int num_of_page = 0;
        //        int upper_bound = 0;
        //        int lower_bound = 0;
        //        ArrayList source = new ArrayList();
        //        List<ExistingProductData> exProdData = null;

        //        if (typeof(T) == typeof(ExistingProductData))
        //        {
        //            exProdData = items.Cast<ExistingProductData>().ToList();
        //        }

               
        //        if (items.Count() > 0)
        //        {
        //            num_of_page = (int)Math.Ceiling(Convert.ToDouble(items.Count()) / Convert.ToDouble(page_size));
        //            lower_bound = page_size * page_index;
        //            upper_bound = (page_size * (page_index + 1)) - 1;

        //            for (int i = lower_bound; i <= upper_bound; i++)
        //            {
        //                if ((i + 1) > items.Count())
        //                {
        //                    upper_bound = i - 1;
        //                    break;
        //                }
                        
                        
        //                source.Add(items[i]);
        //            }
        //        }

        //        UpdatePageControl(num_of_page, page_index, lower_bound, upper_bound, items.Count());
        //        BindGridview(source);
        //        pnPageControl.Visible = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private void BindGridview(ArrayList source)
        {
            if (_gvMain != null)
            {
                _gvMain.DataSource = source;
                _gvMain.DataBind();
            }
        }

        private void BindGridview(DataTable dtSource)
        {
            if (_gvMain != null)
            {
                _gvMain.DataSource = dtSource;
                _gvMain.DataBind();
            }
        }

        protected void cmbPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (PageChange != null)
                    PageChange(this, e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void lnbFirst_Click(object sender, EventArgs e)
        {
            try
            {
                cmbPage.SelectedIndex = 0;
                if (PageChange != null)
                    PageChange(this, e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void lnbNext_Click(object sender, EventArgs e)
        {
            try
            {
                cmbPage.SelectedIndex = cmbPage.SelectedIndex + 1;
                if (PageChange != null)
                    PageChange(this, e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void lnbBack_Click(object sender, EventArgs e)
        {
            try
            {
                cmbPage.SelectedIndex = cmbPage.SelectedIndex - 1;
                if (PageChange != null)
                    PageChange(this, e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void lnbLast_Click(object sender, EventArgs e)
        {
            try
            {
                cmbPage.SelectedIndex = cmbPage.Items.Count - 1;
                if (PageChange != null)
                    PageChange(this, e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdatePageControl(int num_of_page, int page_index, int lower_bound, int upper_bound, int item_count)
        {
            try
            {
                cmbPage.Items.Clear();

                if (item_count > 0)
                {
                    for (int i = 0; i < num_of_page; i++)
                    {
                        cmbPage.Items.Add(new ListItem((i + 1).ToString(), i.ToString()));
                    }
                    cmbPage.SelectedIndex = cmbPage.Items.IndexOf(cmbPage.Items.FindByValue(page_index.ToString()));
                    lblTotalPage.Text = num_of_page.ToString("#,##0");

                    lnbFirst.Enabled = (cmbPage.SelectedIndex != 0);
                    lnbFirst.OnClientClick = lnbFirst.Enabled ? "DisplayProcessing();" : "";

                    lnbBack.Enabled = (cmbPage.SelectedIndex != 0);
                    lnbBack.OnClientClick = lnbBack.Enabled ? "DisplayProcessing();" : "";

                    lnbNext.Enabled = (cmbPage.SelectedIndex < cmbPage.Items.Count - 1);
                    lnbNext.OnClientClick = lnbNext.Enabled ? "DisplayProcessing();" : "";

                    lnbLast.Enabled = (cmbPage.SelectedIndex < cmbPage.Items.Count - 1);
                    lnbLast.OnClientClick = lnbLast.Enabled ? "DisplayProcessing();" : "";

                    lblSummary.Text = "รายการที่ " + (lower_bound + 1).ToString("#,##0") + " - " + (upper_bound + 1).ToString("#,##0") + " จาก <font class='hilightGreen'><b>" + item_count.ToString("#,##0") + "</b></font> รายการ";
                }
                else
                {
                    lblTotalPage.Text = "0";

                    lnbFirst.Enabled = false;
                    lnbFirst.OnClientClick = "";
                    lnbBack.Enabled = false;
                    lnbBack.OnClientClick = "";
                    lnbNext.Enabled = false;
                    lnbNext.OnClientClick = "";
                    lnbLast.Enabled = false;
                    lnbLast.OnClientClick = "";

                    lblSummary.Text = "รายการที่ 0 - 0 จาก <font class='hilightGreen'><b>0</b></font> รายการ";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GenerateRecordNumber(int columnIndexOfRecordNumber, int gvPageIndex)
        {
            GenerateRecordNumber(columnIndexOfRecordNumber, gvPageIndex, int.Parse(System.Configuration.ConfigurationManager.AppSettings["GridviewPageSize"]));
        }
        public void GenerateRecordNumber(int columnIndexOfRecordNumber, int gvPageIndex, int gvPageSize)
        {
            if (_gvMain != null)
            {
                for (int i = 0; i < _gvMain.Rows.Count; i++)
                {
                    _gvMain.Rows[i].Cells[columnIndexOfRecordNumber].Text = ((gvPageIndex * gvPageSize) + (i + 1)).ToString();
                }
            }
        }
    }
}