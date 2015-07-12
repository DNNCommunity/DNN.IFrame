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
Imports Microsoft.ApplicationBlocks.Data
Imports DotNetNuke.Modules.IFrame.Domain
Imports DotNetNuke.Modules.IFrame.Data
Imports DotNetNuke.Framework.Providers

Namespace DotNetNuke.Modules.IFrame
    ''' <summary>
    ''' IFrame Module data provider for Microsoft SQL Server.
    ''' </summary>
    ''' <history>
    '''     [flanakin]   04/08/2006     Genesis
    ''' </history>
    Public Class SqlDataProvider
        Inherits DataProvider

#Region "| Fields |"

        ' provider constants
        Private Const ObjectPrefix As String = "IFrame_"
        Private Const ConnectionStringProperty As String = "connectionString"
        Private Const ConnectionStringNameProperty As String = "connectionStringName"
        Private Const ProviderPathProperty As String = "providerPath"
        Private Const ObjectQualifierProperty As String = "objectQualifier"
        Private Const DatabaseOwnerProperty As String = "databaseOwner"

        ' sproc constants
        Private Const AddParamProc As String = "AddParameter"
        Private Const GetParamProc As String = "GetParameter"
        Private Const GetParamsProc As String = "GetParameters"
        Private Const UpdateParamProc As String = "UpdateParameter"
        Private Const DeleteParamProc As String = "DeleteParameter"

        ' private
        Private _
            _providerConfiguration As ProviderConfiguration = _
                ProviderConfiguration.GetProviderConfiguration(ProviderType)

        Private _connectionString As String
        Private _providerPath As String
        Private _objectQualifier As String
        Private _databaseOwner As String

#End Region


#Region "| Initialization |"

        ''' <summary>
        ''' Instantiates a new instance of the <c>SqlDataProvider</c> class.
        ''' </summary>
        Public Sub New()
            ' init vars
            Dim _
                objProvider As Provider = _
                    CType(Me._providerConfiguration.Providers(Me._providerConfiguration.DefaultProvider), Provider)

            ' conn string
            'Get Connection string from web.config
            _connectionString = Config.GetConnectionString()

            If _connectionString = "" Then
                ' Use connection string specified in provider
                _connectionString = objProvider.Attributes("connectionString")
            End If

            ' path
            Me._providerPath = objProvider.Attributes(ProviderPathProperty)

            ' qualifier
            Me._objectQualifier = objProvider.Attributes(ObjectQualifierProperty)
            If Me._objectQualifier <> "" AndAlso Not Me._objectQualifier.EndsWith("_") Then Me._objectQualifier += "_"

            ' dbo
            Me._databaseOwner = objProvider.Attributes(DatabaseOwnerProperty)
            If Me._databaseOwner <> "" AndAlso Not Me._databaseOwner.EndsWith(".") Then Me._databaseOwner += "."
        End Sub

#End Region


#Region "| Properties |"

        ''' <summary>
        ''' Gets the connection string.
        ''' </summary>
        Public ReadOnly Property ConnectionString() As String
            Get
                Return Me._connectionString
            End Get
        End Property


        ''' <summary>
        ''' Gets the provider path.
        ''' </summary>
        Public ReadOnly Property ProviderPath() As String
            Get
                Return Me._providerPath
            End Get
        End Property


        ''' <summary>
        ''' Gets the object qualifier.
        ''' </summary>
        Public ReadOnly Property ObjectQualifier() As String
            Get
                Return Me._objectQualifier
            End Get
        End Property


        ''' <summary>
        ''' Gets the database owner.
        ''' </summary>
        Public ReadOnly Property DatabaseOwner() As String
            Get
                Return Me._databaseOwner
            End Get
        End Property

#End Region


#Region "| Methods [Public] |"

        ''' <summary>
        ''' Creates a new  object in the data store.
        ''' </summary>
        ''' <param name="Parameter">Parameter object.</param>
        Public Overrides Sub AddParameter(ByVal Parameter As IFrameParameter)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetName(AddParamProc), _
                                       Parameter.ModuleID, Parameter.Name, Parameter.Type, Parameter.TypeArgument)
        End Sub


        ''' <summary>
        ''' Retrieves an existing  object from the data store.
        ''' </summary>
        ''' <param name="Key">Parameter identifier.</param>
        Public Overrides Function GetParameter(ByVal Key As IFrameParameter.UniqueKey) As IFrameParameter
            ' init vars
            Dim objReader As IDataReader = SqlHelper.ExecuteReader(ConnectionString, GetName(GetParamProc), Key.ID)
            Dim objParam As New IFrameParameter

            ' loop thru data
            If objReader.Read Then
                objParam.ID = objReader.GetInt32(0)
                objParam.ModuleID = objReader.GetInt32(1)
                objParam.Name = objReader.GetString(2)
                objParam.Type = IFrameParameter.ParseType(objReader.GetString(3))
                If Not objReader.IsDBNull(4) Then _
                    objParam.TypeArgument = objReader.GetString(4)
            End If
            objReader.Close()

            ' return
            Return objParam
        End Function


        ''' <summary>
        ''' Retrieves a collection of  objects from the data store.
        ''' </summary>
        ''' <param name="ModuleID">Module identifier.</param>
        Public Overrides Function GetParameters(ByVal ModuleID As Integer) As IFrameParameterCollection
            ' init vars
            Dim objReader As IDataReader = SqlHelper.ExecuteReader(ConnectionString, GetName(GetParamsProc), ModuleID)
            Dim colParams As New IFrameParameterCollection

            ' loop thru data
            While objReader.Read
                Dim objParam As New IFrameParameter
                objParam.ID = objReader.GetInt32(0)
                objParam.ModuleID = objReader.GetInt32(1)
                objParam.Name = objReader.GetString(2)
                objParam.Type = IFrameParameter.ParseType(objReader.GetString(3))
                If Not objReader.IsDBNull(4) Then _
                    objParam.TypeArgument = objReader.GetString(4)
                colParams.Add(objParam)
            End While
            objReader.Close()

            ' return
            Return colParams
        End Function


        ''' <summary>
        ''' Updates an existing  object in the data store.
        ''' </summary>
        ''' <param name="Parameter">Parameter object.</param>
        Public Overrides Sub UpdateParameter(ByVal Parameter As IFrameParameter)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetName(UpdateParamProc), _
                                       Parameter.ID, Parameter.Name, Parameter.Type, Parameter.TypeArgument)
        End Sub


        ''' <summary>
        ''' Removes an existing  object from the data store.
        ''' </summary>
        ''' <param name="Key">Parameter identifier.</param>
        Public Overrides Sub DeleteParameter(ByVal Key As IFrameParameter.UniqueKey)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetName(DeleteParamProc), Key.ID)
        End Sub

#End Region


#Region "| Methods [Private] |"

        ''' <summary>
        ''' Returns the fully-qualified database object name.
        ''' </summary>
        ''' <param name="Name">Base object name.</param>
        Private Function GetName(ByVal Name As String) As String
            Return DatabaseOwner + ObjectQualifier + ObjectPrefix + Name
        End Function


        ''' <summary>
        ''' Returns <see cref="DBNull.Value"/> if <paramref name="Value"/> is null.
        ''' </summary>
        ''' <param name="Value">Object to evaluate.</param>
        Private Function GetNull(ByVal Value As Object) As Object
            Return Null.GetNull(Value, DBNull.Value)
        End Function

#End Region
    End Class
End Namespace