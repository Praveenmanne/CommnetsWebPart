<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommentsView.ascx.cs"
    Inherits="TillidSocialNetworking.CommentsView,TillidSocialNetworking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=13c452dc6efc9d74" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="TillidSocialNetworking" %>
<script src="jquery-1.4.1.js" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $('.displike').live('click', function () {
            $(this).prev().show();
            $(this).hide();

            // hiding image
            $(this).next().hide();

            //diplaying dislike content
            $(this).next().next().show();

        });
        $('.dispdislike').live('click', function () {
            $(this).prev().show();
            $(this).hide();

            // hiding image
            $(this).prev().prev().show();

            //diplaying like content
            $(this).prev().prev().prev().hide();
        });
        $('.reply').click(function () {
            $('.CommentInputBox').focus();
        });
    });

</script>
<%
   
    bool viewMoreFlag = (bool)ViewState["HasMore"];
    List<CommentModel> commentModels = (List<CommentModel>)ViewState["Comments"];

    if (commentModels != null)
    {
%>
<% if (viewMoreFlag)
   { %>
<%--<asp:HyperLink ID="hlviewcomments" runat="server">View More Comments</asp:HyperLink>--%>
<asp:Button ID="hlviewcomments" runat="server" Text="View More Comments" onclick="hlviewcomments_Click" 
     />
<% } %>
<% foreach (CommentModel commentModel in commentModels)
   { %>
<div class="CommentBox">
    <div class="image">
        <% if (!string.IsNullOrEmpty(commentModel.ImageUrl))
           {%>
        <a href="<%= commentModel.ProfileURl %>" target="_blank">
            <img src="<%= commentModel.ImageUrl %>" alt='' id="Image" /></a>
        <%
           }
           else
           {
        %><a href="<%= commentModel.ProfileURl %>" target="_blank">
            <img src="/_layouts/TillidSocialNetworking/Avatar.jpg" id="Image" alt='' /></a>
        <%
           }%>
    </div>
    <div class="txtarea">
        <div class="Info">
            <%= commentModel.PostedBy %>
            on
            <abbr id="timestamptooltip" title="<%= commentModel.Postedon %>">
                <%= commentModel.PostedDifference %></abbr>
            <%--  <%= commentModel.Postedon %>--%>
        </div>
        <div class="Comment">
            <%= commentModel.Comment %>
        </div>
        <div class="clear">
        </div>
        <div class="likeImg">
            <img src="/_layouts/TillidSocialNetworking/likeimg.jpg" />
        </div>
        <div class="displike">
            like
        </div>
        <div class="dislike">
            <img src="/_layouts/TillidSocialNetworking/dislikeimg.jpg" /></div>
        <div class="dispdislike">
            dislike</div>
        <div class="reply">
            Reply
        </div>
    </div>
</div>
<div class="clear">
</div>
<%} %>
<% }%>