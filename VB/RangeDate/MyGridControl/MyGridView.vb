' Developer Express Code Central Example:
' How to implement excel style tree filtering in a filter popup.
' 
' This example demonstrates how to filter dates using a tree and select date
' ranges.
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=T143068

Imports System
Imports System.Linq
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports System.Collections.Generic
Imports System.ComponentModel
Imports DevExpress.Data.Filtering
Imports DevExpress.XtraEditors


Namespace DateRange
	<System.ComponentModel.DesignerCategory("")>
	Public Class MyGridView
		Inherits GridView

		Public ReadOnly customName As String = "Get treelist"
		Public Sub New()
			Me.New(Nothing)
		End Sub
		Public Sub New(ByVal grid As GridControl)
			MyBase.New(grid)
			DateFilterInfo = Nothing
		End Sub
		Friend DateFilterInfo As ColumnFilterInfo
		Protected Overrides Function CreateColumnCollection() As GridColumnCollection
			Return New MyGridColumnCollection(Me)
		End Function		
		Protected Overrides Function CreateDateFilterPopup(ByVal column As GridColumn, ByVal ownerControl As System.Windows.Forms.Control, ByVal creator As Object) As DateFilterPopup

			Return New MyDateFilterPopup(Me, column, ownerControl, creator)
		End Function
		Protected Overrides Sub RaiseFilterPopupDate(ByVal filterPopup As DateFilterPopup, ByVal list As List(Of FilterDateElement))
			list.RemoveRange(0, list.Count)
			Dim filterString As String = If(DateFilterInfo IsNot Nothing, DateFilterInfo.FilterString, "")
			Dim filterCriteria As CriteriaOperator = If(DateFilterInfo IsNot Nothing, DateFilterInfo.FilterCriteria, Nothing)
			list.Add(New FilterDateElement(customName, filterString, filterCriteria))
			MyBase.RaiseFilterPopupDate(filterPopup, list)
		End Sub
		Public Property treeListSource() As Object()
		Public Function GetFilteredValues(ByVal column As GridColumn, ByVal showAll As Boolean, ByVal completed As DevExpress.Data.OperationCompleted) As Object()
			Return MyBase.GetFilterPopupValues(column, False, completed)
		End Function
		Protected Overrides Function GetFilterPopupValues(ByVal column As GridColumn, ByVal showAll As Boolean, ByVal completed As DevExpress.Data.OperationCompleted) As Object()
			Dim filterPopupValues() As Object = MyBase.GetFilterPopupValues(column, showAll, completed)
			If DirectCast(column, MyGridColumn).PIsDateFilterPopup Then
				treeListSource = filterPopupValues
			End If
			Return filterPopupValues
		End Function
	End Class
End Namespace