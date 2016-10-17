using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace InterfaceForCV
{
    public partial class Form1 : Form
    {
        bool paint;
        Point lastPoint;
        Graphics g;
        int widthOfLine;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            paint = false;
            widthOfLine = 10;
        }

        private void PaintingPanel_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Image image = pictureBox1.Image;
            g = Graphics.FromImage(image);
            lastPoint = e.Location; // может стоит копии точек создавать. Они не уничтожаются до того, как используются?
            paint = true;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            paint = false;
            g.Dispose();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (paint)
            {
                Point сurrPoint = e.Location;
                Pen pen = new Pen(Brushes.Black, widthOfLine);
                g.DrawLine(pen, lastPoint, сurrPoint);
                g.FillEllipse(Brushes.Black, e.X - widthOfLine / 2, e.Y - widthOfLine / 2, widthOfLine, widthOfLine);

                lastPoint = сurrPoint;
                pictureBox1.Invalidate();
            }
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            Image SelectedImg = Image.FromFile(openFileDialog1.FileName);
            int img_width = SelectedImg.Width;
            int img_height = SelectedImg.Height;
            int win_width = pictureBox1.Width;
            int win_height = pictureBox1.Height;

            int M = Math.Min(win_width * img_height, win_height * img_width);
            int newImgWidth = M / img_height;
            int newImgHeight = M / img_width;

            Bitmap bmp = new Bitmap(SelectedImg, newImgWidth, newImgHeight);
            Image image = pictureBox1.Image;
            g = Graphics.FromImage(image);
            Point Location = new Point();
            Location.X = (win_width - newImgWidth) / 2;
            Location.Y = (win_height - newImgHeight) / 2;
            g.DrawImage(bmp, Location.X, Location.Y); //по центру нужно
            g.Dispose();
            pictureBox1.Invalidate();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            Image image = pictureBox1.Image;
            g = Graphics.FromImage(image);
            g.Clear(Color.White);
            g.Dispose();
            pictureBox1.Invalidate();
        }

        private void recognizeButton_Click(object sender, EventArgs e)
        {
            outputLabel.Text = "";

            int[,] image = ConvertImage(pictureBox1.Image);
            // TODO: make recognition
            outputLabel.Text = "NaN";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private int[,] ConvertImage(Image image)
        {
            int[,] convertedImage = new int[image.Height, image.Width];

            using (Bitmap bitmap = new Bitmap(image))
            {
                int height = bitmap.Height;
                int width = bitmap.Width;
                for (int i = 0; i < height; ++i)
                {
                    for (int j = 0; j < width; ++j)
                    {
                        convertedImage[i, j] = bitmap.GetPixel(j, i) == Color.Black ? 1 : 0;
                    }
                }
            }

            return convertedImage;
        }
    }
}