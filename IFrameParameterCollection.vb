'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2006
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


Namespace DotNetNuke.Modules.IFrame.Domain
    ''' <summary>
    ''' Represents a collection of <see cref="IFrameParameter"/> objects.
    ''' </summary>
    ''' <history>
    '''     [flanakin]   04/15/2006     Genesis
    ''' </history>
    Public NotInheritable Class IFrameParameterCollection
        Inherits CollectionBase

#Region "| Fields |"

        Private _key As IFrameParameter.UniqueKey
        Private _moduleID As Integer
        Private _ParamName As String
        Private _type As IFrameParameterType
        Private _typeArgument As String

#End Region


#Region "| Initialization |"

        ''' <summary>
        ''' Instantiates a new instance of the <c>IFrameParameterCollection</c> module.
        ''' </summary>
        Public Sub New()
        End Sub

#End Region


#Region "| Properties |"

        Default Public Overloads Property Item(ByVal index As Integer) As IFrameParameter
            Get
                Return CType(Me.List.Item(index), IFrameParameter)
            End Get
            Set(ByVal value As IFrameParameter)
                Me.List.Item(index) = value
            End Set
        End Property

        Default Public Overloads ReadOnly Property Item(ByVal ParamName As String) As IFrameParameter
            Get
                Return Me(IndexOf(ParamName))
            End Get
        End Property

#End Region

#Region "| Methods [Public] |"

        Public Function Add(ByVal Param As IFrameParameter) As Integer
            Return Me.List.Add(Param)
        End Function


        Public Overloads Function Contains(ByVal Param As IFrameParameter) As Boolean
            Return Me.List.Contains(Param)
        End Function


        Public Overloads Function Contains(ByVal ParamName As String) As Boolean
            Return (IndexOf(ParamName) >= 0)
        End Function


        Public Overloads Function IndexOf(ByVal Param As IFrameParameter) As Integer
            Return Me.List.IndexOf(Param)
        End Function


        Public Overloads Function IndexOf(ByVal ParamName As String) As Integer
            For i As Integer = 0 To Count - 1
                If Me(i).Name = ParamName Then
                    Return i
                End If
            Next
            Return -1
        End Function


        Public Overloads Sub Remove(ByVal Param As IFrameParameter)
            InnerList.Remove(Param)
        End Sub


        Public Overloads Sub Remove(ByVal ParamName As String)
            Me.RemoveAt(IndexOf(ParamName))
        End Sub


        Public Overrides Function ToString() As String
            Dim sbValue As New StringBuilder
            Dim bParameterAdded As Boolean = False
            For i As Integer = 0 To Me.Count - 1
                Dim param As String = Me(i).ToString()
                If param.Length > 0 Then
                    If bParameterAdded Then sbValue.Append("&")
                    sbValue.Append(param)
                    bParameterAdded = True
                End If
            Next
            Return sbValue.ToString()
        End Function

#End Region


#Region "| Methods [Protected] |"

        Protected Overrides Sub OnValidate(ByVal value As Object)
            If Not TypeOf value Is IFrameParameter Then
                Throw New ArgumentException("Must add a IFrameParameter")
            End If
        End Sub

#End Region
    End Class
End Namespace