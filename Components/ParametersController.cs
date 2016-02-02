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

using DotNetNuke.Common;
using DotNetNuke.Data;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DotNetNuke.Modules.IFrame.Components
{
   public class ParametersController : IUpgradeable, IPortable
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
#endregion

#region Data Access
      public ParametersInfo GetParameter(int parameterID)
      {
         ParametersInfo parameter;
         using (IDataContext ctx = DataContext.Instance())
         {
            var rep = ctx.GetRepository<ParametersInfo>();
            parameter = rep.GetById(parameterID);
         }
         return parameter;
      }

      public ParametersInfo GetParameter(int moduleID, string name)
      {
         IEnumerable<ParametersInfo> parameters;
         using (IDataContext ctx = DataContext.Instance())
         {
            var rep = ctx.GetRepository<ParametersInfo>();
            parameters = rep.Find("WHERE ModuleID = @0 AND Name = @1", new object[] { moduleID, name });
         }
         // Can only be one or no record as there is a unique index on ModuleID + Name
         return parameters.FirstOrDefault();
      }

      public IEnumerable<ParametersInfo> GetParameters(int moduleID)
      {
         IEnumerable<ParametersInfo> parameters;
         using(IDataContext ctx = DataContext.Instance())
         {
            var rep = ctx.GetRepository<ParametersInfo>();
            parameters = rep.Get(moduleID);
         }
         return parameters;
      }

      public void AddParameter(ParametersInfo parameter)
      {
         using (IDataContext ctx = DataContext.Instance())
         {
            var rep = ctx.GetRepository<ParametersInfo>();
            rep.Insert(parameter);
         }
      }

      public void ChangeParameter(ParametersInfo parameter)
      {
         using (IDataContext ctx = DataContext.Instance())
         {
            var rep = ctx.GetRepository<ParametersInfo>();
            rep.Update(parameter);
         }
      }

      public void DropParameter(ParametersInfo parameter)
      {
         using (IDataContext ctx = DataContext.Instance())
         {
            var rep = ctx.GetRepository<ParametersInfo>();
            rep.Delete(parameter);
         }
      }
#endregion

#region IUpgradeable
      public string UpgradeModule(string version)
      {
         return string.Format("Upgrading to version {0}.", version);
      }
#endregion

#region IPortable
      public string ExportModule(int moduleID)
      {
         StringBuilder content = new StringBuilder();
         Hashtable moduleSettings = ModuleController.GetModule(moduleID).ModuleSettings;
         List<ParametersInfo> parameters = (List<ParametersInfo>)GetParameters(moduleID);

         content.Append("<IFrameParameters>");

         if (moduleSettings.Count > 0)
         {
            content.Append("<ModuleSettings>");
            string s;

            // Source
            if (!(string.IsNullOrEmpty(s = Convert.ToString(moduleSettings["Source"]))))
               content.Append(string.Format("<Source>{0}</Source>", s));

            // UrlType
            if (!(string.IsNullOrEmpty(s = Convert.ToString(moduleSettings["UrlType"]))))
               content.Append(string.Format("<UrlType>{0}</UrlType>", s));

            // Width
            if (!(string.IsNullOrEmpty(s = Convert.ToString(moduleSettings["Width"]))))
               content.Append(string.Format("<Width>{0}</Width>", s));

            // Height
            if (!(string.IsNullOrEmpty(s = Convert.ToString(moduleSettings["Height"]))))
               content.Append(string.Format("<Height>{0}</Height>", s));

            // AutoHeight
            if (!(string.IsNullOrEmpty(s = Convert.ToString(moduleSettings["AutoHeight"]))))
               content.Append(string.Format("<AutoHeight>{0}</AutoHeight>", s));

            // Scrolling
            if (!(string.IsNullOrEmpty(s = Convert.ToString(moduleSettings["Scrolling"]))))
               content.Append(string.Format("<Scrolling>{0}</Scrolling>", s));

            // Border
            if (!(string.IsNullOrEmpty(s = Convert.ToString(moduleSettings["Border"]))))
               content.Append(string.Format("<Border>{0}</Border>", s));

            // AllowTransparency
            if (!(string.IsNullOrEmpty(s = Convert.ToString(moduleSettings["AllowTransparency"]))))
               content.Append(string.Format("<AllowTransparency>{0}</AllowTransparency>", s));

            // Name
            if (!(string.IsNullOrEmpty(s = Convert.ToString(moduleSettings["Name"]))))
               content.Append(string.Format("<Name>{0}</Name>", s));

            // ToolTip
            if (!(string.IsNullOrEmpty(s = Convert.ToString(moduleSettings["ToolTip"]))))
               content.Append(string.Format("<ToolTip>{0}</ToolTip>", s));

            // CssStyle
            if (!(string.IsNullOrEmpty(s = Convert.ToString(moduleSettings["CssStyle"]))))
               content.Append(string.Format("<CssStyle>{0}</CssStyle>", s));

            // OnLoadJavaScript
            if (!(string.IsNullOrEmpty(s = Convert.ToString(moduleSettings["OnLoadJavaScript"]))))
               content.Append(string.Format("<OnLoadJavaScript>{0}</OnLoadJavaScript>", s));

            content.Append("</ModuleSettings>");
         }

         if (parameters.Count > 0)
         {
            content.Append("<Parameters>");
            foreach(ParametersInfo pi in parameters)
            {
               content.Append("<Parameter>");
               content.Append(string.Format("<Type>{0}</Type>", pi.Type));
               content.Append(string.Format("<Name>{0}</Name>", pi.Name));
               if (!(string.IsNullOrEmpty(pi.Argument)))
                  content.Append(string.Format("<Argument>{0}</Argument>", pi.Argument));
               else
                  content.Append("<Argument />");
               content.Append(string.Format("<UseAsHash>{0}</UseAsHash>", pi.UseAsHash));
               content.Append("</Parameter>");
            }
            content.Append("</Parameters>");
         }

         content.Append("</IFrameParameters>");
         return content.ToString();
      }

      public void ImportModule(int moduleID, string content, string version, int userID)
      {
         XmlNode iFrame = Globals.GetContent(content, "IFrameParameters");

         if (iFrame != null)
         {
            XmlNode moduleSettings = iFrame.SelectSingleNode("ModuleSettings");
            if (moduleSettings != null)
            {
               XmlNode x;
               // Source
               if ((x = moduleSettings.SelectSingleNode("Source")) != null)
                  ModuleController.UpdateModuleSetting(moduleID, "Source", x.InnerText);
               // UrlType
               if ((x = moduleSettings.SelectSingleNode("UrlType")) != null)
                  ModuleController.UpdateModuleSetting(moduleID, "UrlType", x.InnerText);
               // Width
               if ((x = moduleSettings.SelectSingleNode("Width")) != null)
                  ModuleController.UpdateModuleSetting(moduleID, "Width", x.InnerText);
               // Height
               if ((x = moduleSettings.SelectSingleNode("Height")) != null)
                  ModuleController.UpdateModuleSetting(moduleID, "Height", x.InnerText);
               // AutoHeight
               if ((x = moduleSettings.SelectSingleNode("AutoHeight")) != null)
                  ModuleController.UpdateModuleSetting(moduleID, "AutoHeight", x.InnerText);
               // Scrolling
               if ((x = moduleSettings.SelectSingleNode("Scrolling")) != null)
                  ModuleController.UpdateModuleSetting(moduleID, "Scrolling", x.InnerText);
               // Border
               if ((x = moduleSettings.SelectSingleNode("Border")) != null)
                  ModuleController.UpdateModuleSetting(moduleID, "Border", x.InnerText);
               // AllowTransparency
               if ((x = moduleSettings.SelectSingleNode("AllowTransparency")) != null)
                  ModuleController.UpdateModuleSetting(moduleID, "AllowTransparency", x.InnerText);
               // Name
               if ((x = moduleSettings.SelectSingleNode("Name")) != null)
                  ModuleController.UpdateModuleSetting(moduleID, "Name", x.InnerText);
               // ToolTip
               if ((x = moduleSettings.SelectSingleNode("ToolTip")) != null)
                  ModuleController.UpdateModuleSetting(moduleID, "ToolTip", x.InnerText);
               // CssStyle
               if ((x = moduleSettings.SelectSingleNode("CssStyle")) != null)
                  ModuleController.UpdateModuleSetting(moduleID, "CssStyle", x.InnerText);
               // OnLoadJavaScript
               if ((x = moduleSettings.SelectSingleNode("OnLoadJavaScript")) != null)
                  ModuleController.UpdateModuleSetting(moduleID, "OnLoadJavaScript", x.InnerText);
            }

            XmlNode parameters = iFrame.SelectSingleNode("Parameters");
            if (parameters != null)
            {
               foreach (XmlNode parameter in parameters.SelectNodes("Parameter"))
               {
                  try
                  {
                     string type = parameter.SelectSingleNode("Type").InnerText;
                     string name = parameter.SelectSingleNode("Name").InnerText;
                     string argument = parameter.SelectSingleNode("Argument").InnerText;
                     bool useAsHash = Convert.ToBoolean(parameter.SelectSingleNode("UseAsHash").InnerText);
                     bool update = true;

                     ParametersInfo pi = GetParameter(moduleID, name);
                     if (pi == null)
                     {
                        pi = new ParametersInfo();
                        update = false;
                     }

                     pi.ModuleID = moduleID;
                     ParameterType pt;
                     if (Enum.TryParse(type, out pt))
                        pi.Type = pt;
                     pi.Name = name;
                     pi.Argument = argument;
                     pi.UseAsHash = useAsHash;

                     if (update)
                        ChangeParameter(pi);
                     else
                        AddParameter(pi);
                  }
                  catch(Exception ex)
                  {
                     Exceptions.LogException(ex);
                  }
               }
            }
         }
      }
#endregion
   }
}
