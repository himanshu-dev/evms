using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcApplication1.Models;

namespace MvcApplication1.Controllers
{
    public class RegistrationController : Controller
    {
        int id;
        public static int EmpId;
        public static String UserName = "";
		public static bool LoginStatus;
        RegistrationEntities db = new RegistrationEntities();
        
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.Employee employee)
        {
            if (IsValid(employee.Email, employee.Password))
            {
				LoginStatus = true;
                FormsAuthentication.SetAuthCookie(employee.Email, false);
                String usertype = CheckUserType(employee);
                UserName = GetUserName(employee);
                EmpId = getEmpID(employee);
                if (usertype.Equals("Employee"))
                {
                    return RedirectToAction("Index", "Voucher");
                }
                else if (usertype.Equals("Dipartment Manager"))
                {
                    return RedirectToAction("DM_Index", "Voucher");
                }
                else if (usertype.Equals("Financial Services Officer"))
                {
                    return RedirectToAction("FSO_Index", "Voucher");
                }
                return RedirectToAction("Login", "Registration");
            }
            else
            {
                ModelState.AddModelError("", "Login details are wrong.");
            }
            return View(employee);
        }

        [HttpGet]
        public ActionResult Register()
        {
            ViewBag.UserType = new SelectList(new[] { "Employee", "Dipartment Manager", "Financial Services Officer" });
            return View();
        }

        [HttpPost]
        public ActionResult Register(Models.Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var db = new MvcApplication1.Models.RegistrationEntities())
                    {
                        setId();
                        var crypto = new SimpleCrypto.PBKDF2();
                        var encryPass = crypto.Compute(employee.Password);
                        var newUser = db.Employees.Create();
                        newUser.EmployeeID = this.id;
                        newUser.FirstName = employee.FirstName;
                        newUser.LastName = employee.LastName;
                        newUser.Email = employee.Email;
                        newUser.UserType = employee.UserType;
                        newUser.Password = encryPass;
                        newUser.PasswordSalt = crypto.Salt;
                        newUser.CreatedDate = DateTime.Now;
                        newUser.IPAddress = "Not Available";
                        db.Employees.Add(newUser);
                        db.SaveChanges();
                        return RedirectToAction("Login", "Registration");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Data is not correct");
                }
            }
            catch (DbUpdateException e)
            {
                
            }
            catch (DbEntityValidationException e)
            {
                
            }
            return View(employee);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
			LoginStatus = false;
			UserName = "";
            return RedirectToAction("Login", "Registration");
        }

        private bool IsValid(string email, string password)
        {
            var crypto = new SimpleCrypto.PBKDF2();
            bool IsValid = false;
            using (var db = new MvcApplication1.Models.RegistrationEntities())
            {
                var user = db.Employees.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    if (user.Password == crypto.Compute(password, user.PasswordSalt))
                    {
                        IsValid = true;
                    }
                }
            }
            return IsValid;
        }

        public void setId()
        {
            var db = new RegistrationEntities();
            int[] data = (from s in db.Employees select s.EmployeeID).ToArray();
            int c = 0, i, j;
            if (db.Employees.Count() == 0)
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

        public String CheckUserType(Employee emp)
        {
            String [] types = (from s in db.Employees where s.Email == emp.Email select s.UserType).ToArray();
            return types[0];
        }
        public String GetUserName(Employee emp)
        {
            String name = "";
            var data = (from s in db.Employees where s.Email == emp.Email select s);
            foreach (var i in data)
            {
                name += i.FirstName + " " + i.LastName;
            }
            return name;
        }
        public int getEmpID(Employee emp)
        {
            int[] id = (from s in db.Employees where s.Email == emp.Email select s.EmployeeID).ToArray();
            return id[0];
        }
    }
}
