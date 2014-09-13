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
    public partial class Form1 : Form
    {
        SqlConnection con;
        string conStr = "Data Source=SOJHARO\\SQLEXPRESS;Initial Catalog=TestDB;User ID=sa;Password=sojharo";//"Data Source=SOJHARO\\SQLEXPRESS;AttachDbFilename=DB\\TestDb.mdf;Initial Catalog=TestDB;Integrated Security=True";
            //"Data Source=SOJHARO\\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True";

        List<Int32> cityNumbList = new List<Int32>();
        List<Int32> supplierListBoxList = new List<Int32>();
        List<Int32> supplierListComboBox = new List<Int32>();

        public Form1()
        {
            InitializeComponent();

            fillComboBoxes();

            try
            {
                comboBox6.SelectedIndex = 0;
            }
            catch (System.ArgumentOutOfRangeException ex)
            {

            }
            try
            {
                comboBox3.SelectedIndex = 0;
            }
            catch (System.ArgumentOutOfRangeException ex)
            {

            }
            try
            {
                comboBox1.SelectedIndex = 0;
            }
            catch (System.ArgumentOutOfRangeException ex)
            {

            }

            textBox5.Text = "400";
            textBox6.Text = "10000";
            

            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker2.Value = DateTime.Today;
            dateTimePicker3.Value = DateTime.Today;

            try
            {
                comboBox4.SelectedIndex = 0;
            }
            catch (System.ArgumentOutOfRangeException ex)
            {

            }
            try 
            {
                listBox1.SelectedIndex = 0;
            }
            catch (System.ArgumentOutOfRangeException ex)
            {

            }
            try
            {
                comboBox2.SelectedIndex = 0;
            }
            catch(System.ArgumentOutOfRangeException ex)
            {
                
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            con = new SqlConnection(conStr);

            SqlCommand com = new SqlCommand();

            if (comboBox3.SelectedIndex == 0)
            {
                com.CommandText = "select cname as 'City', sum(amount_payable) as 'Amount Payable', " +
                    "sum(amount_paid) as 'Amount Paid', sum(amount_paid) - sum(amount_payable) as 'Balance' " +
                    "from transactionvoucher, vouch_shop, shop, shop_city where " +
                    "vouch_shop.voucher_numb = transactionvoucher.voucher_numb " +
                    "and vouch_shop.shop_no = shop.shop_no and shop.cnumb = shop_city.cnumb group by cname order by cname";
            }
            else if (comboBox3.SelectedIndex == 1)
            {
                com.CommandText = "select name as 'Name', cname as 'City Name', sum(amount_payable) as 'Amount Payable', " +
                    "sum(amount_paid) as 'Amount Paid', sum(amount_paid) - sum(amount_payable) as 'Balance' "+
                    "from transactionvoucher, vouch_shop, shop, shop_city where "+
                    "vouch_shop.voucher_numb = transactionvoucher.voucher_numb "+
                    "and vouch_shop.shop_no = shop.shop_no and shop_city.cnumb = shop.cnumb "+
                    "group by name, cname order by name";
            }

            con.Open();
            try
            {
                com.Connection = con;
                
                com.CommandType = System.Data.CommandType.Text;
                
                SqlDataAdapter da = new SqlDataAdapter(com);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt.DefaultView;
                

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

        public void fillComboBoxes()
        {
            con = new SqlConnection(conStr);

            SqlCommand com = new SqlCommand();
            con.Open();

            try
            {
                if (comboBox4.Items.Count > 0)
                {
                    comboBox1.Items.Clear();
                    comboBox4.Items.Clear();
                    cityNumbList.Clear();
                }

                com.Connection = con;
                com.CommandText = "select * from shop_city order by cname";
                com.CommandType = System.Data.CommandType.Text;
                SqlDataReader dr = com.ExecuteReader();

                bool hasValues = false;
                while (dr.Read())
                {
                    comboBox4.Items.Add(dr["cname"].ToString());

                    cityNumbList.Add(int.Parse(dr["cnumb"].ToString()));

                    comboBox1.Items.Add(dr["cname"].ToString());
                    hasValues = true;
                }
                if (hasValues)
                {
                    comboBox1.SelectedIndex = 0;
                    comboBox4.SelectedIndex = 0;
                }
                else
                {
                    comboBox1.SelectedIndex = -1;
                    comboBox4.SelectedIndex = -1;
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

        public void fillList()
        {
            con = new SqlConnection(conStr);

            SqlCommand com = new SqlCommand();
            con.Open();

            try
            {
                if (listBox1.Items.Count>0)
                {
                    listBox1.Items.Clear();
                    supplierListBoxList.Clear();
                }

                com.Connection = con;
                com.CommandText = "select shop_no, name, cnumb from shop where cnumb = @cno order by name";

                int cpara= comboBox4.SelectedIndex;


                com.Parameters.Add(new SqlParameter("@cno", cityNumbList[cpara]));

                com.CommandType = System.Data.CommandType.Text;

                SqlDataReader dr = com.ExecuteReader();


                while (dr.Read())
                {
                    listBox1.Items.Add(dr["name"].ToString());

                    supplierListBoxList.Add(int.Parse(dr["shop_no"].ToString()));
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

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillList();
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
                if(hasValues)
                    comboBox2.SelectedIndex = 0;
                else
                    comboBox2.SelectedIndex = -1;

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

            com.CommandText = "select tran_date as 'Date', bill_numb as 'Bill Number', cheque_numb as 'Bank', " +
                                "amount_payable as 'Amount Payable', amount_paid as 'Amount Paid', (amount_paid - amount_payable) as 'Balance', " +
                                "discount as 'Discount' " +
                                "from transactionvoucher, shop, vouch_shop where "+
                                "vouch_shop.shop_no = shop.shop_no "+
                                "and vouch_shop.voucher_numb = transactionvoucher.voucher_numb "+
                                "and shop.shop_no = @shpno";

            com.Parameters.Add(new SqlParameter("@shpno", supplierListBoxList[listBox1.SelectedIndex]));

            con.Open();
            try
            {
                com.Connection = con;

                com.CommandType = System.Data.CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter(com);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);
                dataGridView2.DataSource = dt.DefaultView;


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

        private void button11_Click(object sender, EventArgs e)
        {
            con = new SqlConnection(conStr);

            SqlCommand com = new SqlCommand();

            com.CommandText = "select name as 'Supplier', bill_numb as 'Bill Number', cheque_numb as 'Bank', " +
                                   "amount_payable as 'Amount Payable', amount_paid as 'Amount Paid', (amount_paid - amount_payable) as 'Balance', " +
                                    "discount as 'Discount', tran_date as 'Transaction Date' " +
                                   "from shop, transactionvoucher, vouch_shop where " +
                                   "(vouch_shop.shop_no = shop.shop_no " +
                                   "and vouch_shop.voucher_numb = transactionvoucher.voucher_numb) " +
                                   "";
            com.CommandText += "and tran_date between @date1 and @date2 ";

            if (textBox5.Text.Equals("") || textBox6.Text.Equals(""))
            {
                com.Parameters.Add(new SqlParameter("@date1", dateTimePicker2.Value));//.ToString("yyyy-MM-dd")));
                com.Parameters.Add(new SqlParameter("@date2", dateTimePicker3.Value));//.ToString("yyyy-MM-dd")));
            }
            else
            {

                com.CommandText += "and ";

                if (comboBox6.SelectedIndex == 0)
                    com.CommandText += "((amount_paid - amount_payable) between ";
                else if (comboBox6.SelectedIndex == 1)
                    com.CommandText += "(amount_paid between ";
                else if (comboBox6.SelectedIndex == 2)
                    com.CommandText += "(amount_payable between ";

                com.CommandText += "@amount1 and @amount2) order by name";

                com.Parameters.Add(new SqlParameter("@date1", dateTimePicker2.Value));//.ToString("yyyy-MM-dd")));
                com.Parameters.Add(new SqlParameter("@date2", dateTimePicker3.Value));//.ToString("yyyy-MM-dd")));

                try
                {
                    com.Parameters.Add(new SqlParameter("@amount1", Int32.Parse(textBox5.Text)));
                    com.Parameters.Add(new SqlParameter("@amount2", Int32.Parse(textBox6.Text)));
                }
                catch (FormatException ed)
                {
                    MessageBox.Show("Kindly use numbers in text boxes.");
                    return ;
                }

            }

            con.Open();
            try
            {
                com.Connection = con;

                com.CommandType = System.Data.CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter(com);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);
                dataGridView2.DataSource = dt.DefaultView;


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

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox8.Text = "";
            textBox7.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            textBox12.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.Equals("") && textBox4.Text.Equals("") && textBox10.Text.Equals(""))
            {
                con = new SqlConnection(conStr);
                con.Open();
                SqlCommand com3 = new SqlCommand();

                com3.Connection = con;
                com3.CommandText = "select sum(amount_paid)-sum(amount_payable) as Balance " +
                "from transactionvoucher, vouch_shop " +
                "where transactionvoucher.voucher_numb = vouch_shop.voucher_numb " +
                "and vouch_shop.shop_no = @shp";

                com3.Parameters.Add(new SqlParameter("@shp", supplierListComboBox[comboBox2.SelectedIndex]));

                com3.CommandType = System.Data.CommandType.Text;

                SqlDataReader dr2 = com3.ExecuteReader();

                while (dr2.Read())
                {
                    textBox7.Text = dr2.GetValue(0).ToString();
                }

                dr2.Close();

                con.Close();
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
                    comTemp.CommandText = "select voucher_numb from transactionvoucher";
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
                    com.CommandText = "insert into transactionvoucher (voucher_numb, bill_numb, cheque_numb, amount_payable, " +
                                        "amount_paid, tran_date, discount) values (@vno, @bill, @chq, @payb, @ampaid, @date, @dis)";

                    com.Parameters.Add(new SqlParameter("@vno", newKeyNo));
                    com.Parameters.Add(new SqlParameter("@bill", textBox1.Text.Trim().ToUpper()));
                    com.Parameters.Add(new SqlParameter("@chq", textBox2.Text.Trim().ToUpper()));
                    if (textBox4.Text.Equals(""))
                    {
                        com.Parameters.Add(new SqlParameter("@payb", Int32.Parse("0")));
                    }
                    else
                    {
                        try
                        {
                            com.Parameters.Add(new SqlParameter("@payb", Int32.Parse(textBox4.Text.Trim())));
                        }
                        catch (FormatException ed)
                        {
                            MessageBox.Show("Kindly use numerical value in Amount Payable Field");
                            return;
                        }
                    }
                    if (textBox3.Text.Equals(""))
                    {
                        com.Parameters.Add(new SqlParameter("@ampaid", Int32.Parse("0")));
                    }
                    else
                    {
                        try
                        {
                            com.Parameters.Add(new SqlParameter("@ampaid", Int32.Parse(textBox3.Text.Trim())));
                        }
                        catch (FormatException ed)
                        {
                            MessageBox.Show("Kindly use numerical value in Amount Paid Field");
                            return;
                        }
                    }
                    com.Parameters.Add(new SqlParameter("@date", dateTimePicker1.Value));
                    if (textBox10.Text.Equals(""))
                    {
                        com.Parameters.Add(new SqlParameter("@dis", Int32.Parse("0")));
                    }
                    else
                    {
                        try
                        {
                            com.Parameters.Add(new SqlParameter("@dis", Int32.Parse(textBox10.Text.Trim())));
                        }
                        catch (FormatException ed)
                        {
                            MessageBox.Show("Kindly use numerical value in Discount Field");
                            return;
                        }
                    }

                    com.ExecuteNonQuery();
                    SqlCommand com2 = new SqlCommand();

                    com2.Connection = con;
                    com2.CommandText = "insert into vouch_shop (vouch_shop.voucher_numb, vouch_shop.shop_no) values (@v, @s)";

                    com2.Parameters.Add(new SqlParameter("@v", newKeyNo));
                    try
                    {
                        com2.Parameters.Add(new SqlParameter("@s", supplierListComboBox[comboBox2.SelectedIndex]));
                    }
                    catch (ArgumentOutOfRangeException ed)
                    {
                        MessageBox.Show("Selected city has no supplier. Kindly create one before making records.");
                        return;
                    }

                    com2.ExecuteNonQuery();
                    
                    SqlCommand com3 = new SqlCommand();

                    com3.Connection = con;
                    com3.CommandText = "select sum(amount_paid)-sum(amount_payable) as Balance "+
                    "from transactionvoucher, vouch_shop "+
                    "where transactionvoucher.voucher_numb = vouch_shop.voucher_numb "+
                    "and vouch_shop.shop_no = @shp";

                    com3.Parameters.Add(new SqlParameter("@shp", supplierListComboBox[comboBox2.SelectedIndex]));

                    com3.CommandType = System.Data.CommandType.Text;

                    SqlDataReader dr2 = com3.ExecuteReader();

                    while (dr2.Read())
                    {
                        textBox7.Text = dr2.GetValue(0).ToString();
                    }

                    dr2.Close();

                    SqlCommand com4 = new SqlCommand();

                    com4.Connection = con;

                    com4.CommandText = "select account_title, account_no, sh_bank " +
                    "from shop " +
                    "where shop_no = @shp";

                    com4.Parameters.Add(new SqlParameter("@shp", supplierListComboBox[comboBox2.SelectedIndex]));

                    com4.CommandType = System.Data.CommandType.Text;

                    SqlDataReader dr3 = com4.ExecuteReader();

                    while (dr3.Read())
                    {
                        textBox8.Text = dr3.GetValue(0).ToString();
                        textBox9.Text = dr3.GetValue(1).ToString();
                        textBox12.Text = dr3.GetValue(2).ToString();
                    }

                    dr3.Close();

                    SqlCommand com5 = new SqlCommand();

                    com5.Connection = con;

                    com5.CommandText = "select sum(discount) as \"Total Discount\" " +
                    "from transactionvoucher, vouch_shop " +
                    "where transactionvoucher.voucher_numb = vouch_shop.voucher_numb " +
                    "and vouch_shop.shop_no = @shp";

                    com5.Parameters.Add(new SqlParameter("@shp", supplierListComboBox[comboBox2.SelectedIndex]));

                    com5.CommandType = System.Data.CommandType.Text;

                    SqlDataReader dr4 = com5.ExecuteReader();

                    while (dr4.Read())
                    {
                        textBox11.Text = dr4.GetValue(0).ToString();
                    }

                    dr4.Close();

                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox10.Text = "";

                    MessageBox.Show("Information successfully stored.");
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

        // Create New City Button
        private void button3_Click(object sender, EventArgs e)
        {
            NewCity city = new NewCity();
            city.Show();
            fillComboBoxes();
        }

        // Refresh
        private void button12_Click(object sender, EventArgs e)
        {
            fillComboBoxes();
            fillSupplierComboBox();
            fillList();
        }

        //Create new Supplier Button
        private void button6_Click(object sender, EventArgs e)
        {
            NewSupplier supplier = new NewSupplier();
            supplier.Show();
            fillComboBoxes();
        }

        // Edit City Button
        private void button5_Click(object sender, EventArgs e)
        {
            EditCity editC = new EditCity();
            editC.Show();
            fillComboBoxes();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            EditSupplier editS = new EditSupplier();
            editS.Show();
            fillComboBoxes();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            DeleteRecord delRec = new DeleteRecord();
            delRec.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'TestDBDataSet1.shop' table. You can move, or remove it, as needed.
            this.shopTableAdapter.Fill(this.TestDBDataSet1.shop);
            // TODO: This line of code loads data into the 'TestDBDataSet.view2' table. You can move, or remove it, as needed.
            this.view2TableAdapter.Fill(this.TestDBDataSet.view2);

            reportViewer1.RefreshReport();
            reportViewer2.RefreshReport();
            reportViewer3.RefreshReport();
         
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'TestDBDataSet1.shop' table. You can move, or remove it, as needed.
            this.shopTableAdapter.Fill(this.TestDBDataSet1.shop);
            // TODO: This line of code loads data into the 'TestDBDataSet.view2' table. You can move, or remove it, as needed.
            this.view2TableAdapter.Fill(this.TestDBDataSet.view2);

            this.reportViewer1.RefreshReport();
            this.reportViewer2.RefreshReport();
            this.reportViewer3.RefreshReport();
        }

        
        
    }
}
