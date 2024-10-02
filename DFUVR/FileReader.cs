using System.IO;
using System;
using UnityEngine;
namespace DFUVR
{
    //pretty self-explanatory. This class reads the contents from a file. In this case, we're only dealing with txt files
    public class FileReader
    {
        public static string ReadFromFile(string filePath)
        {
            try
            {
                string content = File.ReadAllText(filePath);
                return content;
            }
            catch (Exception exception)//if we get an error, it's probably because the file just doesn't exist
            {
                Application.Quit();
                return "Whoopsie, file not found/read error. You done fucked up lmao";


            }

        }
    }
}