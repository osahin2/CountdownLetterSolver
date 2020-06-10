using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Letters
{
    public partial class Form1 : Form
    {
        public static int puan = 0;
        public static Random rnd = new Random();
        public static string[] bulunanKelimeler = new string[200];
        public static int k = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public static void GetRandomString(Button btn)
        {
            string harfler = "abcçdefgğhıijklmnoöpqrsştuüvyz";
            int num = rnd.Next(0, harfler.Length - 1);
            btn.Text = harfler[num].ToString();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Exit_btn_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void Random_btn_Click(object sender, EventArgs e)
        {
            puan = 0;
            Button[] butonlar =
                {harfBir_btn,harfIkı_btn,harfUc_btn,harfDort_btn,harfBes_btn,harfAlti_btn,harfYedi_btn,harfSekiz_btn,harfDokuz_btn
            };
            for(int i = 0; i < butonlar.Length; i++)
            {
                GetRandomString(butonlar[i]);
            }
        }

        private void Joker_btn_Click(object sender, EventArgs e)
        {
            Button[] buttons =
                {harfBir_btn,harfIkı_btn,harfUc_btn,harfDort_btn,harfBes_btn,harfAlti_btn,harfYedi_btn,harfSekiz_btn,harfDokuz_btn
            };
            int num = rnd.Next(1, 9);
            GetRandomString(buttons[num]);
            Joker_btn.Enabled = false;
        }

        private void KelimeBul_btn_Click(object sender, EventArgs e)
        {
            Kelimeler_lst.Items.Clear();

            String[] Input = {
                harfBir_btn.Text,harfIkı_btn.Text,harfUc_btn.Text,harfDort_btn.Text,harfBes_btn.Text,harfAlti_btn.Text,harfYedi_btn.Text,harfSekiz_btn.Text,harfDokuz_btn.Text
            };
            string dosya_yolu = @"D:\Dersler\Yazılım Yapımı\kelime-listesi.txt";
            string[] wordList = System.IO.File.ReadAllLines(dosya_yolu, Encoding.UTF8);

            HashSet<String> s_Words = new HashSet<String>(wordList);

            Dictionary<String, String[]> s_Dict = s_Words
                .Select(word => new
                {
                    Key = String.Concat(word.OrderBy(c => c)),
                    Value = word
                })
                .GroupBy(item => item.Key, item => item.Value)
                .ToDictionary(chunk => chunk.Key, chunk => chunk.ToArray());

            string source = String.Concat(Input.OrderBy(c => c));

            var result = Enumerable
                .Range(1, (1 << source.Length) - 1)
                .Select(index => string.Concat(source.Where((item, idx) => ((1 << idx) & index) != 0)))
                .SelectMany(key =>
                {
                    String[] words;
                    if (s_Dict.TryGetValue(key, out words))
                        return words;
                    else
                        return new String[0];
                })
                .Distinct()
                .OrderBy(word => word);
            foreach (string s in result)
            {
                switch (s.Length)
                {
                    case 3:
                        puan += 3;
                        break;
                    case 4:
                        puan += 4;
                        break;
                    case 5:
                        puan += 5;
                        break;
                    case 6:
                        puan += 7;
                        break;
                    case 7:
                        puan += 9;
                        break;
                    case 8:
                        puan += 11;
                        break;
                    case 9:
                        puan += 15;
                        break;
                    default:
                        break;
                }
                Kelimeler_lst.Items.Add(s);
            }
            
            Puan_lbl.Text = puan.ToString();

            if (Kelimeler_lst.Items.Count == 0)
                Joker_btn.Enabled = true;
        }
    }
}
