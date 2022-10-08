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

        bool[,] cell_states = null;

        int ARRAY_SIZE = 5;
        public formMain()
        {

            InitializeComponent();
        }

        private void formMain_Load(object sender, EventArgs e)
        {
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
            }

            DataGrid.Rows.Clear();
            DataGridViewImageColumn column = (DataGridViewImageColumn) DataGrid.Columns[0];
            for (int i = 0; i < grid_size; i++)
            {
                DataGridViewRow new_row = new DataGridViewRow();
                new_row.Height = column.Width;
                DataGrid.Rows.Add(new_row);
            }

            cell_states = new bool[grid_size, grid_size];
            for (int i = 0; i < grid_size; i++)
            {
                for (int j = 0; j < grid_size; j++)
                {
                    DataGridViewImageCell cell = (DataGridViewImageCell)DataGrid.Rows[j].Cells[i];
                    cell.ChangeImage(opened_state);
                    cell_states[i, j] = true;
                }
            }
            RandomizeLocks(grid_size / 2, grid_size);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Console.WriteLine(e.ColumnIndex.ToString() + ":" + e.RowIndex.ToString());
            ChangeLocks(e.ColumnIndex, e.RowIndex);
            Console.WriteLine(CheckWinCondition().ToString());
        }

        private void RandomizeLocks(int number_of_randoms, int max_random)
        {
            Random random = new Random();
            for (int i = 0; i < number_of_randoms; i++)
            {
                ChangeLocks(random.Next(0, max_random), random.Next(0, max_random));
            }
        }

        private void ChangeLocks(int column, int row)
        {
            for (int i = 0; i < DataGrid.ColumnCount; i++)
            {
                DataGridViewImageCell cell = (DataGridViewImageCell)DataGrid.Rows[row].Cells[i];
                SwitchCellImage(cell, row, i);
                if (i == row)
                {
                    continue;
                }
                cell = (DataGridViewImageCell)DataGrid.Rows[i].Cells[column];
                SwitchCellImage(cell, i, column);
            }
        }
        

        private bool CheckWinCondition()
        {
            bool win_condition = true;
            for (int i = 0; i < ARRAY_SIZE; i++)
            {
                for (int j = 0; j < ARRAY_SIZE; j++)
                {
                    win_condition &= cell_states[i, j];
                    if (!win_condition)
                    {
                        break;
                    }
                }
                if (!win_condition)
                {
                    break;
                }
            }
            return win_condition;
        }

        private void SwitchCellImage(DataGridViewImageCell cell, int cell_x, int cell_y)
        {
            bool cell_state = cell_states[cell_x, cell_y];
            if (cell_state)
            {
                cell.ChangeImage(closed_state);
            }
            else
            {
                cell.ChangeImage(opened_state);
            }
            cell_states[cell_x, cell_y] = !cell_state;
        }
    }
    
    static class ExtensionMethod
    {

        public static bool state_opened = true;

        public static void ChangeImage(this DataGridViewImageCell cell, Image img_to_set)
        {
            cell.Value = img_to_set;
        }
    }
}
