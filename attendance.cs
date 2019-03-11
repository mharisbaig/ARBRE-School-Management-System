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
    public partial class attendance : Form
    {
        public attendance()
        {
            InitializeComponent();
        }

        int a = 1;
        int b = 1;

        string today = DateTime.Now.ToString("dd-MMM-yy");


        private void fillGrid()
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            
            
            //MessageBox.Show(dt.ToString());

            string qry = "select attendance.attendance_id, attendance.sid, attendance.classid, attendance.grno, student.SName, classes.classname, attendance.attenday, attendance.status " +
                "  from attendance " +
                "inner join student on attendance.sid = student.id " +
                "inner join classes on attendance.classid = classes.classid " +
                "where attendance.attenday = '" + today + "'";

            SqlDataAdapter adpt = new SqlDataAdapter(qry, cn);

            DataTable dt = new DataTable();

            cn.Open();
            adpt.Fill(dt);

            dataGridView1.DataSource = dt;
        }

        private void attendance_Load(object sender, EventArgs e)
        {
            //fillGrid();
            fillClass();

            cmbClass.SelectedIndex = 0;
            cmbStatus.SelectedIndex = 0;

            //cmbStatus.SelectedIndex = 0;

            //dataGridView1.Columns[0].Visible = false;
            //dataGridView1.Columns[1].Visible = false;          
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

        private void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (a > 2)
            {
                if (cmbClass.Text != "Select Class")
                {
                    SqlConnection cn = new SqlConnection(Program.myConnection);
                    string qry = "select attendance.attendance_id, attendance.sid, attendance.classid, attendance.attenday, attendance.status, attendance.grno, " +
                        "student.SName, classes.classname from attendance " +
                        "inner join student on attendance.sid = student.id " +
                        "inner join classes on attendance.classid = classes.classid " +
                        "where attendance.attenday = '" + today + "' and attendance.classid=" + cmbClass.SelectedValue;

                    SqlDataAdapter adpt = new SqlDataAdapter(qry, cn);
                    DataTable dt = new DataTable();

                    try
                    {
                        cn.Open();
                        adpt.Fill(dt);

                        lblID.Text = cmbClass.SelectedValue.ToString();

                        //dataGridView1.DataSource = null;

                        //MessageBox.Show(dt.Rows.Count.ToString());

                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = dt;

                        /*
                        if (dr.Read())
                        {
                            txtSName.Text = dr["sname"].ToString();
                            txtFName.Text = dr["fname"].ToString();
                            cmbPresent.SelectedIndex = Convert.ToInt32(dr["present"]);
                            cmbClass.SelectedValue = dr["classid"];
                            pictureBox1.ImageLocation = Application.StartupPath + "\\studentimg\\" + dr["img"].ToString();
                        }
                        */


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Combo Selection Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        cn.Close();
                        cn.Dispose();

                    }
                }
                else
                {
                    MessageBox.Show("Please select any class for view record", "Select Class", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            a = a + 1;

             
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string sid = dataGridView1.SelectedRows[0].Cells["attendance_id"].Value.ToString();
            MessageBox.Show(sid);

            SqlConnection cn = new SqlConnection(Program.myConnection);
            string sql = "select * from attendance where attendance_id= " + sid;

            SqlCommand cmd = new SqlCommand(sql, cn);
            SqlDataReader dr;

            try
            {
                cn.Open();
                dr = cmd.ExecuteReader();

                if (dr.Read())
                {

                    txtgrno.Text = dr["grno"].ToString();
                    txtrollno.Text = dr["rollno"].ToString();
                    txtSName.Text = dr["studentname"].ToString();
                    txtFName.Text = dr["Fathername"].ToString();
                    //cmbClass.SelectedValue = dr["classid"];
                    cmbStatus.SelectedValue = dr["status"];

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Grid Selection Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.Close();
                cn.Dispose();
                cmd.Dispose();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            updateRecord();
            fillGrid();
        }

        private void updateRecord()
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string sql = "update attendance set status=@status" + dataGridView1.SelectedRows[0].Cells["attendance_id"].Value.ToString();

            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@status", cmbStatus.Text);

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

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            insertAttendance();
        }
        private void insertAttendance()
        {
            if (isCreate() == false)
            {
                SqlConnection cn = new SqlConnection(Program.myConnection);
                string qry = "select * from student where present=1";

                SqlCommand cmd = new SqlCommand(qry, cn);
                SqlDataReader dr = null;
                try
                {
                    cn.Open();
                    dr = cmd.ExecuteReader();


                    string sid, classid, grNo, rollno, studentname, Fathername;


                    while (dr.Read())
                    {
                        sid = dr["id"].ToString();
                        classid = dr["classid"].ToString();
                        grNo = dr["grno"].ToString();
                        rollno = dr["Rollno"].ToString();
                        studentname = dr["SName"].ToString();
                        Fathername = dr["FName"].ToString();

                        generateFee(sid, classid, grNo, rollno, studentname, Fathername);
                    }
                    MessageBox.Show("Attendance has been generated.", "Attendance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    cn.Close();
                    cn.Dispose();
                    dr.Close();
                }
            }
            else
            {
                MessageBox.Show("Today's attendance is already exist", "Attendance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void generateFee(string sid, string classid, string grno, string rollno, string studentname, string Fathername)
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string qry = "insert into attendance (sid,classid,grno,rollno,studentname,Fathername,status,attenday) values (@sid,@classid,@grno,@rollno,@studentname,@Fathername,@status,@attenday)";
            SqlCommand cmd = new SqlCommand(qry, cn);

            //int year = DateTime.Now.Year;
            string month = DateTime.Now.ToString("dd-MMM-yyyy");
            //MessageBox.Show(month);

            cmd.Parameters.AddWithValue("@sid", sid);
            cmd.Parameters.AddWithValue("@classid", classid);
            cmd.Parameters.AddWithValue("@grno", grno);
            cmd.Parameters.AddWithValue("@rollno", rollno);
            cmd.Parameters.AddWithValue("@studentname", studentname);
            cmd.Parameters.AddWithValue("@Fathername", Fathername);
            cmd.Parameters.AddWithValue("@attenday", month);
            cmd.Parameters.AddWithValue("@status", "PRESENT");

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
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

        private bool isCreate()
        {
            bool res = true;

            //int year = DateTime.Now.Year;
            string month = DateTime.Now.ToString("dd-MMM-yyyy");

            //string duemonth = month.ToUpper() + "-" + year.ToString();

            SqlConnection cn = new SqlConnection(Program.myConnection);
            string qry = "select attendance_id from attendance where attenday ='" + month + "'";
            SqlCommand cmd = new SqlCommand(qry, cn);

            try
            {
                cn.Open();
                object feeid = cmd.ExecuteScalar();

                if (Convert.ToInt32(feeid) > 0)
                {
                    //MessageBox.Show("Fee already generated");
                    res = true;
                }
                else
                {
                    //MessageBox.Show("Yes! You can generate fee");
                    res = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cn.Close();
                cn.Dispose();
                cmd.Dispose();
            }
            return res;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {  
            /*
            if (b > 4)
            {
                    string sid = dataGridView1.SelectedRows[0].Cells["attendance_id"].Value.ToString();

                    //MessageBox.Show("Class id is" + lblID.Text);

                    SqlConnection cn = new SqlConnection(Program.myConnection);
                    string sql = "select * from attendance where attendance_id= " + sid;

                    SqlCommand cmd = new SqlCommand(sql, cn);
                    SqlDataReader dr;

                    try
                    {
                        cn.Open();
                        dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {

                            txtgrno.Text = dr["grno"].ToString();
                            txtrollno.Text = dr["rollno"].ToString();
                            txtSName.Text = dr["studentname"].ToString();
                            txtFName.Text = dr["Fathername"].ToString();
                            //cmbClass.SelectedValue = dr["classid"];
                            cmbStatus.SelectedValue = dr["status"];

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Grid Selection Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        cn.Close();
                        cn.Dispose();
                        cmd.Dispose();
                    }
                
            }
            b = b + 1;
            */
        }
    }
}
