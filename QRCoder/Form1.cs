using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.QrCode;

namespace QRCoder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (!validate()) return;
                        
            pictureBox1.Image = generateQRCode(textBox1.Text);
        }

        private bool validate()
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || "https://".Equals(textBox1.Text.Trim()))
            {
                MessageBox.Show("TextBox is Empty", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private Bitmap generateQRCode(string url, int width = 600, int height = 600)
        {
            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions { Width = width, Height = height }
            };

            var bmp = barcodeWriter.Write(textBox1.Text);
            return bmp;
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            if (!validate()) return;

            SaveFileDialog d = new SaveFileDialog { Filter = "PNG|*.png" };
            if (d.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Bitmap bmp = generateQRCode(textBox1.Text, 600, 600);
                    bmp.Save(d.FileName, ImageFormat.Png);

                    MessageBox.Show("QRCode Saved", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error encountered!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
