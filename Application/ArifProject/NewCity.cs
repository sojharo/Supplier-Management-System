using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArifProject
{
    public partial class NewCity : Form
    {
        SqlConnection con;
        string conStr = "Data Source=SOJHARO\\SQLEXPRESS;Initial Catalog=TestDB;User ID=sa;Password=sojharo";//"Data Source=SOJHARO\\SQLEXPRESS;AttachDbFilename=DB\\TestDb.mdf;Initial Catalog=TestDB;Integrated Security=True";

        public NewCity()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                MessageBox.Show("Kindly enter the city name first.");
            }
            else
            {
                con = new SqlConnection(conStr);
                con.Open();
                int newKeyNo = -1;
                try
                {
                    SqlCommand comTemp = new SqlCommand();
                    comTemp.Connection = con;
                    comTemp.CommandText = "select cnumb from shop_city";
                    comTemp.CommandType = System.Data.CommandType.Text;
                    SqlDataReader dr = comTemp.ExecuteReader();

                    dr.Read();

                    try
                    {
                        newKeyNo = Int32.Parse(dr.GetValue(0).ToString());
                    }
                    catch (System.InvalidOperationException ee)
                    {
                        Console.WriteLine(ee.Message);
                        newKeyNo = 0;
                    }

                    while (dr.Read())
                    {
                        newKeyNo = Int32.Parse(dr.GetValue(0).ToString());
                    }

                    newKeyNo += 1;

                    dr.Close();

                    SqlCommand com = new SqlCommand();

                    com.Connection = con;
                    com.CommandText = "insert into shop_city (cnumb, cname) values (@cno, @cn)";

                    com.Parameters.Add(new SqlParameter("@cno", newKeyNo));
                    com.Parameters.Add(new SqlParameter("@cn", textBox1.Text.Trim().ToUpper()));

                    com.ExecuteNonQuery();

                    MessageBox.Show("Information successfully stored.");
                    this.Close();
                }
                catch (SqlException eee)
                {
                    MessageBox.Show("Error: " + eee.Message);
                }
                finally
                {
                    con.Dispose();
                }
            }
        }
    }
}
