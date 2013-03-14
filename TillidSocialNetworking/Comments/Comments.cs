using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;

namespace TillidSocialNetworking
{
    [ToolboxItemAttribute(false)]
    public class Comments : WebPart
    {
        [Personalizable(), WebBrowsable, Category("Comments Custom Properties"), WebDisplayName("Page Count")]
        public String RowLimit { get; set; }

      
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string AscxPath = @"~/_CONTROLTEMPLATES/TillidSocialNetworking/Comments/CommentsUserControl.ascx";

        protected override void CreateChildControls()
        {
            var control = (CommentsUserControl)Page.LoadControl(AscxPath);
            control.WebPartUID = ID;
            control.WebPart = this;
            control.PageCount = RowLimit;
            Controls.Add(control);
        }


    }
}
