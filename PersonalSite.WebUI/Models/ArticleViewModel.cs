using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalSite.WebUI.Models
{
    public class ArticleViewModel
    {
        public int Id { get; set; }
        public List<int> ArticlePageIds { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}