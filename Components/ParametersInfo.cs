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

using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security;
using System;
using System.Threading;
using System.Web;
using System.Web.Caching;

namespace DotNetNuke.Modules.IFrame.Components
{
   [TableName("IFrame_Parameters")]
   [PrimaryKey("ParameterID", AutoIncrement=true)]
   [Scope("ModuleID")]
   [Cacheable("IFrame_Parameters", CacheItemPriority.Normal, 20)]
   public class ParametersInfo
   {
      private char[] _invalidCharacters = new char[]{'<', '>', ',', ' ', ';', '\"', '\'', '?', '&' };

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

      private ModuleInfo _moduleInfo = null;
      protected ModuleInfo ModuleInfo
      {
         get
         {
            if (_moduleController == null)
               _moduleInfo = ModuleController.GetModule(ModuleID);
            return _moduleInfo;
         }
      }

      private PortalController _portalController = null;
      protected PortalController PortalController
      {
         get
         {
            if (_portalController == null)
               _portalController = new PortalController();
            return _portalController;
         }
      }

      private PortalInfo _portalInfo = null;
      private PortalInfo PortalInfo
      {
         get
         {
            if (_portalInfo == null)
               _portalInfo = PortalController.GetPortal(ModuleInfo.PortalID);
            return _portalInfo;
         }
      }

      public int ParameterID { get; set; }
      public int ModuleID { get; set; }
      public string Name { get; set; }
      public ParameterType Type { get; set; }
      public string Argument { get; set; }
      public bool UseAsHash { get; set; }

      public override string ToString()
      {
         if (UseAsHash)
            return HttpUtility.UrlEncode(Name);
         else
            return string.Format("{0}={1}", Name, HttpUtility.UrlEncode(GetValue()));
      }

      [IgnoreColumn]
      public bool IsValid
      {
         get
         {
            if ((string.IsNullOrEmpty(Name)) || (Name.IndexOfAny(_invalidCharacters) != -1))
               return false;
            else
               return (!(IsArgumentRequired()) || (Argument.Length > 0));
         }
      }

      public bool IsArgumentRequired()
      {
         bool argumentIsRequired;
         switch (Type)
         {
            case ParameterType.StaticValue:
            case ParameterType.PassThrough:
            case ParameterType.UserCustomProperty:
            case ParameterType.FormPassThrough:
               argumentIsRequired = true;
               break;
            default:
               argumentIsRequired = false;
               break;
         }
         return argumentIsRequired;
      }

      public string GetValue()
      {
         string result = string.Empty;
         PortalSecurity portalSecurity = new PortalSecurity();

         switch (Type)
         {
            case ParameterType.StaticValue:
               result = Argument;
               break;
            case ParameterType.PassThrough:
               if (string.IsNullOrEmpty(Argument))
                  result = string.Empty;
               else
               {
                  if (HttpContext.Current != null)
                  {
                     HttpRequest request = HttpContext.Current.Request;
                     string getString = CStrN(request.QueryString[Argument]);
                     result = portalSecurity.InputFilter(getString, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting);
                  }
                  else
                  {
                     result = string.Empty;
                  }
               }
               break;
            case ParameterType.FormPassThrough:
               if (string.IsNullOrEmpty(Argument))
                  result = string.Empty;
               else
               {
                  if (HttpContext.Current != null)
                  {
                     string postString = CStrN(HttpContext.Current.Request.Form[Argument]);
                     result = portalSecurity.InputFilter(postString, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting);
                  }
                  else
                  {
                     result = string.Empty;
                  }
               }
               break;
            case ParameterType.PortalID:
               result = Convert.ToString(PortalInfo.PortalID);
               break;
            case ParameterType.PortalName:
               result = Convert.ToString(PortalInfo.PortalName);
               break;
            case ParameterType.TabID:
               result = Convert.ToString(ModuleInfo.TabID);
               break;
            case ParameterType.ModuleID:
               result = Convert.ToString(ModuleID);
               break;
            case ParameterType.Locale:
               result = Thread.CurrentThread.CurrentCulture.Name;
               break;
            default:
               UserInfo currentUser = UserController.Instance.GetCurrentUserInfo();
               switch(Type)
               {
                  case ParameterType.UserCustomProperty:
                     if (string.IsNullOrEmpty(Argument))
                        result = string.Empty;
                     else
                        result = currentUser.Profile.GetPropertyValue(Argument);
                     break;
                  case ParameterType.UserID:
                     result = Convert.ToString(currentUser.UserID);
                     break;
                  case ParameterType.UserUsername:
                     result = currentUser.Username;
                     break;
                  case ParameterType.UserFirstName:
                     result = currentUser.FirstName;
                     break;
                  case ParameterType.UserLastName:
                     result = currentUser.LastName;
                     break;
                  case ParameterType.UserDisplayname:
                     result = currentUser.DisplayName;
                     break;
                  case ParameterType.UserEmail:
                     result = currentUser.Email;
                     break;
                  case ParameterType.UserWebsite:
                     result = currentUser.Profile.Website;
                     break;
                  case ParameterType.UserIM:
                     result = currentUser.Profile.IM;
                     break;
                  case ParameterType.UserStreet:
                     result = currentUser.Profile.Street;
                     break;
                  case ParameterType.UserUnit:
                     result = currentUser.Profile.Unit;
                     break;
                  case ParameterType.UserCity:
                     result = currentUser.Profile.City;
                     break;
                  case ParameterType.UserCountry:
                     result = currentUser.Profile.Country;
                     break;
                  case ParameterType.UserRegion:
                     result = currentUser.Profile.Region;
                     break;
                  case ParameterType.UserPostalCode:
                     result = currentUser.Profile.PostalCode;
                     break;
                  case ParameterType.UserPhone:
                     result = currentUser.Profile.Telephone;
                     break;
                  case ParameterType.UserCell:
                     result = currentUser.Profile.Cell;
                     break;
                  case ParameterType.UserFax:
                     result = currentUser.Profile.Fax;
                     break;
                  case ParameterType.UserLocale:
                     result = currentUser.Profile.PreferredLocale;
                     break;
                  case ParameterType.UserTimeZone:
                     result = Convert.ToString(currentUser.Profile.PreferredTimeZone);
                     break;
                  case ParameterType.UserIsAuthorized:
                     result = Convert.ToString(currentUser.Membership.Approved);
                     break;
                  case ParameterType.UserIsLockedOut:
                     result = Convert.ToString(currentUser.Membership.LockedOut);
                     break;
                  case ParameterType.UserIsSuperUser:
                     result = Convert.ToString(currentUser.IsSuperUser);
                     break;
               }
               break;
         }
         return result;
      }

      private string CStrN(object value)
      {
         return CStrN(value, string.Empty);
      }


      private string CStrN(object value, string defaultValue)
      {
         if (value == null) return defaultValue;
         if (value == DBNull.Value) return defaultValue;
         if (value.ToString() == string.Empty) return defaultValue;
         return value.ToString();
      }
   }

}