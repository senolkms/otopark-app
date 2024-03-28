using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Otopark
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        void datarenk()
        {
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                DataGridViewCellStyle renk = new DataGridViewCellStyle();
                if (Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value) == 0)
                {renk.BackColor = Color.Red;
                    renk.ForeColor = Color.White;
                }
                dataGridView1.Rows[i].DefaultCellStyle = renk;
            }
        }
        public OleDbConnection baglan = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;data source=Otopark1.mdb");
        public DataSet ds = new DataSet();

        private void Form1_Load(object sender, EventArgs e)
        {
            baglan.Open();
            OleDbDataAdapter da = new OleDbDataAdapter("Select * from Tablo1 ", baglan);
            da.Fill(ds,"Tablo1");
            dataGridView1.DataSource = ds.Tables["Tablo1"];
            baglan.Close();
            datarenk();
        }
    
    private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="")
            {
                MessageBox.Show("Herhangi bir plaka girmediniz...!");
            }
            baglan.Open();
            OleDbCommand kmt = new OleDbCommand("insert into Tablo1(plaka,gir_saat,cik_saat,borcu) values ('" + textBox1.Text + 
                "','"+ dateTimePicker1.Text + "','" + dateTimePicker2.Text + "',0 )",baglan);
            kmt.ExecuteReader();
            ds.Clear();
            textBox1.Clear();
            OleDbDataAdapter da = new OleDbDataAdapter("Select * from Tablo1 ", baglan);
            da.Fill(ds, "Tablo1");
            dataGridView1.DataSource = ds.Tables["Tablo1"];
            baglan.Close();
            datarenk();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            double b, t;
            DateTime dttarih1 = Convert.ToDateTime(label7.Text.ToString().Substring(11, 5));
            DateTime dttarih2 = Convert.ToDateTime(dateTimePicker2.Text);
            TimeSpan tsfark;
            tsfark = dttarih2.Subtract(dttarih1);
            double dfark = tsfark.TotalHours;
            double kk = Math.Round((dfark * 60));
            if (kk<=15)
            {
                textBox3.Text = kk.ToString();
                textBox4.Text = "1";
                MessageBox.Show("15 dk dan az kaldığı için sabit ücret 1TL'dir..");
            }
            else
            {
                textBox3.Text = kk.ToString();
                b = kk * 0.1;
                t = Math.Round(b, 0);
                textBox3.Text = t.ToString();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            label7.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox5.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ds.Clear();
            baglan.Open();
            OleDbCommand kmt = new OleDbCommand("update tablo1 SET plaka='" +textBox5.Text + "',cik_saat='" + dateTimePicker2.Text + "',borcu='" + textBox4.Text + "' where plaka = '" + textBox5.Text + "'", baglan);
         
            kmt.ExecuteNonQuery();
            OleDbDataAdapter da = new OleDbDataAdapter("select * from tablo1", baglan);
            da.Fill(ds, "tablo1");
            dataGridView1.DataSource = ds.Tables["tablo1"];
            textBox1.Clear();
            baglan.Close();
            datarenk();
        }
    }
}
