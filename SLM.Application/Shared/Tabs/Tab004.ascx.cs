using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SLM.Resource.Data;

namespace SLM.Application.Shared.Tabs
{
    public partial class Tab004 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        public void GetLeadData(LeadData lead)
        {
            try
            {
                txtAddressNo.Text = lead.AddressNo;
                txtBuildingName.Text = lead.BuildingName;
                txtFloor.Text = lead.Floor;
                txtSoi.Text = lead.Soi;
                txtStreet.Text = lead.Street;
                txtTambon.Text = lead.TambolName;
                txtAmphur.Text = lead.AmphurName;
                txtProvince.Text = lead.ProvinceName;
                txtPostalCode.Text = lead.PostalCode;
                txtIsCustomer.Text = (string.IsNullOrEmpty(lead.IsCustomer) ? "": (lead.IsCustomer.Trim() == "1" ? "เคย" : "ไม่เคย"));
                txtCusCode.Text = lead.CusCode;
                txtOccupation.Text = lead.OccupationName;
                if (lead.BaseSalary != null)
                    txtBaseSalary.Text = lead.BaseSalary.Value.ToString("#,##0.00");
                txtLicenseNo.Text = lead.LicenseNo;
                txtYearOfCar.Text = lead.YearOfCar;
                txtYearOfCarRegis.Text = lead.YearOfCarRegis;
                txtBrand.Text = lead.BrandName;
                if (lead.CarPrice != null)
                    txtCarPrice.Text = lead.CarPrice.Value.ToString("#,##0.00");
                txtModel.Text = lead.ModelName;
                txtSubmodel.Text = lead.SubModelName;
                if (lead.DownPayment != null)
                    txtDownPayment.Text = lead.DownPayment.Value.ToString("#,##0.00");
                txtDownPercent.Text = lead.DownPercent.ToString();
                if (lead.FinanceAmt != null)
                    txtFinanceAmt.Text = lead.FinanceAmt.Value.ToString("#,##0.00");
                txtPaymentTerm.Text = lead.PaymentTerm;
                txtPaymentType.Text = lead.PaymentName;
                if (lead.BalloonAmt != null)
                    txtBalloonAmt.Text = lead.BalloonAmt.Value.ToString("#,##0.00");
                txtBalloonPercent.Text = lead.BalloonPercent.ToString();
                txtProvinceRegis.Text = lead.ProvinceRegisName;
                if (!string.IsNullOrEmpty(lead.CoverageDate))
                {
                    if (lead.CoverageDate.Trim().Length == 8)
                        txtCoverageDate.Text = lead.CoverageDate.Substring(6, 2) + "/" + lead.CoverageDate.Substring(4, 2) + "/" + lead.CoverageDate.Substring(0, 4);
                }
                txtPlanType.Text = lead.PlanBancName;
                txtAccType.Text = lead.AccTypeName;
                txtAccPromotion.Text = lead.PromotionName;
                txtAccTerm.Text = lead.AccTerm;
                txtInterest.Text = lead.Interest;
                if(!string.IsNullOrEmpty(lead.Invest))
                    txtInvest.Text = Convert.ToDecimal(lead.Invest).ToString("#,##0.00");
                txtLoanOd.Text = lead.LoanOd;
                txtLoanOdTerm.Text = lead.LoanOdTerm;
                txtEbank.Text = (string.IsNullOrEmpty(lead.Ebank) ? "": (lead.Ebank.Trim() == "1" ? "ใช่" : "ไม่ใช่"));
                txtAtm.Text = (string.IsNullOrEmpty(lead.Atm) ? "": (lead.Atm.Trim() == "1" ? "ใช่" : "ไม่ใช่"));
                txtCompany.Text = lead.Company;

                lbPathLink.Text = lead.PathLink;
                if (!string.IsNullOrEmpty(lead.PathLink))
                {
                    if (lead.PathLink.IndexOf("http") < 0)
                        lbPathLink.OnClientClick = "window.open('http://" + lead.PathLink + "'), '_blank'";
                    else
                        lbPathLink.OnClientClick = "window.open('" + lead.PathLink + "'), '_blank'";
                }
                
                if (lead.Birthdate != null)
                    txtBitrhDate.Text = lead.Birthdate.Value.ToString("dd/MM/") + lead.Birthdate.Value.Year.ToString();

                txtCardType.Text = lead.CardTypeDesc;
                txtCitizenId.Text = lead.CitizenId;
                txtTopic.Text = lead.Topic;
                txtDetail.Text = lead.Detail;
                txtChannelName.Text = lead.ChannelDesc;
                if (lead.CreatedDateView != null)
                    txtCreateDate.Text = lead.CreatedDateView.Value.ToString("dd/MM/") + lead.CreatedDateView.Value.Year.ToString();
                txtBranchName.Text = lead.BranchName;
                txtBranchprod.Text = lead.Branchprod;
                if (lead.CreatedDateView != null)
                    txtCreateTime.Text = lead.CreatedDateView.Value.ToString("HH:mm:ss");

                if (!string.IsNullOrEmpty(lead.AvailableTime) && lead.AvailableTime.Trim().Length == 6)
                {
                    txtAvailableTime.Text = lead.AvailableTime.Substring(0, 2) + ":" + lead.AvailableTime.Substring(2, 2) + ":" + lead.AvailableTime.Substring(4, 2);
                }
                txtEmail.Text = lead.Email;
                txtInterestedProd.Text = (string.IsNullOrEmpty(lead.CarType) ? "" : (lead.CarType.Trim() == "0" ? "รถใหม่" :(lead.CarType.Trim() == "1"?"รถเก่า":"")));
                txtCreateBy.Text = lead.LeadCreateBy;
                txtDealerCode.Text = lead.DealerCode;
                txtDealerName.Text = lead.DealerName;
                txtDealerName.ToolTip = lead.DealerName;
                txtContractNoRefer.Text = lead.ContractNoRefer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/SLM_SCR_003.aspx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}