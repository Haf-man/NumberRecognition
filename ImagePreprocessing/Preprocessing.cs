using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagePreprocessing
{
    public class Preprocessing
    {
        const int IMAGE_WIDTH = 32;
        const int IMAGE_HEIGHT = 32;

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

        public int[,] preprocessing(int[,] _image, int _imageWidth, int _imageHeight)
        {
            resize();
            thickening();
            this.numAreas = countAreas();

            return finalImage;
        }

        // масштабирование
        private void resize()
        {
            double stepX = (double)IMAGE_HEIGHT / imageHeight;
            double stepY = (double)IMAGE_WIDTH / imageWidth;

            finalImage = new int[IMAGE_HEIGHT, IMAGE_WIDTH];

            for (int i = 0; i < IMAGE_HEIGHT; i++)
                for (int j = 0; j < IMAGE_WIDTH; j++)
                    finalImage[i, j] = 0;

            for (int i = 0; i < imageHeight; i++)
                for (int j = 0; j < imageWidth; j++)
                    if (image[i, j] == 1)
                        finalImage[(int)(i * stepX), (int)(j * stepY)] = 1;
        }

        //утолщение цифр 
        private void thickening()
        {
            bool isAlone;

            int[] dx = { -1, 1, 0, 0, -1, 1, -1, 1 };
            int[] dy = { -1, 1, 0, 0, -1, 1, -1, 1 };

            for (int i = 1; i < imageWidth - 1; i++)
                for (int j = 1; j < imageHeight-1; j++)
                {
                    if (image[i, j] == 1)
                    {
                        finalImage[i, j] = 1;
                        for (int k = 0; k < 8; k++)
                            finalImage[i + dx[k], j + dy[k]] = 1;
                    }
                }

            //убирает "одинокие" нули
            for (int i = 1; i < imageWidth - 1; i++)
                for (int j = 1; j < imageHeight - 1; j++)
                {
                    isAlone = true;
                    if (image[i, j] == 0)
                        for (int k = 0; k < 8; k++)
                            if (finalImage[i + dx[k], j + dy[k]] == 0)
                            {
                                isAlone = false;
                                break;
                            }
                    if (isAlone)
                        image[i, j] = 1;
                }
        }

        //наращивает связную область из центральной точки
        private void segmentImage(int[,] segmentedImage, int label)
        {
            int startX = 0;
            int startY = 0;
            bool isFind = false;


            for (int i = 0; i < IMAGE_HEIGHT; i++)
            {
                for (int j = 0; j < IMAGE_WIDTH; j++)
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

                        if ((p.X >= 0) && (p.X < IMAGE_WIDTH) && (p.Y >= 0) && (p.Y < IMAGE_HEIGHT)
                                       && (segmentedImage[p.X, p.Y] == startValue))
                            points.Add(p);
                    }

                    segmentedImage[current.X, current.Y] = label;
                }

                for (int i = 0; i < IMAGE_HEIGHT; i++)
                    for (int j = 0; j < IMAGE_WIDTH; j++)
                        if (segmentedImage[i, j] == label)
                            segmentedPoints++;
            }

        }

        // считает связные области (хз зачем, но пусть будет, может пригодится)
        private int countAreas()
        {
            int[,] segmentedImage = (int[,])finalImage.Clone();

            int label = 2;

            while (segmentedPoints < IMAGE_HEIGHT * IMAGE_WIDTH)
            {
                label++;
                segmentImage(segmentedImage, label);
            }

            return label - 2;
        }
    }
}
