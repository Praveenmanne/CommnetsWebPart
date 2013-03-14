using System;
using System.Collections.Generic;
using System.Web.UI;
using TillidSocialNetworking.Layouts.TillidSocialNetworking;

namespace TillidSocialNetworking
{

    public partial class CommentsView : UserControl
    {
        readonly CommentModelContainer _commentModelContainer = new CommentModelContainer();
        protected void Page_Load(object sender, EventArgs e)
        {
            //if(!IsPostBack)
            //{
            //    SetCommentsModel(_commentModelContainer);
            //    Session["CommentCount"] = _commentModelContainer.CommentModels.Count;
            //}
        }
        public void SetCommentsModel(CommentModelContainer commentModels)
        {

            ViewState["Comments"] = commentModels.CommentModels;
            ViewState["HasMore"] = commentModels.HasMoreRecords;

        }

        protected void hlviewcomments_Click(object sender, EventArgs e)
        {
            //if (Session["CommentCount"] == null)
            //{
            //    var list =(List<CommentModel>) ViewState["Comments"];

            //    Session["CommentCount"] = list.Count;
            //}
            //else
            //{
            //    var list = (List<CommentModel>)ViewState["Comments"];
            //    Session["CommentCount"] = (list.Count) + Convert.ToInt32(Session["CommentCount"]);
            //}
        }


    }
}
