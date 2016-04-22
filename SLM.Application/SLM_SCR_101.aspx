<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SaleLead.Master" AutoEventWireup="true"
    CodeBehind="SLM_SCR_101.aspx.cs" Inherits="SLM.Application.UploadLeadSearch" %>

<%@ Register Src="Shared/GridviewPageController.ascx" TagName="GridviewPageController"
    TagPrefix="uc1" %>
<%@ Register Src="Shared/TextDateMask.ascx" TagName="TextDateMask" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .ColInfo
        {
            font-weight: bold;
            width: 120px;
        }
        .ColInput
        {
            width: 250px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:Image ID="imgSearch" runat="server" ImageUrl="~/Images/hSearch.gif" />
    <asp:UpdatePanel ID="upSearch" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="2" cellspacing="0" border="0">
                <tr>
                    <td colspan="4" style="height: 2px;">
                    </td>
                </tr>
                <tr>
                    <td class="ColInfo">
                        ชื่อไฟล์
                    </td>
                    <td class="ColInput">
                        <asp:TextBox ID="txtFileName" runat="server" CssClass="Textbox" Width="250px"></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        สถานะไฟล์
                    </td>
                    <td class="ColInput">
                        <asp:DropDownList ID="cmbStatus" runat="server" Width="100px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="ColInfo">
                        วันที่ Upload Lead
                    </td>
                    <td class="ColInput">
                        <uc2:TextDateMask ID="tdmFormDatePopup" runat="server" Width="182px" />
                        &nbsp;
                        <asp:Label ID="alertFormDatePopup" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                    <td class="ColInfo">
                        ถึง
                    </td>
                    <td class="ColInput">
                        <uc2:TextDateMask ID="tdmToDatePopup" runat="server" runat="server" Width="182px" />
                        &nbsp;
                        <asp:Label ID="alertToDatePopup" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 10px;">
                    </td>
                </tr>
                <tr>
                    <td class="ColInfo">
                    </td>
                    <td class="ColInput">
                        <asp:Button ID="btnSearch" runat="server" CssClass="Button" Text="ค้นหา" Width="100px"
                            OnClick="btnSearch_Click" OnClientClick="DisplayProcessing();" />
                            <asp:Button ID="btnClear" runat="server" CssClass="Button" Text="Clear" Width="100px"
                            OnClick="btnClear_Click" OnClientClick="DisplayProcessing();" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <div class="Line">
    </div>
    <br />
    <asp:UpdatePanel ID="upResult" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Image ID="imgResult" runat="server" ImageUrl="~/Images/hResult.gif" ImageAlign="Top" />&nbsp;
            <asp:Button ID="btnAddPosition" runat="server" Text="Upload File" Width="150px" OnClientClick="DisplayProcessing();"
                CssClass="Button" Height="23px" OnClick="btnUpload_Click" />
           <span Style="margin-left:250px;"> Download Template
            <asp:HyperLink  ID="HyperLink1" runat="server" NavigateUrl="~/Download/uploadTemplate.xlsx" Target="_blank" ImageUrl="~/Images/Excel-icon.png">downloadTemplate</asp:HyperLink></span>
            <br />
            <uc1:GridviewPageController ID="pcTop" runat="server" OnPageChange="PageSearchChange"
                Width="900px" />
            <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" Width="900px"
                GridLines="Horizontal" BorderWidth="0px" OnRowDataBound="gvResult_RowDataBound"
                EnableModelValidation="True" EmptyDataText="<span style='color:Red;'>ไม่พบข้อมูล</span>">
                <Columns>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:ImageButton ID="imbView" runat="server" ImageUrl="~/Images/view.gif" CommandArgument='<%# Eval("slm_UploadLeadId") %>'
                                OnClick="imbView_Click" ToolTip="ดูรายละเอียด Upload Lead" />
                            <asp:ImageButton ID="imbEdit" runat="server" ImageUrl="~/Images/edit.gif" CommandArgument='<%# Eval("slm_UploadLeadId") %>'
                                OnClick="imbEdit_Click" ToolTip="แก้ไขข้อมูล Upload Lead" OnClientClick="DisplayProcessing();" />
                        </ItemTemplate>
                        <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Top" />
                        <HeaderStyle Width="50px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="slm_UploadLeadId" HeaderText="UploadLeadId" Visible="false">
                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                        <ItemStyle Width="100px" HorizontalAlign="Center" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="slm_FileName" HeaderText="ชื่อไฟล์">
                        <HeaderStyle Width="600px" HorizontalAlign="Center" />
                        <ItemStyle Width="600px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    
                    <asp:BoundField DataField="slm_UpdatedDate" HeaderText="วันที่ Upload ล่าสุด">
                        <HeaderStyle Width="200px" HorizontalAlign="Center" />
                        <ItemStyle Width="200px" HorizontalAlign="Center" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="slm_AssignedDate" HeaderText="วันที่แจกงาน">
                        <HeaderStyle Width="150px" HorizontalAlign="Center" />
                        <ItemStyle Width="150px" HorizontalAlign="Center" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="slm_UpdatedBy" HeaderText="ชื่อผู้ upload">
                        <HeaderStyle Width="250px" HorizontalAlign="Center" />
                        <ItemStyle Width="250px" HorizontalAlign="Center" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="slm_Status" HeaderText="สถานะ">
                        <HeaderStyle Width="90px" HorizontalAlign="Center" />
                        <ItemStyle Width="90px" HorizontalAlign="Center" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="slm_LeadCount" HeaderText="จำนวน Lead">
                        <HeaderStyle Width="90px" HorizontalAlign="Center" />
                        <ItemStyle Width="90px" HorizontalAlign="Center" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="slm_LeadAssignedCount" HeaderText="จำนวน Lead ที่จ่ายงานแล้ว">
                        <HeaderStyle Width="90px" HorizontalAlign="Center" />
                        <ItemStyle Width="90px" HorizontalAlign="Center" VerticalAlign="Top" />
                    </asp:BoundField>
                </Columns>
                <HeaderStyle CssClass="t_rowhead" />
                <RowStyle CssClass="t_row" BorderStyle="Dashed" />
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
