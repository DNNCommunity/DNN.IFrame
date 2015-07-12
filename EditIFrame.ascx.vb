'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2005
' by Perpetual Motion Interactive Systems Inc. ( http://www.perpetualmotion.ca )
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'
Imports DotNetNuke.UI.Skins.Controls
Imports DotNetNuke.Modules.IFrame.Domain
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.UI.Utilities

Namespace DotNetNuke.Modules.IFrame
    ''' <summary>
    ''' IFrame Module edit control.
    ''' </summary>
    Partial Public Class EditIFrame
        Inherits PortalModuleBase


#Region "| Sub-Classes |"

        ''' <summary>
        ''' Represents misc constants for the <see cref="EditIFrame"/> control.
        ''' </summary>
        Private NotInheritable Class Constants
            Public Const TableHeadScope As String = "scope"
            Public Const TableHeadRowScope As String = "row"
            Public Const TableHeadColScope As String = "col"
        End Class


        ''' <summary>
        ''' Represents child control names for the <see cref="EditIFrame"/> control.
        ''' </summary>
        Private NotInheritable Class ControlNames
            Public Const ParameterDeleteButton As String = "cmdDeleteParam"
            Public Const ParameterNameLabel As String = "lblParamName"
            Public Const ParameterName As String = "txtParamName"
            Public Const ParameterTypeLabel As String = "lblParamType"
            Public Const ParameterType As String = "cboParamType"
            Public Const ParameterArgumentLabel As String = "lblParamArgument"
            Public Const ParameterArgument As String = "txtParamArgument"
            Public Const ParameterIsHashLabel As String = "lblParamIsHash"
            Public Const ParameterIsHash As String = "radParamIsHash"
            Public Const ParameterScript As String = "lblParamScript"
        End Class


        ''' <summary>
        ''' Represents localization keys for the <see cref="EditIFrame"/> control.
        ''' </summary>
        Private NotInheritable Class LocaleKeys
            Public Const ParameterNameHeader As String = "Name.Header"
            Public Const ParameterTypeHeader As String = "Type.Header"
            Public Const ParameterArgumentHeader As String = "Argument.Header"
            Public Const ParameterIsHashHeader As String = "IsHash.Header"
            Public Const ParameterDeleteConfirmation As String = "DeleteParamConfirmation"
            Public Const ParameterType As String = "txtParamType"
            Public Const ParameterArgument As String = "txtParamArgument"
            Public Const ParameterInvalidHeader As String = "ParameterInvalid.Header"
            Public Const ParameterInvalid As String = "ParameterInvalid"
        End Class

#End Region


