﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BetterCms.Module.Blog.Views.Blog.Partial
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 1 "..\..\Views\Blog\Partial\BlogsFilterTemplate.cshtml"
    using BetterCms.Module.Blog.Content.Resources;
    
    #line default
    #line hidden
    
    #line 3 "..\..\Views\Blog\Partial\BlogsFilterTemplate.cshtml"
    using BetterCms.Module.Blog.ViewModels.Blog;
    
    #line default
    #line hidden
    
    #line 4 "..\..\Views\Blog\Partial\BlogsFilterTemplate.cshtml"
    using BetterCms.Module.Blog.ViewModels.Filter;
    
    #line default
    #line hidden
    
    #line 2 "..\..\Views\Blog\Partial\BlogsFilterTemplate.cshtml"
    using BetterCms.Module.Root.Content.Resources;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Blog/Partial/BlogsFilterTemplate.cshtml")]
    public partial class BlogsFilterTemplate : System.Web.Mvc.WebViewPage<BlogsGridViewModel<SiteSettingBlogPostViewModel>>
    {
        public BlogsFilterTemplate()
        {
        }
        public override void Execute()
        {




WriteLiteral("\r\n");


WriteLiteral("<div id=\"bcms-filter-template\">\r\n    <div class=\"bcms-grid-filtering\" data-bind=\"" +
"css: { \'bcms-active-filter\': isVisible() }\">\r\n        <div class=\"bcms-filterbox" +
"\" data-bind=\"click: toggleFilter\">\r\n            ");


            
            #line 10 "..\..\Views\Blog\Partial\BlogsFilterTemplate.cshtml"
       Write(BlogGlobalization.SiteSettings_Blogs_Filter);

            
            #line default
            #line hidden
WriteLiteral("\r\n            <div class=\"bcms-filter-modified\" data-bind=\"style: { display: isEd" +
"ited() ? \'inline-block\' : \'none\' }\">");


            
            #line 11 "..\..\Views\Blog\Partial\BlogsFilterTemplate.cshtml"
                                                                                                              Write(BlogGlobalization.SiteSettings_Blogs_FilterIsModified);

            
            #line default
            #line hidden
WriteLiteral(@"</div>
        </div>
    </div>
    <div class=""bcms-tags-block"" style=""display: none;"" data-bind=""visible: isVisible()"">
        <div class=""bcms-featured-tags"">
            <div class=""bcms-clearfix"">

                <div class=""bcms-filter-options"" data-bind=""with: tags"">
                    <div class=""bcms-filter-text"">");


            
            #line 19 "..\..\Views\Blog\Partial\BlogsFilterTemplate.cshtml"
                                             Write(BlogGlobalization.SiteSettings_Blogs_FilterByTags);

            
            #line default
            #line hidden
WriteLiteral(@"</div>
                    <div class=""bcms-filter-field-holder"">
                        <input type=""text"" class=""bcms-add-tags-field"" data-bind=""
    css: { 'bcms-tag-validation-error': newItem.hasError() },
    value: newItem,
    valueUpdate: 'afterkeydown',
    escPress: clearItem,
    autocompleteList: 'onlyExisting'"" />
                        <!-- ko if: newItem.hasError()  -->
                        <span class=""bcms-tag-field-validation-error"">
                            <span data-bind=""text: newItem.validationMessage()""></span>
                        </span>
                        <!-- /ko -->
                    </div>
                </div>

                <div class=""bcms-filter-options"">
                    <div class=""bcms-filter-text"">");


            
            #line 36 "..\..\Views\Blog\Partial\BlogsFilterTemplate.cshtml"
                                             Write(BlogGlobalization.SiteSettings_Blogs_FilterByCategory);

            
            #line default
            #line hidden
WriteLiteral(@"</div>
                    <select class=""bcms-global-select"" name=""CategoryId"" data-bind=""options: categories, value: categoryId, optionsText: 'Value', optionsValue: 'Key'"" />
                </div>
                
                <div class=""bcms-filter-options"">
                    <div class=""bcms-filter-text"">");


            
            #line 41 "..\..\Views\Blog\Partial\BlogsFilterTemplate.cshtml"
                                             Write(BlogGlobalization.SiteSettings_Blogs_FilterByStatus);

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                    ");


            
            #line 42 "..\..\Views\Blog\Partial\BlogsFilterTemplate.cshtml"
               Write(Html.DropDownListFor(m => m.Status, new List<SelectListItem>(), new
                                                                      {
                                                                          data_bind = "options: statuses, value: status, optionsText: 'Value', optionsValue: 'Key'",
                                                                          @class = "bcms-global-select"
                                                                      }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n\r\n                <div class=\"bcms-filter-options\">\r\n  " +
"                  <div class=\"bcms-filter-text\">");


            
            #line 50 "..\..\Views\Blog\Partial\BlogsFilterTemplate.cshtml"
                                             Write(BlogGlobalization.SiteSettings_Blogs_FilterBySEO);

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                    ");


            
            #line 51 "..\..\Views\Blog\Partial\BlogsFilterTemplate.cshtml"
               Write(Html.DropDownListFor(m => m.SeoStatus, new List<SelectListItem>(), new
                                                                                            {
                                                                                                data_bind = "options: seoStatuses, value: seoStatus, optionsText: 'Value', optionsValue: 'Key'",
                                                                                                @class = "bcms-global-select"
                                                                                            }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n\r\n");


            
            #line 58 "..\..\Views\Blog\Partial\BlogsFilterTemplate.cshtml"
                 if (Model.Languages != null && Model.Languages.Any())
                {

            
            #line default
            #line hidden
WriteLiteral("                    <div class=\"bcms-filter-options\">\r\n                        <d" +
"iv class=\"bcms-filter-text\">");


            
            #line 61 "..\..\Views\Blog\Partial\BlogsFilterTemplate.cshtml"
                                                 Write(BlogGlobalization.SiteSettings_Blogs_FilterByLanguage);

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                        <select class=\"bcms-global-select\" name=\"Language" +
"Id\" data-bind=\"options: languages, value: languageId, optionsText: \'Value\', opti" +
"onsValue: \'Key\'\" />\r\n                    </div>\r\n");


            
            #line 64 "..\..\Views\Blog\Partial\BlogsFilterTemplate.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral(@"
            </div>

            <div class=""bcms-single-tag-holder"" data-bind=""foreach: tags.items()"">
                <div class=""bcms-single-tag"" data-bind=""css: { 'bcms-single-tag-active': isActive() }"">
                    <span data-bind=""text: name()""></span><a data-bind=""    click: remove"">");


            
            #line 70 "..\..\Views\Blog\Partial\BlogsFilterTemplate.cshtml"
                                                                                      Write(RootGlobalization.Button_Remove);

            
            #line default
            #line hidden
WriteLiteral(@"</a>
                </div>
                <input type=""hidden"" data-bind=""attr: { name: getItemInputName($index()) + '.Key', value: id() }"" />
                <input type=""hidden"" data-bind=""attr: { name: getItemInputName($index()) + '.Value', value: name() }"" />
            </div>

        </div>

        <div class=""bcms-clearfix"">
            <div class=""bcms-check-field-holder"">
                ");


            
            #line 80 "..\..\Views\Blog\Partial\BlogsFilterTemplate.cshtml"
           Write(Html.CheckBoxFor(model => model.IncludeArchived, new { data_bind = "checked: includeArchived" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                <span class=\"bcms-pointer\" data-bind=\"click: changeIncludeArchi" +
"ved\">");


            
            #line 81 "..\..\Views\Blog\Partial\BlogsFilterTemplate.cshtml"
                                                                               Write(BlogGlobalization.SiteSettings_Blogs_FilterIncludeArchived);

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n            </div>\r\n            <div class=\"bcms-btn-field-holder\">\r\n   " +
"             <div class=\"bcms-btn-links-small\" data-bind=\"click: clearFilter\">");


            
            #line 84 "..\..\Views\Blog\Partial\BlogsFilterTemplate.cshtml"
                                                                            Write(BlogGlobalization.SiteSettings_Blogs_FilterClear);

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                <div class=\"bcms-btn-small\" data-bind=\"click: searchWithF" +
"ilter\">");


            
            #line 85 "..\..\Views\Blog\Partial\BlogsFilterTemplate.cshtml"
                                                                           Write(BlogGlobalization.SiteSettings_Blogs_FilterSearch);

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n");


        }
    }
}
#pragma warning restore 1591