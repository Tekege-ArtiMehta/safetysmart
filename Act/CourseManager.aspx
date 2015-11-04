<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/BongardeMaster.Master" CodeBehind="CourseManager.aspx.vb" Inherits="SafetySmart.CourseManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
.confirmationPopUP
{

position:fixed;
_position:absolute; /* hack for internet explorer 6*/
height:auto;
font-size:13px;
background-color: #f5f7f7;
background-repeat: repeat-x;
width:544px;
}
.confirmationPopUP h1
{
    font-family: Arial, Helvetica, sans-serif;
	font-size:16px;
	font-weight:bold;
	color:#2a3438;
	background-color:#eacfce;
	padding:6px 10px;
	margin-top: 0px;
	margin-bottom: 0px;
	line-height: 1em;
}

 .txtenddate
 {
	width: 117px;
	margin-right: 8px;
 }
.txtstartdate{
	width: 117px;
}
.ddlDateRange{
	height: 21px;
	margin-right: 8px;
	margin-left: 8px;
}
.divDateFilter{
	padding-bottom: 10px;
	display:inline-flex;
	
}


</style>
<script type="text/javascript" src="../jquery/jquery-1.4.2.js"></script>
 
   
<link href ="../css/general.css" rel ="Stylesheet" />
<link href ="../css/CalendarEvent.css" rel ="Stylesheet" />

