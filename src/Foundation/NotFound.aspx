<%@ Page Language="c#" AutoEventWireup="false" Inherits="BVNetwork.NotFound.Core.NotFoundPage.NotFoundBase" %>

<%--
    Note! This file has no code-behind. It inherits from the NotFoundBase class. You can
    make a copy of this file into your own project, change the design and keep the inheritance
    WITHOUT having to reference the BVNetwork.EPi404.dll assembly.

    If you want to use your own Master Page, inherit from SimplePageNotFoundBase instead of
    NotFoundBase, as that will bring in what is needed by EPiServer. Note! you do not need to
    create a page type for this 404 page. If you use the EPiServer API, and inherit from
    SimplePageNotFoundBase, this page will run in the context of the site start page.

    Be very careful with the code you write here. If you reference resources that cannot be found
    you could end up in an infinite loop.

    The code behind file might do a redirect to a new page based on the redirect configuration if
    it matches the url not found. The Error event (where the rest of the redirection is done)
    might not run for .aspx files that are not found, instead it redirects here with the url it
    could not find in the query string.

    Available properties:
        Content (BVNetwork.FileNotFound.Content.PageContent)
            // Labels you can use - fetched from the language file
            Content.BottomText
            Content.CameFrom
            Content.LookingFor
            Content.TopText
            Content.Title

        UrlNotFound (string)
            The url that cannot be found

        Referer (string)
            The url that brought the user here
            It no referer, the string is empty (not null)

    If you are using a master page, you should add this:
        <meta content="noindex, nofollow" name="ROBOTS">
    to your head tag for this page (NOT all pages)
 --%>

<script runat="server" type="text/C#">
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        // Add your own logic (like databinding) here
    }
</script>

<%@ Register TagPrefix="EPiServer" Namespace="EPiServer.WebControls" Assembly="EPiServer" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title><%= Content.Title %></title>
    <meta content="noindex, nofollow" name="ROBOTS" />
    <style type="text/css">
        body
        {
            font-family: Verdana, Arial, Helvetica, Tahoma;
            font-size: 0.65em;
            color: #333;
            background-color: #ffffff;
        }

        a
        {
            color: 0000cc;
        }
        a:hover
        {
            color: #000;
            text-decoration: none;
        }
        h1
        {
            font-weight: bold;
            font-size: 1.8em;
            color: #606060;
            margin-bottom: 0.5em;
            margin-top: 0.5em;
        }
        div.lookingfor
        {
            border: #660033 1px solid;
            padding: 10px;
            background-color: #ffdab5;
            width: 100%;
        }
        .notfoundbox
        {
            border-bottom: solid 1px #cccccc;
            border-right: solid 1px #cccccc;
            border-left: solid 1px #f8f8f8;
            border-top: solid 1px #f8f8f8;
            padding: 10px 10px 10px 10px;
            width: 100%;
            background-color: #f8f8f8;
            font-weight: bold;
            width: 100%;
        }
        .logo
        {
            font-family: Verdana;
            font-size: 5em;
            color: #a0a0a0;
            padding-bottom: 0.5em;
            letter-spacing: -0.08em;
        }
        div.floater
        {
            position: absolute;
            bottom: 0;
            right: 0;
            font-family: Times New Roman;
            font-size: 10em;
            font-style: italic;
            color: #f0f0f0;
            margin: 0 20px 10px 0;
            letter-spacing: -0.08em;
        }
    </style>
</head>
<body>
    <form id="FileNotFoundForm" method="post" runat="server">
    <div class="logo">
        Company Logo Here
    </div>
    <h1>
        <%= Content.Title %></h1>
    <div style="width: 760px">
        <div style="padding-left: 10px; float: left; width: 550px">
            <%= Content.TopText %>
            <%= Content.LookingFor %>
            <div class="notfoundbox">
                <%= HttpUtility.HtmlEncode(UrlNotFound.ToString()) %>
                <%= Referer.Length > 0 ? Content.CameFrom : "" %>
                <%= Referer.Length > 0 ? HttpUtility.HtmlEncode(Referer) : "" %>
            </div>
            <%= Content.BottomText %>
        </div>
        <div style="padding-right: 10px; padding-left: 10px; float: right; width: 200px">
            &nbsp;
        </div>
    </div>
    <div class="floater">
        404
    </div>
    </form>
</body>
</html>
