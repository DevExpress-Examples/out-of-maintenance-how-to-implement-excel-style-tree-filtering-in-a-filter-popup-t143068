// Developer Express Code Central Example:
// How to implement excel style tree filtering in a filter popup.
// 
// This example demonstrates how to filter dates using a tree and select date
// ranges.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=T143068


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;


namespace DateRange
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Range Date", typeof(DateTime));
            dataTable.Columns.Add("Event", typeof(string));
            Random r = new Random();
            for (int i = 0; i < 50; i++)
            {
                dataTable.Rows.Add(new object[] { DateTime.Today.AddDays(r.Next(1000)), "test" });
            }
            myGridControl1.DataSource = dataTable;
            myGridView1.OptionsFilter.ColumnFilterPopupMode = DevExpress.XtraGrid.Columns.ColumnFilterPopupMode.Classic;
        }

    }
}
