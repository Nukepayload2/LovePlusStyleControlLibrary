Imports System.Windows.Interop

Public Class LPInputBox
    Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。

    End Sub

    Sub New(Message As String)

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        MsgText.Text = Message
    End Sub
    Protected Overrides Sub OnStateChanged(e As System.EventArgs)
        MyBase.OnStateChanged(e)
        If WindowState = Windows.WindowState.Maximized Then WindowState = Windows.WindowState.Normal
    End Sub
    Private Sub RectDrag_PreviewMouseLeftButtonDown(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs) Handles RectDrag.PreviewMouseLeftButtonDown
        DragMove()
    End Sub
    Private Sub btnCancel_PreviewMouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)
        txtRetValue.Text = ""
        Close()
    End Sub
    Public Overloads Function ShowDialog() As String
        MyBase.ShowDialog()
        Return txtRetValue.Text
    End Function

    Private Sub BtnOK_PreviewMouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)
        Close()
    End Sub

End Class
