using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BotApplication.Helpers.Interfaces;

namespace BotApplication
{
    public partial class OcrAnalyzerForm : Form
    {
        private readonly IOcrHelper _helper;

        private readonly ICollection<Rectangle> _areasUsed;

        public OcrAnalyzerForm(
            IOcrHelper helper)
        {
            _helper = helper;
            _areasUsed = new HashSet<Rectangle>();

            Load += OcrAnalyzerForm_Load;
            Closing += OcrAnalyzerForm_Closing;
            Activated += OcrAnalyzerForm_Activated;

            InitializeComponent();
        }

        private void OcrAnalyzerForm_Activated(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                using (var graphics = Graphics.FromImage(pictureBox1.Image))
                {
                    foreach (var rectangle in _areasUsed)
                    {
                        graphics.DrawRectangle(Pens.Red, rectangle);
                    }
                }
                if (_areasUsed.Count > 100)
                {
                    _areasUsed.Clear();
                }
            }
        }

        private void OcrAnalyzerForm_Closing(object sender, CancelEventArgs e)
        {
            _helper.OcrTextScanPerformed -= HelperOcrTextScanPerformed;
        }

        private void OcrAnalyzerForm_Load(object sender, EventArgs e)
        {
            _helper.OcrTextScanPerformed += HelperOcrTextScanPerformed;
        }

        private void HelperOcrTextScanPerformed(object sender, Events.OcrTextScanPerformedEventArgs e)
        {
            _areasUsed.Add(e.Region);
            pictureBox1.Image = e.ImageUsed;
            Text = e.Text ?? Text;
        }
    }
}
