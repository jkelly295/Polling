using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PollingMVC_CS.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DoInitAssemble(string PLP_ID, string UserID)
        {
            return Json(Poll.InitAssemble(PLP_ID, UserID));
        }

        public ActionResult DoPoll(string PLP_ID)
        {
            Poll p = new Poll(PLP_ID);
            return Json(p);
        }

        public ActionResult DoKillAssembly(string PLP_ID)
        {
            string retval = "";
            try
            {
                retval = Poll.KillAssembly(PLP_ID);
            }
            catch (Exception ex)
            {
                return Json(ex.StackTrace.ToString());
            }

            return Json(retval);
        }


        public ActionResult DoLongRunningStuff(string PLP_ID)
        {
            string RetVal = "";

            for(int i = 0; i < 10; i++)
            {
                int PC = i * 10;
                Poll.UpdatePercentComplete(PLP_ID, PC.ToString(), "Step: " + i);
                System.Threading.Thread.Sleep(5000);


            }
            Poll.MarkAssemblyComplete(PLP_ID);
            RetVal = "Task is Complete, Bruh.";



            return Json(RetVal);
        }



    }
}