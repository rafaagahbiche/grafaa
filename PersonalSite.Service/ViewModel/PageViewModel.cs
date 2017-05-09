
namespace PersonalSite.Service.ViewModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.Web.Mvc;
    
    public class PageViewModel
    {
        public int PageId { get; set; }

        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string PageContent { get; set; }

        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string DecodedPageContent
        {
            get { return HttpUtility.HtmlDecode(PageContent); }
            set { PageContent = HttpUtility.HtmlEncode(value); }
        }
        public ArticleViewModel Article { get; set; }
        public int ParentArticleId { get; set; }
    }
}
