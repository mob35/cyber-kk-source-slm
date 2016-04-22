<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SaleLead.Master" AutoEventWireup="true" CodeBehind="SLM_SCR_018.aspx.cs" Inherits="SLM.Application.SLM_SCR_018" %>
<%@ Register src="Shared/Tabs/Tab018_1.ascx" tagname="Tab018_1" tagprefix="uc1" %>
<%@ Register src="Shared/Tabs/Tab018_2.ascx" tagname="Tab018_2" tagprefix="uc2" %>
<%@ Register src="Shared/GridviewPageController.ascx" tagname="GridviewPageController" tagprefix="uc7" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
        .ColInfo
        {
            font-weight:bold;
            width:160px;
        }
        .ColInput
        {
            width:250px;
        }
        .ColCheckBox
        {
            width:160px;
        }
        .style1
        {
            width: 50px;
        }
        .style2
        {
            width: 200px;
            text-align:left;
            font-weight:bold;
        }
        .style3
        {
            width: 380px;
            text-align: left;
        }
        .style4
        {
            font-family: Tahoma;
            font-size: 9pt;
            color: Red;
        }
        .style5
        {
            width: 500px;
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:UpdatePanel ID="upInfo" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="2" cellspacing="0" border="0" >
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">Windows Username <span class="style4">*</span></td>
                    <td class="style3">
                        <asp:Label ID="lblUsername" runat="server" Font-Size="13px" BorderStyle="Solid" BorderWidth="1px" BorderColor="#7f9db9" Width="152px" BackColor="#e5edf5"></asp:Label>
                        <asp:TextBox ID="txtUsername" runat="server" Visible="false" CssClass="Textbox" ReadOnly="true" Width="150px" MaxLength="100" ></asp:TextBox>
                        <asp:TextBox ID="txtStaffId" runat="server" Width="10px" Visible="false"></asp:TextBox>
                        <asp:Label ID="vtxtUsername" runat="server" CssClass="style4"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">รหัสพนักงานธนาคาร <span class="style4">*</span></td>
                    <td class="style3">
                        <asp:TextBox ID="txtEmpCode" runat="server" CssClass="Textbox" Width="150px" MaxLength="6" ></asp:TextBox>
                        <asp:Label ID="vtxtEmpCode" runat="server" CssClass="style4"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">รหัสเจ้าหน้าที่การตลาด</td>
                    <td class="style3">
                        <asp:TextBox ID="txtMarketingCode" runat="server" CssClass="Textbox" Width="150px" MaxLength="6" ></asp:TextBox>
                        <asp:Label ID="vtxtMarketingCode" runat="server" CssClass="style4"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">ชื่อ-นามสกุลพนักงาน <span class="style4">*</span></td>
                    <td class="style5">
                        <asp:TextBox ID="txtStaffNameTH" runat="server" CssClass="Textbox" Width="260px" MaxLength="100" ></asp:TextBox>
                        <asp:Label ID="vtxtStaffNameTH" runat="server" CssClass="style4"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">เบอร์โทรศัพท์</td>
                    <td class="style3">
                        <asp:TextBox ID="txtTellNo" runat="server" CssClass="Textbox" Width="100px" MaxLength="10" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">E-mail <span class="style4">*</span></td>
                    <td class="style3">
                        <asp:TextBox ID="txtStaffEmail" runat="server" CssClass="Textbox" Width="260px" 
                        MaxLength="100" AutoPostBack="True" ontextchanged="txtEmail_TextChanged" ></asp:TextBox>
                        <asp:Label ID="vtxtStaffEmail" runat="server" CssClass="style4"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">ตำแหน่ง <span class="style4">*</span></td>
                    <td class="style3">
                        <asp:DropDownList ID="cmbPosition" runat="server" CssClass="Dropdownlist" Width="263px"></asp:DropDownList>
                        <asp:Label ID="vtxtPositionName" runat="server" CssClass="style4"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">Role <span class="style4">*</span></td>
                    <td class="style3">
                        <asp:DropDownList ID="cmbStaffType" runat="server" CssClass="Dropdownlist" Width="263px"  ></asp:DropDownList>
                        <asp:Label ID="vcmbStaffType" runat="server" CssClass="style4"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">ทีมการตลาด</td>
                    <td class="style3">
                        <asp:TextBox ID="txtTeam" runat="server" CssClass="Textbox" Width="260px" MaxLength="100"  ></asp:TextBox>
                        <asp:Label ID="vtxtTeam" runat="server" CssClass="style4"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">สาขาพนักงาน <span class="style4">*</span></td>
                    <td class="style3">
                        <asp:DropDownList ID="cmbBranchCode" runat="server" CssClass="Dropdownlist"  
                            Width="263px" ></asp:DropDownList>
                        <asp:TextBox ID="txtOldBranchCode" runat="server" Visible="false" ></asp:TextBox>
                        <asp:Label ID="vcmbBranchCode" runat="server" CssClass="style4"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">สาขาหัวหน้างาน</td>
                    <td class="style3">
                        <asp:DropDownList ID="cmbHeadBranchCode" runat="server" CssClass="Dropdownlist" 
                            Width="263px" AutoPostBack="True" 
                            onselectedindexchanged="cmbHeadBranchCode_SelectedIndexChanged"></asp:DropDownList>
                        <asp:Label ID="vcmbHeadBranchCode" runat="server" CssClass="style4"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">หัวหน้างาน <asp:Label ID="lblHeadStaffId" runat="server" CssClass="style4"></asp:Label></td>
                    <td class="style3">
                        <asp:DropDownList ID="cmbHeadStaffId" runat="server" CssClass="Dropdownlist" Width="263px" ></asp:DropDownList>
                        <asp:Label ID="vcmbHeadStaffId" runat="server" CssClass="style4"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">สถานะ </td>
                    <td class="style3">
                        <asp:RadioButton ID="rdNormal" runat="server" GroupName="EmpStatus" Text="ปกติ" 
                            AutoPostBack="True" oncheckedchanged="rdNormal_CheckedChanged" />
                        <asp:RadioButton ID="rdRetire" runat="server" GroupName="EmpStatus" 
                            Text="ลาออก" AutoPostBack="True" oncheckedchanged="rdRetire_CheckedChanged" />&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="vEmpStatus" runat="server" CssClass="style4"></asp:Label>
                        <asp:TextBox ID="txtOldIsDeleted" runat="server" Visible="false" ></asp:TextBox>
                        <asp:TextBox ID="txtNewIsDeleted" runat="server" Visible="false" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">สาย</td>
                    <td class="style3">
                       <asp:DropDownList ID="cmbDepartment" runat="server" Width="263px" CssClass="Dropdownlist" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="height:15px;">
                    </td>
                </tr>
                <tr style="height:35px;">
                    <td style="width:10px"></td>
                    <td class="style2"></td>
                    <td  class="style3" >
                        <asp:Button ID="btnSave" runat="server" Text="บันทึก" Width="100px" CssClass="Button"    
                            OnClientClick="DisplayProcessing()" onclick="btnSave_Click" />&nbsp;
                        <asp:Button ID="btnClose" runat="server" Text="กลับหน้าหลัก" Width="100px" CssClass="Button" 
                            onclick="btnClose_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <div class="Line"></div>
    <br />
    <%--<asp:UpdatePanel ID="upCampaign" runat="server" UpdateMode="Conditional" Visible="false">
        <ContentTemplate>
            <table cellspacing ="0" border="0" cellpadding ="0">
                <tr>
                    <td style="width:10px"></td>
                    <td ><asp:Image ID="Image1" runat="server" ImageUrl="~/Images/CampaignData.gif" /></td>
                    <td class="style3">&nbsp;&nbsp;
                        <asp:Button ID="btnAdd" runat="server" CssClass="Botton" Text="เพิ่มแคมเปญ" onclick="btnAdd_Click" />
                        <asp:TextBox ID="txtCampaignIdList" runat="server" CssClass="Hidden" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="height:3px;"></td>
                </tr>
            </table>
            <asp:Panel ID="pnlCampaign" runat="server" ScrollBars ="Auto" Height="270px" Width="900px" BorderStyle="Solid" BorderWidth="1px">
                <asp:GridView ID="gvCampaign" runat="server" AutoGenerateColumns="False" 
                    GridLines="Horizontal" BorderWidth="0px" 
                        EnableModelValidation="True" 
                    EmptyDataText="<center><span style='color:Red;'>ไม่พบข้อมูล</span></center>" 
                    ondatabound="gvCampaign_DataBound" >
                    <Columns >
                        <asp:BoundField DataField="StaffGroupId" HeaderText="StaffGroupId">
                            <ControlStyle CssClass="Hidden" />
                            <HeaderStyle CssClass="Hidden" />
                            <ItemStyle CssClass="Hidden" />
                        </asp:BoundField>
                        <asp:BoundField DataField="StaffId" HeaderText="StaffId">
                            <ControlStyle CssClass="Hidden" />
                            <HeaderStyle CssClass="Hidden" />
                            <ItemStyle CssClass="Hidden" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imbDelete" runat="server" ImageAlign="AbsMiddle" Height="20px" Width="20px"
                                        ImageUrl="~/Images/eraser.jpg" ToolTip="Delete" CommandArgument='<%#Bind("StaffGroupId") %>' 
                                         OnClick="imbDelete_Click" OnClientClick="return confirm('ต้องการลบใช่หรือไม่')"/>
                            </ItemTemplate>
                            <ItemStyle Width="30px" HorizontalAlign="Center" VerticalAlign="Top"/>
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="No.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Top"/>
                            <HeaderStyle Width="50px" HorizontalAlign="Center"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="รหัสแคมเปญ">
                            <ItemTemplate>
                                <asp:Label ID="lblCampaignId" runat="server" Text='<%#Bind("CampaignId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="120px" HorizontalAlign="Center" />
                            <HeaderStyle Width="120px" HorizontalAlign="Center"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="แคมเปญ">
                            <ItemTemplate>
                                <asp:Label ID="lblCampaignName" runat="server" Text='<%#Bind("CampaignName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="600px" HorizontalAlign="Left" VerticalAlign="Top"/>
                            <HeaderStyle Width="600px" HorizontalAlign="Center"/>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="t_rowhead" />
                    <RowStyle CssClass="t_row" BorderStyle="Dashed"/>
                </asp:GridView>  
            </asp:Panel>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel  ID="upCampaignPopup" runat="server" UpdateMode="Conditional"  Visible="false" >
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
                                <asp:GridView ID="gvSearchCampaign" runat="server" AutoGenerateColumns ="false" GridLines="Horizontal" BorderWidth="1px"  >
                                        <Columns >
                                            <asp:TemplateField >
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                <HeaderStyle Width="50px" HorizontalAlign="Center"/>
                                            </asp:TemplateField>
                                           <asp:BoundField DataField="CampaignId" HeaderText="รหัสแคมเปญ" >
                                               <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="150px" HorizontalAlign="Center"  />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CampaignName" HeaderText="ชื่อแคมเปญ"  >
                                                <HeaderStyle Width="600px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="600px" HorizontalAlign="Left"  />
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
                            <asp:Button ID="btnOK" runat="server" Text="เลือก" CssClass="Button" Width="90px" OnClick="btnOK_Click" OnClientClick="if(confirm('ต้องการบักทึกแคมเปญใช่หรือไม่?')){DisplayProcessing(); return true;} else{return false;}" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancelCampaign" runat="server" Text="ยกเลิก" CssClass="Button" Width="90px"  />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <act:ModalPopupExtender ID="mpePopup" runat="server" TargetControlID="btnPopup" PopupControlID="pnPopup" BackgroundCssClass="modalBackground" DropShadow="True">
            </act:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>--%>
    <asp:UpdatePanel ID="upTabMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="2" cellspacing="0" border="0" >
                <tr>
                    <td style="height:10px"></td>
                </tr>
                <tr>
                    <td><asp:Image ID="Image3" runat="server" ImageUrl="~/Images/OnHand.gif" /></td>
                </tr>
                <tr>
                    <td style="height:1px"></td>
                </tr>
            </table> 
            <act:TabContainer ID="tabMain" runat="server" ActiveTabIndex="0" Width="1230px">
                <act:TabPanel ID="tab018_1" runat="server"  >
                    <HeaderTemplate>
                        <asp:Label ID="lblHeader018_1" runat="server" Text="&nbsp;Owner&nbsp;" CssClass="tabHeaderText"></asp:Label>                 
                    </HeaderTemplate>
                    <ContentTemplate>
                        <uc1:Tab018_1 ID="tabOwner" runat="server"  />
                    </ContentTemplate>
                </act:TabPanel>
                <act:TabPanel ID="tab018_2" runat="server"  >
                    <HeaderTemplate>
                        <asp:Label ID="lblHeader018_2" runat="server" Text="&nbsp;Delegate&nbsp;" CssClass="tabHeaderText"></asp:Label>                 
                    </HeaderTemplate>
                    <ContentTemplate>
                        <uc2:Tab018_2 ID="tabDelegate" runat="server" />
                    </ContentTemplate>         
                </act:TabPanel>
            </act:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>    
</asp:Content>
