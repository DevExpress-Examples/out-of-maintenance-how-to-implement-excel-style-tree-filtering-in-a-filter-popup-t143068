// Developer Express Code Central Example:
// How to implement excel style tree filtering in a filter popup.
// 
// This example demonstrates how to filter dates using a tree and select date
// ranges.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=T143068

using System;
using System.Linq;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.XtraEditors;


namespace DateRange
{
    [System.ComponentModel.DesignerCategory("")]
    public class MyGridView : GridView
    {
        public readonly string customName = "Get treelist";
        public MyGridView() : this(null) { }
        public MyGridView(GridControl grid)
            : base(grid)
        {
            DateFilterInfo = null;
        }
        internal ColumnFilterInfo DateFilterInfo;
        protected override GridColumnCollection CreateColumnCollection()
        {
            return new MyGridColumnCollection(this);
        }
        protected override DateFilterPopup CreateDateFilterPopup(GridColumn column, System.Windows.Forms.Control ownerControl, object creator)
        {

            return new MyDateFilterPopup(this, column, ownerControl, creator);
        }
        protected override void RaiseFilterPopupDate(DateFilterPopup filterPopup, List<FilterDateElement> list)
        {
            list.RemoveRange(0, list.Count);
            string filterString = DateFilterInfo != null ? DateFilterInfo.FilterString : "";
            CriteriaOperator filterCriteria = DateFilterInfo != null ? DateFilterInfo.FilterCriteria : null;
            list.Add(new FilterDateElement(customName, filterString, filterCriteria));
            base.RaiseFilterPopupDate(filterPopup, list);
        }
        public object[] treeListSource { get; set; }
        public object[] GetFilteredValues(GridColumn column, bool showAll, DevExpress.Data.OperationCompleted completed)
        {
            return base.GetFilterPopupValues(column, false, completed);
        }
        protected override object[] GetFilterPopupValues(GridColumn column, bool showAll, DevExpress.Data.OperationCompleted completed)
        {
            object[] filterPopupValues = base.GetFilterPopupValues(column, showAll, completed);
            if (((MyGridColumn)column).PIsDateFilterPopup)
                treeListSource = filterPopupValues;
            return filterPopupValues;
        }
    }
}