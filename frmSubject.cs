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
    public partial class frmSubject : Form
    {
        public frmSubject()
        {
            InitializeComponent();
        }

        int a = 1;

        private void btnSave_Click(object sender, EventArgs e)
        {
            insertRecord();
        }

        private void insertRecord()
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string sql = "insert into subject (classid,subjectname,STAFFID) values (@classid,@subjectname,@staffid)";

            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@classid", cmbClass.SelectedValue);
            cmd.Parameters.AddWithValue("@subjectname", cmbSubject.Text);
            cmd.Parameters.AddWithValue("@staffid", cmbTeacher.SelectedValue);
            
            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record has been inserted in database", "Insert Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Insert Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.Close();
                cn.Dispose();
                cmd.Dispose();
                fillGrid();
            }
        }

        private void fillClass()
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string qry = "select * from classes";
            SqlDataAdapter adpt = new SqlDataAdapter(qry, cn);

            DataTable dt = new DataTable();

            cn.Open();
            adpt.Fill(dt);

            //dataGridView1.DataSource = dt;

            cmbClass.DataSource = dt;
            cmbClass.DisplayMember = "classname";
            cmbClass.ValueMember = "classid";


        }

        private void fillTeacher()
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string qry = "select * from staff where designation='Teacher'";
            SqlDataAdapter adpt = new SqlDataAdapter(qry, cn);

            DataTable dt = new DataTable();

            cn.Open();
            adpt.Fill(dt);
            
            cmbTeacher.DataSource = dt;
            cmbTeacher.DisplayMember = "staffname";
            cmbTeacher.ValueMember = "staffid";
        }

        private void frmSubject_Load(object sender, EventArgs e)
        {
            fillClass();
            fillTeacher();
        }

        private void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
        {           

            if (a > 2)
            {
                fillGrid();
            }
            a = a + 1;
        }

        private void fillGrid()
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string qry = "select subject.subjectid, classes.classname, subject.subjectname,staff.staffname from subject " +
                "inner join classes on subject.classid = classes.classid " +
                "inner join staff on subject.staffid = staff.staffid " +
                "where subject.classid=" + cmbClass.SelectedValue;

            SqlDataAdapter adpt = new SqlDataAdapter(qry, cn);

            DataTable dt = new DataTable();

            cn.Open();
            adpt.Fill(dt);

            dataGridView1.DataSource = dt;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            updateRecord();
             fillGrid();
        }

        private void updateRecord()
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string sql = "update subject set classid=@classid,subjectname=@subjectname,STAFFID=@staffid where subjectid=" + dataGridView1.SelectedCells[0].Value.ToString();

            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@classid", cmbClass.SelectedValue);
            cmd.Parameters.AddWithValue("@subjectname", cmbSubject.Text);
            cmd.Parameters.AddWithValue("@staffid", cmbTeacher.SelectedValue);
         
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
