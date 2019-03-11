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
    public partial class frmResult : Form
    {
        public frmResult()
        {
            InitializeComponent();
        }
        string studentid = "";
        string classid = "";
       
        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtGRNo.Text = "";
            txtrollno.Text = "";
            txtSName.Text = "";
            txtFName.Text = "";
            txtRemarks.Text = "";
            txtObtainMarks.Text = "";
            txtTotal.Text = "";
            txtPercentage.Text = "";
        }

        private void frmResult_Load(object sender, EventArgs e)
        {
            //cmbSubject.SelectedIndex = 0;
            cmbexam.SelectedIndex = 0;
            fillGrid();
            clearbox();
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
        }

        private void txtTotal_Leave(object sender, EventArgs e)
        {
            double obtMarks = Convert.ToDouble(txtObtainMarks.Text.Trim());
            double totalMarks = Convert.ToDouble(txtTotal.Text.Trim());

            //decimal percentage = Convert.ToInt32(txtPercentage.Text.Trim());

            //decimal percentage = 1;

            double ab = Convert.ToDouble((obtMarks / totalMarks) * 100);

            txtPercentage.Text = ab.ToString();

            if(ab >= 80)
            {
                MessageBox.Show("A1 Grade");
            }
            else if(ab >= 70)
            {
                MessageBox.Show("A Grade");
            }
            else if (ab >= 60)
            {
                MessageBox.Show("B Grade");
            }
            else if (ab >= 50)
            {
                MessageBox.Show("C Grade");
            }
            else if (ab >= 40)
            {
                MessageBox.Show("D Grade");
            }
            else
            {
                MessageBox.Show("Fail");
            }

            MessageBox.Show(ab.ToString());
            
        }

        private void txtGRNo_Leave(object sender, EventArgs e)
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
                        txtrollno.Text = dr["Rollno"].ToString();
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
            string qry = "select Resultinfo.sid, Resultinfo.classid,Resultinfo.result_id, Resultinfo.grno,Resultinfo.rollno, " +
                "student.SName,student.FName, classes.classname,Resultinfo.exam,Resultinfo.obtmarks,Resultinfo.marks,Resultinfo.percentage,Resultinfo.remark,Resultinfo.dtp " +
                " from Resultinfo " +
              " inner join student on Resultinfo.sid = student.id " +
               " inner join classes on Resultinfo.classid = classes.classid "+
               "where Resultinfo.grno='" + txtGRNo.Text.Trim() + "'";
                  
            SqlDataAdapter adpt = new SqlDataAdapter(qry, cn);

            DataTable dt = new DataTable();

            cn.Open();
            adpt.Fill(dt);

            dataGridView1.DataSource = dt;
        }
        private void clearbox()
        {
            txtGRNo.Text = "";
            txtrollno.Text = "";
            txtSName.Text = "";
            txtFName.Text = "";
            txtRemarks.Text = "";
            txtObtainMarks.Text = "";
            txtTotal.Text = "";
            txtPercentage.Text = "";
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string sql = "insert into Resultinfo (grno,rollno,sid,classid,dtp,remark,exam,obtmarks,marks,percentage) " +
                "values (@grno,@rollno,@sid,@classid,@dtp,@remark,@exam,@obtmarks,@marks,@percentage)";

            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@grno", txtGRNo.Text.Trim());
            cmd.Parameters.AddWithValue("@rollno", txtrollno.Text.Trim());
            cmd.Parameters.AddWithValue("@sid", studentid);
            cmd.Parameters.AddWithValue("@classid", classid);
            cmd.Parameters.AddWithValue("@dtp", dtp.Value);
            cmd.Parameters.AddWithValue("@remark", txtRemarks.Text.Trim());
            cmd.Parameters.AddWithValue("@exam", cmbexam.SelectedIndex);
            cmd.Parameters.AddWithValue("@obtmarks", txtObtainMarks.Text.Trim());
            cmd.Parameters.AddWithValue("@marks", txtTotal.Text.Trim());
            cmd.Parameters.AddWithValue("@percentage", txtPercentage.Text.Trim());

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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string sql = "update Resultinfo set dtp=@dtp,remark=@remark,exam=@exam,obtmarks=@obtmarks,marks=@marks,percentage=@percentage " +
            " where result_id=" + dataGridView1.SelectedRows[0].Cells["result_id"].Value.ToString();

            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@dtp", dtp.Value);
            cmd.Parameters.AddWithValue("@remark", txtRemarks.Text.Trim());
            cmd.Parameters.AddWithValue("@exam", cmbexam.SelectedIndex);
            cmd.Parameters.AddWithValue("@obtmarks", txtObtainMarks.Text.Trim());
            cmd.Parameters.AddWithValue("@marks", txtTotal.Text.Trim());
            cmd.Parameters.AddWithValue("@percentage", txtPercentage.Text.Trim());

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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
             string result_id = dataGridView1.SelectedRows[0].Cells["result_id"].Value.ToString();
            //MessageBox.Show(sid);
             if (result_id != "")
             {
                 SqlConnection cn = new SqlConnection(Program.myConnection);
                 string sql = "select * from Resultinfo where result_id = " + result_id;

                 SqlCommand cmd = new SqlCommand(sql, cn);
                 SqlDataReader dr;

                 try
                 {
                     cn.Open();
                     dr = cmd.ExecuteReader();

                     if (dr.Read())
                     {
                         // txtSName.Text = dr["sname"].ToString();
                         // txtFName.Text = dr["fname"].ToString();
                         txtRemarks.Text = dr["remark"].ToString();
                         txtObtainMarks.Text = dr["obtmarks"].ToString();
                         txtTotal.Text = dr["marks"].ToString();
                         txtPercentage.Text = dr["percentage"].ToString();

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
