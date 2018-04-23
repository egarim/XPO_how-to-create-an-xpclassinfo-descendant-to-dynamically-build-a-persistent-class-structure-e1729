Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports DevExpress.Xpo
Imports DevExpress.Xpo.Metadata
Imports System.Collections.Generic

Namespace DynamicDataTable
	Public Class DynamicDataTableClassInfo
		Inherits XPClassInfo
		Private ReadOnly ownMembersCore As IList(Of XPMemberInfo) = New List(Of XPMemberInfo)()
		Private ReadOnly tableNameCore As String
		Private ReadOnly baseClassCore As XPClassInfo
		Public Sub New(ByVal dictionary As XPDictionary, ByVal table As DataTable)
			MyBase.New(dictionary)
			baseClassCore = dictionary.QueryClassInfo(GetType(XPDataTableObject))
			tableNameCore = table.TableName
			For Each column As DataColumn In table.Columns
				Dim member As XPCustomMemberInfo = CreateMember(column.Caption, column.DataType)
				member.AddAttribute(New PersistentAttribute(column.ColumnName))
				member.AddAttribute(New DisplayNameAttribute(column.Caption))
				If table.PrimaryKey.Length > 0 AndAlso table.PrimaryKey(0) Is column Then
					member.AddAttribute(New KeyAttribute(column.AutoIncrement))
				End If
			Next column
		End Sub
		Public Overrides ReadOnly Property AssemblyName() As String
			Get
				Return Me.GetType().Assembly.FullName
			End Get
		End Property
		Public Overrides ReadOnly Property BaseClass() As XPClassInfo
			Get
				Return baseClassCore
			End Get
		End Property
		Public Overrides ReadOnly Property ClassType() As Type
			Get
				Return BaseClass.ClassType
			End Get
		End Property
		Protected Overrides Function CreateObjectInstance(ByVal session As Session, ByVal instantiationClassInfo As XPClassInfo) As Object
			Return New XPDataTableObject(session, instantiationClassInfo)
		End Function
		Public Overrides ReadOnly Property FullName() As String
			Get
				Return tableNameCore
			End Get
		End Property
		Public Overrides ReadOnly Property OwnMembers() As ICollection(Of XPMemberInfo)
			Get
				Return ownMembersCore
			End Get
		End Property
		Public Overrides Sub AddMember(ByVal newMember As XPMemberInfo)
			ownMembersCore.Add(newMember)
			MyBase.AddMember(newMember)
		End Sub
		Protected Overrides ReadOnly Property CanPersist() As Boolean
			Get
				Return Not HasAttribute(NonPersistentAttribute.AttributeType)
			End Get
		End Property
	End Class
End Namespace