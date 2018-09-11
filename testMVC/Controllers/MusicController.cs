using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using testMVC.Models;
using testMVC.ViewModels;
using MusicCollector;
using MusicCollector.Holder;

namespace testMVC.Controllers
{
    public class MusicController : Controller
    {
        Test test = new Test();
        private DataHolder _holder;
        //[Route("/Music/Index")]
        public ActionResult Index(string id,string artist,string track, string genre)
        {
            System.Console.WriteLine("artist = {0}",artist);
            System.Console.WriteLine("låt = {0}",track);
            _holder = new DataHolder();
            var search = new SearchItems(){artist = artist, track=track, genre = genre};

            var viewModel = new MusicViewModel
            {
                SearchItems = search,
                DataHolder = _holder,
                artist = artist,
                track = track,
                genre = genre,
                id = id
            };
            viewModel.artist = artist;
            viewModel.track = track;
            viewModel.genre = genre;
            test.search(viewModel);
            return View(viewModel);
        }
        public ActionResult Search(string id,string artist,string track, string genre)
        {
            _holder = new DataHolder();
            var search = new SearchItems(){artist=artist};
            //search.artist = artist;
            var viewModel = new MusicViewModel
            {
                SearchItems = search,
                DataHolder = _holder,
                artist = artist,
                track = track,
                genre = genre,
                id = id
            };
            test.search(viewModel);
            return View(viewModel);
        }
    }               
}