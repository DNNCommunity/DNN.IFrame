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
Imports DotNetNuke.Modules.IFrame.Data
Imports DotNetNuke.Entities.Modules
Imports System.Xml

Namespace DotNetNuke.Modules.IFrame
    ''' <summary>
    ''' IFrame Module controller.
    ''' </summary>
    Public Class Controller
        Implements IPortable

#Region "| Fields |"

        Protected Const RootElement As String = "iframe"
        Protected Const ParameterGroupElement As String = "parameters"
        Protected Const ParameterElement As String = "param"
        Protected Const ParameterNameElement As String = "name"
        Protected Const ParameterTypeElement As String = "type"
        Protected Const ParameterArgumentElement As String = "argument"

        Private _dataProvider As DataProvider

#End Region


#Region "| Sub-Classes |"

        ''' <summary>
        ''' IFrame Module properties.
        ''' </summary>
        ''' <remarks>
        '''     All properties will be used as <c>iframe</c> tag attributes unless preceded by the 
        '''     NotAnAttributePrefix value.
        ''' </remarks>
        Public Class Properties
            Public Const NotAnAttributePrefix As String = "x-"

            'Public Const UrlParameter As String = "x-url" 'removed!

            Public Const Name As String = "name"
            Public Const Source As String = "src"
            Public Const SourceType As String = NotAnAttributePrefix + "src-type"
            Public Const Height As String = "height"
            Public Const Autoheight As String = NotAnAttributePrefix + "autoheight"
            Public Const Width As String = "width"
            Public Const CssStyle As String = "style"
            Public Const OnLoad As String = "onload"
            Public Const ToolTip As String = "title"
            Public Const Scrolling As String = "scrolling"
            Public Const Border As String = "frameborder"
            Public Const AllowTransparency As String = "allowtransparency"
            Public Const UseAsHash As String = NotAnAttributePrefix + "use-as-hash"
        End Class

#End Region


