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
using System.Threading;
using ImagePreprocessing;
using NeuralNetworik.NN;
using NumberRecognition;
using System.IO;

namespace InterfaceForCV
{
    public partial class Form1 : Form
    {
        bool paint;
        Point lastPoint;
        Graphics g;
        int widthOfLine;
        Bitmap tempImage;
        private NeuralNetwork _neuralNetwork;
    //    private ImageDatabaseKeeper _database;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            paint = false;
            widthOfLine = 10;
          //  _database = new ImageDatabaseKeeper();
        }

        private void PaintingPanel_MouseDown(object sender, MouseEventArgs e)
        {
        }
        
    private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Image image = pictureBox1.Image;
            tempImage = new Bitmap(image);
            g = Graphics.FromImage(tempImage);
            lastPoint = e.Location; // может стоит копии точек создавать. Они не уничтожаются до того, как используются?
            paint = true;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            paint = false;
            pictureBox1.Image = tempImage;
            pictureBox1.Invalidate();
            g.Dispose();
        }
        
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (paint)
            {
                Point сurrPoint = e.Location;
                Pen pen = new Pen(Brushes.Red, widthOfLine);
                g.DrawLine(pen, lastPoint, сurrPoint);
                g.FillEllipse(Brushes.Red, e.X - widthOfLine / 2, e.Y - widthOfLine / 2, widthOfLine, widthOfLine);

                lastPoint = сurrPoint;
                pictureBox1.Image = tempImage;
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
            Image image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
     // _database.saveToFile();
            g = Graphics.FromImage(image);
            g.Clear(Color.White);
            g.Dispose();
            pictureBox1.Invalidate();
        }

        private void recognizeButton_Click(object sender, EventArgs e)
        {
            outputLabel.Text = "";

            int[,] image = ConvertImage(pictureBox1.Image);

            NumberToDigitConverter converter = new NumberToDigitConverter(image, image.GetLength(0), image.GetLength(1));
            List<Tuple<int[,], Pair<Point>>> digits = converter.Convert();
            List<int[,]> digitImages = new List<int[,]>();
            List<Pair<Point>> digitBorders = new List<Pair<Point>>();
           
            foreach (var v in digits)
            {

                digitImages.Add(v.Item1);
                digitBorders.Add(v.Item2);
                drawBorder(v.Item2);
                 
            }

            List<int> predictedInts = new List<int>();
      int k = 0;
            foreach (var v in digitImages)
            {
                AreasCounter areasCounter = new AreasCounter(v, v.GetLength(0), v.GetLength(1));
                int numberOfAreas = areasCounter.countAreas();                 
                RecognitionOfNumbers recognizer = new RecognitionOfNumbers();
                int resultDigit = recognizer.recognizeDigit(v, v.GetLength(0), v.GetLength(1), numberOfAreas);
                predictedInts.Add(resultDigit);
                drawResult(digits[k++].Item2.first, resultDigit);
        
              //  Preprocessing preprocessing = new Preprocessing(v, v.GetLength(0), v.GetLength(1));
    //  var preprocessedImage = preprocessing.Preprocess();
      //  predictedInts.Add( _database.determinateDigit( preprocessing.PreprocessTest())); 
           //  predictedInts.Add(_neuralNetwork.ComputeResponse(preprocessedImage));
            }

            outputLabel.Text = string.Join(" ", predictedInts);
        }
      private void drawResult(Point point, int digit)
    {
      paint = true;
      Image image = pictureBox1.Image;
      tempImage = new Bitmap(image);
      g = Graphics.FromImage(tempImage);
      Font drawFont = new Font("Arial", 16);
      g.DrawString(digit.ToString(), drawFont, Brushes.Black, new Point(point.X - 10, point.Y - 10));
      paint = false;
      pictureBox1.Image = tempImage;
      pictureBox1.Invalidate();
      g.Dispose();
    }
        private void drawBorder(Pair<Point> border)
        {
            paint = true;
            Image image = pictureBox1.Image;
            tempImage = new Bitmap(image);
            g = Graphics.FromImage(tempImage);
            Pen pen = new Pen(Brushes.Black, widthOfLine - 9);
            g.DrawLine(pen, border.first, new Point(border.first.X, border.second.Y));
            g.DrawLine(pen, border.first, new Point(border.second.X, border.first.Y));
            g.DrawLine(pen, new Point(border.first.X, border.second.Y), border.second);
            g.DrawLine(pen, new Point(border.second.X, border.first.Y), border.second);
    
            paint = false;

            pictureBox1.Image = tempImage;
            pictureBox1.Invalidate();
            g.Dispose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _neuralNetwork = NeuralNetwork.ReadFrom("network.json");
        }

        private Bitmap ConvertImageToBitmap(int[,] image)
        {
            int height = image.GetLength(1);
            int width = image.GetLength(0);
            Bitmap bitmap = new Bitmap(width, height);
            {
                for (int i = 0; i < width; ++i)
                {
                    for (int j = 0; j < height; ++j)
                    {
                        bitmap.SetPixel(i, j, Convert.ToBoolean(image[i, j]) ? Color.Red : Color.DarkRed);
                    }
                }
            }
            return bitmap;
        }

        private int[,] ConvertImage(Image image)
        {
            int[,] convertedImage = new int[image.Width, image.Height];
            Color cc = BackColor;
            List<Color> colors = new List<Color>();
            bool flag = true;
            using (Bitmap bitmap = new Bitmap(image))
            {
                int height = bitmap.Height;
                int width = bitmap.Width;

                for (int i = 0; i < width; ++i)
                {
                    for (int j = 0; j < height; ++j)
                    {
                        //   convertedImage[i, j] = bitmap.GetPixel(j, i) == cc ? 0 : 1;

                        if (flag)
                        {
                            cc = bitmap.GetPixel(i, j);
                            flag = false;
                        }
                        Color c = bitmap.GetPixel(i, j);
                        if (!colors.Contains(c))
                            colors.Add(c);
                        if (colors.FindIndex(x => x == c) == 0)
                            convertedImage[i, j] = 0;
                        else
                        {
                            convertedImage[i, j] = 1;
                        }
                        //if (bitmap.GetPixel(i,j) != cc)
                        //{


                        //  System.Drawing.KnownColor jk = Color.Black.ToKnownColor();
                        //  height = bitmap.Height;
                        //  int d = convertedImage[i, j];
                        //} 
                    }
                }
            }

            return convertedImage;
        }

    
  }
}