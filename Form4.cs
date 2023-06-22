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

namespace Disconnected_Environment
{
    public partial class Form4 : Form
    {
        private string stringConnection = " data source = LAPTOP-DP3PQGGM\\DEPIIII;" +
       "database=Activity6;User ID=sa;Password=123";
        private SqlConnection koneksi;
        private void refreshform()
        {
            cbxNama.Enabled = false;
            cbxStatusMahasiswa.Enabled = false;
            cbxTahunMasuk.Enabled = false;
            cbxNama.SelectedIndex = -1;
            cbxStatusMahasiswa.SelectedIndex = -1;
            cbxTahunMasuk.SelectedIndex = -1;
            txtNIM.Visible = false;
            btnSave.Enabled = false;
            btnClear.Enabled = false;
            btnAdd.Enabled = true;
        }
        public Form4()
        {
            InitializeComponent();
            koneksi = new SqlConnection(stringConnection);
            refreshform();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'activity6DataSet.mahasiswa' table. You can move, or remove it, as needed.
            this.mahasiswaTableAdapter.Fill(this.activity6DataSet.mahasiswa);

        }

        private void btnOpen_Click_1(object sender, EventArgs e)
        {
            btnOpen.Enabled = false;
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            cbxTahunMasuk.Enabled = true;
            cbxNama.Enabled = true;
            cbxStatusMahasiswa.Enabled = true;
            txtNIM.Visible = true;
            cbTahunMasuk();
            cbNama();
            btnClear.Enabled = true;
            btnSave.Enabled = true;
            btnAdd.Enabled = false;
        }

        private void cbNama()
        {
            koneksi.Open();
            string query = "SELECT nama_mahasiswa FROM mahasiswa";
            SqlCommand command = new SqlCommand(query, koneksi);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                cbxNama.Items.Add(reader.GetString(0));
            }
            reader.Close();
            koneksi.Close();
        }

        private void cbTahunMasuk()
        {
            int currentYear = DateTime.Now.Year;
            for (int i = 2010; i <= currentYear; i++)
            {
                cbxTahunMasuk.Items.Add(i.ToString());
            }
        }


        private void btnClear_Click_1(object sender, EventArgs e)
        {
            refreshform();
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            string nim = txtNIM.Text;
            string statusMahasiswa = cbxStatusMahasiswa.Text;
            string tahunMasuk = cbxTahunMasuk.Text;
            int count = 0;
            string tempKodeStatus = "";
            string kodeStatus = "";
            koneksi.Open();

            string str = "select count (*) from dbo.status_mahasiswa";
            SqlCommand cm = new SqlCommand(str, koneksi);
            count = (int)cm.ExecuteScalar();
            if (count == 0)
            {
                tempKodeStatus = "1";
            }
            else
            {
                string querystring = "select Max(id_status) from dbo.status_mahasiswa";
                SqlCommand cmStatusMahasiswaSum = new SqlCommand(str, koneksi);
                int totalStatusMahasiswa = (int)cmStatusMahasiswaSum.ExecuteScalar();
                int finalKodeStatusInt = totalStatusMahasiswa + 1;
                kodeStatus = Convert.ToString(finalKodeStatusInt);
            }
            string queryString = "insert into dbo.status_mahasiswa (id_status,nim, " +
                "status_mahasiswa, tahun_masuk)" + "values(@ids, @NIM, @sm, @tm)";
            SqlCommand cmd = new SqlCommand(queryString, koneksi);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("ids", kodeStatus));
            cmd.Parameters.Add(new SqlParameter("NIM", nim));
            cmd.Parameters.Add(new SqlParameter("sm", statusMahasiswa));
            cmd.Parameters.Add(new SqlParameter("tm", tahunMasuk));
            cmd.ExecuteNonQuery();
            koneksi.Close();

            MessageBox.Show("Data Berhasil Disimpan", "Sukses", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            refreshform();
            dataGridView();
        }
        private void dataGridView()
        {
            koneksi.Open();
            string query = "SELECT * FROM status_mahasiswa";
            SqlDataAdapter adapter = new SqlDataAdapter(query, koneksi);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            koneksi.Close();
        }
        private void cbxNama_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            koneksi.Open();
            string nim = "";
            string strs = "select NIM from dbo.mahasiswa where nama_mahasiswa = @nm";
            SqlCommand cm = new SqlCommand(strs, koneksi);
            cm.CommandType = CommandType.Text;
            cm.Parameters.Add(new SqlParameter("@nm", cbxNama.Text));
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                nim = dr["NIM"].ToString();
            }
            dr.Close();
            koneksi.Close();

            txtNIM.Text = nim;
        }

        private void cbxStatusMahasiswa_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbxTahunMasuk_SelectedIndexChanged(object sender, EventArgs e)
        {
            int y = DateTime.Now.Year - 2010;
            string[] type = new string[y];
            int i = 0;
            for (i = 0; i < type.Length; i++)
            {
                if (i == 0)
                {
                    cbxTahunMasuk.Items.Add("2010");
                }
                else
                {
                    int l = 2010 + i;
                    cbxTahunMasuk.Items.Add(l.ToString());
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            koneksi.Open();
            string str = "Select * form dbo.status_mahasiswa";
            SqlDataAdapter da = new SqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            koneksi.Close();
        }
    }
}
