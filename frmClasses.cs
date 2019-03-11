using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace School1
{
    public partial class frmClasses : Form
    {
        public frmClasses()
        {
            InitializeComponent();
        }

        private void frmClasses_Load(object sender, EventArgs e)
        {
            fillGrid();
        }

        private void fillGrid()
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string qry = "select * from classes";
            SqlDataAdapter adpt = new SqlDataAdapter(qry, cn);

            DataTable dt = new DataTable();

            cn.Open();
            adpt.Fill(dt);

            dataGridView1.DataSource = dt;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

           private void btnupdate_Click(object sender, EventArgs e)
         {
             updateRecord();
             fillGrid();
         }

         private void updateRecord()
         {
             SqlConnection cn = new SqlConnection(Program.myConnection);
             string qry = " Update classes set classname=@classname where classid=" + dataGridView1.SelectedCells[0].Value.ToString();
             SqlCommand cmd = new SqlCommand(qry, cn);
             cmd.Parameters.AddWithValue("@classname",txtclassname.Text);
             try
             {
                 cn.Open();
                 cmd.ExecuteNonQuery();
                 MessageBox.Show("Record has been updated successfully", "Update Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
             }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.Message, "Update Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
             }
             finally
             {
                 cn.Close();
                 cn.Dispose();
                 cmd.Dispose();
             }
         }
    }
}
