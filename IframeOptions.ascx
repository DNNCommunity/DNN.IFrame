<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IframeOptions.ascx.cs" Inherits="DotNetNuke.Modules.IFrame.UI.IframeOptions" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Url" Src="~/controls/UrlControl.ascx" %>

<div class="dnnForm" id="IFrameOptionTabs" runat="server">
   <div class="dnnFormItem dnnFormHelp dnnClear">
      <asp:Label ID="RequiredLabel" runat="server" CssClass="dnnFormRequired" ResourceKey="Required" />
   </div>
   <ul class="dnnAdminTabNav">
      <li><a href="#Options"><asp:Label ID="OptionsLabel" runat="server" ResourceKey="Options" /></a></li>
      <li><a href="#MoreOptions"><asp:Label ID="MoreOptionsLabel" runat="server" ResourceKey="MoreOptions" /></a></li>
      <li><a href="#Parameters"><asp:Label ID="ParametersLabel" runat="server" ResourceKey="Parameters" /></a></li>
   </ul>

   <div id="Options" class="dnnClear">
      <fieldset>
         <div class="dnnFormItem">
            <dnn:Label ID="SourceLabel" runat="server"
               ControlName="SourceUrl"
               CssClass="dnnFormRequired"
               ResourceKey="Source"
               Suffix=":" />
            <dnn:Url ID="SourceUrl" runat="server"
               ShowUrls="true"
               ShowTrack="false"
               ShowTabs="true"
               ShowLog="false"
               ShowFiles="true"
               Required="true"
               UrlType="U"
               Width="300" />
            <asp:RegularExpressionValidator ID="SourceUrlRegExpValidator" runat="server"
               ControlToValidate="SourceUrl$txtUrl"
               CssClass="dnnFormMessage dnnFormError"
               ForeColor="White"
               ResourceKey="SourceUrlValidator.ErrorMessage"
               ValidationExpression="http(s)?:\/\/([\w-]+\.)+[\w-]+(\/[\w- .\/?%&amp;=,!+]*)?"
               ValidationGroup="IFrameParametersValidation" />
            <asp:RequiredFieldValidator ID="SourceUrlRequiredValidator" runat="server"
               ControlToValidate="SourceUrl$txtUrl"
               CssClass="dnnFormMessage dnnFormError"
               ForeColor="White"
               ResourceKey="SourceUrlValidator.ErrorMessage"
               ValidationGroup="IFrameParametersValidation" />
         </div>

         <div class="dnnFormItem">
            <dnn:Label ID="WidthLabel" runat="server"
               ControlName="WidthTextBox"
               ResourceKey="Width"
               Suffix=":" />
            <asp:TextBox ID="WidthTextBox" runat="server"
               CssClass="NumberBox"
               Width="100" />
         </div>

         <div class="dnnFormItem">
            <dnn:Label ID="HeightLabel" runat="server"
               ControlName="HeightTextBox"
               ResourceKey="Height"
               Suffix=":" />
            <asp:TextBox ID="HeightTextBox" runat="server"
               CssClass="NumberBox"
               Width="100" />
         </div>

         <div class="dnnFormItem">
            <dnn:Label ID="AutoHeightLabel" runat="server"
               ControlName="AutoHeightCheckBox"
               ResourceKey="AutoHeight"
               Suffix="?" />
            <asp:CheckBox ID="AutoHeightCheckBox" runat="server"
               AutoPostBack="true"
               OnCheckedChanged="AutoHeightCheckBox_CheckedChanged" />
         </div>

         <div class="dnnFormItem">
            <dnn:Label ID="ScrollingLabel" runat="server"
               ControlName="ScrollingDropDownList"
               ResourceKey="Scrolling"
               Suffix=":" />
            <asp:DropDownList ID="ScrollingDropDownList" runat="server"
               OnLoad="ScrollingDropDownList_Load"
               Width="100" />
         </div>

         <div class="dnnFormItem">
            <dnn:Label ID="BorderLabel" runat="server"
               ControlName="BorderCheckBox"
               ResourceKey="Border"
               Suffix="?" />
            <asp:CheckBox ID="BorderCheckBox" runat="server" />
         </div>
      </fieldset>
   </div>

   <div id="MoreOptions" class="dnnClear">
      <fieldset>
         <div class="dnnFormItem">
            <dnn:Label ID="AllowTransparencyLabel" runat="server"
               ControlName="AllowTransparencyCheckBox"
               ResourceKey="AllowTransparency"
               Suffix="?" />
            <asp:CheckBox ID="AllowTransparencyCheckBox" runat="server" />
         </div>

         <div class="dnnFormItem">
            <dnn:Label ID="NameLabel" runat="server"
               ControlName="NameTextBox"
               ResourceKey="Name"
               Suffix=":" />
            <asp:TextBox ID="NameTextBox" runat="server" />
         </div>

         <div class="dnnFormItem">
            <dnn:Label ID="ToolTipLabel" runat="server"
               ControlName="ToolTipTextBox"
               ResourceKey="ToolTip"
               Suffix=":" />
            <asp:TextBox ID="ToolTipTextBox" runat="server" />
         </div>

         <div class="dnnFormItem">
            <dnn:Label ID="CssStyleLabel" runat="server"
               ControlName="CssStyleTextBox"
               ResourceKey="CssStyle"
               Suffix=":" />
            <asp:TextBox ID="CssStyleTextBox" runat="server" />
         </div>

         <div class="dnnFormItem" id="JavaScriptPanel" runat="server">
            <dnn:Label ID="JavaScriptLabel" runat="server"
               ControlName="JavascriptTextBox"
               ResourceKey="JavaScript"
               Suffix=":" />
            <asp:TextBox ID="JavascriptTextBox" runat="server"
               TextMode="MultiLine"
               Rows="10" />
         </div>
      </fieldset>
   </div>

   <div id="Parameters" class="dnnClear">
      <div id="ParametersMessage" runat="server"
         class="dnnFormMessage dnnFormInfo">
         <asp:Label ID="ParametersMessageLabel" runat="server" ResourceKey="NoParamaters.Text" />
      </div>
      <asp:DataGrid ID="ParametersGrid" runat="server"
         AutoGenerateColumns="false"
         CssClass="dnnGrid"
         OnItemDataBound="ParametersGrid_ItemDataBound"
         ResourceKey="ParametersGrid"
         Width="98%">
         <HeaderStyle CssClass="dnnGridHeader" VerticalAlign="Top" />
         <ItemStyle CssClass="dnnGridItem" HorizontalAlign="Left" />
         <AlternatingItemStyle CssClass="dnnGridAltItem" />
         <SelectedItemStyle CssClass="IFrameParameterSelected" />
         <Columns>
            <asp:TemplateColumn>
               <ItemStyle Wrap="false" HorizontalAlign="Center" Width="1%" />
               <ItemTemplate>
                  <asp:ImageButton ID="EditButton" runat="server"
                     CausesValidation="false"
                     CommandArgument='<%# Eval("ParameterID") %>'
                     CommandName="Select"
                     ImageUrl="~/icons/sigma/Edit_16X16_Standard.png"
                     OnClick="EditButton_Click"
                     ResourceKey="Edit" />
                  <asp:ImageButton ID="DeleteButton" runat="server"
                     CausesValidation="false"
                     CommandArgument='<%# Eval("ParameterID") %>'
                     CssClass="DeleteButton"
                     ImageUrl="~/icons/sigma/Delete_16X16_Gray.png"
                     OnClick="DeleteButton_Click"
                     ResourceKey="Delete" />
               </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Type">
               <ItemTemplate>
                  <asp:Label ID="TypeLabel" runat="server" Text='<%# Eval("Type") %>' />
               </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Name">
               <%-- The ConfirmData class is used as a selector to inject this text in a jQuery confirmation dialog --%>
               <ItemTemplate>
                  <asp:Label ID="NameLabel" runat="server" CssClass="ConfirmData" Text='<%# Eval("Name") %>' />
               </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Argument">
               <ItemTemplate>
                  <asp:Label ID="ArgumentLabel" runat="server" Text='<%# Eval("Argument") %>' />
               </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="UseAsHash">
               <ItemStyle HorizontalAlign="Center" Width="1%" />
               <HeaderStyle Wrap="false" />
               <ItemTemplate>
                  <asp:Image ID="HashImage" runat="server"
                     ImageUrl="~/icons/Sigma/Checked_16x16_Standard.png"
                     Visible='<%# Eval("UseAsHash") %>' />
               </ItemTemplate>
            </asp:TemplateColumn>
         </Columns>
      </asp:DataGrid>
      <fieldset>
         <div class="dnnFormItem">
            <dnn:Label ID="ParameterTypeLabel" runat="server"
               ControlName="ParameterTypeDropDownList"
               ResourceKey="ParameterType"
               Suffix=":" />
            <asp:DropDownList ID="ParameterTypeDropDownList" runat="server"
               AutoPostBack="true"
               OnLoad="ParameterTypeDropDownList_Load"
               OnSelectedIndexChanged="ParameterTypeDropDownList_SelectedIndexChanged" />
         </div>

         <div class="dnnFormItem" id="UseAsHashPanel" runat="server">
            <dnn:Label ID="UseAsHashLabel" runat="server"
               ControlName="UseAsHashCheckBox"
               ResourceKey="UseAsHash"
               Suffix="?" />
            <asp:CheckBox ID="UseAsHashCheckBox" runat="server"
               AutoPostBack="true"
               OnCheckedChanged="UseAsHashCheckBox_CheckedChanged" />
         </div>

         <div class="dnnFormItem">
            <dnn:Label ID="ParameterNameLabel" runat="server"
               ControlName="ParameterNameTextBox"
               CssClass="dnnFormRequired"
               ResourceKey="ParameterName"
               Suffix=":" />
            <asp:TextBox ID="ParameterNameTextBox" runat="server"
               Columns="30"
               MaxLength="50" />
            <asp:RequiredFieldValidator ID="ParameterNameValidator" runat="server"
               ControlToValidate="ParameterNameTextBox"
               CssClass="dnnFormMessage dnnFormError"
               ForeColor="White"
               ResourceKey="ParameterNameValidator.ErrorMessage"
               ValidationGroup="ParametersFormValidation" />
         </div>

         <div id="ParameterValuePanel" runat="server" class="dnnFormItem">
            <dnn:Label ID="ParameterValueLabel" runat="server"
               ControlName="ParameterValueTextBox"
               CssClass="dnnFormRequired"
               ResourceKey="ParameterValue"
               Suffix=":" />
            <asp:TextBox ID="ParameterValueTextBox" runat="server" />
            <asp:RequiredFieldValidator ID="ParameterValueValidator" runat="server"
               ControlToValidate="ParameterValueTextBox"
               CssClass="dnnFormMessage dnnFormError"
               ForeColor="White"
               ResourceKey="ParameterValueValidator.ErrorMessage"
               ValidationGroup="ParametersFormValidation" />
         </div>
      </fieldset>
      <ul class="dnnActions dnnClear ParametersForm">
         <li>
            <asp:LinkButton ID="UpdateParameterButton" runat="server"
               CausesValidation="true"
               CommandArgument="-1"
               CssClass="dnnPrimaryAction"
               OnClick="UpdateParameterButton_Click"
               ValidationGroup="ParametersFormValidation" />
         </li>
         <li>
            <asp:LinkButton ID="ResetParametersFormButton" runat="server"
               CausesValidation="false"
               CssClass="dnnSecondaryAction"
               OnClick="ResetParametersFormButton_Click"
               ResourceKey="Reset" />
         </li>
      </ul>
   </div>

   <ul class="dnnActions dnnClear">
      <li>
         <asp:LinkButton ID="UpdateButton" runat="server"
            CausesValidation="true"
            CssClass="dnnPrimaryAction"
            OnClick="UpdateButton_Click"
            ResourceKey="cmdUpdate"
            ValidationGroup="IFrameParametersValidation" />
      </li>
      <li>
         <asp:LinkButton ID="CancelButton" runat="server"
            CausesValisation="false"
            CssClass="dnnSecondaryAction"
            OnClick="CancelButton_Click"
            ResourceKey="Cancel" />
      </li>
   </ul>
</div>

