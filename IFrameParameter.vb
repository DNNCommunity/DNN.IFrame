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
Imports System.Threading

Namespace DotNetNuke.Modules.IFrame.Domain
    ''' <summary>
    ''' Represents a querystring parameter for the IFrame module.
    ''' </summary>
    ''' <history>
    '''     [flanakin]   04/08/2006     Genesis
    ''' </history>
    Public Class IFrameParameter

#Region "| Fields |"

        Private _key As UniqueKey
        Private _moduleID As Integer
        Private _name As String
        Private _type As IFrameParameterType
        Private _typeArgument As String
        Private _invalidCharacters As Char() = "<>, ;""'?&".ToCharArray

#End Region


#Region "| Initialization |"

        ''' <summary>
        ''' Instantiates a new instance of the <c>IFrameParameter</c> module.
        ''' </summary>
        Public Sub New()
            Me._key = New UniqueKey
            Me._type = IFrameParameterType.StaticValue
        End Sub

#End Region


#Region "| Sub-Classes |"

        ''' <summary>
        ''' Represents a unique <see cref="IFrameParameter"/>.
        ''' </summary>
        Public Class UniqueKey
            ' fields
            Private _id As Integer = -1

            ''' <summary>
            ''' Gets or sets the unique identifier
            ''' </summary>
            Public Property ID() As Integer
                Get
                    Return Me._id
                End Get
                Set(ByVal Value As Integer)
                    Me._id = Value
                End Set
            End Property

            ''' <summary>
            ''' Gets a value indicating whether the <see cref="IFrameParameter"/> is new.
            ''' </summary>
            Public ReadOnly Property IsNew() As Boolean
                Get
                    Return (Me._id <= 0)
                End Get
            End Property
        End Class

#End Region


#Region "| Properties |"

#Region "| Key |"

        ''' <summary>
        ''' Gets or sets the unique key.
        ''' </summary>
        Public Property Key() As UniqueKey
            Get
                Return Me._key
            End Get
            Set(ByVal Value As UniqueKey)
                Me._key = Value
            End Set
        End Property


        ''' <summary>
        ''' Gets or sets the unique identifier.
        ''' </summary>
        Public Property ID() As Integer
            Get
                Return Key.ID
            End Get
            Set(ByVal Value As Integer)
                Key.ID = Value
            End Set
        End Property


        ''' <summary>
        ''' Gets a value indicating whether the object is new.
        ''' </summary>
        Public ReadOnly Property IsNew() As Boolean
            Get
                Return Key.IsNew
            End Get
        End Property

#End Region


        ''' <summary>
        ''' Gets or sets the module identifier.
        ''' </summary>
        Public Property ModuleID() As Integer
            Get
                Return Me._moduleID
            End Get
            Set(ByVal Value As Integer)
                Me._moduleID = Value
            End Set
        End Property


        ''' <summary>
        ''' Gets or sets the parameter name.
        ''' </summary>
        Public Property Name() As String
            Get
                Return Me._name
            End Get
            Set(ByVal Value As String)
                Me._name = Value
            End Set
        End Property


        ''' <summary>
        ''' Gets or sets the parameter type.
        ''' </summary>
        Public Property Type() As IFrameParameterType
            Get
                Return Me._type
            End Get
            Set(ByVal Value As IFrameParameterType)
                Me._type = Value
            End Set
        End Property


        ''' <summary>
        ''' Gets or sets the parameter type argument.
        ''' </summary>
        ''' <remarks>
        ''' The parameter type argument varies in purpose and usage. For instance, the argument may 
        ''' represent a value, querystring parameter name, or custom user profile property.
        ''' </remarks>
        Public Property TypeArgument() As String
            Get
                Return Me._typeArgument
            End Get
            Set(ByVal Value As String)
                Me._typeArgument = Value
            End Set
        End Property


        ''' <summary>
        ''' Gets a value indicating whether the parameter is valid.
        ''' </summary>
        Public ReadOnly Property IsValid() As Boolean
            Get
                ' check name
                If Name Is Nothing OrElse Name.Length = 0 OrElse Name.IndexOfAny(_invalidCharacters) <> -1 Then _
                    Return False
                Return Not IsArgumentRequired() Or CStrN(TypeArgument).Length > 0
            End Get
        End Property

#End Region


