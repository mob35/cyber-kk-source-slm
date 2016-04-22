<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SaleLead.Master" AutoEventWireup="true" CodeBehind="SLM_SCR_010.aspx.cs" Inherits="SLM.Application.SLM_SCR_010" %>
<%@ Register src="Shared/LeadInfo.ascx" tagname="LeadInfo" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:LeadInfo ID="LeadInfo1" runat="server" />
</asp:Content>
