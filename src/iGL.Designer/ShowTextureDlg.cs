using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iGL.Engine.Resources;

namespace iGL.Designer
{
    public partial class ShowTextureDlg : Form
    {
        public Texture Texture { get; set; }

        private Bitmap bmp;

        public ShowTextureDlg()
        {
            InitializeComponent();
            this.Load += new EventHandler(ShowTextureDlg_Load);
            this.Paint += new PaintEventHandler(ShowTextureDlg_Paint);
        }

        void ShowTextureDlg_Paint(object sender, PaintEventArgs e)
        {
            if (bmp == null) return;
            e.Graphics.FillRectangle(Brushes.Pink, new Rectangle(0, 0, bmp.Width, bmp.Height));
            e.Graphics.DrawImage(bmp, 0, 0);
        }

        void ShowTextureDlg_Load(object sender, EventArgs e)
        {
            var resourceAsm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(asm => asm.GetManifestResourceNames().Contains(Texture.ResourceName));

            if (resourceAsm == null) throw new Exception(Texture.ResourceName + " not found.");

            using (var stream = resourceAsm.GetManifestResourceStream(Texture.ResourceName))
            {
                var bytes = new byte[stream.Length - 8];

                var intBytes = new byte[8];
                stream.Read(intBytes, 0, 8);
                var width = BitConverter.ToInt32(intBytes, 0);
                var height = BitConverter.ToInt32(intBytes, 4);

                stream.Read(bytes, 0, (int)bytes.Length);

                bmp = new Bitmap(width, height);
                var data = bmp.LockBits(new Rectangle() { Width = width, Height = height }, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                unsafe
                {
                    fixed (byte* p = bytes)
                    {
                       data.Scan0 = (IntPtr)p;
                    }
                }

                bmp.UnlockBits(data);

                this.ClientSize = new Size(width, height);
            }
        }

        
    }
}
