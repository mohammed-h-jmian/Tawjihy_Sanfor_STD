using Microsoft.AspNetCore.Mvc;
using tawjihy.Data.Models;
using tawjihy.Data;
using System.Text;
using NPOI.XSSF.UserModel;
using System.IO;
using OfficeOpenXml;
using Microsoft.Extensions.Caching.Memory;

namespace tawjihy.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;


        public HomeController(IMemoryCache memoryCache, IWebHostEnvironment hostingEnvironment, ApplicationDbContext context) : base(memoryCache)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        public IActionResult Index()
        {
            var studentCount = _context.Students.Count();
            var majoringCount = _context.Majorings.Count();
            var universityCount = _context.Universities.Count();
            var visitorsCount = _context.Visitors.Count();

            ViewBag.StudentCount = studentCount;
            ViewBag.MajoringCount = majoringCount;
            ViewBag.UniversityCount = universityCount;
            ViewBag.VisitorsCount = visitorsCount;

            return View();
        }
        [HttpGet]
        public IActionResult DownloadExcelFile()
        {
            var visitors = _context.Visitors.ToList();

            byte[] fileContents;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Visitors");

                // Add header row
                var properties = typeof(Visitor).GetProperties();
                for (int col = 0; col < properties.Length; col++)
                {
                    worksheet.Cells[1, col + 1].Value = properties[col].Name;
                }

                // Add data rows
                for (int row = 0; row < visitors.Count; row++)
                {
                    for (int col = 0; col < properties.Length; col++)
                    {
                        worksheet.Cells[row + 2, col + 1].Value = properties[col].GetValue(visitors[row]);
                    }
                }

                fileContents = package.GetAsByteArray();
            }

            // Return the Excel file as a FileResult with appropriate content type and file name
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Visitors.xlsx");
        }

        [HttpPost]
        public async Task<IActionResult> UploadStudents(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                return RedirectToAction("Index");
            }

            try
            {
                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                    if (worksheet == null)
                    {
                        return RedirectToAction("Index");
                    }

                    var rowCount = worksheet.Dimension.Rows;

                    var students = new List<Student>();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            var student = new Student
                            {
                                SeatingNo = int.Parse(worksheet.Cells[row, 1].Value.ToString()),
                                IdNo = int.Parse(worksheet.Cells[row, 2].Value.ToString()),
                                Name = worksheet.Cells[row, 3].Value.ToString(),
                                Branch = worksheet.Cells[row, 4].Value.ToString(),
                                Gender = worksheet.Cells[row, 5].Value.ToString(),
                                Rate = double.Parse(worksheet.Cells[row, 6].Value.ToString()),
                                SchoolName = worksheet.Cells[row, 7].Value?.ToString() ?? string.Empty,
                                DirectorateName = worksheet.Cells[row, 8].Value?.ToString() ?? string.Empty,
                                PhoneNumber = worksheet.Cells[row, 9].Value?.ToString() ?? string.Empty,
                            };


                            students.Add(student);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                    ClearCacheMethod();
                    _context.Students.AddRange(students);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return Redirect("/Admin/Base/ErrorUpload");
            }

        }

        [HttpGet]
        public async Task<IActionResult> DeleteAllStudents()
        {
            try
            {
                _context.Students.RemoveRange(_context.Students);
                await _context.SaveChangesAsync();
                ClearCacheMethod();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Redirect("/Admin/Base/ErrorDelete");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadMajorings(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                return RedirectToAction("Index");
            }

            try
            {
                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                    if (worksheet == null)
                    {
                        return RedirectToAction("Index");
                    }

                    var rowCount = worksheet.Dimension.Rows;

                    var majorings = new List<Majoring>();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            var majoring = new Majoring
                            {
                                University = worksheet.Cells[row, 1].Value.ToString(),
                                Name = worksheet.Cells[row, 2].Value.ToString(),
                                Gender = worksheet.Cells[row, 3].Value?.ToString() ?? string.Empty,
                                College = worksheet.Cells[row, 4].Value.ToString(),
                                Degree = worksheet.Cells[row, 5].Value.ToString(),
                                Scientific = double.TryParse(worksheet.Cells[row, 6].Value?.ToString(), out double scientificValue) ? scientificValue : 0,
                                Literary = double.TryParse(worksheet.Cells[row, 7].Value?.ToString(), out double literaryValue) ? literaryValue : 0,
                                Industrial = double.TryParse(worksheet.Cells[row, 8].Value?.ToString(), out double industrialValue) ? industrialValue : 0,
                                Agrarian = double.TryParse(worksheet.Cells[row, 9].Value?.ToString(), out double agrarianValue) ? agrarianValue : 0,
                                Entrepreneurship = double.TryParse(worksheet.Cells[row, 10].Value?.ToString(), out double entrepreneurshipValue) ? entrepreneurshipValue : 0,
                                Lawful = double.TryParse(worksheet.Cells[row, 11].Value?.ToString(), out double lawfulValue) ? lawfulValue : 0,

                            };

                            majorings.Add(majoring);
                        }
                        catch (Exception)
                        {
                            continue;
                        }


                    }
                    ClearCacheMethod();
                    _context.Majorings.AddRange(majorings);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Redirect("/Admin/Base/ErrorUpload");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAllMajorings()
        {
            try
            {
                _context.Majorings.RemoveRange(_context.Majorings);
                await _context.SaveChangesAsync();
                ClearCacheMethod();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Redirect("/Admin/Base/ErrorDelete");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UploadUniversity(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                return RedirectToAction("Index");
            }

            try
            {
                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                    if (worksheet == null)
                    {
                        return RedirectToAction("Index");
                    }

                    var rowCount = worksheet.Dimension.Rows;

                    var universities = new List<University>();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            var university = new University
                            {
                                Name = worksheet.Cells[row, 1].Value.ToString(),
                                URL = worksheet.Cells[row, 2].Value?.ToString() ?? string.Empty,
                            };
                            universities.Add(university);
                        }
                        catch (Exception)
                        {
                            continue;
                        }


                    }
                    ClearCacheMethod();
                    _context.Universities.AddRange(universities);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Redirect("/Admin/Base/ErrorUpload");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAllUniversities()
        {
            try
            {
                _context.Universities.RemoveRange(_context.Universities);
                await _context.SaveChangesAsync();
                ClearCacheMethod();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Redirect("/Admin/Base/ErrorDelete");
            }
        }
    }
}

