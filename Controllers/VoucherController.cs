using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;

namespace MvcApplication1.Controllers
{
    public class VoucherController : Controller
    {
        /*================================+|  VARIABLE DECLARATION  |+====================================================================*/
        private VoucherEntities db = new VoucherEntities();
        public static String VoucherType;
        public static CVoucher cvoucher;
        public static MAVoucher mavoucher;
        public static TAVoucher tavoucher;
        public static PBVoucher pbvoucher;
        public static Voucher voucher_send;
        private String description;
        private double amount;
        private String destination;
        private int leaveDuration;
        private static int voucherid;
        private bool flag = false;
        /*=================================+|    INDEX(EMPLOYEES)   |+====================================================================*/
        public ActionResult Index()
        {
            var res = (from s in db.Vouchers where s.EmployeeID == RegistrationController.EmpId select s).ToArray();
            ViewBag.Vouchers = res;
            return View();
        }
        public ActionResult Approved()
        {
            var res = (from s in db.Vouchers where s.EmployeeID == RegistrationController.EmpId && ((s.StatusByDM == "Approved")|| (s.StatusByFSO == "Approved")) select s).ToArray();
            ViewBag.Vouchers = res;
            return View();
        }
        public ActionResult Rejected()
        {
            var res = (from s in db.Vouchers where s.EmployeeID == RegistrationController.EmpId && ((s.StatusByDM == "Rejected") || (s.StatusByFSO == "Rejected")) select s).ToArray();
            ViewBag.Vouchers = res;
            return View();
        }
        /*================================+|   DETAILS START  |+==========================================================================*/

        public ActionResult Details(int id = 0)
        {
            Voucher voucher = db.Vouchers.Find(id);
            voucher_send = db.Vouchers.Find(id);
            VoucherType = voucher.VoucherType;
            voucherid = id;
            if (voucher == null)
            {
                return HttpNotFound();
            }
            if (VoucherType.Equals("CV"))
            {
                cvoucher = db.CVouchers.Find(id);
                
                return RedirectToAction("CV_Details");
            }
            else if (VoucherType.Equals("TAV"))
            {
                tavoucher = db.TAVouchers.Find(id);
                
                return RedirectToAction("TAV_Details");
            }
            else if (VoucherType.Equals("MAV"))
            {
                mavoucher = db.MAVouchers.Find(id);
                
                return RedirectToAction("MAV_Details");
            }
            else if (VoucherType.Equals("PBV"))
            {
                pbvoucher = db.PBVouchers.Find(id);
                
                return RedirectToAction("PBV_Details");
            }
            else
                return RedirectToAction("Index");
        }
        public ActionResult CV_Details()
        {
            return View();
        }
        public ActionResult MAV_Details()
        {
            return View();
        }
        public ActionResult TAV_Details()
        {
            return View();
        }
        public ActionResult PBV_Details()
        {
            return View();
        }

        /*================================+|    DETAILS END   |+==========================================================================*/


        /*================================+|     EDIT START   |+==========================================================================*/

        public ActionResult Edit(int id = 0)
        {
            Voucher voucher = db.Vouchers.Find(id);
            VoucherType = voucher.VoucherType;
            voucherid = id;
            if (voucher == null)
            {
                return HttpNotFound();
            }
            if (VoucherType.Equals("CV"))
            {
                cvoucher = db.CVouchers.Find(id);
                return RedirectToAction("CV_Edit");
            }
            else if (VoucherType.Equals("TAV"))
            {
                tavoucher = db.TAVouchers.Find(id);
                return RedirectToAction("TAV_Edit");
            }
            else if (VoucherType.Equals("MAV"))
            {
                mavoucher = db.MAVouchers.Find(id);
                return RedirectToAction("MAV_Edit");
            }
            else if (VoucherType.Equals("PBV"))
            {
                pbvoucher = db.PBVouchers.Find(id);
                return RedirectToAction("PBV_Edit");
            }
            else
            return View("Index");
        }

        public ActionResult CV_Edit()
        {
            return View();
        }
        public ActionResult MAV_Edit()
        {
            return View();
        }
        public ActionResult TAV_Edit()
        {
            return View();
        }
        public ActionResult PBV_Edit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Edit(Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                if (VoucherType.Equals("CV"))
                {
                    amount = double.Parse(Request.Form[1]);
                    description = Request.Form[2];
                    CV_Update();
                }
                else if (VoucherType.Equals("TAV"))
                {
                    leaveDuration = int.Parse(Request.Form[1]);
                    destination = Request.Form[2];
                    TAV_Update();
                }
                else if (VoucherType.Equals("MAV"))
                {
                    leaveDuration = int.Parse(Request.Form[1]);
                    description = Request.Form[2];
                    MAV_Update();
                }
                else if (VoucherType.Equals("PBV"))
                {
                    amount = double.Parse(Request.Form[1]);
                    description = Request.Form[2];
                    PBV_Update();
                }
            }
            if (flag)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        /*================================+|     EDIT END    |+===========================================================================*/

