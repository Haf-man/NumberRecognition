using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Remoting.Messaging;


namespace NumberRecognition
{
    public struct Pair<T>
    {
        public T first, second;

        public Pair(T _first, T _second)
        {
            first = _first;
            second = _second;
        }
    }

    public class NumberToDigitConverter
    {
        private int[,] image;

        private int imageHeight;
        private int imageWidth;
        private List<int[,]> images;
        private int[] dx = {-1, 0, 1, -1, 1, -1, 0, 1};
        private int[] dy = {-1, -1, -1, 0, 0, 1, 1, 1};
        private int[,] imageZero;

        public NumberToDigitConverter(int[,] _image, int height, int width)
        {
            image = new int[height, width];
            imageZero = new int[height, width];
            Array.Copy(_image, image, height * width);
            imageHeight = height;
            imageWidth = width;
            images = new List<int[,]>();
            Array.Copy(_image, imageZero, height * width);
        }

        public List<Tuple<int[,], Pair<Point>>> Convert()
        {
            List<Tuple<int[,], Pair<Point>>> digitImages = new List<Tuple<int[,], Pair<Point>>>();
            Pair<Point> border = new Pair<Point>(new Point(imageHeight, imageWidth), new Point(0, 0));

            for (int i = 0; i < imageHeight; i++)
                for (int j = 0; j < imageWidth; j++)
                    if (image[i, j] == 1)
                    {
                        Pair<Point> findedBorder = FindDigit(i, j, border);

                        digitImages.Add(new Tuple<int[,], Pair<Point>>(ExtendToSquare(CreateDigitImage(findedBorder)), findedBorder));
                    }

            return digitImages;
        }

        public int[,] ExtendToSquare(int[,] image)
        {
            int sideSize = Math.Max(image.GetLength(0), image.GetLength(1));

            var squaredImage = new int[sideSize,sideSize];

            int difference;
            if (image.GetLength(0) < sideSize)
            {
                difference = sideSize - image.GetLength(0);
                int halfDifference = (int) Math.Ceiling((0.0 + difference)/2);
                for (int i = 0; i < sideSize; ++i)
                {
                    for (int j = 0; j < sideSize; ++j)
                    {
                        if (i >= halfDifference && i < sideSize - halfDifference)
                        {
                            squaredImage[i, j] = image[i - halfDifference, j];
                        }
                        else
                        {
                            squaredImage[i, j] = 0;
                        }
                    }
                }
            }
            else
            {
                difference = sideSize - image.GetLength(1);
                int halfDifference = (int)Math.Ceiling((0.0 + difference) / 2);
                for (int i = 0; i < sideSize; ++i)
                {
                    for (int j = 0; j < sideSize; ++j)
                    {
                        if (j >= halfDifference && j < sideSize - halfDifference)
                        {
                            squaredImage[i, j] = image[i, j - halfDifference];
                        }
                        else
                        {
                            squaredImage[i, j] = 0;
                        }
                    }
                }
            }

            return squaredImage;
        }

        private int[,] CreateDigitImage(Pair<Point> pair)
        {
            int[,] digitImage = new int[pair.second.X - pair.first.X + 1, pair.second.Y - pair.first.Y + 1];
            int k = 0;
            for (int i = pair.first.X; i <= pair.second.X; i++)
                for (int j = pair.first.Y; j <= pair.second.Y; j++)
                {
                    digitImage[i - pair.first.X, j - pair.first.Y] = imageZero[i, j];
                }

            return digitImage;
        }

        private Pair<Point> Compare(Pair<Point> pair, Point point)
        {
            Pair<Point> rpair = new Pair<Point>(new Point(), new Point());
            if (pair.first.X < point.X)
                rpair.first.X = pair.first.X;
            else
            {
                rpair.first.X = point.X;
            }
            if (pair.first.Y < point.Y)
                rpair.first.Y = pair.first.Y;
            else
            {
                rpair.first.Y = point.Y;
            }

            if (pair.second.X > point.X)
                rpair.second.X = pair.second.X;
            else
            {
                rpair.second.X = point.X;
            }
            if (pair.second.Y > point.Y)
                rpair.second.Y = pair.second.Y;
            else
            {
                rpair.second.Y = point.Y;
            }

//            var minPoint = new Point(Math.Min(pair.first.Y, pair.second.Y), Math.Min(pair.first.X, pair.second.X));
//            var maxPoint = new Point(Math.Max(pair.first.Y, pair.second.Y), Math.Max(pair.first.X, pair.second.X));

            return rpair;
        }

        private Pair<Point> FindDigit(int x, int y, Pair<Point> border)
        {
#if DEBUG
            Console.WriteLine("findDigit");
#endif
            Queue<Point> queue = new Queue<Point>();
            queue.Enqueue(new Point(x, y));
            while (queue.Count != 0)
            {
                Point p = queue.Dequeue();
                int px = p.X;
                int py = p.Y;
                image[px, py] = 0;
                border = Compare(border, p);
                for (int i = 0; i < 8; i++)
                {
                    if (px + dx[i] >= 0 && px + dx[i] < imageHeight && py + dy[i] >= 0 && py + dy[i] < imageWidth)
                        if (image[px + dx[i], py + dy[i]] == 1)
                        {
                            queue.Enqueue(new Point(px + dx[i], py + dy[i]));
                            image[px + dx[i], py + dy[i]] = 0;
                            //  border = compare(border, compare(new Point(x + dx[i], y + dy[i]), new Point(px, py)));
                        }
                }
            }
            return border;
        }
    }
}