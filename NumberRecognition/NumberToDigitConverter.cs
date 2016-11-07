using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
 

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
    public NumberToDigitConverter(int[,] _image, int width, int height)
    {
      image = new int[width,height];
      imageZero = new int[width,height];
       Array.Copy(_image, image, height * width);
      imageHeight = height;
      imageWidth = width;
      images = new List<int[,]>();
       Array.Copy(_image,imageZero, height*width);
    }
    
    public List<Tuple<int[,],Pair<Point>>> Convert()
    {
      List<Tuple<int[,], Pair<Point>>> digitImages = new List<Tuple<int[,], Pair<Point>>>();
      Pair<Point> border = new Pair<Point>(new Point(imageWidth, imageHeight), new Point(0, 0));

      for(int i = 0 ; i < imageWidth ; i++)
        for(int j = 0 ; j < imageHeight ; j++)
          if (image[i, j] == 1)
          {
            Pair<Point> findedBorder = findDigit(i, j, border);
            digitImages.Add(new Tuple<int[,], Pair<Point>>(createDigitImage(findedBorder), findedBorder ));
          }
      return digitImages;
    }

    private int[,] createDigitImage(Pair<Point> pair)
    {
      int[,] digitImage = new int[pair.second.X-pair.first.X + 1,pair.second.Y-pair.first.Y +1];
      int k = 0;
      for(int i =  pair.first.X ; i<= pair.second.X ; i++)
        for (int j = pair.first.Y; j <= pair.second.Y; j++)
        {
           
          digitImage[i - pair.first.X, j - pair.first.Y] = imageZero[i, j];
        }
    
      return digitImage;
    }
    private Pair<Point> compare(Pair<Point> pair,  Point point)
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
      return rpair;
    }
    private Pair<Point> findDigit(int x, int y,Pair<Point> border )
    {
      
      Queue<Point> queue = new Queue<Point>();
      queue.Enqueue(new Point(x, y));
      while (queue.Count != 0)
      {
        Point p = queue.Dequeue();
        int px = p.X;
        int py = p.Y;
        image[px, py] = 0;
        border = compare(border, p);
        for (int i = 0; i < 8; i++)
        {
          if (px + dx[i] >= 0 && px + dx[i] < imageWidth && py + dy[i] >= 0 && py + dy[i] < imageHeight)
            if (image[px + dx[i], py + dy[i]] == 1)
            {
              queue.Enqueue(new Point(px+dx[i],py+dy[i]));
              image[px + dx[i], py + dy[i]] = 0;
            //  border = compare(border, compare(new Point(x + dx[i], y + dy[i]), new Point(px, py)));
            }
        }
      }
      return border;
    }
  }
}
