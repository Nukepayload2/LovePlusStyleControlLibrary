''' <summary>
''' 各种消息框
''' </summary>
''' <remarks></remarks>
Public Module 消息框
    ''' <summary>
    ''' Loveplus+ 风格的对话框 （不是线程安全的，在非STA线程调用时需要Dispatcher.Invoke）
    ''' </summary>
    ''' <param name="Message">消息文本</param>
    ''' <param name="style">消息框样式</param>
    ''' <param name="Header">红心上的字</param>
    ''' <returns>按的按钮</returns>
    ''' <remarks></remarks>
    Public Function Msg(Message As String, Optional style As LPMessageBox.LPMesageBoxStyle = LPMessageBox.LPMesageBoxStyle.okonly, Optional Header As String = "GET!!") As LPMessageBox.LPMesageBoxResult
        Return New LPMessageBox(Message, style, Header).ShowDialog
    End Function
    ''' <summary>
    ''' 线程安全的错误提示框
    ''' </summary>
    ''' <param name="wnd">目标窗体</param>
    ''' <param name="ex">异常</param>
    ''' <remarks></remarks>
    Public Sub ErrorBox(wnd As Window, ex As Exception)
        Dim errtxt As String = ""
        If TypeOf ex Is InvalidOperationException Then
            errtxt = "发生操作无效异常"
        ElseIf TypeOf ex Is ArgumentException Then
            errtxt = "发生参数无效异常"
        End If
        If Not String.IsNullOrEmpty(errtxt) Then errtxt += vbLf
        wnd.Dispatcher.Invoke(Sub() Msg(errtxt + ex.Message, LPMessageBox.LPMesageBoxStyle.fault, "错误"))
    End Sub

    Public Function InputBox(Message As String) As String
        Return New LPInputBox(Message).ShowDialog
    End Function
End Module
