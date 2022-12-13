using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImportForm
{
    public partial class Import : Form
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString);
        public Import()
        {
            InitializeComponent();
            loadComboBoxProduct();
            loadDataGridView();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            int n;
            if (txtQuantity.Text == "")
            {
                MessageBox.Show("Plese insert the quantity!");
            }
            else if (!int.TryParse(txtQuantity.Text, out n))
            {
                MessageBox.Show("Please insert the number, not string!");
            }
            else
            {
                conn.Open();
                //Add to import
                String query = "insert into import(created_date) values ('" + DateTime.Today.ToShortDateString() + "')";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                //Get import id just inserted
                query = "select max(id) from import";
                cmd = new SqlCommand(query, conn);
                int idImport = (int)cmd.ExecuteScalar();

                //Get product id
                query = "select id from products where name = '" + cbProduct.Text + "'";
                cmd = new SqlCommand(query, conn);
                int idProduct = (int)cmd.ExecuteScalar();

                ////Get old quantity
                //query = "select quantity from import_detail where id_import_detail=(select max(id_import_detail) from import_detail where id_product =" + idProduct.ToString() + ")";
                //cmd = new SqlCommand(query, conn);
                //int Quantity = 0;
                //if (cmd.ExecuteScalar() == null)
                //{
                //}
                //else
                //{
                //    Quantity = (int)cmd.ExecuteScalar();
                //}


                //Update Quantity
                query = "select quantity from products where id=" + idProduct.ToString();
                cmd = new SqlCommand(query, conn);
                int Quantity = (int)cmd.ExecuteScalar();

                Quantity+=Int32.Parse(txtQuantity.Text);

                query = "update products set quantity=" + Quantity.ToString() + "where id=" + idProduct.ToString();
                cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                //Add to import_detail
                query = "insert into import_detail(id_import, id_product, quantity) values (" + idImport.ToString() + ", " + idProduct.ToString() + ", " + txtQuantity.Text + ")";
                cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Added successfully");
                conn.Close();
                loadDataGridView();
            }
        }

        private void loadComboBoxProduct()
        {
            String query = "select * from products";
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            conn.Open();
            DataSet ds = new DataSet();
            da.Fill(ds);
            cbProduct.DataSource = ds.Tables[0];
            cbProduct.DisplayMember = "name";
            cbProduct.ValueMember = "id";
            conn.Close();
        }

        private void loadDataGridView()
        {
            String query = "select * from products";
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            conn.Open();
            DataSet ds = new DataSet();
            da.Fill(ds);
/*
            ds.Tables[0].Columns.Add("product_name", typeof(string));
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                row["product_name"] = getProductName(row["id"].ToString());
            }
*/
            dataGridView1.DataSource = ds.Tables[0];
            conn.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.RowIndex==dataGridView1.Rows.Count-1) return;
            cbProduct.SelectedIndex = (int)dataGridView1.Rows[e.RowIndex].Cells["id"].Value-1;
        }
    }
}
