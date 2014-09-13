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
    public partial class EditSupplier : Form
    {
        SqlConnection con;
        string conStr = "Data Source=SOJHARO\\SQLEXPRESS;Initial Catalog=TestDB;User ID=sa;Password=sojharo";//"Data Source=SOJHARO\\SQLEXPRESS;AttachDbFilename=DB\\TestDb.mdf;Initial Catalog=TestDB;Integrated Security=True";
        List<int> cityNumbList = new List<int>();
        List<Int32> supplierListComboBox = new List<Int32>();

        public EditSupplier()
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

                bool hasValues = false;
                while (dr.Read())
                {

                    cityNumbList.Add(int.Parse(dr["cnumb"].ToString()));

                    comboBox1.Items.Add(dr["cname"].ToString());

                    comboBox3.Items.Add(dr["cname"].ToString());
                    hasValues = true;
                }

                if (hasValues)
                {
                    comboBox1.SelectedIndex = 0;
                    comboBox3.SelectedIndex = 0;
                }
                else
                {
                    comboBox1.SelectedIndex = 0;
                    comboBox3.SelectedIndex = 0;
                }
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
                try
                {

                    SqlCommand com = new SqlCommand();

                    com.Connection = con;
                    com.CommandText = "update shop set name = @cn, account_title = @at, account_no = @an, " +
                                        "cnumb = @shn, sh_bank = @shb where shop_no = @sh";

                    

                    com.Parameters.Add(new SqlParameter("@cn", textBox1.Text.Trim().ToUpper()));
                    com.Parameters.Add(new SqlParameter("@at", textBox2.Text.Trim().ToUpper()));
                    com.Parameters.Add(new SqlParameter("@an", textBox3.Text.Trim().ToUpper()));
                    com.Parameters.Add(new SqlParameter("@shb", textBox4.Text.Trim().ToUpper()));

                    com.Parameters.Add(new SqlParameter("@shn", cityNumbList[comboBox3.SelectedIndex]));

                    try
                    {
                        com.Parameters.Add(new SqlParameter("@sh", supplierListComboBox[comboBox2.SelectedIndex]));
                    }
                    catch (ArgumentOutOfRangeException ed)
                    {
                        MessageBox.Show("Selected city has no suppliers.");
                        return;
                    }

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

        public void fillSupplierComboBox()
        {
            con = new SqlConnection(conStr);

            SqlCommand com = new SqlCommand();
            con.Open();

            try
            {
                if (comboBox2.Items.Count > 0)
                {
                    comboBox2.Items.Clear();
                    supplierListComboBox.Clear();
                }

                com.Connection = con;
                com.CommandText = "select shop_no, name, cnumb from shop where cnumb = @cno order by name";

                int cpara = comboBox1.SelectedIndex;


                com.Parameters.Add(new SqlParameter("@cno", cityNumbList[cpara]));

                com.CommandType = System.Data.CommandType.Text;

                SqlDataReader dr = com.ExecuteReader();

                bool hasValues = false;

                while (dr.Read())
                {
                    comboBox2.Items.Add(dr["name"].ToString());

                    supplierListComboBox.Add(int.Parse(dr["shop_no"].ToString()));
                    hasValues = true;
                }
                if (hasValues)
                    comboBox2.SelectedIndex = 0;
                else
                {
                    comboBox2.SelectedIndex = -1;

                    textBox1.Text = "";

                    textBox2.Text = "";

                    textBox3.Text = "";
                }


            }
            catch (SqlException exception)
            {
                MessageBox.Show("Error: " + exception.Message);
            }
            finally
            {
                con.Dispose();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillSupplierComboBox();

            comboBox3.SelectedIndex = comboBox1.SelectedIndex;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            con = new SqlConnection(conStr);

            SqlCommand com = new SqlCommand();
            con.Open();

            try
            {

                com.Connection = con;
                com.CommandText = "select * from shop where shop_no = @sno";

                int cpara = comboBox2.SelectedIndex;

                com.Parameters.Add(new SqlParameter("@sno", supplierListComboBox[cpara]));

                com.CommandType = System.Data.CommandType.Text;

                SqlDataReader dr = com.ExecuteReader();

                while (dr.Read())
                {
                    textBox1.Text = dr["name"].ToString();

                    textBox2.Text = dr["account_title"].ToString();

                    textBox3.Text = dr["account_no"].ToString();

                    textBox4.Text = dr["sh_bank"].ToString();
                }

            }
            catch (SqlException exception)
            {
                MessageBox.Show("Error: " + exception.Message);
            }
            finally
            {
                con.Dispose();
            }
        }
    }
}
