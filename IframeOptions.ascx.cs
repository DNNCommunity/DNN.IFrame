/*
* DNN Corp. - http://www.dnnsoftware.com
* Copyright (c) 2016
* by DNN Corp.
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
* documentation files (the "Software"), to deal in the Software without restriction, including without limitation
* the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
* to permit persons to whom the Software is furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in all copies or substantial portions
* of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
* TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
* THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
* CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
* DEALINGS IN THE SOFTWARE.
*/

using DotNetNuke.Modules.IFrame.Components;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Framework;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotNetNuke.Modules.IFrame.UI
{
   public partial class IframeOptions : PortalModuleBase
   {
      #region Private Properties
      private ModuleController _moduleController = null;
      protected ModuleController ModuleController
      {
         get
         {
            if (_moduleController == null)
               _moduleController = new ModuleController();
            return _moduleController;
         }
      }

      private ParametersController _parametersController = null;
      protected ParametersController ParametersController
      {
         get
         {
            if (_parametersController == null)
               _parametersController = new ParametersController();
            return _parametersController;
         }
      }

      protected bool HashParameterSet
      {
         get { return Convert.ToBoolean(ViewState["HashParameterSet"]); }
         set { ViewState["HashParameterSet"] = value; }
      }

      protected string LocalSharedResourceFile
      {
         get { return string.Format("{0}{1}/{2}", ControlPath, Localization.LocalResourceDirectory, Localization.LocalSharedResourceFile); }
      }
      #endregion

      #region Settings
      private string Source
      {
         get { return Convert.ToString(Settings["Source"]); }
         set { UpdateTextSetting("Source", value); }
      }

      private string UrlType
      {
         get { return Convert.ToString(Settings["UrlType"]); }
         set { UpdateTextSetting("UrlType", value); }
      }

      private bool IgnoreSourceUrlValidation
      {
         get { return Convert.ToBoolean(Settings["IgnoreSourceUrlValidation"]); }
         set { UpdateBooleanSetting("IgnoreSourceUrlValidation", value); }
      }
      private string Width
      {
         get { return Convert.ToString(Settings["Width"]); }
         set { UpdateTextSetting("Width", value); }
      }

      private string Height
      {
         get { return Convert.ToString(Settings["Height"]); }
         set { UpdateTextSetting("Height", value); }
      }

      private bool AutoHeight
      {
         get
         {
            bool autoHeight;
            try
            {
               object o = Settings["AutoHeight"];
               if (o == null)
                  autoHeight = false;
               else
                  autoHeight = Convert.ToBoolean(o);
            }
            catch (Exception ex)
            {
               Exception ex1 = new Exception(string.Format("Invalid AutoHeight value: {0}", Settings["AutoHeight"]), ex);
               Exceptions.LogException(ex1);
               autoHeight = false;
            }
            return autoHeight;
         }
         set { UpdateBooleanSetting("AutoHeight", value); }
      }

      private Scrolling Scrolling
      {
         get
         {
            Scrolling scrolling;
            try
            {
               object o = Settings["Scrolling"];
               if (o == null)
                  scrolling = Scrolling.Auto;
               else
                  scrolling = (Scrolling)Convert.ToInt32(o);
            }
            catch (Exception ex)
            {
               Exception ex1 = new Exception(string.Format("Invalid Scrolling value: {0}", Settings["Scrolling"]), ex);
               Exceptions.LogException(ex1);
               scrolling = Scrolling.Auto;
            }
            return scrolling;
         }
         set { UpdateIntegerSetting("Scrolling", Convert.ToInt32(value)); }
      }

      private bool Border
      {
         get
         {
            bool border;
            try
            {
               border = Convert.ToBoolean(Settings["Border"]);
            }
            catch (Exception ex)
            {
               Exception ex1 = new Exception(string.Format("Invalid Border value: {0}", Settings["Border"]), ex);
               Exceptions.LogException(ex1);
               border = false;
            }
            return border;
         }
         set { UpdateBooleanSetting("Border", value); }
      }

      private bool AllowTransparency
      {
         get
         {
            bool allowTransparency;
            try
            {
               allowTransparency = Convert.ToBoolean(Settings["AllowTransparency"]);
            }
            catch (Exception ex)
            {
               Exception ex1 = new Exception(string.Format("Invalid Border value: {0}", Settings["AllowTransparency"]), ex);
               Exceptions.LogException(ex1);
               allowTransparency = false;
            }
            return allowTransparency;
         }
         set { UpdateBooleanSetting("AllowTransparency", value); }
      }

      private string Name
      {
         get { return Convert.ToString(Settings["Name"]); }
         set { UpdateTextSetting("Name", value); }
      }

      private string ToolTip
      {
         get { return Convert.ToString(Settings["ToolTip"]); }
         set { UpdateTextSetting("ToolTip", value); }
      }

      private string CssStyle
      {
         get { return Convert.ToString(Settings["CssStyle"]); }
         set { UpdateTextSetting("CssStyle", value); }
      }

      private string OnLoadJavaScript
      {
         get { return Convert.ToString(Settings["OnLoadJavaScript"]); }
         set { UpdateTextSetting("OnLoadJavaScript", value); }
      }
      #endregion

      #region Events (Page)
      protected override void OnInit(EventArgs e)
      {
         JavaScript.RequestRegistration(CommonJs.DnnPlugins);
         ClientAPI.RegisterClientReference(Page, ClientAPI.ClientNamespaceReferences.dnn);

         if (AJAX.IsEnabled())
         {
            ScriptManager.GetCurrent(Page).RegisterPostBackControl(UpdateParameterButton);
            ScriptManager.GetCurrent(Page).RegisterPostBackControl(UpdateButton);
         }
      }

      protected void Page_Load(object sender, EventArgs e)
      {
         if (!(Page.IsPostBack))
         {
            // Assign values
            SourceUrl.Url = Source;
            SourceUrl.UrlType = UrlType;
            IgnoreSourceUrlValidationCheckBox.Checked = IgnoreSourceUrlValidation;
            SourceUrlRegExpValidator.Enabled = (!(IgnoreSourceUrlValidation));
            WidthTextBox.Text = Width;
            HeightTextBox.Text = Height;
            AutoHeightCheckBox.Checked = AutoHeight;
            BorderCheckBox.Checked = Border;
            AllowTransparencyCheckBox.Checked = AllowTransparency;
            NameTextBox.Text = Name;
            ToolTipTextBox.Text = ToolTip;
            CssStyleTextBox.Text = CssStyle;
            JavascriptTextBox.Text = (AutoHeight ? string.Empty : OnLoadJavaScript);

            UpdateParameterButton.Text = Localization.GetString("AddParameter", LocalResourceFile);

            JavaScriptPanel.Visible = (!(AutoHeight));

            Localization.LocalizeDataGrid(ref ParametersGrid, LocalResourceFile);

            BindData();
         }
      }

      protected override void OnPreRender(EventArgs e)
      {
         StringBuilder scriptBuilder = new StringBuilder();
         scriptBuilder.Append("jQuery(function ($, Sys) {\r\n");

         scriptBuilder.Append("   function setUpIFrameOptionTabs() {\r\n");
         scriptBuilder.Append("      $(\"#");
         scriptBuilder.Append(IFrameOptionTabs.ClientID);
         scriptBuilder.Append("\").dnnTabs();\r\n");
         scriptBuilder.Append("   }\r\n");

         // This jQuery script adds a confirmation dialog to the delete buttons in the parameter grid.
         // The text for the parameter name is taken from the cell with the style class "ConfirmData", this
         // is only used a selector and does not apply the form stylings
         scriptBuilder.Append("   function confirmDelete() {\r\n");
         scriptBuilder.Append("      $(\".dnnGrid .DeleteButton\").each(function() {\r\n");
         scriptBuilder.Append("         $(this).dnnConfirm({\r\n");
         scriptBuilder.Append("            text: \"Are you sure you want to delete '\" + $(\".ConfirmData\", $(this).parents(\"tr\")).text() + \"'?\",\r\n");
         scriptBuilder.Append("            yesText: \"Yes\",\r\n");
         scriptBuilder.Append("            noText: \"No\",\r\n");
         scriptBuilder.Append("            title: \"Confirm\",\r\n");
         scriptBuilder.Append("            dialogClass: \"dnnFormPopup\",\r\n");
         scriptBuilder.Append("            isButton: true\r\n");
         scriptBuilder.Append("         });\r\n");
         scriptBuilder.Append("      });\r\n");
         scriptBuilder.Append("   }\r\n");

         scriptBuilder.Append("   $(document).ready(function () {\r\n");
         scriptBuilder.Append("      setUpIFrameOptionTabs();\r\n");
         scriptBuilder.Append("      Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {\r\n");
         scriptBuilder.Append("         setUpIFrameOptionTabs();\r\n");
         scriptBuilder.Append("      });\r\n");

         scriptBuilder.Append("      Sys.Application.add_load(function () {\r\n");
         scriptBuilder.Append("         confirmDelete();");
         scriptBuilder.Append("      });\r\n");

         scriptBuilder.Append("   });\r\n");

         scriptBuilder.Append("}(jQuery, window.Sys));\r\n");

         if (!(Page.ClientScript.IsStartupScriptRegistered("IFrameOptionTabs")))
            Page.ClientScript.RegisterStartupScript(GetType(), "IFrameOptionTabs", scriptBuilder.ToString(), true);

         DisplayOrHideParameterFormPanels();
      }

      protected void IgnoreSourceUrlValidationCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         CheckBox ignoreSourceUrlValidationCheckBox = (CheckBox)sender;
         SourceUrlRegExpValidator.Enabled = (!(ignoreSourceUrlValidationCheckBox.Checked));
      }

      public void ScrollingDropDownList_Load(object sender, EventArgs e)
      {
         if (!(Page.IsPostBack))
         {
            try
            {
               DropDownList scrollingDropDownList = (DropDownList)sender;
               for (int i = 0; i < Enum.GetNames(typeof(Scrolling)).Length; i++)
               {
                  ListItem li = new ListItem();
                  li.Value = Convert.ToString(i);
                  li.Text = Localization.GetString(string.Format("Scrolling.{0}.Text", Enum.GetNames(typeof(Scrolling))[i]), LocalSharedResourceFile);
                  scrollingDropDownList.Items.Add(li);
               }
               scrollingDropDownList.SelectedValue = Convert.ToString(Convert.ToInt32(Scrolling));
            }
            catch (Exception ex)
            {
               Exceptions.ProcessModuleLoadException(this, ex);
            }
         }
      }

      protected void UpdateButton_Click(object sender, EventArgs e)
      {
         try
         {
            Source = SourceUrl.Url;
            UrlType = SourceUrl.UrlType;
            IgnoreSourceUrlValidation = IgnoreSourceUrlValidationCheckBox.Checked;
            Width = WidthTextBox.Text;
            Height = HeightTextBox.Text;
            AutoHeight = AutoHeightCheckBox.Checked;
            Scrolling = (Scrolling)Convert.ToInt32(ScrollingDropDownList.SelectedValue);
            Border = BorderCheckBox.Checked;
            AllowTransparency = AllowTransparencyCheckBox.Checked;
            Name = NameTextBox.Text;
            ToolTip = ToolTipTextBox.Text;
            CssStyle = CssStyleTextBox.Text;
            OnLoadJavaScript = (JavaScriptPanel.Visible ? JavascriptTextBox.Text : string.Empty);
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(), true);
            Response.Clear();
         }
         catch (Exception ex)
         {
            Exceptions.ProcessModuleLoadException(this, ex);
         }
      }

      protected void CancelButton_Click(object sender, EventArgs e)
      {
         try
         {
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(), true);
            Response.Clear();
         }
         catch (Exception ex)
         {
            Exceptions.ProcessModuleLoadException(this, ex);
         }
      }
      #endregion

      #region Events (Controls)
      protected void AutoHeightCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         CheckBox autoHeightCheckBox = (CheckBox)sender;
         JavaScriptPanel.Visible = (!(autoHeightCheckBox.Checked));
      }
      #endregion

      #region Events (Parameters)
      protected void ParametersGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
      {
         try
         {
            DataGrid parametersGrid = (DataGrid)sender;
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
               Label typeLabel = (Label)e.Item.FindControl("TypeLabel");
               if (typeLabel != null)
                  typeLabel.Text = Localization.GetString(string.Format("ParameterType.{0}.Text", typeLabel.Text), LocalSharedResourceFile);
            }
         }
         catch (Exception ex)
         {
            Exceptions.ProcessModuleLoadException(this, ex);
         }
      }

      protected void EditButton_Click(object sender, ImageClickEventArgs e)
      {
         try
         {
            ImageButton editButton = (ImageButton)sender;
            ParametersInfo parameter = ParametersController.GetParameter(Convert.ToInt32(editButton.CommandArgument));
            ParameterTypeDropDownList.SelectedValue = Convert.ToString(Convert.ToInt32(parameter.Type));
            ParameterNameTextBox.Text = parameter.Name;
            ParameterValueTextBox.Text = parameter.Argument;
            UseAsHashCheckBox.Checked = parameter.UseAsHash;
            UpdateParameterButton.Text = Localization.GetString("UpdateParameter", LocalResourceFile);
            UpdateParameterButton.CommandArgument = editButton.CommandArgument;
            DisplayOrHideParameterFormPanels();
         }
         catch (Exception ex)
         {
            Exceptions.ProcessModuleLoadException(this, ex);
         }
      }

      protected void DeleteButton_Click(object sender, ImageClickEventArgs e)
      {
         try
         {
            ImageButton deleteButton = (ImageButton)sender;
            if (deleteButton != null)
            {
               ParametersInfo parameter = ParametersController.GetParameter(Convert.ToInt32(deleteButton.CommandArgument));
               ParametersController.DropParameter(parameter);
               BindData();
            }
         }
         catch (Exception ex)
         {
            Exceptions.ProcessModuleLoadException(this, ex);
         }
      }

      protected void ParameterTypeDropDownList_Load(object sender, EventArgs e)
      {
         if (!(Page.IsPostBack))
         {
            try
            {
               DropDownList parameterTypeDropDownList = (DropDownList)sender;
               for (int i = 0; i < Enum.GetNames(typeof(ParameterType)).Length; i++)
               {
                  ListItem li = new ListItem();
                  li.Value = Convert.ToString(i);
                  li.Text = li.Text = Localization.GetString(string.Format("ParameterType.{0}.Text", Enum.GetNames(typeof(ParameterType))[i]), LocalSharedResourceFile);
                  parameterTypeDropDownList.Items.Add(li);
               }
               parameterTypeDropDownList.SelectedValue = "0";
            }
            catch (Exception ex)
            {
               Exceptions.ProcessModuleLoadException(this, ex);
            }
         }
      }

      protected void ParameterTypeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
      {
         try
         {
            DisplayOrHideParameterFormPanels();
         }
         catch (Exception ex)
         {
            Exceptions.ProcessModuleLoadException(this, ex);
         }
      }

      protected void UseAsHashCheckBox_CheckedChanged(object sender, EventArgs e)
      {
         try
         {
            DisplayOrHideParameterFormPanels();
         }
         catch (Exception ex)
         {
            Exceptions.ProcessModuleLoadException(this, ex);
         }
      }

      protected void UpdateParameterButton_Click(object sender, EventArgs e)
      {
         try
         {
            LinkButton updateParameterButton = (LinkButton)sender;
            ParametersInfo parameter;

            if (updateParameterButton.CommandArgument == "-1")
               parameter = new ParametersInfo();
            else
               parameter = ParametersController.GetParameter(Convert.ToInt32(updateParameterButton.CommandArgument));
            parameter.ModuleID = ModuleId;
            parameter.Type = (ParameterType)Convert.ToInt32(ParameterTypeDropDownList.SelectedValue);
            parameter.Name = ParameterNameTextBox.Text;
            parameter.Argument = (ParameterValuePanel.Visible ? ParameterValueTextBox.Text : null);
            parameter.UseAsHash = UseAsHashCheckBox.Checked;
            if (updateParameterButton.CommandArgument == "-1")
               ParametersController.AddParameter(parameter);
            else
               ParametersController.ChangeParameter(parameter);

            ResetParametersForm(true);
         }
         catch (Exception ex)
         {
            Exceptions.ProcessModuleLoadException(this, ex);
         }
      }

      protected void ResetParametersFormButton_Click(object sender, EventArgs e)
      {
         try
         {
            ResetParametersForm(false);
         }
         catch (Exception ex)
         {
            Exceptions.ProcessModuleLoadException(this, ex);
         }
      }
      #endregion

      #region Private Methods
      private void BindData()
      {
         List<ParametersInfo> parameters = (List<ParametersInfo>)ParametersController.GetParameters(ModuleId);
         if ((parameters == null) || (parameters.Count == 0))
         {
            ParametersMessage.Visible = true;
            ParametersGrid.Visible = false;
         }
         else
         {
            ParametersMessage.Visible = false;
            ParametersGrid.Visible = true;
            ParametersGrid.DataSource = parameters;
            ParametersGrid.DataBind();
            ParametersMessage.Visible = false;
            HashParameterSet = false;
            foreach (ParametersInfo p in parameters)
            {
               if (p.UseAsHash)
               {
                  HashParameterSet = true;
                  break;
               }
            }
         }
      }

      private void DisplayOrHideParameterFormPanels()
      {
         switch ((ParameterType)Convert.ToInt32(ParameterTypeDropDownList.SelectedValue))
         {
            case ParameterType.StaticValue:
            case ParameterType.PassThrough:
            case ParameterType.UserCustomProperty:
            case ParameterType.FormPassThrough:
               ParameterValuePanel.Visible = true;
               break;
            default:
               ParameterValuePanel.Visible = false;
               break;
         }

         UseAsHashPanel.Visible = ((!(HashParameterSet)) || ((UpdateParameterButton.CommandArgument != "-1") && (UseAsHashCheckBox.Checked)));

         ParameterValuePanel.Visible = (!(UseAsHashCheckBox.Checked)) && ParameterValuePanel.Visible;
      }

      private void ResetParametersForm(bool bindData)
      {
         ParameterTypeDropDownList.SelectedValue = "0";
         ParameterNameTextBox.Text = string.Empty;
         ParameterValueTextBox.Text = string.Empty;
         ParameterValuePanel.Visible = true;
         UseAsHashCheckBox.Checked = false;
         ParametersGrid.SelectedIndex = -1;
         if (bindData)
            BindData();

         UpdateParameterButton.Text = Localization.GetString("AddParameter", LocalResourceFile);
         UpdateParameterButton.CommandArgument = "-1";

         DisplayOrHideParameterFormPanels();
      }

      private void UpdateTextSetting(string setting, string settingValue)
      {
         if (Settings[setting] == null)
         {
            if (!(string.IsNullOrEmpty(settingValue)))
               ModuleController.UpdateModuleSetting(ModuleId, setting, settingValue);
         }
         else
         {
            if (!(string.IsNullOrEmpty(settingValue)))
               ModuleController.UpdateModuleSetting(ModuleId, setting, settingValue);
            else
               ModuleController.DeleteModuleSetting(ModuleId, setting);
         }
      }

      private void UpdateIntegerSetting(string setting, int? settingValue)
      {
         if (Settings[setting] == null)
         {
            if (settingValue != null)
            {
               ModuleController.UpdateModuleSetting(ModuleId, setting, settingValue.ToString());
            }
         }
         else
         {
            if (settingValue != null)
               ModuleController.UpdateModuleSetting(ModuleId, setting, settingValue.ToString());
            else
               ModuleController.DeleteModuleSetting(ModuleId, setting);
         }
      }

      private void UpdateBooleanSetting(string setting, bool settingValue)
      {
         if (Settings[setting] == null)
         {
            if (settingValue)
               ModuleController.UpdateModuleSetting(ModuleId, setting, settingValue.ToString());
         }
         else
         {
            if (settingValue)
               ModuleController.UpdateModuleSetting(ModuleId, setting, settingValue.ToString());
            else
               ModuleController.DeleteModuleSetting(ModuleId, setting);
         }
      }
      #endregion
   }
}