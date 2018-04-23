// Developer Express Code Central Example:
// How to implement excel style tree filtering in a filter popup.
// 
// This example demonstrates how to filter dates using a tree and select date
// ranges.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=T143068

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Helpers;
using System.Collections.Generic;
using DevExpress.XtraEditors;
using DevExpress.Data.Filtering;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System.Globalization;
using DevExpress.Data.Filtering.Helpers;


namespace DateRange
{

    public class MyDateFilterPopup : DateFilterPopup
    {
        RepositoryItemPopupContainerEdit item;
        CheckEdit customCheck;

        public MyDateFilterPopup(ColumnView view, GridColumn column, Control ownerControl, object creator)
            : base(view, column, ownerControl, creator)
        {

        }
        Point treelistLocation { get; set; }
        TreeList treelist { get; set; }
        PopupOutlookDateFilterControl DateFilterControl
        {
            get;
            set;
        }
        private void CalcControlsLocation()
        {
            foreach (Control ctrl in DateFilterControl.Controls)
            {
                CheckEdit ce = (ctrl as CheckEdit);
                if (ce != null && ce.Text == View.customName)
                    treelistLocation = new Point(ctrl.Location.X, ctrl.Location.Y + 30);
            }
        }
        #region Creators
        
       
        protected override RepositoryItemPopupBase CreateRepositoryItem()
        {

            item = base.CreateRepositoryItem() as RepositoryItemPopupContainerEdit;
            DateFilterControl = item.PopupControl.Controls.OfType<PopupOutlookDateFilterControl>().First();
            customCheck = GetCheckEdit();
            customCheck.Visible = false;
            if (View.treeListSource != null && DateFilterControl.Controls.Count > 0)
            {
                CreateTreeList(); 
                DateFilterControl.Controls.Add(treelist);
                foreach (Control ctrl in DateFilterControl.Controls)
                {
                    CheckEdit ce = (ctrl as CheckEdit);
                    if (ce != null)
                    {
                        if (ce.Text != View.customName)
                            ce.CheckedChanged += OriginalDateFilterPopup_CheckedChanged;
                    }
                    else
                    {
                        DateControlEx dateControlEx = (ctrl as DateControlEx);
                        if (dateControlEx != null)
                        {
                            dateControlEx.Click += dateControlEx_Click;
                        }
                    }
                }
                item.PopupFormMinSize = new Size(440, 280 + treelist.Height);
            }
            return item;
        }
        private void dateControlEx_Click(object sender, EventArgs e)
        {
            CreateDataSourceTreeList();
        }
        public CheckEdit GetCheckEdit()
        {
            foreach (Control ctrl in DateFilterControl.Controls)
            {
                if (ctrl.Text == View.customName)
                    return ctrl as CheckEdit;
            }
            return null;
        }
        private void CreateDataSourceTreeList()
        {
            treelist.BeginUnboundLoad();
            treelist.ClearNodes();
            TreeListNode parentForRootNodes = null;
            TreeListNode rootNode = null,
                childRootNode = null,
                childChildRootNode = null;
            var filterRowArray = (View as MyGridView).GetFilteredValues(this.Column, false, null);
            var distinctYearsArray = this.View.treeListSource.OfType<DateTime>().Select((d) => d.Year).Distinct().ToArray();

            foreach (int currentYear in distinctYearsArray)
            {
                rootNode = treelist.AppendNode(new object[] { currentYear }, parentForRootNodes);
                var distinctMonthArray = this.View.treeListSource.OfType<DateTime>().Where((dt) => dt.Year == currentYear).Select((dt) => dt.ToString("MMMM")).Distinct().ToArray();
                foreach (string currentMonth in distinctMonthArray)
                {
                    childRootNode = treelist.AppendNode(new object[] { currentMonth }, rootNode);
                    var distinctDayArray = this.View.treeListSource.OfType<DateTime>().Where((dt) => dt.Year == currentYear && dt.ToString("MMMM") == currentMonth).Select((dt) => dt.Day).Distinct().ToArray();
                    foreach (int currentDay in distinctDayArray)
                    {
                        childChildRootNode = treelist.AppendNode(new object[] { currentDay }, childRootNode);
                        if (filterRowArray != null)
                        {
                            var currentFilter = filterRowArray.OfType<DateTime>().Where((dt) => (dt.Year == currentYear) && (dt.ToString("MMMM") == currentMonth) && (dt.Day == currentDay)).ToArray();
                            if (currentFilter.ToList().Count > 0)
                                childChildRootNode.Checked = true;
                        }
                    }
                }
                SetCheckNode(rootNode);
            }
            treelist.EndUnboundLoad();
        }
        
