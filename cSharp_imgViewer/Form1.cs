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

namespace cSharp_imgViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.button1.Click += button1_Click;

        }

        // find 버튼 및 폴더경로 textbox로 넣음
        public void button1_Click(object sender, EventArgs e)
        {
            string fileContent;
            string filePath;
            string fileName;
            string dirPath;
            result.Text = null;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; | All files (*.*) | *.*; ";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    fileName = openFileDialog.SafeFileName;

                    filePath = openFileDialog.FileName;

                    Image img = Image.FromFile(filePath);

                    pictureBox1.Image = img;

                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }

                    dirPath = filePath.Replace(fileName, "");

                    result.Text = dirPath;

                    makeList();
                }
            }

        }

        public void makeList()
        {
            string folderName;
            string[] arr = new string[100];
            ListViewItem itm;
            int idx = 0;

            listView1.Items.Clear();

            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;

            listView1.Columns.Add("이름", 100);
            listView1.Columns.Add("크기", 60);
            listView1.Columns.Add("날짜", 500);

            folderName = string.Empty;

            if (result.Text.Length > 0)
            {
                folderName = result.Text.Substring(0, result.Text.Length - 1);
            }
            else
            {
                MessageBox.Show("지정된 경로가 없습니다.");
            }

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(folderName);

            foreach (System.IO.FileInfo file in dir.GetFiles())
            {
                if (file.Name.Contains(".jpg") || file.Name.Contains(".png") || file.Name.Contains(".jfif") || file.Name.Contains(".jpeg")
                    || file.Name.Contains(".jpe"))
                {
                    arr[idx] = file.Name;
                    arr[idx + 1] = file.Length.ToString();
                    arr[idx + 2] = file.CreationTime.ToString();
                    itm = new ListViewItem(arr);
                    listView1.Items.Add(itm);
                }
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                ListView.SelectedListViewItemCollection items = listView1.SelectedItems;
                ListViewItem lvItem = items[0];

                string Path = result.Text + lvItem.SubItems[0].Text;

                Console.WriteLine(Path);

                Image img = Image.FromFile(Path);

                pictureBox1.Image = img;

              
            }
        }
    }
}