<script type ="text/javascript" language ="javascript" >
    function DescriptionPOpUP(url) {
        var width = 600;
        var height = 200;
        var left = (screen.width - width) / 2;
        var top = (screen.height - height) / 2;
        var params = 'width=' + width + ', height=' + height;
        params += ', top=' + top + ', left=' + left;
        params += ', directories=no';
        params += ', location=no';
        params += ', menubar=no';
        params += ', resizable=no';
        params += ', scrollbars=no';
        params += ', status=no';
        params += ', toolbar=no';
        newwin = window.open(url, 'windowname5', params);
        if (window.focus) { newwin.focus() }
        return false;
    }

    var popupStatus = 0;

    //loading popup with jQuery magic!
    function loadPopup() {
        //loads popup only if it is disabled
        if (popupStatus == 0) {
            $("#backgroundPopup").css({
                "opacity": "0.7"
            });
            $("#backgroundPopup").fadeIn("slow");
            $("#popupContact1").fadeIn("slow");
            popupStatus = 1;
        }
    }

    //disabling popup with jQuery magic!
    function disablePopup() {
        //disables popup only if it is enabled
        if (popupStatus == 1) {
            $("#backgroundPopup").fadeOut("slow");
            $("#popupContact1").fadeOut("slow");
            popupStatus = 0;
        }
    }

    //centering popup
    function centerPopup() {
        //request data for centering
        var windowWidth = document.documentElement.clientWidth;
        var windowHeight = document.documentElement.clientHeight;
        var popupHeight = $("#popupContact1").height();
        var popupWidth = $("#popupContact1").width();
        //centering
        $("#popupContact1").css({
            //"position": "absolute",
            "top": windowHeight / 2 - popupHeight / 2,
            "left": windowWidth / 2 - popupWidth / 2
        });
        //only need force for IE6

        $("#backgroundPopup").css({
            //"height": windowHeight
        });

    }



    $(document).ready(function () {

        //CLOSING POPUP
        //Click the x event!
        $("#popupContactClose1").click(function () {
            disablePopup();
        });
        //Click out event!
        $("#backgroundPopup").click(function () {
            disablePopup();
        });
        //Press Escape event!
        $(document).keypress(function (e) {
            if (e.keyCode == 27 && popupStatus == 1) {
                disablePopup();
            }
        });

    });

    //CLOSING POPUP
    //Click the x event!






    $(document).ready(function () {

        //alert("test");       
        var tP = document.getElementById("ctl00_contentPlaceHolderTask_gvActive");
        var theadP = tP.getElementsByTagName("thead")[0];
        var t1P = tP.cloneNode(false);

        t1P.appendChild(theadP);
        var tableHeaderP = document.getElementById("thdWYNTD");
        tableHeaderP.appendChild(t1P);

        var t = document.getElementById("ctl00_contentPlaceHolderTask_gvMaster");
        var thead = t.getElementsByTagName("thead")[0];
        var t1 = t.cloneNode(false);
        t1.appendChild(thead);
        var tableHeader = document.getElementById("divMaster");
        tableHeader.appendChild(t1);

    });


    function OpenPopUp(temp, temp1) {
        document.getElementById('ctl00_contentPlaceHolderTask_hiddenField').value = temp;
        document.getElementById('ctl00_contentPlaceHolderTask_hiddenField1').value = temp1
        //centering with css
        centerPopup();
        //load popup
        loadPopup();
    }
    function OpenPopUpWindow(courseId) {
        //debugger;
        // centerPopup ();
        loadPopup();
        $('#ctl00_contentPlaceHolderTask_iframe1').attr('src', courseId);


    }


    function fixHeader() {
        var tP = document.getElementById("ctl00_contentPlaceHolderTask_gvActive");
        var theadP = tP.getElementsByTagName("thead")[0];
        var t1P = tP.cloneNode(false);

        t1P.appendChild(theadP);
        var tableHeaderP = document.getElementById("thdWYNTD");
        tableHeaderP.appendChild(t1P);

        var t = document.getElementById("ctl00_contentPlaceHolderTask_gvMaster");
        var thead = t.getElementsByTagName("thead")[0];
        var t1 = t.cloneNode(false);
        t1.appendChild(thead);
        var tableHeader = document.getElementById("divMaster");
        tableHeader.appendChild(t1);
    }
    //window.onload = fixHeader;


    function fixHeader1() {

        var tP = document.getElementById("ctl00_contentPlaceHolderTask_gvActive");
        var theadP = tP.getElementsByTagName("thead")[0];
        var t1P = tP.cloneNode(false);

        t1P.appendChild(theadP);
        var tableHeaderP = document.getElementById("thdWYNTD");
        tableHeaderP.appendChild(t1P);

        var t = document.getElementById("ctl00_contentPlaceHolderTask_gvMaster");
        var thead = t.getElementsByTagName("thead")[0];
        var t1 = t.cloneNode(false);
        t1.appendChild(thead);
        var tableHeader = document.getElementById("divMaster");
        tableHeader.appendChild(t1);
    }

    function CloseCOnfirmationPanel() {
        $find("ConfirmationPanel").hide();
        $find("confirmationDeletePanel").hide();
        return false;
    }
    function ConfirmationMessage(id) {
        var flag = false;
        $("#<%=gvMaster.ClientID %> tbody tr").each(function () {
            //Skip first(header) row
            //if (!this.rowIndex) return;
            var selectedValue = $(this).find("td:nth-child(5) select option:selected").text();
            //var dtmpDate=new Date (providedDate);
            //dtmpDate.setDate(dtmpDate.getDate()+parseInt(notifyAfter.toString()));
            //var dateFormat=dtmpDate .getMonth ()+1 + "/"+dtmpDate .getDate ()+"/"+dtmpDate.getFullYear();
            //$(this).find("td:nth-child(5) span").text(dateFormat.toString());
            var clickedBtnId = $(this).find("td:nth-child(5) a").attr("title");
            if ((selectedValue == "Delete") && (clickedBtnId == id)) {

                var SelectedElementId = $(this).find("td:nth-child(5) a").attr("title");
                $("#ctl00_contentPlaceHolderTask_lnkBtnYes").attr("title", SelectedElementId);
                $("#ctl00_contentPlaceHolderTask_hdnSelectedMeetingId").val(SelectedElementId);
                $find("ConfirmationPanel").show();
                flag = false;
                return flag;
            }
            else {
                flag = true;
            }
        });
        if (flag == true) {
            return flag;
        }
        else {
            return flag;
        }

    }
    //New javascript Function For new Deltion Confirmation Box
    function ConfirmationMessageDelete(id, msg) {
        var flag = false;
        $("#<%=gvMaster.ClientID %> tbody tr").each(function () {
            //Skip first(header) row
            //if (!this.rowIndex) return;
            var selectedValue = $(this).find("td:nth-child(5) select option:selected").text();
            //var dtmpDate=new Date (providedDate);
            //dtmpDate.setDate(dtmpDate.getDate()+parseInt(notifyAfter.toString()));
            //var dateFormat=dtmpDate .getMonth ()+1 + "/"+dtmpDate .getDate ()+"/"+dtmpDate.getFullYear();
            //$(this).find("td:nth-child(5) span").text(dateFormat.toString());
            var clickedBtnId = $(this).find("td:nth-child(5) a").attr("title");
            if ((selectedValue == "Delete") && (clickedBtnId == id)) {

                var SelectedElementId = $(this).find("td:nth-child(5) a").attr("title");
                $("#ctl00_contentPlaceHolderTask_lnkBtnCOnfirmationDeleteYes").attr("title", SelectedElementId);
                $("#ctl00_contentPlaceHolderTask_hdnSelectedMeetingId").val(SelectedElementId);
                $("#ctl00_contentPlaceHolderTask_lblDeletionMessage").html(msg);

                $find("confirmationDeletePanel").show();
                flag = false;
                return flag;
            }
            else {
                flag = true;
            }
        });
        if (flag == true) {
            return flag;
        }
        else {
            return flag;
        }
    }
    function Confirm(did) {
        debugger;
        if (document.getElementById('<%= hdTraining.ClientID %>').value == "") {
            document.getElementById('<%= hdTraining.ClientID %>').value = did.id;
            //document.getElementById('<%= hdTrainingRowID.ClientID %>').value = did.id;            
        }
        else if (document.getElementById(document.getElementById('<%= hdTraining.ClientID %>').value).value == "Print Attendance Sheet") {
            var id = document.getElementById('<%= hdTraining.ClientID %>').value;
            if (did.id != id) {
                document.getElementById(document.getElementById('<%= hdTraining.ClientID %>').value).value = "Select";
                document.getElementById('<%= hdTraining.ClientID %>').value = did.id;
                //var id2=document.getElementById('<%= hdTrainingRowID.ClientID %>').value;
                var id = document.getElementById('<%= hdTraining.ClientID %>').value;
            }
            else {
                //var id2=document.getElementById('<%= hdTrainingRowID.ClientID %>').value;
                var id = document.getElementById('<%= hdTraining.ClientID %>').value;
            }
        }
        else {
            var id = document.getElementById('<%= hdTraining.ClientID %>').value;
            if ((navigator.appName == "Netscape") && (did.id != id)) {
                document.getElementById(document.getElementById('<%= hdTraining.ClientID %>').value).value = "Select";
                document.getElementById('<%= hdTraining.ClientID %>').value = did.id;
                var id2 = document.getElementById('<%= hdTrainingRowID.ClientID %>').value;
            }
        }
        if (document.getElementById('<%= hdTraining2.ClientID %>').value != "") {
            document.getElementById(document.getElementById('<%= hdTraining2.ClientID %>').value).value = "Select";
        }

        var DDLId = did.value; //document.getElementById(did).SelectedItem.text;       
        __doPostBack("ddlAActions", "ddlAActions_SelectedIndexChanged");
        return true;

    }

    function Confirm2(did) {
        debugger;
        if (document.getElementById('<%= hdTraining2.ClientID %>').value == "") {
            document.getElementById('<%= hdTraining2.ClientID %>').value = did.id;
        }
        else {
            var id = document.getElementById('<%= hdTraining2.ClientID %>').value;
            if ((navigator.appName == "Netscape") && (did.id != id)) {
                document.getElementById(document.getElementById('<%= hdTraining2.ClientID %>').value).value = "Select";
                document.getElementById('<%= hdTraining2.ClientID %>').value = did.id;
            }
        }
        if (document.getElementById('<%= hdTraining.ClientID %>').value != "") {
            document.getElementById(document.getElementById('<%= hdTraining.ClientID %>').value).value = "Select";
        }
        var DDLId = did.value; //document.getElementById(did).SelectedItem.text;       
        __doPostBack("ddlMActions", "ddlMActions_SelectedIndexChanged");
        return true;

    }
    // Date range on 13th May
    function DateRangeValidator() {
        var SDate = document.getElementById('txtStartDate').value;
        var EDate = document.getElementById('txtEndDate').value;
        var alertReason1 = 'End Date must be greater than Start Date.'
        var alertReason2 = 'End Date can not be less than Current Date.';

        var endDate = new Date(EDate);
        var startDate = new Date(SDate);

        if (SDate != '' && EDate != '' && startDate > endDate) {
            document.getElementById('txtEndDate').value = "";
            alert(alertReason1);
            return false;
        }

        return true;
    }

    function ShowHideCustomDate(flag) {
        if ($("#ddlTimeInterval").val() == 4) {
            $("#divCustomDate").show();
            if (flag == 1) {
                $("#txtStartDate").val("");
                $("#txtEndDate").val("");
            }
        }
        else {
            $("#divCustomDate").hide();
        }
    }
    function CustomdivValidation() {
        if ($("#ddlTimeInterval").val() == 4) {

            if ($("#txtStartDate").val() == "" && $("#txtEndDate").val() == "") {
                alert("Enter Start Date and End Date");
                return false;
            }
            else if ($("#txtStartDate").val() == "") {
                alert("Enter Start Date");
                return false;
            }
            else if ($("#txtEndDate").val() == "") {
                alert("Enter End Date");
                return false;
            }
        }
    }
