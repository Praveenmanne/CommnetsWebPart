using System;
using System.Collections.Generic;
using Microsoft.SharePoint;

namespace TillidSocialNetworking
{
    public partial class PostComment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string comment = Request.Form["comment"];
            string webPartId = Request.Form["webpartID"];
            var currentuser = SPContext.Current.Web.CurrentUser.LoginName;
            var listModels = new CommonUtillites();
            var control = (CommentsView)Page.LoadControl("/_layouts/TillidSocialNetworking/CommentsView.ascx");
            control.SetCommentsModel(listModels.Add(webPartId, comment, currentuser));
            form1.Controls.Add(control);
           
        }
    }
}