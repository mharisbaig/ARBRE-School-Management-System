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
    public partial class frmstaff : Form
    {
        public frmstaff()
        {
            InitializeComponent();
        }

        private void frmstaff_Load(object sender, EventArgs e)
        {
            fillGrid();
            clearBox();
        }
        private void clearBox()
        {
            txtStaffName.Text = "";
            txtmobileNo.Text = "";
            txtdesignation.Text = "";
            txtaddress.Text = "";
            dtp.Text = "";

        }
        private void fillGrid()
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string qry = "Select *from staff";
            SqlDataAdapter adpt = new SqlDataAdapter(qry, cn);

            DataTable dt = new DataTable();

            cn.Open();
            adpt.Fill(dt);

            dataGridView1.DataSource = dt;
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string sql = "insert into staff (staffname,mobile,designation,address,date) " +
                "values (@staffname,@mobile,@designation,@address,@date)";

            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@staffname", txtStaffName.Text.Trim());
            cmd.Parameters.AddWithValue("@mobile", txtmobileNo.Text.Trim());
            cmd.Parameters.AddWithValue("@designation", txtdesignation.Text.Trim());
            cmd.Parameters.AddWithValue("@address", txtaddress.Text.Trim());
            cmd.Parameters.AddWithValue("@date", dtp.Value);

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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtStaffName.Text = "";
            txtmobileNo.Text = "";
            txtdesignation.Text = "";
            txtaddress.Text = "";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string staffid = dataGridView1.SelectedRows[0].Cells["staffid"].Value.ToString();
            //MessageBox.Show(sid);
            if (staffid != "")
            {
                SqlConnection cn = new SqlConnection(Program.myConnection);
                string sql = "select * from staff where staffid= " + staffid;

                SqlCommand cmd = new SqlCommand(sql, cn);
                SqlDataReader dr;

                try
                {
                    cn.Open();
                    dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        txtStaffName.Text = dr["staffname"].ToString();
                        txtmobileNo.Text = dr["mobile"].ToString();
                        txtdesignation.Text = dr["designation"].ToString();
                        txtaddress.Text = dr["address"].ToString();
                      
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Selection Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    cn.Close();
                    cn.Dispose();
                    cmd.Dispose();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string sql = "update staff set staffname=@staffname,@mobile=@mobile,designation=@designation,address=@address,date=@date where staffid=" + dataGridView1.SelectedRows[0].Cells["staffid"].Value.ToString();

            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@staffname", txtStaffName.Text.Trim());
            cmd.Parameters.AddWithValue("@mobile", txtmobileNo.Text.Trim());
            cmd.Parameters.AddWithValue("@designation", txtdesignation.Text.Trim());
            cmd.Parameters.AddWithValue("@address", txtaddress.Text.Trim());
            cmd.Parameters.AddWithValue("@date", dtp.Value);
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
                fillGrid();
            }
        }
    }
}
