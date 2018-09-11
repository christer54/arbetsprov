using System.Collections.Generic;
using testMVC.Models;
using MusicCollector;
using MusicCollector.Holder;
namespace testMVC.ViewModels
{
    public class MusicViewModel
    {
        public string id {get;set;}
        public string artist {get;set;}
        public string track {get;set;}
        public string genre {get;set;}
        public SearchItems SearchItems {get;set;}
        public DataHolder DataHolder {get;set;}
    
    }
}