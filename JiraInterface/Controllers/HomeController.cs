using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Atlassian.Jira;
using JiraInterface.Models;

namespace JiraInterface.Controllers
{
    public class HomeController : Controller
    {
        private Jira jira;
        private IEnumerable<IssuePriority> priorities;
        private IEnumerable<IssueStatus> statusses;

        public HomeController()
        {
            var credentials = Helpers.Data.GetJiraCredentials();
            jira = new Jira(credentials.Item1, credentials.Item2, credentials.Item3);
        }

        //
        // GET: /Home/

        private void CheckStatusses()
        {
            if (statusses == null)
                statusses = jira.GetIssueStatuses();
        }

        private void CheckPriorities()
        {
            if (priorities == null)
                priorities = jira.GetIssuePriorities();
        }

        public ActionResult Index()
        {
            CheckPriorities();
            CheckStatusses();

            var issues = jira.GetIssuesFromFilter("Z_Group_Public");

            ViewBag.Priorities = priorities;
            ViewBag.Statusses = statusses;


            return View(issues);
        }

        public ActionResult Create()
        {
            //Build the priorities manual
            var extendedPriorities = new List<ExtendedPriority>();
            extendedPriorities.Add(new ExtendedPriority()
                {
                    Id = 1,
                    Name = "Blocker",
                    Description = "De ZGMS is onbruikbaar"
                });
            extendedPriorities.Add(new ExtendedPriority()
                {
                    Id = 2,
                    Name = "Critical",
                    Description = "De ZGMS is bruikbaar op zich maar sleutel mogelijkheden lukken niet"
                });
            extendedPriorities.Add(new ExtendedPriority()
                {
                    Id = 3,
                    Name = "Major",
                    Description = "De ZGMS is volledig bruikbaar maar deze fout moet dringend worden opgelost"
                });
            extendedPriorities.Add(new ExtendedPriority()
                {
                    Id = 4,
                    Name = "Minor",
                    Description = "De ZGMS is volledig bruikbaar en deze fout moet in een volgende versie worden opgelost"
                });
            extendedPriorities.Add(new ExtendedPriority()
                {
                    Id = 5,
                    Name = "Trivial",
                    Description = "De ZGMS is volledig bruikbaar en deze fout moet bekeken en besproken worden"
                });
            extendedPriorities.Add(new ExtendedPriority()
                {
                    Id = 7,
                    Name = "New scope",
                    Description =
                        "De ZGMS is volledig bruikbaar zoals beschreven maar er moet functionaliteit aangepast worden"
                });

            ViewBag.Priorities = extendedPriorities;

            var version = jira.GetProjectVersions("ZGRP").FirstOrDefault(v => !v.IsArchived && !v.IsReleased);
            ViewBag.Version = version;

            var chapters = Helpers.Data.GetChapters();
            ViewBag.Chapters = chapters;

            return View();
        }

        //
        // POST: /Home/Create

        [HttpPost]
        public ActionResult Create(CreateIssue issue)
        {
            CheckPriorities();
            CheckStatusses();
            try
            {
                var jiraIssue = jira.CreateIssue("ZGRP");
                jiraIssue.Type = "Bug";

                var priority =
                    priorities.FirstOrDefault(p => p.Id == issue.Priority.ToString(CultureInfo.InvariantCulture));
                jiraIssue.Priority = priority != null ? priority.Name : "Minor";
                jiraIssue.Summary = issue.Title;
                var sb = new StringBuilder();
                sb.AppendFormat("Gemeld door {0} - {1}- {2}. {3}", issue.Reporter, issue.Email,
                                issue.Tel, Environment.NewLine);
                sb.AppendFormat("Document referentie: {0} - {1} {2}", issue.DocumentChapter, issue.DocumentItem,
                                Environment.NewLine);
                sb.Append(issue.Description);
                jiraIssue.Description = sb.ToString();
                var version = jira.GetProjectVersions("ZGRP").FirstOrDefault(v => !v.IsArchived && !v.IsReleased);
                if (version != null)
                    jiraIssue.AffectsVersions.Add(version);
                jiraIssue.SaveChanges();

                Mail(issue.Title);


                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
        }

        private void Mail(string title)
        {
            var emailAddresses = Helpers.Data.GetEmailAddresses();
            var client = new SmtpClient();

            foreach (var emailAddress in emailAddresses)
            {
                var mm = new MailMessage(emailAddress.Item1, emailAddress.Item2);
                mm.Body = string.Format("ZGROUP: Er is een nieuwe onvolkomenheid gemeld: {0}", title);
                mm.BodyEncoding = new UTF8Encoding();
                mm.IsBodyHtml = false;
                mm.Subject = mm.Body;

                client.Send(mm);
            }

            //ADDED SOME LINES TO TEST MERGE
            //ADDED SOME LINES TO TEST MERGE
            //ADDED SOME LINES TO TEST MERGE
            //ADDED SOME LINES TO TEST MERGE
            //ADDED SOME LINES TO TEST MERGE
        }
    }
}
