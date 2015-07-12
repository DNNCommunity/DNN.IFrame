<%@ Control Language="vb" CodeFile="EditIFrame.ascx.vb" AutoEventWireup="false" Inherits="DotNetNuke.Modules.IFrame.EditIFrame" %>
<%@ Register TagPrefix="dnn" TagName="Url" Src="~/controls/UrlControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Import Namespace="DotNetNuke.Modules.IFrame.Domain" %>
<div style="text-align: left">
    <dnn:SectionHead ID="dshOptions" IncludeRule="true" ResourceKey="dshOptions" Section="pnlOptions"
        runat="server" CssClass="Head"></dnn:SectionHead>
</div>
<div id="pnlOptions" runat="server">
    <table cellspacing="0" cellpadding="0" width="600" summary="Table to update IFrame settings"
        border="0">
        <tr>
            <td style="width: 125">
            </td>
            <td style="width: 475">
            </td>
        </tr>
        <tr>
            <td valign="top" class="SubHead">
                <dnn:Label ID="plSource" runat="server" Suffix=":" ControlName="urlSource"></dnn:Label>
            </td>
            <td>
                <p>
                    <dnn:Url ID="urlSource" runat="server" ShowUrls="true" ShowTrack="false" ShowTabs="true"
                        ShowLog="false" ShowFiles="true" Required="true" UrlType="U" Width="300"></dnn:Url>
                </p>
            </td>
        </tr>
        <tr>
            <td valign="top" class="SubHead">
                <dnn:Label ID="plWidth" runat="server" Suffix=":" ControlName="txtWidth"></dnn:Label>
            </td>
            <td>
                <asp:TextBox ID="txtWidth" runat="server" CssClass="NormalTextBox" Columns="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td valign="top" class="SubHead">
                <dnn:Label ID="plHeight" runat="server" Suffix=":" ControlName="txtHeight"></dnn:Label>
            </td>
            <td>
                <asp:TextBox ID="txtHeight" runat="server" CssClass="NormalTextBox" Columns="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td valign="top" class="SubHead">
                <dnn:Label ID="plAutoHeight" runat="server" ControlName="cbAutoHeight"></dnn:Label>
            </td>
            <td>
                <asp:CheckBox ID="cbAutoheight" runat="server" />
            </td>
        </tr>
        <tr>
            <td valign="top" class="SubHead">
                <dnn:Label ID="plScrolling" runat="server" Suffix=":" ControlName="cboScrolling">
                </dnn:Label>
            </td>
            <td>
                <asp:DropDownList ID="cboScrolling" runat="server" CssClass="NormalTextBox">
                    <asp:ListItem resourcekey="Auto" Value="auto">Auto</asp:ListItem>
                    <asp:ListItem resourcekey="No" Value="no">No</asp:ListItem>
                    <asp:ListItem resourcekey="Yes" Value="yes">Yes</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td valign="top" class="SubHead">
                <dnn:Label ID="plBorder" runat="server" Suffix=":" ControlName="cboBorder"></dnn:Label>
            </td>
            <td>
                <asp:DropDownList ID="cboBorder" runat="server" CssClass="NormalTextBox">
                    <asp:ListItem resourcekey="No" Value="0">No</asp:ListItem>
                    <asp:ListItem resourcekey="Yes" Value="1">Yes</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <br />
                <div style="text-align: left">
                    <dnn:SectionHead ID="dshLegacy" IncludeRule="false" ResourceKey="dshLegacy" Section="tblLegacy"
                        runat="server" CssClass="Head" IsExpanded="false"></dnn:SectionHead>
                </div>
                <table cellspacing="0" cellpadding="0" width="600" summary="Table to update IFrame settings"
                    border="0" runat="server" id="tblLegacy">
                    <tr>
                        <td valign="top" class="SubHead">
                            <dnn:Label ID="plAllowTransparency" runat="server" Suffix=":" ControlName="cbAllowTransparency">
                            </dnn:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbAllowTransparency" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" class="SubHead">
                            <dnn:Label ID="plName" runat="server" Suffix=":" ControlName="txtName"></dnn:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server" CssClass="NormalTextBox" Columns="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" class="SubHead">
                            <dnn:Label ID="plToolTip" runat="server" Suffix=":" ControlName="txtToolTip"></dnn:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtToolTip" runat="server" CssClass="NormalTextBox" Columns="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" class="SubHead">
                            <dnn:Label ID="plCssStyle" runat="server" Suffix=":" ControlName="txtCssStyle"></dnn:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCssStyle" runat="server" CssClass="NormalTextBox" Columns="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" class="SubHead">
                            <dnn:Label ID="plOnLoad" runat="server" Suffix=":" ControlName="txtOnLoad"></dnn:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOnload" runat="server" CssClass="NormalTextBox" Columns="50"
                                Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
