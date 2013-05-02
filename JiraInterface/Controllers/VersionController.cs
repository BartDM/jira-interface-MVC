using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Atlassian.Jira;

namespace JiraInterface.Controllers
{
    public class VersionController : Controller
    {
        private Jira jira;

        public VersionController()
        {
            var credentials = Helpers.Data.GetJiraCredentials();
            jira = new Jira(credentials.Item1, credentials.Item2, credentials.Item3);
        }

        public ActionResult Index()
        {
            var issues = jira.GetIssuesFromFilter("Z_Group_Public_All");
            return View(issues);
        }
    }
}
