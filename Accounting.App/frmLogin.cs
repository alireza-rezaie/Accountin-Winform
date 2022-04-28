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
using ValidationComponents;

namespace Accounting.App
{
    public partial class frmLogin : Form
    {
        public bool IsEdite = false;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (BaseValidator.IsFormValid(this.components))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    if (IsEdite)
                    {
                        var Login = db.LoginRepository.Get().First();
                        Login.UserName = txtUserName.Text;
                        Login.Password = txtPassword.Text;
                        db.LoginRepository.Update(Login);
                        db.Save();
                        Application.Restart();
                    }
                    else
                    {
                        if (db.LoginRepository.Get(l => l.UserName == txtUserName.Text && l.Password == txtPassword.Text).Any())
                        {
                            DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            RtlMessageBox.Show("کاربری یافت نشد");
                        }
                    }
               
                }
            }

        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if (IsEdite)
            {
                this.Text = "تنظیمات ورود برنامه";
                btnLogin.Text = "ذخیره تغییرات";
                using (UnitOfWork db = new UnitOfWork())
                {
                    var Login = db.LoginRepository.Get().First();
                    txtUserName.Text = Login.UserName;
                    txtPassword.Text = Login.Password;
                }
            }
        }
    }
}
