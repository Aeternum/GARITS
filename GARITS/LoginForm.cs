using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GARITS
{
    public partial class LoginForm : Form
    {
        private String username;
        private String password;
        private Boolean authenticated = false;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void login_Click(object sender, EventArgs e)
        {
            username = usernameTextBox.Text;
            password = passwordTextBox.Text;
            this.Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public string getUsername()
        {
            return username;
        }

        public string getPassword()
        {
            return password;
        }

        public Boolean isAuthenticated()
        {
            return authenticated;
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
