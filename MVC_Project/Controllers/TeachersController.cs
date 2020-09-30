using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_Project.Models;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Threading.Tasks;

namespace MVC_Project.Controllers
{
    public class TeachersController : Controller
    {
        private TeacherContext db = new TeacherContext();

        // GET: Teachers
        public ActionResult Index()
        {
            return View(db.tblTeachers.ToList());
        }


        public ActionResult Report(int id)
        {
            


            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report/"), "CrystalReportTeacher.rpt"));

            rd.SetDataSource(db.tblTeachers.Where(t=>t.id==id));

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "Teacher.pdf");
        }


        public ActionResult AllReport()
        {



            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report/"), "CrystalReportTeacher.rpt"));

            rd.SetDataSource(db.tblTeachers.ToList());

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "Teacher.pdf");
        }










        // GET: Teachers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTeacher tblTeacher = db.tblTeachers.Find(id);
            if (tblTeacher == null)
            {
                return HttpNotFound();
            }
            return View(tblTeacher);
        }

        // GET: Teachers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblTeacher tblTeacher,HttpPostedFileBase FileUpload)
        {
            if (ModelState.IsValid)
            {
                //Save image in folder

                string FileName = Path.GetFileName(FileUpload.FileName);
                string SaveLocation = Server.MapPath("~/Upload/"+FileName);
                FileUpload.SaveAs(SaveLocation);

                //save image name in database

                tblTeacher.teacherPicName= "~/Upload/" + FileName;


                // byte image Save


                tblTeacher.teacherPicture=new byte [FileUpload.ContentLength];
              ViewBag.TeacherPicture = FileUpload.InputStream.Read(tblTeacher.teacherPicture,0, FileUpload.ContentLength);

                db.tblTeachers.Add(tblTeacher);
                db.SaveChanges();


                return RedirectToAction("Index");
            }

            

            return View(tblTeacher);
        }

        // GET: Teachers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTeacher tblTeacher = db.tblTeachers.Find(id);
            if (tblTeacher == null)
            {
                return HttpNotFound();
            }
            return View(tblTeacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,teacherName,teacherEmail,teacherPicture,teacherPicName")] tblTeacher tblTeacher,HttpPostedFileBase FileUploadEdit)
        {
            if (ModelState.IsValid)
            {

                //Save image in folder

                string FileName = Path.GetFileName(FileUploadEdit.FileName);
                string SaveLocation = Server.MapPath("~/Upload/" + FileName);
                FileUploadEdit.SaveAs(SaveLocation);

                //save image name in database

                tblTeacher.teacherPicName = "~/Upload/" + FileName;


                // byte image Save


                tblTeacher.teacherPicture = new byte[FileUploadEdit.ContentLength];
                ViewBag.TeacherPicture = FileUploadEdit.InputStream.Read(tblTeacher.teacherPicture, 0, FileUploadEdit.ContentLength);


                db.Entry(tblTeacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblTeacher);
        }

        // GET: Teachers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTeacher tblTeacher = db.tblTeachers.Find(id);
            if (tblTeacher == null)
            {
                return HttpNotFound();
            }
            return View(tblTeacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblTeacher tblTeacher = db.tblTeachers.Find(id);
            db.tblTeachers.Remove(tblTeacher);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