#Region "| Methods [Public] |"

        ''' <summary>
        ''' Converts a string representation of a <see cref="IFrameParameterType"/> to its object value.
        ''' </summary>
        ''' <param name="Type">Value to convert.</param>
        Public Shared Function ParseType(ByVal Type As String) As IFrameParameterType
            Dim objType As IFrameParameterType
            Try
                objType = CType([Enum].Parse(objType.GetType(), Type), IFrameParameterType)
            Catch ex As Exception
                objType = IFrameParameterType.StaticValue
            End Try
            Return objType
        End Function


        ''' <summary>
        ''' Converts the <see cref="Type"/> to a string value.
        ''' </summary>
        Public Function ConvertTypeToString() As String
            Return [Enum].GetName(Type.GetType(), Type)
        End Function


        ''' <summary>
        ''' Determines parameter value based on applied settings.
        ''' </summary>
        Public Function GetValue() As String
            ' init vars
            Dim ArgumentIsEmpty As Boolean = (TypeArgument Is Nothing OrElse TypeArgument.Length = 0)
            Dim objSecurity As New PortalSecurity
            ' get value based on type
            Select Case Type

                Case IFrameParameterType.StaticValue ' static
                    Return TypeArgument

                Case IFrameParameterType.PassThrough ' pass-thru parameter / Querystring
                    If ArgumentIsEmpty Then Return ""
                    If Not HttpContext.Current Is Nothing Then
                        Dim qString As String = CStrN(HttpContext.Current.Request.QueryString(TypeArgument))
                        Return objSecurity.InputFilter(qString, PortalSecurity.FilterFlag.NoMarkup _
                                                                 Or PortalSecurity.FilterFlag.NoScripting)
                    End If

                Case IFrameParameterType.FormPassThrough ' pass-thru parameter 
                    If ArgumentIsEmpty Then Return ""
                    If Not HttpContext.Current Is Nothing Then
                        Dim fString As String = CStrN(HttpContext.Current.Request.Form(TypeArgument))
                        Return objSecurity.InputFilter(fString, PortalSecurity.FilterFlag.NoMarkup _
                                                                 Or PortalSecurity.FilterFlag.NoScripting)

                    End If

                Case IFrameParameterType.PortalID ' portal id
                    Return Convert.ToString(GetPortalSettings().PortalId)

                Case IFrameParameterType.PortalName ' portal name
                    Return Convert.ToString(GetPortalSettings().PortalName)

                Case IFrameParameterType.TabID ' active tab id
                    Return Convert.ToString(GetPortalSettings().ActiveTab.TabID)

                Case IFrameParameterType.ModuleID
                    Return Convert.ToString(ModuleID)

                Case IFrameParameterType.Locale
                    Return Thread.CurrentThread.CurrentCulture.ToString()

                Case Else ' user property
                    ' get current user
                    Dim objUser As UserInfo = UserController.GetCurrentUserInfo

                    ' handle user property
                    Select Case Type

                        Case IFrameParameterType.UserCustomProperty ' custom property
                            If ArgumentIsEmpty Then Return ""
                            Return objUser.Profile.GetPropertyValue(TypeArgument)
                        Case IFrameParameterType.UserID
                            Return Convert.ToString(objUser.UserID)

                        Case IFrameParameterType.UserUsername
                            Return objUser.Username

                        Case IFrameParameterType.UserFirstName
                            Return objUser.FirstName

                        Case IFrameParameterType.UserLastName
                            Return objUser.LastName

                        Case IFrameParameterType.UserFullName, IFrameParameterType.UserDisplayname
                            Return objUser.DisplayName

                        Case IFrameParameterType.UserEmail
                            Return objUser.Email

                        Case IFrameParameterType.UserWebsite
                            Return objUser.Profile.Website

                        Case IFrameParameterType.UserIM
                            Return objUser.Profile.IM

                        Case IFrameParameterType.UserStreet
                            Return objUser.Profile.Street

                        Case IFrameParameterType.UserUnit
                            Return objUser.Profile.Unit

                        Case IFrameParameterType.UserCity
                            Return objUser.Profile.City

                        Case IFrameParameterType.UserCountry
                            Return objUser.Profile.Country

                        Case IFrameParameterType.UserRegion
                            Return objUser.Profile.Region

                        Case IFrameParameterType.UserPostalCode
                            Return objUser.Profile.PostalCode

                        Case IFrameParameterType.UserPhone
                            Return objUser.Profile.Telephone

                        Case IFrameParameterType.UserCell
                            Return objUser.Profile.Cell

                        Case IFrameParameterType.UserFax
                            Return objUser.Profile.Fax

                        Case IFrameParameterType.UserLocale
                            Return objUser.Profile.PreferredLocale

                        Case IFrameParameterType.UserTimeZone
                            Return Convert.ToString(objUser.Profile.TimeZone)

                        Case IFrameParameterType.UserIsAuthorized
                            Return Convert.ToString(objUser.Membership.Approved)

                        Case IFrameParameterType.UserIsLockedOut
                            Return Convert.ToString(objUser.Membership.LockedOut)

                        Case IFrameParameterType.UserIsSuperUser
                            Return Convert.ToString(objUser.IsSuperUser)

                    End Select

            End Select

            Return ""
        End Function


        ''' <summary>
        ''' Determines whether the <see cref="TypeArgument"/> is required based on the <see cref="Type"/>.
        ''' </summary>
        Public Function IsArgumentRequired() As Boolean
            Select Case Type
                Case IFrameParameterType.StaticValue, IFrameParameterType.PassThrough, _
                    IFrameParameterType.UserCustomProperty, IFrameParameterType.FormPassThrough
                    Return True
                Case Else
                    Return False
            End Select
        End Function


        ''' <summary>
        ''' Determines parameter value based on applied settings.
        ''' </summary>
        Public Overrides Function ToString() As String
            ' if valid, return formatted parameter; otherwise, return empty string
            If IsValid Then
                Return String.Format("{0}={1}", Name, HttpUtility.UrlEncode(GetValue()))
            Else
                Return ""
            End If
        End Function

#End Region

#Region "Helper Functions"

        Public Function CStrN(ByVal value As Object, Optional ByVal [default] As String = "") As String
            If value Is Nothing Then Return [default]
            If value Is DBNull.Value Then Return [default]
            If CStr(value) = "" Then Return [default]
            Return CStr(value)
        End Function

#End Region
    End Class
End Namespace