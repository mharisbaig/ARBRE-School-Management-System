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
    public partial class Schoolinfo : Form
    {
        public Schoolinfo()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Schoolinfo_Load(object sender, EventArgs e)
        {
            fillGrid();
        }
        private void fillGrid()
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string qry = "Select * from schoolinfo";
            SqlDataAdapter adpt = new SqlDataAdapter(qry, cn);

            DataTable dt = new DataTable();

            cn.Open();
            adpt.Fill(dt);

            dataGridView1.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            updateRecord();
            fillGrid();
        }

        private void updateRecord()
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string sql = "update schoolinfo set schoolname=@schoolname,phone=@phone,regno=@regno, " +
            "mobile=@mobile,address=@address,email=@email,website=@website "; 

            SqlCommand cmd = new SqlCommand(sql, cn);
            
            cmd.Parameters.AddWithValue("@schoolname", txtschoolname.Text.Trim());
            cmd.Parameters.AddWithValue("@phone", txtphone.Text.Trim());
            cmd.Parameters.AddWithValue("@mobile", txtmobile.Text.Trim());
            cmd.Parameters.AddWithValue("@regno", txtregno.Text.Trim());
            cmd.Parameters.AddWithValue("@address", txtaddress.Text.Trim());
            cmd.Parameters.AddWithValue("@email", txtemail.Text.Trim());
            cmd.Parameters.AddWithValue("@website",txtwebsite.Text.Trim());
          

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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           // string result_id = dataGridView1.SelectedRows[0].Cells["schoolname"].Value.ToString();
            //MessageBox.Show(sid);

            SqlConnection cn = new SqlConnection(Program.myConnection);
            string sql = "select * from schoolinfo ";

            SqlCommand cmd = new SqlCommand(sql, cn);
            SqlDataReader dr;

            try
            {
                cn.Open();
                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                     txtregno.Text = dr["regno"].ToString();
                     txtschoolname.Text = dr["schoolname"].ToString();
                    txtaddress.Text = dr["address"].ToString();
                    txtemail.Text = dr["email"].ToString();
                    txtwebsite.Text = dr["website"].ToString();
                    txtphone.Text = dr["phone"].ToString();
                    txtmobile.Text = dr["mobile"].ToString();

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
