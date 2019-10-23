using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HRMSWin
{
    public partial class Form1 : Form
    {
        Employee emp = new Employee();
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnCnl_Click(object sender, EventArgs e)
        {
            clear();
        }
        void clear()
        {
            txtFN.Text = txtLN.Text = txtEmail.Text = txtContact.Text = txtAdd.Text = "";
            btnSave.Text = "Save";
            btnDlt.Enabled = false;
            emp.Id = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            clear();
            PopulateData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            emp.FirstName = txtFN.Text.Trim();
            emp.LastName = txtLN.Text.Trim();
            emp.Contact = txtContact.Text.Trim();
            emp.Email = txtEmail.Text.Trim();
            emp.Address = txtAdd.Text.Trim();
            emp.DateofBirth =  Convert.ToDateTime(txtDob.Text);
            if (emp.FirstName == "" || emp.LastName == "" || emp.Contact == "" || emp.Email=="" || emp.Address == "")
            {
                MessageBox.Show("Please fill all the field");
            }
            else
            {
                using (HRMDBEntities db = new HRMDBEntities())
                {
                    if (emp.Id == 0)    //insert
                    {
                        Random ran = new Random();
                        int rand = 100 + ran.Next(100, 999999);
                        emp.Id = rand;
                        db.Employees.Add(emp);
                    }
                        
                    else  //Update
                    {
                        db.Entry(emp).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                }
                clear();
                PopulateData();
                MessageBox.Show("Submitted Successfully");
            }
           
        }

        void PopulateData()
        {
            using (HRMDBEntities db = new HRMDBEntities())
            {
                dgvEmp.DataSource = db.Employees.ToList<Employee>();
            }
        }

        private void btnDlt_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure to delete this record ?", "EF CRUD OPERATION", MessageBoxButtons.YesNo)==DialogResult.Yes)
            {
                using (HRMDBEntities db = new HRMDBEntities())
                {
                    var entry = db.Entry(emp);
                    if (entry.State == EntityState.Detached)
                        db.Employees.Attach(emp);
                    db.Employees.Remove(emp);
                    db.SaveChanges();
                    PopulateData();
                    clear();
                    MessageBox.Show("Deleted Successfully");
                }
            }
        }

        private void dgvEmp_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dgvEmp.CurrentRow.Index!=-1)
            {
                emp.Id = Convert.ToInt32(dgvEmp.CurrentRow.Cells["Id"].Value);
                using (HRMDBEntities db = new HRMDBEntities())
                {
                    emp = db.Employees.Where(x => x.Id == emp.Id).FirstOrDefault();
                    txtFN.Text = emp.FirstName;
                    txtLN.Text = emp.LastName;
                    txtContact.Text = emp.Contact;
                    txtEmail.Text = emp.Email;
                    txtAdd.Text = emp.Address;
                    txtDob.Text = emp.DateofBirth.Value.ToShortDateString();
                    btnSave.Text = "Update";
                    btnDlt.Enabled = true;
                }
            }
        }
    }
}
