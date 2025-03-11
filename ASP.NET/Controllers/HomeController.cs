using ASP.NET.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Project.Data;
using System.Collections;

namespace ASP.NET.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ORM db;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            db = new ORM("../My_lib/bin/Debug/net8.0/My_lib.dll", "Data Source=../Project/bin/Debug/net8.0/MyDatabase.db;", "My_lib.Models.Applicationn", "My_lib.Models.Highschool", "My_lib.Models.Specialization", "My_lib.Models.Student");
        }

        public IActionResult Index()
        {
            TempData["isRegisteredStudent"] = false;
            TempData["isRegisteredApplication"] = false;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RedirectToFormOptions()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterRedirectOptions()
        {
            return RedirectToAction("RedirectToFormOptions");
        }

        [HttpPost("Home/FormStudent")]
        public IActionResult FormStudent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterStudent(FormStudentModel model)
        {
            var studentType = ReflectionLoader.loadedTypes["Student"];
            var student = Activator.CreateInstance(studentType);
            model.Id_Student = (int)typeof(ORM).GetMethod("GetFreeId").MakeGenericMethod(studentType).Invoke(db, null);

            var modelProperties = model.GetType().GetProperties();
            foreach (var property in modelProperties)
            {
                var studentProperty = studentType.GetProperty(property.Name);
                studentProperty.SetValue(student, property.GetValue(model));
            }

            typeof(ORM).GetMethod("Save").Invoke(db, new object[] { student });

            TempData["isRegisteredStudent"] = true;

            return View("Index");
        }

        [HttpPost]
        public IActionResult FormApplication()
        {
            var applicationType = ReflectionLoader.loadedTypes["Applicationn"];
            var highschoolType = ReflectionLoader.loadedTypes["Highschool"];
            var studentType = ReflectionLoader.loadedTypes["Student"];
            var specializationType = ReflectionLoader.loadedTypes["Specialization"];

            var method = typeof(ORM).GetMethod("LoadAll").MakeGenericMethod(applicationType);
            var applications = method.Invoke(db, null) as IEnumerable<dynamic>;
            ViewData["Applications"] = applications;

            method = typeof(ORM).GetMethod("LoadAll").MakeGenericMethod(highschoolType);
            var highschools = method.Invoke(db, null) as IEnumerable<dynamic>;
            ViewData["Highschools"] = highschools;

            method = typeof(ORM).GetMethod("LoadAll").MakeGenericMethod(studentType);
            var students = method.Invoke(db, null) as IEnumerable<dynamic>;
            ViewData["Students"] = students;

            method = typeof(ORM).GetMethod("LoadAll").MakeGenericMethod(specializationType);
            var specializations = method.Invoke(db, null) as IEnumerable<dynamic>;
            ViewData["Specializations"] = specializations;

            return View();
        }

        public IActionResult RegisterApplication(FormApplicationModel model)
        {
            var applicationType = ReflectionLoader.loadedTypes["Applicationn"];
            var application = Activator.CreateInstance(applicationType);

            var GetFreeIdMethod = typeof(ORM).GetMethod("GetFreeId").MakeGenericMethod(ReflectionLoader.loadedTypes["Specialization"]);
            var freeId = (int)GetFreeIdMethod.Invoke(db, null);

            var modelProperties = model.GetType().GetProperties();
            foreach (var property in modelProperties)
            {
                var applicationProperty = applicationType.GetProperty(property.Name);
                if (property.Name == "Id_Applications")
                {
                    applicationProperty.SetValue(application, freeId);
                    continue;
                }
                applicationProperty.SetValue(application, property.GetValue(model));
            }

            var method = typeof(ORM).GetMethod("Save");
            method.Invoke(db, new object[] { application });

            TempData["isRegisteredApplication"] = true;

            return View("Index");
        }

        public IActionResult ViewApplications()
        {
            var applicationType = ReflectionLoader.loadedTypes["Applicationn"];
            var highschoolType = ReflectionLoader.loadedTypes["Highschool"];
            var studentType = ReflectionLoader.loadedTypes["Student"];
            var specializationType = ReflectionLoader.loadedTypes["Specialization"];

            var method = typeof(ORM).GetMethod("LoadAll").MakeGenericMethod(applicationType);
            var applications = method.Invoke(db, null) as IEnumerable<dynamic>;
            ViewData["Applications"] = applications;
            
            method = typeof(ORM).GetMethod("LoadAll").MakeGenericMethod(highschoolType);
            var highschools = method.Invoke(db, null) as IEnumerable<dynamic>;
            ViewData["Highschools"] = highschools;

            method = typeof(ORM).GetMethod("LoadAll").MakeGenericMethod(studentType);
            var students = method.Invoke(db, null) as IEnumerable<dynamic>;
            ViewData["Students"] = students;
            
            method = typeof(ORM).GetMethod("LoadAll").MakeGenericMethod(specializationType);
            var specializations = method.Invoke(db, null) as IEnumerable<dynamic>;
            ViewData["Specializations"] = specializations;

            return View();
        }

        [HttpGet]
        public IActionResult GetSpecializations(int schoolId)
        {
            var LoadAllMethod = typeof(ORM).GetMethod("LoadAll").MakeGenericMethod(ReflectionLoader.loadedTypes["Specialization"]);
            var specializations = LoadAllMethod.Invoke(db, null) as IEnumerable<dynamic>;

            if (specializations != null)
            {
                var results = specializations
                    .Where(s => s.Id_School == schoolId)
                    .Select(s => new { id = s.Id_Specialization, label = s.Name }).ToList();

                return Json(results);
            }
            return Json(new List<dynamic>());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
