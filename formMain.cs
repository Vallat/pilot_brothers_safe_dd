using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DevGridContol
{
    public partial class formMain : Form
    {
        private readonly Image opened_state = Image.FromFile("resources/opened.png");
        private readonly Image closed_state = Image.FromFile("resources/closed.png");
        public formMain()
        {
            InitializeComponent();
        }

        private void formMain_Load(object sender, EventArgs e)
        {
            int ARRAY_SIZE = 30;

            InitializeDataGrid(ARRAY_SIZE);
        }

        private void InitializeDataGrid(int grid_size)
        {
            DataGrid.Columns.Clear();
            for (int i = 0; i < grid_size; i++)
            {
                DataGridViewImageColumn new_column = new DataGridViewImageColumn();
                new_column.ImageLayout = DataGridViewImageCellLayout.Stretch;
                DataGrid.Columns.Add(new_column);

                DataGridViewRow new_row = new DataGridViewRow();
                new_row.Height = new_column.Width;
                DataGrid.Rows.Add(new_row);
            }

            DataGridViewImageColumn column = (DataGridViewImageColumn) DataGrid.Columns[0];
            DataGrid.Rows.Clear();
            for (int i = 0; i < grid_size; i++)
            {
                DataGridViewRow new_row = new DataGridViewRow();
                new_row.Height = column.Width;
                DataGrid.Rows.Add(new_row);
            }

            for (int i = 0; i < grid_size; i++)
            {
                for (int j = 0; j < grid_size; j++)
                {
                    DataGridViewImageCell cell = (DataGridViewImageCell)DataGrid.Rows[j].Cells[i];
                    cell.ChangeImage(closed_state);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ChangeLocks(e.ColumnIndex, e.RowIndex);
        }

        private void ChangeLocks(int column, int row)
        {
            for (int i = 0; i < DataGrid.ColumnCount; i++)
            {
                DataGridViewImageCell cell = (DataGridViewImageCell) DataGrid.Rows[i].Cells[column];
                SwitchCellImage(cell);
                if(i == column)
                {
                    continue;
                }
                cell = (DataGridViewImageCell)DataGrid.Rows[row].Cells[i];
                SwitchCellImage(cell);
            }
        }
        
        private void SwitchCellImage(DataGridViewImageCell cell)
        {
            if (cell.Value == opened_state)
            {
                cell.ChangeImage(closed_state);
            }
            else
            {
                cell.ChangeImage(opened_state);
            }
        }
    }
    
    static class ExtensionMethod
    {
        public static void ChangeImage(this DataGridViewImageCell cell, Image img_to_set)
        {
            cell.Value = img_to_set;
        }
    }
}