        private void SetCheckNode(TreeListNode node)
        {
            bool childCheck;
            bool check = true;
            treelist.NodesIterator.DoLocalOperation((childNode) => 
            {
                childCheck = true;
                if (childNode.Level == 1)
                {
                    treelist.NodesIterator.DoLocalOperation((childChildNode) =>
                       {
                           if (!childChildNode.Checked)
                           {
                               childCheck = false;
                               check = false;
                           }
                       }, childNode.Nodes);
                    childNode.Checked = childCheck;
                }
            }, node.Nodes);
            node.Checked = check;
        }
        private void CreateTreeList()
        {
            CalcControlsLocation();
            treelist = new TreeList();

            treelist.Location = treelistLocation;

            treelist.BeginUpdate();
            treelist.Columns.Add();
            treelist.Columns[0].Caption = "Date";
            treelist.Columns[0].VisibleIndex = 0;
            treelist.OptionsView.ShowCheckBoxes = true;

            treelist.AfterCheckNode += treelist_AfterCheckNode;
            treelist.EndUpdate();
            CreateDataSourceTreeList();
        }
        private void CreateActiveFilterCriteria(NodeEventArgs e)
        {
            List<CriteriaOperator> listCriteriaOperator = new List<CriteriaOperator>();
            treelist.NodesIterator.DoLocalOperation((node) => { 
                node.CheckState = e.Node.CheckState; 
            }
            , e.Node.Nodes);
            treelist.NodesIterator.DoOperation((node) =>
            {
                if (node.Level == 0)
                { 
                   SetCheckNode(node);
                    if (node.Checked)
                        listCriteriaOperator.Add(GetFilterCriteriaByControlState(node));
                    else
                    {
                        treelist.NodesIterator.DoLocalOperation((childNode) =>
                        {
                            AddCriteria(childNode, listCriteriaOperator);
                        }, node.Nodes);
                    }
                }
            });       
            this.View.ActiveFilterCriteria = GroupOperator.Or(listCriteriaOperator);
        }
        private void AddCriteria(TreeListNode childNode, List<CriteriaOperator> listCriteriaOperator)
        {
            if (childNode.Level == 1)
            {
                if (childNode.Checked)
                    listCriteriaOperator.Add(GetFilterCriteriaByControlState(childNode));
                else
                    treelist.NodesIterator.DoLocalOperation((childChildNode) =>
                    {
                        if (childChildNode.Checked)
                            listCriteriaOperator.Add(GetFilterCriteriaByControlState(childChildNode));
                    }, childNode.Nodes);
            }
        }
        void treelist_AfterCheckNode(object sender, NodeEventArgs e)
        {
            foreach (Control ctrl in DateFilterControl.Controls)
            {
                if (ctrl.Text == View.customName)
                {
                    CheckEdit checkEdit = (CheckEdit)ctrl;
                    checkEdit.CheckState = CheckState.Checked;
                }
            }
            CreateActiveFilterCriteria(e);
        }

        #endregion

        private CriteriaOperator GetFilterCriteriaByControlState(TreeListNode node)
        {
            return GetBetweenOperatorByName(node);
        }

        protected new MyGridView View
        {
            get { return base.View as MyGridView; }
        }

        void OriginalDateFilterPopup_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit checkEdit = (CheckEdit)sender;
            if (checkEdit.Checked && checkEdit.Text != "Filter by a specific date:")
                CreateDataSourceTreeList();
        }
        protected virtual BetweenOperator GetBetweenOperatorByName(TreeListNode node)
        {
            if (node.Level == 0)
            {
                DateTime firstDay = new DateTime((int)node.GetValue(treelist.Columns[0]), 01, 01);
                DateTime endDay = new DateTime((int)node.GetValue(treelist.Columns[0]), 12, 31);
                return new BetweenOperator(this.Column.FieldName, firstDay, endDay);
            }
            else if (node.Level == 1)
            {
                DateTime firstDay = new DateTime((int)node.ParentNode.GetValue(treelist.Columns[0]), GetMonth(node), 01);
                DateTime endDay = new DateTime((int)node.ParentNode.GetValue(treelist.Columns[0]), GetMonth(node), DateTime.DaysInMonth((int)node.ParentNode.GetValue(treelist.Columns[0]), GetMonth(node)));
                return new BetweenOperator(this.Column.FieldName, firstDay, endDay);
            }
            else
            {
                DateTime firstDay = new DateTime((int)node.ParentNode.ParentNode.GetValue(treelist.Columns[0]), GetMonth(node.ParentNode), (int)node.GetValue(treelist.Columns[0]));
                DateTime endDay = new DateTime((int)node.ParentNode.ParentNode.GetValue(treelist.Columns[0]), GetMonth(node.ParentNode), (int)node.GetValue(treelist.Columns[0]));
                return new BetweenOperator(this.Column.FieldName, firstDay, endDay);
            }
        }
        private int GetMonth(TreeListNode node)
        {
            DateTime dt = DateTime.ParseExact((string)node.GetValue(treelist.Columns[0]), "MMMM", CultureInfo.InvariantCulture);
            return dt.Month;
        }
    }
}