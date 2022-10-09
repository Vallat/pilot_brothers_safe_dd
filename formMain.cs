using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
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

        private bool[,] cell_states = null;

        private bool is_playing = false;

        private int array_size_ = 5;
        public int ARRAY_SIZE
        {
            get { return array_size_; }
            set {
                value = Math.Min(Math.Max(2, value), Int32.MaxValue);
                array_size_ = value;
                InitializeDataGrid(ARRAY_SIZE);
            }
        }
        
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
            is_playing = false;
            DataGrid.Columns.Clear();
            for (int i = 0; i < grid_size; i++)
            {
                DataGridViewImageColumn new_column = new DataGridViewImageColumn
                {
                    ImageLayout = DataGridViewImageCellLayout.Stretch
                };
                DataGrid.Columns.Add(new_column);
            }

            DataGrid.Rows.Clear();
            DataGridViewImageColumn column = (DataGridViewImageColumn) DataGrid.Columns[0];
            for (int i = 0; i < grid_size; i++)
            {
                DataGridViewRow new_row = new DataGridViewRow
                {
                    Height = column.Width
                };
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
            is_playing = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!is_playing)
            {
                return;
            }
            ChangeLocks(e.ColumnIndex, e.RowIndex);
            if (CheckWinCondition())
            {
                WinText.Visible = true;
                ArraySizeInput.Visible = false;
                N_label.Visible = false;
                is_playing = false;
            }
        }

        private void RandomizeLocks(int number_of_randoms, int max_random)
        {
            var exclude_rows = new HashSet<int> { };
            var exclude_columns = new HashSet<int> { };
            Random random = new Random();
            for (int element_num = 0; element_num < number_of_randoms; element_num++)
            {
                var rows_range = Enumerable.Range(0, max_random).Where(i => !exclude_rows.Contains(i));
                var column_range = Enumerable.Range(0, max_random).Where(i => !exclude_columns.Contains(i));

                int row_index = random.Next(0, max_random - exclude_rows.Count());
                int column_index = random.Next(0, max_random - exclude_columns.Count());

                int row_random_number = rows_range.ElementAt(row_index);
                int column_random_number = column_range.ElementAt(column_index);
                exclude_rows.Add(row_random_number);
                exclude_columns.Add(column_random_number);
                ChangeLocks(row_random_number, column_random_number);
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

        private void ArraySizeInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar != Convert.ToChar(Keys.Return))
            {
                return;
            }
            int new_array_size = Int32.Parse(ArraySizeInput.Text);
            ARRAY_SIZE = new_array_size;
            ArraySizeInput.Text = "";
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
