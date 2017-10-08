using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SoThuSaiGon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
         private void ListBox_MouseDown(object sender, MouseEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            int index = lb.IndexFromPoint(e.X, e.Y);

            if (index != -1)
                    lb.DoDragDrop(lb.Items[index].ToString(),
                            DragDropEffects.Copy);
        }
        private void ListBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.Move;
        }
        bool isItemChanged = false;
        private void lstDanhSanh_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                bool test = false;
                for (int i = 0; i < lstDanhSanh.Items.Count; i++)
                {
                    string st = lstDanhSanh.Items[i].ToString();
                    string data = e.Data.GetData(DataFormats.Text).ToString();
                    if (data == st)
                        test = true;
                }
                if (test == false)
                {
                    int newIndex = lstDanhSanh.IndexFromPoint(lstDanhSanh.PointToClient(new Point(e.X, e.Y)));
                    lstDanhSanh.Items.Remove(e.Data.GetData(DataFormats.Text));
                    if (newIndex != -1)
                        lstDanhSanh.Items.Insert(newIndex, e.Data.GetData(DataFormats.Text));
                    else
                    {
                        ListBox lb = (ListBox)sender;
                        lb.Items.Add(e.Data.GetData(DataFormats.Text));
                    }
                }

            }
        }
        bool isSaves = false;
        private void Save(object sender, EventArgs e)
        {
            StreamWriter write = new StreamWriter("Danhsachthu.txt");
            if (write == null) return;
            foreach (var item in lstDanhSanh.Items)
                write.WriteLine(item.ToString());
            write.Close();
            isSaves = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StreamReader reader = new StreamReader("Thumoi.txt");

            if (reader == null)
                return;

            string input;
            while ((input = reader.ReadLine()) != null)
                lstThuMoi.Items.Add(input);
            reader.Close();

            using (StreamReader rs = new StreamReader("Danhsachthu.txt"))
            {
                input = null;
                while ((input = rs.ReadLine()) != null)
                    lstDanhSanh.Items.Add(input);
            }
        }

        private void mnuEnd_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mnuLoad_Click(object sender, EventArgs e)
        {
            StreamReader reader = new StreamReader("Thumoi.txt");

            if (reader == null)
                return;

            string input;
            while ((input = reader.ReadLine()) != null)
                lstThuMoi.Items.Add(input);
            reader.Close();

            using (StreamReader rs = new StreamReader("Danhsachthu.txt"))
            {
                input = null;
                while ((input = rs.ReadLine()) != null)
                    lstDanhSanh.Items.Add(input);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = string.Format("Bây giờ là {0}:{1}:{2} ngày {3} tháng {4} năm {5}.",
                DateTime.Now.Hour,
                DateTime.Now.Minute,
                DateTime.Now.Second,
                DateTime.Now.Day,
                DateTime.Now.Month, 
                DateTime.Now.Year);
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            lstDanhSanh.Items.Remove(lstDanhSanh.SelectedItem);
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSaves == false)
            {
                DialogResult kq = MessageBox.Show("Bạn có muốn lưu danh sách?", "THÔNG BÁO", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (kq == DialogResult.Yes)
                {
                    Save(sender, e);
                    e.Cancel = false;
                }
                else if (kq == DialogResult.No)
                    e.Cancel = false;
                else
                    e.Cancel = true;
            }
            else
                mnuEnd_Click(sender, e);
        }
    }
}
