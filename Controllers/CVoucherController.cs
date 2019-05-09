using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace MvcApplication1.Controllers
{
    public class CVoucherController : Controller
    {
        int id;
        private VoucherEntities db = new VoucherEntities();
        
        [HttpPost]
        public ActionResult Create(CVoucher cvoucher)
        {
            if (ModelState.IsValid)
            {
                using (var db = new MvcApplication1.Models.VoucherEntities())
                {
                    setId();
                    cvoucher.VoucherID = this.id;
                    cvoucher.SubmittedBy = RegistrationController.UserName;
                    cvoucher.SubmitDate = DateTime.Now;
                    db.CVouchers.Add(cvoucher);

                    var newVoucher = db.Vouchers.Create();
                    newVoucher.VoucherID = id;
                    newVoucher.VoucherType = "CV";
                    newVoucher.SubmittedBy = RegistrationController.UserName;
                    newVoucher.SubmitDate = DateTime.Now;
                    newVoucher.StatusByDM = "Unseen";
                    newVoucher.StatusByFSO = "Unseen";
                    newVoucher.EmployeeID = RegistrationController.EmpId;
                    db.Vouchers.Add(newVoucher);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateException e) { }
                    catch (UpdateException e) { }
                    catch (SqlException e) { }
                    return RedirectToAction("Index", "Voucher");
                }
            }

            return View(cvoucher);
        }
        
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        public void setId()
        {
            var db = new VoucherEntities();
            int[] data = (from s in db.Vouchers select s.VoucherID).ToArray();
            int c = 0, i, j;
            if (db.Vouchers.Count() == 0)
            {
                id = 1;
            }
            else
            {
                for (int k = 0; k < data.Length; k++)
                {
                    i = data[k];
                    j = i - (c++);
                    if (j > 1)
                    {
                        id = c;
                        break;
                    }
                    else
                    {
                        id = i + 1;
                    }
                }
            }
        }
    }
}