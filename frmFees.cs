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
    public partial class frmFees : Form
    {
        public frmFees()
        {
            InitializeComponent();
        }

        string studentid = "";
        string classid = "";

        private void frmFees_Load(object sender, EventArgs e)
        {
            cmbStatus.SelectedIndex = 0;

            int year = DateTime.Now.Year;
            txtYear.Text = year.ToString();
            fillGrid();

            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (txtGRNo.Text.Trim().Length <= 0)
            {
                MessageBox.Show("Please enter valid GR Number");
                txtGRNo.Focus();
            }
            else
            {
                SqlConnection cn = new SqlConnection(Program.myConnection);
                string sql = "select * from student where grno= " + txtGRNo.Text.Trim();

                SqlCommand cmd = new SqlCommand(sql, cn);
                SqlDataReader dr;

                try
                {
                    cn.Open();
                    dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        studentid = dr["id"].ToString();
                        txtSName.Text = dr["sname"].ToString();
                        txtFName.Text = dr["fname"].ToString();
                        classid = dr["classid"].ToString();
                        getClassName(dr["classid"].ToString());

                    }
                    else
                    {
                        MessageBox.Show("Please enter valid GR Number", "Invalid GR Number", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtGRNo.Focus();
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
                    fillGrid();
                }
            }
        }

        private void getClassName(string classid)
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string sql = "select * from classes where classid= " + classid;

            SqlCommand cmd = new SqlCommand(sql, cn);
            SqlDataReader dr;

            try
            {
                cn.Open();
                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtClass.Text = dr["classname"].ToString();


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
        private void fillGrid()
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string qry = "select fees.sid, fees.classid,fees.feeid, fees.grno, " +
                "student.SName,student.FName, classes.classname,fees.fees,fees.ReceiptNo,fees.receivedate,fees.paymonth,fees.status,fees.remarks " +
                " from fees " +
              " inner join student on fees.sid = student.id " +
               " inner join classes on fees.classid = classes.classid " +
               "where fees.grno='" + txtGRNo.Text.Trim() + "'";
            SqlDataAdapter adpt = new SqlDataAdapter(qry, cn);

            DataTable dt = new DataTable();

            cn.Open();
            adpt.Fill(dt);

            dataGridView1.DataSource = dt;
        }
        private void clearbox()
        {
            txtGRNo.Text = "";
            txtFees.Text = "";
            txtReceiptNo.Text = "";
            txtRemarks.Text = "";
            txtClass.Text = "";

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            insertFeesRecord();
            fillGrid();
        }

        private void insertFeesRecord()
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string sql = "insert into fees (sid,classid,grno,fees,ReceiptNo,receivedate,paymonth,status,remarks) " +
                "values (@sid,@classid,@grno,@fees,@ReceiptNo,@receivedate,@paymonth,@status,@remarks)";

            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@sid", studentid);
            cmd.Parameters.AddWithValue("@classid", classid);
            cmd.Parameters.AddWithValue("@grno", txtGRNo.Text.Trim());
            cmd.Parameters.AddWithValue("@fees", txtFees.Text.Trim());
            cmd.Parameters.AddWithValue("@ReceiptNo", txtReceiptNo.Text.Trim());
            cmd.Parameters.AddWithValue("@receivedate", dtPayDate.Value);
            cmd.Parameters.AddWithValue("@paymonth", cmbMonth.Text + "-" + txtYear.Text.Trim());
            cmd.Parameters.AddWithValue("@status", cmbStatus.Text);
            cmd.Parameters.AddWithValue("@remarks", txtRemarks.Text.Trim());

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
                clearbox();
            }
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            updateRecord();
            fillGrid();
        }

        private void updateRecord()
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string sql = "update fees set sid=@sid,classid=@classid,grno=@grno,fees=@fees,ReceiptNo=@ReceiptNo,receivedate=@receivedate," +
                "paymonth=@paymonth,status=@status,remarks=@remarks where feeid=" + dataGridView1.SelectedRows[0].Cells["feeid"].Value.ToString();

            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@sid", studentid);
            cmd.Parameters.AddWithValue("@classid", classid);
            cmd.Parameters.AddWithValue("@grno", txtGRNo.Text.Trim());
            cmd.Parameters.AddWithValue("@fees", txtFees.Text.Trim());
            cmd.Parameters.AddWithValue("@ReceiptNo", txtReceiptNo.Text.Trim());
            cmd.Parameters.AddWithValue("@receivedate", dtPayDate.Value);
            cmd.Parameters.AddWithValue("@paymonth", cmbMonth.Text + "-" + txtYear.Text.Trim());
            cmd.Parameters.AddWithValue("@status", cmbStatus.Text);
            cmd.Parameters.AddWithValue("@remarks", txtRemarks.Text.Trim());
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtGRNo.Text = "";
            txtReceiptNo.Text = "";
            txtRemarks.Text = "";
            txtFees.Text = "";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string feeid = dataGridView1.SelectedRows[0].Cells["feeid"].Value.ToString();
            //MessageBox.Show(sid);
            if (feeid != "")
            {
                SqlConnection cn = new SqlConnection(Program.myConnection);
                string sql = "select * from fees where feeid= " + feeid;

                SqlCommand cmd = new SqlCommand(sql, cn);
                SqlDataReader dr;

                try
                {
                    cn.Open();
                    dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        txtGRNo.Text = dr["grno"].ToString();
                        //  txtSName.Text = dr["sname"].ToString();
                        // txtFName.Text = dr["fname"].ToString();
                        txtFees.Text = dr["fees"].ToString();
                        txtReceiptNo.Text = dr["ReceiptNo"].ToString();
                        txtRemarks.Text = dr["remarks"].ToString();

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
    }
}
