using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BotApplication
{
    public partial class ImageDebuggerForm : Form
    {
        private static readonly Queue<Bitmap> Images;

        private static int _offset;

        public ImageDebuggerForm()
        {
            InitializeComponent();
        }

        static ImageDebuggerForm()
        {
            var existingFiles = Directory.GetFiles(Environment.CurrentDirectory, "*.png");
            foreach (var file in existingFiles)
            {
                File.Delete(file);
            }

            Images = new Queue<Bitmap>();
        }

        public static void DebugImage(Bitmap image)
        {
            image.Save(_offset++ + ".png");
            Images.Enqueue(image);
            Thread.Sleep(250);
            Application.DoEvents();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (Images.Count > 0)
            {
                pictureBox.Image = Images.Dequeue();
                Application.DoEvents();
            }
        }
    }
}
