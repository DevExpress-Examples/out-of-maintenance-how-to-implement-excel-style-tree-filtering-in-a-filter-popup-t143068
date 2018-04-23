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
Imports DevExpress.XtraGrid.Columns
Imports System.Collections.Generic
Imports DevExpress.Utils.Controls

Namespace DateRange
	Public Class MyOptionsColumnFilter
		Inherits OptionsColumnFilter

		Protected Friend UseFilterPopupRangeDateMode As Boolean = False
'INSTANT VB NOTE: The variable filterPopupMode was renamed since Visual Basic does not allow variables and other class members to have the same name:
		Private filterPopupMode_Renamed As FilterPopupModeExtended

		Public Shadows Property FilterPopupMode() As FilterPopupModeExtended
			Get
				Return filterPopupMode_Renamed
			End Get
			Set(ByVal value As FilterPopupModeExtended)
				If FilterPopupMode = value Then
					Return
				End If
				Dim prevValue As FilterPopupModeExtended = FilterPopupMode
				filterPopupMode_Renamed = value
				OnChanged(New BaseOptionChangedEventArgs("FilterPopupMode", prevValue, FilterPopupMode))
			End Set
		End Property
	End Class
End Namespace
