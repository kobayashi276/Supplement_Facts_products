using System;
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
    public partial class Export : Form
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString);
        String indexOrder;
        String indexAgent;

        public Export()
        {
            InitializeComponent();
            loadDataGridView();
        }

        private void loadDataGridView()
        {
            String query = "select * from agent";
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
            dataGridViewAgent.DataSource = ds.Tables[0];
            conn.Close();
        }

        private void dataGridViewAgent_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            txtAgent.Text = dataGridViewAgent.Rows[e.RowIndex].Cells["agent_name"].Value.ToString();
            txtAddress.Text = dataGridViewAgent.Rows[e.RowIndex].Cells["address"].Value.ToString();
            txtPhone.Text = dataGridViewAgent.Rows[e.RowIndex].Cells["phone"].Value.ToString();
            if (e.RowIndex < dataGridViewAgent.RowCount-1)
            {
                dataGridViewOrderLoad(dataGridViewAgent.Rows[e.RowIndex].Cells["id"].Value.ToString());
            }

        }

        private void dataGridViewOrderLoad(String idAgent)
        {
            indexAgent = idAgent;
            String query = "select * from order_product where agent_id=" + idAgent;
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            conn.Open();
            DataSet ds = new DataSet();
            if (ds != null)
            {
                da.Fill(ds);
                dataGridViewOrders.DataSource = ds.Tables[0];

            }
            conn.Close();
        }

        private void dataGridViewOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.RowIndex == dataGridViewOrders.Rows.Count - 1) return;
            indexOrder = dataGridViewOrders.Rows[e.RowIndex].Cells["id"].Value.ToString();
            if (e.RowIndex == -1) return;
            DateTime dt = (DateTime)dataGridViewOrders.Rows[e.RowIndex].Cells["order_at"].Value;
            txtOrderDate.Text = dt.ToShortDateString();
            txtMethod.Text = dataGridViewOrders.Rows[e.RowIndex].Cells["method_pay"].Value.ToString();
            bool check = (bool)dataGridViewOrders.Rows[e.RowIndex].Cells["status_pay"].Value;
            if (check)
            {
                txtPayment.Text = "Pay Successful";
            }
            else {
                txtPayment.Text = "Not Pay";
            }
            check = (bool)dataGridViewOrders.Rows[e.RowIndex].Cells["status_order"].Value;
            if (check)
            {
                txtOrderStatus.Text = "Delivered";
            }
            else
            {
                txtOrderStatus.Text = "Processing";
            }
            if (e.RowIndex < dataGridViewOrders.RowCount - 1)
            {
                dataGridViewOrderDetails(dataGridViewOrders.Rows[e.RowIndex].Cells["id"].Value.ToString());
            }
        }

        private void dataGridViewOrderDetails(String idOrder)
        {
            String query = "select  p.id, p.name,p.price,p.Units,od.quantity,(p.price * od.quantity) as 'price of product' from order_detail od, order_product op,products p where op.id = od.order_id and p.id = od.product_id and op.id =" + idOrder;
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            conn.Open();
            DataSet ds = new DataSet();
            if (ds != null)
            {
                da.Fill(ds);
                dataGridViewOrderDetail.DataSource = ds.Tables[0];
            }
            query = "select sum(s) as total from (select  (p.price * od.quantity) as s from order_detail od, order_product op,products p where op.id = od.order_id and p.id = od.product_id and op.id =" + idOrder + ") as tableSum ";
            SqlCommand cmd = new SqlCommand(query, conn);
            txtTotal.Text = cmd.ExecuteScalar().ToString();
            conn.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            
            if (txtAgent.Text == "" || txtOrderDate.Text == "")
            {
                MessageBox.Show("Make sure that you fill all information");
            }
            else if (txtOrderStatus.Text=="Delivered" && txtPayment.Text == "Pay Successful")
            {
                MessageBox.Show("This order already done");
            }
            else
            {
                conn.Open();
                for (int i = 0; i < dataGridViewOrderDetail.Rows.Count-1; i++)
                {
                    String q = "update products set quantity=quantity-" + dataGridViewOrderDetail.Rows[i].Cells["quantity"].Value.ToString() + " where name='" + dataGridViewOrderDetail.Rows[i].Cells["name"].Value.ToString() + "'";
                    SqlCommand c = new SqlCommand(q, conn);
                    c.ExecuteNonQuery();
                }
                String query = "update order_product set status_order=1, status_pay=1 where id=" + indexOrder;
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();


                
                for (int i = 0; i < dataGridViewOrderDetail.Rows.Count - 1; i++)
                {
                    query = "insert into selled (product_id, quantity) values (" + dataGridViewOrderDetail.Rows[i].Cells["id"].Value + ", " + dataGridViewOrderDetail.Rows[i].Cells["quantity"].Value + ")";
                    SqlCommand c = new SqlCommand(query, conn);
                    c.ExecuteNonQuery();
                }
                conn.Close();
                dataGridViewOrderLoad(indexAgent);
                Exporting exporting = new Exporting(txtAgent.Text, txtPhone.Text, txtAddress.Text, txtOrderDate.Text, txtOrderStatus.Text, txtPayment.Text, txtTotal.Text, txtMethod.Text, dataGridViewOrderDetail);
                exporting.Show();
            }

        }
    }
}
