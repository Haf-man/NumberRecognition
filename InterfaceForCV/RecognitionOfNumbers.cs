using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace InterfaceForCV
{
    public class RecognitionOfNumbers
    {
    private string _path = "distribution.in";
    private string _logPath = "results.log";
    private List<int[]> distributions;
    private int[] digitDistribution;
    private int numberOfExamples;
    public RecognitionOfNumbers()
    {
      distributions = new List<int[]>();
      if (File.Exists(_path))
      {

        string[] database = File.ReadAllLines(_path);

        foreach (var digitDescription in database)
        {
          int tmpResult = 0;
          if (Int32.TryParse(digitDescription, out tmpResult))
          {
            numberOfExamples = tmpResult;
            continue;
          }
          int[] distribution = new int[4];
          string[] nums = digitDescription.Split(' ');
          for (int i = 0; i < 4; i++)
          {
            distribution[i] = Convert.ToInt32(nums[i]);
          }
          distributions.Add(distribution);
        }
      }
      else
      {
        throw new Exception("Can't find destribution file");
      }

    }

    public void clarifyDistribution(int[] distribution, int digit)
    {
      int[] tmpDistribution = distributions.ElementAt(digit);
      for (int i = 0; i < 4; i++)
      {
        tmpDistribution[i] = (numberOfExamples * tmpDistribution[i] + distribution[i]) / (numberOfExamples + 1);
      }
      distributions[digit] = tmpDistribution;
      numberOfExamples++;
    }
    public void saveToFile()
    {
      using (StreamWriter sw = new StreamWriter(_path))
      {
        sw.WriteLine(numberOfExamples.ToString());
        foreach (var distribution in distributions)
        {
          string savingString = "";

          for (int i = 0; i < 4; i++)
          {
            savingString += distribution[i].ToString();
            if (i != 3)
              savingString += ' ';
          }
          sw.WriteLine(savingString);
        }

      }
    }
    public void convertDistributionToDouble()
    {
      for (int i = 0; i < 10; i++)
        for (int j = 0; j < 4; j++)
          standardFrequance[i][j] = (double)(distributions[i][j])/100;
    }
    public int [] Recognize(Image image)
        {
            int[] numbers = { 1, 2, 3 };
            return numbers;
        }
        //эталонные значения частот
        private double[][] standardFrequance = 
        {new double[]{0.25, 0.25, 0.25, 0.25},
         new double[]{0.2,  0.5,  0.3,  0},
         new double[]{0.2,  0.3,  0.19, 0.31},
         new double[]{0.15, 0.35, 0.35, 0.15},
         new double[]{0.33, 0.33, 0.33, 0},
         new double[]{0.36, 0.15, 0.35, 0.14},
         new double[]{0.29, 0.12, 0.29, 0.3},
         new double[]{0.19, 0.49, 0.02, 0.3},
         new double[]{0.25, 0.25, 0.25, 0.25},
         new double[]{0.29, 0.3,  0.29, 0.12},
        };
        int [] oneClosedArea = {0, 6, 9};
        int [] noClosedArea = {1,2,3,4,5,7};
    //евклидова метрика
    private double distance(double [] x,double [] y, int N)
        {
            double sum = 0;
            for(int i = 0;i<N;++i)
            {
                sum += (x[i] - y[i])*(x[i] - y[i]);
            }
            return Math.Sqrt(sum);
        }
    //выделить самое похожее число с данным количеством замкнутых областей
    private int selectSelectedDigit(double [] freq,int [] possibleDigits,int N)
        {
            int resDig = possibleDigits[0];
            double minDist = distance(freq,standardFrequance[resDig],4);
            for(int i = 1;i < N; ++i)
            {
                int currDig = possibleDigits[i];
                double currDist =  distance(freq,standardFrequance[currDig],4);
                if(currDist < minDist)
                {
                    resDig = currDig;
                    minDist = currDist;
                }
            }
            return resDig;
        }
    //наиболее похожая цифра
    private int selectDigit(double [] freq,int numOfClosedArea)
        {
            int res = -1;
            if(numOfClosedArea == 2)
            {
                res = 8;
            }
            else
            {
                if(numOfClosedArea == 1)
                {
                    res = selectSelectedDigit(freq, oneClosedArea, 3);
                }
                else
                {
                    if(numOfClosedArea == 0)
                    {
                        res = selectSelectedDigit(freq, noClosedArea, 6);
                    }
                }
            }
            return res;
        }
        
        //вычисление относительных частот
        private double [] getStatistic(int [,] image,int N,int M)
        {
            int [] partialCount = new int[4];
            //константы для корректировки в случае нечетного количества пикселей
            int ci = N%2;
            int cj = M%2;
            for(int i = 0;i<N/2;++i)
            {
                for(int j = 0;j<M/2;++j)
                {
                    if(image[i,j] == 1)
                    {
                        ++partialCount[0];
                    }
                    if(image[i,j+M/2+cj] == 1)
                    {
                        ++partialCount[1];
                    }
                    if(image[i+N/2+ci,j+M/2+cj] == 1)
                    {
                        ++partialCount[2];
                    }
                    if(image[i+N/2+ci,j] == 1)
                    {
                        ++partialCount[3];
                    }
                }
            }
            int generalCount = 0;
            for(int i = 0; i<4;++i)
            {
                generalCount += partialCount[i];
            }
            double [] freq = new double[4];
            for(int i = 0; i<4;++i)
            {
                freq[i] = (double)(partialCount[i])/generalCount;
            }
            return freq;
        }

    public int recognizeDigit(int [,] image,int N,int M,int numOfClosedArea)
        {
            double [] freq = getStatistic(image,N, M);
            int res = selectDigit(freq, numOfClosedArea);
        // need to add real digit value
            if(File.Exists(_logPath))
            {
              string[] line = new string[1];
              line[0] = res.ToString() + " : ";

              for (int i = 0; i< 4; i++)
                {
                  line[0] += Convert.ToInt32(freq[i] * 100).ToString();
                  if (i != 3)
                    line[0] += " ";
                }
             File.AppendAllLines(_logPath, line);
            }

            return res;
        }
    }
}
