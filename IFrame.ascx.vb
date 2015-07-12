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
Imports DotNetNuke.Modules.IFrame.Domain
Imports DotNetNuke.Entities.Modules.Actions
Imports DotNetNuke.Entities.Modules

Namespace DotNetNuke.Modules.IFrame
    ''' <summary>
    ''' IFrame Module view control.
    ''' </summary>
    Partial Public Class IFrame
        Inherits PortalModuleBase
        Implements IActionable

#Region "| Constants |"

        ' constants
        Public Const SupportKey As String = "Support"
        Private Const ResizeScript As String = "function autoIframe(frameId, moreSpace){" _
            + "try{" _
            + "frame = document.getElementById(frameId);" _
            + "innerDoc = (frame.contentDocument) ? frame.contentDocument : frame.contentWindow.document;" _
            + "objToResize = (frame.style) ? frame.style : frame;" _
            + "objToResize.height =  innerDoc.body.scrollHeight +moreSpace;" _
            + "}" _
            + "catch(err){ " _
            + "window.status = err.message;" _
            + "}}"

#End Region

#Region "| Event Handlers |"

                        ''' <summary>
                        ''' Handles the <see cref="Control.Load"/> event.
                        ''' </summary>
        Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            Try

                ' get default
                Dim strSource As String = Convert.ToString(Settings(Controller.Properties.Source))
                Dim strSourceType As String = Convert.ToString(Settings(Controller.Properties.SourceType))
                Dim useAsHash As String = CType(Settings(Controller.Properties.UseAsHash), String)
                Dim colParams As IFrameParameterCollection = (New Controller).GetParameters(ModuleId)

                ' if source is specified
                If Not String.IsNullOrEmpty(strSource) Then
                    If Not String.IsNullOrEmpty(strSourceType) AndAlso strSourceType.Equals("T") Then
                        strSource = NavigateURL(Convert.ToInt32(strSource))
                    ElseIf strSource.IndexOf("://") < 0 AndAlso Not strSource.StartsWith("/") Then
                        ' prepend portal directory
                        strSource = PortalSettings.HomeDirectory + strSource
                    End If

                    ' append dynamic parameters
                    If colParams.Count > 0 Then
                        Dim hashParam As String = String.Empty
                        If Not useAsHash Is Nothing Then
                            For Each param_ As IFrameParameter In colParams
                                If param_.ID = useAsHash Then
                                    hashParam = param_.ToString()
                                    Exit For
                                End If
                            Next
                        End If

                        Dim params_ As String = colParams.ToString()
                        If Not String.IsNullOrEmpty(hashParam) Then
                            params_ = params_.Replace("&" & hashParam, String.Empty).Replace(hashParam, String.Empty) + "#" & hashParam.Substring(hashParam.IndexOf("=") + 1)
                        End If

                        strSource += IIf(strSource.IndexOf("?") = -1, "?", "&").ToString() + params_
                    End If


                    ' add source and unsupported text
                    htmIFrame.Attributes.Add(Controller.Properties.Source, strSource)
                    htmIFrame.InnerText = Localization.GetString(SupportKey, Me.LocalResourceFile)

                    ' add attributes
                    For Each key As Object In Settings.Keys
                        ' if valid attribute, setting has value, and the attribute hasn't already been added...
                        If Not Convert.ToString(key).StartsWith(Controller.Properties.NotAnAttributePrefix) _
                           AndAlso Convert.ToString(Settings(key)) <> "" _
                           AndAlso Convert.ToString(key) <> Controller.Properties.Source _
                           AndAlso Convert.ToString(key).Length > 0 Then
                            htmIFrame.Attributes.Add(Convert.ToString(key), Convert.ToString(Settings(key)))
                        End If
                    Next
                Else
                    htmIFrame.Visible = False
                End If
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

#End Region

#Region "| Properties |"

        ''' <summary>
        ''' Gets a collection of <see cref="Actions.ModuleAction"/> objects representing the actions a user 
        ''' can take from the control.
        ''' </summary>
        Public ReadOnly Property ModuleActions() As ModuleActionCollection Implements IActionable.ModuleActions
            Get
                Dim Actions As New ModuleActionCollection
                Actions.Add(GetNextActionID, Localization.GetString(ModuleActionType.AddContent, LocalResourceFile), _
                             ModuleActionType.AddContent, "", "", EditUrl(), False, SecurityAccessLevel.Edit, True, _
                             False)
                Return Actions
            End Get
        End Property

#End Region

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            Dim isAutoHeight As Boolean = False
            Dim AdditionalHeight As Integer = 0
            If Settings.ContainsKey(Controller.Properties.Border) AndAlso Integer.TryParse(CType(Settings(Controller.Properties.Border), String), AdditionalHeight) Then AdditionalHeight *= 4
            If Settings.ContainsKey(Controller.Properties.Scrolling) AndAlso CType(Settings(Controller.Properties.Scrolling), String).ToLower() = "yes" Then AdditionalHeight += 14

            If Settings.ContainsKey(Controller.Properties.Autoheight) AndAlso Boolean.TryParse(CType(Settings(Controller.Properties.Autoheight), String), isAutoHeight) AndAlso isAutoHeight Then
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "IFrameAutoResize", ResizeScript, True)
                htmIFrame.Attributes.Add("onload", "if (window.parent && window.parent.autoIframe) {window.parent.autoIframe('" + htmIFrame.ClientID + "'," + AdditionalHeight.ToString("g") + ");}")
            End If

        End Sub

    End Class
End Namespace
