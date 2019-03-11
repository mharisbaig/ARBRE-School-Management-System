using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace School1
{
    public partial class frmRegister : Form
    {
        public frmRegister()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.RegistryKey key;
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Arbre");
            key.SetValue("ProductKey",txtRegister.Text.Trim());


            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\Arbre");
            DateTime admonth = DateTime.Now.AddMonths(3);
            //key.GetValue("RegisterTime");
                        
                        

            key.SetValue("RegisterTime",admonth.ToShortDateString());

            //string pNumber = obj.ToString();
            key.Close();

            MessageBox.Show("Thank you! for register");

            //Application.Exit();

            frmLogin frm = new frmLogin();
            frm.Show();

            this.Hide();

            //Application.Run(new frmMain());

        }
    }
}
