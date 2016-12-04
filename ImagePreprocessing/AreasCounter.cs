using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Counter
{
    public class AreasCounter
    {
        private int[,] image;
        private int[,] finalImage;

        private int imageHeight;
        private int imageWidth;

        public int numAreas;
        private int segmentedPoints = 0;

        private struct Point
        {
            public int X, Y;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        public AreasCounter(int[,] _image, int _imageWidth, int _imageHeight)
        {
            //this.numAreas = countAreas();
            this.image = _image;

            this.imageHeight = _imageHeight;
            this.imageWidth = _imageWidth;
        }

        //наращивает связную область из центральной точки
        private void segmentImage(int[,] segmentedImage, int label)
        {
            int startX = 0;
            int startY = 0;
            bool isFind = false;


            for (int i = 0; i < imageWidth + 2; i++)
            {
                for (int j = 0; j < imageHeight + 2; j++)
                    if (segmentedImage[i, j] < 2)
                    {
                        startX = i;
                        startY = j;
                        isFind = true;
                        break;
                    }
                if (isFind) break;
            }

            if (isFind)
            {
                Point start = new Point(startX, startY);
                int startValue = segmentedImage[start.X, start.Y];
                Point current;

                int[] neighbX = { -1, 1, 0, 0, -1, 1, -1, 1 };
                int[] neighbY = { 0, 0, -1, 1, -1, -1, 1, 1 };

                List<Point> points = new List<Point>();

                points.Add(start);

                while (points.Count() > 0)
                {
                    current = new Point(points.Last().X, points.Last().Y);

                    points.Remove(points.Last());

                    for (int i = 0; i < 8; i++)
                    {
                        Point p = new Point(current.X + neighbX[i], current.Y + neighbY[i]);

                        if ((p.X >= 0) && (p.X < imageWidth + 2) && (p.Y >= 0) && (p.Y < imageHeight + 2)
                            && (segmentedImage[p.X, p.Y] == startValue))
                            points.Add(p);
                    }

                    segmentedImage[current.X, current.Y] = label;
                }

                for (int i = 0; i < imageWidth + 2; i++)
                    for (int j = 0; j < imageHeight + 2; j++)
                        if (segmentedImage[i, j] == label)
                            segmentedPoints++;
            }
        }

        // Areas counter 
        public int countAreas()
        {
            int[,] segmentedImage = new int[imageWidth + 2, imageHeight + 2];

            for (int i = 1; i <= imageWidth; i++)
                for (int j = 1; j <= imageHeight; j++)
                {
                    if (i == 0 || j == 0 || i == imageWidth + 1 || j == imageHeight + 1)
                        segmentedImage[i, j] = 0;
                    else
                        segmentedImage[i, j] = image[i - 1, j - 1];

                }

            int label = 1;
            
            while (segmentedPoints < (imageHeight + 2) * (imageWidth + 2))
            {
                label++;
                segmentImage(segmentedImage, label);
            }
            
            return label - 2;
        }


    }
}
