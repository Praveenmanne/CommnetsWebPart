<%@ Assembly Name="TillidSocialNetworking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=13c452dc6efc9d74" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommentsUserControl.ascx.cs"
    Inherits="TillidSocialNetworking.CommentsUserControl" %>

<%@ Register TagPrefix="uc1" TagName="comments" Src="~/_layouts/TillidSocialNetworking/CommentsView.ascx" %>

<script src="/_layouts/TillidSocialNetworking/jquery-1.4.1.min.js" type="text/javascript"></script>
<script src="/_layouts/TillidSocialNetworking/comments.js" type="text/javascript"></script>
<link href="/_layouts/TillidSocialNetworking/Stylesheet.css" rel="stylesheet" type="text/css" />

<ul class="CommentBoxContainer">
    <li class="CommentsHolder">
        <uc1:comments ID="comments1" runat="server" />
    </li>
    <li class="CommentNewBoxContainer">
        <div class="label">
            Leave Comment</div>
        <div class="CommentNewBox">
            <div class="image">
                <img src="/_layouts/TillidSocialNetworking/Avatar.jpg" alt='' /></div>
            <textarea class="CommentInputBox" rows="-1" cols="-1"></textarea>
            <div class="ButtonContainer">
                <span class="ErrorInfo"></span><span id="flash">
                    <img alt='' src="/_layouts/TillidSocialNetworking/ajax.gif" /></span>
                <input type="submit" class="submitComment" value=" Post Comment " /></div>
        </div>
        <div class="clear">
        </div>
    </li>
    <li>
        <asp:HiddenField runat="server" ID="HdnWebPartID"/>
         
    </li>
</ul>
