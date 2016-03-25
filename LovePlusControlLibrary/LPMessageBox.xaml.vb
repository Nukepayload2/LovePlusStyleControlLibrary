Imports System.Windows.Interop
Imports System.Windows.Threading

Public Class LPMessageBox
    Protected Overrides Sub OnStateChanged(e As System.EventArgs)
        MyBase.OnStateChanged(e)
        If WindowState = Windows.WindowState.Maximized Then WindowState = Windows.WindowState.Normal
    End Sub
    Private Sub RectDrag_PreviewMouseLeftButtonDown(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs) Handles RectDrag.PreviewMouseLeftButtonDown
        DragMove()
    End Sub

    Private Sub HeartControl1_PreviewMouseLeftButtonDown(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs) Handles HeartControl1.PreviewMouseLeftButtonDown
        DragMove()
    End Sub
    Enum LPMesageBoxResult
        ok
        yes
        no
    End Enum
    Enum LPMesageBoxStyle
        okonly
        yesno
        fault
    End Enum
    Dim curResult As LPMesageBoxResult

    Dim WithEvents RinkoAnim As New DispatcherTimer With {.Interval = TimeSpan.FromMilliseconds(150), .IsEnabled = False}


    Sub New(Message As String, style As LPMesageBoxStyle, Optional Header As String = "GET!!")

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Select Case style
            Case LPMesageBoxStyle.yesno
                BtnOK.Visibility = Windows.Visibility.Collapsed
            Case LPMesageBoxStyle.okonly
                btnNo.Visibility = Windows.Visibility.Collapsed
                btnYes.Visibility = Windows.Visibility.Collapsed
                RinkoAnim.IsEnabled = True
            Case LPMesageBoxStyle.fault
                btnNo.Visibility = Windows.Visibility.Collapsed
                btnYes.Visibility = Windows.Visibility.Collapsed
                Dim graybru As New SolidColorBrush(Colors.Gray)
                HeartControl1.BackColor = graybru
                PinkBorder.Background = graybru
                PinkBorder.BorderBrush = graybru
                HeartControl1.FontColor = New SolidColorBrush(Colors.Red)
        End Select
        HeartControl1.Text = Header
        MsgText.Text = Message
    End Sub
    Public Overloads Function ShowDialog() As LPMesageBoxResult
        MyBase.ShowDialog()
        Return curResult
    End Function

    Private Sub btnYes_PreviewMouseLeftButtonUp(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs) Handles btnYes.PreviewMouseLeftButtonUp
        curResult = LPMesageBoxResult.yes
        Close()
    End Sub

    Private Sub BtnOK_PreviewMouseLeftButtonUp(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs) Handles BtnOK.PreviewMouseLeftButtonUp
        curResult = LPMesageBoxResult.ok
        Close()
    End Sub

    Private Sub btnNo_PreviewMouseLeftButtonUp(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs) Handles btnNo.PreviewMouseLeftButtonUp
        curResult = LPMesageBoxResult.no
        Close()
    End Sub

    Private Sub MsgText_PreviewMouseLeftButtonDown(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs) Handles MsgText.PreviewMouseLeftButtonDown
        DragMove()
    End Sub

End Class