#Region "| Event Handlers |"

        ''' <summary>
        ''' Handles the <see cref="Control.Load"/> event.
        ''' </summary>
        Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            Try
                If Not Page.IsPostBack AndAlso ModuleId > 0 Then
                    'localize(grid)
                    Localization.LocalizeDataGrid(grdParams, Me.LocalResourceFile)
                    BindData()
                End If
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub


#Region "| Parameter Grid |"

        ''' <summary>
        ''' Handles the grdParams.ItemCreated event.
        ''' </summary>
        Protected Sub grdParams_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) _
            Handles grdParams.ItemCreated
            Try
                ' add delete confirmation
                Dim cmdDeleteParam As Control = e.Item.FindControl(ControlNames.ParameterDeleteButton)
                If Not cmdDeleteParam Is Nothing Then
                    ClientAPI.AddButtonConfirm(CType(cmdDeleteParam, WebControl), _
                                                Localization.GetString(LocaleKeys.ParameterDeleteConfirmation, _
                                                                        Me.LocalResourceFile))
                End If

                ' add accessible column headers
                If e.Item.ItemType = ListItemType.Header Then
                    e.Item.Cells(1).Attributes.Add(Constants.TableHeadScope, Constants.TableHeadColScope)
                    e.Item.Cells(2).Attributes.Add(Constants.TableHeadScope, Constants.TableHeadColScope)
                End If
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub


        ''' <summary>
        ''' Handles the grdParams.ItemDataBound event.
        ''' </summary>
        Protected Sub grdParams_ItemDataBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) _
            Handles grdParams.ItemDataBound
            Try
                Const LabelFormat As String = "<label style=""display:none;"" for=""{0}"">{1}</label>"
                Dim objItemType As ListItemType = CType(e.Item.ItemType, ListItemType)
                If objItemType = ListItemType.EditItem Then
                    Dim ctlLabel As Label
                    Dim ctlSetting As Control
                    Dim strTypeID As String
                    Dim strArgID As String

                    ' name
                    ctlLabel = CType(e.Item.FindControl(ControlNames.ParameterNameLabel), Label)
                    ctlSetting = e.Item.FindControl(ControlNames.ParameterName)
                    ctlLabel.Text = _
                        String.Format(LabelFormat, ctlSetting.ClientID.ToString(), _
                                       Localization.GetString(LocaleKeys.ParameterNameHeader, LocalResourceFile))

                    ' type - also add javascript to show/hide arg textbox
                    ctlLabel = CType(e.Item.FindControl(ControlNames.ParameterTypeLabel), Label)
                    ctlSetting = e.Item.FindControl(ControlNames.ParameterType)
                    ctlLabel.Text = _
                        String.Format(LabelFormat, ctlSetting.ClientID.ToString(), _
                                       Localization.GetString(LocaleKeys.ParameterTypeHeader, LocalResourceFile))
                    strTypeID = ctlSetting.ClientID
                    CType(ctlSetting, WebControl).Attributes.Add("onblur", "iframe_showArgument();")
                    CType(ctlSetting, WebControl).Attributes.Add("onchange", "iframe_showArgument();")

                    ' argument - also add javascript to set default visiblity based on type
                    ctlLabel = CType(e.Item.FindControl(ControlNames.ParameterArgumentLabel), Label)
                    ctlSetting = e.Item.FindControl(ControlNames.ParameterArgument)
                    ctlLabel.Text = _
                        String.Format(LabelFormat, ctlSetting.ClientID.ToString(), _
                                       Localization.GetString(LocaleKeys.ParameterArgumentHeader, LocalResourceFile))
                    strArgID = ctlSetting.ClientID

                    ' add javascript
                    ctlLabel = CType(e.Item.FindControl(ControlNames.ParameterScript), Label)
                    ctlLabel.Text = _
                        "<script type=""text/javascript"">iframe_showArgument();function iframe_showArgument(){document.getElementById('" + _
                        strArgID + "').style.display=((document.getElementById('" + strTypeID + _
                        "').selectedIndex<4)?'inline':'none');}</script>"
                End If
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Protected Function IsHash(ByVal paramId As Integer) As Boolean
            Dim useAsHash As Integer = CType(Settings(Controller.Properties.UseAsHash), Integer)

            Return paramId = useAsHash
        End Function

        ''' <summary>
        ''' Handles <see cref="grdParams"/>'s <see cref="DataGrid.EditCommand"/> event.
        ''' </summary>
        Protected Sub grdParams_EditCommand(ByVal sender As Object, ByVal e As DataGridCommandEventArgs) _
            Handles grdParams.EditCommand
            Try
                SaveParameterEditRow(sender, grdParams)
                grdParams.EditItemIndex = e.Item.ItemIndex
                grdParams.SelectedIndex = -1
                BindData()
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub


        ''' <summary>
        ''' Handles the grdParams.UpdateCommand event.
        ''' </summary>
        Protected Sub grdParams_UpdateCommand(ByVal sender As Object, ByVal e As DataGridCommandEventArgs) _
            Handles grdParams.UpdateCommand
            Try
                ' init vars
                Dim objParam As New IFrameParameter

                ' set values
                If e.Item.ItemIndex > -1 Then _
                    objParam.ID = Convert.ToInt32(grdParams.DataKeys(e.Item.ItemIndex))
                objParam.ModuleID = ModuleId
                objParam.Name = CType(e.Item.FindControl(ControlNames.ParameterName), TextBox).Text
                objParam.Type = _
                    IFrameParameter.ParseType( _
                                               CType(e.Item.FindControl(ControlNames.ParameterType), DropDownList). _
                                                  SelectedValue)
                If objParam.IsArgumentRequired() Then _
                    objParam.TypeArgument = CType(e.Item.FindControl(ControlNames.ParameterArgument), TextBox).Text

                ' add/update param
                If objParam.IsValid Then
                    Dim objController As New Controller
                    If objParam.IsNew Then
                        objController.AddParameter(objParam)
                    Else
                        objController.UpdateParameter(objParam)
                    End If

                    Dim UseAsHash As Boolean = CType(e.Item.FindControl(ControlNames.ParameterIsHash), RadioButton).Checked
                    If UseAsHash Then
                        Dim objModController As New ModuleController
                        objModController.UpdateModuleSetting(ModuleId, Controller.Properties.UseAsHash, objParam.ID)
                    End If

                Else
                    Skins.Skin.AddModuleMessage(Me, _
                                                 Localization.GetString(LocaleKeys.ParameterInvalidHeader, _
                                                                         LocalResourceFile), _
                                                 Localization.GetString(LocaleKeys.ParameterInvalid, LocalResourceFile), _
                                                 ModuleMessage.ModuleMessageType.RedError)
                End If
                ' clear edit row
                grdParams.EditItemIndex = -1
                ' bind data
                BindData()
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub


        ''' <summary>
        ''' Handles the grdParams.DeleteCommand event.
        ''' </summary>
        Protected Sub grdParams_DeleteCommand(ByVal sender As Object, ByVal e As DataGridCommandEventArgs) _
            Handles grdParams.DeleteCommand
            Try
                ' init vars
                Dim objController As New Controller
                Dim objParamKey As New IFrameParameter.UniqueKey

                ' assign key values
                objParamKey.ID = Convert.ToInt32(grdParams.DataKeys(e.Item.ItemIndex))

                Dim param_ As IFrameParameter = objController.GetParameter(objParamKey)
                If IsHash(param_.ID) Then
                    Dim objModuleController As New ModuleController
                    objModuleController.DeleteModuleSetting(ModuleId, Controller.Properties.UseAsHash)
                End If

                ' delete parameter
                objController.DeleteParameter(objParamKey)

                ' reset edit row
                grdParams.EditItemIndex = -1
                BindData()
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub


        ''' <summary>
        ''' Handles the <see cref="grdParams.CancelCommand"/> event.
        ''' </summary>
        Protected Sub grdParams_CancelCommand(ByVal sender As Object, ByVal e As DataGridCommandEventArgs) _
            Handles grdParams.CancelCommand
            Try
                grdParams.EditItemIndex = -1
                BindData()
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub


        ''' <summary>
        ''' Handles the <see cref="grdParams"/> control's contained <c>cboParamType.SelectedIndexChanged</c> event.
        ''' </summary>
        Protected Sub grdParams_cboParamType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        End Sub

