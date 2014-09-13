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
    public partial class NewSupplier : Form
    {
        SqlConnection con;
        string conStr = "Data Source=SOJHARO\\SQLEXPRESS;Initial Catalog=TestDB;User ID=sa;Password=sojharo";//"Data Source=SOJHARO\\SQLEXPRESS;AttachDbFilename=DB\\TestDb.mdf;Initial Catalog=TestDB;Integrated Security=True";
        List<int> cityNumbList = new List<int>();

        public NewSupplier()
        {
            InitializeComponent();

            con = new SqlConnection(conStr);
            con.Open();
            try
            {
                SqlCommand com1 = new SqlCommand();
                com1.Connection = con;

                com1.Connection = con;
                com1.CommandText = "select * from shop_city order by cname";
                com1.CommandType = System.Data.CommandType.Text;
                SqlDataReader dr = com1.ExecuteReader();


                while (dr.Read())
                {

                    cityNumbList.Add(int.Parse(dr["cnumb"].ToString()));

                    comboBox1.Items.Add(dr["cname"].ToString());
                }

                comboBox1.SelectedIndex = 0;
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                MessageBox.Show("Kindly enter the supplier name first.");
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
                    comTemp.CommandText = "select shop_no from shop";
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
                    com.CommandText = "insert into shop (shop_no, name, account_title, "+
                                    "account_no, cnumb, sh_bank) values (@sh, @n, @at, @an, @cn, @sb)";

                    com.Parameters.Add(new SqlParameter("@sh", newKeyNo));
                    com.Parameters.Add(new SqlParameter("@n", textBox1.Text.Trim().ToUpper()));
                    com.Parameters.Add(new SqlParameter("@at", textBox2.Text.Trim().ToUpper()));
                    com.Parameters.Add(new SqlParameter("@an", textBox3.Text.Trim().ToUpper()));
                    com.Parameters.Add(new SqlParameter("@cn", cityNumbList[comboBox1.SelectedIndex]));
                    com.Parameters.Add(new SqlParameter("@sb", textBox4.Text.Trim().ToUpper()));

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
