using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InterfaceForCV
{
  class ImageDatabaseKeeper
  {
    List<Tuple<int, int[]>> imageDatabase;
    string pathToDatabase = "imageBase.in";
    public ImageDatabaseKeeper()
    {
      imageDatabase = new List<Tuple<int, int[]>>();
      ReadDatabase();
    }
    private void ReadDatabase()
    {
      if(File.Exists(pathToDatabase))
      {  

          string[] database =  File.ReadAllLines(pathToDatabase);
          foreach (var digitDescription in database)
          {
            int k = 0;
            int[] digitRepresentation = new int[32 * 32];
          char digitTmp = digitDescription[0];
            int digit = (int)Char.GetNumericValue(digitTmp);
            for (int i = 1; i <= 32*32; i++)
            {
            char tmp = digitDescription[i];
              digitRepresentation[k] = (int)Char.GetNumericValue(tmp);
            k++;
            }
            imageDatabase.Add(new Tuple<int, int[]>(digit, digitRepresentation));
          }
       }
      else
      {
        FileStream fs = File.Create(pathToDatabase);
      }
    }
    public void AddToDatabase(int digit, int[] image)
    {
      imageDatabase.Add(new Tuple<int, int[]>(digit, image));
    }
    public void saveToFile()
    {
      using (StreamWriter sw = new StreamWriter(pathToDatabase))
      {
        foreach (var image in imageDatabase)
        {
          string savingString = image.Item1.ToString();

          for (int i = 0; i < image.Item2.Length; i++)
            savingString += image.Item2[i].ToString();
          sw.WriteLine(savingString);
        }

      }
    }
    public int determinateDigit(int[] digit)
    {
      int returnValue = 0;
      int maxDistant = 32 * 32;
      foreach (var pattern in imageDatabase)
      {
        int distant = 0;
        for (int i = 0; i < 32 * 32; i++)
        {
          if (pattern.Item2[i] != digit[i])
            distant++;
        }
        if(distant<maxDistant)
        {
          maxDistant = distant;
          returnValue = pattern.Item1;
        }
      }
      return returnValue;
    }
  }
}
