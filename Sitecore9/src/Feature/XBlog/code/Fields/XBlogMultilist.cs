using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Web.UI;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Web.UI.HtmlControls.Data;
using Sitecore.Resources;
using Sitecore.Feature.XBlog.General;
using Sitecore.Feature.XBlog.Models.Blog;
using Sitecore.Feature.XBlog.Items.Blog;

namespace Sitecore.Feature.XBlog.Fields
{
    
    public class XBlogMultilist : MultilistEx
    {
        private void RewriteSource()
        {
            if (string.IsNullOrEmpty(this.Source))
            {
                return;
            }
            if (this.Source.ToLower().Contains("settingsearch"))
            {
                Item current = Sitecore.Context.ContentDatabase.GetItem(this.ItemID, Language.Parse(this.ItemLanguage));
                BlogSettings settingsItem = DataManager.GetBlogSettingsItem(current);
                if (settingsItem != null)
                {
                    if (this.Source.ToLower().Contains("category"))
                    {
                        this.Source = settingsItem.CategoryFolder.Paths.FullPath;
                    }

                    if (this.Source.ToLower().Contains("author"))
                    {
                        this.Source = settingsItem.AuthorFolder.Paths.FullPath;
                    }

                    if (this.Source.ToLower().Contains("tag"))
                    {
                        this.Source = settingsItem.TagFolder.Paths.FullPath;
                    }
                }
            }
        }




        protected override void DoRender(HtmlTextWriter output)
        {
            this.RewriteSource();

            IDictionary dictionary;
            ArrayList list;
            Assert.ArgumentNotNull(output, "output");
            Item current = Sitecore.Context.ContentDatabase.GetItem(this.ItemID, Language.Parse(this.ItemLanguage));
            Item[] sources = null;
            using (new LanguageSwitcher(this.ItemLanguage))
            {
                sources = LookupSources.GetItems(current, this.Source);
            }
            this.GetSelectedItems(sources, out list, out dictionary);
            base.ServerProperties["ID"] = this.ID;
            string str = string.Empty;
            if (this.ReadOnly)
            {
                str = " disabled=\"disabled\"";
            }
            output.Write("<input id=\"" + this.ID + "_Value\" type=\"hidden\" value=\"" + StringUtil.EscapeQuote(this.Value) + "\" />");
            output.Write("<table" + this.GetControlAttributes() + ">");
            output.Write("<tr>");
            output.Write("<td class=\"scContentControlMultilistCaption\" width=\"50%\">" + Translate.Text("All") + "</td>");
            output.Write("<td width=\"20\">" + Images.GetSpacer(20, 1) + "</td>");
            output.Write("<td class=\"scContentControlMultilistCaption\" width=\"50%\">" + Translate.Text("Selected") + "</td>");
            output.Write("<td width=\"20\">" + Images.GetSpacer(20, 1) + "</td>");
            output.Write("</tr>");
            output.Write("<tr>");
            output.Write("<td valign=\"top\" height=\"100%\">");
            output.Write("<select id=\"" + this.ID + "_unselected\" class=\"scContentControlMultilistBox\" multiple=\"multiple\" size=\"10\"" + str + " ondblclick=\"javascript:scContent.multilistMoveRight('" + this.ID + "')\" onchange=\"javascript:document.getElementById('" + this.ID + "_all_help').innerHTML=this.selectedIndex>=0?this.options[this.selectedIndex].innerHTML:''\" >");
            foreach (DictionaryEntry entry in dictionary)
            {
                Item item = entry.Value as Item;
                if (item != null)
                {
                    output.Write("<option value=\"" + this.GetItemValue(item) + "\">" + item.DisplayName + "</option>");
                }
            }
            output.Write("</select>");
            output.Write("</td>");
            output.Write("<td valign=\"top\">");
            this.RenderButton(output, "Core/16x16/arrow_blue_right.png", "javascript:scContent.multilistMoveRight('" + this.ID + "')");
            output.Write("<br />");
            this.RenderButton(output, "Core/16x16/arrow_blue_left.png", "javascript:scContent.multilistMoveLeft('" + this.ID + "')");
            output.Write("</td>");
            output.Write("<td valign=\"top\" height=\"100%\">");
            output.Write("<select id=\"" + this.ID + "_selected\" class=\"scContentControlMultilistBox\" multiple=\"multiple\" size=\"10\"" + str + " ondblclick=\"javascript:scContent.multilistMoveLeft('" + this.ID + "')\" onchange=\"javascript:document.getElementById('" + this.ID + "_selected_help').innerHTML=this.selectedIndex>=0?this.options[this.selectedIndex].innerHTML:''\">");
            for (int i = 0; i < list.Count; i++)
            {
                Item item3 = list[i] as Item;
                if (item3 != null)
                {
                    output.Write("<option value=\"" + this.GetItemValue(item3) + "\">" + item3.DisplayName + "</option>");
                }
                else
                {
                    string path = list[i] as string;
                    if (path != null)
                    {
                        string str3;
                        Item item4 = Sitecore.Context.ContentDatabase.GetItem(path);
                        if (item4 != null)
                        {
                            str3 = item4.DisplayName + ' ' + Translate.Text("[Not in the selection List]");
                        }
                        else
                        {
                            str3 = path + ' ' + Translate.Text("[Item not found]");
                        }
                        output.Write("<option value=\"" + path + "\">" + str3 + "</option>");
                    }
                }
            }
            output.Write("</select>");
            output.Write("</td>");
            output.Write("<td valign=\"top\">");
            this.RenderButton(output, "Core/16x16/arrow_blue_up.png", "javascript:scContent.multilistMoveUp('" + this.ID + "')");
            output.Write("<br />");
            this.RenderButton(output, "Core/16x16/arrow_blue_down.png", "javascript:scContent.multilistMoveDown('" + this.ID + "')");
            output.Write("</td>");
            output.Write("</tr>");
            output.Write("<tr>");
            output.Write("<td valign=\"top\">");
            output.Write("<div style=\"border:1px solid #999999;font:8pt tahoma;padding:2px;margin:4px 0px 4px 0px;height:14px\" id=\"" + this.ID + "_all_help\"></div>");
            output.Write("</td>");
            output.Write("<td></td>");
            output.Write("<td valign=\"top\">");
            output.Write("<div style=\"border:1px solid #999999;font:8pt tahoma;padding:2px;margin:4px 0px 4px 0px;height:14px\" id=\"" + this.ID + "_selected_help\"></div>");
            output.Write("</td>");
            output.Write("<td></td>");
            output.Write("</tr>");
            output.Write("</table>");
        }

        private void RenderButton(HtmlTextWriter output, string icon, string click)
        {
            Assert.ArgumentNotNull(output, "output");
            Assert.ArgumentNotNull(icon, "icon");
            Assert.ArgumentNotNull(click, "click");
            ImageBuilder builder = new ImageBuilder();
            builder.Src = icon;
            builder.Width = 0x10;
            builder.Height = 0x10;
            builder.Margin = "2px";
            if (!this.ReadOnly)
            {
                builder.OnClick = click;
            }
            output.Write(builder.ToString());
        }

        
    }
}
