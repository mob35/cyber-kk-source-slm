<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SaleLead.Master" AutoEventWireup="true" CodeBehind="SLM_SCR_004.aspx.cs" Inherits="SLM.Application.SLM_SCR_004" %>
<%@ Register src="Shared/Tabs/Tab005.ascx" tagname="Tab005" tagprefix="uc1" %>
<%@ Register src="Shared/Tabs/Tab008.ascx" tagname="Tab008" tagprefix="uc2" %>
<%@ Register src="Shared/Tabs/Tab004.ascx" tagname="Tab004" tagprefix="uc3" %>
<%@ Register src="Shared/Tabs/Tab009.ascx" tagname="Tab009" tagprefix="uc4" %>
<%@ Register src="Shared/Tabs/Tab007.ascx" tagname="Tab007" tagprefix="uc5" %>
<%@ Register src="Shared/Tabs/Tab006.ascx" tagname="Tab006" tagprefix="uc6" %>
<%@ Register src="Shared/GridviewPageController.ascx" tagname="GridviewPageController" tagprefix="uc7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .ColInfo
        {
            font-weight:bold;
            width:220px;
        }
        .ColInputView
        {
            width:150px;
        }
        .ColCheckBox
        {
            width:160px;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function ConfirmSave() {

            return confirm('ต้องการบันทึกใช่หรือไม่');
        }
        function GetScreenHeight() {

            document.getElementById('<%= txtScreenWidth.ClientID %>').value = screen.width;
            document.getElementById('<%= txtScreenHeight.ClientID %>').value = screen.height;
            DisplayProcessing();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:Image ID="imgSearch" runat="server" ImageUrl="~/Images/hGeneral.gif" />
    <asp:UpdatePanel ID="upMainData" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="2" cellspacing="0" border="0">
                <tr>
                    <td class="ColInfo">
                        Ticket ID
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtTicketID" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                        <asp:TextBox ID="txtCitizenId" runat="server" Visible="false" Width="10px" ></asp:TextBox>
                        <asp:TextBox ID="txtTelNo1" runat="server" Visible="false" Width="10px" ></asp:TextBox>
                        <asp:TextBox ID="txtChannelId" runat="server" Visible="false" Width="10px" ></asp:TextBox>
                        <asp:TextBox ID="txtCampaignId" runat="server" Visible="false" Width="10px" ></asp:TextBox>
                        <asp:TextBox ID="txtUserLoginChannelId" runat="server" Visible="false"  Width="10px" ></asp:TextBox>
                        <asp:TextBox ID="txtUserLoginChannelDesc" runat="server" Visible="false"  Width="10px" ></asp:TextBox>
                        <asp:TextBox ID="txtIsCOC" runat="server" Visible="false" Width="10px"></asp:TextBox>
                        <asp:TextBox ID="txtLoginEmpCode" runat="server" Visible="false" Width="10px"></asp:TextBox>
                        <asp:TextBox ID="txtLoginNameTH" runat="server" Visible="false" Width="10px"></asp:TextBox>
                        <asp:TextBox ID="txtLoginStaffTypeId" runat="server" Visible="false" Width="10px"></asp:TextBox>
                        <asp:TextBox ID="txtLoginStaffTypeDesc" runat="server" Visible="false" Width="10px"></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        สถานะของ lead
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtstatus" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        สถานะย่อยของ Lead
                    </td>
                    <td class="ColInputView">
                         <asp:TextBox ID="txtExternalSubStatusDesc" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="ColInfo">
                        ชื่อ
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtFirstname" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        นามสกุล
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtLastname" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        หมายเลขโทรศัพท์ 1(มือถือ)
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtTelNo_1" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="ColInfo">
                        แคมเปญ
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtCampaignName" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        ผลิตภัณฑ์/บริการ ที่สนใจ
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtInterestedProd" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        หมายเลขโทรศัพท์ 2
                    </td>
                    <td class="ColInputView">
                       <asp:TextBox ID="txtTelNo2" runat="server" CssClass="TextboxView" ReadOnly="true" Width="72px" ></asp:TextBox>
                        <asp:Label ID="label1" runat="server" Width="10px" CssClass="LabelC" Text="-"></asp:Label>
                        <asp:TextBox ID="txtExt2" runat="server" CssClass="TextboxView" Width="38px" ReadOnly="true" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="ColInfo">
                        วันเวลาที่ได้ติดต่อ Lead ล่าสุด (SLM)
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtContactLatestDate" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        วันเวลาที่ได้รับมอบหมายล่าสุด (SLM)
                    </td>
                    <td class="ColInputView"> 
                        <asp:TextBox ID="txtAssignDate" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo">หมายเลขโทรศัพท์ 3</td>
                    <td class="ColInputView">
                         <asp:TextBox ID="txtTelNo3" runat="server" CssClass="TextboxView" ReadOnly="true" Width="72px" ></asp:TextBox>
                        <asp:Label ID="label2" runat="server" Width="10px" CssClass="LabelC" Text="-"></asp:Label>
                        <asp:TextBox ID="txtExt3" runat="server" CssClass="TextboxView" Width="38px" ReadOnly="true" ></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                    <td class="ColInfo">
                        วันเวลาที่ติดต่อ Lead ครั้งแรก (SLM)
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtContactFirstDate" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        วันเวลาที่ได้รับมอบหมายล่าสุด (COC)
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtCOCAssignDate" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo"></td>
                    <td class="ColInputView"></td>
                </tr>
                <tr>
                    <td class="ColInfo">
                        Owner Branch
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtOwnerBranch" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        Owner Lead
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtOwnerLead" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo"></td>
                    <td class="ColInputView"></td>
                </tr>
                <tr>
                    <td class="ColInfo">
                        Delegate Branch
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtDelegateBranch" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                        
                    </td>
                    <td class="ColInfo">
                        Delegate Lead
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtDelegateLead" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        
                    </td>
                    <td class="ColInputView" >
                        
                    </td>
                </tr>
                <tr>
                    <td class="ColInfo">
                        Marketing Owner
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtMarketingOwner" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px"  Text=""></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        สถานะของ COC
                    </td>
                    <td colspan="3" class="ColInputView">
                        <asp:TextBox ID="txtCocStatus" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" Text="" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="ColInfo">
                        LastOwner
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtLastOwner" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px"  Text=""></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        Team
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtCocTeam" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px"  Text=""></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        <asp:Button ID="btnBack" runat="server" Text="ย้อนกลับ" CssClass="Button" Width="90px" onclick="btnBack_Click"  />&nbsp;&nbsp;
                        <asp:ImageButton ID="imbCal" runat="server" Width="20px" Height="20px" ImageUrl="~/Images/Calculator.png" ImageAlign="AbsMiddle" ToolTip="Calculator"  />&nbsp;&nbsp;
                        <asp:ImageButton id="imbDoc" runat="server" ImageUrl="~/Images/Document.png" Width="20px" Height="20px" ImageAlign="AbsMiddle" ToolTip="แนบเอกสาร"  />&nbsp;&nbsp;
                        <asp:ImageButton ID="imbOthers" runat="server" Width="20px" Height="20px" ImageUrl="~/Images/Others.png" ImageAlign="AbsMiddle" ToolTip="เรียกดูข้อมูลเพิ่มเติม" />&nbsp;&nbsp;
                        <asp:ImageButton id="imbCopyLead" runat="server" 
                            ImageUrl="~/Images/page_copy.png" Width="20px" Height="20px" 
                            ImageAlign="AbsMiddle" ToolTip="คัดลอกข้อมูลผู้มุ่งหวัง" 
                            onclick="imbCopyLead_Click" />
                    </td>
                    <td class="ColInputView" >
                        
                    </td>
                </tr>
                 <tr>
                    <td valign="top" class="style2">รายละเอียด</td>
                    <td colspan="5">
                         <asp:TextBox ID="txtDetail" runat="server" CssClass="TextboxView" Width="770px" Height="70px" TextMode ="MultiLine"  ReadOnly="true" ></asp:TextBox>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br /><br />
    <asp:UpdatePanel ID="upHistory" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="1190px" >
                <tr>
                    <td>
                        <asp:Image ID="imgHistory" runat="server" ImageUrl="~/Images/CampaignFinal.gif" ImageAlign="Top" />&nbsp;&nbsp;
                        <asp:Button ID="btnAdd" runat="server" Visible="false" Text ="แคมเปญทั้งหมด" Width="120px" CssClass="Button" onclick="btnAdd_Click" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btnOfferCampaign" runat="server" Visible="false" Text ="แนะนำแคมเปญ" Width="120px" CssClass="Button" onclick="btnOfferCampaign_Click" /> 
                        <asp:Button ID="btnAllCampaign" runat="server" Text ="แคมเปญทั้งหมด" Width="120px" CssClass="Button" OnClientClick="GetScreenHeight()" onclick="btnAllCampaign_Click" />
                        <asp:TextBox ID="txtScreenHeight" runat="server" CssClass="Hidden"></asp:TextBox>
                        <asp:TextBox ID="txtScreenWidth" runat="server" CssClass="Hidden"></asp:TextBox>
                    </td>
                    <td align="right">
                        รวมทั้งสิ้น
                        <asp:Label ID="lbSum" runat="server"></asp:Label>
                        รายการ
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlHistory" runat="server" ScrollBars ="Auto" Height="170px" Width="1190px" BorderStyle="Solid" BorderWidth="1px" >
                <asp:GridView ID="gvCampaign" runat="server" AutoGenerateColumns="False" 
                    GridLines="Horizontal" BorderWidth="0px"  Width="1160px"
                    EnableModelValidation="True" 
                    EmptyDataText="<center><span style='color:Red;'>ไม่พบข้อมูล</span></center>" 
                    onrowdatabound="gvCampaign_RowDataBound"  > 
                    <Columns>
                        <asp:TemplateField HeaderText="CampaignFinalId">
                            <ItemTemplate>
                                <asp:Label ID="lbCampaignFinalId"  runat="server" Text='<%#Bind("CampaignFinalId") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lbCampaignFinalIdEdit" runat="server" Text='<%#Bind("CampaignFinalId") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemStyle CssClass="Hidden" />
                            <HeaderStyle CssClass="Hidden" />
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="No.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <ItemStyle Width="40px" HorizontalAlign="Center" VerticalAlign="Top"/>
                            <HeaderStyle Width="40px" HorizontalAlign="Center"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ชื่อ Product/Campaign">
                            <ItemTemplate>
                                <asp:Label ID="lbCampaign"  runat="server" Text='<%#Bind("CampaignName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="220px" HorizontalAlign="Left" VerticalAlign="Top"/>
                            <HeaderStyle Width="220px" HorizontalAlign="Center"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="รายละเอียด">
                            <ItemTemplate>
                                <asp:Label ID="lbCampaignDetail" runat="server" Text='<%#Bind("CampaignDetail") %>'></asp:Label>
                                <asp:LinkButton ID="lbShowCampaignDesc" runat="server" Text="อ่านต่อ" CommandArgument='<%# Eval("CampaignId") %>' Visible="false" ></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="600px" HorizontalAlign="Left" VerticalAlign="Top"/>
                            <HeaderStyle Width="600px" HorizontalAlign="Center"/>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="วันที่ดำเนินการ">
                            <ItemTemplate>
                                <%# Eval("CreatedDate") != null ? Convert.ToDateTime(Eval("CreatedDate")).ToString("dd/MM/") + Convert.ToDateTime(Eval("CreatedDate")).Year.ToString() + " " + Convert.ToDateTime(Eval("CreatedDate")).ToString("HH:mm:ss") : ""%>
                            </ItemTemplate>
                            <ItemStyle Width="140px" HorizontalAlign="Center" VerticalAlign="Top"/>
                            <HeaderStyle Width="140px" HorizontalAlign="Center"/>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="ผู้ดำเนินการ">
                            <ItemTemplate>
                                <asp:Label ID="lbCreatedByName"  runat="server" Text='<%#Bind("CreatedByName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="160px" HorizontalAlign="Left" VerticalAlign="Top"/>
                            <HeaderStyle Width="160px" HorizontalAlign="Center"/>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="t_rowhead" />
                    <RowStyle CssClass="t_row" BorderStyle="Dashed"/>
                </asp:GridView>
            </asp:Panel>
        </ContentTemplate> 
    </asp:UpdatePanel> 
    <br />
    <asp:UpdatePanel ID="upTabMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <act:TabContainer ID="tabMain" runat="server" ActiveTabIndex="0" Width="1190px" >
                <act:TabPanel ID="tab004" runat="server">
                    <HeaderTemplate>
                        <asp:Label ID="lblHeader004" runat="server" Text="&nbsp;ข้อมูล Lead&nbsp;" CssClass="tabHeaderText"></asp:Label>                 
                    </HeaderTemplate>
                    <ContentTemplate>
                        <uc3:Tab004 ID="tabLeadInfo" runat="server" />
                    </ContentTemplate>
                </act:TabPanel>
                <act:TabPanel ID="tab005" runat="server" >
                    <HeaderTemplate>
                        <asp:Label ID="lblHeader005" runat="server" Text="&nbsp;Existing Lead&nbsp;" CssClass="tabHeaderText"></asp:Label>                 
                    </HeaderTemplate>
                    <ContentTemplate>
                        <uc1:Tab005 ID="tabExistingLead" runat="server" />
                    </ContentTemplate>         
                </act:TabPanel>
                <act:TabPanel ID="tab006" runat="server" >
                    <HeaderTemplate>
                        <asp:Label ID="lblHeader006" runat="server" Text="&nbsp;Existing Product&nbsp;" CssClass="tabHeaderText"></asp:Label>                 
                    </HeaderTemplate>
                    <ContentTemplate>
                        <uc6:Tab006 ID="tabExistingProduct" runat="server" />
                    </ContentTemplate>
                </act:TabPanel>
                <act:TabPanel ID="tab007" runat="server" >
                    <HeaderTemplate>
                        <asp:Label ID="lblHeader007" runat="server" Text="&nbsp;Owner Logging&nbsp;" CssClass="tabHeaderText"></asp:Label>                 
                    </HeaderTemplate>
                    <ContentTemplate>
                        <uc5:Tab007 ID="tabOwnerLogging" runat="server" />
                    </ContentTemplate>
                </act:TabPanel>
                <act:TabPanel ID="tab008" runat="server" >
                    <HeaderTemplate>
                        <asp:Label ID="lblHeader008" runat="server" Text="&nbsp;Activity&nbsp;" CssClass="tabHeaderText"></asp:Label>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <uc2:Tab008 ID="tabPhoneCallHistory" runat="server" OnUpdatedDataChanged="UpdateStatusDesc" />
                    </ContentTemplate>
                </act:TabPanel>
                <act:TabPanel ID="tab009" runat="server" >
                    <HeaderTemplate>
                        <asp:Label ID="lblHeader009" runat="server" Text="&nbsp;Note History&nbsp;" CssClass="tabHeaderText"></asp:Label>                 
                    </HeaderTemplate>
                    <ContentTemplate>
                        <uc4:Tab009 ID="tabNoteHistory" runat="server" />
                    </ContentTemplate>
                </act:TabPanel>
            </act:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upPopupSearchCampaign" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Button runat="server" ID="btnPopupSearchCampaign" CssClass="Hidden"/>
	    <asp:Panel runat="server" ID="pnPopupSearchCampaign" style="display:none" CssClass="modalBoxlSearchBundleCampaign" ScrollBars="Auto">
            <table>
                <tr>
                    <td style="color:Red; padding-left:15px; height:20px; vertical-align:bottom">* เลือกแคมเปญได้มากกว่า 1 รายการ แล้วกดปุ่ม "เลือก"
                    </td>
                </tr>
            </table>
            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Image ID="Image2" runat="server" ImageUrl="~/Images/SearchBundle.jpg" />
            <table cellpadding="2" cellspacing="0" border="0">
                <tr>
                    <td style="width:20px; height:212px;"></td>
                    <td style="height:212px; vertical-align:top;">
                        <asp:TextBox ID="txtBundleCampaignIdList" runat="server" Width="30px" Visible="false"></asp:TextBox>
                        <uc7:GridviewPageController ID="pcGridBundleCampaign" runat="server" OnPageChange="PageSearchChangeBundleCampaign" Width="910px" />
                        <asp:GridView ID="gvBundleCampaign" runat="server" AutoGenerateColumns="False" Width="910px"
                            GridLines="Horizontal" BorderWidth="1px" EnableModelValidation="True" EmptyDataText="<span style='color:Red;'>ไม่พบข้อมูล</span>" 
                            onrowdatabound="gvBundleCampaign_RowDataBound"  >
                            <Columns>
                                <asp:TemplateField HeaderText="เลือก">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbSelectCampaign" runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="40px" HorizontalAlign="Center"/>
                                    <ItemStyle Width="40px" HorizontalAlign="Center" VerticalAlign="Top"  />
                                </asp:TemplateField>
                                <asp:BoundField DataField="ProductGroupName" HeaderText="กลุ่มผลิตภัณฑ์/บริการ"  >
                                    <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                                    <ItemStyle Width="150px" HorizontalAlign="Left" VerticalAlign="Top"  />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="ผลิตภัณฑ์/บริการ">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("ProductName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="140px" HorizontalAlign="Center"/>
                                    <ItemStyle Width="140px" HorizontalAlign="Left" VerticalAlign="Top"  />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="แคมเปญ">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCampaignName" runat="server" Text='<%# Eval("CampaignName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                                    <ItemStyle Width="150px" HorizontalAlign="Left" VerticalAlign="Top"  />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="รายละเอียดแคมเปญ">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCampaignDesc" runat="server" Text='<%# Eval("CampaignDesc") %>'></asp:Label>
                                        <asp:LinkButton ID="lbShowCampaignDesc" runat="server" Text="อ่านต่อ" CommandArgument='<%# Eval("CampaignId") %>' Visible="false" ></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="180px" HorizontalAlign="Center"/>
                                    <ItemStyle Width="180px" HorizontalAlign="Left" VerticalAlign="Top"  />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="วันที่เริ่มต้น">
                                    <ItemTemplate>
                                        <%# Eval("StartDate") != null ? Convert.ToDateTime(Eval("StartDate")).ToString("dd/MM/") + Convert.ToDateTime(Eval("StartDate")).Year.ToString() : ""%>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" HorizontalAlign="Center" VerticalAlign="Top" />
                                    <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="วันที่สิ้นสุด">
                                    <ItemTemplate>
                                        <%# Eval("EndDate") != null ? Convert.ToDateTime(Eval("EndDate")).ToString("dd/MM/") + Convert.ToDateTime(Eval("EndDate")).Year.ToString() : ""%>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" HorizontalAlign="Center" VerticalAlign="Top" />
                                    <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="แนะนำ">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCmt" runat="server" ForeColor="Red" Font-Bold="true" Text='<%# Eval("Recommend") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="50px" HorizontalAlign="Center"/>
                                    <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Top"  />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CampaignId">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCampaignId" runat="server" Text='<%# Eval("CampaignId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle CssClass="Hidden" />
                                    <HeaderStyle CssClass="Hidden" />
                                    <ItemStyle CssClass="Hidden" />
                                    <FooterStyle CssClass="Hidden" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ProductGroupId">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductGroupId" runat="server" Text='<%# Eval("ProductGroupId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle CssClass="Hidden" />
                                    <HeaderStyle CssClass="Hidden" />
                                    <ItemStyle CssClass="Hidden" />
                                    <FooterStyle CssClass="Hidden" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ProductId">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductId" runat="server" Text='<%# Eval("ProductId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle CssClass="Hidden" />
                                    <HeaderStyle CssClass="Hidden" />
                                    <ItemStyle CssClass="Hidden" />
                                    <FooterStyle CssClass="Hidden" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="t_rowhead" />
                            <RowStyle CssClass="t_row" BorderStyle="Dashed"/>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            
            <hr style="border-top:1px solid gray; border-bottom-style:none; border-left-style:none; border-right-style:none;" />
            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Image ID="Image3" runat="server" ImageUrl="~/Images/SearchCampaignAll.jpg" ImageAlign="AbsMiddle" />
            <asp:Label ID="lblPopupInfo" runat="server" ForeColor="Red"></asp:Label>
            <asp:UpdatePanel ID="upPopupSearchCampaignInner" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table cellpadding="2" cellspacing="0" border="0">
                        <tr>
                            <td style="width:20px; "></td>
                            <td>
                            </td>
                            <td style="width:220px; font-weight:bold;">กลุ่มผลิตภัณฑ์/บริการ</td>
                            <td style="width:220px; font-weight:bold;">ผลิตภัณฑ์/บริการ</td>
                            <td style="font-weight:bold;">แคมเปญ</td>
                        </tr>
                        <tr>
                            <td style="width:20px;"></td>
                            <td>
                                <asp:RadioButton ID="rbSearchByCombo" runat="server" GroupName="Campaign" AutoPostBack="true"
                                    Checked="true" oncheckedchanged="rbSearchByCombo_CheckedChanged"  />
                            </td>
                            <td style="width:220px;">
                                <asp:DropDownList ID="cmbProductGroup" runat="server" AutoPostBack="true" CssClass="Dropdownlist" Width="200px"
                                onselectedindexchanged="cmbProductGroup_SelectedIndexChanged" ></asp:DropDownList>
                            </td>
                            <td style="width:220px;">
                                <asp:DropDownList ID="cmbProduct" runat="server" AutoPostBack="true" CssClass="Dropdownlist" Width="200px"
                                onselectedindexchanged="cmbProduct_SelectedIndexChanged" ></asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbCampaign" runat="server" CssClass="Dropdownlist" Width="200px"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:20px;"></td>
                            <td>
                            </td>
                            <td colspan="3" style="font-weight:bold;" >คำที่ต้องการค้นหา</td>
                        </tr>
                        <tr >
                            <td style="width:20px;"></td>
                            <td>
                                <asp:RadioButton ID="rbSearchByText" runat="server" GroupName="Campaign" AutoPostBack="true" 
                                    oncheckedchanged="rbSearchByText_CheckedChanged"  />
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtFullSearchCampaign" runat="server" CssClass="Textbox" Width="420px"></asp:TextBox>
                            </td>
                            <td>
                        
                            </td>
                        </tr>
                        <tr><td colspan="5" style="height:1px;"></td></tr>
                        <tr>
                            <td style="width:20px;"></td>
                            <td colspan="4">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnSearchCampaign" runat="server" Text="ค้นหา" Width="80px" 
                                    CssClass="Button" onclick="btnSearchCampaign_Click" OnClientClick="DisplayProcessing()" />
                            </td>
                        </tr>
                    </table>
                    <table cellpadding="2" cellspacing="0" border="0">
                        <tr>
                            <td style="width:20px;"></td>
                            <td style="height:200px; vertical-align:top; "><br />
                                <uc7:GridviewPageController ID="pcGridCampaign" runat="server" OnPageChange="PageSearchChangeCampaign" Width="910px" />
                                <asp:GridView ID="gvAllCampaign" runat="server" AutoGenerateColumns="False" Width="910px"
                                    GridLines="Horizontal" BorderWidth="1px" EnableModelValidation="True" EmptyDataText="<span style='color:Red;'>ไม่พบข้อมูล</span>" 
                                    onrowdatabound="gvAllCampaign_RowDataBound"  >
                                    <Columns>
                                        <asp:TemplateField HeaderText="เลือก">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbSelectCampaign" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="40px" HorizontalAlign="Center"/>
                                            <ItemStyle Width="40px" HorizontalAlign="Center" VerticalAlign="Top"  />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ProductGroupName" HeaderText="กลุ่มผลิตภัณฑ์/บริการ"  >
                                            <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                                            <ItemStyle Width="150px" HorizontalAlign="Left"  VerticalAlign="Top"/>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="ผลิตภัณฑ์/บริการ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("ProductName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="140px" HorizontalAlign="Center"/>
                                            <ItemStyle Width="140px" HorizontalAlign="Left" VerticalAlign="Top"  />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="แคมเปญ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCampaignName" runat="server" Text='<%# Eval("CampaignName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                                            <ItemStyle Width="150px" HorizontalAlign="Left" VerticalAlign="Top"  />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="รายละเอียดแคมเปญ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCampaignDesc" runat="server" Text='<%# Eval("CampaignDesc") %>'></asp:Label>
                                                <asp:LinkButton ID="lbShowCampaignDesc" runat="server" Text="อ่านต่อ" CommandArgument='<%# Eval("CampaignId") %>' Visible="false" ></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="180px" HorizontalAlign="Center"/>
                                            <ItemStyle Width="180px" HorizontalAlign="Left" VerticalAlign="Top"  />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="วันที่เริ่มต้น">
                                            <ItemTemplate>
                                                <%# Eval("StartDate") != null ? Convert.ToDateTime(Eval("StartDate")).ToString("dd/MM/") + Convert.ToDateTime(Eval("StartDate")).Year.ToString() : ""%>
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" HorizontalAlign="Center" VerticalAlign="Top"/>
                                            <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="วันที่สิ้นสุด">
                                            <ItemTemplate>
                                                <%# Eval("EndDate") != null ? Convert.ToDateTime(Eval("EndDate")).ToString("dd/MM/") + Convert.ToDateTime(Eval("EndDate")).Year.ToString() : ""%>
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" HorizontalAlign="Center" VerticalAlign="Top"/>
                                            <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="แนะนำ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCmt" runat="server" ForeColor="Red" Font-Bold="true" Text='<%# Eval("Recommend") %>' ></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="50px" HorizontalAlign="Center"/>
                                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CampaignId">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCampaignId" runat="server" Text='<%# Eval("CampaignId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ControlStyle CssClass="Hidden" />
                                            <HeaderStyle CssClass="Hidden" />
                                            <ItemStyle CssClass="Hidden" />
                                            <FooterStyle CssClass="Hidden" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ProductGroupId">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProductGroupId" runat="server" Text='<%# Eval("ProductGroupId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ControlStyle CssClass="Hidden" />
                                            <HeaderStyle CssClass="Hidden" />
                                            <ItemStyle CssClass="Hidden" />
                                            <FooterStyle CssClass="Hidden" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ProductId">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProductId" runat="server" Text='<%# Eval("ProductId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ControlStyle CssClass="Hidden" />
                                            <HeaderStyle CssClass="Hidden" />
                                            <ItemStyle CssClass="Hidden" />
                                            <FooterStyle CssClass="Hidden" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="t_rowhead" />
                                    <RowStyle CssClass="t_row" BorderStyle="Dashed"/>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:20px;"></td>
                            <td > 
                                <asp:Button id="btnSelectCampaign" runat="server" Text="เลือก" Width="100px" OnClientClick="DisplayProcessing()"
                                    onclick="btnSelectCampaign_Click"  />
                                <asp:Button id="btnClose" runat="server" Text="ปิดหน้าต่าง" Width="100px" 
                                    onclick="btnClose_Click" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
        </asp:Panel>
        <act:ModalPopupExtender ID="mpePopupSearchCampaign" runat="server" TargetControlID="btnPopupSearchCampaign" PopupControlID="pnPopupSearchCampaign" BackgroundCssClass="modalBackground" DropShadow="True">
	    </act:ModalPopupExtender>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdatePanel ID="upPopupSaveResult" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Button runat="server" ID="btnPopupSaveResult" Width="0px" CssClass="Hidden"/>
	    <asp:Panel runat="server" ID="pnPopupSaveResult" style="display:none" CssClass="modalPopupCreateLeadResultList" ScrollBars="Auto">
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <b style="font-size:14px;">บันทึกข้อมูลผู้มุ่งหวังสำเร็จ</b>
            <br /><br />
            <asp:Panel ID="pnInner" runat="server" CssClass="modalPopupCreateLeadResultListInner" ScrollBars="Auto" BorderStyle="None">
                <asp:Repeater ID="rptPopupSaveResult" runat="server" >
                    <HeaderTemplate>
                        <table cellpadding="2" cellspacing="0" border="0">
                    </HeaderTemplate>
                    <ItemTemplate>
                         <tr>
                            <td style="width:40px;"></td>
                            <td style="width:380px;">
                                <b>Ticket Id:</b>&nbsp;<asp:Label ID="lblResultTicketId" runat="server" Text='<%# Eval("TicketId") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:40px;"></td>
                            <td>
                                <b>แคมเปญ:</b>&nbsp;<asp:Label ID="lblResultCampaign" runat="server" Text='<%# Eval("CampaignName") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:40px;"></td>
                            <td>
                                <b>ช่องทาง:</b>&nbsp;<asp:Label ID="lblResultChannel" runat="server" Text='<%# Eval("ChannelDesc") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:40px;"></td>
                            <td>
                                <b>Owner Lead:</b>&nbsp;<asp:Label ID="lblResultOwnerLead" runat="server" Text='<%# Eval("OwnerName") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr><td colspan="2" style="height:15px;"></td></tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </asp:Panel>
            <br />
            <center><asp:Button ID="btnPopupSaveResultOK" runat="server" Text="OK" CssClass="Button"  Width="100px" OnClick="btnPopupSaveResultOK_Click" OnClientClick="DisplayProcessing();" /></center>
            <br style="height:10px;" />
        </asp:Panel>
        <act:ModalPopupExtender ID="mpePopupSaveResult" runat="server" TargetControlID="btnPopupSaveResult" PopupControlID="pnPopupSaveResult" BackgroundCssClass="modalBackground" DropShadow="True">
	    </act:ModalPopupExtender>
    </ContentTemplate>
</asp:UpdatePanel>
    
    <!-- ตั้งแต่บรรทัดนี้ ซ่อนไว้ ไม่ได้ใช้งาน -->
    <asp:UpdatePanel  ID="upCampaignPopup" runat="server" UpdateMode="Conditional">
        <ContentTemplate >
            <asp:Button runat="server" ID="btnPopup" Width="0px" CssClass="Hidden"/>
            <asp:Panel runat="server" ID="pnPopup" style="display:none" CssClass="modalPopupCampaignSCR004">
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;<asp:Image ID="imgSaveNote" runat="server" ImageUrl="~/Images/ProductAll.gif" />
                <table cellpadding="2" cellspacing="0" border="0">
                     <tr>
                        <td style="padding-left:12px;">
                            <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto" ><br />
                                <uc7:GridviewPageController ID="pcTop" runat="server" OnPageChange="PageSearchChange" Width="800px" />
                                <asp:GridView ID="gvSearchCampaign" runat="server" AutoGenerateColumns ="false" 
                                    GridLines="Horizontal" BorderWidth="1px" 
                                    onrowdatabound="gvSearchCampaign_RowDataBound"  >
                                        <Columns >
                                            <asp:TemplateField >
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                <HeaderStyle Width="50px" HorizontalAlign="Center"/>
                                            </asp:TemplateField>
                                           <asp:BoundField DataField="CampaignId" HeaderText="รหัส Product/Campaign" >
                                               <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="150px" HorizontalAlign="Left"  />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CampaignName" HeaderText="ชื่อ Product/Campaign"  >
                                                <HeaderStyle Width="250px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="250px" HorizontalAlign="Left"  />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="รายละเอียด Product/Campaign" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCampaignDetail" runat="server" ToolTip='<%# Bind("CampaignDetail") %>' Text='<%# Bind("CampaignDetail") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="350px" HorizontalAlign="Left" />
                                                <HeaderStyle Width="350px" HorizontalAlign="Center"/>
                                            </asp:TemplateField>

                                        </Columns>
                                    <HeaderStyle CssClass="t_rowhead" />
                                    <RowStyle CssClass="t_row" BorderStyle="Dashed"/>
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left:12px; height:20px">
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left:12px;">
                            <asp:Button ID="btnOK" runat="server" Text="เลือก" CssClass="Button" Width="90px" OnClick="btnOK_Click" OnClientClick="if(confirm('ต้องการบักทึกแนะนำ Product/Campaign ใช่หรือไม่?')){DisplayProcessing(); return true;} else{return false;}" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancelCampaign" runat="server" Text="ยกเลิก" CssClass="Button" Width="90px"  />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <act:ModalPopupExtender ID="mpePopup" runat="server" TargetControlID="btnPopup" PopupControlID="pnPopup" BackgroundCssClass="modalBackground" DropShadow="True">
            </act:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel  ID="upOfferCampaign" runat="server" UpdateMode="Conditional">
        <ContentTemplate >
            <asp:Button runat="server" ID="btnofferCampaigntest" Width="0px" CssClass="Hidden"/>
            <asp:Panel runat="server" ID="pnlOfferCampaign" style="display:none" CssClass="modalPopupOfferCampaignSCR004">
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;<asp:Image ID="Image1" runat="server" ImageUrl="~/Images/ProductOffer.gif" />
                <table cellpadding="2" cellspacing="0" border="0">
                     <tr>
                        <td style="padding-left:12px;">
                            <asp:Panel ID="Panel3" runat="server" ScrollBars="Auto" ><br />
                                <asp:GridView ID="gvOfferCampaign" runat="server" AutoGenerateColumns="False" 
                                    GridLines="Horizontal" BorderWidth="0px" EnableModelValidation="True" 
                                    onrowdatabound="gvOfferCampaign_RowDataBound"  >
                                    <Columns>
                                        <asp:TemplateField HeaderText="สนใจ">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="50px" HorizontalAlign="Center" />
                                            <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CampaignCode" HeaderText="รหัส Product/Campaign"  >
                                            <HeaderStyle Width="180px" HorizontalAlign="Center"/>
                                            <ItemStyle Width="180px" HorizontalAlign="Center"  />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CampaignName" HeaderText="ชื่อ Product/Campaign"  >
                                            <HeaderStyle Width="200px" HorizontalAlign="Center"/>
                                            <ItemStyle Width="200px" HorizontalAlign="Left"  />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="รายละเอียด Product/Campaign">
                                            <ItemTemplate>
                                                <asp:Label ID="lbDetail" runat="server" ToolTip='<%#Bind("CampaignDetail") %>'  Text='<%#Bind("CampaignDetail")  %>' Width="240px" ></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="250px" HorizontalAlign="Center"/>
                                            <ItemStyle Width="250px" HorizontalAlign="Left"  />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="วันที่สิ้นสุด">
                                            <ItemTemplate>
                                                <%# Eval("ExpireDate") != null ? Convert.ToDateTime(Eval("ExpireDate")).ToString("dd/MM/") + Convert.ToDateTime(Eval("ExpireDate")).Year.ToString() : ""%>
                                            </ItemTemplate>
                                            <HeaderStyle Width="180px" HorizontalAlign="Center"/>
                                            <ItemStyle Width="180px" HorizontalAlign="Center"  />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ChannelName" HeaderText="ช่องทาง" >
                                            <HeaderStyle Width="150px" HorizontalAlign="Center" />
                                            <ItemStyle Width="150px" HorizontalAlign="Left"  />
                                        </asp:BoundField>
                                    </Columns>
                                    <HeaderStyle CssClass="t_rowhead" />
                                    <RowStyle CssClass="t_row" BorderStyle="Dashed"/>
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left:12px; height:20px">
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left:12px;">
                            <asp:Button ID="btnSelectOfferCampaign" runat="server" Text="เลือก" CssClass="Button" Width="90px" OnClick="btnSelectOfferCampaign_Click" OnClientClick="if(confirm('ต้องการบักทึกแนะนำ Product/Campaign ใช่หรือไม่?')){DisplayProcessing(); return true;} else{return false;}" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancelOfferCampaign" runat="server" Text="ยกเลิก" CssClass="Button" Width="90px"  />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <act:ModalPopupExtender ID="mpeOfferCampaignPopup" runat="server" TargetControlID="btnofferCampaigntest" PopupControlID="pnlOfferCampaign" BackgroundCssClass="modalBackground" DropShadow="True">
            </act:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
