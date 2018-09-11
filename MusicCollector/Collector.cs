namespace MusicCollector
{
    using System;
    using System.Linq;

    using WebConnect;
    using Parse;

    
    public abstract class Collector
    {
        public Holder.DataHolder holder;
        public Collector(Holder.DataHolder h)
        {
            this.holder = h;
        }
        public abstract void searchArtist(SearchItems search);
        public abstract void getArtistById(string id);
        public abstract void getTracksByArtistId(string id);
        public abstract void getRelatedArtistsById(string id);
        public abstract void searchTrack(string artist, string track);
        public abstract void searchTrack(SearchItems search);
        public abstract void searchGenre(string genre);
        public abstract void listGenres();

    }
    public class SpotifyCollector : Collector
    {
        private ErrorHandler error = new ErrorHandler();
        
        public SpotifyCollector(Holder.DataHolder h) : base(h)
        {
        }
        HttpConnector connector = new HttpConnector();

        public override void searchTrack(SearchItems search)
        {
            int offset = 0;
            //string url = string.Format("https://api.spotify.com/v1/search?q={0} {1}&type=track&market=US&limit=10&offset={2}",search.track,search.artist,offset.ToString());
            string url = string.Format("https://api.spotify.com/v1/search?q={0} {1}&type=track&limit=10&offset={2}",search.track,search.artist,offset.ToString());
            var jsonData = connector.getJsonData(url);
            //Console.WriteLine(jsonData);
  
            //System.IO.File.WriteAllText(@"../MusicCollector/raw_data/search_result.json", jsonData);

            try
            {
                var parse = new CollectTracks(jsonData);
                Console.WriteLine("Laddar på med Tracks");
                parse.collect(holder);
            }
            catch(Exception e)
            {
                error.addError(e);
            }
        }

        public override void searchTrack(string trackArtist, string trackTitle)
        {
            searchTrack(new SearchItems(){artist=trackArtist,track=trackTitle});
        }
        public override void searchGenre(string genre)
        {

        }

        private string listGenres(string url)
        {
            var jsonData = connector.getJsonData(url);
            //System.IO.File.WriteAllText(@"../MusicCollector/raw_data/categories_result.json", jsonData);
            string next_url = "";
            try
            {
                var parse = new CollectCategories(jsonData);
                Console.WriteLine("Laddar på med Cateregories");
                parse.collect(holder);
                next_url = parse.next;
            }
            catch(Exception e)
            {
                error.addError(e);
            }
            return next_url;
        }
        public override void listGenres()
        {

            string url = "https://api.spotify.com/v1/browse/categories?offset=0&limit=20";
            while(String.IsNullOrEmpty(url) == false)
            {
                url = listGenres(url);
            }
        }

        public override void getRelatedArtistsById(string artistId)
        {
            string url = string.Format("https://api.spotify.com/v1/artists/{0}/related-artists",artistId);
            var jsonData = connector.getJsonData(url);
            //System.IO.File.WriteAllText(@"../MusicCollector/raw_data/related_artists_result.json", jsonData);

            try
            {
                var parse = new CollectRelated(jsonData);
                Console.WriteLine("Laddar på med Related");
                parse.collect(holder);
            }
            catch(Exception e)
            {
                error.addError(e);
            }
            
        }
        public override void getArtistById(string artistId)
        {
            string url = string.Format("https://api.spotify.com/v1/artists/{0}",artistId);
            var jsonData = connector.getJsonData(url);
            //System.IO.File.WriteAllText(@"../MusicCollector/raw_data/artist_result.json", jsonData);
        }
        public override void getTracksByArtistId(string artistId)
        {
            string url = string.Format("https://api.spotify.com/v1/artists/{0}/top-tracks?country=SE&type=track",artistId);
            var jsonData = connector.getJsonData(url);
            //System.IO.File.WriteAllText(@"../MusicCollector/raw_data/artist_result.json", jsonData);
        }

        public void link(string url)
        {
            var jsonData = connector.getJsonData(url);
            //System.IO.File.WriteAllText(@"../MusicCollector/raw_data/url_result.json", jsonData);
        }
        public override void searchArtist(SearchItems search)
        {
            if(String.IsNullOrEmpty(search.genre))
                search.genre = "*";
                
            if(search.genre.Contains(' '))
            {
                search.genre.Trim();
                string[] genreParts = search.genre.Split(' ');
                string tmp = "%22";
                
                for(int n = 0 ; n < genreParts.Count(); n++)
                {
                    tmp += genreParts[n];
                    if(n+1 != genreParts.Count())
                        tmp += "%20";
                }
                tmp += "%22";
                Console.WriteLine("HAR MELLANRUM");
                search.genre = tmp;
                Console.WriteLine(search.genre);
                //Console.ReadKey();
            }
            
            string str = string.Format("q={0}%20genre:{1}&type=artist",search.artist,search.genre);
            string url = string.Format("https://api.spotify.com/v1/search?{0}",str);
            var jsonData = connector.getJsonData(url);
            //System.IO.File.WriteAllText(@"../MusicCollector/raw_data/test_result.json", jsonData);

            try
            {
                var parse = new CollectArtists(jsonData);
                Console.WriteLine("Laddar på med Artists");
                parse.collect(holder);
            }
            catch(Exception e)
            {
                error.addError(e);
            }

        }
    }
}

            