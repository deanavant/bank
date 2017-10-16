using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using bank.Models;

namespace bank.Controllers
{
    public class HomeController : Controller
    {
        private BankContext _context;

        public HomeController (BankContext context)
        {
            _context = context;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            HttpContext.Session.SetString("username","");
            HttpContext.Session.SetInt32("userid",0);
            return View("index");
        }

        [HttpGet]
        [Route("showReg")]
        public IActionResult showReg()
        {
            return View("register");
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterViewModel reg)
        {
            if(ModelState.IsValid)
            {
                List<User> u = _context.Users.Where(x => x.Email == reg.Email).ToList();
                if (u.Count == 0)
                {
                    // Console.WriteLine($"****{reg.Email} was not in db, need to add ");
                    User x = new User(reg.First,reg.Last,reg.Email,reg.Password);
                    _context.Add(x);
                    _context.SaveChanges();
                    HttpContext.Session.SetString("username",x.First);
                    HttpContext.Session.SetInt32("userid",x.Id);
                    Account a = new Account(0,x.Id);
                    _context.Add(a);
                    _context.SaveChanges();
                    string address = $"account/{x.Id}";
                    return Redirect(address);
                } else
                {
                    ModelState.AddModelError("Email",$"{reg.Email} already registered");
                }
            }
            return View("register");
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login (LoginViewModel log)
        {
            List<User> usrs = _context.Users.Where(x => x.Email == log.Email).ToList();
            if (usrs.Count < 1)
            {
                ModelState.AddModelError("Email",$"{log.Email} is not in database");
            } else
            {
                User x = usrs[0];
                if (x.Password != log.Password)
                {
                    ModelState.AddModelError("Password",$"Password does not match for {log.Email}");
                } else
                {
                    HttpContext.Session.SetString("username",x.First);
                    HttpContext.Session.SetInt32("userid",x.Id);
                    string address = $"account/{x.Id}";
                    return Redirect(address);
                }
            }
            return View("index");
        }

        [HttpPost]
        [Route("transact")]
        public IActionResult transact(TransactionViewModel t)
        {
            int? id = HttpContext.Session.GetInt32("userid");
            Account a = _context.Accounts.Where(x => x.UserId == id)
                .Include(x => x.Transactions)
                .SingleOrDefault();
            if (ModelState.IsValid)
            {
                if (a.Balance + t.Amount >= 0)
                {
                    a.Balance += t.Amount;
                    Transaction tr = new Transaction(t.Amount,DateTime.Now,a.Id);
                    _context.Add(tr);
                    _context.SaveChanges();
                    return Redirect("logout");
                }
                ModelState.AddModelError("Amount","Withdrawl cannot exceed balance");
            }
            ViewBag.name = HttpContext.Session.GetString("username");
            ViewBag.balance = a.Balance;
            List<string> results = new List<string>();
            foreach(Transaction tr in a.Transactions)
            {
                string d = tr.Date.ToString("MMM d yyyy");
                results.Add($"<td>${tr.Amount}</td><td>{d}</td>");
            }
            ViewBag.results = results;
            return View("account");
        }

        [HttpGet]
        [Route("account/{id}")]
        public IActionResult showAcount(int id)
        {
            int? uid = HttpContext.Session.GetInt32("userid");
            if(uid != id)
            {
                Console.WriteLine("*********  authentication problem");
                Console.WriteLine(id.ToString(),(int)HttpContext.Session.GetInt32("userid"));
                return Redirect("/");
            }
            ViewBag.name = HttpContext.Session.GetString("username");
            Account a = _context.Accounts.Where(x => x.UserId == id)
                .Include(t => t.Transactions)
                .SingleOrDefault();
            ViewBag.balance = a.Balance;
            HttpContext.Session.SetInt32("actid",a.Id);
            List<string> results = new List<string>();
            foreach(Transaction tr in a.Transactions)
            {
                string d = tr.Date.ToString("MMM d yyyy");
                results.Add($"<td>${tr.Amount}</td><td>{d}</td>");
            }
            ViewBag.results = results;
            return View("account");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
    }
}
