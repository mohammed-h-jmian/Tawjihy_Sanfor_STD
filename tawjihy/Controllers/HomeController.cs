using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using OfficeOpenXml;
using System.Diagnostics;
using System.Linq;
using tawjihy.Data;
using tawjihy.Data.Models;
using tawjihy.ViewModels;

namespace tawjihy.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public IActionResult Index()
        {
            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
                TempData.Remove("Message");
            }

            return View();
        }


        [HttpPost]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Search(int seatNumber)
        {
            if (seatNumber.ToString().Length != 6 && seatNumber.ToString().Length != 8)
            {
                TempData["Message"] = "يجب إدخال رقم مكون من 6 أو 8 أرقام.";
                return RedirectToAction("Index");
            }

            var student = await _context.Students.FirstOrDefaultAsync(s => s.SeatingNo.ToString().Contains(seatNumber.ToString()));

            if (student == null)
            {
                TempData["Message"] = "لا يوجد طالب بهذا الرقم";
                return RedirectToAction("Index");
            }

            student.Rate = Math.Round(student.Rate, 1);

            var majors = await GetMajorsByBranch(student.Branch, student.Rate);
            ViewBag.Majors = majors;

            await AddVisitor(seatNumber);
            return View(student);
        }

        private async Task<List<MajorViewModel>> GetMajorsByBranch(string branch, double rate)
        {
            IQueryable<Majoring> majorsQuery = _context.Majorings.AsNoTracking();

            switch (branch)
            {
                case "العلمي":
                    majorsQuery = majorsQuery.Where(m => m.Scientific != 0 && m.Scientific <= rate);
                    break;
                case "الأدبي":
                    majorsQuery = majorsQuery.Where(m => m.Literary != 0 && m.Literary <= rate);
                    break;
                case "الصناعي":
                    majorsQuery = majorsQuery.Where(m => m.Industrial != 0 && m.Industrial <= rate);
                    break;
                case "الزراعي":
                    majorsQuery = majorsQuery.Where(m => m.Agrarian != 0 && m.Agrarian <= rate);
                    break;
                case "الريادة والأعمال":
                    majorsQuery = majorsQuery.Where(m => m.Entrepreneurship != 0 && m.Entrepreneurship <= rate);
                    break;
                default:
                    majorsQuery = majorsQuery.Where(m => m.Lawful != 0 && m.Lawful <= rate);
                    break;
            }

            var majors = await majorsQuery
              .Select(m => new MajorViewModel
              {
                  Id = m.Id,
                  Name = m.Name,
                  University = m.University,
                  Gender = m.Gender,
                  Degree = m.Degree,
                  College = m.College,
                  Universitykey = branch == "العلمي" ? m.Scientific :
                                 branch == "الأدبي" ? m.Literary :
                                 branch == "الصناعي" ? m.Industrial :
                                 branch == "الزراعي" ? m.Agrarian :
                                 branch == "الريادة والأعمال" ? m.Entrepreneurship :
                                 m.Lawful
              })
              .ToListAsync();

            return majors;
        }
        
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public IActionResult About()
        {

            return View();
        }
       
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Universities()
        {
            var universities = await _context.Universities
                .ToListAsync();


            return View(universities);
        }
        //[HttpPost]
        //public async Task<IActionResult> Search(int seatNumber)
        //{
        //    if (seatNumber.ToString().Length != 6 && seatNumber.ToString().Length != 8)
        //    {
        //        TempData["Message"] = "يجب إدخال رقم مكون من 6 أو 8 أرقام.";
        //        return RedirectToAction("Index");
        //    }

        //    var student = await _context.Students.FirstOrDefaultAsync(s => s.SeatingNo.ToString().Contains(seatNumber.ToString()));

        //    if (student == null)
        //    {
        //        TempData["Message"] = "لا يوجد طالب بهذا الرقم";
        //        return RedirectToAction("Index");
        //    }

        //    student.Rate = Math.Round(student.Rate, 1);

        //    if (student.Branch.Contains("علمي"))
        //    {
        //        var majors = await _context.Majorings
        //            .Where(m => m.Scientific != 0 && m.Scientific <= student.Rate)
        //            .ToListAsync();

        //        var majorsVM = new List<MajorViewModel>();

        //        foreach (var major in majors)
        //        {
        //            var majorVM = new MajorViewModel
        //            {
        //                Id = major.Id,
        //                Name = major.Name,
        //                University = major.University,
        //                Gender = major.Gender,
        //                Degree = major.Degree,
        //                College = major.College,
        //                Universitykey = major.Scientific
        //            };

        //            majorsVM.Add(majorVM);
        //        }
        //        ViewBag.Majors = majorsVM;
        //    }
        //    else if (student.Branch.Contains("أدبي"))
        //    {
        //        var majors = await _context.Majorings
        //            .Where(m => m.Literary != 0 && m.Literary <= student.Rate)
        //            .ToListAsync();

        //        var majorsVM = new List<MajorViewModel>();

        //        foreach (var major in majors)
        //        {
        //            var majorVM = new MajorViewModel
        //            {
        //                Id = major.Id,
        //                Name = major.Name,
        //                University = major.University,
        //                Gender = major.Gender,
        //                Degree = major.Degree,
        //                College = major.College,
        //                Universitykey = major.Literary
        //            };

        //            majorsVM.Add(majorVM);
        //        }
        //        ViewBag.Majors = majorsVM;
        //    }
        //    else if (student.Branch.Contains("صناعي"))
        //    {
        //        var majors = await _context.Majorings
        //            .Where(m => m.Industrial != 0 && m.Industrial <= student.Rate)
        //            .ToListAsync();

        //        var majorsVM = new List<MajorViewModel>();

        //        foreach (var major in majors)
        //        {
        //            var majorVM = new MajorViewModel
        //            {
        //                Id = major.Id,
        //                Name = major.Name,
        //                University = major.University,
        //                Gender = major.Gender,
        //                Degree = major.Degree,
        //                College = major.College,
        //                Universitykey = major.Industrial
        //            };

        //            majorsVM.Add(majorVM);
        //        }
        //        ViewBag.Majors = majorsVM;
        //    }
        //    else if (student.Branch.Contains("زراعي"))
        //    {
        //        var majors = await _context.Majorings
        //            .Where(m => m.Agrarian != 0 && m.Agrarian <= student.Rate)
        //            .ToListAsync();

        //        var majorsVM = new List<MajorViewModel>();

        //        foreach (var major in majors)
        //        {
        //            var majorVM = new MajorViewModel
        //            {
        //                Id = major.Id,
        //                Name = major.Name,
        //                University = major.University,
        //                Gender = major.Gender,
        //                Degree = major.Degree,
        //                College = major.College,
        //                Universitykey = major.Agrarian
        //            };

        //            majorsVM.Add(majorVM);
        //        }
        //        ViewBag.Majors = majorsVM;
        //    }
        //    else if (student.Branch.Contains("ريادة"))
        //    {
        //        var majors = await _context.Majorings
        //            .Where(m => m.Entrepreneurship != 0 && m.Entrepreneurship <= student.Rate)
        //            .ToListAsync();

        //        var majorsVM = new List<MajorViewModel>();

        //        foreach (var major in majors)
        //        {
        //            var majorVM = new MajorViewModel
        //            {
        //                Id = major.Id,
        //                Name = major.Name,
        //                University = major.University,
        //                Gender = major.Gender,
        //                Degree = major.Degree,
        //                College = major.College,
        //                Universitykey = major.Entrepreneurship
        //            };

        //            majorsVM.Add(majorVM);
        //        }
        //        ViewBag.Majors = majorsVM;
        //    }
        //    else
        //    {
        //        var majors = await _context.Majorings
        //            .Where(m => m.Lawful != 0 && m.Lawful <= student.Rate)
        //            .ToListAsync();

        //        var majorsVM = new List<MajorViewModel>();

        //        foreach (var major in majors)
        //        {
        //            var majorVM = new MajorViewModel
        //            {
        //                Id = major.Id,
        //                Name = major.Name,
        //                University = major.University,
        //                Gender = major.Gender,
        //                Degree = major.Degree,
        //                College = major.College,
        //                Universitykey = major.Lawful
        //            };

        //            majorsVM.Add(majorVM);
        //        }
        //        ViewBag.Majors = majorsVM;
        //    }

        //    await AddVisitor(seatNumber);
        //    return View(student);
        //}


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<int> AddVisitor(int seatingNo)
        {
            var newVisitor = new Visitor { SeatingNo = seatingNo };
            await _context.Visitors.AddAsync(newVisitor);
            await _context.SaveChangesAsync();
            return newVisitor.Id;
        }
    }
}