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
            //Starts the assembly process.
            return Json(Poll.InitAssemble(PLP_ID, UserID));
        }

        public ActionResult DoPoll(string PLP_ID)
        {
            //Polls the db, and returns the progress of the process

            Poll p = new Poll(PLP_ID);
            return Json(p);
        }

        public ActionResult DoKillAssembly(string PLP_ID)
        {
            //We added this in case the long running process gets stopped / error's out / etc
            //We show this button only to certain "admins", so just anyone can't kill a process
            //started by someone else.

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
            //This is where you would do some long running task in steps preferably.
            //Just make sure after each "step" or piece of the process, use the UpdatePercentComplete
            //Method to update the user on the progress.
            //At the end of the process, we call MarkAssemblyComplete() to stop the polling.
            //This would likely be a separate view / web API / process / etc

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