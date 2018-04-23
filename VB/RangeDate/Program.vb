' Developer Express Code Central Example:
' How to implement excel style tree filtering in a filter popup.
' 
' This example demonstrates how to filter dates using a tree and select date
' ranges.
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=T143068

' Developer Express Code Central Example:
' How to add custom filter items into the DateTime filter popup window
' 
' This example demonstrates how to add the capability to enter "greater than",
' "less than", and "between" directly from the calendar popup filter list.
' To
' provide this functionality, it is necessary to create a GridControl descendant
' with a custom MyGridView.
' First, override the MyGridView.CreateDateFilterPopup
' and MyGridView.RaiseFilterPopupDate methods.
' protected override DateFilterPopup
' CreateDateFilterPopup(GridColumn column, System.Windows.Forms.Control
' ownerControl, object creator)
' {  return new MyDateFilterPopup(this, column,
' ownerControl, creator);
' }
' 
' 
' 
' In the MyGridView.RaiseFilterPopupDate method add
' required FilterCriteria.    protected override void
' RaiseFilterPopupDate(DateFilterPopup filterPopup,
' List<DevExpress.XtraEditors.FilterDateElement> list)    {      CriteriaOperator
' filter = new BinaryOperator(FocusedColumn.FieldName, DateTime.Today,
' BinaryOperatorType.Greater);      list.Add(new
' FilterDateElement(Localizer.Active.GetLocalizedString(StringId.FilterClauseGreater)
' ,"", filter));      filter = new BinaryOperator(FocusedColumn.FieldName,
' DateTime.Today, BinaryOperatorType.Less);      list.Add(new
' FilterDateElement(Localizer.Active.GetLocalizedString(StringId.FilterClauseLess)
' ,"", filter));      filter = new BetweenOperator(FocusedColumn.FieldName,
' DateTime.Today, DateTime.Today);      list.Add(new
' FilterDateElement(Localizer.Active.GetLocalizedString(StringId.FilterClauseBetween)
' ,"", filter));      base.RaiseFilterPopupDate(filterPopup, list);
' }
' 
' 
' 
' 
' Now, the CreateDateFilterPopup method returns the MyDateFilterPopup
' object instead of the DateFilterPopup one. Override the
' Popup.CreateRepositoryItem method to create additional controls and add handlers
' for them:    protected override RepositoryItemPopupBase CreateRepositoryItem()
' {      item = base.CreateRepositoryItem() as RepositoryItemPopupContainerEdit;
' if (DateFilterControl.Controls.Count > 0)      {        DateCalendar1 =
' CreateCalendar(DateCalendar1, DateCalendar.SelectionStart, DateCalendar.Top,
' DateCalendar.Left);        DateCalendar1.Visible = false;        DateCalendar2 =
' CreateCalendar(DateCalendar2, DateCalendar.SelectionStart, DateCalendar1.Top,
' DateCalendar.Left + DateCalendar1.Width);        DateCalendar2.Visible = false;
' Greater =
' GetCheckEditByName(Localizer.Active.GetLocalizedString(StringId.FilterClauseGreater));
' Less =
' GetCheckEditByName(Localizer.Active.GetLocalizedString(StringId.FilterClauseLess));
' Between =
' GetCheckEditByName(Localizer.Active.GetLocalizedString(StringId.FilterClauseBetween));
' Greater.CheckedChanged += CheckedChanged;        Less.CheckedChanged +=
' CheckedChanged;        Between.CheckedChanged += CheckedChanged;        foreach
' (Control ctrl in DateFilterControl.Controls)        {          if (ctrl is
' CheckEdit)            if (NotOurControl(ctrl as CheckEdit))              (ctrl
' as CheckEdit).CheckedChanged += OriginalDateFilterPopup_CheckedChanged;        }
' }      return item;    }
' 
' 
' 
' When a custom item's check state changes,
' update the popup window layout to display the calendar at a required place.
' void CheckedChanged(object sender, EventArgs e) {      if((sender as
' CheckEdit).Checked) {        UpdateOurControlCheckedState((sender as
' CheckEdit).Text);        CalcControlsLocation((sender as CheckEdit).Text);
' this.View.ActiveFilterCriteria = GetFilterCriteriaByControlState();      }
' else {        if(DateCalendar1.Visible || DateCalendar2.Visible) {
' ReturnOriginalView();          ReturnOriginalControlsLocation();        }      }
' }
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=E4265

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Windows.Forms

Namespace DateRange
	Friend NotInheritable Class Program

		Private Sub New()
		End Sub

		<STAThread>
		Shared Sub Main()

			Application.EnableVisualStyles()
			Application.SetCompatibleTextRenderingDefault(False)
			Application.Run(New Form1())
		End Sub
	End Class
End Namespace
