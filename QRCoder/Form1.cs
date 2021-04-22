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
using ZXing.QrCode.Internal;

namespace QRCoder
{
    public partial class Form1 : Form
    {
        private string bitmapFile = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cboCorrectionCode.SelectedIndex = 0;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (!validate()) return;
            Image logo = (bitmapFile == null) ? null : Image.FromFile(bitmapFile);
            pictureBox1.Image = generateQRCode(textBox1.Text, logo);
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

        private ErrorCorrectionLevel GetCorrectionLevel()
        {
            switch (cboCorrectionCode.SelectedIndex)
            {
                case 0: return ErrorCorrectionLevel.L;
                case 1: return ErrorCorrectionLevel.M;
                case 2: return ErrorCorrectionLevel.Q;
                case 3: return ErrorCorrectionLevel.H;
            }
            return ErrorCorrectionLevel.L;
        }

        private Bitmap generateQRCode(string url, Image logo = null, int width = 600, int height = 600)
        {
            var encodingOptions = new QrCodeEncodingOptions
            {
                Width = width,
                Height = height,
                Margin = 1,
                PureBarcode = false
            };
            encodingOptions.Hints.Add(EncodeHintType.ERROR_CORRECTION, GetCorrectionLevel());

            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = encodingOptions
            };                        
            var bmpQR = barcodeWriter.Write(textBox1.Text);

            if (logo != null)
            {
                Graphics g = Graphics.FromImage(bmpQR);
                g.DrawImage(logo, new Point((bmpQR.Width - logo.Width) / 2, (bmpQR.Height - logo.Height) / 2));
            }

            return bmpQR;
        }

        private Image GetLogo(string file)
        {
            return Image.FromFile(file);
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            if (!validate()) return;

            SaveFileDialog d = new SaveFileDialog { Filter = "PNG|*.png" };
            if (d.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image logo = (bitmapFile == null) ? null : Image.FromFile(bitmapFile);
                    Bitmap bmp = generateQRCode(textBox1.Text, logo, 600, 600);
                    bmp.Save(d.FileName, ImageFormat.Png);

                    MessageBox.Show("QRCode Saved", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error encountered!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLogo_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog { Filter = "PNG|*.png||*.jpg" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {

                lblLogo.Text = System.IO.Path.GetFileName(dlg.FileName);
                bitmapFile = dlg.FileName;
            }
        }
    }
}
