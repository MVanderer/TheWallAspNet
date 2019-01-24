using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheWall.Data;
using TheWall.Models;

namespace TheWall.Controllers {
    public class HomeController : Controller {
        public DataContext dbContext;
        public HomeController (DataContext context) {
            dbContext = context;
        }
        // GET: /Home/
        [HttpGet]
        [Route ("")]
        public IActionResult Index () {
            if (CheckLogin ()) {
                List<Message> allMessages = dbContext.Messages
                    .Include (m => m.Author)
                    .Include (m => m.Comments)
                    .ThenInclude (c => c.Author)
                    .ToList ();
                ViewBag.allMessages = allMessages;
                return View ();

            } else {
                return RedirectToAction ("UserGate");
            }
        }

        [HttpGet ("logout")]
        public IActionResult Logout () {
            HttpContext.Session.Clear ();
            return RedirectToAction ("UserGate");
        }

        [HttpGet ("gate")]
        public IActionResult UserGate () {
            return View ();
        }

        [HttpPost ("register")]
        public IActionResult Register (User user) {
            if (ModelState.IsValid) {
                if (dbContext.Users.Any (u => u.Email == user.Email)) {
                    ModelState.AddModelError ("Email", "A user with this e-mail already exists.");
                    return View ("UserGate");
                } else {
                    PasswordHasher<User> Hasher = new PasswordHasher<User> ();
                    user.Password = Hasher.HashPassword (user, user.Password);
                    dbContext.Add (user);
                    dbContext.SaveChanges ();
                    HttpContext.Session.SetInt32 ("LoggedInUserID", user.UserId);
                    HttpContext.Session.SetString ("LoggedInUserName", user.FirstName);
                    return RedirectToAction ("Index");
                }

            } else {
                return View ("UserGate");
            }
        }

        [HttpPost ("login")]
        public IActionResult Login (Login user) {
            if (ModelState.IsValid) {
                User dbUser = dbContext.Users.FirstOrDefault (u => u.Email == user.LoginEmail);
                if (dbUser == null) {
                    ModelState.AddModelError ("LoginEmail", "No user with this e-mail exists");
                    return View ("UserGate");
                } else {
                    var hasher = new PasswordHasher<Login> ();
                    var result = hasher.VerifyHashedPassword (user, dbUser.Password, user.LoginPassword);
                    if (result != 0) {
                        HttpContext.Session.SetInt32 ("LoggedInUserID", dbUser.UserId);
                        HttpContext.Session.SetString ("LoggedInUserName", dbUser.FirstName);
                        return RedirectToAction ("Index");
                    } else {
                        ModelState.AddModelError ("Password", "The password is incorrect");
                        return View ("UserGate");
                    }
                }

            } else {
                return View ("UserGate");
            }
        }

        public bool CheckLogin () {
            //Additional tests here, as needed
            if (HttpContext.Session.GetInt32 ("LoggedInUserID") != null) {
                return true;
            } else {
                return false;
            }
        }

        //custom messages stuff
        [HttpPost ("message")]
        public IActionResult NewMessage (Message mes) {
            if (ModelState.IsValid) {
                mes.AuthorId = (int) HttpContext.Session.GetInt32 ("LoggedInUserID");
                dbContext.Add (mes);
                dbContext.SaveChanges ();
                return RedirectToAction ("Index");
            } else {
                return View ("Index");
            }
        }

        [HttpPost ("comment")]
        public IActionResult NewComment (Comment comment) {
            System.Console.WriteLine("--------------------------------------------");
            System.Console.WriteLine(comment.MessageId);
            if (ModelState.IsValid) {
                comment.AuthorId=(int)HttpContext.Session.GetInt32("LoggedInUserID");
                dbContext.Add(comment);
                dbContext.SaveChanges();
                return RedirectToAction ("Index");

            } else {
                return View ("Index");

            }
        }

        [HttpGet("deletemessage/{mesId}")]
        public IActionResult MessageDelete(int mesId){
            Message mes = dbContext.Messages.FirstOrDefault(m=>m.MessageId==mesId);
            dbContext.Remove(mes);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet("deletecomment/{comId}")]
        public IActionResult CommentDelete(int comId){
            Comment com = dbContext.Comments.FirstOrDefault(m=>m.CommentId==comId);
            dbContext.Remove(com);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}