' Developer Express Code Central Example:
' How to implement excel style tree filtering in a filter popup.
' 
' This example demonstrates how to filter dates using a tree and select date
' ranges.
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=T143068

Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Linq
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors.Helpers
Imports System.Collections.Generic
Imports DevExpress.XtraEditors
Imports DevExpress.Data.Filtering
Imports DevExpress.XtraTreeList
Imports DevExpress.XtraTreeList.Nodes
Imports System.Globalization
Imports DevExpress.Data.Filtering.Helpers


Namespace DateRange

	Public Class MyDateFilterPopup
		Inherits DateFilterPopup

		Private item As RepositoryItemPopupContainerEdit
		Private customCheck As CheckEdit

		Public Sub New(ByVal view As ColumnView, ByVal column As GridColumn, ByVal ownerControl As Control, ByVal creator As Object)
			MyBase.New(view, column, ownerControl, creator)

		End Sub
		Private Property treelistLocation() As Point
		Private Property treelist() As TreeList
		Private Property DateFilterControl() As PopupOutlookDateFilterControl
		Private Sub CalcControlsLocation()
			For Each ctrl As Control In DateFilterControl.Controls
				Dim ce As CheckEdit = (TryCast(ctrl, CheckEdit))
				If ce IsNot Nothing AndAlso ce.Text = View.customName Then
					treelistLocation = New Point(ctrl.Location.X, ctrl.Location.Y + 30)
				End If
			Next ctrl
		End Sub
		#Region "Creators"


		Protected Overrides Function CreateRepositoryItem() As RepositoryItemPopupBase

			item = TryCast(MyBase.CreateRepositoryItem(), RepositoryItemPopupContainerEdit)
			DateFilterControl = item.PopupControl.Controls.OfType(Of PopupOutlookDateFilterControl)().First()
			customCheck = GetCheckEdit()
			customCheck.Visible = False
			If View.treeListSource IsNot Nothing AndAlso DateFilterControl.Controls.Count > 0 Then
				CreateTreeList()
				DateFilterControl.Controls.Add(treelist)
				For Each ctrl As Control In DateFilterControl.Controls
					Dim ce As CheckEdit = (TryCast(ctrl, CheckEdit))
					If ce IsNot Nothing Then
						If ce.Text <> View.customName Then
							AddHandler ce.CheckedChanged, AddressOf OriginalDateFilterPopup_CheckedChanged
						End If
					Else
						Dim dateControlEx As DateControlEx = (TryCast(ctrl, DateControlEx))
						If dateControlEx IsNot Nothing Then
							AddHandler dateControlEx.Click, AddressOf dateControlEx_Click
						End If
					End If
				Next ctrl
				item.PopupFormMinSize = New Size(440, 280 + treelist.Height)
			End If
			Return item
		End Function
		Private Sub dateControlEx_Click(ByVal sender As Object, ByVal e As EventArgs)
			CreateDataSourceTreeList()
		End Sub
		Public Function GetCheckEdit() As CheckEdit
			For Each ctrl As Control In DateFilterControl.Controls
				If ctrl.Text = View.customName Then
					Return TryCast(ctrl, CheckEdit)
				End If
			Next ctrl
			Return Nothing
		End Function
		Private Sub CreateDataSourceTreeList()
			treelist.BeginUnboundLoad()
			treelist.ClearNodes()
			Dim parentForRootNodes As TreeListNode = Nothing
			Dim rootNode As TreeListNode = Nothing, childRootNode As TreeListNode = Nothing, childChildRootNode As TreeListNode = Nothing
			Dim filterRowArray = (TryCast(View, MyGridView)).GetFilteredValues(Me.Column, False, Nothing)
			Dim distinctYearsArray = Me.View.treeListSource.OfType(Of Date)().Select(Function(d) d.Year).Distinct().ToArray()

			For Each currentYear As Integer In distinctYearsArray
				rootNode = treelist.AppendNode(New Object() { currentYear }, parentForRootNodes)
				Dim distinctMonthArray = Me.View.treeListSource.OfType(Of Date)().Where(Function(dt) dt.Year = currentYear).Select(Function(dt) dt.ToString("MMMM")).Distinct().ToArray()
				For Each currentMonth As String In distinctMonthArray
					childRootNode = treelist.AppendNode(New Object() { currentMonth }, rootNode)
					Dim distinctDayArray = Me.View.treeListSource.OfType(Of Date)().Where(Function(dt) dt.Year = currentYear AndAlso dt.ToString("MMMM") = currentMonth).Select(Function(dt) dt.Day).Distinct().ToArray()
					For Each currentDay As Integer In distinctDayArray
						childChildRootNode = treelist.AppendNode(New Object() { currentDay }, childRootNode)
						If filterRowArray IsNot Nothing Then
							Dim currentFilter = filterRowArray.OfType(Of Date)().Where(Function(dt) (dt.Year = currentYear) AndAlso (dt.ToString("MMMM") = currentMonth) AndAlso (dt.Day = currentDay)).ToArray()
							If currentFilter.ToList().Count > 0 Then
								childChildRootNode.Checked = True
							End If
						End If
					Next currentDay
				Next currentMonth
				SetCheckNode(rootNode)
			Next currentYear
			treelist.EndUnboundLoad()
		End Sub

		Private Sub SetCheckNode(ByVal node As TreeListNode)
			Dim childCheck As Boolean
			Dim check As Boolean = True
			treelist.NodesIterator.DoLocalOperation(Sub(childNode)
				childCheck = True
				If childNode.Level = 1 Then
					treelist.NodesIterator.DoLocalOperation(Sub(childChildNode)
						If Not childChildNode.Checked Then
							childCheck = False
							check = False
						End If
					End Sub, childNode.Nodes)
					childNode.Checked = childCheck
				End If
			End Sub, node.Nodes)
			node.Checked = check
		End Sub
		Private Sub CreateTreeList()
			CalcControlsLocation()
			treelist = New TreeList()

			treelist.Location = treelistLocation

			treelist.BeginUpdate()
			treelist.Columns.Add()
			treelist.Columns(0).Caption = "Date"
			treelist.Columns(0).VisibleIndex = 0
			treelist.OptionsView.ShowCheckBoxes = True

			AddHandler treelist.AfterCheckNode, AddressOf treelist_AfterCheckNode
			treelist.EndUpdate()
			CreateDataSourceTreeList()
		End Sub
		Private Sub CreateActiveFilterCriteria(ByVal e As NodeEventArgs)
			Dim listCriteriaOperator As New List(Of CriteriaOperator)()
			treelist.NodesIterator.DoLocalOperation(Sub(node) node.CheckState = e.Node.CheckState, e.Node.Nodes)
			treelist.NodesIterator.DoOperation(Sub(node)
				If node.Level = 0 Then
					SetCheckNode(node)
					If node.Checked Then
						listCriteriaOperator.Add(GetFilterCriteriaByControlState(node))
					Else
						treelist.NodesIterator.DoLocalOperation(Sub(childNode) AddCriteria(childNode, listCriteriaOperator), node.Nodes)
					End If
				End If
			End Sub)
			Me.View.ActiveFilterCriteria = GroupOperator.Or(listCriteriaOperator)
		End Sub
		Private Sub AddCriteria(ByVal childNode As TreeListNode, ByVal listCriteriaOperator As List(Of CriteriaOperator))
			If childNode.Level = 1 Then
				If childNode.Checked Then
					listCriteriaOperator.Add(GetFilterCriteriaByControlState(childNode))
				Else
					treelist.NodesIterator.DoLocalOperation(Sub(childChildNode)
						If childChildNode.Checked Then
							listCriteriaOperator.Add(GetFilterCriteriaByControlState(childChildNode))
						End If
					End Sub, childNode.Nodes)
				End If
			End If
		End Sub
		Private Sub treelist_AfterCheckNode(ByVal sender As Object, ByVal e As NodeEventArgs)
			For Each ctrl As Control In DateFilterControl.Controls
				If ctrl.Text = View.customName Then
					Dim checkEdit As CheckEdit = CType(ctrl, CheckEdit)
					checkEdit.CheckState = CheckState.Checked
				End If
			Next ctrl
			CreateActiveFilterCriteria(e)
		End Sub

		#End Region

		Private Function GetFilterCriteriaByControlState(ByVal node As TreeListNode) As CriteriaOperator
			Return GetBetweenOperatorByName(node)
		End Function

		Protected Shadows ReadOnly Property View() As MyGridView
			Get
				Return TryCast(MyBase.View, MyGridView)
			End Get
		End Property

		Private Sub OriginalDateFilterPopup_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
			Dim checkEdit As CheckEdit = DirectCast(sender, CheckEdit)
			If checkEdit.Checked AndAlso checkEdit.Text <> "Filter by a specific date:" Then
				CreateDataSourceTreeList()
			End If
		End Sub
		Protected Overridable Function GetBetweenOperatorByName(ByVal node As TreeListNode) As BetweenOperator
			If node.Level = 0 Then
				Dim firstDay As New Date(CInt(Math.Truncate(node.GetValue(treelist.Columns(0)))), 01, 01)
				Dim endDay As New Date(CInt(Math.Truncate(node.GetValue(treelist.Columns(0)))), 12, 31)
				Return New BetweenOperator(Me.Column.FieldName, firstDay, endDay)
			ElseIf node.Level = 1 Then
				Dim firstDay As New Date(CInt(Math.Truncate(node.ParentNode.GetValue(treelist.Columns(0)))), GetMonth(node), 01)
				Dim endDay As New Date(CInt(Math.Truncate(node.ParentNode.GetValue(treelist.Columns(0)))), GetMonth(node), Date.DaysInMonth(CInt(Math.Truncate(node.ParentNode.GetValue(treelist.Columns(0)))), GetMonth(node)))
				Return New BetweenOperator(Me.Column.FieldName, firstDay, endDay)
			Else
				Dim firstDay As New Date(CInt(Math.Truncate(node.ParentNode.ParentNode.GetValue(treelist.Columns(0)))), GetMonth(node.ParentNode), CInt(Math.Truncate(node.GetValue(treelist.Columns(0)))))
				Dim endDay As New Date(CInt(Math.Truncate(node.ParentNode.ParentNode.GetValue(treelist.Columns(0)))), GetMonth(node.ParentNode), CInt(Math.Truncate(node.GetValue(treelist.Columns(0)))))
				Return New BetweenOperator(Me.Column.FieldName, firstDay, endDay)
			End If
		End Function
		Private Function GetMonth(ByVal node As TreeListNode) As Integer
			Dim dt As Date = Date.ParseExact(CStr(node.GetValue(treelist.Columns(0))), "MMMM", CultureInfo.InvariantCulture)
			Return dt.Month
		End Function
	End Class
End Namespace