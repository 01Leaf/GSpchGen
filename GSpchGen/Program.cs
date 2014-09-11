using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Globalization;

//NAudio
using NAudio;
using NAudio.Wave;


//Debug
using System.Diagnostics;

namespace GSpchGen
{
    class Program
    {
        static void Main(string[] args)
        {
            string sLang = "en";
            string sText = "";
            Uri uriMp3;
            string sMp3Path;
            string sWavPath;


            //Read text from text.txt
            sText = File.ReadAllText(Environment.CurrentDirectory + @"\text.txt", Encoding.UTF8);

            //TODO: Detect Language
            //using (WebClient wcLang = new WebClient())            
            //{
            //    Uri uriLang = new Uri("http://translate.google.com/translate_a/t?client=t&sl=auto&text=" + sText);
            //    wcLang.DownloadFile(uriLang, "lang.txt");
            //    Console.WriteLine(File.ReadAllText("lang.txt"));
            //    Console.ReadLine();
            //}

            uriMp3 = new Uri("http://translate.google.cn/translate_tts?tl=" + sLang + "&q=" + sText);  
            sMp3Path = Environment.CurrentDirectory + @"\res\sound\tmp.mp3";
            sWavPath=Environment.CurrentDirectory+@"\res\sound\"+sText+".wav";

            //Get MP3 from Google
            using (WebClient wcMp3Speech = new WebClient())
            {
                wcMp3Speech.DownloadFile(uriMp3, sMp3Path);
            }

            //Now convert the MP3 to WAV
            using (Mp3FileReader reader = new Mp3FileReader(new FileStream(sMp3Path,FileMode.OpenOrCreate)))
            {
                WaveFileWriter.CreateWaveFile(sWavPath, reader);
            }

            //Now copy the WAV to speech.wav
            if (File.Exists(Environment.CurrentDirectory + @"\res\sound\speech.wav"))
            {
                File.Delete(Environment.CurrentDirectory + @"\res\sound\speech.wav");
            }

            File.Copy(sWavPath, Environment.CurrentDirectory + @"\res\sound\speech.wav");
        }
    }
}
