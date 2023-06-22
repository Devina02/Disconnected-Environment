using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Disconnected_Environment
{
    public partial class Form2 : Form
    {
        
        private string stringConnection = " data source = LAPTOP-DP3PQGGM\\DEPIIII;" +
        "database=Activity6;User ID=sa;Password=123";
        private SqlConnection koneksi;
        public Form2()
        {
            InitializeComponent();
            koneksi = new SqlConnection(stringConnection);
            refreshForm();
        }

        private void Form2_Load(object sender, EventArgs e)
        {}
        private void refreshForm()
        {
            nmp.Text = "";
            nmp.Enabled = false;
            btnSave.Enabled = false;
            btnClear.Enabled = false;
        }
        private void dataGridView()
        {
            koneksi.Open();
            string str = "select nama_prodi from dbo.Prodi";
            SqlDataAdapter da = new SqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            koneksi.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            nmp.Enabled = true;
            btnSave.Enabled = true;
            btnClear.Enabled = true;
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            string nmProdi = nmp.Text.Trim();
            string idProdi = textBox1.Text.Trim();
            if (nmProdi == "")
            {
                MessageBox.Show("Masukan Nama Prodi", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (idProdi == "")
            {
                MessageBox.Show("Masukan Kode Prodi", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                koneksi.Open();
                string str = "insert into dbo.Prodi (ID_Prodi, nama_prodi)" + "values(@ID_Prodi, @nama_prodi)";
                SqlCommand cmd = new SqlCommand(str, koneksi);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@ID_Prodi", idProdi));
                cmd.Parameters.Add(new SqlParameter("@nama_prodi", nmProdi));
                cmd.ExecuteNonQuery();

                koneksi.Close();
                MessageBox.Show("Data Berhasil Disimpan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView();
                refreshForm();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            refreshForm();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            dataGridView();
            btnOpen.Enabled = false;
        }

        private void nmp_TextChanged(object sender, EventArgs e)
        {

        }
    }        
}

