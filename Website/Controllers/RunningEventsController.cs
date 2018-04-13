//Generated Scaffolding code from Entity Framework, modified to use a business library instead of direct EF

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Website.Models;
using Website.Business;
using Microsoft.VisualBasic.FileIO;

namespace Website.Controllers
{
    [SimpleMembership]
    public class RunningEventsController : Controller
    {
        // GET: RunningEvents
        public ActionResult Index()
        {
            var allActivities = WorkoutEventManager.ReadAll();
            return View(allActivities.OrderByDescending(r => r.Date));
        }

        // GET: RunningEvents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var workoutEvent = WorkoutEventManager.Read((int)id);
            if (workoutEvent == null)
            {
                return HttpNotFound();
            }
            return View(workoutEvent);
        }

        // GET: RunningEvents/Create
        public ActionResult Create()
        {
            ViewBag.ActivityId = new SelectList(WorkoutEventManager.GetActivityTypes(), "Id", "Name");
            ViewBag.PersonId = new SelectList(MembershipProvider.ReadAll(), "Id", "Name");
            ViewBag.UnitId = new SelectList(WorkoutEventManager.GetUnitOfMeasurements(), "Id", "Name");
            return View();
        }

        // POST: RunningEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PersonId,ActivityId,Date,Duration,Distance,UnitId,Title,Notes")] WorkoutEventViewModel formData)
        {
            if (ModelState.IsValid)
            {
                var workoutEvent = new WorkoutEvent()
                {
                    Id = formData.Id,
                    User = new User(formData.PersonId, ""),
                    Type = new ActivityType(formData.ActivityId, ""),
                    Date = formData.Date,
                    Duration = formData.Duration,
                    Distance = formData.Distance,
                    Unit = new UnitOfMeasurement(formData.UnitId, ""),
                    Title = formData.Title,
                    Notes = formData.Notes
                };
                WorkoutEventManager.Insert(workoutEvent);
                return RedirectToAction("Index");
            }

            ViewBag.ActivityId = new SelectList(WorkoutEventManager.GetActivityTypes(), "Id", "Name", formData.ActivityId);
            ViewBag.PersonId = new SelectList(MembershipProvider.ReadAll(), "Id", "Name", formData.PersonId);
            ViewBag.UnitId = new SelectList(WorkoutEventManager.GetUnitOfMeasurements(), "Id", "Name", formData.UnitId);
            return View(formData);
        }

        // GET: RunningEvents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var workoutEvent = WorkoutEventManager.Read((int) id);
            if (workoutEvent == null)
            {
                return HttpNotFound();
            }

            ViewBag.ActivityId = new SelectList(WorkoutEventManager.GetActivityTypes(), "Id", "Name", workoutEvent.Type.Id);
            ViewBag.PersonId = new SelectList(MembershipProvider.ReadAll(), "Id", "Name", workoutEvent.User.Id);
            ViewBag.UnitId = new SelectList(WorkoutEventManager.GetUnitOfMeasurements(), "Id", "Name", workoutEvent.Unit.Id);
            return View(workoutEvent);
        }

        // POST: RunningEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PersonId,ActivityId,Date,Duration,Distance,UnitId,Title,Notes")] WorkoutEventViewModel formData)
        {
            if (ModelState.IsValid)
            {
                var workoutEvent = new WorkoutEvent()
                {
                    Id = formData.Id,
                    User = new User(formData.PersonId, ""),
                    Type = new ActivityType(formData.ActivityId, ""),
                    Date = formData.Date,
                    Duration = formData.Duration,
                    Distance = formData.Distance,
                    Unit = new UnitOfMeasurement(formData.UnitId, ""),
                    Title = formData.Title,
                    Notes = formData.Notes
                };
                WorkoutEventManager.Update(workoutEvent);
                return RedirectToAction("Index");
            }

            ViewBag.ActivityId = new SelectList(WorkoutEventManager.GetActivityTypes(), "Id", "Name", formData.ActivityId);
            ViewBag.PersonId = new SelectList(MembershipProvider.ReadAll(), "Id", "Name", formData.PersonId);
            ViewBag.UnitId = new SelectList(WorkoutEventManager.GetUnitOfMeasurements(), "Id", "Name", formData.UnitId);
            return View(formData);
        }

        // GET: RunningEvents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var workoutEvent = WorkoutEventManager.Read((int)id);
            if (workoutEvent == null)
            {
                return HttpNotFound();
            }
            return View(workoutEvent);
        }

        // POST: RunningEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WorkoutEventManager.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]  
        public ActionResult Import()
        {
            //Populate the person dropdown
            ViewBag.PersonId = new SelectList(MembershipProvider.ReadAll(), "Id", "Name");

            return View();  
        }  

        [HttpPost]  
        public ActionResult Import(int personId, HttpPostedFileBase file)
        {
            ViewBag.PersonId = new SelectList(MembershipProvider.ReadAll(), "Id", "Name", personId);

            //TODO: Pull this out into a business class
            try
            {  
                if (file.ContentLength > 0)  
                {
                    var data = ParseCsv(file);

                    var importer = new GarminImporter();
                    importer.Import(data, personId);

                    ViewBag.Message = "File Uploaded Successfully!!";
                }
                else
                {
                    ViewBag.Message = "The selected file was empty or not found!";   
                }
                return View();  
            }  
            catch (Exception ex)
            {  
                ViewBag.Message = "File upload failed! " + ex.Message;  
                return View();  
            }  
        }

        private IEnumerable<IEnumerable<string>> ParseCsv(HttpPostedFileBase file)
        {
            TextFieldParser parser = new TextFieldParser(file.InputStream) { TextFieldType = FieldType.Delimited };
            parser.SetDelimiters(",");

            var fileData = new List<string[]>();

            while (!parser.EndOfData)
            {
                string[] row = parser.ReadFields();
                fileData.Add(row);
            }
            parser.Close();

            return fileData;
        }
    }
}
