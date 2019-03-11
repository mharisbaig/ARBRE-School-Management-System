using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Data.SqlClient;


namespace School1
{
    static class Program
    {
        public static string myConnection = "Data Source=" + Environment.MachineName + ";Initial Catalog=School;Integrated Security=True";

        public static object getProductKey()
        {
            Microsoft.Win32.RegistryKey key;
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Arbre");
            object obj = key.GetValue("ProductKey");
            //string pNumber = obj.ToString();
            key.Close();

            /********/

            //Microsoft.Win32.RegistryKey key;
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\Arbre");
            DateTime admonth = DateTime.Now.AddMonths(3);
            //key.GetValue("RegisterTime");

            object objTime = key.GetValue("RegisterTime");
            //string pNumber = obj.ToString();
            key.Close();



            //string pNumber = obj.ToString();
            key.Close();

            return obj;
        }

        public static int getID(string table, string primarykey)
        {
            SqlConnection cn = new SqlConnection(Program.myConnection);
            string sql = "select max("+ primarykey + ") from " + table;
            SqlCommand cmd = new SqlCommand(sql, cn);
            SqlDataReader dr = null;

            int id = 0;
            string sid="";

            try
            {
                cn.Open();
                dr = cmd.ExecuteReader();
                if(dr.Read())
                {
                    sid = dr[0].ToString();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cn.Close();
                dr.Dispose();
            }
            id = Convert.ToInt32(sid);
            return id;
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            object productNum = getProductKey();
            if (productNum == null)
            {
                MessageBox.Show("Please enter the valid product key for continue process", "Product Key", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Run(new frmRegister());
            }
            else
            {
                Application.Run(new frmMain());
            }
        }

    }
}
