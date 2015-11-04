Imports System.Data.SqlClient
Imports System.Data
Imports System.Net
Imports System.IO
Imports System.Reflection
Public Class CourseManager
    Inherits System.Web.UI.Page

    Dim CourseDAO As New CourseDAO
    Dim dv As New Data.DataView
    Dim cmpID As String
    Dim ds As New DataSet
    Dim oneClick As Boolean = True
    Dim Sdate As String = String.Empty
    Dim Edate As String = String.Empty
    'Ts-Added by Mohit For Table Header
    'Adjusted by JD Feb 28 to catch when there is no header generated for GV, which I guess happens when there are no rows in the GV
    Protected Sub gvActive_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvActive.PreRender
        'gvActive.UseAccessibleHeader = True
        'If Not gvActive.HeaderRow Is Nothing Then
        'gvActive.HeaderRow.TableSection = TableRowSection.TableHeader
        'End If
    End Sub

    Protected Sub gvMaster_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvMaster.PreRender
        'gvMaster.UseAccessibleHeader = True
        'If Not gvMaster.HeaderRow Is Nothing Then
        'gvMaster.HeaderRow.TableSection = TableRowSection.TableHeader
        'End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim lbMaster As New Label

        If Session("User") IsNot Nothing Then
            cmpID = Session("company").companyID
            If Session("CompanyRole").roleID = 30 Then
                Response.Redirect("../Learner.aspx")
            End If
        Else
            Response.Redirect("../login.aspx?message=error")
        End If

        'TS - Added for Sub Menu Control. Megha
        lbMaster = Master.FindControl("lnkTraining")
        If Not lbMaster Is Nothing Then
            lbMaster.Font.Bold = True
            'lbMaster.Font.Underline = True
        End If

        If (Session("Company").CompanyID > 503 And Session("Company").CompanyID < 512) Or _
        (Session("Company").CompanyID > 3679 And Session("Company").CompanyID < 3688) Or _
        Session("Company").CompanyID = 203 Or _
        Session("Company").CompanyID = 201 Or _
        Session("Company").CompanyID = 2800 Or _
        Session("Company").CompanyID = 364 Or _
        CType(Session("RefUrl"), String) = "https://www.libertymutualsafetynet.com/omapps/ContentServer" Or _
        CType(Session("RefUrl"), String) = "https://www.summitholdings.com" Or _
        CType(Session("RefUrl"), String) = "http://www.americafirst-ins.com/omapps/ContentServer" Or _
        CType(Session("RefUrl"), String) = "http://www.coloradocasualty-ins.com/omapps/ContentServer" Or _
        CType(Session("RefUrl"), String) = "http://www.indiana-ins.com/omapps/ContentServer" Or _
        CType(Session("RefUrl"), String) = "http://www.goldeneagle-ins.com/omapps/ContentServer" Or _
        CType(Session("RefUrl"), String) = "http://www.libertynorthwest-ins.com/omapps/ContentServer" Or _
        CType(Session("RefUrl"), String) = "http://www.montgomery-ins.com/omapps/ContentServer" Or _
        CType(Session("RefUrl"), String) = "http://www.ohiocasualty-ins.com/omapps/ContentServer" Or _
        CType(Session("RefUrl"), String) = "http://www.libertymutualgroup.com/business-insurance" Or _
        CType(Session("RefUrl"), String) = "http://www.peerless-ins.com/omapps/ContentServer" Then
            divLearningBtn.Visible = False
        Else
            divLearningBtn.Visible = True
        End If

        Try
            If Not IsPostBack Then

                If Not Session("ddlTimeInterval") = Nothing Then
                    ddlTimeInterval.SelectedValue = Convert.ToInt32(Session("ddlTimeInterval"))
                    If Not Session("CustomDates") Is Nothing Then
                        Dim list As List(Of String) = Session("CustomDates")
                        Sdate = list.Item(0)
                        Edate = list.Item(1)
                        txtStartDate.Text = Sdate
                        txtEndDate.Text = Edate
                        divCustomDate.Style("display") = "block"
                    End If
                End If

                Dim tmpList As List(Of CourseManage) = New List(Of CourseManage)
                Edate = Date.Now.ToString("MM/dd/yy")
                tmpList = getCourse("", Edate)
                If tmpList.Count = 0 Then
                    Session("dvActiveData") = Nothing
                Else
                    Session("dvActiveData") = tmpList
                End If




                'tmpList.Sort(Addressof SortByDueDate)

                'gvActive.DataSource = tmpList
                'gvActive.DataBind()
                'Ts date : 4-april-2014 add sorting to the gvactive grid
                If Session("dvActiveData") Is Nothing Then
                    If tmpList.Count > 0 Then
                        gvActive.DataSource = tmpList
                        gvActive.DataBind()
                        'For sorting 
                        Session("dvActiveData") = tmpList
                    Else
                        gvActive.DataSource = Nothing
                        gvActive.DataBind()

                    End If
                    ViewState("sortActive") = "ASC"
                Else
                    Dim tmpList1 As List(Of CourseManage) = New List(Of CourseManage)
                    tmpList1 = CType(Session("dvActiveData"), List(Of CourseManage))

                    If Not tmpList1 Is Nothing Then
                        gvActive.DataSource = tmpList1
                        gvActive.DataBind()
                    Else
                        gvActive.DataSource = Nothing
                        gvActive.DataBind()

                    End If

                End If
                'End sorting 


                If Session("dvMasterData") Is Nothing Then
                    Dim dv As DataView = Nothing
                    dv = getmasterCourse(oneClick)
                    If Not dv Is Nothing Then
                        gvMaster.DataSource = dv
                        gvMaster.DataBind()
                        'JD - Added Jun 17 2013 to support sorting
                        Session("dvMasterData") = dv.Table
                        'Ts-Added By Mohit Date 04 May 2011
                    Else
                        gvMaster.DataSource = Nothing
                        gvMaster.DataBind()
                    End If
                    ViewState("sortMaster") = "ASC"
                Else
                    Dim dt As New DataTable
                    dt = Session("dvMasterData")
                    If Not dt Is Nothing Then
                        gvMaster.DataSource = dt
                        gvMaster.DataBind()
                    Else
                        gvMaster.DataSource = Nothing
                        gvMaster.DataBind()
                    End If
                End If
            End If

            If Session("sMsg") IsNot Nothing Then
                If Session("sMsg") <> "" Then
                    msgLabel.Text = Session("sMsg")
                    Session("sMsg") = ""
                Else
                    msgLabel.Text = ""
                End If
            End If

            If gvActive.Rows.Count = 4 Then
                ' PanelS.Height = 100
            ElseIf gvActive.Rows.Count = 3 Then
                'PanelS.Height = 80
            ElseIf gvActive.Rows.Count = 2 Then
                'PanelS.Height = 60
            ElseIf gvActive.Rows.Count = 1 Then
                'PanelS.Height = 40
            Else
                'PanelS.ScrollBars = ScrollBars.Vertical
                'PanelS.Height = 150
            End If

            If gvMaster.Rows.Count = 4 Then
                'PanelMaster.Height = 100
            ElseIf gvMaster.Rows.Count = 3 Then
                'PanelMaster.Height = 80
            ElseIf gvMaster.Rows.Count = 2 Then
                'PanelMaster.Height = 60
            ElseIf gvMaster.Rows.Count = 1 Then
                'PanelMaster.Height = 40
            Else
                ' PanelMaster.ScrollBars = ScrollBars.Vertical
                'PanelMaster.Height = 150
            End If

            If ConfigurationManager.AppSettings("OneClickTemplateOwner").ToString = Session("user").userId.ToString Then
                chkOneClick.Visible = True
                lnkOnlyOne.Visible = True
                spanOneClick.Style.Add("display", "inline")
            End If

            Dim lb As Label = CType(Master.FindControl("lnkTraining"), Label)
            lb.Font.Underline = True
        Catch ex As Exception

        End Try
    End Sub

    Protected Function getCourse1() As List(Of CourseManage)
        Dim lstReturn As New List(Of CourseManage)
        Dim ds As New Data.DataSet
        Sdate = DateAndTime.Now.AddYears(-1).ToString("MM/dd/yy")
        Edate = Date.Now.ToString("MM/dd/yy")
        ds = CourseDAO.getActiveCourse(Session("company").companyID, Sdate, Edate)

        For Each row As Data.DataRow In ds.Tables(0).Rows
            If Not isdbnull(row(5)) Then
                'Ts-Added By Mohit for Course Completion  Filter
                Dim tmpCourseId As Integer = 0
                Dim tmpDueDateId As Integer = 0
                If Not IsDBNull(row) Then
                    tmpCourseId = CInt(row(0).ToString())
                    tmpDueDateId = CInt(row(9).ToString())

                End If
                If CourseDAO.GetCompletedCourseInfoForCourseManager(tmpCourseId, tmpDueDateId, CInt(Session("company").companyID)) Then
                    'If True Don't Include Those Courses
                Else

                    Dim obj As New CourseManage
                    obj.CourseID = row(0).ToString()
                    obj.Name = row(1).ToString()
                    obj.CategoryID = row(2).ToString()
                    obj.Type = row(3).ToString()
                    obj.DateD = DateTime.Parse(row(5).ToString()) 'Convert.ToDateTime(row(5).ToString())
                    obj.SDate = row(4).ToString()
                    obj.Enrollees = row(6).ToString()
                    obj.FullName = row(8).ToString()
                    obj.DueDateIdForElearning = CInt(row(9).ToString())
                    If obj.Type = "Training Program" Then
                        obj.Type = "Program"
                    End If

                    If obj.Type = "Safety Meeting" Then
                        obj.Type = "Meeting"
                    End If

                    If obj.CategoryID = "2" Then
                        If DateTime.Now < obj.DateD Then
                            obj.Status = "Scheduled"
                        ElseIf DateTime.Now > obj.DateD Then
                            'Changed By Mohit Date 8 March 2011
                            'Previously It was Completed
                            obj.Status = "Assigned"
                        End If
                    ElseIf obj.CategoryID = "1" Then
                        If obj.Enrollees = "0" Or obj.Enrollees = "" Then
                            obj.Status = "Scheduled"
                        Else
                            obj.Status = "Assigned"
                        End If
                    ElseIf obj.CategoryID = "3" Then
                        If obj.Enrollees = "0" Or obj.Enrollees = "" Then
                            obj.Status = "Scheduled"
                        Else
                            obj.Status = "Assigned"
                        End If
                    End If

                    obj.Completions = row(7).ToString()
                    'Ts-Added By Mohit
                    'If Not IsDBNull(row(8)) Then
                    'obj.AssignerId = CInt(row(8).ToString())
                    'End If

                    lstReturn.Add(obj)
                End If
            End If
        Next

        Dim tempMeetingTable As DataTable = New DataTable()
        tempMeetingTable = CourseDAO.GetActiveMeetingSessions(CInt(Session("company").companyID), Sdate, Edate)
        If tempMeetingTable.Rows.Count > 0 Then
            For Each row As Data.DataRow In tempMeetingTable.Rows
                Dim obj As New CourseManage
                obj.CourseID = row("sessionId").ToString()
                If row("meetingname").ToString().Length > 16 Then
                    obj.Name = row("meetingname").ToString().Substring(0, 16) + "..."
                Else
                    obj.Name = row("meetingname").ToString()
                End If
                obj.CategoryID = "2"
                obj.Type = "Meeting"
                obj.DateD = DateTime.Parse(row("startdate").ToString())
                obj.SDate = row("startdate").ToString()
                obj.Enrollees = row("assignuser").ToString()
                obj.Completions = row("attended").ToString()
                obj.FullName = row("meetingname").ToString()
                'If DateTime.Now < obj.DateD Then
                'obj.Status = "Scheduled"
                'ElseIf DateTime.Now > obj.DateD Then
                'obj.Status = "Completed"
                'End If
                'Ts Added By Mohit Date 24 Feb 2011
                If SafetyMeetingDAO.IsSessionAttendanceExist(CInt(row("sessionId").ToString())) Then
                    obj.Status = "Completed"
                Else
                    obj.Status = "Scheduled"
                End If

                lstReturn.Add(obj)
            Next

        End If
        Return lstReturn

    End Function
    'Sorting Active Course List 
    Private Shared Function SortByDueDate(ByVal objtmp As CourseManage, ByVal objtmp1 As CourseManage) As Integer
        Return (objtmp.DateD).CompareTo(objtmp1.DateD)
    End Function


    Protected Sub ddlAActions_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim ddl As DropDownList = CType(sender, DropDownList)
            For Each row As GridViewRow In gvActive.Rows
                Dim action As DropDownList = CType(row.FindControl("ddlAActions"), DropDownList)

                If ddl.ClientID = action.ClientID Then
                    Dim lnk As LinkButton = CType(row.FindControl("btGo"), LinkButton)
                    Dim type As Image = row.FindControl("lbAType")
                    Dim courseId As Label = row.FindControl("lbCourseId")
                    Dim lblDate As Label = row.FindControl("lbADate")

                    If action.SelectedValue = "Attendance" Then
                        Response.Redirect("../act/attendance.aspx?id=" & courseId.Text & "&type=" & type.ToolTip.ToString()) 'changed BY Mohit 14 Jan 
                        'ElseIf action.SelectedValue = "Edit Program" Then
                        'Response.Redirect(url + "?id=" & courseId.Text)
                    ElseIf action.SelectedValue = "Program Details" Then
                        Response.Redirect("../Admin/courseDetails.aspx?id=" & courseId.Text)
                    ElseIf action.SelectedValue = "Meeting Details" Then
                        Response.Redirect("../Admin/courseDetails.aspx?id=" & courseId.Text & "&type=" & type.ToolTip.ToString())
                    ElseIf action.SelectedValue = "Cancel" Then
                        'CourseDAO.CancelMeeting(courseId.Text, Session("user").userid)
                        'Session("sMsg") = "Meeting Cancelled successfully"
                        'Response.Redirect("courseManager.aspx")
                        Dim objSafetyMeetingDao As SafetyMeetingDAO = New SafetyMeetingDAO()
                        Dim sessionId As Integer = CInt(courseId.Text.ToString())
                        If objSafetyMeetingDao.IsSessionCreator(sessionId, CInt(Session("user").userId)) Then
                            objSafetyMeetingDao.DeleteSession(sessionId)
                            'Ts change by Mohit 07 Feb 2011
                            UpdateActiveSection()
                        Else
                            'Error Msg That User Is not Creator
                        End If
                    ElseIf action.SelectedValue = "Enroll/Unenroll" Then
                        'Ts-Changed By Mohit Date 2 march 
                        'Ts-CHanged BY Mohit Date 17 March 2011
                        If Not type Is Nothing Then
                            If type.ToolTip.ToString().ToLower().Contains("meeting") Then
                                Dim strRedirectionUrl As String = String.Format("../Act/Unenroll.aspx?sessionid={0}&type=meeting", courseId.Text)
                                Response.Redirect(strRedirectionUrl)
                            Else
                                Dim courseDueDateId As Integer = 0
                                If Not lblDate Is Nothing Then
                                    courseDueDateId = CInt(lblDate.ToolTip().ToString())
                                    If courseDueDateId > 0 Then
                                        Dim strRedirectionUrl As String = String.Format("../Act/Unenroll.aspx?id={0}&type={1}&dueDate={2}", courseId.Text, type.ToolTip.ToString(), courseDueDateId)
                                        Response.Redirect(strRedirectionUrl)
                                    End If
                                End If
                            End If
                        End If

                    ElseIf action.SelectedValue = "Schedule Another" Then
                        Dim objsafetyMeetingDao As SafetyMeetingDAO = New SafetyMeetingDAO()
                        Dim meetingId As Integer = objsafetyMeetingDao.GetMeetingId(CInt(courseId.Text.ToString()))
                        If meetingId > 0 Then
                            Response.Redirect(String.Format("../Meetings/sendInvitations.aspx?meetingId={0}&sta={1}", meetingId, "true"))
                        End If
                    ElseIf action.SelectedValue.ToString().Contains("Copy") Then
                        Dim objsafetyMeetingDao As SafetyMeetingDAO = New SafetyMeetingDAO()
                        Dim meetingId As Integer = objsafetyMeetingDao.GetMeetingId(CInt(courseId.Text.ToString()))
                        Dim sessionId As Integer = objsafetyMeetingDao.CopySession(CInt(courseId.Text.ToString()))
                        If sessionId > 0 Then
                            'After Successful Completion where to redirect
                            'Dim strRedirectionString As String = String.Format("../Meetings/confirmMeeting.aspx?sessionId={0}&meetingId={1}", sessionId, meetingId)
                            Dim strRedirectionString As String = String.Format("../Meetings/sendInvitations.aspx?meetingId={0}", meetingId)
                            'Ts-Added By Mohit Redirecting on Successfull Copying of Meeting Session
                            Response.Redirect(strRedirectionString)
                        Else
                            'Where to Redirect From Here
                        End If
                    ElseIf action.SelectedValue = "Session Details" Then
                        Dim objsafetyMeetingDao As SafetyMeetingDAO = New SafetyMeetingDAO()
                        Dim meetingId As Integer = objsafetyMeetingDao.GetMeetingId(CInt(courseId.Text.ToString()))
                        Dim sessionId As Integer = (CInt(courseId.Text.ToString()))
                        If sessionId > 0 Then
                            'After Successful Completion where to redirect
                            Dim strRedirectionString As String = String.Format("../Meetings/MeetingDescription.aspx?sessionId={0}&meetingId={1}", sessionId, meetingId)
                            Response.Redirect(strRedirectionString)
                        Else
                            'Where to Redirect From Here
                        End If
                        'Ts-added by Mohit Date 16 Feb 2011
                    ElseIf action.SelectedValue = "View Syllabus" Then
                        Dim objsafetyMeetingDao As SafetyMeetingDAO = New SafetyMeetingDAO()
                        Dim meetingId As Integer = objsafetyMeetingDao.GetMeetingId(CInt(courseId.Text.ToString()))
                        Dim sessionId As Integer = (CInt(courseId.Text.ToString()))
                        If sessionId > 0 Then
                            'After Successful Completion where to redirect
                            Dim strRedirectionString As String = String.Format("../Meetings/ViewMeeting.aspx?meetingId={0}&sessionID={1}", meetingId, sessionId)
                            Response.Redirect(strRedirectionString)
                        Else
                            'Where to Redirect From Here
                        End If
                        'Ts-Added By MOhit Date 8 March For Marking Course Complete
                    ElseIf action.SelectedValue = "Complete" Then
                        Dim selectedCourseId As Integer = (CInt(courseId.Text.ToString()))
                        Dim courseDueDateId As Integer = 0
                        If Not lblDate Is Nothing Then
                            courseDueDateId = CInt(lblDate.ToolTip().ToString())
                            If courseDueDateId > 0 Then
                                Dim result As Integer = CourseDAO.MarkCourseAsComplete(selectedCourseId, courseDueDateId, CInt(Session("user").userid))
                                UpdateActiveSection()
                            End If
                        End If

                    ElseIf action.SelectedValue = "Print Attendance Sheet" Then
                        'Dim sessionId As Integer = (CInt(courseId.Text.ToString()))
                        Dim strRedirectionString As String = String.Format("../Meetings/meetingAttendance.aspx?sessionID={0}", courseId.Text)
                        Response.Redirect(strRedirectionString)
                    End If


                End If
            Next
        Catch ex As Exception
            'lblMsg.Text = ex.Message
        End Try

    End Sub

    Protected Function getmasterCourse(ByVal oneClick As Boolean) As DataView
        Dim strSQL As String = ""
        Dim dbUtil As New DbUtil
        Dim ds As New Data.DataSet
        ds = Nothing
        Dim dv As DataView = Nothing
        ds = CourseDAO.getMasterCourses(cmpID, oneClick)
        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                If ViewState("sortExpr") <> "" Then
                    dv = ds.Tables(0).DefaultView
                    dv.Sort = ViewState("sortExpr").ToString()
                Else
                    dv = ds.Tables(0).DefaultView
                End If
            End If
        End If
        Return dv
    End Function

    Protected Sub gvActive_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvActive.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim obj As New CourseManage
            obj = e.Row.DataItem
            Dim ddlAction As DropDownList = e.Row.FindControl("ddlAActions")
            ddlAction.Attributes.Add("onchange", "return Confirm(" + ddlAction.ClientID + ")")
            Dim aCourse As HtmlAnchor = e.Row.FindControl("aCourse")
            Dim courseId As Label = e.Row.FindControl("lbCourseId")


            If obj.Type = "Meeting" Then
                aCourse.Attributes.Add("OnClick", "javascript:OpenPopUpWindow('../courseDetailsPopUpNew.aspx?id=" & courseId.Text & "&contenttype=Meeting Individual','','width=700,height=300,scrollbars=yes');")
                'Changed BY Mohit Date 14 Jan 2011

                Dim type As Image = CType(e.Row.FindControl("lbAType"), Image)
                type.ImageUrl = "../images/TrainingProgram/meeting.gif"
                If obj.Status = "Scheduled" Then

                    If obj.Assigner = True Then
                        ddlAction.Items.Add("Select")
                        ddlAction.Items.Add("Attendance")
                        ddlAction.Items.Add("Enroll/Unenroll")
                        ddlAction.Items.Add("Schedule Another")
                        ddlAction.Items.Add("Session Details")
                        ddlAction.Items.Add("Cancel")
                        ddlAction.Items.Add("Copy To New")
                        'Ts-Added by Mohit Date 16 Feb 2011
                        ddlAction.Items.Add("View Syllabus")
                        ddlAction.Items.Add("Print Attendance Sheet")
                    Else
                        ddlAction.Items.Add("Select")
                        ddlAction.Items.Add("Schedule Another")
                        ddlAction.Items.Add("Session Details")
                        ddlAction.Items.Add("Copy To New")
                        'Ts-Added by Mohit Date 16 Feb 2011
                        ddlAction.Items.Add("View Syllabus")
                        ddlAction.Items.Add("Print Attendance Sheet")
                    End If
                ElseIf obj.Status = "Completed" Then
                    'ddlAction.Items.Add("Set Attendee Completions")

                    If obj.Assigner = True Then
                        ddlAction.Items.Add("Select")
                        ddlAction.Items.Add("Session Details")
                        ddlAction.Items.Add("Enroll/Unenroll")
                        ddlAction.Items.Add("Attendance")
                        ddlAction.Items.Add("Cancel")
                        'Ts-Added by Mohit Date 16 Feb 2011
                        ddlAction.Items.Add("View Syllabus")
                        ddlAction.Items.Add("Print Attendance Sheet")
                    Else
                        ddlAction.Items.Add("Select")
                        ddlAction.Items.Add("Session Details")
                        'Ts-Added by Mohit Date 16 Feb 2011
                        ddlAction.Items.Add("View Syllabus")
                        ddlAction.Items.Add("Print Attendance Sheet")
                    End If
                End If

            ElseIf obj.Type = "eLearning" Then
                'Changed BY Mohit 14 Jan 2011
                Dim type As Image = CType(e.Row.FindControl("lbAType"), Image)
                type.ImageUrl = "../images/TrainingProgram/eLearning.gif"
                aCourse.Attributes.Add("OnClick", "javascript:OpenPopUpWindow('../courseDetailsPopUpNew.aspx?id=" & courseId.Text & "','','width=700,height=300,scrollbars=yes');")
                'Ts-Changed By Mohit Date 23 Feb 2011
                'JD - Jul 17 2013 - Fix - Permitting admins to unenroll for any activity
                If obj.Assigner = True Or Session("CompanyRole").roleID = 10 Then

                    'Ts-Added BY Mohit Date 26 April for New Requirement of Hiding enroll and Unenroll Option
                    'Dim dueDate As String = CourseDAO.GetDueDateForGivenCourse(obj.DueDateIdForElearning, CInt(Session("Company").companyId))
                    '               Dim courseDueDate As Date
                    '               DateTime.TryParse(dueDate, courseDueDate)
                    ddlAction.Items.Add("Select")

                    'TS-Changed by Sanjay on 26-07-2011
                    ddlAction.Items.Add("Enroll/Unenroll")
                    'TS- Megha Course Details removed dated 8th August 2011
                    'ddlAction.Items.Add("Course Details")
                Else
                    ddlAction.Items.Add("Select")
                    'TS- Megha Course Details removed dated 8th August 2011
                    ddlAction.Enabled = False
                    'ddlAction.Items.Add("Course Details")
                End If

            ElseIf obj.Type = "Program" Then
                'Changed BY Mohit 14 jan 2011
                Dim type As Image = CType(e.Row.FindControl("lbAType"), Image)
                type.ImageUrl = "../images/TrainingProgram/program.gif"
                aCourse.Attributes.Add("OnClick", "javascript:OpenPopUpWindow('../courseDetailsPopUpNew.aspx?id=" & courseId.Text & "','','width=700,height=300,scrollbars=yes');")
                ddlAction.Items.Add("Select")
                ddlAction.Items.Add("Attendance")
                ddlAction.Items.Add("Enroll/Unenroll")
                'ddlAction.Items.Add("Edit Program")
                ddlAction.Items.Add("Cancel")
                ddlAction.Items.Add("Program Details")
            End If
        End If
    End Sub

    Protected Sub gvMaster_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMaster.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ddlAction As DropDownList = e.Row.FindControl("ddlMActions")
            ddlAction.Attributes.Add("onchange", "return Confirm2(" + ddlAction.ClientID + ")")
            Dim aCourse As HtmlAnchor = e.Row.FindControl("aMCourse")
            Dim courseId As Label = e.Row.FindControl("lbMId")
            Dim type As Label = e.Row.FindControl("lbMType")

            If type.Text = "Training Program" Then
                type.Text = "Safety Program"
            End If


            If e.Row.DataItem.Row.ItemArray(4).ToString().Contains("Meeting-Draft") Then
                aCourse.Attributes.Add("OnClick", "javascript:OpenPopUpWindow('../courseDetailsPopUpNew.aspx?id=" & courseId.Text & "&contenttype=MeetingTemplate','','width=700,height=300,scrollbars=yes');")



                If e.Row.DataItem.Row("IsCreator") = 1 Then
                    ddlAction.Items.Add("Select")
                    ddlAction.Items.Add("Delete")
                    ddlAction.Items.Add("Edit")
                    'ddlAction.Items.Add("View")
                    'ddlAction.Items.Add("Copy To New")
                    'ddlAction.Items.Add("Schedule")

                    'Ts-Added By Mohit Feb 11 2011
                    'Ts-For Message According to Meeting Sessions Existence or Not
                    Dim objLnkBtnAction As LinkButton = CType(e.Row.FindControl("btMGo"), LinkButton)
                    If e.Row.DataItem.Row("IsMeetingHasSession") <> "0" Then
                        If Not objLnkBtnAction Is Nothing Then
                            Dim msg As String = String.Empty
                            msg = "There are scheduled activities in progress that use this template. How would you like to proceed?"
                            objLnkBtnAction.Attributes.Add("OnClick", "javascript:return ConfirmationMessageDelete(" & courseId.Text & ",'" + msg + "');")
                        End If
                    Else
                        If Not objLnkBtnAction Is Nothing Then
                            objLnkBtnAction.Attributes.Add("OnClick", "javascript:return ConfirmationMessage(" & courseId.Text & ");")
                        End If
                    End If

                Else
                    ddlAction.Items.Add("Select")
                End If

            ElseIf ((e.Row.DataItem.Row.ItemArray(4).ToString().Contains("Meeting-Live")) Or (e.Row.DataItem.Row.ItemArray(4).ToString().Contains("Meeting-Template"))) And Not e.Row.DataItem.Row.ItemArray(6).ToString().Contains("Bongarde One-Click") Then
                aCourse.Attributes.Add("OnClick", "javascript:OpenPopUpWindow('../courseDetailsPopUpNew.aspx?id=" & courseId.Text & "&contenttype=MeetingTemplate','','width=700,height=300,scrollbars=yes');")



                If e.Row.DataItem.Row("IsCreator") = 1 Then
                    'ddlAction.Items.Add("Select")-Changed by Deepak to remove the Select option twice in DDL

                    'TS- Megha change for ONE CLICK MEETINGS.Checking if meeting is oneClick.
                    Dim objSafetyMeetingDao As New SafetyMeetingDAO
                    Dim searchIn As String = String.Empty
                    searchIn = objSafetyMeetingDao.GetMeetingSearchCriteria(courseId.Text, "")

                    If searchIn <> String.Empty Then
                        ddlAction.Items.Add("Select")
                        ddlAction.Items.Add("Delete")
                        ddlAction.Items.Add("Edit")
                        ddlAction.Items.Add("View")
                        ddlAction.Items.Add("Copy To New")
                        ddlAction.Items.Add("Schedule")
                    End If
                    'Ts-Added By Mohit Feb 11 2011
                    'Ts-For Message According to Meeting Sessions Existence or Not
                    Dim objLnkBtnAction As LinkButton = CType(e.Row.FindControl("btMGo"), LinkButton)
                    If e.Row.DataItem.Row("IsMeetingHasSession") <> "0" Then
                        If Not objLnkBtnAction Is Nothing Then
                            Dim msg As String = String.Empty
                            msg = "There are scheduled activities in progress that use this template. How would you like to proceed?"
                            objLnkBtnAction.Attributes.Add("OnClick", "javascript:return ConfirmationMessageDelete(" & courseId.Text & ",'" + msg + "');")
                        End If
                    Else
                        If Not objLnkBtnAction Is Nothing Then
                            objLnkBtnAction.Attributes.Add("OnClick", "javascript:return ConfirmationMessage(" & courseId.Text & ");")
                        End If
                    End If

                Else
                    'Ts Megha OneClick changes
                    'If ConfigurationManager.AppSettings("OneClickTemplateOwner").ToString = Session("user").userId.ToString Then
                    '    ddlAction.Items.Add("Select")
                    '    ddlAction.Items.Add("View")
                    '    ddlAction.Items.Add("Copy To New")
                    'Else
                    ddlAction.Items.Add("Select")
                    ddlAction.Items.Add("View")
                    ddlAction.Items.Add("Copy To New")
                    ddlAction.Items.Add("Schedule")
                    'End If

                End If
                'ElseIf e.Row.DataItem.Row.ItemArray(4).ToString().Contains("Program-Draft") Then
                'aCourse.Attributes.Add("OnClick", "javascript:OpenPopUpWindow('../courseDetailsPopUpNew.aspx?id=" & courseId.Text & "','','width=700,height=300,scrollbars=yes');")
                'Dim objProgramDao As New TrainingProgramDao()
                'If objProgramDao.CheckProgramCreator(CInt(Session("user").userId), CInt(courseId.Text.ToString())) Then
                ' ddlAction.Items.Add("Select")
                'ddlAction.Items.Add("Delete")
                'ddlAction.Items.Add("Edit")
                'ddlAction.Items.Add("View")
                'ddlAction.Items.Add("Copy")
                'ddlAction.Items.Add("Schedule")
                ' Else
                'ddlAction.Items.Add("Select")
                'End If
            ElseIf e.Row.DataItem.Row.ItemArray(6).ToString().Contains("Bongarde One-Click") Then
                aCourse.Attributes.Add("OnClick", "javascript:OpenPopUpWindow('../courseDetailsPopUpNew.aspx?id=" & courseId.Text & "&contenttype=MeetingTemplate','','width=700,height=500,scrollbars=yes');")
                If ConfigurationManager.AppSettings("OneClickTemplateOwner").ToString = Session("user").userId.ToString Then
                    ddlAction.Items.Add("Select")
                    'ddlAction.Items.Add("View")
                    ddlAction.Items.Add("Copy To New")
                Else
                    ddlAction.Items.Add("Select")
                    ddlAction.Items.Add("View")
                    ddlAction.Items.Add("Copy To New")
                    ddlAction.Items.Add("Schedule")
                End If
            End If
        End If
    End Sub


    Protected Sub ddlMActions_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ID As Integer
        Dim url As String
        Dim meetingId As String
        Dim meetSname As String
        Dim Unow, duration As Long
        Dim status As String
        Dim isMaster As Integer = 0
        Dim Location As String = String.Empty

        Unow = DateDiff(DateInterval.Second, Convert.ToDateTime("01/01/1970"), DateTime.Now)
        duration = 0

        url = ConfigurationManager.AppSettings("CourseUrl").ToString()

        Try
            Dim ddl As DropDownList = CType(sender, DropDownList)
            For Each row As GridViewRow In gvMaster.Rows
                Dim action As DropDownList = CType(row.FindControl("ddlMActions"), DropDownList)

                If ddl.ClientID = action.ClientID Then
                    Dim type As Label = row.FindControl("lbMType")
                    Dim courseId As Label = row.FindControl("lbMId")
                    Dim shortname As Label = row.FindControl("lbMshortName")
                    Dim fullname As Label = row.FindControl("lbMFName")
                    Dim summary As Label = row.FindControl("lbMsummary")
                    Dim lnk As LinkButton = CType(row.FindControl("btMGo"), LinkButton)
                    Dim contentId As Label = row.FindControl("lblMContentId")
                    Dim creator As Label = row.FindControl("lbMSCreator")

                    ID = lnk.CommandArgument
                    meetSname = shortname.Text.ToString + Now.TimeOfDay.TotalSeconds.ToString

                    If CInt(courseId.Text) = ID Then

                        'Response.Write(action.selectedValue)
                        'Response.End
                        If type.Text.ToString().Contains("Program") Then
                            If action.SelectedValue = "Edit" Then
                                Dim strMeetingUrl As String = String.Empty
                                strMeetingUrl = String.Format("../TrainingProgram/TrainingProgramStep1.aspx?prgId={0}", CInt(courseId.Text.ToString()))
                                Response.Redirect(strMeetingUrl)
                            End If
                            If action.SelectedValue = "Delete" Then
                                lnkBtnYes.ToolTip = courseId.Text.ToString()
                                modalCOnfirmation.Show()
                            End If
                            'Ts Megha OneClick changes
                        ElseIf type.Text.ToString().Contains("Meeting") And Not creator.Text.ToString().Contains("Bongarde One-Click") Then
                            If action.SelectedValue = "Edit" Then
                                Dim strMeetingUrl As String = String.Empty
                                strMeetingUrl = String.Format("../Meetings/addMeeting1.aspx?Type=Meetings&meetingId={0}", CInt(courseId.Text.ToString()))
                                Response.Redirect(strMeetingUrl)
                            End If
                            If action.SelectedValue = "View" Then
                                Dim meetingUrl As String = String.Format("../Meetings/ViewMeeting.aspx?meetingId={0}", CInt(courseId.Text.ToString()))
                                Response.Redirect(meetingUrl)
                            End If
                            If action.SelectedValue = "Delete" Then
                                lnkBtnYes.ToolTip = courseId.Text.ToString()
                                modalCOnfirmation.Show()
                            End If
                            If action.SelectedValue = "Copy To New" Then
                                Dim objSafetyMeetingDao As New SafetyMeetingDAO
                                Dim result As Integer = 0
                                result = objSafetyMeetingDao.CopyMeeting(CInt(courseId.Text.ToString()))
                                If result > 0 Then
                                    'What Operation  need to be performed
                                    'Ts-Removed BY Mohit As per New Requirements Date 19 APril 2010
                                    'Response.Redirect(String.Format("../Meetings/MeetingConfirmation.aspx?meetingId={0}", result))
                                    Response.Redirect(String.Format("../Meetings/addMeeting1.aspx?Type=Meetings&meetingId={0}", result))
                                End If
                            End If
                            If action.SelectedValue = "Schedule" Then
                                Dim strMeetingUrl As String = String.Empty
                                strMeetingUrl = String.Format("../Meetings/sendInvitations.aspx?meetingId={0}", CInt(courseId.Text.ToString()))
                                Response.Redirect(strMeetingUrl)
                            End If
                            'ElseIf type.Text.ToString().Contains("One-Click Template") Then
                            '    If ddl.SelectedItem.Text = "View" Then
                            '        Response.Redirect("~/Meetings/OneClickMeeting.aspx?id=" & courseId.Text.ToString() & "&articleId=" & contentId.Text)
                            '    ElseIf ddl.SelectedItem.Text = "Copy To New" Then
                            '        Response.Redirect("~/Meetings/OneClickMeeting.aspx?id=" & courseId.Text.ToString() & "&articleId=" & contentId.Text & "&Type=Copy")
                            '    ElseIf ddl.SelectedItem.Text = "Schedule" Then
                            '        Response.Redirect("~/Meetings/OneClickMeeting.aspx?id=" & courseId.Text.ToString() & "&articleId=" & contentId.Text & "&Type=Schedule")
                            '    End If
                        ElseIf creator.Text.ToString().Contains("Bongarde One-Click") Then
                            If ConfigurationManager.AppSettings("OneClickTemplateOwner").ToString = Session("user").userId.ToString Then
                                If ddl.SelectedItem.Text = "View" Then
                                    Response.Redirect("~/Meetings/OneClickMeeting.aspx?id=" & courseId.Text.ToString() & "&articleId=" & contentId.Text)
                                ElseIf ddl.SelectedItem.Text = "Copy To New" Then
                                    Response.Redirect("~/Meetings/OneClickMeeting.aspx?id=" & courseId.Text.ToString() & "&articleId=" & contentId.Text & "&Type=Copy")
                                ElseIf ddl.SelectedItem.Text = "Schedule" Then
                                    Response.Redirect("~/Meetings/OneClickMeeting.aspx?id=" & courseId.Text.ToString() & "&articleId=" & contentId.Text & "&Type=Schedule")
                                End If
                            Else
                                If ddl.SelectedItem.Text = "Copy To New" Then
                                    Dim objSafetyMeetingDao As New SafetyMeetingDAO
                                    Dim result As Integer = 0
                                    result = objSafetyMeetingDao.CopyTemplateMeeting(CInt(courseId.Text.ToString()))
                                    If result > 0 Then
                                        Response.Redirect(String.Format("../Meetings/addMeeting1.aspx?Type=Meetings&meetingId={0}", result))
                                    End If
                                    'Response.Redirect("~/Meetings/SafetyMeetingReview.aspx?meetingId=" & courseId.Text.ToString())
                                ElseIf ddl.SelectedItem.Text = "View" Then
                                    Dim meetingUrl As String = String.Format("../Meetings/ViewMeeting.aspx?meetingId={0}", CInt(courseId.Text.ToString()))
                                    Response.Redirect(meetingUrl)
                                ElseIf ddl.SelectedItem.Text = "Schedule" Then
                                    Dim newTemplateMeetingId As Integer = 0
                                    Dim objSafetyMeeting As New SafetyMeetingDAO
                                    newTemplateMeetingId = objSafetyMeeting.CopyTemplateMeeting(CInt(courseId.Text))
                                    Response.Redirect(String.Format("../meetings/sendInvitations.aspx?meetingId={0}", newTemplateMeetingId))
                                End If
                            End If
                        End If

                        If action.SelectedValue = "Details" Then
                            Response.Redirect("../Admin/courseDetails.aspx?id=" & courseId.Text)
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            'lblMsg.Text = ex.Message
        End Try

    End Sub


    Function GetCourseId(ByVal strURI As String) As String
        Dim strHTML As String
        Dim CookieJar As New CookieContainer()
        Dim strLoginURL As String
        strHTML = ""

        Try
            Dim Stream As Stream
            Dim Temp As String
            strLoginURL = ConfigurationManager.AppSettings("moodleDirectLogin").ToString() '"http://67.202.16.216:8000/moodle/login/direct.php"
            Dim Request1 As HttpWebRequest = HttpWebRequest.Create(strLoginURL)
            Request1.CookieContainer = CookieJar

            Dim Response1 As HttpWebResponse = Request1.GetResponse()
            Stream = Response1.GetResponseStream

            Temp = New StreamReader(Stream).ReadToEnd()
            Stream.Close()
            Response1.Close()

            'Dim Stream As Stream
            'Dim Temp As String
            Request1 = HttpWebRequest.Create(strURI)
            Request1.CookieContainer = CookieJar

            Response1 = Request1.GetResponse()
            Stream = Response1.GetResponseStream

            Temp = New StreamReader(Stream).ReadToEnd()
            Stream.Close()
            Response1.Close()
            strHTML = Temp

        Catch ex As Exception
        End Try

        GetCourseId = strHTML
    End Function

    Protected Sub DeleteContent(ByRef meetingId As Integer)
        Dim objSafetyMeetingDao As SafetyMeetingDAO = New SafetyMeetingDAO()
        If objSafetyMeetingDao.CheckMeetingCreator(CInt(Session("user").userId), meetingId) Then
            Dim result As Integer = objSafetyMeetingDao.SoftDeleteMeetingAndSessions(meetingId)
            If result > 0 Then
                'What Operation to Do Prompt a message here or Redirection to Other Page Page need to be rebind
            Else
                'Eror
            End If
            'End If
        Else
            'Whether Allowed or Not Business Logic Need to Consult
        End If
    End Sub

    Protected Sub lnkBtnYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkBtnYes.Click
        Try
            Dim objLinkBtn As LinkButton = CType(sender, LinkButton)
            If Not objLinkBtn Is Nothing Then
                'Ts-Mohit CHange of Logic Feb 11,2011
                Dim meetingId As Integer = 0
                If objLinkBtn.ToolTip <> String.Empty Then
                    meetingId = CType(objLinkBtn.ToolTip, Integer)
                ElseIf hdnSelectedMeetingId.Value <> String.Empty Then
                    meetingId = CType(hdnSelectedMeetingId.Value, Integer)
                End If
                DeleteContent(meetingId)
                'TS-Change Incorporated By Mohit 07 Feb 2010
                Dim dv As DataView = Nothing
                dv = getmasterCourse(oneClick)
                If Not dv Is Nothing Then
                    gvMaster.DataSource = dv
                    gvMaster.DataBind()
                    'JD - Added Jun 17 2013 to support sorting
                    Session("dvMasterData") = dv.Table
                Else
                    gvMaster.DataSource = Nothing
                    gvMaster.DataBind()
                End If
                Dim objList As List(Of CourseManage) = New List(Of CourseManage)
                Sdate = DateAndTime.Now.AddYears(-1).ToString("MM/dd/yy")
                Edate = Date.Now.ToString("MM/dd/yy")
                objList = getCourse(Sdate, Edate)
                'objList.Sort(Addressof SortByDueDate)
                If objList.count > 0 Then
                    gvActive.DataSource = objList
                    gvActive.DataBind()
                Else
                    gvActive.DataSource = Nothing
                    gvActive.DataBind()
                End If
                'UpdatePanel1.Update()

            End If
        Catch ex As Exception
            'Response.Write(ex.Message().ToString())
            'Response.End()
        End Try
    End Sub

    ''' <summary>
    ''' Function to Update Active Section
    ''' Created By Mohit
    ''' Date 07 Feb 2011
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub UpdateActiveSection()
        Dim objTempList As List(Of CourseManage) = New List(Of CourseManage)
        Sdate = DateAndTime.Now.AddYears(-1).ToString("MM/dd/yy")
        Edate = Date.Now.ToString("MM/dd/yy")
        objTempList = getCourse(Sdate, Edate)
        'Ts Added by Mohit FOr SOrting After Operation
        'objTempList.Sort(AddressOf SortByDueDate)
        gvActive.DataSource = objTempList
        gvActive.DataBind()
        'UpdatePanel1.Update()
    End Sub

    ''' <summary>
    ''' Function For New Confirmation Box Ok Button Click Event
    ''' Created BY Mohit
    ''' Date 16 Feb 2011
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lnkBtnCOnfirmationDeleteYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkBtnCOnfirmationDeleteYes.Click
        Try

            Dim objLinkBtn As LinkButton = CType(sender, LinkButton)
            If Not objLinkBtn Is Nothing Then
                'Ts-Mohit CHange of Logic Feb 11,2011
                Dim meetingId As Integer = 0
                If objLinkBtn.ToolTip <> String.Empty Then
                    meetingId = CType(objLinkBtn.ToolTip, Integer)
                ElseIf hdnSelectedMeetingId.Value <> String.Empty Then
                    meetingId = CType(hdnSelectedMeetingId.Value, Integer)
                End If
                If radoBtn1.Checked = True Then
                    DeleteMeetingOnly(meetingId)
                Else
                    DeleteContent(meetingId)
                End If

                'TS-Change Incorporated By Mohit 07 Feb 2010
                Dim dv As DataView = Nothing
                dv = getmasterCourse(oneClick)
                If Not dv Is Nothing Then
                    gvMaster.DataSource = dv
                    gvMaster.DataBind()
                    'JD - Added Jun 17 2013 to support sorting
                    Session("dvMasterData") = dv.Table
                Else
                    gvMaster.DataSource = Nothing
                    gvMaster.DataBind()
                End If
                Dim objList As List(Of CourseManage) = New List(Of CourseManage)
                Sdate = DateAndTime.Now.AddYears(-1).ToString("MM/dd/yy")
                Edate = Date.Now.ToString("MM/dd/yy")
                objList = getCourse(Sdate, Edate)
                'objList.Sort(AddressOf SortByDueDate)
                If objList.Count > 0 Then
                    gvActive.DataSource = objList
                    gvActive.DataBind()
                Else
                    gvActive.DataSource = Nothing
                    gvActive.DataBind()
                End If
                'UpdatePanel1.Update()

            End If
        Catch ex As Exception
            'Response.Write(ex.Message().ToString())
            'Response.End()
        End Try
    End Sub
    ''' <summary>
    ''' Function to Soft Delete Meeting Only
    ''' </summary>
    ''' <param name="meetingId"></param>
    ''' <remarks></remarks>
    Protected Sub DeleteMeetingOnly(ByRef meetingId As Integer)
        Dim objSafetyMeetingDao As SafetyMeetingDAO = New SafetyMeetingDAO()
        If objSafetyMeetingDao.CheckMeetingCreator(CInt(Session("user").userId), meetingId) Then
            Dim result As Integer = objSafetyMeetingDao.SoftDeleteMeetingOnly(meetingId)
            If result > 0 Then
                'What Operation to Do Prompt a message here or Redirection to Other Page Page need to be rebind
            Else
                'Eror
            End If
            'End If
        Else
            'Whether Allowed or Not Business Logic Need to Consult
        End If
    End Sub


    Protected Sub chkOneClick_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkOneClick.CheckedChanged
        If chkOneClick.Checked Then
            dv = getmasterCourse(oneClick)
            If Not dv Is Nothing Then
                gvMaster.DataSource = dv
                gvMaster.DataBind()
                'JD - Added Jun 17 2013 to support sorting
                Session("dvMasterData") = dv.Table
            Else
                gvMaster.DataSource = Nothing
                gvMaster.DataBind()
            End If
            'OneClickHide.Visible = True
        Else
            oneClick = False
            'OneClickHide.Visible = False
            dv = getmasterCourse(oneClick)
            If Not dv Is Nothing Then
                gvMaster.DataSource = dv
                gvMaster.DataBind()
                'JD - Added Jun 17 2013 to support sorting
                Session("dvMasterData") = dv.Table
            Else
                gvMaster.DataSource = Nothing
                gvMaster.DataBind()
            End If
        End If

        'If OneClickHide.Visible = True Then
        '    lnkShowAll.Visible = False
        'End If


    End Sub

    Protected Sub lnkOnlyOne_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkOnlyOne.Click

        dv = getOneClickMeeting()
        If Not dv Is Nothing Then
            gvMaster.DataSource = dv
            gvMaster.DataBind()
            'Ts-Added By Mohit Date 04 May 2011
            'JD - Added Jun 17 2013 to support sorting
            Session("dvMasterData") = dv.Table
        Else
            gvMaster.DataSource = Nothing
            gvMaster.DataBind()
        End If

    End Sub

    Protected Function getOneClickMeeting() As DataView
        Dim strSQL As String = ""
        Dim dbUtil As New DbUtil
        Dim ds As New Data.DataSet
        ds = Nothing
        Dim dv As DataView = Nothing

        Dim objsafetyMeetingDao As SafetyMeetingDAO = New SafetyMeetingDAO()
        ds = SafetyMeetingDAO.getOneClickMeetings()

        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                If ViewState("sortExpr") <> "" Then
                    dv = ds.Tables(0).DefaultView
                    dv.Sort = ViewState("sortExpr").ToString()
                Else
                    dv = ds.Tables(0).DefaultView
                End If
            End If
        End If
        Return dv
    End Function

    Protected Sub lnkShowAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkShowAll.Click
        divhide1.Visible = True
        divhide2.Visible = True
        OneClickHide.Visible = True

        If OneClickHide.Visible = True Then
            lnkShowAll.Visible = False
        End If
    End Sub

    Protected Function getCourse(ByVal SDate As String, ByVal EDate As String) As List(Of CourseManage)
        Dim lstReturn As New List(Of CourseManage)
        Dim ds As New Data.DataSet
        ds = CourseDAO.getActiveCourse(Session("company").companyID, SDate, EDate)
        For Each row As Data.DataRow In ds.Tables(0).Rows
            If row(8).ToString <> "" Then
                If Not IsDBNull(row(5)) Then
                    'Ts-Added By Mohit for Course Completion  Filter
                    Dim tmpCourseId As Integer = 0
                    Dim tmpDueDateId As Integer = 0
                    If Not IsDBNull(row) Then
                        tmpCourseId = CInt(row(0).ToString())
                        tmpDueDateId = CInt(row(9).ToString())

                    End If
                    If CourseDAO.GetCompletedCourseInfoForCourseManager(tmpCourseId, tmpDueDateId, CInt(Session("company").companyID)) Then
                        'If True Don't Include Those Courses
                    Else

                        Dim obj As New CourseManage
                        obj.CourseID = row(0).ToString()
                        obj.Name = row(1).ToString()
                        obj.FullName = row(2).ToString()
                        obj.CategoryID = row(3).ToString()
                        obj.Type = row(4).ToString()
                        obj.DateD = DateTime.Parse(row(8).ToString()) 'Convert.ToDateTime(row(5).ToString())
                        obj.SDate = DateTime.Parse(row(7).ToString())
                        obj.Enrollees = Convert.ToInt32(row(5).ToString())

                        obj.DueDateIdForElearning = CInt(row(9).ToString())
                        If obj.Type = "Training Program" Then
                            obj.Type = "Program"
                        End If

                        If obj.Type = "Safety Meeting" Then
                            obj.Type = "Meeting"
                        End If

                        If obj.CategoryID = "2" Then
                            If DateTime.Now < obj.DateD Then
                                obj.Status = "Scheduled"
                            ElseIf DateTime.Now > obj.DateD Then
                                'Changed By Mohit Date 8 March 2011
                                'Previously It was Completed
                                obj.Status = "Assigned"
                            End If
                        ElseIf obj.CategoryID = "1" Then
                            If obj.Enrollees = 0 Then 'Or obj.Enrollees = ""
                                obj.Status = "Scheduled"
                            Else
                                obj.Status = "Assigned"
                            End If
                        ElseIf obj.CategoryID = "3" Then
                            If obj.Enrollees = 0 Then 'Or obj.Enrollees = ""
                                obj.Status = "Scheduled"
                            Else
                                obj.Status = "Assigned"
                            End If
                        End If

                        'Dim dt As Date = row("AssignmentDate")

                        Dim completed = (From o In ds.Tables(1).AsEnumerable() _
                             Where (o(1).ToString = obj.DueDateIdForElearning) _
                            Select o(1)).Count


                        obj.Completions = Convert.ToInt32(completed)
                        obj.Assigner = row("Assigner")

                        'Ts-Added By Mohit
                        'If Not IsDBNull(row(8)) Then
                        'obj.AssignerId = CInt(row(8).ToString())
                        'End If

                        lstReturn.Add(obj)
                    End If
                End If
            End If
        Next

        If SDate = "" Then
            Dim lst = (From o In lstReturn Where o.Enrollees > Convert.ToInt32(o.Completions)).ToList
            lstReturn = lst
        End If


        Dim tempMeetingTable As DataTable = New DataTable()
        tempMeetingTable = CourseDAO.GetActiveMeetingSessions(CInt(Session("company").companyID), SDate, EDate)
        If tempMeetingTable.Rows.Count > 0 Then
            For Each row As Data.DataRow In tempMeetingTable.Rows
                Dim obj As New CourseManage
                obj.CourseID = row("sessionId").ToString()
                If row("meetingname").ToString().Length > 16 Then
                    obj.Name = row("meetingname").ToString().Substring(0, 16) + "..."
                Else
                    obj.Name = row("meetingname").ToString()
                End If
                obj.CategoryID = "2"
                obj.Type = "Meeting"
                obj.DateD = DateTime.Parse(row("startdate").ToString())
                obj.SDate = DateTime.Parse(row("startdate").ToString())
                obj.Enrollees = Convert.ToInt32(row("assignuser").ToString())
                obj.Completions = Convert.ToInt32(row("attended").ToString())
                obj.FullName = row("meetingname").ToString()
                'If DateTime.Now < obj.DateD Then
                'obj.Status = "Scheduled"
                'ElseIf DateTime.Now > obj.DateD Then
                'obj.Status = "Completed"
                'End If
                'Ts Added By Mohit Date 24 Feb 2011
                If SafetyMeetingDAO.IsSessionAttendanceExist(CInt(row("sessionId").ToString())) Then
                    obj.Status = "Completed"
                Else
                    obj.Status = "Scheduled"
                End If

                obj.Assigner = row("Assigner")

                lstReturn.Add(obj)
            Next

        End If
        Return lstReturn

    End Function

    Protected Sub gvMaster_Sort(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        Dim dt As New DataTable
        Dim sSort As String = GetSortDirection(e.SortExpression)
        dt = Session("dvMasterData")
        If Not dt Is Nothing Then
            dt.DefaultView.Sort = e.SortExpression & " " & sSort
            gvMaster.DataSource = dt
            gvMaster.DataBind()
        End If

        ScriptManager.RegisterStartupScript(Me.Page, Me.[GetType](), "tmp", "<script type='text/javascript'>fixHeader();</script>", False)
    End Sub

    Private Function GetSortDirection(ByVal column As String) As String

        ' By default, set the sort direction to ascending.
        Dim sortDirection As String
        If column = "Date" Then
            sortDirection = "DESC"
        Else
            sortDirection = "ASC"
        End If


        ' Retrieve the last column that was sorted.
        Dim sortExpression = TryCast(ViewState("SortExpression"), String)

        If sortExpression IsNot Nothing Then
            ' Check if the same column is being sorted.
            ' Otherwise, the default value can be returned.
            If sortExpression = column Then
                Dim lastDirection = TryCast(ViewState("SortDirection"), String)
                If lastDirection IsNot Nothing _
                  AndAlso lastDirection = "ASC" Then

                    sortDirection = "DESC"
                Else
                    sortDirection = "ASC"

                End If
            End If
        End If

        ' Save new values in ViewState.
        ViewState("SortDirection") = sortDirection
        ViewState("SortExpression") = column

        Return sortDirection

    End Function

    'Ts Date - 5,july 2013 : Design change

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Not Session("company") Is Nothing Then

            'check the user weather user is logged in or not
            'JD - Aug 24 2013 - Updated to include all IDs and session ref's for Liberty Mutual companies and child companies
            If Session("company").companyID = 2553 Or Session("company").companyID = 4216 Or Session("company").companyID = 8385 Or Session("company").companyID = 9346 Or Session("company").companyID = 9807 Or _
   (Session("Company").CompanyID > 503 And Session("Company").CompanyID < 512) Or _
   (Session("Company").CompanyID > 3679 And Session("Company").CompanyID < 3688) Or _
   Session("Company").CompanyID = 203 Or _
   Session("Company").CompanyID = 201 Or _
   Session("Company").CompanyID = 2800 Or _
   Session("Company").CompanyID = 364 Or _
   CType(Session("RefUrl"), String) = "https://www.libertymutualsafetynet.com/omapps/ContentServer" Or _
   CType(Session("RefUrl"), String) = "https://www.summitholdings.com" Or _
   CType(Session("RefUrl"), String) = "http://www.americafirst-ins.com/omapps/ContentServer" Or _
   CType(Session("RefUrl"), String) = "http://www.coloradocasualty-ins.com/omapps/ContentServer" Or _
   CType(Session("RefUrl"), String) = "http://www.indiana-ins.com/omapps/ContentServer" Or _
   CType(Session("RefUrl"), String) = "http://www.goldeneagle-ins.com/omapps/ContentServer" Or _
   CType(Session("RefUrl"), String) = "http://www.libertynorthwest-ins.com/omapps/ContentServer" Or _
   CType(Session("RefUrl"), String) = "http://www.montgomery-ins.com/omapps/ContentServer" Or _
   CType(Session("RefUrl"), String) = "http://www.ohiocasualty-ins.com/omapps/ContentServer" Or _
   CType(Session("RefUrl"), String) = "http://www.libertymutualgroup.com/business-insurance" Or _
   CType(Session("RefUrl"), String) = "http://www.peerless-ins.com/omapps/ContentServer" Then
                Me.Page.MasterPageFile = "~/Master_OLD/BongardeMaster.Master"
            Else
                Me.Page.MasterPageFile = "~/Master/BongardeMaster.Master"
            End If

        End If

    End Sub

    'Ts dated 4-april-2014 : add sorting to the gvactive grid

    Protected Sub gvActive_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvActive.Sorting



        '-----------------
        If ViewState("sortExpr") <> "" Then
            If ViewState("sortExpr") = e.SortExpression Then
                If ViewState("sortExpr").ToString() = "Enrollees" Or ViewState("sortExpr").ToString() = "Completions" Or ViewState("sortExpr").ToString() = "SDate" Then
                    If ViewState("dir") <> "" Then
                        If ViewState("dir") = "A" Then
                            ViewState("dir") = "D"
                        ElseIf ViewState("dir") = "D" Then
                            ViewState("dir") = "A"
                        End If
                    Else
                        ViewState("dir") = "D"
                    End If
                Else
                    If ViewState("dir") <> "" Then
                        If ViewState("dir") = "D" Then
                            ViewState("dir") = "A"
                        ElseIf ViewState("dir") = "A" Then
                            ViewState("dir") = "D"
                        End If
                    Else
                        ViewState("dir") = "D"
                    End If
                End If

            End If
        End If

        If ViewState("sortExpr") <> e.SortExpression Then
            If e.SortExpression = "Enrollees" Or e.SortExpression = "Completions" Or e.SortExpression = "SDate" Then
                ViewState("dir") = "D"
            Else
                ViewState("dir") = "A"
            End If
        End If
        ViewState("sortExpr") = e.SortExpression
        bindSortedgridview()
        ScriptManager.RegisterStartupScript(Me.Page, Me.[GetType](), "tmp", "<script type='text/javascript'>fixHeader1();</script>", False)
    End Sub

    Protected Sub bindSortedgridview()
        Try
            'GridWeb1.WebWorksheets.Clear()
            Dim tmpList As List(Of CourseManage) = New List(Of CourseManage)
            tmpList = CType(Session("dvActiveData"), List(Of CourseManage))
            Dim ds As System.Data.DataSet = New System.Data.DataSet
            Dim _result As New System.Data.DataSet()
            _result.Tables.Add("results")
            _result.Tables("results").Columns.Add("Name")
            _result.Tables("results").Columns.Add("Status")
            _result.Tables("results").Columns.Add("Enrollees", GetType(Integer))

            _result.Tables("results").Columns.Add("Completions")
            _result.Tables("results").Columns.Add("SDate", GetType(DateTime))
            _result.Tables("results").Columns.Add("Type")
            _result.Tables("results").Columns.Add("CourseID")
            _result.Tables("results").Columns.Add("Fullname")
            _result.Tables("results").Columns.Add("DueDateIdForElearning", GetType(Integer))
            _result.Tables("results").Columns.Add("Assigner", GetType(Boolean))




            For Each item As CourseManage In tmpList
                Dim newRow As System.Data.DataRow = _
                    _result.Tables("results").NewRow()
                newRow("Name") = item.Name
                newRow("Status") = item.Status
                newRow("Enrollees") = CInt(item.Enrollees)
                newRow("Completions") = item.Completions
                newRow("SDate") = DateTime.Parse(item.SDate.ToString())
                newRow("Type") = item.Type
                newRow("CourseID") = item.CourseID
                newRow("Fullname") = item.FullName
                newRow("DueDateIdForElearning") = CInt(item.DueDateIdForElearning)
                newRow("Assigner") = item.Assigner
                _result.Tables("results").Rows.Add(newRow)
            Next


            Dim dv As System.Data.DataView


            If ViewState("sortExpr") <> "" Then
                dv = New System.Data.DataView(_result.Tables(0))
                If ViewState("sortExpr").ToString() = "Enrollees" Or ViewState("sortExpr").ToString() = "Completions" Or ViewState("sortExpr").ToString() = "SDate" Then
                    If ViewState("dir") = "A" Then
                        dv.Sort = ViewState("sortExpr").ToString() + " ASC"
                        ViewState("dir") = "A"
                    Else
                        dv.Sort = ViewState("sortExpr").ToString() + " DESC"
                        ViewState("dir") = "D"
                    End If
                Else
                    If ViewState("dir") = "D" Then
                        dv.Sort = ViewState("sortExpr").ToString() + " DESC"
                        ViewState("dir") = "D"
                    Else
                        dv.Sort = ViewState("sortExpr").ToString()
                        ViewState("dir") = "A"
                    End If
                End If

            Else
                dv = _result.Tables(0).DefaultView
            End If

            If dv.Count > 0 Then
                'ViewState("count") = dv.Count
                'Session("FilterListdv") = dv
                'BindIncidents()
                'PagerSetting()
                'dv.ApplyDefaultSort = True
                Dim lst As List(Of CourseManage) = ToCollection(Of CourseManage)(dv.ToTable)
                gvActive.DataSource = lst
                gvActive.DataBind()

            Else
                gvActive.DataSource = Nothing
                gvActive.DataBind()

            End If


            '_result.Tables(0).Columns(0).ColumnName = "Employee"
            '_result.Tables(0).Columns(1).ColumnName = "ReportedOn"
            '_result.Tables(0).Columns(2).ColumnName = "Location"
            ''_result.Tables(0).Columns(3).ColumnName = "IncidentStatus"
            ''_result.Tables(0).Columns(4).ColumnName = "IncidentID"

            'Dim dv1 As System.Data.DataView = New System.Data.DataView(_result.Tables(0))
            'GridWeb1.WebWorksheets.ImportDataView(dv1, Nothing, Nothing, "Incident Report", 1, 0)

        Catch ex As Exception

        End Try



    End Sub

    Public Shared Function ListToDataTable(Of T)(list As List(Of T)) As DataTable
        Dim dt As New DataTable()
        For Each info As PropertyInfo In GetType(T).GetProperties()
            dt.Columns.Add(New DataColumn(info.Name, info.PropertyType))
        Next
        For Each t1 As T In list
            'For Each t As T In list
            Dim row As DataRow = dt.NewRow()
            For Each info As PropertyInfo In GetType(T).GetProperties()
                row(info.Name) = info.GetValue(t1, Nothing)
            Next
            dt.Rows.Add(row)
        Next
        Return dt


    End Function


    Public Shared Function ToCollection(Of T)(dt As DataTable) As List(Of T)
        Dim lst As List(Of T) = New System.Collections.Generic.List(Of T)()
        Dim tClass As Type = GetType(T)
        Dim pClass As PropertyInfo() = tClass.GetProperties()
        Dim dc As List(Of DataColumn) = dt.Columns.Cast(Of DataColumn)().ToList()
        Dim cn As T
        For Each item As DataRow In dt.Rows
            cn = DirectCast(Activator.CreateInstance(tClass), T)
            For Each pc As PropertyInfo In pClass
                ' Can comment try catch block. 
                Try
                    Dim d As DataColumn = dc.Find(Function(c) c.ColumnName = pc.Name)
                    If d IsNot Nothing Then
                        pc.SetValue(cn, item(pc.Name), Nothing)
                    End If
                Catch
                End Try
            Next
            lst.Add(cn)
        Next
        Return lst
    End Function

    'End sorting
    Protected Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        Dim tmpList As List(Of CourseManage) = New List(Of CourseManage)
        Edate = Date.Now.ToString("MM/dd/yy")

        If ddlTimeInterval.SelectedValue = "0" Then
            Sdate = DateAndTime.Now.AddMonths(-1).ToString("MM/dd/yy")
        ElseIf ddlTimeInterval.SelectedValue = "1" Then
            Sdate = DateAndTime.Now.AddMonths(-3).ToString("MM/dd/yy")
        ElseIf ddlTimeInterval.SelectedValue = "2" Then
            Sdate = DateAndTime.Now.AddMonths(-6).ToString("MM/dd/yy")
        ElseIf ddlTimeInterval.SelectedValue = "3" Then
            Sdate = DateAndTime.Now.AddYears(-1).ToString("MM/dd/yy")
        ElseIf ddlTimeInterval.SelectedValue = "4" Then

            Sdate = txtStartDate.Text
            Edate = txtEndDate.Text
            Dim li As List(Of String) = New List(Of String)
            li.Add(Sdate)
            li.Add(Edate)
            Session("CustomDates") = li
        Else

        End If

        Session("ddlTimeInterval") = Convert.ToString(ddlTimeInterval.SelectedValue)


        tmpList = getCourse(Sdate, Edate)

        If tmpList.Count > 0 Then
            gvActive.DataSource = tmpList
            gvActive.DataBind()
            'For sorting 
            Session("dvActiveData") = tmpList
        Else
            gvActive.DataSource = Nothing
            gvActive.DataBind()
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "ShowHideCustomDate(0);", True)
    End Sub

    <System.Web.Services.WebMethod(EnableSession:=True)> _
    Public Shared Function ClearSession() As String
        HttpContext.Current.Session("ddlTimeInterval") = Nothing
        HttpContext.Current.Session("dvActiveData") = Nothing
        HttpContext.Current.Session("CustomDates") = Nothing
        Return "true"
    End Function
End Class