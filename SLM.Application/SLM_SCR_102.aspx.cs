using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.OleDb;
using SLM.Biz;
using SLM.Application.Utilities;
using SLM.Resource.Data;
using log4net;
using System.Web.Configuration;

namespace SLM.Application
{
    public partial class UploadLeadManagement : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(UploadLeadManagement));

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            var campaigns = CampaignMasterBiz.GetCampaignMasterList();
            cmbCampaign.DataSource = campaigns;
            cmbCampaign.DataValueField = "slm_CampaignCode";
            cmbCampaign.DataTextField = "slm_CampaignName";
            cmbCampaign.DataBind();
            cmbCampaign.Items.Insert(0, new ListItem("", "0"));


        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ((Label)Page.Master.FindControl("lblTopic")).Text = "Upload Lead Management";

                ScreenPrivilegeData priData = RoleBiz.GetScreenPrivilege(HttpContext.Current.User.Identity.Name, "SLM_SCR_102");
                if (priData == null || priData.IsView != 1)
                {
                    AppUtil.ClientAlertAndRedirect(Page, "คุณไม่มีสิทธิ์เข้าใช้หน้าจอนี้", "SLM_SCR_102.aspx");
                    return;
                }

                gvError.Visible = false;
                gvResult.Visible = false;
                pcTopError.Visible = false;
                pcTop.Visible = false;
                initControl();
            }else{
                vtxtFileName.Text = "";
                lblAlertCampaign.Text = "";
            }
        }

        private void initControl()
        {
            if (Request.QueryString["type"] == "v")
            {
                trupload.Visible = false;
                //trfilename.Visible = true;
                btnDeleteAll.Visible = false;
                btnSaveAll.Visible = false;

                cmbCampaign.Enabled = false;

                if (Request.QueryString["uploadleadid"] == null && Request.QueryString["uploadleadid"] == "")
                {
                    //btnDeleteAll.Visible = false;
                }
                else
                {
                    DoSearchRanking(Request.QueryString["uploadleadid"]);

                    DoSearchDetail(0);

                }
            }
            else if (Request.QueryString["type"] == null || Request.QueryString["type"] == "")
            {
                trupload.Visible = true;
                //trfilename.Visible = true;
                btnDeleteAll.Visible = false;
            }
            else
            {
                trupload.Visible = true;
                //trfilename.Visible = false;
                if (Request.QueryString["uploadleadid"] == null && Request.QueryString["uploadleadid"] == "")
                {
                    btnDeleteAll.Visible = false;
                }
                else
                {
                    DoSearchRanking(Request.QueryString["uploadleadid"]);

                    DoSearchDetail(0);
                    btnDeleteAll.Visible = true;
                }
            }

        }

        private void DoSearchRanking(string uploadleadid)
        {
            UploadLeadData uploadLead = UploadLeadBiz.SearchUploadLeadData(uploadleadid);

            hidUploadLeadId.Value = uploadLead.slm_UploadLeadId.ToString();
            hidLeadCount.Value = uploadLead.slm_LeadCount.ToString();

            txtFileName.Text = uploadLead.slm_FileName.ToString();

            hidFileName.Value = uploadLead.slm_FileName.ToString();
            cmbCampaign.SelectedValue = uploadLead.slm_CampaignCode;
        }

        private void DoSearchDetail(int pageIndex)
        {
            try
            {
                //UploadLeadBiz biz = new UploadLeadBiz();


                List<UploadLeadDetailData> list = UploadLeadBiz.GetUploadLeadDetailList(hidUploadLeadId.Value.Trim());
                ViewState["Result"] = list;
                gvError.Visible = false;
                gvResult.Visible = true;
                pcTopError.Visible = false;
                pcTop.Visible = true;
                BindGridview(gvResult, pcTop, list.ToArray(), 0);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                //if (cmbCampaign.SelectedValue == "0") throw new Exception("กรุณา Campaign");

                if (fuLead.HasFile)
                {
                    var ext = Path.GetExtension(fuLead.FileName).ToLower();
                    if (ext != ".xls")
                    {
                        lblUploadError.Text = "กรุณาระบุไฟล์ให้ถูกต้อง (.xls)";
                        txtFileName.Text = hidFileName.Value;
                        return;
                    }
                    using (OleDbConnection conn = new OleDbConnection())
                    {
                        DataTable dt = new DataTable();
                        string filename = Path.GetFileName(fuLead.FileName);

                        string fullname = Server.MapPath(WebConfigurationManager.AppSettings["UploadLeadPath"] + filename);
                        if (hidUploadLeadId.Value != "")
                        {
                            if (filename != hidFileName.Value)
                            {
                                txtFileName.Text = hidFileName.Value;
                                AppUtil.ClientAlert(this, "ชื่อไฟล์ต้องตรงกับชื่อไฟล์เดิม");
                                return;
                            }
                        }
                        else if (UploadLeadBiz.CheckFileExist(filename, AppUtil.SafeInt(hidUploadLeadId.Value)))
                        {

                            //vtxtName.Text = "ชื่อลำดับที่นี้มีอยู่แล้วในระบบแล้ว";
                            AppUtil.ClientAlert(this, "ไฟล์นี้มีอยู่แล้วในระบบแล้ว");
                            txtFileName.Text = hidFileName.Value;
                            return;
                        }

                        CreateFolderIfNeeded(Server.MapPath(WebConfigurationManager.AppSettings["UploadLeadPath"]));

                        fuLead.SaveAs(fullname);

                        conn.ConnectionString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=No;IMEX=1;'", fullname);
                        OleDbCommand cmd = new OleDbCommand();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM [Sheet1$]";

                        OleDbDataAdapter adp = new OleDbDataAdapter(cmd);
                        adp.Fill(dt);

                        List<ControlListData> errLst = new List<ControlListData>();
                        // check error
                        List<UploadLeadDetailData> UploadLeadDetailDatas = new List<UploadLeadDetailData>();
                        if ((dt.Rows.Count <= 3001))
                        {

                            for (int i = 1; i < dt.Rows.Count; i++)
                            {
                                var row = dt.Rows[i];
                                string clmer = "";

                                try
                                {
                                    // check item
                                    if (row[0].ToString().Trim() == "") clmer += (clmer == "" ? "" : ",<br/>") + "Column A: กรุณาระบุ สาขาของสัญญา ";
                                    if (row[1].ToString().Trim() == "") clmer += (clmer == "" ? "" : ",<br/>") + "Column B: กรุณาระบุ Owner lead";
                                    if (row[2].ToString().Trim() == "") clmer += (clmer == "" ? "" : ",<br/>") + "Column C: กรุณาระบุ ชื่อลูกค้า";
                                    if (row[3].ToString().Trim() == "") clmer += (clmer == "" ? "" : ",<br/>") + "Column D: กรุณาระบุ นามสกุลลูกค้า";
                                    if (row[4].ToString().Trim() == "") clmer += (clmer == "" ? "" : ",<br/>") + "Column E: กรุณาระบุ ประเภทลูกค้า";
                                    
                                    if (row[5].ToString().Trim() == "") clmer += (clmer == "" ? "" : ",<br/>") + "Column F: กรุณาระบุ เลขที่บัตร";
                                    if (row[5].ToString().Trim().Length > 13) clmer += (clmer == "" ? "" : ",<br/>") + "Column F: เลขที่บัตรประชาชน/นิติบุคคล ไม่ถูกต้อง";
                                    if (row[6].ToString().Trim() == "") clmer += (clmer == "" ? "" : ",<br/>") + "Column G: หมายเลขโทรศัพท์ 1(มือถือ)";
                                    if (row[7].ToString().Trim() == "") clmer += (clmer == "" ? "" : ",<br/>") + "Column H: หมายเลขโทรศัพท์ 2";
                                    if (row[8].ToString().Trim() == "") clmer += (clmer == "" ? "" : ",<br/>") + "Column I: หมายเลขโทรศัพท์ 3";
                                    if (row[9].ToString().Trim() == "") clmer += (clmer == "" ? "" : ",<br/>") + "Column J: ยี่ห้อรถ";
                                    if (row[10].ToString().Trim() == "") clmer += (clmer == "" ? "" : ",<br/>") + "Column K: รุ่นรถ";
                                    if (row[11].ToString().Trim() == "") clmer += (clmer == "" ? "" : ",<br/>") + "Column L: ปีรถ";
                                    if (row[12].ToString().Trim() == "") clmer += (clmer == "" ? "" : ",<br/>") + "Column M: B-SCORE";
                                    if (row[13].ToString().Trim() == "") clmer += (clmer == "" ? "" : ",<br/>") + "Column N: Remark";
                                    if (row[14].ToString().Trim() == "") clmer += (clmer == "" ? "" : ",<br/>") + "Column O: Ticket ID";
                                }
                                catch (Exception ex)
                                {
                                    errLst.Add(new ControlListData() { TextField = ex.Message, ValueField = (i + 1).ToString() });
                                }


                                if (clmer != "")
                                {
                                    errLst.Add(new ControlListData() { TextField = clmer, ValueField = (i + 1).ToString() });
                                }
                                else
                                {
                                    UploadLeadDetailData uploadleaddetaildata = new UploadLeadDetailData();

                                    uploadleaddetaildata.slm_ContractBranchName = row[0].ToString().Trim();
                                    uploadleaddetaildata.slm_OwnerLead = row[1].ToString().Trim();
                                    uploadleaddetaildata.slm_ThaiFirstName = row[2].ToString().Trim();
                                    uploadleaddetaildata.slm_ThaiLastName = row[3].ToString().Trim();
                                    uploadleaddetaildata.slm_CardIdType = row[4].ToString().Trim();
                                    uploadleaddetaildata.slm_CardId = row[5].ToString().Trim();
                                    uploadleaddetaildata.slm_CustTelephoneMobile = row[6].ToString().Trim();
                                    uploadleaddetaildata.slm_CustTelephoneHome = row[7].ToString().Trim();
                                    uploadleaddetaildata.slm_CustTelephoneOther = row[8].ToString().Trim();
                                    uploadleaddetaildata.slm_BrandName = row[9].ToString().Trim();
                                    uploadleaddetaildata.slm_ModelName = row[10].ToString().Trim();
                                    uploadleaddetaildata.slm_ModelYear = row[11].ToString().Trim();
                                    uploadleaddetaildata.slm_HpBscodeXsell = row[12].ToString().Trim();
                                    uploadleaddetaildata.slm_Remark = row[13].ToString().Trim();
                                    uploadleaddetaildata.slm_TicketID = row[14].ToString().Trim();



                                    if (UploadLeadDetailDatas.Where(u => u.slm_CardId == row[5].ToString().Trim()).Count() == 0)
                                    {
                                        UploadLeadDetailDatas.Add(uploadleaddetaildata);
                                    }
                                    else
                                    {
                                        errLst.Add(new ControlListData() { TextField = "เลขที่บัตรประชาชน/นิติบุคคล " + row[5].ToString().Trim() + " ซ้ำ", ValueField = (i + 1).ToString() });
                                    }

                                }

                            }

                            if (errLst.Count > 0)
                            {
                                // show error
                                //gvError.DataSource = errLst;
                                //gvError.DataBind();

                                BindGridview(gvError, pcTopError, errLst.ToArray(), 0);
                                ViewState["Error"] = errLst;
                                AppUtil.ClientAlert(this, "upload ไม่สำเร็จ");
                                gvError.Visible = true;
                                gvResult.Visible = false;
                                pcTopError.Visible = true;
                                pcTop.Visible = false;


                            }
                            else
                            {
                                if (UploadLeadDetailDatas.Count == 0)
                                {
                                    AppUtil.ClientAlert(this, "ไม่พบข้อมูลในไฟล์");
                                }
                                else
                                {

                                    // save data
                                    gvError.Visible = false;
                                    gvResult.Visible = true;
                                    pcTopError.Visible = false;
                                    pcTop.Visible = true;

                                    ViewState["Result"] = UploadLeadDetailDatas;
                                    hidLeadCount.Value = UploadLeadDetailDatas.Count().ToString();
                                    BindGridview(gvResult, pcTop, UploadLeadDetailDatas.ToArray(), 0);
                                    txtFileName.Text = filename;
                                    hidFileName.Value = filename;
                                    AppUtil.ClientAlert(this, "Upload Complete");
                                }
                            }
                        }
                        else
                        {
                            AppUtil.ClientAlert(this, "upload ไม่สำเร็จ ข้อมูลต้องไม่มากกว่า 3000 Record");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppUtil.ClientAlert(this, ex.Message);
            }
        }

        public static bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            }
            return result;
        }


        protected void PageSearchChange(object sender, EventArgs e)
        {
            try
            {
                var pageControl = (SLM.Application.Shared.GridviewPageController)sender;
                BindGridview(gvResult, pcTop, ((List<UploadLeadDetailData>)ViewState["Result"]).ToArray(), pageControl.SelectedPageIndex);

            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void ErrorPageSearchChange(object sender, EventArgs e)
        {
            try
            {
                var pageControl = (SLM.Application.Shared.GridviewPageController)sender;
                BindGridview(gvResult, pcTop, ((List<ControlListData>)ViewState["Errot"]).ToArray(), pageControl.SelectedPageIndex);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void BindGridview(GridView gv, SLM.Application.Shared.GridviewPageController pageControl, object[] items, int pageIndex)
        {
            pageControl.SetGridview(gv);
            pageControl.Update(items, pageIndex);
            pageControl.GenerateRecordNumber(1, pageIndex);
            upResult.Update();
        }

        protected void btnDeleteAll_Click(object sender, EventArgs e)
        {
            try
            {
                //if (UploadLeadBiz.CheckDeleteUploadLead(hidUploadLeadId.Value))
                //{
                UploadLeadBiz.DeleteUploadLead(AppUtil.SafeInt(hidUploadLeadId.Value),HttpContext.Current.User.Identity.Name);

                AppUtil.ClientAlertAndRedirect(Page, "บันทึกข้อมูล UploadLead สำเร็จ", "SLM_SCR_101.aspx");
                //}
                //else
                //{
                //    AppUtil.ClientAlert(Page, "ข้อมูลที่เลือกไม่สามารถลบได้");
                //}
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateData())
                {
                    UploadLeadData data = new UploadLeadData();

                    data.slm_UploadLeadId = AppUtil.SafeInt(hidUploadLeadId.Value);
                    data.slm_LeadCount = AppUtil.SafeInt(hidLeadCount.Value);
                    data.slm_Status = "Submit";
                    //data.FileName = txtFileName.Text;
                    data.slm_FileName = hidFileName.Value;
                    data.slm_CampaignCode = cmbCampaign.SelectedItem.Value;


                    if (hidUploadLeadId.Value == "")
                    {
                        UploadLeadBiz.AddUploadLead(data, (List<UploadLeadDetailData>)ViewState["Result"], HttpContext.Current.User.Identity.Name);
                    }
                    else
                    {
                        UploadLeadBiz.EditUploadLead(data, (List<UploadLeadDetailData>)ViewState["Result"], HttpContext.Current.User.Identity.Name);
                    }

                    AppUtil.ClientAlertAndRedirect(Page, "บันทึกข้อมูล UploadLead สำเร็จ", "SLM_SCR_101.aspx");
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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                
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
                Response.Redirect("SLM_SCR_101.aspx");
            }
            catch (Exception ex)
            {
                AppUtil.ClientAlert(Page, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }


        private bool ValidateData()
        {
            int i = 0;
            //************************************Windows Username********************************************
            if (hidFileName.Value.Trim() == "")
            {
                vtxtFileName.Text = "กรุณา Upload Lead";
                vtxtFileName.ForeColor = System.Drawing.Color.Red;
                i += 1;
            }
            else {
                vtxtFileName.Text = "";
            }

            if (cmbCampaign.SelectedItem.Value == "0")
            {
                lblAlertCampaign.Text = "กรุณาระบุ Campaign";
                lblAlertCampaign.ForeColor = System.Drawing.Color.Red;
                i += 1;
            }
            else {
                lblAlertCampaign.Text = "";
            }

            if (i > 0)
            {
                upMain.Update();
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CheckDate(string dtString)
        {
            var chk = dtString.Split('-');
            if (chk.Length != 3) return false;
            int d, m, y;
            int.TryParse(chk[0], out y);
            int.TryParse(chk[1], out m);
            int.TryParse(chk[2], out d);

            if (y == 0 || m == 0 || d == 0 || m > 12) return false;
            else return true;

        }
    }
}