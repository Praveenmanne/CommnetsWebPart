using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace TillidSocialNetworking
{
    [ToolboxItem(false)]
    public partial class CommentsUserControl : UserControl
    {
        public Comments WebPart { get; set; }

        public string WebPartUID;

        public string PageCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            HdnWebPartID.Value = WebPartUID;
            var commentsView = (CommentsView)comments1;
            if (Session["CommentCount"] == null)
            {
                Session["CommentCount"] = PageCount;
                commentsView.SetCommentsModel(new CommonUtillites().Get(WebPartUID, int.Parse(PageCount)));
            }
            else
            {
                var count = Convert.ToInt32(Session["CommentCount"].ToString());
                count = count + Convert.ToInt32(PageCount);
                commentsView.SetCommentsModel(new CommonUtillites().Get(WebPartUID, count));
            }

        }

    }
}
