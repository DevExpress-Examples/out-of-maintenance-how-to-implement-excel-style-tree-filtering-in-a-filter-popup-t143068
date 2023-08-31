' Developer Express Code Central Example:
' How to implement excel style tree filtering in a filter popup.
' 
' This example demonstrates how to filter dates using a tree and select date
' ranges.
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=T143068


Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Linq
Imports System.Windows.Forms


Namespace DateRange
	Partial Public Class Form1
		Inherits Form

		Public Sub New()
			InitializeComponent()

		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			Dim dataTable As New DataTable()
			dataTable.Columns.Add("Range Date", GetType(Date))
			dataTable.Columns.Add("Event", GetType(String))
			Dim r As New Random()
			For i As Integer = 0 To 49
				dataTable.Rows.Add(New Object() { Date.Today.AddDays(r.Next(1000)), "test" })
			Next i
            myGridControl1.DataSource = dataTable
			myGridView1.OptionsFilter.ColumnFilterPopupMode = DevExpress.XtraGrid.Columns.ColumnFilterPopupMode.Excel
		End Sub

	End Class
End Namespace
