using System;
using MusicCollector;
using MusicCollector.Holder;
using testMVC.ViewModels;

namespace testMVC.Models
{
    public class Test
    {
        
        public void search(MusicViewModel holder)
        {
            Console.WriteLine("Skickar och klockan är {0}",DateTime.Now);
            Console.WriteLine("Låt {0}",holder.track);
            Console.WriteLine("Artist {0}",holder.artist);
            Console.WriteLine("Genre {0}",holder.genre);
            Collector webb = new SpotifyCollector(holder.DataHolder);
            webb.searchTrack(holder.artist,holder.track);
            
            Console.WriteLine("\n\nTestar lista {0} artister",holder.DataHolder.getArtists().Count);

            Console.WriteLine("{0} kategorier finns",holder.DataHolder.getGenresList().Count);

            foreach(var idName in holder.DataHolder.getIdNameList())
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
            Console.WriteLine("{0} kategorier finns",holder.DataHolder.getGenresList().Count);
        }
    }
    
}