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

namespace School1.CReports
{
    public partial class frmfeesReport : Form
    {
        public frmfeesReport()
        {
            InitializeComponent();
        }

        private void frmfeesReport_Load(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string qry = "DECLARE @schoolname varchar(100); " +
                "SET NOCOUNT OFF; " +
                "SET @schoolname = (SELECT schoolname FROM Schoolinfo) " +
                "select fees.sid,fees.fees, classes.classname,student.GRno,student.SName,student.FName, @schoolname as schoolname from fees " +
                "inner join classes on fees.classid = classes.classid " +
                "inner join student on fees.sid = student.id where status='unpaid'";

            SqlDataAdapter adpt = new SqlDataAdapter(qry, cn);
            //DataTable dt = new DataTable();

            DataSet2 ds = new DataSet2();
            

            try
            {
               cn.Open();
                adpt.Fill(ds.fees);

                feesReport1 rpt = new feesReport1();
                rpt.SetDataSource(ds);
                crystalReportViewer1.ReportSource = rpt;

             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cn.Close();
                cn.Dispose();
                
            }
        }
    }
}
