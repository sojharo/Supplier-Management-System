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
    public partial class EditCity : Form
    {
        SqlConnection con;
        string conStr = "Data Source=SOJHARO\\SQLEXPRESS;Initial Catalog=TestDB;User ID=sa;Password=sojharo";//"Data Source=SOJHARO\\SQLEXPRESS;AttachDbFilename=DB\\TestDb.mdf;Initial Catalog=TestDB;Integrated Security=True";
        List<int> cityNumbList = new List<int>();

        public EditCity()
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
                MessageBox.Show("Kindly enter the city name first.");
            }
            else
            {
                con = new SqlConnection(conStr);
                con.Open();
                try
                {

                    SqlCommand com = new SqlCommand();

                    com.Connection = con;
                    com.CommandText = "update shop_city set cname = @cn "+
                                        "where cnumb = @cnum";

                    com.Parameters.Add(new SqlParameter("@cn", textBox1.Text.Trim().ToUpper()));
                    com.Parameters.Add(new SqlParameter("@cnum", cityNumbList[comboBox1.SelectedIndex]));

                    com.ExecuteNonQuery();

                    MessageBox.Show("Information successfully updated.");
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = comboBox1.SelectedItem.ToString();
        }
    }
}
