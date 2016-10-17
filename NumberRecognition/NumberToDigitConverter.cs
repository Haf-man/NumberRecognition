using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
 

namespace NumberRecognition
{

  struct Pair<T>
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
      image = _image;
      imageHeight = height;
      imageWidth = width;
      images = new List<int[,]>();
      this.imageZero = _image;
    }

    public List<int[,]> Convert()
    {
      List<int[,]> digitImages = new List<int[,]>();
      for(int i = 0 ; i < imageWidth ; i++)
        for(int j = 0 ; j < imageHeight ; j++)
          if (image[i, j])
            digitImages.Add(createDigitImage(findDigit(i, j)));

      return digitImages;
    }

    private int[,] createDigitImage(Pair<Point> pair)
    {
      int[,] digitImage = new int[pair.second.X-pair.first.X + 1,pair.second.Y-pair.first.Y +1];

      for(int i =  pair.first.X ; i<= pair.second.X ; i++)
        for (int j = pair.first.Y; j <= pair.second.Y; j++)
          digitImage[i - pair.first.X, j - pair.first.Y] = imageZero[i, j];

      return digitImage;
    }
    private Pair<Point> compare(Pair<Point> pair1, Pair<Point> pair2)
    {
      Pair<Point> pair = new Pair<Point>(new Point(), new Point());
      if (pair1.first.X < pair2.first.X)
        pair.first.X = pair1.first.X;
      else
      {
        pair.first.X = pair2.first.X;
      }
      if (pair1.first.Y < pair2.first.Y)
        pair.first.Y = pair1.first.Y;
      else
      {
        pair.first.Y = pair2.first.Y;
      }

       if (pair1.second.X > pair2.second.X)
        pair.second.X = pair1.second.X;
      else
      {
        pair.second.X = pair2.second.X;
      }
       if (pair1.second.Y > pair2.second.Y)
        pair.second.Y = pair1.second.Y;
      else
      {
        pair.second.Y = pair2.second.Y;
      }
      return pair;
    }
    private Pair<Point> findDigit(int x, int y)
    {
      Pair<Point> border = new Pair<Point>( new Point(imageWidth, imageHeight), new Point(0, 0));

      image[x, y] = 0;
      for (int i = 0; i < 8; i++)
      {
        if (x + dx[i] >= 0 && x + dx[i] < imageWidth && y + dy[i] >= 0 && y + dy[i] < imageHeight)
          if (image[x + dx[i], y + dy[i]])
            border = compare(findDigit(x + dx[i], y + dy[i]), border);
      }
      return border;
    }
  }
}
