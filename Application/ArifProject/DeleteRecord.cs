using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArifProject
{
    public partial class DeleteRecord : Form
    {
        SqlConnection con;
        string conStr = "Data Source=SOJHARO\\SQLEXPRESS;Initial Catalog=TestDB;User ID=sa;Password=sojharo";//"Data Source=SOJHARO\\SQLEXPRESS;AttachDbFilename=DB\\TestDb.mdf;Initial Catalog=TestDB;Integrated Security=True";

        List<Int32> cityNumbList = new List<Int32>();
        List<Int32> supplierListComboBox = new List<Int32>();
        List<Int32> supplierListBoxList = new List<Int32>();

        public DeleteRecord()
        {
            InitializeComponent();

            fillComboBox();
        }

        public void fillComboBox()
        {
            con = new SqlConnection(conStr);

            SqlCommand com = new SqlCommand();
            con.Open();

            try
            {
                if (comboBox1.Items.Count > 0)
                {
                    comboBox1.Items.Clear();
                    cityNumbList.Clear();
                }

                com.Connection = con;
                com.CommandText = "select * from shop_city order by cname";
                com.CommandType = System.Data.CommandType.Text;
                SqlDataReader dr = com.ExecuteReader();

                bool hasValues = false;
                while (dr.Read())
                {

                    cityNumbList.Add(int.Parse(dr["cnumb"].ToString()));

                    comboBox1.Items.Add(dr["cname"].ToString());

                    hasValues = true;
                }

                if (hasValues)
                    comboBox1.SelectedIndex = 0;
                else
                    comboBox1.SelectedIndex = -1;

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
                    comboBox2.Items.Clear();
                    listBox1.Items.Clear();
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
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillRecordComboBox();
        }

        public void fillRecordComboBox()
        {
            con = new SqlConnection(conStr);

            if (listBox1.Items.Count > 0)
            {
                listBox1.Items.Clear();
                supplierListBoxList.Clear();
            }

            SqlCommand com = new SqlCommand();

            com.CommandText = "select transactionvoucher.voucher_numb, convert(varchar(10), tran_date, 120) as ddate " +
                                "from transactionvoucher, shop, vouch_shop where " +
                                "vouch_shop.shop_no = shop.shop_no " +
                                "and vouch_shop.voucher_numb = transactionvoucher.voucher_numb " +
                                "and shop.shop_no = @shpno";

            com.Parameters.Add(new SqlParameter("@shpno", supplierListComboBox[comboBox2.SelectedIndex]));

            com.CommandType = System.Data.CommandType.Text;


            con.Open();
            try
            {
                com.Connection = con;

                SqlDataReader dr = com.ExecuteReader();

                while (dr.Read())
                {
                    listBox1.Items.Add(dr["ddate"].ToString());

                    supplierListBoxList.Add(int.Parse(dr["voucher_numb"].ToString()));
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            con = new SqlConnection(conStr);

            SqlCommand com = new SqlCommand();

            com.CommandText = "select bill_numb, cheque_numb, amount_payable, amount_paid, convert(varchar(10), tran_date, 120) as ddate, "
            +"(amount_paid - amount_payable) as Balance, discount from transactionvoucher where voucher_numb = @vno";

            if (listBox1.SelectedIndex != -1)
            {

                com.Parameters.Add(new SqlParameter("@vno", supplierListBoxList[listBox1.SelectedIndex]));

                con.Open();
                try
                {
                    com.Connection = con;

                    SqlDataReader dr = com.ExecuteReader();

                    while (dr.Read())
                    {
                        textBox1.Text = dr["bill_numb"].ToString();

                        textBox2.Text = dr["cheque_numb"].ToString();

                        textBox3.Text = dr["amount_payable"].ToString();

                        textBox4.Text = dr["amount_paid"].ToString();

                        textBox5.Text = dr["ddate"].ToString();

                        textBox6.Text = dr["discount"].ToString();
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Kindly select the record from the list first.");
            }
            else
            {
                con = new SqlConnection(conStr);
                con.Open();
                try
                {
                    SqlCommand com1 = new SqlCommand();

                    com1.Connection = con;
                    com1.CommandText = "delete from vouch_shop where voucher_numb = @vno";

                    com1.Parameters.Add(new SqlParameter("@vno", supplierListBoxList[listBox1.SelectedIndex]));

                    com1.ExecuteNonQuery();

                    SqlCommand com = new SqlCommand();

                    com.Connection = con;
                    com.CommandText = "delete from transactionvoucher where voucher_numb = @vno";

                    com.Parameters.Add(new SqlParameter("@vno", supplierListBoxList[listBox1.SelectedIndex]));

                    com.ExecuteNonQuery();

                    fillRecordComboBox();

                    textBox1.Text = "";

                    textBox2.Text = "";

                    textBox3.Text = "";

                    textBox4.Text = "";

                    textBox5.Text = "";

                    textBox6.Text = "";

                    MessageBox.Show("Record Successfully Deleted.");
                    
                    //this.Close();
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Kindly select the record from the list first.");
            }
            else
            {
                con = new SqlConnection(conStr);
                con.Open();
                try
                {
                    SqlCommand com1 = new SqlCommand();

                    com1.Connection = con;
                    com1.CommandText = "update transactionvoucher set bill_numb = @bnum, "+
                    "cheque_numb = @cnum, amount_payable = @apay, amount_paid = @apaid, tran_date "+
                    "= CONVERT(datetime, @tdate, 120), discount = @dis where voucher_numb = @vno";

                    com1.Parameters.Add(new SqlParameter("@bnum", textBox1.Text.Trim().ToUpper()));
                    com1.Parameters.Add(new SqlParameter("@cnum", textBox2.Text.Trim().ToUpper()));
                    try
                    {
                        com1.Parameters.Add(new SqlParameter("@apay", Int32.Parse(textBox4.Text.Trim())));
                    }
                    catch (FormatException ed)
                    {
                        MessageBox.Show("Kindly enter numerical value in Amount Payable Field");
                        return;
                    }
                    try
                    {
                        com1.Parameters.Add(new SqlParameter("@apaid", Int32.Parse(textBox3.Text.Trim())));
                    }
                    catch (FormatException ed)
                    {
                        MessageBox.Show("Kindly enter numerical value in Amount Paid Field");
                        return;
                    }
                    com1.Parameters.Add(new SqlParameter("@tdate", textBox5.Text.Trim().ToUpper()));
                    com1.Parameters.Add(new SqlParameter("@vno", supplierListBoxList[listBox1.SelectedIndex]));
                    try
                    {
                        com1.Parameters.Add(new SqlParameter("@dis", Int32.Parse(textBox6.Text.Trim())));
                    }
                    catch (FormatException ed)
                    {
                        MessageBox.Show("Kindly enter numerical value in Discount Field");
                        return;
                    }

                    com1.ExecuteNonQuery();

                    fillRecordComboBox();

                    MessageBox.Show("Record Successfully Updated.");

                    //this.Close();
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
