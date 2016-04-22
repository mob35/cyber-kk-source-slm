using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SLM.Application.Utilities;
using SLM.Biz;
using SLM.Resource;
using SLM.Resource.Data;
using log4net;

namespace SLM.Application
{
    public partial class SLM_SCR_027 : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(SLM_SCR_027));

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ((Label)Page.Master.FindControl("lblTopic")).Text = "ค้นหาข้อมูลวันหยุดสาขา";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ScreenPrivilegeData priData = RoleBiz.GetScreenPrivilege(HttpContext.Current.User.Identity.Name, "SLM_SCR_027");
                    if (priData == null || priData.IsView != 1)
                    {
                        AppUtil.ClientAlertAndRedirect(Page, "คุณไม่มีสิทธิ์เข้าใช้หน้าจอนี้", "SLM_SCR_003.aspx");
                        return;
                    }

                    Page.Form.DefaultButton = btnSearch.UniqueID;
                    InitialControl();
                    DoSearchCalendarBranch(0);
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
            cmbBranchSearch.DataSource = BranchBiz.GetBranchList(SLMConstant.Branch.All);
            cmbBranchSearch.DataTextField = "TextField";
            cmbBranchSearch.DataValueField = "ValueField";
            cmbBranchSearch.DataBind();
            cmbBranchSearch.Items.Insert(0, new ListItem("", ""));
        }

        private void BindListBox(ListBox listBox, List<ControlListData> source)
        {
            listBox.DataSource = source;
            listBox.DataTextField = "TextField";
            listBox.DataValueField = "ValueField";
            listBox.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DoSearchCalendarBranch(0);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void DoSearchCalendarBranch(int pageIndex)
        {
            try
            {
                string holidayDate = "";
                if (tdmHolidayDateSearch.DateValue.Year != 1)
                    holidayDate = tdmHolidayDateSearch.DateValue.Year.ToString() + tdmHolidayDateSearch.DateValue.ToString("-MM-dd");

                List<CalendarBranchData> list = CalendarBranchBiz.SearchCalendarBranch(holidayDate, txtHolidayDescSearch.Text.Trim(), cmbBranchSearch.SelectedItem.Value);
                BindGridview(pcTop, list.ToArray(), pageIndex);
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
            pageControl.GenerateRecordNumber(1, pageIndex);
            upResult.Update();
        }

        protected void PageSearchChange(object sender, EventArgs e)
        {
            try
            {
                var pageControl = (SLM.Application.Shared.GridviewPageController)sender;
                DoSearchCalendarBranch(pageControl.SelectedPageIndex);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        #endregion

        #region Popup

        protected void btnAddBranchHoliday_Click(object sender, EventArgs e)
        {
            try
            {
                cbEdit.Checked = false;
                tdmHolidayDatePopup.Enabled = true;

                BindListBox(lboxBranchAll, BranchBiz.GetBranchList(SLMConstant.Branch.All));
                lblBranchAllTotal.Text = lboxBranchAll.Items.Count.ToString();

                lboxBranchSelected.Items.Clear();
                lblBranchSelectedTotal.Text = lboxBranchSelected.Items.Count.ToString();

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

        protected void btnSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                lboxBranchSelected.Items.AddRange(lboxBranchAll.Items.OfType<ListItem>().ToArray());
                List<ControlListData> list = lboxBranchSelected.Items.OfType<ListItem>().OrderBy(p => p.Text).Select(p => new ControlListData { TextField = p.Text, ValueField = p.Value }).ToList();
                BindListBox(lboxBranchSelected, list);
                lblBranchSelectedTotal.Text = lboxBranchSelected.Items.Count.ToString();

                lboxBranchAll.Items.Clear();
                lblBranchAllTotal.Text = lboxBranchAll.Items.Count.ToString();

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

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (lboxBranchAll.GetSelectedIndices().Count() > 0)
                {
                    List<ListItem> selectedList = new List<ListItem>();

                    List<int> indices = lboxBranchAll.GetSelectedIndices().ToList();
                    foreach (int index in indices)
                    {
                        selectedList.Add(lboxBranchAll.Items[index]);
                    }

                    //Remove selecteditems from lboxBranchAll
                    foreach (ListItem item in selectedList)
                    {
                        lboxBranchAll.Items.Remove(item);
                    }

                    lboxBranchSelected.Items.AddRange(selectedList.ToArray());
                    List<ControlListData> list = lboxBranchSelected.Items.OfType<ListItem>().OrderBy(p => p.Text).Select(p => new ControlListData { TextField = p.Text, ValueField = p.Value }).ToList();
                    BindListBox(lboxBranchSelected, list);

                    lblBranchAllTotal.Text = lboxBranchAll.Items.Count.ToString();
                    lblBranchSelectedTotal.Text = lboxBranchSelected.Items.Count.ToString();
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

        protected void btnDeselectAll_Click(object sender, EventArgs e)
        {
            try
            {
                lboxBranchAll.Items.AddRange(lboxBranchSelected.Items.OfType<ListItem>().ToArray());
                List<ControlListData> list = lboxBranchAll.Items.OfType<ListItem>().OrderBy(p => p.Text).Select(p => new ControlListData { TextField = p.Text, ValueField = p.Value }).ToList();
                BindListBox(lboxBranchAll, list);
                lblBranchAllTotal.Text = lboxBranchAll.Items.Count.ToString();

                lboxBranchSelected.Items.Clear();
                lblBranchSelectedTotal.Text = lboxBranchSelected.Items.Count.ToString();

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

        protected void btnDeselect_Click(object sender, EventArgs e)
        {
            try
            {
                if (lboxBranchSelected.GetSelectedIndices().Count() > 0)
                {
                    List<ListItem> deSelectedList = new List<ListItem>();

                    List<int> indices = lboxBranchSelected.GetSelectedIndices().ToList();
                    foreach (int index in indices)
                    {
                        deSelectedList.Add(lboxBranchSelected.Items[index]);
                    }

                    //Remove deSelecteditems from lboxBranchSelected
                    foreach (ListItem item in deSelectedList)
                    {
                        lboxBranchSelected.Items.Remove(item);
                    }

                    lboxBranchAll.Items.AddRange(deSelectedList.ToArray());
                    List<ControlListData> list = lboxBranchAll.Items.OfType<ListItem>().OrderBy(p => p.Text).Select(p => new ControlListData { TextField = p.Text, ValueField = p.Value }).ToList();
                    BindListBox(lboxBranchAll, list);

                    lblBranchAllTotal.Text = lboxBranchAll.Items.Count.ToString();
                    lblBranchSelectedTotal.Text = lboxBranchSelected.Items.Count.ToString();
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

        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    if (cbEdit.Checked)
                        CalendarBranchBiz.UpdateData(tdmHolidayDatePopup.DateValue, txtHolidayDescPopup.Text.Trim(), GetSelectedBranchCode(), HttpContext.Current.User.Identity.Name);
                    else
                        CalendarBranchBiz.InsertData(tdmHolidayDatePopup.DateValue, txtHolidayDescPopup.Text.Trim(), GetSelectedBranchCode(), HttpContext.Current.User.Identity.Name);

                    AppUtil.ClientAlert(Page, "บันทึกข้อมูลเรียบร้อย");

                    ClearPopupControl();
                    mpePopup.Hide();

                    DoSearchCalendarBranch(0);
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

        private List<string> GetSelectedBranchCode()
        {
            try
            {
                List<string> branchCodeList = new List<string>();
                foreach (ListItem item in lboxBranchSelected.Items)
                {
                    branchCodeList.Add(item.Value);
                }
                return branchCodeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void imbEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                cbEdit.Checked = true;

                int index = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                string holidayDate = ((Label)gvResult.Rows[index].FindControl("lblHolidayDate")).Text.Trim();
                string holidayDesc = ((Label)gvResult.Rows[index].FindControl("lblHolidayDesc")).Text.Trim();

                if (!string.IsNullOrEmpty(holidayDate))
                {
                    string[] obj = holidayDate.Split('/');
                    tdmHolidayDatePopup.DateValue = new DateTime(int.Parse(obj[2]), int.Parse(obj[1]), int.Parse(obj[0]));
                    tdmHolidayDatePopup.Enabled = false;
                    txtHolidayDescPopup.Text = holidayDesc;

                    SetBranchForEdit(tdmHolidayDatePopup.DateValue.Year.ToString() + tdmHolidayDatePopup.DateValue.ToString("-MM-dd"));

                    upPopup.Update();
                    mpePopup.Show();
                }
                else
                    throw new Exception("ไม่พบวันหยุดที่ต้องการแก้ไข");
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void SetBranchForEdit(string holidayDate)
        {
            try
            {
                var list = CalendarBranchBiz.SearchCalendarBranch(holidayDate, "", "");
                List<ControlListData> selectedList = list.Select(p => new ControlListData { TextField = p.BranchName, ValueField = p.BranchCode }).OrderBy(p => p.TextField).ToList();
                BindListBox(lboxBranchSelected, selectedList);

                List<ControlListData> allBranchList = BranchBiz.GetBranchList(SLMConstant.Branch.All);
                foreach (ControlListData data in selectedList)
                {
                    ControlListData obj = allBranchList.Where(p => p.ValueField == data.ValueField).FirstOrDefault();
                    if (obj != null)
                        allBranchList.Remove(obj);
                }

                BindListBox(lboxBranchAll, allBranchList);

                lblBranchAllTotal.Text = lboxBranchAll.Items.Count.ToString();
                lblBranchSelectedTotal.Text = lboxBranchSelected.Items.Count.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearPopupControl();
            mpePopup.Hide();
        }

        private void ClearPopupControl()
        {
            cbEdit.Checked = false;
            tdmHolidayDatePopup.DateValue = new DateTime();
            tdmHolidayDatePopup.Enabled = true;
            txtHolidayDescPopup.Text = "";
            lboxBranchAll.Items.Clear();
            lboxBranchSelected.Items.Clear();

            alertHolidayDatePopup.Text = "";
            alertHolidayDescPopup.Text = "";
            alertBranchSelected.Text = "";
        }

        private bool ValidateInput()
        {
            int i = 0;
            int holidayDescMaxLength = 100;

            if (tdmHolidayDatePopup.DateValue.Year == 1)
            {
                alertHolidayDatePopup.Text = "กรุณาเลือก วันหยุด";
                i += 1;
            }
            else
                alertHolidayDatePopup.Text = "";

            if (txtHolidayDescPopup.Text.Trim() == "")
            {
                alertHolidayDescPopup.Text = "กรุณาระบุ รายละเอียดวันหยุด";
                i += 1;
            }
            else
            {
                if (txtHolidayDescPopup.Text.Trim().Length > holidayDescMaxLength)
                {
                    alertHolidayDescPopup.Text = "กรุณาระบุ รายละเอียดวันหยุดไม่เกิน " + holidayDescMaxLength.ToString() + " ตัวอักษร";
                    i += 1;
                }
                else
                    alertHolidayDescPopup.Text = "";
            }

            if (cbEdit.Checked == false && lboxBranchSelected.Items.Count == 0)
            {
                alertBranchSelected.Text = "กรุณาเลือก สาขา";
                i += 1;
            }
            else
                alertBranchSelected.Text = "";

            return i > 0 ? false : true;
        }
    }
}