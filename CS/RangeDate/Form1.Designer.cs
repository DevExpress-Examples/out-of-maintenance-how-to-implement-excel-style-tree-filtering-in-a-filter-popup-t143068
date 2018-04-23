// Developer Express Code Central Example:
// How to implement excel style tree filtering in a filter popup.
// 
// This example demonstrates how to filter dates using a tree and select date
// ranges.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=T143068

// Developer Express Code Central Example:
// How to add custom filter items into the DateTime filter popup window
// 
// This example demonstrates how to add the capability to enter "greater than",
// "less than", and "between" directly from the calendar popup filter list.
// To
// provide this functionality, it is necessary to create a GridControl descendant
// with a custom MyGridView.
// First, override the MyGridView.CreateDateFilterPopup
// and MyGridView.RaiseFilterPopupDate methods.
// protected override DateFilterPopup
// CreateDateFilterPopup(GridColumn column, System.Windows.Forms.Control
// ownerControl, object creator)
// {  return new MyDateFilterPopup(this, column,
// ownerControl, creator);
// }
// 
// 
// 
// In the MyGridView.RaiseFilterPopupDate method add
// required FilterCriteria.    protected override void
// RaiseFilterPopupDate(DateFilterPopup filterPopup,
// List<DevExpress.XtraEditors.FilterDateElement> list)    {      CriteriaOperator
// filter = new BinaryOperator(FocusedColumn.FieldName, DateTime.Today,
// BinaryOperatorType.Greater);      list.Add(new
// FilterDateElement(Localizer.Active.GetLocalizedString(StringId.FilterClauseGreater)
// ,"", filter));      filter = new BinaryOperator(FocusedColumn.FieldName,
// DateTime.Today, BinaryOperatorType.Less);      list.Add(new
// FilterDateElement(Localizer.Active.GetLocalizedString(StringId.FilterClauseLess)
// ,"", filter));      filter = new BetweenOperator(FocusedColumn.FieldName,
// DateTime.Today, DateTime.Today);      list.Add(new
// FilterDateElement(Localizer.Active.GetLocalizedString(StringId.FilterClauseBetween)
// ,"", filter));      base.RaiseFilterPopupDate(filterPopup, list);
// }
// 
// 
// 
// 
// Now, the CreateDateFilterPopup method returns the MyDateFilterPopup
// object instead of the DateFilterPopup one. Override the
// Popup.CreateRepositoryItem method to create additional controls and add handlers
// for them:    protected override RepositoryItemPopupBase CreateRepositoryItem()
// {      item = base.CreateRepositoryItem() as RepositoryItemPopupContainerEdit;
// if (DateFilterControl.Controls.Count > 0)      {        DateCalendar1 =
// CreateCalendar(DateCalendar1, DateCalendar.SelectionStart, DateCalendar.Top,
// DateCalendar.Left);        DateCalendar1.Visible = false;        DateCalendar2 =
// CreateCalendar(DateCalendar2, DateCalendar.SelectionStart, DateCalendar1.Top,
// DateCalendar.Left + DateCalendar1.Width);        DateCalendar2.Visible = false;
// Greater =
// GetCheckEditByName(Localizer.Active.GetLocalizedString(StringId.FilterClauseGreater));
// Less =
// GetCheckEditByName(Localizer.Active.GetLocalizedString(StringId.FilterClauseLess));
// Between =
// GetCheckEditByName(Localizer.Active.GetLocalizedString(StringId.FilterClauseBetween));
// Greater.CheckedChanged += CheckedChanged;        Less.CheckedChanged +=
// CheckedChanged;        Between.CheckedChanged += CheckedChanged;        foreach
// (Control ctrl in DateFilterControl.Controls)        {          if (ctrl is
// CheckEdit)            if (NotOurControl(ctrl as CheckEdit))              (ctrl
// as CheckEdit).CheckedChanged += OriginalDateFilterPopup_CheckedChanged;        }
// }      return item;    }
// 
// 
// 
// When a custom item's check state changes,
// update the popup window layout to display the calendar at a required place.
// void CheckedChanged(object sender, EventArgs e) {      if((sender as
// CheckEdit).Checked) {        UpdateOurControlCheckedState((sender as
// CheckEdit).Text);        CalcControlsLocation((sender as CheckEdit).Text);
// this.View.ActiveFilterCriteria = GetFilterCriteriaByControlState();      }
// else {        if(DateCalendar1.Visible || DateCalendar2.Visible) {
// ReturnOriginalView();          ReturnOriginalControlsLocation();        }      }
// }
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E4265

namespace DateRange
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.myGridControl1 = new DateRange.MyGridControl();
            this.myGridView1 = new DateRange.MyGridView();
            ((System.ComponentModel.ISupportInitialize)(this.myGridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // myGridControl1
            // 
            this.myGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myGridControl1.Location = new System.Drawing.Point(0, 0);
            this.myGridControl1.LookAndFeel.SkinName = "Office 2010 Silver";
            this.myGridControl1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.myGridControl1.MainView = this.myGridView1;
            this.myGridControl1.Name = "myGridControl1";
            this.myGridControl1.Size = new System.Drawing.Size(608, 487);
            this.myGridControl1.TabIndex = 0;
            this.myGridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.myGridView1});
            // 
            // myGridView1
            // 
            this.myGridView1.GridControl = this.myGridControl1;
            this.myGridView1.Name = "myGridView1";
            this.myGridView1.OptionsView.HeaderFilterButtonShowMode = DevExpress.XtraEditors.Controls.FilterButtonShowMode.Button;
            this.myGridView1.treeListSource = null;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 487);
            this.Controls.Add(this.myGridControl1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GridView Custom Column Filter ";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.myGridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MyGridControl myGridControl1;
        private MyGridView myGridView1;


    }
}

