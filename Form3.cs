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
    public partial class Form3 : Form
    {
        private string stringConnection = " data source = LAPTOP-DP3PQGGM\\DEPIIII;" +
        "database=Activity6;User ID=sa;Password=123";
        private SqlConnection koneksi;
        private string nim, nama, alamat, jk, prodi;
        private DateTime tgl;
        private BindingSource customerBindingSource;

        private void Form3_Load(object sender, EventArgs e)
        {
            this.mahasiswaTableAdapter.Fill(this.activity6DataSet.mahasiswa);
            koneksi.Open();
            string query = "SELECT nim, nama_mahasiswa, jenis_kelamin, alamat, tgl_lahir, id_prodi FROM mahasiswa";
            SqlDataAdapter adapter = new SqlDataAdapter(query, koneksi);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            customerBindingSource = new BindingSource();


            koneksi.Close();

        }

        private void clearBinding()
        {
            this.txtNIM.DataBindings.Clear();
            this.txtNama.DataBindings.Clear();
            this.txtAlamat.DataBindings.Clear();
            this.cbxJenisKelamin.DataBindings.Clear();
            this.dtTanggalLahir.DataBindings.Clear();
            this.cbxProdi.DataBindings.Clear();
        }
        private void refreshform()
        {
            txtNIM.Enabled = true;
            txtNama.Enabled = true;
            cbxJenisKelamin.Enabled = true;
            txtAlamat.Enabled = true;
            dtTanggalLahir.Enabled = true;
            cbxProdi.Enabled = true;
            btnAdd.Enabled = true;
            btnSave.Enabled = true;
            btnClear.Enabled = true;
            clearBinding();
        }
        public Form3()
        {
            InitializeComponent();
            koneksi = new SqlConnection(stringConnection);
            this.bnMahasiswa.BindingSource = this.customerBindingSource;
            refreshform();
        }

        private void bnMahasiswa_RefreshItems(object sender, EventArgs e)
        {

        }

        private void cbxProdi_SelectedIndexChanged(object sender, EventArgs e)
        {
            koneksi.Open();
            string str = "select nama_prodi from dbo.prodi";
            SqlCommand cmd = new SqlCommand(str, koneksi);
            SqlDataAdapter da = new SqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.ExecuteReader();
            koneksi.Close();
            cbxProdi.DisplayMember = "nama_prodi";
            cbxProdi.ValueMember = "id_prodi";
            cbxProdi.DataSource = ds.Tables[0];
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtNIM.Text = "";
            txtNama.Text = "";
            txtAlamat.Text = "";
            dtTanggalLahir.Value = DateTime.Today;
            txtNIM.Enabled = true;
            txtNama.Enabled = true;
            cbxJenisKelamin.Enabled = true;
            txtAlamat.Enabled = true;
            dtTanggalLahir.Enabled = true;
            cbxProdi.Enabled = true;
            ProdiCbx();
            btnSave.Enabled = true;
            btnClear.Enabled = true;
            btnAdd.Enabled = false;
        }

        private void dtTanggalLahir_ValueChanged(object sender, EventArgs e)
        {

        }

        private void ProdiCbx()
        {
            koneksi.Open();
            string str = "SELECT id_prodi, nama_prodi FROM dbo.prodi";
            SqlCommand cmd = new SqlCommand(str, koneksi);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            koneksi.Close();

            cbxProdi.DisplayMember = "nama_prodi";
            cbxProdi.ValueMember = "id_prodi";
            cbxProdi.DataSource = dt;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            refreshform();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            nim = txtNIM.Text.Trim();
            nama = txtNama.Text.Trim();
            alamat = txtAlamat.Text.Trim();
            jk = cbxJenisKelamin.SelectedItem.ToString();
            prodi = cbxProdi.SelectedValue.ToString();
            tgl = dtTanggalLahir.Value;

            if (string.IsNullOrEmpty(nim) || string.IsNullOrEmpty(nama) || string.IsNullOrEmpty(alamat) || string.IsNullOrEmpty(jk) || string.IsNullOrEmpty(prodi))
            {
                MessageBox.Show("Please fill in all identity fields!");
            }
            else
            {
                koneksi.Open();
                string query = "INSERT INTO mahasiswa (nim, nama_mahasiswa, alamat, jenis_kelamin, id_prodi, tgl_lahir) VALUES (@nim, @nama_mahasiswa, @alamat, @jenis_kelamin, @id_prodi, @tgl_lahir)";
                SqlCommand command = new SqlCommand(query, koneksi);
                command.Parameters.AddWithValue("@nim", nim);
                command.Parameters.AddWithValue("@nama_mahasiswa", nama);
                command.Parameters.AddWithValue("@alamat", alamat);
                command.Parameters.AddWithValue("@jenis_kelamin", jk);
                command.Parameters.AddWithValue("@id_prodi", prodi);
                command.Parameters.AddWithValue("@tgl_lahir", tgl);
                command.ExecuteNonQuery();
                koneksi.Close();

                MessageBox.Show("Data has been saved to the database.");
            }
        }

        private void FormDataMahasiswa_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 hu = new Form1();
            hu.Show();
            this.Hide();
        }

       
    }
}
