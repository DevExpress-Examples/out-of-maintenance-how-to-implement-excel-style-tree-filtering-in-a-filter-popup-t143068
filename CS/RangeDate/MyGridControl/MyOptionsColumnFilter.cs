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
using DevExpress.XtraGrid.Columns;
using System.Collections.Generic;
using DevExpress.Utils.Controls;

namespace DateRange {
    public class MyOptionsColumnFilter : OptionsColumnFilter {
        protected internal bool UseFilterPopupRangeDateMode = false;
        FilterPopupModeExtended filterPopupMode;
        public MyOptionsColumnFilter(GridColumn column)
            : base(column)
        {
        }
        public new FilterPopupModeExtended FilterPopupMode {
            get { return filterPopupMode; }
            set {
                if(FilterPopupMode == value) return;
                FilterPopupModeExtended prevValue = FilterPopupMode;
                filterPopupMode = value;
                OnChanged(new BaseOptionChangedEventArgs("FilterPopupMode", prevValue, FilterPopupMode));
            }
        }
    }
}
