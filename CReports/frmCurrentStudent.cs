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
using CrystalDecisions.CrystalReports.Engine;

namespace School1.CReports
{
    public partial class frmCurrentStudent : Form
    {
        public frmCurrentStudent()
        {
            InitializeComponent();
        }

        private void frmCurrentStudent_Load(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string qry = "select student.id, student.GRno, student.SName, student.FName, student.mobile, student.classid, student.fee, classes.classname from student " +
                "inner join classes on student.classid = classes.classid where student.present=1";
            
            SqlDataAdapter adpt = new SqlDataAdapter(qry, cn);
            DataTable dt = new DataTable();

            DataSet1 ds = new DataSet1();
            
            try
            {
                cn.Open();
                adpt.Fill(ds.currentStudent);

                currentStudentReport rpt = new currentStudentReport();
                rpt.SetDataSource(ds);
                crystalReportViewer1.ReportSource = rpt;

                /*
                adpt.Fill(ds.dsMemberCard);
                rptMemberCardList mcl = new rptMemberCardList();
                mcl.SetDataSource(ds);
                crystalReportViewer1.ReportSource = mcl;
                */
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cn.Close();
                cn.Dispose();

            }
            /*
            string test = "select * from tablename";
            DataSet testds = new DataSet();
            SqlConnection cnn = new SqlConnection("user id=username;password=pwd;server" +
              "=dataserver;Trusted_Connection=false;database=dbname;connection timeout=30");
            SqlCommand testcmd = new SqlCommand(test, cnn);
            testcmd.CommandType = CommandType.Text;
            SqlDataAdapter testda = new SqlDataAdapter(testcmd);
            testda.Fill(testds, "testttable");
            cnn.Open();
            ReportDocument myReportDocument;
            myReportDocument = new ReportDocument();
            myReportDocument.Load(@"D:\Reports\rptitemintrans.rpt");
            myReportDocument.SetDataSource(testds);
            myReportDocument.SetDatabaseLogon("username", "pwd");
            crystalReportViewer1.ReportSource = myReportDocument;
            crystalReportViewer1.DisplayToolbar = true;
             */
 
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
