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
        @import url("https://fonts.googleapis.com/css?family=Fira+Sans");
        /*Variables */
        .left-section .inner-content {
            position: absolute;
            top: 50%;
            transform: translateY(-50%);
        }

        * {
            box-sizing: border-box;
        }

        html, body {
            margin: 0;
            padding: 0;
        }

        body {
            font-family: "Fira Sans", sans-serif;
            color: #f5f6fa;
        }

        .background {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: linear-gradient(#0C0E10, #446182);
        }

            .background .ground {
                position: absolute;
                bottom: 0;
                width: 100%;
                height: 25vh;
                background: #0C0E10;
            }

        @media (max-width: 770px) {
            .background .ground {
                height: 0vh;
            }
        }

        .container {
            position: relative;
            margin: 0 auto;
            width: 85%;
            height: 100vh;
            padding-bottom: 25vh;
            display: flex;
            flex-direction: row;
            justify-content: space-around;
        }

        @media (max-width: 770px) {
            .container {
                flex-direction: column;
                padding-bottom: 0vh;
            }
        }

        .left-section, .right-section {
            position: relative;
        }

        .left-section {
            width: 40%;
        }

        @media (max-width: 770px) {
            .left-section {
                width: 100%;
                height: 40%;
                position: absolute;
                top: 0;
            }
        }

        @media (max-width: 770px) {
            .left-section .inner-content {
                position: relative;
                padding: 1rem 0;
            }
        }

        .heading {
            text-align: center;
            font-size: 9em;
            line-height: 1.3em;
            margin: 2rem 0 0.5rem 0;
            padding: 0;
            text-shadow: 0 0 1rem #fefefe;
        }

        @media (max-width: 770px) {
            .heading {
                font-size: 7em;
                line-height: 1.15;
                margin: 0;
            }
        }

        .subheading {
            text-align: center;
            max-width: 480px;
            font-size: 1.5em;
            line-height: 1.15em;
            padding: 0 1rem;
            margin: 0 auto;
        }

        @media (max-width: 770px) {
            .subheading {
                font-size: 1.3em;
                line-height: 1.15;
                max-width: 100%;
            }
        }

        .right-section {
            width: 50%;
        }

        @media (max-width: 770px) {
            .right-section {
                width: 100%;
                height: 60%;
                position: absolute;
                bottom: 0;
            }
        }

        .svgimg {
            position: absolute;
            bottom: 0;
            padding-top: 10vh;
            padding-left: 1vh;
            max-width: 100%;
            max-height: 100%;
        }

        @media (max-width: 770px) {
            .svgimg {
                padding: 0;
            }
        }

        .svgimg .bench-legs {
            fill: #0C0E10;
        }

        .svgimg .top-bench, .svgimg .bottom-bench {
            stroke: #0C0E10;
            stroke-width: 1px;
            fill: #5B3E2B;
        }

            .svgimg .bottom-bench path:nth-child(1) {
                fill: #432d20;
            }

        .svgimg .lamp-details {
            fill: #202425;
        }

        .svgimg .lamp-accent {
            fill: #2c3133;
        }

        .svgimg .lamp-bottom {
            fill: linear-gradient(#202425, #0C0E10);
        }

        .svgimg .lamp-light {
            fill: #EFEFEF;
        }

        @keyframes glow {
            0% {
                text-shadow: 0 0 1rem #fefefe;
            }

            50% {
                text-shadow: 0 0 1.85rem #ededed;
            }

            100% {
                text-shadow: 0 0 1rem #fefefe;
            }
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
