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
    public partial class frmStudent : Form
    {
        public frmStudent()
        {
            InitializeComponent();
        }

        int a = 1;

        DataTable studentDataTable = new DataTable();

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbPresent.SelectedIndex = 1;

            //generateSerial();
            fillGrid();
            
            fillClass();

            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].Visible = true;
            dataGridView1.Columns[9].Visible = false;
           

            dataGridView1.Columns[1].HeaderText = "S #";
            dataGridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dataGridView.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            dataGridView1.Columns[1].Width = 50;

            dataGridView1.RowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.White;

        }

        private void generateSerial()
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string qry = "select * from student where present=1 order by id desc";
         
            SqlCommand cmd = new SqlCommand(qry, cn);
            SqlDataReader dr = null;

            cn.Open();
            dr = cmd.ExecuteReader();
            

            string sid = "";
            
            int sNumber = 0;

            while(dr.Read())
            {
                sid = dr["id"].ToString();
                sNumber = sNumber + 1;
                updateSerial(sid, sNumber);
            }
        }

        private void updateSerial(string studentId,int serialNumber)
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string qry = "update student set sno=@sno where id=" + studentId;
            
            SqlCommand cmd = new SqlCommand(qry, cn);
            cmd.Parameters.AddWithValue("@sno", serialNumber);

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
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

        private void clearBox()
        {
            txtAddress.Text = "";
            txtBrowse.Text = "";
            txtFName.Text = "";
            txtFormNo.Text = "";
            txtMobile.Text = "";
            txtPhone.Text = "";
            txtSName.Text = "";
            txtgrno.Text = "";
            txtrollno.Text = "";
            txtFee.Text = "";
            pictureBox1.ImageLocation = null;
        }

        private void fillGrid()
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string qry = "spfillstudent";
            SqlDataAdapter adpt = new SqlDataAdapter(qry, cn);
           
            cn.Open();
            adpt.Fill(studentDataTable);

            dataGridView1.DataSource = studentDataTable;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int sid = insertRecord();
            //generateSerial();
            insertImage();

            freshData(sid);

            //fillGrid();
            clearBox();
        }

        private void insertImage()
        {

            //pictureBox1.Image.Save(dlg.FileName);
            string filename = lblImage.Text;

            string onlyfilename = Path.GetFileName(filename);

            //updateImage(onlyfilename);

            string imagePath = Application.StartupPath + "\\studentimg";

            

            bool checkFolder = Directory.Exists(imagePath);
            if (checkFolder == false)
            {
                Directory.CreateDirectory(imagePath);
            }
            

            string destinationFile = System.IO.Path.Combine(imagePath, onlyfilename);

            if (File.Exists(destinationFile))
            {
                File.Delete(destinationFile);
            }

            File.Copy(filename, destinationFile);

            pictureBox1.ImageLocation = destinationFile;


        }

        private void updateUploadImage()
        {

            //pictureBox1.Image.Save(dlg.FileName);
            string filename = lblImage.Text;

            string onlyfilename = Path.GetFileName(filename);

            updateImage(onlyfilename);

            string imagePath = Application.StartupPath + "\\studentimg";



            bool checkFolder = Directory.Exists(imagePath);
            if (checkFolder == false)
            {
                Directory.CreateDirectory(imagePath);
            }


            string destinationFile = System.IO.Path.Combine(imagePath, onlyfilename);

            if (File.Exists(destinationFile))
            {
                File.Delete(destinationFile);
            }

            File.Copy(filename, destinationFile);

            pictureBox1.ImageLocation = destinationFile;


        }

        private int insertRecord()
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string sql = "insert into student (sname,fname,phoneno,mobile,address,present,regdate,GRno,dob,formno,classid,FEE,Rollno,img) " +
                "values (@sname,@fname,@phoneno,@mobile,@address,@present,@regdate,@GRno,@dob,@formno,@classid,@fee,@Rollno,@img)";

            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@sname", txtSName.Text.Trim());
            cmd.Parameters.AddWithValue("@fname", txtFName.Text.Trim());
            cmd.Parameters.AddWithValue("@phoneno", txtPhone.Text.Trim());
            cmd.Parameters.AddWithValue("@mobile", txtMobile.Text.Trim());
            cmd.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
            cmd.Parameters.AddWithValue("@present", cmbPresent.SelectedIndex);
            cmd.Parameters.AddWithValue("@regdate", dtRegistration.Value);
            cmd.Parameters.AddWithValue("@GRno", txtgrno.Text.Trim());
            cmd.Parameters.AddWithValue("@dob", dtDOB.Value);
            cmd.Parameters.AddWithValue("@formno", txtFormNo.Text.Trim());
            cmd.Parameters.AddWithValue("@fee", txtFee.Text.Trim());
            cmd.Parameters.AddWithValue("@Rollno", txtrollno.Text.Trim());
            cmd.Parameters.AddWithValue("@classid", cmbClass.SelectedValue);
            cmd.Parameters.AddWithValue("@img", Path.GetFileName(lblImage.Text));

            int sid = 0;

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record has been inserted in database", "Insert Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                sid = Program.getID("student", "id");

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
            }
            return sid;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            /*
            string sid = dataGridView1.SelectedCells[0].Value.ToString();
            //MessageBox.Show(sid);
            if (sid != "")
            {
                SqlConnection cn = new SqlConnection(Program.myConnection);
                string sql = "select * from student where id= " + sid;

                SqlCommand cmd = new SqlCommand(sql, cn);
                SqlDataReader dr;

                try
                {
                    cn.Open();
                    dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        txtSName.Text = dr["sname"].ToString();
                        txtFName.Text = dr["fname"].ToString();
                        txtrollno.Text = dr["Rollno"].ToString();
                        cmbPresent.SelectedIndex = Convert.ToInt32(dr["present"]);
                        cmbClass.SelectedValue = dr["classid"];
                        pictureBox1.ImageLocation = Application.StartupPath + "\\studentimg\\" + dr["img"].ToString();
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
            */
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int sid = updateRecord();

            updateUploadImage();

            freshData(sid);
        }

        private int updateRecord()
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string sql = "update student set sname=@sname,fname=@fname,phoneno=@phoneno, " +
            "mobile=@mobile,address=@address,present=@present,regdate=@regdate,GRno=@GRno,dob=@dob, img=@img, " +
                "formno=@formno,classid=@classid,fee=@fee,Rollno=@Rollno where id=" + dataGridView1.SelectedCells[0].Value.ToString();
            
            string sid = dataGridView1.SelectedCells[0].Value.ToString();
            lblSid.Text = sid;


            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@sname", txtSName.Text.Trim());
            cmd.Parameters.AddWithValue("@fname", txtFName.Text.Trim());
            cmd.Parameters.AddWithValue("@phoneno", txtPhone.Text.Trim());
            cmd.Parameters.AddWithValue("@mobile", txtMobile.Text.Trim());
            cmd.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
            cmd.Parameters.AddWithValue("@present", cmbPresent.SelectedIndex);
            cmd.Parameters.AddWithValue("@regdate", dtRegistration.Value);
            cmd.Parameters.AddWithValue("@GRno", txtgrno.Text.Trim());
            cmd.Parameters.AddWithValue("@dob", dtDOB.Value);
            cmd.Parameters.AddWithValue("@formno", txtFormNo.Text.Trim());
            cmd.Parameters.AddWithValue("@fee", txtFee.Text.Trim());
            cmd.Parameters.AddWithValue("@Rollno", txtrollno.Text.Trim());
            cmd.Parameters.AddWithValue("@classid", cmbClass.SelectedValue);
            cmd.Parameters.AddWithValue("@img", Path.GetFileName(lblImage.Text));

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

            int id = Convert.ToInt32(sid);
            return id;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are you sure you want to delete that record?", "Deletion Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                deleteRecord();
                fillGrid();
            }
        }

        private void deleteRecord()
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string sql = "delete from student where id=" + dataGridView1.SelectedCells[0].Value.ToString();

            SqlCommand cmd = new SqlCommand(sql, cn);

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record has been deleted", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Deletion Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.Close();
                cn.Dispose();
                cmd.Dispose();
            }
        }

        private void txtBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                lblImage.Text = dlg.FileName;

                pictureBox1.ImageLocation = lblImage.Text;

                //pictureBox1.Image.Save(dlg.FileName);
                /*
                string filename = dlg.FileName;

                string onlyfilename = Path.GetFileName(filename);
                if (lblSid.Text == "")
                {
                    updateImage(onlyfilename);

                    string imagePath = Application.StartupPath + "\\studentimg";

                   

                    bool checkFolder = Directory.Exists(imagePath);
                    if (checkFolder == false)
                    {
                        Directory.CreateDirectory(imagePath);
                    }
                   

                    string destinationFile = System.IO.Path.Combine(imagePath, onlyfilename);

                    if (File.Exists(destinationFile))
                    {
                        File.Delete(destinationFile);
                    }

                    File.Copy(filename, destinationFile);

                    pictureBox1.ImageLocation = destinationFile;
                }
                */
            }
        }

        private void updateImage(string imagefilename)
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string sql = "update student set img=@img where id=" + dataGridView1.SelectedCells[0].Value.ToString();

            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@img", imagefilename);

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                pictureBox1.ImageLocation = imagefilename;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Update Image Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txtAddress.Text = "";
            txtFName.Text = "";
            txtSName.Text = "";
            txtFormNo.Text = "";
            txtFee.Text = "";
            txtgrno.Text = "";
            txtrollno.Text = "";
            pictureBox1.ImageLocation = "";
            txtFormNo.Focus();
            lblSid.Text = "";
        }

        private void freshData(object sid)
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string str = "select * from student where (id='" + sid + "')";
            SqlCommand cmd = new SqlCommand(str, cn);
            SqlDataReader dr = null;

            cn.Open();
            dr = cmd.ExecuteReader();

            studentDataTable.Load(dr, LoadOption.OverwriteChanges);
            studentDataTable.AcceptChanges();
        }



        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (a > 4)
            {
                string sid = dataGridView1.SelectedCells[0].Value.ToString();
                //MessageBox.Show(sid);
                if (sid != "")
                {
                    SqlConnection cn = new SqlConnection(Program.myConnection);
                    string sql = "select * from student where id= " + sid;

                    SqlCommand cmd = new SqlCommand(sql, cn);
                    SqlDataReader dr;

                    try
                    {
                        cn.Open();
                        dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {
                            txtSName.Text = dr["sname"].ToString();
                            txtFName.Text = dr["fname"].ToString();
                            txtgrno.Text = dr["grno"].ToString();
                            txtrollno.Text = dr["Rollno"].ToString();
                            cmbPresent.SelectedIndex = Convert.ToInt32(dr["present"]);
                            cmbClass.SelectedValue = dr["classid"];

                            lblSid.Text = dr["id"].ToString();
                            pictureBox1.ImageLocation = Application.StartupPath + "\\studentimg\\" + dr["img"].ToString();
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

                /*
                string sid = dataGridView1.SelectedCells[0].Value.ToString();
                //MessageBox.Show(sid);

                SqlConnection cn = new SqlConnection(Program.myConnection);
                string sql = "select * from student where id= " + sid;

                SqlCommand cmd = new SqlCommand(sql, cn);
                SqlDataReader dr;

                try
                {
                    cn.Open();
                    dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        txtSName.Text = dr["sname"].ToString();
                        txtFName.Text = dr["fname"].ToString();
                        cmbPresent.SelectedIndex = Convert.ToInt32(dr["present"]);
                        cmbClass.SelectedValue = dr["classid"];
                        pictureBox1.ImageLocation = Application.StartupPath + "\\studentimg\\" + dr["img"].ToString();
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
                }*/
            }
            a = a + 1;
        }

        private void lblImage_Click(object sender, EventArgs e)
        {

        }
    }
}
