using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MusicCollector.Holder
{
    public class Years
    {
        int first=0, latest=0;
        public void check(int year)
        {
            if(first==0 || year<first)
                first = year;
            if(latest==0 || year>latest)
                latest = year;
        }
    }
    public struct IdName
    {
        public string id;
        public string name;
    }
    public class InfoArtist
    {
        public InfoArtist(string id)
        {
            this.id = id;
        }
        public string name;
        public string id;
        public string[] genres;
        public string[] related_artists;
        public List<IdName> relatedArtists = new List<IdName>();
        public Years years = new Years();
        public string[] tracks;
    }
    public class DataHolder
    {
        public SearchItems searchItems;
        private Dictionary<string, string> _dictionaryGenres;
        private Dictionary<string, InfoArtist> _dictionaryArtists;
        public DataHolder()
        {
            _dictionaryArtists = new Dictionary<string, InfoArtist>();
            _dictionaryGenres = new Dictionary<string, string>();
            searchItems = new SearchItems();
        }
        private InfoArtist get(string id)
        {
            if (_dictionaryArtists.ContainsKey(id) == false)
                _dictionaryArtists[id] = new InfoArtist(id);
            return _dictionaryArtists[id];
        }
        public void add_name(string id, string name)
        {
            var info = get(id);
            info.name = name;
        }
        public void add_related_artists(string id, string[] related)
        {
            var info = get(id);
            info.related_artists = related;
        }
        public void add_relatedArtists(string id, IdName idName)
        {
            var info = get(id);
            info.relatedArtists.Add(idName);
        }
        public void add_genres(string id, string[] genres)
        {
            if(genres.Length>0)
            {
                var info = get(id);
                info.genres = genres;
                foreach (string part in genres)
                    add_genre(part.Trim());
            }  
        }

        public void add_genre(string genre)
        {
            _dictionaryGenres[genre] = genre;
        }
        public Dictionary<string, InfoArtist> getArtists()
        {
            return _dictionaryArtists;
        }
        public Dictionary<string, string> getGenresDictionary()
        {
            return _dictionaryGenres;
        }
        public List<string> getGenresList()
        {
            return _dictionaryGenres.Keys.ToList();
        }
        public List<IdName> getIdNameList()
        {
            List<IdName> list = new List<IdName>();
            foreach(var d in _dictionaryArtists)
            {
                list.Add(new IdName(){id = d.Value.id, name = d.Value.name});
            }
            return list;
        }
    }
}