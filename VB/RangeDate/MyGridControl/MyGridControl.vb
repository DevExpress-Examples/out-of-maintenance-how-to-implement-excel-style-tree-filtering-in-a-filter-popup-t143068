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
Imports DevExpress.XtraGrid.Registrator
Imports System.Collections.Generic
Imports DevExpress.XtraGrid.Views.Base


Namespace DateRange
	<System.ComponentModel.DesignerCategory("")>
	Public Class MyGridControl
		Inherits GridControl

		Protected Overrides Sub RegisterAvailableViewsCore(ByVal collection As InfoCollection)
			MyBase.RegisterAvailableViewsCore(collection)
			collection.Add(New MyGridViewInfoRegistrator())
		End Sub
	End Class
	Public Class MyGridViewInfoRegistrator
		Inherits GridInfoRegistrator

		Public Overrides ReadOnly Property ViewName() As String
			Get
				Return "MyGridView"
			End Get
		End Property
		Public Overrides Function CreateView(ByVal grid As GridControl) As BaseView
			Return New MyGridView(TryCast(grid, GridControl))
		End Function
	End Class
End Namespace