<br />
<div style="text-align: left">
    <dnn:SectionHead ID="dshParams" IncludeRule="true" ResourceKey="dshParams" Section="pnlParams"
        runat="server" CssClass="Head" IsExpanded="true"></dnn:SectionHead>
</div>
<div id="pnlParams" runat="server">
    <asp:DataGrid ID="grdParams" runat="server" CssClass="Normal" summary="List of iframe URL parameters"
        DataKeyField="ID" AutoGenerateColumns="False" CellPadding="2" GridLines="None"
        BorderWidth="0px">
        <Columns>
            <asp:TemplateColumn>
                <ItemStyle Wrap="False" VerticalAlign="Top"></ItemStyle>
                <ItemTemplate>
                    <asp:ImageButton runat="server" CausesValidation="false" CommandName="Edit" ImageUrl="~/images/edit.gif"
                        AlternateText="Edit" resourcekey="Edit" ID="Imagebutton1" />
                    <asp:ImageButton ID="cmdDeleteParam" runat="server" CausesValidation="false" CommandName="Delete"
                        ImageUrl="~/images/delete.gif" AlternateText="Delete" resourcekey="Delete" />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:ImageButton runat="server" CausesValidation="false" CommandName="Update" ImageUrl="~/images/save.gif"
                        AlternateText="Save" resourcekey="Save" ID="Imagebutton2" />
                    <asp:ImageButton runat="server" CausesValidation="false" CommandName="Cancel" ImageUrl="~/images/cancel.gif"
                        AlternateText="Cancel" resourcekey="Cancel" ID="Imagebutton3" />
                </EditItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Name">
                <HeaderStyle CssClass="NormalBold"></HeaderStyle>
                <ItemStyle Wrap="False" CssClass="Normal" VerticalAlign="Top"></ItemStyle>
                <ItemTemplate>
                    <%# CType(Container.DataItem, IFrameParameter).Name %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Label ID="lblParamName" runat="server" />
                    <asp:TextBox ID="txtParamName" runat="server" Columns="30" MaxLength="50" Text='<%# CType(Container.DataItem, IFrameParameter).Name %>' />
                </EditItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Value">
                <HeaderStyle CssClass="NormalBold"></HeaderStyle>
                <ItemStyle Wrap="False" CssClass="Normal" VerticalAlign="Top"></ItemStyle>
                <ItemTemplate>
                    <asp:Label runat="server" resourcekey='<%# CType(Container.DataItem, IFrameParameter).ConvertTypeToString() %>'
                        ID="Label1">
						<%# CType(Container.DataItem, IFrameParameter).ConvertTypeToString() %>
                    </asp:Label>
                    <%# IIf(CType(Container.DataItem, IFrameParameter).IsArgumentRequired(), "(" + Convert.ToString(CType(Container.DataItem, IFrameParameter).TypeArgument) + ")", "") %>                    
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Label ID="lblParamType" runat="server" />
                    <asp:DropDownList ID="cboParamType" runat="server" SelectedIndex='<%# CType(Container.DataItem, IFrameParameter).Type %>'>
                        <asp:ListItem Value="StaticValue" resourcekey="StaticValue">Static Value</asp:ListItem>
                        <asp:ListItem Value="PassThrough" resourcekey="PassThrough">Pass-Through Parameter</asp:ListItem>
                        <asp:ListItem Value="FormPassThrough" resourcekey="FormPassThrough">Form Pass-Through Parameter</asp:ListItem>
                        <asp:ListItem Value="UserCustomProperty" resourcekey="UserCustomProperty">Custom User Property</asp:ListItem>
                        <asp:ListItem Value="PortalID" resourcekey="PortalID">Portal ID</asp:ListItem>
                        <asp:ListItem Value="PortalName" resourcekey="PortalName">Portal Name</asp:ListItem>
                        <asp:ListItem Value="Locale" resourcekey="Locale">Current Locale</asp:ListItem>
                        <asp:ListItem Value="TabID" resourcekey="TabID">Tab ID</asp:ListItem>
                        <asp:ListItem Value="ModuleID" resourcekey="ModuleID">Module ID</asp:ListItem>
                        <asp:ListItem Value="UserID" resourcekey="UserID">User ID</asp:ListItem>
                        <asp:ListItem Value="UserUsername" resourcekey="UserUsername">User's Username</asp:ListItem>
                        <asp:ListItem Value="UserFirstName" resourcekey="UserFirstName">User's First Name</asp:ListItem>
                        <asp:ListItem Value="UserLastName" resourcekey="UserLastName">User's Last Name</asp:ListItem>
                        <asp:ListItem Value="UserDisplayname" resourcekey="UserDisplayName">User's Display Name</asp:ListItem>
                        <asp:ListItem Value="UserEmail" resourcekey="UserEmail">User's Email</asp:ListItem>
                        <asp:ListItem Value="UserWebsite" resourcekey="UserWebsite">User's Website</asp:ListItem>
                        <asp:ListItem Value="UserIM" resourcekey="UserIM">User's IM</asp:ListItem>
                        <asp:ListItem Value="UserStreet" resourcekey="UserStreet">User's Street</asp:ListItem>
                        <asp:ListItem Value="UserUnit" resourcekey="UserUnit">User's Unit</asp:ListItem>
                        <asp:ListItem Value="UserCity" resourcekey="UserCity">User's City</asp:ListItem>
                        <asp:ListItem Value="UserCountry" resourcekey="UserCountry">User's Country</asp:ListItem>
                        <asp:ListItem Value="UserRegion" resourcekey="UserRegion">User's Region</asp:ListItem>
                        <asp:ListItem Value="UserPostalCode" resourcekey="UserPostalCode">User's Postal Code</asp:ListItem>
                        <asp:ListItem Value="UserPhone" resourcekey="UserPhone">User's Phone</asp:ListItem>
                        <asp:ListItem Value="UserCell" resourcekey="UserCell">User's Cell</asp:ListItem>
                        <asp:ListItem Value="UserFax" resourcekey="UserFax">User's Fax</asp:ListItem>
                        <asp:ListItem Value="UserLocale" resourcekey="UserLocale">User's Locale</asp:ListItem>
                        <asp:ListItem Value="UserTimeZone" resourcekey="UserTimeZone">User's TimeZone</asp:ListItem>
                        <asp:ListItem Value="UserIsAuthorized" resourcekey="UserIsAuthorized">User's Authorized Flag</asp:ListItem>
                        <asp:ListItem Value="UserIsLockedOut" resourcekey="UserIsLockedOut">User's Lock Out Flag</asp:ListItem>
                        <asp:ListItem Value="UserIsSuperUser" resourcekey="UserIsSuperUser">User's SuperUser Flag</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="lblParamArgument" runat="server" />
                    <asp:TextBox ID="txtParamArgument" runat="server" MaxLength="2000" Text='<%# CType(Container.DataItem, IFrameParameter).TypeArgument %>' />
                    <asp:Label ID="lblParamScript" runat="server" />
                </EditItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderTemplate><dnn:Label ID="plUseAsHash" runat="server" Suffix=""></dnn:Label></HeaderTemplate>
                <HeaderStyle CssClass="NormalBold"></HeaderStyle>
                <ItemStyle Wrap="False" CssClass="Normal" VerticalAlign="Top"></ItemStyle>
                <ItemTemplate>
                    <input type="radio" name="hash" disabled="disabled" <%# IIf(IsHash(CType(Container.DataItem, IFrameParameter).ID), "checked='checked'", String.Empty) %> />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Label ID="lblParamIsHash" runat="server" />
                    <asp:RadioButton runat="server" ID="radParamIsHash" name="radParamIsHash" Checked="<%# IsHash(CType(Container.DataItem, IFrameParameter).ID) %>" />
                </EditItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    <p>
        <asp:LinkButton ID="cmdAddParam" resourcekey="cmdAddParam" runat="server" Text="Add New Column"
            CausesValidation="False" CssClass="CommandButton"></asp:LinkButton>&nbsp;
    </p>
</div>
<p>
    <asp:LinkButton class="CommandButton" ID="cmdSave" resourcekey="Save" runat="server"
        Text="Save" BorderStyle="none"></asp:LinkButton>&nbsp;
    <asp:LinkButton class="CommandButton" ID="cmdCancel" resourcekey="Cancel" runat="server"
        Text="Cancel" CausesValidation="False" BorderStyle="none"></asp:LinkButton></p>
