using Accounting.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Accounting.App
{
    public partial class frmCustomers : Form
    {
        public frmCustomers()
        {
            InitializeComponent();
        }

        private void frmCustomers_Load(object sender, EventArgs e)
        {
            BindGrid();
        }
        void BindGrid()
        {
            using (UnitOfWork db=new UnitOfWork())
            {
                dgvCustomers.AutoGenerateColumns = false;
                dgvCustomers.DataSource = db.CustomerRepository.GetAllCustomers();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                dgvCustomers.DataSource = db.CustomerRepository.GetCustomerByFilter(txtSearch.Text);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindGrid();
            txtSearch.Text = "";
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.CurrentRow!=null)
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    string Name = dgvCustomers.CurrentRow.Cells[1].Value.ToString();
                    if (RtlMessageBox.Show($"ایا از حذف {Name}  مطمئن هستید","توجه",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)==DialogResult.Yes)
                    {
                        int customerId = int.Parse(dgvCustomers.CurrentRow.Cells[0].Value.ToString());
                        db.CustomerRepository.DeleteCustomer(customerId);
                        db.Save();
                        BindGrid();
                    }
                  
                }
            }
            else
            {
                RtlMessageBox.Show("لطفا شخصی را انتخاب کنید");
            }
        }

        private void btnAddNewCustomer_Click(object sender, EventArgs e)
        {
            frmAddOrEditeCustomers frmAdd = new frmAddOrEditeCustomers();
            if (frmAdd.ShowDialog()==DialogResult.OK)
            {
                BindGrid();
            }
        }

        private void btnEditCustomer_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.CurrentRow!=null)
            {
                int customerId = int.Parse(dgvCustomers.CurrentRow.Cells[0].Value.ToString());
                frmAddOrEditeCustomers frmEdite = new frmAddOrEditeCustomers();
                frmEdite.customerId = customerId;
                if (frmEdite.ShowDialog()==DialogResult.OK)
                {
                    BindGrid();
                }

            }
        }
    }
}
