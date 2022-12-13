using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImportForm
{
    public partial class Exporting : Form
    {
        public Exporting(string Agent, string Phone, string Address, string OrderDate, string OrderStatus, string Payment, string Total, string Method, DataGridView dt)
        {
            InitializeComponent();
            txtAgent.Text = Agent;
            txtPhone.Text = Phone;
            txtAddress.Text = Address;
            txtOrderDate.Text = OrderDate;
            txtPayment.Text = Payment;
            txtTotal.Text = Total;
            txtMethod.Text = Method;
            txtOrderStatus.Text = OrderStatus;
            dataGridView1.DataSource = dt.DataSource;
        }

        private void Exporting_Load(object sender, EventArgs e)
        {

        }
    }
}
