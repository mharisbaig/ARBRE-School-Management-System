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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            attendance frm = new attendance();
            frm.MdiParent = this;
            frm.Show();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            frmClasses frm = new frmClasses();
            frm.MdiParent = this;
            frm.Show();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            frmSubject frm = new frmSubject();
            frm.MdiParent = this;
            frm.Show();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            frmFees frm = new frmFees();
            frm.MdiParent = this;
            frm.Show();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {

            bool createRes = isCreate();
            if(createRes == true)
            {
                MessageBox.Show("This month fee is already generated");
            }
            else
            {
                insertFee();
                MessageBox.Show("This month fee has been generated", "Fee Generate", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }                    
        }

        private void insertFee()
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string qry = "select * from student where present=1";

            SqlCommand cmd = new SqlCommand(qry, cn);
            SqlDataReader dr = null;
            try
            {
                cn.Open();
                dr = cmd.ExecuteReader();


                string sid, classid, fees;


                while (dr.Read())
                {
                    sid = dr["id"].ToString();
                    classid = dr["classid"].ToString();
                    fees = dr["fee"].ToString();


                    generateFee(sid, classid, fees);
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
                dr.Close();
            }
        }

        private void generateFee(string sid, string classid, string fees)
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string qry = "insert into fees (sid,classid,fees,paymonth,status) values (@sid,@classid,@fees,@paymonth,@status)";
            SqlCommand cmd = new SqlCommand(qry, cn);

            int year = DateTime.Now.Year;
            string month = DateTime.Now.ToString("MMM");

            string duemonth = month.ToUpper() + "-" + year.ToString();

            cmd.Parameters.AddWithValue("@sid", sid);
            cmd.Parameters.AddWithValue("@classid", classid);
            cmd.Parameters.AddWithValue("@fees", fees);
            cmd.Parameters.AddWithValue("@paymonth", duemonth);
            cmd.Parameters.AddWithValue("@status", "UNPAID");
            
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

            int year = DateTime.Now.Year;
            string month = DateTime.Now.ToString("MMM");

            string duemonth = month.ToUpper() + "-" + year.ToString();

            SqlConnection cn = new SqlConnection(Program.myConnection);
            string qry = "select feeid from fees where paymonth='" + duemonth + "'";
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

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void currentStudentReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CReports.frmCurrentStudent frm = new CReports.frmCurrentStudent();
            frm.Show();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            frmResult frm = new  frmResult();
            frm.MdiParent = this;
            frm.Show();
        }

        private void feesReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CReports.frmfeesReport frm = new CReports.frmfeesReport();
            frm.Show();
        }

        private void schoolinfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Schoolinfo frm = new Schoolinfo();
            frm.MdiParent = this;
            frm.Show();
        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            frmStudent frm = new frmStudent();
            frm.MdiParent = this;
            frm.Show();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            frmbackup frm = new frmbackup();
            frm.MdiParent = this;
            frm.Show();
        }

        private void sTAFFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmstaff frm = new frmstaff();
            frm.MdiParent = this;
            frm.Show();

        }

        private void staffSalaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frmpayroll frm = new Frmpayroll();
            frm.MdiParent = this;
            frm.Show();
        }

        private void exToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmexpense frm = new frmexpense();
            frm.MdiParent = this;
            frm.Show();
        }
    }
}
