using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using TillidSocialNetworking.Layouts.TillidSocialNetworking;


namespace TillidSocialNetworking
{
    public class CommonUtillites
    {
        public void Createlist(string firstlist, string secondlist)
        {
            try
            {
                string url = SPContext.Current.Site.Url;
                using (var site = new SPSite(url))
                {
                    using (var web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;

                        SPList commentWebPartReferencelist = web.Lists.TryGetList(firstlist);
                        if (commentWebPartReferencelist == null)
                        {
                            var listUid = web.Lists.Add(firstlist, string.Empty, SPListTemplateType.GenericList);
                            commentWebPartReferencelist = web.Lists[listUid];
                            commentWebPartReferencelist.OnQuickLaunch = true;
                            commentWebPartReferencelist.Update();
                        }

                        if (!commentWebPartReferencelist.Fields.ContainsField(Utilities.WebPartId))
                        {
                            var field = commentWebPartReferencelist.Fields[SPBuiltInFieldId.Title];
                            field.Title = Utilities.WebPartId;
                            field.Update();
                        }

                        SPList commentslist = web.Lists.TryGetList(secondlist);
                        if (commentslist == null)
                        {
                            var listUid = web.Lists.Add(secondlist, string.Empty, SPListTemplateType.GenericList);
                            commentslist = web.Lists[listUid];
                            commentslist.OnQuickLaunch = true;
                            commentslist.Update();
                        }

                        if (!commentslist.Fields.ContainsField(Utilities.Index))
                        {
                            var field = commentslist.Fields[SPBuiltInFieldId.Title];
                            field.Title = Utilities.Index;

                            field.Update();
                        }

                        if (!commentslist.Fields.ContainsField("Comment"))
                        {
                            commentslist.Fields.Add("Comment", SPFieldType.Note, false);
                            SPField textField = commentslist.Fields["Comment"];
                            textField.StaticName = "Comment";
                            textField.Update();
                            AddFieldOnView(commentslist, commentslist.Fields["Comment"]);
                        }

                        if (!commentslist.Fields.ContainsField("Date"))
                        {
                            string textField = commentslist.Fields.Add("Date", SPFieldType.DateTime, false);
                            var fieldDateTime = new SPFieldDateTime(commentslist.Fields, textField)
                            {
                                DisplayFormat = SPDateTimeFieldFormatType.DateOnly,
                                DefaultValue = "[today]"
                            };
                            fieldDateTime.Update();
                            AddFieldOnView(commentslist, commentslist.Fields["Date"]);
                        }

                        if (!commentslist.Fields.ContainsField("Posted By"))
                        {
                            commentslist.Fields.Add("Posted By", SPFieldType.User, false);
                            var empname = (SPFieldUser)commentslist.Fields["Posted By"];
                            if (empname != null)
                            {
                                empname.AllowMultipleValues = false;
                                empname.Presence = true;
                                empname.SelectionMode = SPFieldUserSelectionMode.PeopleOnly;
                                empname.StaticName = "Posted By";
                                empname.Update();
                            }
                            AddFieldOnView(commentslist, commentslist.Fields["Posted By"]);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }
        public CommentModelContainer Add(string webpartid, string comment, string currentuser)
        {
            bool hasMoreComments = false;
            int itemcount = 0;
            int currentitemcount = 0;

            string Url = string.Empty;
            var commentModels = new List<CommentModel>();
            using (var site = new SPSite(SPContext.Current.Site.Url))
            {
                using (var web = site.OpenWeb())
                {
                    web.AllowUnsafeUpdates = true;
                    var commnetweblist = web.Lists.TryGetList(Utilities.CommentWebPartReference);
                    if (commnetweblist == null)
                    {
                        Createlist(Utilities.CommentWebPartReference, Utilities.Comments);
                        commnetweblist = web.Lists.TryGetList(Utilities.CommentWebPartReference);
                    }
                    var id = GetListItemCollection(commnetweblist, Utilities.WebPartId, webpartid);
                    var newItem = commnetweblist.Items.Add();
                    if (id.Count == 0)
                    {
                        newItem["WebPart ID"] = webpartid;
                        newItem.Update();
                    }
                    var filter = GetListItemCollection(web.Lists.TryGetList(Utilities.CommentWebPartReference), Utilities.WebPartId, webpartid);
                    foreach (SPListItem collection in filter)
                    {
                        var list = web.Lists.TryGetList(Utilities.Comments);
                        var items = list.Items.Add();
                        items["Index"] = collection["ID"].ToString();
                        items["Comment"] = comment;
                        items["Date"] = DateTime.Now;
                        items["Posted By"] = web.CurrentUser;
                        items.Update();

                        SPServiceContext serverContext = SPServiceContext.GetContext(web.Site);

                        var profileManager = new UserProfileManager(serverContext);

                        if (profileManager.UserExists(currentuser))
                        {
                            UserProfile profile = profileManager.GetUserProfile(currentuser);
                            if (profile != null)
                            {
                                Url = profile["PictureURL"].Value.ToString();
                            }
                        }



                        commentModels.Add(new CommentModel
                                              {
                                                  ImageUrl = Url,
                                                  Comment = comment,
                                                  PostedBy = currentuser,
                                                  PostedOn = DateTime.Now
                                              });
                    }
                    web.AllowUnsafeUpdates = false;

                }

            }
            return new CommentModelContainer() { CommentModels = commentModels };

        }
        public CommentModelContainer Get(string wepartid, int pageCount)
        {
            bool hasMoreComments = false;
            int itemcount = 0;
            const int currentitemcount = 0;
            int itemlimit = pageCount;
            string url = string.Empty;

            var commentModels = new List<CommentModel>();
            try
            {
                using (var site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (var web = site.OpenWeb())
                    {
                        var getwebpartid = GetListItemCollection(web.Lists.TryGetList(Utilities.CommentWebPartReference), Utilities.WebPartId, wepartid);

                        foreach (SPListItem item in getwebpartid)
                        {
                            var index = item["ID"].ToString();
                            SPListItemCollection getcomments = GetListItemCollection(web.Lists.TryGetList(Utilities.Comments), "Index", index);

                            itemcount = getcomments.Count;

                            commentModels.AddRange( (( from SPListItem comments in getcomments
                                                   let comment = comments[Utilities.Comment].ToString()
                                                   let date = Convert.ToDateTime(comments[Utilities.Date].ToString())
                                                   let postedby = new SPFieldUserValue(web, comments[Utilities.Postedby].ToString()).User.Name
                                                   let id = comments["ID"].ToString()

                                                   select new CommentModel
                                                              {
                                                                  Comment = comment,
                                                                  PostedBy = "<a href='" + GetPersonalSiteUrl(site, postedby) + "'>" + postedby + "</a>",
                                                                  ImageUrl = GetProfile(postedby),
                                                                  PostedDifference = GetDiffferenceString(DateTime.Now, date),
                                                                  Postedon = String.Format("{0:ddd, MMM d, yyyy}", date),
                                                                  ProfileURl = GetPersonalProfileUrl(site, postedby),
                                                                  ID=id
                                                              }).OrderByDescending(model => model.ID)).Take(pageCount).Reverse());
                        }



                    }
                }

                int balanceitemcount = itemcount - currentitemcount;
                int div = balanceitemcount / itemlimit;
                if (div == 0)
                {
                    hasMoreComments = false;
                }
                else
                {
                    if (balanceitemcount == itemlimit)
                    {
                        hasMoreComments = false;
                    }
                    else
                    {
                        hasMoreComments = true;

                    }
                }
            }
            catch (Exception ex)
            {
                Error(ex.Message);

            }
            return new CommentModelContainer() { CommentModels = commentModels, HasMoreRecords = hasMoreComments };


        }
        public string GetDiffferenceString(DateTime fromdate, DateTime todate)
        {
            TimeSpan span = (fromdate - todate);

            string returnValue = string.Empty;

            if (span.Days == 0 && span.Hours == 0 && span.Minutes == 0)
            {
                returnValue = span.Seconds + " seconds";
            }
            else if (span.Days == 0 && span.Hours == 0)
            {
                returnValue = span.Minutes + " minutes";
            }
            else if (span.Days == 0)
            {
                returnValue = span.Hours + " hours";
            }
            else
            {
                returnValue = span.Days + " days";
            }
            returnValue = returnValue + " ago";
            return returnValue;
        }
        private void AddFieldOnView(SPList list, SPField spField)
        {
            try
            {
                list.DefaultView.ViewFields.Add(spField);
                list.DefaultView.Update();
                for (int i = 0; i < list.Views.Count; i++)
                {
                    SPView view = list.Views[i];
                    view.ViewFields.Add(spField);
                    view.Update();
                }
                list.Update();

            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory("Comments", TraceSeverity.Monitorable, EventSeverity.Error), TraceSeverity.Monitorable, ex.Message, new object[] { ex.StackTrace });
            }
        }
        internal static SPListItemCollection GetListItemCollection(SPList spList, string key, string value)
        {
            // Return list item collection based on the lookup field  
            SPField spField = spList.Fields[key];
            var query = new SPQuery
            {
                Query = @"<Where>  
                                    <Eq>              
                                        <FieldRef Name='" + spField.InternalName + @"'/><Value Type='" + spField.Type.ToString() + @"'>" + value + @"</Value> 
                                    </Eq>      
                                    </Where>"
            };
            return spList.GetItems(query);
        }
        public static SPListItemCollection GetListItems(SPWeb web, uint rowlimit, int pageNo)
        {
            SPList oList = web.Lists["Comments"];
            int totalListItems = oList.ItemCount;
            var query = new SPQuery
            {
                RowLimit = rowlimit,
                Query = "<OrderBy Override=\"TRUE\"><FieldRef Name=\"FileLeafRef\" /></OrderBy>"
            };

            int index = 1;

            SPListItemCollection items;
            do
            {
                items = oList.GetItems(query);
                if (index == pageNo)
                    break;
                query.ListItemCollectionPosition = items.ListItemCollectionPosition;
                index++;

            }
            while (query.ListItemCollectionPosition != null);
            return items;


        }
        private void Error(string error)
        {

        }
        public string GetProfile(string postedby)
        {
            string Url = string.Empty;

            SPServiceContext serverContext = SPServiceContext.GetContext(SPContext.Current.Web.Site);

            var profileManager = new UserProfileManager(serverContext);

            if (profileManager.UserExists(postedby))
            {
                UserProfile profile = profileManager.GetUserProfile(postedby);
                if (profile != null)
                {
                    Url = profile["PictureURL"].Value.ToString();
                }
                if (profile == null)
                {
                    Url = "/Layouts/TillidSocialNetworking/noavatar.png";
                }
            }
            return Url;
        }
        public string GetPersonalSiteUrl(SPSite site, string postedby)
        {
            string Url = string.Empty;
            var currentuser = SPContext.Current.Web.CurrentUser.Name;
            SPServiceContext context = SPServiceContext.GetContext(site);
            var upm = new UserProfileManager(context);
            if (upm.UserExists(postedby))
            {
                UserProfile profile = upm.GetUserProfile(postedby);
                if (profile != null)
                {
                    Url = profile.PublicUrl.AbsoluteUri;
                }
                if (profile == null)
                {
                    Url = "Display is not avialiable to access the My Site URL";
                }

            }
            return Url;
        }
        public string GetPersonalProfileUrl(SPSite site, string postedby)
        {
            string Url = string.Empty;
            var currentuser = SPContext.Current.Web.CurrentUser.Name;
            var context = SPServiceContext.GetContext(site);

            var upm = new UserProfileManager(context);
            if (upm.UserExists(postedby))
            {
                UserProfile profile = upm.GetUserProfile(postedby);
                if (profile != null)
                {

                    Url = profile.PublicUrl.AbsoluteUri;
                }
                if (profile == null)
                {

                    Url = "Display is not avialiable to access the My Site URL";
                }
            }
            return Url;
        }
    }
}
