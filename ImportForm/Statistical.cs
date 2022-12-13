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
    public partial class Statistical : Form
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString);
        public Statistical()
        {
            InitializeComponent();
        }

        private void cbChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            String query ="";
            if (cbChoice.SelectedIndex == 0)
            {
                query = "select p.id, p.name,p.quantity, sum(id.quantity) as 'number of product import' from import_detail id, products p where id.id_product = p.id group by  p.id, p.name,p.quantity";
            }
            else if (cbChoice.SelectedIndex == 1)
            {
                query = "select p.id, p.name,p.quantity, sum(s.quantity) as 'number of selling products' from selled s,products p where p.id = s.product_id group by p.id, p.name,p.quantity";
            }
            else if (cbChoice.SelectedIndex == 2)
            {
                query = "select p.id, p.name,p.quantity, sum(s.quantity) as 'number of selling products' from selled s,products p where p.id = s.product_id group by p.id, p.name,p.quantity having sum(s.quantity) = (select  max(max_sell.sum_quantity) as 'sellest_product' from (select p.id, p.name,p.quantity, sum(s.quantity) as sum_quantity from selled s,products p where p.id = s.product_id group by  p.id, p.name,p.quantity) as max_sell)";
            }
            else
            {
                query = "";
            }
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            conn.Open();
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            conn.Close();
        }
    }
}
