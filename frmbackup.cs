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
using System.IO;

namespace School1
{
    public partial class frmbackup : Form
    {
        public frmbackup()
        {
            InitializeComponent();
        }

        private void btnbackup_Click(object sender, EventArgs e)
        {
            DialogResult dlg = MessageBox.Show("Are you sure you want to create backup of database", "CREATE BACKUP", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg == DialogResult.Yes)
            {
                string path = Application.StartupPath + "\\Backup";
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    string School = path + "\\" + DateTime.Today.ToString("dd-MMM-yyyy") + " " + DateTime.Now.ToString("hh:mm:ss").Replace(':','_')  + ".bak";

                    if (File.Exists(School))
                    {
                        File.Delete(School);
                    }
                    SqlConnection cn = new SqlConnection(Program.myConnection);
                    string str = "BACKUP DATABASE School to disk='" + School + "'";
                    SqlCommand cmd = new SqlCommand(str, cn);

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    //Startup.ExeQuery("BACKUP DATABASE Ant to disk='" + fileName + "'");
                    MessageBox.Show(this, "Database backeup has been created sucessfully at following location\r\n" + path + "\r\n\r\nWith Backup File Name\r\n" + DateTime.Today.ToString("dd-MMM-yy") + ".bak", "Backup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Following error occured in backup process\r\nDescription : " + ex.Message, "School Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                this.Cursor = Cursors.Default;
            }
        }
    }
}
