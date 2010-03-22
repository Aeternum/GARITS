using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace GARITS
{
    public partial class Administration : Form
    {
        private Database database;
        private int selected = 0;

        public Administration(Database db)
        {
            InitializeComponent();
            database = db;
            MySqlDataAdapter MyDA = new MySqlDataAdapter();
            string sqlDept = "SELECT users.userID as 'ID', users.username as 'Username', roles.rolename as 'Role', users.firstname as 'Firstname', users.surname as 'Surname', users.active as 'Active' FROM USERS, ROLES WHERE users.roleid = roles.roleid AND users.deleted <> 1 AND users.username <> 'root'";
            MyDA.SelectCommand = new MySqlCommand(sqlDept, db.sqlConn);

            DataTable table = new DataTable();
            MyDA.Fill(table);

            BindingSource bSource = new BindingSource();
            bSource.DataSource = table;

            //DataGridView dgView = new DataGridView();
            dataGridView1.DataSource = bSource;

            for (int i = 1; i < dataGridView1.Columns.Count; i++)
                dataGridView1.Columns[i].ReadOnly = true;
            selectionCombo.SelectedIndex = 0;

        }



        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            AddUser addUser = new AddUser(database, int.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()));
            addUser.ShowDialog();
        }

        private void newUserButton_Click(object sender, EventArgs e)
        {
            AddUser addUser = new AddUser(database);
            addUser.ShowDialog();
        }

        private void selectionCombo_Changed(object sender, EventArgs e)
        {
            if (selectionCombo.SelectedItem.ToString() == "This Page")
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                    row.Cells[0].Value = true;
                selectionCombo.SelectedIndex = 0;
            }
            else if (selectionCombo.SelectedItem.ToString() == "None")
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                    row.Cells[0].Value = false;
                selectionCombo.SelectedIndex = 0;
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                //MessageBox.Show("Start: " + dataGridView1.Rows[e.RowIndex].Cells[0].Value, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if((bool)cell.EditedFormattedValue)
                    selected++;
                else
                    selected--;
                
                selectedLabel.Text = "Selected: " + selected;
            }
        }
    }
}
