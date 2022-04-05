using Final_Project_myTunes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Final_Project_myTunes.Controllers
{
    public class MusicController : Controller
    {

        public myTunesContext db { get; set; }

        public MusicController()
        {
            db = new myTunesContext();
        }


        public IActionResult Index()
        {
            List<Song> songs = db.Songs.Include(a => a.Artist).ToList();

            return View(songs);
        }


        public IActionResult ArtistSearch(int? id)
        {
            ViewBag.Artists = new SelectList(db.Artists, "Id", "Name");

            if (id != null)
            {
                try
                {
                    Artist artist = db.Artists.Include(s => s.Songs).ThenInclude(s => s.Purchases).First(a => a.Id == id);
                    return View(artist);
                }

                catch (Exception ex)
                {
                    return RedirectToAction("Error", new { message = "Something went wrong." });
                }
            }

            return View();
        }


        public IActionResult TopSellingSong()
        {

            var SongsByPurchase = db.Songs.Include(p => p.Purchases).Include(a => a.Artist).OrderByDescending(s => s.Purchases.Count());
            ViewBag.MostBoughtSong = SongsByPurchase.First();


            var SongsByRating = db.Songs.Include(p => p.Purchases).Include(a => a.Artist).OrderByDescending(s => s.AverageRating);
            ViewBag.HighestRatedSongs = SongsByRating.Take(3);

            // in class solution
            //var TopArtist = db.Artists.Include(s => s.Songs).ThenInclude(p => p.Purchases).OrderByDescending(a => a.Songs.Sum(s => s.Purchases.Count)).First();
            List<Artist> artists = db.Artists.Include(s => s.Songs).ThenInclude(s => s.Purchases).ToList();
            Artist topArtist = artists.OrderByDescending(a => a.Songs.Sum(s => s.Purchases.Count)).First();
            ViewBag.PurchaseCount = topArtist.Songs.Sum(s => s.Purchases.Count);
            ViewBag.TopArtist = topArtist;

            return View();
        }


        public IActionResult UserLibrary(int? userId)
        {
            ViewBag.Users = new SelectList(db.Users, "Id", "UserName");
            ViewBag.UserId = userId;

            if (userId != null)
            {

                try
                {
                    User user = db.Users.Include(p => p.Purchases).ThenInclude(s => s.Song).ThenInclude(a => a.Artist).First(u => u.Id == userId);
                    ViewBag.SortByArtist = user.Purchases.OrderBy(u => u.Song.Artist.Name);

                    DateTime expiryDate = DateTime.Now.AddDays(-30);
                    ViewBag.ExpiryDate = expiryDate;

                    return View(user);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error", new { message = "Something went wrong." });
                }

            }

            return View();
        }


        //GET
        public IActionResult RateSong(int? userId, int? songId)
        {
            if (userId != null && songId != null)
            {
                try
                {
                    Song song = db.Songs.Include(a => a.Artist).Include(p => p.Purchases).First(s => s.Id == songId);
                    ViewBag.CurrentRating = song.Purchases.First().UserRating;
                    ViewBag.UserId = userId;
                    ViewBag.SongId = songId;


                    return View(song);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error", new { message = "Something went wrong." });
                }

            }

            return View();
        }

        [HttpPost]
        public IActionResult RateSong(int? userId, int? songId, double? userRating)
        {

            if (userId != null && songId != null && userRating != null)
            {
                if (userRating <= 0 || userRating > 5)
                {
                    return RedirectToAction("Error", new { message = "Ratings must be between 1 and 5" });
                }

                else
                {
                    try
                    {
                        User user = db.Users.First(u => u.Id == userId);
                        Song song = db.Songs.Include(a => a.Artist).Include(p => p.Purchases).First(s => s.Id == songId);
                        var preExistingPurchase = song.Purchases.Where(p => p.UserId == userId).ToList();
                        var preExistingRating = preExistingPurchase.Select(p => p.UserRating);

                        preExistingPurchase.First().UserRating = userRating;
                        song.CalculateAvg();

                        db.SaveChanges();
                    }

                    catch (Exception ex)
                    {
                        return RedirectToAction("Error", new { message = "Something went wrong." });
                    }

                }
            }

            return RedirectToAction("UserLibrary", new { userId });
        }


        [HttpPost]
        public IActionResult RefundSong(int? userId, int? songId)
        {
            if (userId != null && songId != null)
            {
                User user = db.Users.First(u => u.Id == userId);
                Song song = db.Songs.Include(p => p.Purchases).First(s => s.Id == songId);
                Purchase purchase = song.Purchases.First(p => p.UserId == userId);

                try
                {
                    user.WalletBalance += song.Cost;
                    db.Purchases.Remove(purchase);

                    db.SaveChanges();

                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error", new { message = "Something went wrong." });
                }
            }

            return RedirectToAction("UserLibrary", new { userId });
        }


        [HttpPost]
        public IActionResult AddMoney(int? userId, decimal? money)
        {
            User user = db.Users.First(u => u.Id == userId);

            try
            {
                user.WalletBalance += money;

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = "Something went wrong." });
            }

            return RedirectToAction("UserLibrary", new { userId });
        }


        // GET
        public IActionResult BuySongs(int? userId)
        {
            ViewBag.Users = new SelectList(db.Users, "Id", "UserName");

            if (userId != null)
            {

                try
                {
                    User user = db.Users.Include(p => p.Purchases).ThenInclude(s => s.Song).ThenInclude(a => a.Artist).First(u => u.Id == userId);
                    var userPurchases = user.Purchases.ToList();
                    var UserSongs = userPurchases.Select(p => p.Song);
                    var AllSongs = db.Songs.Include(a => a.Artist).Include(p => p.Purchases).ThenInclude(u => u.User).ToList();

                    ViewBag.ErrorMessage = TempData["error"];
                    ViewBag.SongsToBuy = AllSongs.Except(UserSongs);

                    return View(user);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error", new { message = "Something went wrong." });
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult BuySongs(int? userId, int? songId)
        {
            if (userId != null && songId != null)
            {
                User user = db.Users.First(u => u.Id == userId);
                Song song = db.Songs.First(s => s.Id == songId);

                if (user.WalletBalance < song.Cost)
                {
                    TempData["error"] = "You do not have enough in your wallet";
                    return RedirectToAction("BuySongs", new { userId });
                }
                else
                {
                    try
                    {

                        Purchase newPurchase = new Purchase(song, user);
                        song.BuySong(newPurchase);
                        user.WalletBalance -= song.Cost;

                        db.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Error", new { message = "Something went wrong." });
                    }
                }
            }

            return RedirectToAction("BuySongs", new { userId });
        }


        public IActionResult Error(string? message)
        {
            if (message != null)
            {
                return View(message);
            }

            return View();
        }

    }

}

