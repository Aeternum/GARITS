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
    public partial class GARITS : Form
    {
        private Utils utils;
        private bool loggedIn;
        private Permissions permissions;
        private string currentUser;
        private int currentRoleID = -1;
        private string currentRole;
        private Database db;

        public GARITS()
        {
            InitializeComponent();
            permissions = new Permissions();
            try
            {
                utils = new Utils();
                db = new Database(utils.getDBConnectionString());
                db.Connect();
                permissions.readPermissions(db);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
            permissions.AddComponent(administrationToolStripMenuItem, "Administration Menu Item");
            

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loginToolStripMenuItem.Visible = true;
            logoutToolStripMenuItem.Visible = false;
            toolStripStatusLabel1.Text = "Login to begin";
            toolStripStatusLabel2.Text = "";
            loggedIn = false;
            currentUser = "";
            currentRoleID = -1;
            currentRole = "";
            permissions.disableAll();
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog();
            try
            {
                Console.WriteLine(utils.getDBConnectionString());
                
                loggedIn = db.VerifyUser(loginForm.getUsername(), loginForm.getPassword());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK ,MessageBoxIcon.Error);
                return;
            }
            if (!loggedIn)
                MessageBox.Show("Authentication failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                currentUser = loginForm.getUsername();
                int[] roleid = db.getCurrentRoleID(currentUser);
                string[] role = db.getCurrentRole(currentUser);
                if ((roleid[0] != 1) || (role[0] != "Found")) // If not found
                {
                    MessageBox.Show("Critial Database Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(1);
                }
                currentRoleID = roleid[1];
                currentRole = role[1];
                toolStripStatusLabel1.Text = "Logged in as " + currentUser;
                toolStripStatusLabel2.Text = currentRole;
                //Here we will have to start enabling controls around the forum depending on what user has access to
                permissions.updateComponents(currentRoleID);
                logoutToolStripMenuItem.Visible = true;
                loginToolStripMenuItem.Visible = false;
            }
            
            loginForm.Dispose();
        }

        private void administrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Administration adminForm = new Administration();
            adminForm.ShowDialog();
            JobEdit jedit = new JobEdit();
            jedit.ShowDialog();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {/*
            //obj.GetType() == typeof(MyClass)
            Object test = permissions.getStuff();
            //if (permissions.getStuff().GetType() is System.Windows.Forms.ToolStripMenuItem)
            if (permissions.getStuff().GetType() == typeof(System.Windows.Forms.ToolStripMenuItem))
            ((ToolStripItem)permissions.getStuff()).Enabled = true;
            else
                MessageBox.Show("Nope its " + test.GetType() );*/
        }

        private void GARITS_Load(object sender, EventArgs e)
        {

        }

        private void Jobs_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //Database db2 = new Database("Server = localhost; Database = garits; Uid = root;");
            //db.Connect();
            // Connect to the database
            //DbWrapper myWrapper = new DbWrapper();
            //myWrapper.Connect();


            MySqlDataAdapter MyDA = new MySqlDataAdapter();
            string sqlDept = "SELECT jobid as 'ID', jobTYPE, registration, jobid, jobTYPE, registration FROM JOBS;";
            MyDA.SelectCommand = new MySqlCommand(sqlDept, db.sqlConn);

            DataTable table = new DataTable();
            MyDA.Fill(table);
            
            BindingSource bSource = new BindingSource();
            bSource.DataSource = table;

            //DataGridView dgView = new DataGridView();
            dataGridView1.DataSource = bSource;

            db.Disconnect();

        }

        private void button50_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

    } 
}
