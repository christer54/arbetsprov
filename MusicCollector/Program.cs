using System;
using System.Collections.Generic;
namespace MusicCollector
{
    class Program
    {
        static void Main(string[] args)
        {

            Holder.DataHolder holder = new Holder.DataHolder();
            Collector webb = new SpotifyCollector(holder);

            string trackTitle = "How we move";
            string trackArtist = "Lil Bibby";
            Console.WriteLine("\n\nLetar rätt på en låt:");
            webb.searchTrack(trackArtist,trackTitle);
            
            Console.WriteLine("\n\nTestar lista {0} artister",holder.getArtists().Count);

            Console.WriteLine("{0} kategorier finns",holder.getGenresList().Count);
            var infos = holder.getArtists(); // pekar, är ingen kopia
            foreach(var idName in holder.getIdNameList())
            {
                Console.WriteLine("namn artist: {0} (id={1})\n\n\tRelaterade artister:",idName.name,idName.id);
                
                webb.getArtistById(idName.id);
                
                webb.getRelatedArtistsById(idName.id);
                Console.WriteLine("\n\n");
            }

            var list = webb.holder.getIdNameList();
            foreach(var idName in list)
            {
                Console.WriteLine("ID={0}, NAME={1}",idName.id,idName.name);
            }

            Console.WriteLine("Totalt {0} antal relaterade ",webb.holder.getIdNameList().Count);
            Console.WriteLine("{0} kategorier finns",holder.getGenresList().Count);

            var genres = holder.getGenresList();
            int nr = 0;
            foreach(var genre in genres)
            {
                Console.WriteLine("{0}\t\"{1}\"",++nr,genre);
            }

            Console.WriteLine("Testar ladda låtar från artisten {0}",list[0].name);
            webb.searchTrack(list[0].name,"*");


            webb.searchTrack("Denzel Curry","*");
        }
    }
}
