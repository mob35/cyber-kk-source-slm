<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SaleLead.Master" AutoEventWireup="true"
    CodeBehind="SLM_SCR_102.aspx.cs" Inherits="SLM.Application.UploadLeadManagement" %>

<%@ Register Src="Shared/GridviewPageController.ascx" TagName="GridviewPageController"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .required
        {
            color: red;
        }
        
        .style6
        {
            text-align: right;
        }
        
        .style4
        {
            font-family: Tahoma;
            font-size: 9pt;
            color: Red;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table>
                <tr>
                    <td style="width: 100px">
                        <b>ช่องทาง</b>
                    </td>
                    <td>
                        <asp:HiddenField ID="hidUploadLeadId" runat="server" />
                        <asp:HiddenField ID="hidLeadCount" runat="server" />
                        <asp:TextBox ID="txtType" runat="server" disabled="disabled" CssClass="Textbox" Width="250px"
                            Text="Referal"></asp:TextBox>
                    </td>
                    <td>
                        <b>Campaign</b> <span class="required">*</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbCampaign" runat="server" CssClass="Dropdownlist" Width="203px">
                        </asp:DropDownList>
                        <asp:Label ID="lblAlertCampaign" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>ชื่อไฟล์</b> <span class="required">*</span>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtFileName" runat="server" Width="380px" disabled="disabled" />
                        <asp:HiddenField ID="hidFileName" runat="server" />
                        <asp:Label ID="vtxtFileName" runat="server" CssClass="style4"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="lblUploadError" runat="server" ForeColor="Red" ViewStateMode="Disabled"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table>
        <tr id="trupload" runat="server">
            <td style="width: 100px">
                <b>แนบไฟล์</b> <span class="required">*</span>
            </td>
            <td colspan="3">
                <asp:FileUpload ID="fuLead" runat="server" Width="380px" size="45" />
                <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="Button" Width="80px"
                    OnClick="btnUpload_Click" />
            </td>
        </tr>
    </table>
    <div class="Line">
    </div>
    <asp:UpdatePanel ID="upResult" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:GridviewPageController ID="pcTop" runat="server" OnPageChange="PageSearchChange"
                Width="900px" />
            <asp:GridView runat="server" ID="gvResult" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="slm_UploadLeadDetailId" HeaderText="" Visible="false">
                        <ItemStyle Width="500px" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="ลำดับที่">
                        <ItemTemplate>
                            <asp:Label ID="No" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle Width="30px" HorizontalAlign="Center" />
                        <ItemStyle Width="30px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                     <asp:BoundField DataField="slm_ContractBranchName" HeaderText="สาขาของสัญญา">
                        <HeaderStyle Width="300px" HorizontalAlign="Center" />
                        <ItemStyle Width="300px" HorizontalAlign="left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="slm_OwnerLead" HeaderText="Owner lead">
                        <HeaderStyle Width="150px" HorizontalAlign="Center" />
                        <ItemStyle Width="150px" HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="slm_ThaiFirstName" HeaderText="ชื่อ">
                        <HeaderStyle Width="300px" HorizontalAlign="Center" />
                        <ItemStyle Width="300px" HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="slm_ThaiLastName" HeaderText="นามสกุล">
                        <HeaderStyle Width="300px" HorizontalAlign="Center" />
                        <ItemStyle Width="300px" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="slm_CardIdType" HeaderText="ประเภทลูกค้า">
                        <HeaderStyle Width="300px" HorizontalAlign="Center" />
                        <ItemStyle Width="300px" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="slm_CardId" HeaderText="เลขที่บัตร">
                        <HeaderStyle Width="150px" HorizontalAlign="Center" />
                        <ItemStyle Width="150px" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="slm_CustTelephoneMobile" HeaderText="หมายเลขโทรศัพท์ 1(มือถือ)">
                        <ItemStyle Width="150px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="slm_CustTelephoneHome" HeaderText="หมายเลขโทรศัพท์ 2">
                        <ItemStyle Width="150px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="slm_CustTelephoneOther" HeaderText="หมายเลขโทรศัพท์ 3">
                        <ItemStyle Width="150px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="slm_BrandName" HeaderText="ยี่ห้อรถ">
                        <ItemStyle Width="150px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="slm_ModelName" HeaderText="รุ่นรถ">
                        <ItemStyle Width="300px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="slm_ModelYear" HeaderText="ปีรถ">
                        <ItemStyle Width="150px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="slm_HpBscodeXsell" HeaderText="B-SCORE">
                        <ItemStyle Width="150px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="slm_Remark" HeaderText="Remark ">
                        <ItemStyle Width="500px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="slm_TicketID" HeaderText="Ticket ID">
                        <ItemStyle Width="150px" />
                    </asp:BoundField>
      
                </Columns>
                <HeaderStyle CssClass="t_rowhead" />
                <RowStyle CssClass="t_row" BorderStyle="Dashed" />
            </asp:GridView>
            <uc1:GridviewPageController ID="pcTopError" runat="server" OnPageChange="ErrorPageSearchChange"
                Width="900px" />
            <asp:GridView runat="server" ID="gvError" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="ValueField" HeaderText="แถวที่">
                        <ItemStyle Width="50px" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="ลำดับที่" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="No" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle Width="30px" HorizontalAlign="Center" />
                        <ItemStyle Width="30px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="TextField" HeaderText="รายละเอียด" HtmlEncode="false">
                        <ItemStyle Width="800px" />
                    </asp:BoundField>
                </Columns>
                <HeaderStyle CssClass="t_rowhead" />
                <RowStyle CssClass="t_row" BorderStyle="Dashed" />
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upButton" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="2" cellspacing="0" border="0" width="1200px">
                <tr style="height: 35px;">
                    <td style="width: 10px">
                    </td>
                    <td class="style2">
                    </td>
                    <td class="style6">
                    <asp:Button ID="btnExportAll" runat="server" Text="Export" Width="100px" OnClick="btnExport_Click"
                            OnClientClick="if ( confirm('ต้องการ Export Excel ใช่หรือไม่')) { DisplayProcessing(); return true; } else { return false; }" />&nbsp;
                        <asp:Button ID="btnDeleteAll" runat="server" Text="ลบ" Width="100px" OnClick="btnDeleteAll_Click"
                            OnClientClick="if ( confirm('ต้องการลบใช่หรือไม่')) { DisplayProcessing(); return true; } else { return false; }" />&nbsp;
                        <asp:Button ID="btnSaveAll" runat="server" Text="บันทึก" Width="100px" OnClick="btnSaveAll_Click"
                            OnClientClick="if (confirm('ต้องการบันทึกข้อมูลใช่หรือไม่')) { DisplayProcessing(); return true; } else { return false; }" />&nbsp;
                        <asp:Button ID="btnCancelAll" runat="server" Text="ยกเลิก" Width="100px" OnClick="btnClose_Click"
                            OnClientClick="return confirm('ต้องการยกเลิกใช่หรือไม่')" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>