#Region "| Import/Export |"

        ''' <summary>
        ''' Exports module settings.
        ''' </summary>
        ''' <param name="ModuleID">Unique identifier of the module to be exported.</param>
        Public Function ExportModule(ByVal ModuleID As Integer) As String Implements IPortable.ExportModule

            ' constants
            Const StartTagFormat As String = "<{0}>"
            Const EndTagFormat As String = "</{0}>"
            Const TagFormat As String = "<{0}>{1}</{0}>"

            ' init vars
            Dim objController As New ModuleController
            Dim objSettings As Hashtable = objController.GetModuleSettings(ModuleID)
            Dim sbXml As New StringBuilder
            Dim colParameters As IFrameParameterCollection

            ' start xml
            sbXml.AppendFormat(StartTagFormat, RootElement)

            ' save all keys
            For Each key As Object In objSettings.Keys
                sbXml.AppendFormat(TagFormat, Convert.ToString(key), _
                                    XmlUtils.XMLEncode(Convert.ToString(objSettings(key))))
            Next

            ' save all parameters
            sbXml.AppendFormat(StartTagFormat, ParameterGroupElement)
            colParameters = GetParameters(ModuleID)
            For i As Integer = 0 To colParameters.Count - 1
                If colParameters(i).IsValid Then
                    sbXml.AppendFormat(StartTagFormat, ParameterElement)
                    sbXml.AppendFormat(TagFormat, ParameterNameElement, XmlUtils.XMLEncode(colParameters(i).Name))
                    sbXml.AppendFormat(TagFormat, ParameterTypeElement, _
                                        XmlUtils.XMLEncode( _
                                                            [Enum].GetName(colParameters(i).Type.GetType(), _
                                                                            colParameters(i).Type)))
                    If colParameters(i).IsArgumentRequired Then _
                        sbXml.AppendFormat(TagFormat, ParameterArgumentElement, _
                                            XmlUtils.XMLEncode(colParameters(i).TypeArgument))
                    sbXml.AppendFormat(EndTagFormat, ParameterElement)
                End If
            Next
            sbXml.AppendFormat(EndTagFormat, ParameterGroupElement)

            ' end xml
            sbXml.AppendFormat(EndTagFormat, RootElement)

            ' return xml
            Return sbXml.ToString()

        End Function


        ''' <summary>
        ''' Imports module settings.
        ''' </summary>
        ''' <param name="ModuleID">Unique identifier of the module to be exported.</param>
        ''' <param name="Content">XML content to import.</param>
        ''' <param name="Version">Version of the content being imported.</param>
        ''' <param name="UserID">Unique identifier of the user importing the content.</param>
        Public Sub ImportModule(ByVal ModuleID As Integer, ByVal Content As String, ByVal Version As String, _
                                 ByVal UserID As Integer) Implements IPortable.ImportModule

            ' init vars
            Dim objController As New ModuleController
            Dim objXml As XmlNode = GetContent(Content, RootElement)

            ' update settings
            For i As Integer = 0 To objXml.ChildNodes.Count - 1
                ' init vars
                Dim objNode As XmlNode = objXml.ChildNodes(i)
                Dim strKey As String = objNode.LocalName

                ' if not parameters node, save setting; otherwise, add to data store
                If strKey <> ParameterGroupElement Then
                    ' handle version conflicts
                    Select Case Version
                        Case "03.02.00"
                        Case Else
                            If strKey = "border" Then strKey = Properties.Border
                    End Select

                    ' update settings
                    objController.UpdateModuleSetting(ModuleID, strKey, objNode.InnerText)
                Else
                    ' loop thru parameters
                    For j As Integer = 0 To objNode.ChildNodes.Count - 1
                        Dim objParam As New IFrameParameter
                        objParam.ModuleID = ModuleID
                        For k As Integer = 0 To objNode.ChildNodes(j).ChildNodes.Count - 1
                            Dim objParamPropertyNode As XmlNode = objNode.ChildNodes(j).ChildNodes(k)
                            Select Case objParamPropertyNode.LocalName
                                Case ParameterNameElement
                                    objParam.Name = objParamPropertyNode.InnerText
                                Case ParameterTypeElement
                                    objParam.Type = IFrameParameter.ParseType(objParamPropertyNode.InnerText)
                                Case ParameterArgumentElement
                                    objParam.TypeArgument = objParamPropertyNode.InnerText
                            End Select
                        Next

                        ' add param
                        If objParam.IsValid Then AddParameter(objParam)
                    Next
                End If
            Next

        End Sub

#End Region


#Region "| Data Access |"

        ''' <summary>
        ''' Gets the single instance of the current <see cref="DataProvider"/>.
        ''' </summary>
        Protected ReadOnly Property DataProvider() As DataProvider
            Get
                If Me._dataProvider Is Nothing Then
                    Me._dataProvider = DataProvider.Instance
                End If
                Return Me._dataProvider
            End Get
        End Property


        ''' <summary>
        ''' Creates a new  object in the data store.
        ''' </summary>
        ''' <param name="Parameter">Parameter object.</param>
        Public Sub AddParameter(ByVal Parameter As IFrameParameter)
            DataProvider.AddParameter(Parameter)
        End Sub


        ''' <summary>
        ''' Retrieves an existing  object from the data store.
        ''' </summary>
        ''' <param name="Key">Parameter identifier.</param>
        Public Function GetParameter(ByVal Key As IFrameParameter.UniqueKey) As IFrameParameter
            Return DataProvider.GetParameter(Key)
        End Function


        ''' <summary>
        ''' Retrieves a collection of  objects from the data store.
        ''' </summary>
        ''' <param name="ModuleID">Module identifier.</param>
        Public Function GetParameters(ByVal ModuleID As Integer) As IFrameParameterCollection
            Return DataProvider.GetParameters(ModuleID)
        End Function


        ''' <summary>
        ''' Updates an existing  object in the data store.
        ''' </summary>
        ''' <param name="Parameter">Parameter object.</param>
        Public Sub UpdateParameter(ByVal Parameter As IFrameParameter)
            DataProvider.UpdateParameter(Parameter)
        End Sub


        ''' <summary>
        ''' Removes an existing  object from the data store.
        ''' </summary>
        ''' <param name="Key">Parameter identifier.</param>
        Public Sub DeleteParameter(ByVal Key As IFrameParameter.UniqueKey)
            DataProvider.DeleteParameter(Key)
        End Sub

#End Region
    End Class
End Namespace