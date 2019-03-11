using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace School1
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string qry = "select userid from userlogin where username='" + txtUsername.Text.Trim() + "' and password='" + txtPassword.Text.Trim() +"'";
            SqlCommand cmd = new SqlCommand(qry, cn);

            try
            {
                cn.Open();
                object id = cmd.ExecuteScalar();
                int userid = Convert.ToInt32(id);

                if(userid <= 0)
                {
                    MessageBox.Show("Invalid user");
                }
                else
                {
                    frmMain frm = new frmMain();
                    frm.Show();
                    this.Hide();

                    //MessageBox.Show("Login Successfully");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cn.Close();
                cn.Dispose();
                cmd.Dispose();
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
        }
    }
}
