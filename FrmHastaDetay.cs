using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace Proje_Hastane

// uygulamayı müşteriye sunmadan önce mutlak içeriği doldur verileri doldur.
// böyle müşterinin daha çok dikkatini çeker ve tatmin olur.
{
    public partial class FrmHastaDetay : Form
    {
        public FrmHastaDetay()
        {
            InitializeComponent();
        }
        public string tc;

        sqlbaglantisi bgl = new sqlbaglantisi();


        private void FrmHastaDetay_Load(object sender, EventArgs e)
        {
            LblTC.Text = tc;

            // Ad Soyad çekme

            SqlCommand komut = new SqlCommand("Select HastaAd,HastaSoyad From Tbl_Hastalar where HastaTC=@p1", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1",LblTC.Text);
            SqlDataReader dr = komut.ExecuteReader(); 
            while(dr.Read())  // direkt listelemek için kullanılıyor  
            {
                LblAdSoyad.Text = dr[0] + " " + dr[1];
            }
            bgl.baglanti().Close();


            // Randevu Geçmişi
            
            DataTable dt = new DataTable();   // bir veri tablosu oluşturuyoruz
            SqlDataAdapter da = new SqlDataAdapter("Select * from Tbl_Randevular where HastaTC= " + tc, bgl.baglanti());  // dataadapter >> veri aktarımı için
            da.Fill(dt);   // veri tablosunu DataAdapter a aktarıyoruz
            dataGridView1.DataSource = dt;
            // burda da datagriedwiev ile gösteriyoruz.
            // datagriedview de bağlantı açıp kapatmaya gerek kalmıyor.


            // Branşları Çekme

            SqlCommand komut2 = new SqlCommand("Select BransAd From Tbl_Branslar",bgl.baglanti());
            SqlDataReader dr2 = komut2.ExecuteReader(); // komut2 den gelen verişleri okusun
            while(dr2.Read()) // veri okunduğu sürece
            {
                CmbBrans.Items.Add(dr2[0]);
            }
            bgl.baglanti().Close();
            

           

        }
        
        private void CmbBrans_SelectedIndexChanged(object sender, EventArgs e)
        {
            // branşa ait doktorları listeleyecez

            CmbDoktor.Items.Clear();

            SqlCommand komut3 = new SqlCommand("Select DoktorAd,DoktorSoyad from Tbl_Doktorlar where DoktorBrans=@p1",bgl.baglanti());
            komut3.Parameters.AddWithValue("p1",CmbBrans.Text);
            SqlDataReader dr3 = komut3.ExecuteReader(); // komut3 e gelen verileri oku
            while(dr3.Read()) // veriler okunabildiği sürece
            {
                CmbDoktor.Items.Add(dr3[0] + " " + dr3[1]);
            }
            bgl.baglanti().Close(); 
        }

        private void CmbDoktor_SelectedIndexChanged(object sender, EventArgs e)
        {
            

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select * From Tbl_Randevular where RandevuBrans='" + CmbBrans.Text + " ' "+ "and RandevuDoktor='"+ CmbDoktor.Text + "' and RandevuDurum=0",bgl.baglanti());
            da.Fill(dt);
            dataGridView2.DataSource = dt;
        }

        private void LnkBilgiDuzenle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmBilgiDuzenle fr = new FrmBilgiDuzenle();
            fr.TCno = LblTC.Text;
            fr.Show();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView2.SelectedCells[0].RowIndex;
            Txtid.Text = dataGridView2.Rows[secilen].Cells[0].Value.ToString();
        }

        private void BtnRandevuAl_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("Update Tbl_Randevular set RandevuDurum=1,HastaTC=@p1, HastaSikayet=@p2 where Randevuid=@p3",bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", LblTC.Text);
            komut.Parameters.AddWithValue("@p2", RchSikayet.Text);
            komut.Parameters.AddWithValue("@p3", Txtid.Text);
            komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Randevu Alındı", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