</script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentPlaceHolderTask" Runat="Server">
            <h1>Training Manager <a href="http://help.safetysmart.com/article/3-trainingmanager" style="float:right;font-family: Arial, Helvetica, sans-serif;font-size:10pt;font-weight: bold;">Learn More</a></h1>
            <table width="100%">                
                <tr>
                    <td >
					 Quickly launch and manage safety meetings, eLearning assignments, and other training activities. Click <b>Assign eLearning</b> or <b>Create New Safety Meeting</b> to create your own activity. To use an existing template, select <b>Schedule</b> from the <b>Action</b> menu for that template.
					<br />
                    <asp:Label ID="msgLabel" runat="server" Text="" Font-Bold="true" ForeColor="Blue"></asp:Label>
                    <br /><br />
                        <%--<asp:ImageButton ID="btElearn" runat="server" ImageUrl="~/images/assigneLearning.jpg" ImageAlign="AbsBottom" />&nbsp;--%>
						 <div id="divLearningBtn" runat="server" style="display:inline;"><asp:HyperLink ID ="hylnkAssignElearning" runat ="server" NavigateUrl ="../Elearning/assignCourses.aspx" class="button" >Assign Training</asp:HyperLink>&nbsp;</div>
                        <%--<asp:ImageButton ID="btMeeting" runat="server" ImageUrl="~/images/creatNewMeeting.jpg" ImageAlign="AbsBottom" />&nbsp; --%>
						<asp:HyperLink ID ="hylnkAssignMeeting" runat ="server" NavigateUrl ="../Meetings/addMeeting1.aspx?Type=Meetings" class="button2">Schedule Meeting</asp:HyperLink>&nbsp;
                        <%--<asp:ImageButton ID="btProgram" runat="server" ImageUrl="~/images/creatNewProgram.jpg" ImageAlign="AbsBottom" />&nbsp; --%>
						<%--<asp:HyperLink ID ="hylnkAssignPrg" runat ="server" NavigateUrl ="" > <asp:Image  ID="btProgram" runat="server" ImageUrl="~/images/creatNewProgram.jpg" ImageAlign="AbsBottom"  /></asp:HyperLink>&nbsp;--%>						
                        <%--<asp:ImageButton ID="btReport" runat="server" ImageUrl="~/images/runReports.jpg" ImageAlign="AbsBottom" />&nbsp; --%>     
						<%--<asp:HyperLink ID ="hylnkAssignReport" runat ="server" NavigateUrl ="../Reports/report.aspx" > <asp:Image  ID="btReport" runat="server" ImageUrl="~/images/runReports.jpg" ImageAlign="AbsBottom" /></asp:HyperLink>&nbsp;--%>  						
                    </td>
                </tr>
                <tr >
                  <td style="text-align:center;">
				  <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always" >
				  <ContentTemplate>
                   <div id="divhide1" runat="server" >
                    <br />
                        <p class="title_red_no_bottom_margin" style="text-align:left; background-color:#FFFFFF; font-size:23px; padding-top:12px; padding-bottom:12px; padding-left:0px; font-weight:normal; text-transform:none; color:#555555; <!-- border-top:1px solid #bbbbbb -->;">Scheduled Activities<br />
						<!-- <span style="text-align:left; line-height:20pt; background-color:#FFFFFF; font-size:9pt; padding-top:4px; font-weight:normal; text-transform:none; color:#888888;">Your organization's scheduled training activites.</span></p> -->
						<div class="divDateFilter">
						  Filter By:
							<asp:DropDownList ID="ddlTimeInterval" ClientIDMode="Static" OnChange="ShowHideCustomDate(1);" CssClass="ddlDateRange" runat="server">
								<asp:ListItem Text="Last 30 days" Value="0"></asp:ListItem>
								<asp:ListItem Text="Last 3 months" Value="1"></asp:ListItem>
								<asp:ListItem Text="Last 6 months" Value="2"></asp:ListItem>
								<asp:ListItem Text="Last 12 months" Value="3" ></asp:ListItem>
								<asp:ListItem Text="Custom" Value="4"></asp:ListItem>
                                <asp:ListItem Text="All Outstanding" Value="5" Selected="True"></asp:ListItem>
                                
							</asp:DropDownList>
							<div id="divCustomDate" runat="server" clientidmode="Static" style="float:left; display:none;">
							<asp:TextBox ID="txtStartDate" CssClass="txtstartdate" ClientIDMode="Static" runat="server" OnChange="DateRangeValidator();"></asp:TextBox><cc1:CalendarExtender
								ID="CalenderS" runat="server" TargetControlID="txtStartDate" Enabled="True" Format="MM/dd/yy">
							</cc1:CalendarExtender>
							To
							<asp:TextBox ID="txtEndDate" CssClass="txtenddate" ClientIDMode="Static" OnChange="DateRangeValidator();"
								runat="server"></asp:TextBox><cc1:CalendarExtender ID="CalenderE" runat="server"
									TargetControlID="txtEndDate" Enabled="True" Format="MM/dd/yy">
								</cc1:CalendarExtender>
								</div>
							<asp:Button ID="btnGo" runat="server" OnClientClick="return CustomdivValidation();" ClientIDMode="Static" Text="Go" />
                                    
                        </div>
						<div id="Div1" style="height:225px; overflow:hidden;border-bottom :solid 1px  #999999;" runat="server">
                        <div id="thdWYNTD" style="">
                  </div>
                       <div  style="height: 200px; width:676; overflow-y:scroll;">
                        <%--<asp:Panel ID="PanelS" runat="server" BorderStyle="Solid" BorderWidth="1px" Height="150px" >--%>
                        <asp:GridView ID="gvActive" runat="server" AutoGenerateColumns="False" DataKeyNames="CourseID" Width="100%" AllowSorting="True"  EnableViewState="True"
                         GridLines="none"  ShowHeader ="true" OnPreRender="gvActive_PreRender" EmptyDataText="<center><b>There are no activities currently scheduled.</b> Choose an option above to create a new activity, or if you have any saved activity templates below, select 'Schedule' from the Actions list for that template.</center>">
						 
                            <Columns>
                                <asp:TemplateField HeaderText="Activity Title" SortExpression="Name" ItemStyle-CssClass="manageLeft" HeaderStyle-CssClass="manageLeft"  HeaderStyle-ForeColor="#555555"  ItemStyle-Width="130"  >
								     <EditItemTemplate>
                                        <asp:TextBox ID="txtAName" runat="server" Text='<%# Bind("Name") %>' ></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <a href="javascript:void(0);" id="aCourse" runat="server" >
                                        <asp:Label ID="lbAName" runat="server" Text='<%# Bind("Name") %>' ToolTip='<%#Eval("FullName") %>' ></asp:Label></a>
                                    </ItemTemplate>
                                    
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" SortExpression="Status" ItemStyle-CssClass="manageLeft" HeaderStyle-ForeColor="#555555" HeaderStyle-CssClass="manageLeft"   ItemStyle-Width="70"  >
                                    
                                    <ItemTemplate>
                                        <asp:Label ID="lbAStatus" runat="server" Text='<%# Bind("Status") %>' Width="75px"></asp:Label>
                                    </ItemTemplate>
                                    
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Enrolled" SortExpression ="Enrollees"  ItemStyle-CssClass="manage" HeaderStyle-ForeColor="#555555" HeaderStyle-CssClass="manageLeft" HeaderStyle-Width ="60" ItemStyle-Width="50"  >
                                    
                                    <ItemTemplate>
                                        <asp:Label ID="lbAEnrolled" runat="server" Text='<%# Bind("Enrollees") %>' ></asp:Label>
                                    </ItemTemplate>                                
                                    
                                </asp:TemplateField> 
                                
                                <asp:TemplateField HeaderText="Completed" SortExpression = "Completions" ItemStyle-CssClass="manage" HeaderStyle-ForeColor="#555555" HeaderStyle-CssClass="manageLeft"  HeaderStyle-Width ="75" ItemStyle-Width="65"  >
                                    
                                    <ItemTemplate>
                                        <asp:Label ID="lbAComplete" runat="server" Text='<%# Bind("completions") %>' ></asp:Label>
                                    </ItemTemplate>                                
                                    
                                </asp:TemplateField> 
                                                           
                                <asp:TemplateField HeaderText="Due"  SortExpression = "SDate" HeaderStyle-CssClass="manageLeft" HeaderStyle-ForeColor="#555555" ItemStyle-CssClass="manageLeft"   >
                                   
                                    <ItemTemplate>
                                        <asp:Label ID="lbADate" runat="server" Text='<%# Bind("SDate" , "{0:MM/dd/yy}") %>' ToolTip='<%#Eval("DueDateIdForElearning") %>'></asp:Label>
                                    </ItemTemplate>
                                    
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Type" SortExpression="Type"  HeaderStyle-ForeColor="#555555" HeaderStyle-CssClass="manageLeft" ItemStyle-CssClass="tableMiddle"    >
                                  
                                    <ItemTemplate>
                                         <%-- CHanged By Mohit --%>
                                        <%--<asp:Label ID="lbAType" runat="server" Text='<%# Bind("Type") %>'></asp:Label>--%>
                                        <asp:Image ID ="lbAType" runat="server" ImageUrl="~/images/TrainingProgram/eLearning.JPG" ToolTip='<%# Bind("Type") %>' height="20" width="25" style="margin-left:10px;margin-right:30px;" />
                                    </ItemTemplate>
                                    
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actions" HeaderStyle-ForeColor="#555555" ItemStyle-CssClass="manageLeft" HeaderStyle-CssClass="manageLeft" HeaderStyle-Width ="140" >
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtAActions" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlAActions" runat="server" Autopostback="true" Width="120px" 
                                                AppendDataBoundItems="True" onselectedindexchanged="ddlAActions_SelectedIndexChanged">
                                            
                                        </asp:DropDownList>
                                        <asp:LinkButton ID="btGo" runat="server" CommandName="Edit" 
                                                  CommandArgument='<%# Bind("CourseID")%>' Visible="false" >GO</asp:LinkButton>                                        									
                                    </ItemTemplate>
                                    
                                </asp:TemplateField>
                                
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lbCourseId" runat="server" Text='<%# Bind("CourseID")%>' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <%--<asp:TemplateField ItemStyle-Width="30px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btGo" runat="server" AutoPostBack="true" CommandArgument='<%# Bind("CourseID")%>' 
                                             CommandName="click" ImageUrl="../images/go2.jpg" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                            <RowStyle Height="26px" />
							<SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333"  Height="30px"/>
							<HeaderStyle BackColor="#DDDDDD" Font-Bold="True" ForeColor="White" Height="30px"/>
							<EditRowStyle BackColor="#2461BF" Height="30px"/>
							<AlternatingRowStyle BackColor="#EEEEEE"  Height="30px" />
                            
                        </asp:GridView>
                    <%--</asp:Panel>--%>
					</div>
					</div>
					</div>
					<div id="processing">
						<div class="spindiv">
							<div id="content2">
								<p id="loadingspinner">
								</p>
							</div>
						</div>
                    </div>
					</ContentTemplate>             
					</asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td  style="text-align:center;">
					<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" >
						<ContentTemplate>
							<div id="divhide2" runat="server" >
								<br /><br />
								<p class="title_red_no_bottom_margin" style="text-align:left; background-color:#FFFFFF; font-size:23px; padding-top:12px; padding-bottom:12px; padding-left:2px; font-weight:normal; text-transform:none; color:#555555; <!-- border-top:1px solid #bbbbbb -->;">Meeting Templates<br />
								<!-- <span style="text-align:left; line-height:20pt; background-color:#FFFFFF; font-size:9pt; padding-top:4px; font-weight:normal; text-transform:none; color:#888888;">Training you've saved.  Ready-to-use templates also included.  Use to schedule new sessions or refreshers.</span></p> -->
								<div style="display:none;"><asp:CheckBox id="chkOneClick" runat="server" AutoPostBack="true" Checked="true" visible="false" /><span style="font-weight:normal; text-transform:none; display:none;" runat="server" id="spanOneClick" >One-Click Meetings<asp:LinkButton ID="lnkOnlyOne" runat="server" Text="only" Font-Size="Smaller" visible="false" ></asp:LinkButton></span></div>			   
								<div id="Div2" style="height:170px; overflow:hidden;border-bottom :solid 1px  #bbbbbb;" runat="server">
									<div id="divMaster" style="">
									</div>
								   <div  style="height: 144px; width:676; overflow-y:scroll;">
										<%--<asp:Panel ID="PanelMaster" runat="server" BorderStyle="Solid" BorderWidth="1px" Height="150px" >--%>
										<asp:GridView ID="gvMaster" runat="server" AutoGenerateColumns="False" DataKeyNames="ID" Width="100%"
										 GridLines="none" AllowSorting="True" OnSorting="gvMaster_Sort" EnableViewState="True" OnPreRender="gvMaster_PreRender" showheader="true" EmptyDataText="<center><b>There are no templates currently saved.</b> Choose an option above to create activity and save it as a template.</center>" >
											<Columns>
												<asp:TemplateField HeaderText="Template Name" SortExpression="fullName" HeaderStyle-ForeColor="#555555" ItemStyle-CssClass="manageLeft" HeaderStyle-CssClass="manageLeft" HeaderStyle-Width ="156px" ItemStyle-Width="145px" >
													<ItemTemplate>
														<a href="javascript:void(0);" id="aMCourse" runat="server" >
														<asp:Label ID="lbMFName" runat="server" Text='<%# Bind("fullName") %>' ToolTip='<%#Eval("CompleteName") %>'></asp:Label>
														</a>
													</ItemTemplate>
												</asp:TemplateField> 
												
												<asp:TemplateField HeaderText="Source" SortExpression="creator" HeaderStyle-ForeColor="#555555"  ItemStyle-CssClass="manageLeft" HeaderStyle-CssClass="manageLeft" HeaderStyle-Width ="112px" ItemStyle-Width="101px" >
												   
													<ItemTemplate>
														<asp:Label ID="lbMSCreator" runat="server" Text='<%# Bind("creator") %>'></asp:Label>
													</ItemTemplate>
												</asp:TemplateField>
																										  
												<asp:TemplateField HeaderText="Date" SortExpression="Date" HeaderStyle-ForeColor="#555555"  ItemStyle-CssClass="manageLeft" HeaderStyle-CssClass="manageLeft" HeaderStyle-Width ="70px" >
												   
													<ItemTemplate>
														<asp:Label ID="lbMDate" runat="server" Text='<%# Bind("Date" ,"{0:MM/dd/yy}") %>'></asp:Label>
													</ItemTemplate>
												</asp:TemplateField>
												
												<asp:TemplateField HeaderText="Type"   SortExpression="TypeName" HeaderStyle-ForeColor="#555555" ItemStyle-CssClass="manageLeft" HeaderStyle-CssClass="manageLeft" HeaderStyle-Width ="145px">
												   
													<ItemTemplate>
														<asp:Label ID="lbMType" runat="server" Text='<%# Bind("TypeName") %>'></asp:Label>
													</ItemTemplate>
												</asp:TemplateField>
												
												<asp:TemplateField HeaderText="Actions" HeaderStyle-ForeColor="#555555" ItemStyle-CssClass="manageLeft" HeaderStyle-CssClass="manageLeft" HeaderStyle-Width ="200px">
													
													<ItemTemplate>
														<asp:DropDownList ID="ddlMActions" runat="server" Autopostback="true" Width="130px" 
																AppendDataBoundItems="True" onselectedindexchanged="ddlMActions_SelectedIndexChanged">
															
														</asp:DropDownList>
														<asp:LinkButton ID="btMGo" runat="server" CommandName="Edit" 
																  CommandArgument='<%# Bind("ID")%>' Visible="false" ToolTip='<%# Bind("ID")%>' >GO</asp:LinkButton> 
																										 
													</ItemTemplate>
												</asp:TemplateField>
												
												<asp:TemplateField Visible="false">
													<ItemTemplate>
														<asp:Label ID="lbMId" runat="server" Text='<%# Bind("ID")%>' ></asp:Label>
													</ItemTemplate>
												</asp:TemplateField>
												
												<asp:TemplateField Visible="false">
													
													<ItemTemplate>
														<asp:Label ID="lbMshortName" runat="server" Text='<%# Bind("shortname") %>'></asp:Label>
													</ItemTemplate>
												</asp:TemplateField>
											   
											   <asp:TemplateField Visible="false">
													<ItemTemplate>
														<asp:Label ID="lbMsummary" runat="server" Text='<%# Bind("summary") %>'></asp:Label>
													</ItemTemplate>
												</asp:TemplateField>
												
												<asp:TemplateField Visible="false">
													<ItemTemplate>
														<asp:Label ID="lblMContentId" runat="server" Text='<% #Bind("contentId") %>'></asp:Label>
													</ItemTemplate>
												</asp:TemplateField>
												
												
																				
											</Columns>
											<RowStyle Height="26px" />
											<SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333"  Height="30px"/>
											<HeaderStyle BackColor="#DDDDDD" Font-Bold="True" ForeColor="White" Height="30px"/>
											<EditRowStyle BackColor="#2461BF" Height="30px"/>
											<AlternatingRowStyle BackColor="#EEEEEE"  Height="30px" />
										</asp:GridView>
										<%--</asp:Panel>--%>
									</div>
								</div>
							</div>
							<div id="processing">
                                <div class="spindiv">
                                    <div id="content2">
                                        <p id="loadingspinner">
                                        </p>
                                    </div>
                                </div>
                            </div>
						</ContentTemplate>             
					</asp:UpdatePanel>
                    </td>
                </tr>
				
				 <%--TS- Comment On Dated 23-Aug- 2011--%>
                
                <tr visible="false">                
                   <td style="text-align:center;">
                     <div id="OneClickHide" runat="server" visible="false">
                   <br />
                      <p class="title_red_no_bottom_margin" style="text-align:left;">Bongarde's One-Click Meetings
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <span style="font-size:12px; text-transform:none; text-align:right;">
                        <asp:LinkButton ID="lnkShowAll" runat="server" Text="ShowAll"></asp:LinkButton>
                        </span></p> 
                        <div id="Div3" style="height:170px; overflow:hidden;border-bottom :solid 1px  #999999;" runat="server">
                        <div id="divOneClick" style="">
                       </div>
                       <div  style="height: 155px; width:676; overflow-y:scroll;">
                       
                      </div> 
                      </div> 
                      
                      </div>
                   </td>
                </tr>
				
            </table>          		

   <div id ="popupContact1" style ="width:550px;height :auto ;">
                   <h1>Details <a id="popupContactClose1" href ="javascript:void(0)" style="padding-left: 460px">x</a></h1>
                   <iframe id ="iframe1" runat ="server" frameborder ="0" visible="true" scrolling ="no" width ="550"></iframe>
          
          </div>
          
          <cc1:ModalPopupExtender ID ="modalCOnfirmation" runat="server" TargetControlID="modaltst" PopupControlID="pnlConfirmation" BehaviorID="ConfirmationPanel"></cc1:ModalPopupExtender>
          <asp:LinkButton ID ="modaltst" runat="server" ></asp:LinkButton>
		  <asp:HiddenField ID ="hdnSelectedMeetingId" runat="server" />
		  <asp:TextBox ID="hdTraining" runat="server" style="display:none" />
		  <asp:TextBox ID="hdTraining2" runat="server" style="display:none" />
		  <asp:TextBox ID="hdTrainingRowID" runat="server" style="display:none"/>
           <asp:Panel ID ="pnlConfirmation" runat="server" CssClass="confirmationPopUP" >
            <table  cellpadding="2" >
            <tr>
                <td colspan="2">
                    <h1>Confirm Deletion</h1>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                   <p><asp:Literal ID ="ltrlMessage" runat="server" Text="Currently no Active Session is associated with this meeting.Are you sure you want to do delete?"></asp:Literal></p>
                </td>
            </tr>
            <tr>
                <td style="width:50%;"><b><asp:LinkButton ID ="lnkBtnYes" runat="server" style="padding-left:200px;" >Yes</asp:LinkButton></b> </td>
                <td style="margin-right:100px;width:50%;"><b> <asp:LinkButton ID ="lnkBtnNo" runat="server" OnClientClick="return CloseCOnfirmationPanel();" >No</asp:LinkButton></b></td>
            </tr>
            </table>
          </asp:Panel>
          <asp:LinkButton ID ="LinkButton3" runat="server" ></asp:LinkButton>
		  <cc1:ModalPopupExtender ID ="ModalPopupExtender1" runat="server" TargetControlID="LinkButton3" PopupControlID="pnlConfirmationtoDelete" BehaviorID="confirmationDeletePanel"></cc1:ModalPopupExtender>
          <asp:Panel ID ="pnlConfirmationtoDelete" runat="server" CssClass="confirmationPopUP" >
            <table  cellpadding="2" >
            <tr>
                <td colspan="2">
                    <h1>Confirm Deletion</h1>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                   <b><asp:Label ID ="lblDeletionMessage" runat="server" Text=""></asp:Label></b>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:RadioButton ID ="radoBtn1" runat="server" Text="Do not adjust any related scheduled activities" GroupName="Option" TextAlign="Right" Checked="true" />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                <asp:RadioButton ID ="radoBtn2" runat="server" Text="Cancel all scheduled activities associated with this template" GroupName="Option" TextAlign="Right" />
                </td>
            </tr>
            <tr>
                <td style="width:50%;"><b><asp:LinkButton ID ="lnkBtnCOnfirmationDeleteYes" runat="server" style="padding-left:200px;" >OK</asp:LinkButton></b> </td>
                <td style="margin-right:100px;width:50%;"><b> <asp:LinkButton ID ="lnkbtnConfirmationNo" runat="server" OnClientClick="return CloseCOnfirmationPanel();" >Cancel</asp:LinkButton></b></td>
            </tr>
            </table>
          </asp:Panel>

    </asp:Content>