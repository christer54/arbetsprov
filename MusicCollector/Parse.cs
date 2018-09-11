namespace MusicCollector.Parse
{
    using System;
    using Newtonsoft.Json;
    using Spotify;
    using Holder;

    public abstract class Collect<Data>
    {
        public string next = "";
        protected Data _data;
        public Collect(Data t)
        {
            _data = t;
        }
        public abstract bool collect(DataHolder holder);

        public void read(string json)
        {
            _data = JsonConvert.DeserializeObject<Data>(json);
        }
        
    }
    
    public class CollectArtists : Collect<Spotify.Artists.Data>
    {
        public CollectArtists(Spotify.Artists.Data t) : base(t) {}
        public CollectArtists(string str) : base(Spotify.Artists.Data.FromJson(str)) {}
        public override bool collect(DataHolder holder)
        {
            if(_data == null)
            {
                Console.WriteLine("Misslyckades");
                return false;
            }
            else
            {   int nr = 0;
                foreach(var item in _data.Artists.Items)
                {
                    Console.WriteLine("Namn {0}: {1}",++nr,item.Name);
                    string id = item.Id;
                    holder.add_name(id,item.Name);
                    holder.add_genres(id,item.Genres);
                    Console.Write("\tGenre: ");
                    foreach(var itemSub in item.Genres)
                    {
                        Console.Write("{0}, ",itemSub);
                    }
                    Console.WriteLine();
                }
                return true;
            }
        }
    }
    public class CollectRelated : Collect<Spotify.Related.Related>
    {
        public CollectRelated(Spotify.Related.Related t) : base(t) {}
        public CollectRelated(string str) : base(Spotify.Related.Related.FromJson(str)) {}
        public override bool collect(DataHolder holder)
        {
            if(_data == null)
            {
                Console.WriteLine("Misslyckades");
                return false;
            }
            else
            {   int nr = 0;
                foreach(var item in _data.Artists)
                {
                    Console.WriteLine("Namn {0}: {1}",++nr,item.Name);
                    string id = item.Id;
                    holder.add_name(id,item.Name);
                    holder.add_genres(id,item.Genres);
                    Console.Write("\tGenre: ");
                    foreach(var itemSub in item.Genres)
                    {
                        Console.Write("{0}, ",itemSub);
                    }
                    Console.WriteLine();
                }
                return true;
            }
        }
    }    
    public class CollectTracks : Collect<Spotify.Tracks.Data>
    {
        public CollectTracks(Spotify.Tracks.Data t) : base(t) {}
        public CollectTracks(string str) : base(Spotify.Tracks.Data.FromJson(str)) {}
        public override bool collect(DataHolder holder)
        {
            if(_data == null)
            {
                Console.WriteLine("Misslyckades");
                return false;
            }
            else
            {   int nr = 0;
                Console.WriteLine("Antal hittade {0}",_data.Tracks.Total);
                foreach(var item in _data.Tracks.Items)
                {
                    Console.WriteLine("Namn {0}: {1}",++nr,item.Name);
                    foreach(var itemSub in item.Artists)
                    {
                        Console.Write("{0}, ",itemSub.Name);
                        Console.Write("{0}, ",itemSub.Id);
                        holder.add_name(itemSub.Id,itemSub.Name);
                    }
                    Console.WriteLine();
                }
                return true;
            }
        }
    }

    public class CollectCategories : Collect<Spotify.Categories.Data>
    {
        public CollectCategories(Spotify.Categories.Data t) : base(t) {}
        public CollectCategories(string str) : base(Spotify.Categories.Data.FromJson(str)) {}
        public override bool collect(DataHolder holder)
        {
            foreach(var item in _data.Categories.Items)
            {
                holder.add_genre(item.Name);
            }
            next = _data.Categories.Next;
            Console.WriteLine("Nästa {0}",next);    
            return false;
        }
        public bool collect()
        {
            return false;
        }
    }
}