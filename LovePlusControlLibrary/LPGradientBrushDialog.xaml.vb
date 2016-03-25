Public Class LPGradientBrushDialog
    ''' <summary>
    ''' 你必须使用带DefaultBrush参数的构造函数初始化此对话框
    ''' </summary>
    ''' <remarks></remarks>
    Sub New()
        InitializeComponent()
    End Sub
    Structure MixedPoint
        Dim Point1 As Point
        Dim Point2 As Point
        Sub New(p1 As Point, p2 As Point)
            Point1 = p1
            Point2 = p2
        End Sub
    End Structure

    Dim defBru As Brush
    Sub New(DefaultBrush As Brush)
        InitializeComponent()
        RadLinear.Tag = New MixedPoint(New Point, New Point(0, 1))
        RadRadial.Tag = New MixedPoint(New Point(0.5, 0.5), New Point(0.5, 0.5))
        defBru = DefaultBrush
    End Sub
    Private Sub RectDrag_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles RectDrag.PreviewMouseLeftButtonDown
        DragMove()
    End Sub

    Private Sub UpdateBrush()
        If RadRadial.IsChecked.Value Then
            If lstColor.Items.Count > 1 Then
                Dim sto As New GradientStopCollection
                For Each rec As Rectangle In lstColor.Items
                    sto.Add(New GradientStop(DirectCast(rec.Fill, SolidColorBrush).Color, CDbl(rec.Tag)))
                Next
                RectPrev.Fill = New RadialGradientBrush(sto) With {.Center = DirectCast(RadRadial.Tag, MixedPoint).Point1, .GradientOrigin = DirectCast(RadRadial.Tag, MixedPoint).Point2}
            Else
                RectPrev.Fill = Nothing
                Msg("颜色不足，请添加")
            End If
        ElseIf RadLinear.IsChecked.Value Then
            If lstColor.Items.Count > 1 Then
                Dim sto As New GradientStopCollection
                For Each rec As Rectangle In lstColor.Items
                    sto.Add(New GradientStop(DirectCast(rec.Fill, SolidColorBrush).Color, CDbl(rec.Tag)))
                Next
                Dim pts = DirectCast(RadLinear.Tag, MixedPoint)
                RectPrev.Fill = New LinearGradientBrush(sto, pts.Point1, pts.Point2)
            Else
                RectPrev.Fill = Nothing
                Msg("颜色不足，请添加")
            End If
        Else
            If lstColor.Items.Count > 0 Then
                RectPrev.Fill = DirectCast(lstColor.Items(0), Rectangle).Fill
            Else
                RectPrev.Fill = Nothing
                Msg("颜色不足，请添加")
            End If
        End If
    End Sub
    Private Sub RadSolid_Checked(sender As Object, e As RoutedEventArgs)
        UpdateBrush()
    End Sub

    Private Sub RadLinear_Checked(sender As Object, e As RoutedEventArgs)
        UpdateBrush()
    End Sub

    Private Sub RadRadio_Checked(sender As Object, e As RoutedEventArgs)
        UpdateBrush()
    End Sub

    Public Overloads Function ShowDialog() As Brush
        MyBase.ShowDialog()
        Return RectPrev.Fill
    End Function


    Private Sub LPGradientBrushDialog_Closing(sender As Object, e As ComponentModel.CancelEventArgs) Handles Me.Closing
        If RectPrev.Fill Is Nothing Then
            e.Cancel = True
            Msg("你还没正确选择画刷", LPMessageBox.LPMesageBoxStyle.fault, "错误")
        End If
    End Sub

    Private Sub btnDelColor_PreviewMouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles btnDelColor.PreviewMouseLeftButtonUp
        If lstColor.SelectedItem IsNot Nothing Then
            lstColor.Items.Remove(lstColor.SelectedItem)
            UpdateBrush()
        End If
    End Sub

    Private Sub btnAddColor_PreviewMouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles btnAddColor.PreviewMouseLeftButtonUp
        lstColor.Items.Add(New Rectangle() With {.Fill = New LPPenDialog().ShowDialog.Brush,
                                                       .Width = lstColor.ActualWidth - 30,
                                                       .Height = 20, .Tag = 0D})
        UpdateBrush()
    End Sub

    Private Sub sldPos_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles sldPos.ValueChanged
        If LoadComplete AndAlso lstColor.SelectedItem IsNot Nothing Then
            DirectCast(lstColor.SelectedItem, Rectangle).Tag = sldPos.Value
            UpdateBrush()
        End If
    End Sub

    Private Sub lstColor_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles lstColor.SelectionChanged
        If LoadComplete AndAlso lstColor.SelectedItem IsNot Nothing Then
            sldPos.Value = CDbl(DirectCast(lstColor.SelectedItem, Rectangle).Tag)
            UpdateBrush()
        End If

    End Sub

    Dim pt1 As New Point
    Private Sub RectPrev_Mousemove(sender As Object, e As MouseEventArgs) Handles RectPrev.MouseMove
        If (Mouse.LeftButton = MouseButtonState.Pressed OrElse Stylus.DirectlyOver Is RectPrev) Then
            Dim pt2 = e.GetPosition(RectPrev)
            If RadLinear.IsChecked.Value Then
                Dim st As New Point(pt1.X / RectPrev.ActualWidth, pt1.Y / RectPrev.ActualHeight)
                Dim en As New Point(pt2.X / RectPrev.ActualWidth, pt2.Y / RectPrev.ActualHeight)
                Dim br = DirectCast(RectPrev.Fill, LinearGradientBrush)
                br.EndPoint = en
                br.StartPoint = st
                RadLinear.Tag = New MixedPoint(st, en)
            ElseIf RadRadial.IsChecked.Value Then
                Dim cer = New Point(pt2.X / RectPrev.ActualWidth, pt2.Y / RectPrev.ActualHeight)
                RadRadial.Tag = New MixedPoint(cer, DirectCast(RadRadial.Tag, MixedPoint).Point2)
                DirectCast(RectPrev.Fill, RadialGradientBrush).Center = cer
            End If
        End If
    End Sub

    Private Sub RectPrev_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles RectPrev.PreviewMouseLeftButtonDown
        pt1 = e.GetPosition(RectPrev)
        If RadRadial.IsChecked.Value Then
            Dim ori = New Point(pt1.X / RectPrev.ActualWidth, pt1.Y / RectPrev.ActualHeight)
            RadRadial.Tag = New MixedPoint(DirectCast(RadRadial.Tag, MixedPoint).Point1, ori)
            DirectCast(RectPrev.Fill, RadialGradientBrush).GradientOrigin = ori
        End If
    End Sub
    Dim LoadComplete As Boolean = False
    Private Sub LPGradientBrushDialog_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim DefaultBrush = defBru
        Select Case DefaultBrush.GetType
            Case GetType(SolidColorBrush)
                lstColor.Items.Add(New Rectangle With {.Fill = DefaultBrush,
                                                       .Width = lstColor.ActualWidth - 30,
                                                       .Height = 20, .Tag = 0D})
                RadSolid.IsChecked = True
            Case GetType(LinearGradientBrush)
                Dim br = DirectCast(DefaultBrush, LinearGradientBrush)
                For Each st In br.GradientStops
                    lstColor.Items.Add(New Rectangle With {.Fill = New SolidColorBrush(st.Color),
                                                           .Width = lstColor.ActualWidth - 30,
                                                           .Height = 20, .Tag = st.Offset})
                Next
                RadLinear.Tag = New MixedPoint(br.StartPoint, br.EndPoint)
                RadLinear.IsChecked = True
            Case GetType(RadialGradientBrush)
                Dim br = DirectCast(DefaultBrush, RadialGradientBrush)
                For Each st In br.GradientStops
                    lstColor.Items.Add(New Rectangle With {.Fill = New SolidColorBrush(st.Color),
                                                           .Width = lstColor.ActualWidth - 30,
                                                           .Height = 20, .Tag = st.Offset})
                Next
                RadRadial.Tag = New MixedPoint(br.Center, br.GradientOrigin)
                RadRadial.IsChecked = True
            Case Else
                Throw New NotSupportedException("只能编辑纯色，线性渐变，径向渐变画刷")
        End Select
        LoadComplete = True
    End Sub

    Private Sub btnOK_PreviewMouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)
        Close()
    End Sub

End Class
