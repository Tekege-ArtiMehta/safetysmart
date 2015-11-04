<%@ Page Title="Poster" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/BongardeMaster.Master" CodeBehind="Poster.aspx.vb" Inherits="SafetySmart.Poster1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/Custom.css" rel="Stylesheet" />
    <script src="../jquery/jquery-1.4.2.js" type="text/javascript"></script>
    <script src="../jquery/jquery.accessible-news-slider.js" type="text/javascript"></script>
    <%--<script type ="text/javascript" >
  var Editor1CountLimit = 2500;

        $(document).ready(function() {
            TrackCharacterCount( Editor1CountLimit);
        });

        function TrackCharacterCount(limit) 
        {
      
                      $(function() {
    var content = $('#ctl00_contentPlaceHolderTask_txtcontent').val();

    $('#ctl00_contentPlaceHolderTask_txtcontent').keyup(function() { 
        if ($('#ctl00_contentPlaceHolderTask_txtcontent').val() != content) {
            content = $('#ctl00_contentPlaceHolderTask_txtcontent').val();
            $('#ctl00_contentPlaceHolderTask_pnlPoster p').html(content); 
        }
    });
});

        }

  </script>--%>
    <script type="text/javascript">


        $(function () {

            $("#example_2").accessNews({

                speed: "normal",
                slideBy: 1
            });

        });

    </script>
    <style type="text/css">
        /* slider setting */
        p.back, p.next {
            margin: 0px;
        }

        .accessible_news_slider LI P {
            margin: 0px;
            padding: 0px;
        }

        .accessible_news_slider {
            OVERFLOW: hidden;
            POSITION: relative;
        }

            .accessible_news_slider .back {
                Z-INDEX: 1000;
                WIDTH: 20px;
                HEIGHT: 120px;
                top: 0px;
                left: 0px;
                POSITION: absolute;
            }

                .accessible_news_slider .back A {
                    DISPLAY: none;
                    outline: none;
                }

            .accessible_news_slider .next {
                Z-INDEX: 1000;
                RIGHT: 0px;
                top: 0px;
                POSITION: absolute;
                WIDTH: 20px;
                HEIGHT: 120px;
            }

                .accessible_news_slider .next A {
                    DISPLAY: none;
                    outline: none;
                }

            .accessible_news_slider ul {
                padding: 0px;
                z-index: 1;
                display: block;
                left: 0px;
                margin: 0px 0px 0px 100px;
                overflow: hidden;
                list-style-type: none;
                POSITION: relative;
                height: 118px;
                margin: 0px;
                padding: 0px;
                border: 1px solid #fff;
            }

            .accessible_news_slider LI {
                DISPLAY: inline;
                FLOAT: left;
                WIDTH: 100px;
                padding: 9px 0px;
                OVERFLOW: hidden;
            }

                .accessible_news_slider LI img {
                    display: inline;
                    width: 75px;
                    height: 75px;
                }

                .accessible_news_slider LI a {
                    border: 1px solid #002e3e;
                    display: block;
                    float: right;
                    margin: 0px 1px 0px 6px;
                }

                    .accessible_news_slider LI a:hover {
                        border-color: #fff;
                    }

        .candy_coated {
            MARGIN: 0px 0px 0px 10px;
            WIDTH: 600px;
        }

        .flatview img {
            visibility: visible !important;
            display: block !important;
        }

        .editorclass {
            width: 350px;
            height: 200px;
        }

        /* slider setting close */
    </style>
    <script type="text/javascript" language="javascript">
        function SetUniqueRadioButton(nameregex, current) {

            re = new RegExp(nameregex);
            for (i = 0; i < document.forms[0].elements.length; i++) {
                elm = document.forms[0].elements[i]
                if (elm.type == 'radio') {
                    if (re.test(elm.name)) {
                        elm.checked = false;
                    }
                }
            }
            current.checked = true;
        }

        function deselectRadioButtonPeers(current) {
            // uncheck any radio buttons with the same id as current
            // except for the digit characters, which are wildcarded

            regex = new RegExp(current.id.replace(/\d/g, "\\d"))
            for (i = 0; i < document.forms[0].elements.length; i++) {
                elm = document.forms[0].elements[i];
                if (elm.type == 'radio' && regex.test(elm.id) && elm != current) {
                    elm.checked = false;
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentPlaceHolderTask" runat="Server">
    <h1>Create Your Safety Poster</h1>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td><b>1. Select a Subject using one of the following options:</b></td>
        </tr>
        <tr>
            <td>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>Enter your search item(s):<br />
                            <asp:TextBox ID="txtSearch" runat="server" AutoComplete="Off" name="searchterms"></asp:TextBox>
                            <%--<asp:LinkButton ID ="lnkbtnSearch" runat ="server" ><img src ="images/shared/button-search.gif"/></asp:LinkButton>--%>
                            <asp:Button ID="lnkbtnSearch" runat="server" Text="GO" />
                        </td>
                        <td>OR select by topic:<br />
                            <asp:DropDownList ID="ddlSearchByTopic" runat="server" AutoPostBack="true"></asp:DropDownList></td>
                        <td>OR select by industry:<br />
                            <asp:DropDownList ID="ddlSearchByIndustry" runat="server" AutoPostBack="true"></asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>&nbsp;<br />
                <b>2. Select you materials</b>
                <p style="text-align: justify;">
                    Select an image from the results and add a slogan that you would like in your Safety Poster then select the "Send to PDF" button at the bottom of the page when finished.
                </p>
            </td>
        </tr>
        <tr>
            <td class="article">

                <%-- <div class="accessible_news_slider candy_coated" id="example_2">--%>
                <%--<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td><p class ="back" ><a title="Back" href="#"><img src="images/Img/leftarrow1.png" border="0"></a></p></td>
    <td><ul>--%>
                <div style="width: 555px; overflow-x: scroll;" id="DIvPoster" runat="server" visible="false">
                    <asp:DataList ID="dlPosterImage" runat="server" RepeatDirection="Horizontal">
                        <ItemTemplate>

                            <asp:Image ID="imgPoster" runat="server" Style="padding: 0px 3px 0px 3px;" /><br />
                            <asp:RadioButton ID="RadioSelect" runat="server" AutoPostBack="true" Style="margin-left: 20px;" />
                        </ItemTemplate>
                    </asp:DataList>
                </div>
                <br />
                <%--</ul>
  </td>
    <td><p class ="next"><a title="Next" href="#"><img src="images/Img/rightarrow.png" border="0"></a></p></td>
  </tr>
</table>--%>


                <%--</div>--%>




                <div visible="false" id="divPrintDocument" runat="server">

                    <!-- Poster Will Come Here-->
                    <asp:Panel ID="pnlPoster" runat="server">
                        <div style="max-width: 550px;">
                            <asp:Literal ID="ltrlPoster" runat="server"></asp:Literal>
                        </div>
                        <br />
                        <p style="text-align: center;">
                            <asp:Literal ID="LtrlText" runat="server"></asp:Literal></p>
                        <!-- Poster Closed-->
                    </asp:Panel>

                    &nbsp;

              <table width="100%" cellspacing="0" cellpadding="0">
                  <tr>
                      <td>
                          <h2>Slogan</h2>
                          <asp:TextBox ID="txtcontent" runat="server" TextMode="MultiLine" MaxLength="100" Style="width: 250px;"></asp:TextBox>
                          <br />
                          <asp:LinkButton ID="lnkPrintDocument" runat="server" Style="color: #940d0b;">Send To PDF</asp:LinkButton>
                          &nbsp;
                      </td>
                  </tr>
              </table>
                </div>

            </td>
        </tr>
    </table>

</asp:Content>
