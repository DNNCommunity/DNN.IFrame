<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IFrame.ascx.cs" Inherits="DotNetNuke.Modules.IFrame.UI.IFrame" %>

<iframe id="HtmlIFrame" runat="server" enableviewstate="false" />

<div id="MessageContainer" class="dnnFormMessage dnnFormInfo" runat="server" visible="false" enableviewstate="false">
   <asp:Label ID="MessageLabel" runat="server" />
</div>
