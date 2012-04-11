using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace iGL.Designer
{
    public partial class TextureConverter : Form
    {
        public TextureConverter()
        {
            InitializeComponent();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (Bitmap bmp = new Bitmap(openFileDialog.FileName))
                {
                    BitmapData data = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                                        ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);


                    using (var stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        unsafe
                        {
                            int[] size = new int[2];
                            size[0] = bmp.Width;
                            size[1] = bmp.Height;
                            
                            var bytes = new byte[4 * bmp.Width * bmp.Height];

                            stream.Write(BitConverter.GetBytes(size[0]), 0, 4);
                            stream.Write(BitConverter.GetBytes(size[1]), 0, 4);

                            Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);

                            for (int i = 0; i < bytes.Length; i += 4)
                            {
                                var r = bytes[i + 2];
                                var g = bytes[i + 1];
                                var b = bytes[i];

                                bytes[i] = r;
                                bytes[i + 1] = g;
                                bytes[i + 2] = b;
                            }

                            stream.Write(bytes, 0, bytes.Length);                            
                        }
                    }
                }
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lblTexture.Text = openFileDialog.SafeFileName;
            }
        }
    }
}
