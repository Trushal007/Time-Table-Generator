using Microsoft.AspNetCore.Mvc;
using Time_Table_Generator.Models;

namespace Time_Table_Generator.Controllers
{
    public class TimeTableController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new TimeTableInputModel());
        }

        
        [HttpPost]
        public IActionResult EnterSubjectHours(TimeTableInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var subjectHoursModel = new SubjectHourInputModel
            {
                TotalHoursOfWeek = model.TotalHoursOfWeek,
                SubjectDetails = new List<SubjectHourDetail>()
            };

            for (int i = 0; i < model.TotalSubjects; i++)
            {
                subjectHoursModel.SubjectDetails.Add(new SubjectHourDetail());
            }

            TempData["NoOfWorkingDays"] = model.NoOfWorkingDays;
            TempData["NoOfSubjectsPerDay"] = model.NoOfSubjectsPerDay;
            TempData["TotalSubjects"] = model.TotalSubjects;
            TempData["TotalHoursOfWeek"] = model.TotalHoursOfWeek;

            return View("EnterSubjectHours", subjectHoursModel);
        }


        [HttpPost]
        public IActionResult GenerateTimetable(SubjectHourInputModel model)
        {
            var noOfWorkingDays = (int)TempData["NoOfWorkingDays"];
            var noOfSubjectsPerDay = (int)TempData["NoOfSubjectsPerDay"];
            var totalSubjects = (int)TempData["TotalSubjects"];
            var totalHoursOfWeek = (int)TempData["TotalHoursOfWeek"];

            TempData.Keep("NoOfWorkingDays");
            TempData.Keep("NoOfSubjectsPerDay");
            TempData.Keep("TotalSubjects");
            TempData.Keep("TotalHoursOfWeek");

            if (!ModelState.IsValid)
            {
                model.TotalHoursOfWeek = totalHoursOfWeek; 
                return View("EnterSubjectHours", model);
            }

            var sumOfSubjectHours = model.SubjectDetails.Sum(s => s.Hours);
            if (sumOfSubjectHours != totalHoursOfWeek)
            {
                Console.WriteLine("Validation error triggered");
                ModelState.AddModelError("", $"The sum of subject hours ({sumOfSubjectHours}) must be equal to the Total Hours for the Week ({totalHoursOfWeek}).");
                model.TotalHoursOfWeek = totalHoursOfWeek;
                return View("EnterSubjectHours", model);
            }


            var timetable = new string[noOfSubjectsPerDay, noOfWorkingDays];
            var subjectPool = new List<string>();

            foreach (var subject in model.SubjectDetails)
            {
                for (int i = 0; i < subject.Hours; i++)
                {
                    subjectPool.Add(subject.SubjectName);
                }
            }

            var rng = new Random();
            subjectPool = subjectPool.OrderBy(a => rng.Next()).ToList();

            int subjectIndex = 0;
            for (int day = 0; day < noOfWorkingDays; day++)
            {
                for (int period = 0; period < noOfSubjectsPerDay; period++)
                {
                    if (subjectIndex < subjectPool.Count)
                    {
                        timetable[period, day] = subjectPool[subjectIndex];
                        subjectIndex++;
                    }
                    else
                    {
                        timetable[period, day] = "N/A";
                    }
                }
            }

            var timetableViewModel = new TimeTableViewModel
            {
                NoOfWorkingDays = noOfWorkingDays,
                NoOfSubjectsPerDay = noOfSubjectsPerDay,
                TotalHoursOfWeek = totalHoursOfWeek,
                SubjectDetails = model.SubjectDetails,
                Timetable = timetable
            };

            return View("GeneratedTimetable", timetableViewModel);
        }

        public IActionResult About() {

            return View("About");
        }



    }
}
