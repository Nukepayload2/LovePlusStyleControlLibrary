Imports System.Runtime.InteropServices.Marshal
Imports System.Runtime.CompilerServices

Public Class LPPenDialog

    Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。

    End Sub

    Sub New(DefaultPen As Pen)
        InitializeComponent()
        If Not IsNothing(DefaultPen) Then
            Dim col = DirectCast(DefaultPen.Brush, SolidColorBrush).Color
            txtWid.Text = DefaultPen.Thickness.ToString
            RefreshRectFromColor(col)
        End If
    End Sub

    Private Sub Rectangle_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Dim col = GetColorFromPoint(CType(sender, FrameworkElement), e.GetPosition(CType(sender, IInputElement)))
        RefreshRectFromColor(col)
    End Sub

    Private Sub RefreshRectFromColor(col As Color)
        RecPrev.Fill = New LinearGradientBrush(New GradientStopCollection From {New GradientStop(Color.FromArgb(col.A, 0, 0, 0), 0), New GradientStop(col, 0.5), New GradientStop(Color.FromArgb(col.A, 255, 255, 255), 1)}) With {.StartPoint = New Point, .EndPoint = New Point(1, 0)}
        txtBlue.Text = col.B.ToString
        txtGreen.Text = col.G.ToString
        txtRed.Text = col.R.ToString
        txtTrans.Text = col.A.ToString
    End Sub
    Private Function GetColorFromPoint(ctl As FrameworkElement, pos As Point) As Color
        Dim bmp As New RenderTargetBitmap(CInt(ctl.ActualWidth), CInt(ctl.ActualHeight), 96, 96, PixelFormats.Default)
        Dim dv As New DrawingVisual
        Using dc = dv.RenderOpen()
            dc.DrawRectangle(New VisualBrush(ctl), Nothing, New Rect(0, 0, ctl.ActualWidth, ctl.ActualHeight))
        End Using
        bmp.Render(dv)
        Dim bm As New WriteableBitmap(bmp)
        Dim pColor = bm.BackBuffer + CInt(pos.Y * bm.BackBufferStride) + CInt(pos.X * 4)
        Dim pColorFirst = bm.BackBuffer + CInt(pos.X * 4)
        Dim col As New Color
        col.B = ReadByte(pColorFirst, 0)
        col.G = ReadByte(pColorFirst, 1)
        col.R = ReadByte(pColorFirst, 2)
        col.A = ReadByte(pColor, 3)
         
        Return col
    End Function


    Public Overloads Function ShowDialog() As Pen
        MyBase.ShowDialog()
        Dim pe As New Pen(New SolidColorBrush(Color.FromArgb(CByte(txtTrans.Text), CByte(txtRed.Text), CByte(txtGreen.Text), CByte(txtBlue.Text))), CDbl(txtWid.Text))
        Return pe
    End Function

    Private Function CanClose() As Boolean
        Try
            Dim pe As New Pen(New SolidColorBrush(Color.FromArgb(CByte(txtTrans.Text), CByte(txtRed.Text), CByte(txtGreen.Text), CByte(txtBlue.Text))), CDbl(txtWid.Text))
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub LPPenDialog_Closing(sender As Object, e As ComponentModel.CancelEventArgs) Handles Me.Closing
        e.Cancel = Not CanClose()
        If e.Cancel Then ArgExp()
    End Sub
    Private Sub ArgExp()
        Msg("笔的颜色或粗细不在有效值内，请从指定区域选择颜色", LPMessageBox.LPMesageBoxStyle.fault, "错误")
    End Sub
    Private Sub BtnOK_PreviewMouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)
        Close()
    End Sub

    Private Sub RecPrev_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles RecPrev.PreviewMouseLeftButtonDown
        Dim col = GetColorFromPoint(CType(sender, FrameworkElement), e.GetPosition(CType(sender, IInputElement)))
        txtBlue.Text = col.B.ToString
        txtGreen.Text = col.G.ToString
        txtRed.Text = col.R.ToString
        txtTrans.Text = col.A.ToString
    End Sub

    Private Sub InkPrev_MouseEnter(sender As Object, e As MouseEventArgs) Handles InkPrev.MouseEnter
        Try
            Dim attr As New Ink.DrawingAttributes
            attr.Color = Color.FromArgb(CByte(txtTrans.Text), CByte(txtRed.Text), CByte(txtGreen.Text), CByte(txtBlue.Text))
            attr.Width = CDbl(txtWid.Text)
            attr.Height = CDbl(txtWid.Text)
            InkPrev.DefaultDrawingAttributes = attr
        Catch ex As Exception
            ArgExp()
        End Try

    End Sub

    Private Sub RectDrag_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles RectDrag.PreviewMouseLeftButtonDown
        DragMove()
    End Sub

    Private Sub InkPrev_PreviewMouseRightButtonUp(sender As Object, e As MouseButtonEventArgs) Handles InkPrev.PreviewMouseRightButtonUp
        InkPrev.Strokes.Clear()
    End Sub
End Class