        /*================================+|   DELETE START  |+===========================================================================*/
        public ActionResult Delete(int id = 0)
        {
            Voucher voucher = db.Vouchers.Find(id);
            VoucherType = voucher.VoucherType;
            voucherid = id;
            flag = false;
            if (voucher == null)
            {
                return HttpNotFound();
            }
            if (VoucherType.Equals("CV"))
            {
                cvoucher = db.CVouchers.Find(id);
                db.CVouchers.Remove(cvoucher);
                flag = true;
            }
            else if (VoucherType.Equals("TAV"))
            {
                tavoucher = db.TAVouchers.Find(id);
                db.TAVouchers.Remove(tavoucher);
                flag = true;
            }
            else if (VoucherType.Equals("MAV"))
            {
                mavoucher = db.MAVouchers.Find(id);
                db.MAVouchers.Remove(mavoucher);
                flag = true;
            }
            else if (VoucherType.Equals("PBV"))
            {
                pbvoucher = db.PBVouchers.Find(id);
                db.PBVouchers.Remove(pbvoucher);
                flag = true;
            }
            if(flag)
                db.Vouchers.Remove(voucher);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /*================================+|    DELETE END   |+===========================================================================*/
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        [HttpPost]
        public ActionResult SelectVoucher(String VoucherType)
        {
            if (VoucherType.Equals("Cash Voucher"))
                return PartialView("_CV");
            else if (VoucherType.Equals("Travel Allowance Voucher"))
                return PartialView("_TAV");
            else if (VoucherType.Equals("Medical Allowance Voucher"))
                return PartialView("_MAV");
            else if (VoucherType.Equals("Phone Bill Voucher"))
                return PartialView("_PBV");
            else return View("Create","Voucher");
        }
        public void CV_Update()
        {
            using (var db = new MvcApplication1.Models.VoucherEntities())
            {
                var s = db.CVouchers.Where(u => u.VoucherID == voucherid).SingleOrDefault();
                var t = db.Vouchers.Where(u => u.VoucherID == voucherid).SingleOrDefault();
                s.Amount = (decimal)this.amount;
                s.Description = this.description;
                s.SubmitDate = DateTime.Now;
                t.SubmitDate = DateTime.Now;
                db.Entry(s).State = EntityState.Modified;
                db.SaveChanges();
                flag = true;
            }
        }
        public void MAV_Update()
        {
            using (var db = new MvcApplication1.Models.VoucherEntities())
            {
                var s = db.MAVouchers.Where(u => u.VoucherID == voucherid).SingleOrDefault();
                var t = db.Vouchers.Where(u => u.VoucherID == voucherid).SingleOrDefault();
                s.LeaveDuration = (int)this.leaveDuration;
                s.Description = this.description;
                s.SubmitDate = DateTime.Now;
                t.SubmitDate = DateTime.Now;
                db.Entry(s).State = EntityState.Modified;
                db.SaveChanges();
                flag = true;
            }
        }
        public void TAV_Update()
        {
            using (var db = new MvcApplication1.Models.VoucherEntities())
            {
                var s = db.TAVouchers.Where(u => u.VoucherID == voucherid).SingleOrDefault();
                var t = db.Vouchers.Where(u => u.VoucherID == voucherid).SingleOrDefault();
                s.LeaveDuration = (int)this.leaveDuration;
                s.Destination = this.destination;
                s.SubmitDate = DateTime.Now;
                t.SubmitDate = DateTime.Now;
                db.Entry(s).State = EntityState.Modified;
                db.SaveChanges();
                flag = true;
            }
        }
        public void PBV_Update()
        {
            using (var db = new MvcApplication1.Models.VoucherEntities())
            {
                var s = db.PBVouchers.Where(u => u.VoucherID == voucherid).SingleOrDefault();
                var t = db.Vouchers.Where(u => u.VoucherID == voucherid).SingleOrDefault();
                s.Amount = (decimal)this.amount;
                s.Description = this.description;
                s.SubmitDate = DateTime.Now;
                t.SubmitDate = DateTime.Now;
                db.Entry(s).State = EntityState.Modified;
                db.SaveChanges();
                flag = true;
            }
        }

        /*+++++++++++++++++++++++++++++++++++++|  Department Manager |++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/

        public ActionResult DM_Index()
        {
            var res = (from s in db.Vouchers select s).ToArray();
            ViewBag.Vouchers = res;
            return View();
        }
        public ActionResult DM_Approved()
        {
            var res = (from s in db.Vouchers where s.StatusByDM == "Approved" select s).ToArray();
            ViewBag.Vouchers = res;
            return View();
        }
        public ActionResult DM_Rejected()
        {
            var res = (from s in db.Vouchers where s.StatusByDM == "Rejected" select s).ToArray();
            ViewBag.Vouchers = res;
            return View();
        }
        public ActionResult DM_Unseen()
        {
            var res = (from s in db.Vouchers where s.StatusByDM == "Unseen" select s).ToArray();
            ViewBag.Vouchers = res;
            return View();
        }
        public ActionResult DM_Details(int id = 0)
        {
            Voucher voucher = db.Vouchers.Find(id);
            voucher_send = db.Vouchers.Find(id);
            VoucherType = voucher.VoucherType;
            voucherid = id;
            if (voucher == null)
            {
                return HttpNotFound();
            }
            if (VoucherType.Equals("CV"))
            {
                cvoucher = db.CVouchers.Find(id);
                return RedirectToAction("DM_CV_Details");
            }
            else if (VoucherType.Equals("TAV"))
            {
                tavoucher = db.TAVouchers.Find(id);
                return RedirectToAction("DM_TAV_Details");
            }
            else if (VoucherType.Equals("MAV"))
            {
                mavoucher = db.MAVouchers.Find(id);
                return RedirectToAction("DM_MAV_Details");
            }
            else if (VoucherType.Equals("PBV"))
            {
                pbvoucher = db.PBVouchers.Find(id);
                return RedirectToAction("DM_PBV_Details");
            }
            else
                return RedirectToAction("DM_Index");
        }
        public ActionResult DM_CV_Details()
        {
            return View();
        }
        public ActionResult DM_MAV_Details()
        {
            return View();
        }
        public ActionResult DM_TAV_Details()
        {
            return View();
        }
        public ActionResult DM_PBV_Details()
        {
            return View();
        }
        public ActionResult DM_Edit()
        {
            var DM_Status = Request.Form[8];
            if (DM_Status == "Approve")
            {
                DM_Status += "d";
            }
            else if (DM_Status == "Reject")
            {
                DM_Status += "ed";
            }
            DM_Update(DM_Status);
            return RedirectToAction("DM_Index");
        }
        public void DM_Update(String DM_Status)
        {
            using (var db = new MvcApplication1.Models.VoucherEntities())
            {
                var t = db.Vouchers.Where(u => u.VoucherID == voucherid).SingleOrDefault();
                t.StatusByDM = DM_Status;
                db.Entry(t).State = EntityState.Modified;
                db.SaveChanges();
                flag = true;
            }
        }

        /*+++++++++++++++++++++++++++++++++++|  Financial Services Officer |++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/

        public ActionResult FSO_Index()
        {
            var res = (from s in db.Vouchers select s).ToArray();
            ViewBag.Vouchers = res;
            return View();
        }
        public ActionResult FSO_Approved()
        {
            var res = (from s in db.Vouchers where s.StatusByFSO == "Approved" select s).ToArray();
            ViewBag.Vouchers = res;
            return View();
        }
        public ActionResult FSO_Rejected()
        {
            var res = (from s in db.Vouchers where s.StatusByFSO == "Rejected" select s).ToArray();
            ViewBag.Vouchers = res;
            return View();
        }
        public ActionResult FSO_Unseen()
        {
            var res = (from s in db.Vouchers where s.StatusByFSO == "Unseen" select s).ToArray();
            ViewBag.Vouchers = res;
            return View();
        }
        public ActionResult FSO_Details(int id = 0)
        {
            Voucher voucher = db.Vouchers.Find(id);
            voucher_send = db.Vouchers.Find(id);
            VoucherType = voucher.VoucherType;
            voucherid = id;
            if (voucher == null)
            {
                return HttpNotFound();
            }
            if (VoucherType.Equals("CV"))
            {
                cvoucher = db.CVouchers.Find(id);
                return RedirectToAction("FSO_CV_Details");
            }
            else if (VoucherType.Equals("TAV"))
            {
                tavoucher = db.TAVouchers.Find(id);
                return RedirectToAction("FSO_TAV_Details");
            }
            else if (VoucherType.Equals("MAV"))
            {
                mavoucher = db.MAVouchers.Find(id);
                return RedirectToAction("FSO_MAV_Details");
            }
            else if (VoucherType.Equals("PBV"))
            {
                pbvoucher = db.PBVouchers.Find(id);
                return RedirectToAction("FSO_PBV_Details");
            }
            else
                return RedirectToAction("FSO_Index");
        }
        public ActionResult FSO_CV_Details()
        {
            return View();
        }
        public ActionResult FSO_MAV_Details()
        {
            return View();
        }
        public ActionResult FSO_TAV_Details()
        {
            return View();
        }
        public ActionResult FSO_PBV_Details()
        {
            return View();
        }
        public ActionResult FSO_Edit()
        {
            var FSO_Status = Request.Form[8];
            if (FSO_Status == "Approve")
            {
                FSO_Status += "d";
            }
            else if (FSO_Status == "Reject")
            {
                FSO_Status += "ed";
            }
            FSO_Update(FSO_Status);
            return RedirectToAction("FSO_Index");
        }
        public void FSO_Update(String FSO_Status)
        {
            using (var db = new MvcApplication1.Models.VoucherEntities())
            {
                var t = db.Vouchers.Where(u => u.VoucherID == voucherid).SingleOrDefault();
                t.StatusByFSO = FSO_Status;
                db.Entry(t).State = EntityState.Modified;
                db.SaveChanges();
                flag = true;
            }
        }
    }
}