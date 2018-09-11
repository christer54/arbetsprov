using System;

namespace MusicCollector
{
    public class ErrorHandler
    {
        private long _count = 0;

        public void addError(Exception e)
        {
            string str = string.Format("{0} | Object error nr={1}\t| {2}\n",DateTime.Now,_count++,e.Message);
            System.Console.WriteLine(str);
            System.IO.File.AppendAllText(@"../MusicCollector/raw_data/error_collection.text", str);

            //Console.ReadKey();
        }
    }
}