#End Region


#Region "| Buttons |"

        ''' <summary>
        ''' Handles the <see cref="cmdAddParam.Click"/> event.
        ''' </summary>
        Private Sub cmdAddParam_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAddParam.Click
            Try
                ' save edit row
                SaveParameterEditRow(sender, cmdAddParam)

                ' add item
                grdParams.EditItemIndex = -1
                BindData(True)
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub


        ''' <summary>
        ''' Handles the <see cref="cmdSave.Click"/> event.
        ''' </summary>
        Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave.Click
            Try
                If Page.IsValid Then
                    ' save edit row
                    SaveParameterEditRow(sender, cmdAddParam)
                    ' update settings
                    Dim objController As New ModuleController
                    objController.UpdateModuleSetting(ModuleId, Controller.Properties.Source, urlSource.Url)
                    objController.UpdateModuleSetting(ModuleId, Controller.Properties.SourceType, urlSource.UrlType)
                    objController.UpdateModuleSetting(ModuleId, Controller.Properties.Name, txtName.Text)
                    objController.UpdateModuleSetting(ModuleId, Controller.Properties.Height, txtHeight.Text)
                    objController.UpdateModuleSetting(ModuleId, Controller.Properties.Autoheight, cbAutoheight.Checked.ToString)
                    objController.UpdateModuleSetting(ModuleId, Controller.Properties.Width, txtWidth.Text)
                    objController.UpdateModuleSetting(ModuleId, Controller.Properties.CssStyle, txtCssStyle.Text)
                    If IsAdmin Then objController.UpdateModuleSetting(ModuleId, Controller.Properties.OnLoad, txtOnload.Text)
                    objController.UpdateModuleSetting(ModuleId, Controller.Properties.Scrolling, cboScrolling.SelectedValue)
                    objController.UpdateModuleSetting(ModuleId, Controller.Properties.Border, cboBorder.SelectedValue)
                    objController.UpdateModuleSetting(ModuleId, Controller.Properties.ToolTip, txtToolTip.Text)
                    Dim AllowTransparencyValue As String = ""
                    If cbAllowTransparency.Checked Then AllowTransparencyValue = Boolean.TrueString.ToLower()
                    objController.UpdateModuleSetting(ModuleId, Controller.Properties.AllowTransparency, AllowTransparencyValue)
                    ' return to view control
                    Response.Redirect(NavigateURL(), True)

                End If
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub


        ''' <summary>
        ''' Handles the <see cref="cmdCancel.Click"/> event.
        ''' </summary>
        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
            Try
                Response.Redirect(NavigateURL(), True)
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

#End Region

#End Region


#Region "| Methods [Private] |"

        ''' <summary>
        ''' Saves the <see cref="grdParams"/> edit row.
        ''' </summary>
        ''' <param name="originalSender">Original sender.</param>
        ''' <param name="trigger">Object initiating the save.</param>
        Private Sub SaveParameterEditRow(ByVal originalSender As Object, ByVal trigger As Object)
            If grdParams.EditItemIndex > -1 Then
                Dim _
                    ie As _
                        New DataGridCommandEventArgs(grdParams.Items(grdParams.EditItemIndex), trigger, _
                                                      New CommandEventArgs("Update", Nothing))
                grdParams_UpdateCommand(originalSender, ie)
            End If
        End Sub


        ''' <summary>
        ''' Binds the module settings.
        ''' </summary>
        ''' <param name="ShowAddParamRow">Specifies whether an edit row should be displayed.</param>
        Private Sub BindData(Optional ByVal ShowAddParamRow As Boolean = False)
            If Not IsPostBack Then BindSettings()
            BindParameters(ShowAddParamRow)
        End Sub


        ''' <summary>
        ''' Binds module settings.
        ''' </summary>
        Private Sub BindSettings()
            urlSource.Url = CType(Settings(Controller.Properties.Source), String)
            urlSource.UrlType = CType(Settings(Controller.Properties.SourceType), String)
            txtName.Text = CType(Settings(Controller.Properties.Name), String)
            txtHeight.Text = CType(Settings(Controller.Properties.Height), String)
            txtWidth.Text = CType(Settings(Controller.Properties.Width), String)
            txtCssStyle.Text = CType(Settings(Controller.Properties.CssStyle), String)
            txtOnload.Enabled = IsAdmin
            txtOnload.Text = CType(Settings(Controller.Properties.OnLoad), String)
            txtToolTip.Text = CType(Settings(Controller.Properties.ToolTip), String)
            If CType(Settings(Controller.Properties.Scrolling), String) <> "" Then
                Try
                    cboScrolling.SelectedValue = CType(Settings(Controller.Properties.Scrolling), String)
                Catch
                End Try
            End If
            If CType(Settings(Controller.Properties.Border), String) <> "" Then
                Try
                    cboBorder.SelectedValue = CType(Settings(Controller.Properties.Border), String)
                Catch
                End Try
            End If

            Boolean.TryParse(CType(Settings(Controller.Properties.AllowTransparency), String), cbAllowTransparency.Checked)
            Boolean.TryParse(CType(Settings(Controller.Properties.Autoheight), String), cbAutoheight.Checked)

        End Sub


        ''' <summary>
        ''' Binds the <see cref="Domain.IFrameParameter"/> settings.
        ''' </summary>
        ''' <param name="ShowAddRow">Specifies whether an additional edit row should be displayed.</param>
        Private Sub BindParameters(Optional ByVal ShowAddRow As Boolean = False)
            Dim objController As New Controller
            Dim colParams As IFrameParameterCollection = objController.GetParameters(ModuleId)

            ' add new row
            If ShowAddRow Then
                colParams.Add(New IFrameParameter)
                grdParams.EditItemIndex = colParams.Count - 1
            End If

            ' apply data source
            grdParams.DataSource = colParams
            grdParams.DataBind()
            grdParams.Visible = (colParams.Count > 0 OrElse ShowAddRow)
        End Sub

        Private ReadOnly Property IsAdmin() As Boolean
            Get
                Return UserInfo.IsInRole(Me.PortalSettings.AdministratorRoleName) Or UserInfo.IsSuperUser
            End Get
        End Property

#End Region
    End Class
End Namespace