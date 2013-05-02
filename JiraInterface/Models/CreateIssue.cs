using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Atlassian.Jira;
using DataAnnotationsExtensions;

namespace JiraInterface.Models
{
    public class CreateIssue
    {
        [Required(ErrorMessage = "Geef je naam in.")]
        public string Reporter { get; set; }
        public string Tel { get; set; }
        [Email(ErrorMessage = "Geef een geldig email adres in.")]
        public string Email { get; set; }
        public DateTime DateTime { get; set; }
        public string Fase { get; set; }
        [Required(ErrorMessage = "Geef een titel in.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Geef een omschrijving in.")]
        public string Description { get; set; }
        public int Priority { get; set; }
        public string DocumentChapter { get; set; }
        public string DocumentItem { get; set; }
    }
}