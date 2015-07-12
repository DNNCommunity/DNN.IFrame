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

Namespace DotNetNuke.Modules.IFrame.Data
    ''' <summary>
    ''' Represents an abstract data provider for the IFrame Module.
    ''' </summary>
    ''' <history>
    '''     [flanakin]   04/10/2006     Genesis
    ''' </history>
    Public MustInherit Class DataProvider

#Region "| Fields |"

        Private Shared _provider As DataProvider = Nothing

        Public Const ProviderType As String = "data"
        Public Const ProviderNamespace As String = "DotNetNuke.Modules.IFrame"

#End Region


#Region "| Initialization |"

        ''' <summary>
        ''' Instantiates a new instance of the <c>DataProvider</c> class.
        ''' </summary>
        Shared Sub New()
        End Sub

#End Region


#Region "| Properties |"

        ''' <summary>
        ''' Gets the single instance of the <c>DataProvider</c> object.
        ''' </summary>
        Public Shared ReadOnly Property Instance() As DataProvider
            Get
                If _provider Is Nothing Then
                    _provider = CType(Reflection.CreateObject(ProviderType, ProviderNamespace, ""), DataProvider)
                End If
                Return _provider
            End Get
        End Property

#End Region


#Region "| Methods [Public] |"

        ''' <summary>
        ''' Creates a new  object in the data store.
        ''' </summary>
        ''' <param name="Parameter">Parameter object.</param>
        Public MustOverride Sub AddParameter(ByVal Parameter As IFrameParameter)


        ''' <summary>
        ''' Retrieves an existing  object from the data store.
        ''' </summary>
        ''' <param name="Key">Parameter identifier.</param>
        Public MustOverride Function GetParameter(ByVal Key As IFrameParameter.UniqueKey) As IFrameParameter


        ''' <summary>
        ''' Retrieves a collection of  objects from the data store.
        ''' </summary>
        ''' <param name="ModuleID">Module identifier.</param>
        Public MustOverride Function GetParameters(ByVal ModuleID As Integer) As IFrameParameterCollection


        ''' <summary>
        ''' Updates an existing  object in the data store.
        ''' </summary>
        ''' <param name="Parameter">Parameter object.</param>
        Public MustOverride Sub UpdateParameter(ByVal Parameter As IFrameParameter)


        ''' <summary>
        ''' Removes an existing  object from the data store.
        ''' </summary>
        ''' <param name="Key">Parameter identifier.</param>
        Public MustOverride Sub DeleteParameter(ByVal Key As IFrameParameter.UniqueKey)

#End Region
    End Class
End Namespace