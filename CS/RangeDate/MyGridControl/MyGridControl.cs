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
using DevExpress.XtraGrid.Registrator;
using System.Collections.Generic;
using DevExpress.XtraGrid.Views.Base;


namespace DateRange {
    [System.ComponentModel.DesignerCategory("")]
    public class MyGridControl : GridControl
    {
        protected override void RegisterAvailableViewsCore(InfoCollection collection)
        {
            base.RegisterAvailableViewsCore(collection);
            collection.Add(new MyGridViewInfoRegistrator());
        }
    }
    public class MyGridViewInfoRegistrator : GridInfoRegistrator
    {
        public override string ViewName { get { return "MyGridView"; } }
        public override BaseView CreateView(GridControl grid)
        {
            return new MyGridView(grid as GridControl);
        }
    }
}
