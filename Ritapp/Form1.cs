using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ritapp
{
    public partial class Form1 : Form
    {
        private bool isMouseDown = false;
        private Pen pen;
        private Pen eraser;
        private Bitmap drawingBitmap;
        private Point lastPoint;
        private bool isErasing = false;

        public Form1()
        {
            InitializeComponent();
            pen = new Pen(Color.Black, 2);
            eraser = new Pen(Color.White, 10);
            drawingBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = drawingBitmap;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            lastPoint = e.Location;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                using (Graphics g = Graphics.FromImage(drawingBitmap))
                {
                    if (isErasing)
                    {
                        g.DrawLine(eraser, lastPoint, e.Location);
                    }
                    else
                    {
                        g.DrawLine(pen, lastPoint, e.Location);
                    }
                }
                pictureBox1.Invalidate(); // Force the PictureBox to repaint
                lastPoint = e.Location;
            }
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    pen.Color = colorDialog.Color;
                }
            }
        }

        private void btnThickness_Click(object sender, EventArgs e)
        {
            using (Form thicknessForm = new Form())
            {
                thicknessForm.Text = "Select Pen Thickness";
                TrackBar trackBar = new TrackBar
                {
                    Minimum = 1,
                    Maximum = 10,
                    Value = (int)pen.Width,
                    Dock = DockStyle.Fill
                };
                thicknessForm.Controls.Add(trackBar);
                trackBar.Scroll += (s, args) => { pen.Width = trackBar.Value; };
                thicknessForm.ShowDialog();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            using (Graphics g = Graphics.FromImage(drawingBitmap))
            {
                g.Clear(Color.White);
            }
            pictureBox1.Invalidate();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    drawingBitmap.Save(saveFileDialog.FileName);
                }
            }
        }

        private void btnEraser_Click(object sender, EventArgs e)
        {
            isErasing = !isErasing;
            btnEraser.Text = isErasing ? "Pen" : "Eraser";
        }
    }
}
