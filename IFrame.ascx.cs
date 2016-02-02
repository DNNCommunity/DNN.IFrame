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
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetNuke.Modules.IFrame.UI
{
   public partial class IFrame : PortalModuleBase, IActionable
   {
      #region Settings
      private string Source
      {
         get
         {
            object source = ViewState["Source"];

            if (source == null)
            {
               StringBuilder sourceBuilder = new StringBuilder();
               object o = Settings["Source"];
               if (o != null)
               {
                  if ((!(string.IsNullOrEmpty(UrlType))) && (UrlType == "T"))
                     sourceBuilder.Append(Globals.NavigateURL(Convert.ToInt32(o)));
                  else
                  {
                     sourceBuilder.Append(Convert.ToString(o));
                     if ((sourceBuilder.ToString().IndexOf("://") < 0) && (!(sourceBuilder.ToString().StartsWith("/"))))
                        sourceBuilder.Insert(0, PortalSettings.HomeDirectory);
                  }

                  string hashString = string.Empty;

                  List<ParametersInfo> parameters = (List<ParametersInfo>)ParametersController.GetParameters(ModuleId);

                  if (parameters.Count() > 0)
                  {
                     bool queryStringOpen = false;
                     foreach (ParametersInfo p in parameters)
                     {
                        if (p.UseAsHash)
                           hashString = string.Format("#{0}", p.Name);
                        else
                        {
                           sourceBuilder.Append(queryStringOpen ? "&" : "?");
                           sourceBuilder.Append(p.ToString());
                           queryStringOpen = true;
                        }
                     }
                  }
                  if (!(string.IsNullOrEmpty(hashString)))
                     sourceBuilder.Append(hashString);
               }

               ViewState["Source"] = sourceBuilder.ToString();
               return sourceBuilder.ToString();
            }
            else
            {
               return Convert.ToString(source);
            }
         }
      }

      private string UrlType
      {
         get { return Convert.ToString(Settings["UrlType"]); }
      }

      private string Width
      {
         get { return Convert.ToString(Settings["Width"]); }
      }

      private string Height
      {
         get { return Convert.ToString(Settings["Height"]); }
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
      }

      private string Name
      {
         get { return Convert.ToString(Settings["Name"]); }
      }

      private string ToolTip
      {
         get { return Convert.ToString(Settings["ToolTip"]); }
      }

      private string CssStyle
      {
         get { return Convert.ToString(Settings["CssStyle"]); }
      }

      private string OnLoadJavaScript
      {
         get { return Convert.ToString(Settings["OnLoadJavaScript"]); }
      }
      #endregion

      #region Private Properties
      private ParametersController _parametersController;
      protected ParametersController ParametersController
      {
         get
         {
            if (_parametersController == null)
               _parametersController = new ParametersController();
            return _parametersController;
         }
      }
      #endregion

      #region Event Handlers
      /// <summary>
      /// Handles the <see cref="Control.Init" /> event.
      /// </summary>
      protected void Page_Init(object sender, EventArgs e)
      {
         JavaScript.RequestRegistration(CommonJs.jQuery);
      }

      /// <summary>
      /// Handles the <see cref="Control.Load" /> event.
      /// </summary>
      protected void Page_Load(object sender, EventArgs e)
      {
         try
         {
            if (!(string.IsNullOrEmpty(Source)))
            {
               HtmlIFrame.Visible = true;
               MessageContainer.Visible = false;
            }
            else
            {
               HtmlIFrame.Visible = false;
               MessageLabel.Text = Localization.GetString("NoSource", LocalResourceFile);
               MessageContainer.Visible = true;
            }
         }
         catch (Exception ex)
         {
            // Module falied to load
            Exceptions.ProcessModuleLoadException(this, ex);
         }
      }

      /// <summary>
      /// Handles the <see cref="Control.PreRender"/> event.
      /// </summary>
      protected void Page_PreRender(object sender, EventArgs e)
      {
         // Add source and parameters
         HtmlIFrame.Attributes.Add("src", Source);

         // Add "Unsupported" text
         HtmlIFrame.InnerHtml = string.Format(Localization.GetString("Unsupported", LocalResourceFile), Source);

         // Add width
         if (!(string.IsNullOrEmpty(Width)))
            HtmlIFrame.Attributes.Add("width", Width);

         // Add height
         if (!(string.IsNullOrEmpty(Height)))
            HtmlIFrame.Attributes.Add("height", Height);

         // Auto Height
         if (AutoHeight)
         {
            int additionalHeight = 0;
            if (Border) additionalHeight += 4;
            if (Scrolling != Scrolling.No) additionalHeight += 14;

            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.Append("function autoIFrame(frameId, moreSpace){\r\n");
            scriptBuilder.Append("   try{\r\n");
            scriptBuilder.Append("      frame = document.getElementById(frameId);\r\n");
            scriptBuilder.Append("      innerDoc = (frame.contentDocument) ? frame.contentDocument : frame.contentWindow.document;\r\n");
            scriptBuilder.Append("      frame.height = innerDoc.body.scrollHeight + moreSpace;\r\n");
            scriptBuilder.Append("   }\r\n");
            scriptBuilder.Append("   catch(err){\r\n");
            scriptBuilder.Append("      window.status = err.message;\r\n");
            scriptBuilder.Append("   }\r\n");
            scriptBuilder.Append("}\r\n");

            if (!(Page.ClientScript.IsClientScriptBlockRegistered("IFrameAutoResize")))
               Page.ClientScript.RegisterClientScriptBlock(GetType(), "IFrameAutoResize", scriptBuilder.ToString(), true);

            scriptBuilder.Clear();
            scriptBuilder.Append("function resize_");
            scriptBuilder.Append(ModuleId);
            scriptBuilder.Append("() {\r\n");
            scriptBuilder.Append("   if (window.parent && window.parent.autoIFrame) {\r\n");
            scriptBuilder.Append("      window.parent.autoIFrame(\"");
            scriptBuilder.Append(HtmlIFrame.ClientID);
            scriptBuilder.Append("\", ");
            scriptBuilder.Append(additionalHeight.ToString("g"));
            scriptBuilder.Append(");\r\n");
            scriptBuilder.Append("   }\r\n");
            scriptBuilder.Append("}\r\n");

            if (!(Page.ClientScript.IsClientScriptBlockRegistered(string.Format("AutoResize-{0}", ModuleId))))
               Page.ClientScript.RegisterClientScriptBlock(GetType(), string.Format("AutoResize-{0}", ModuleId), scriptBuilder.ToString(), true);
            HtmlIFrame.Attributes.Add("onload", string.Format("javascript:resize_{0}()", ModuleId));
         }

         // Scrolling
         if (Scrolling != Scrolling.Auto)
            HtmlIFrame.Attributes.Add("scrolling", Enum.GetName(typeof(Scrolling), Scrolling));

         // Border
         if (!(Border))
            HtmlIFrame.Attributes.Add("frameborder", "0");

         // AllowTransparency
         if (AllowTransparency)
            HtmlIFrame.Attributes.Add("allowtransparency", "true");

         // Name
         if (!(string.IsNullOrEmpty(Name)))
            HtmlIFrame.Attributes.Add("name", Name);

         // Tooltip
         if (!(string.IsNullOrEmpty(ToolTip)))
            HtmlIFrame.Attributes.Add("title", ToolTip);

         // CssStyle
         if (!(string.IsNullOrEmpty(CssStyle)))
            HtmlIFrame.Attributes.Add("style", CssStyle);

         // OnLoadJavaScript
         if ((!(string.IsNullOrEmpty(OnLoadJavaScript))) && (!(AutoHeight)))
            HtmlIFrame.Attributes.Add("onload", OnLoadJavaScript);
      }
      #endregion

      #region IActionable
      public ModuleActionCollection ModuleActions
      {
         get
         {
            ModuleActionCollection actions = new ModuleActionCollection();
            actions.Add(GetNextActionID(),
               Localization.GetString("EditOptions.Action", LocalResourceFile),
               ModuleActionType.ContentOptions,
               string.Empty,
               string.Empty,
               EditUrl(),
               false,
               SecurityAccessLevel.Edit,
               true,
               false);
            return actions;
         }
      }
      #endregion
   }
}