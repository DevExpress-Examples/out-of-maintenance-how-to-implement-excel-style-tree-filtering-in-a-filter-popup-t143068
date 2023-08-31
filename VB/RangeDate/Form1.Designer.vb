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

Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid

Namespace DateRange
    Partial Public Class Form1
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.IContainer = Nothing

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso (components IsNot Nothing) Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

#Region "Windows Form Designer generated code"
        Private Sub InitializeComponent()
            Me.myGridControl1 = New GridControl()
            Me.myGridView1 = New GridView()
            DirectCast(Me.myGridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.myGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' myGridControl1
            ' 
            Me.myGridControl1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.myGridControl1.Location = New System.Drawing.Point(0, 0)
            Me.myGridControl1.LookAndFeel.SkinName = "Office 2010 Silver"
            Me.myGridControl1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat
            Me.myGridControl1.MainView = Me.myGridView1
            Me.myGridControl1.Name = "myGridControl1"
            Me.myGridControl1.Size = New System.Drawing.Size(608, 487)
            Me.myGridControl1.TabIndex = 0
            Me.myGridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.myGridView1})
            ' 
            ' myGridView1
            ' 
            Me.myGridView1.GridControl = Me.myGridControl1
            Me.myGridView1.Name = "myGridView1"
            Me.myGridView1.OptionsView.HeaderFilterButtonShowMode = DevExpress.XtraEditors.Controls.FilterButtonShowMode.Button

            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(608, 487)
            Me.Controls.Add(Me.myGridControl1)
            Me.Name = "Form1"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "GridView Custom Column Filter "
            '			Me.Load += New System.EventHandler(Me.Form1_Load)
            DirectCast(Me.myGridControl1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.myGridView1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private myGridControl1 As GridControl
        Private myGridView1 As GridView


    End Class
End Namespace

