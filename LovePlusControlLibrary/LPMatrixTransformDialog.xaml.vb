''' <summary>
''' 得到一个TransformInfo，其Group依次包含平移旋转倾斜和缩放变换
''' </summary>
''' <remarks></remarks>
Public Class LPMatrixTransformDialog
    Public Structure TransformInfo
        Dim Group As TransformGroup
        Dim Origin As Point
        Sub New(ctl As UIElement)
            If TypeOf ctl.RenderTransform Is MatrixTransform Then
                Group = GetEmptyTransformGroup()
            Else
                Group = DirectCast(ctl.RenderTransform, TransformGroup)
            End If
            Origin = ctl.RenderTransformOrigin
        End Sub
        Sub New(Gp As TransformGroup, Ori As Point)
            Group = Gp
            Origin = Ori
        End Sub
    End Structure
    Sub New()
        InitializeComponent()
    End Sub
    Dim defTrans As TransformInfo
    Sub New(DefaultTransformInfo As TransformInfo)
        InitializeComponent()
        defTrans = DefaultTransformInfo
    End Sub
    Public Overloads Function ShowDialog() As TransformInfo
        MyBase.ShowDialog()
        Return New TransformInfo(ctlPrev)
    End Function
    Private Sub btnOK_PreviewMouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles btnOK.PreviewMouseLeftButtonUp
        Close()
    End Sub
    Dim LoadFinished As Boolean = False
    Public Shared Function GetEmptyTransformGroup() As TransformGroup
        Dim co As New TransformCollection
        co.Add(New TranslateTransform)
        co.Add(New RotateTransform)
        co.Add(New SkewTransform)
        co.Add(New ScaleTransform)
        Return New TransformGroup With {.Children = co}
    End Function
    Private Sub LPMatrixTransformDialog_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If defTrans.Group Is Nothing Then defTrans.Group = GetEmptyTransformGroup()
        ctlPrev.RenderTransform = defTrans.Group
        ctlPrev.RenderTransformOrigin = defTrans.Origin
        Dim trs = DirectCast(defTrans.Group.Children(0), TranslateTransform)
        Dim rot = DirectCast(defTrans.Group.Children(1), RotateTransform)
        Dim skw = DirectCast(defTrans.Group.Children(2), SkewTransform)
        Dim scl = DirectCast(defTrans.Group.Children(3), ScaleTransform)
        txtCenterX.Text = CStr(defTrans.Origin.X)
        txtCenterY.Text = CStr(defTrans.Origin.Y)
        txtRotate.Text = rot.Angle.ToString
        txtScaleX.Text = scl.ScaleX.ToString
        txtScaleY.Text = scl.ScaleY.ToString
        txtSkewX.Text = skw.AngleX.ToString
        txtSkewY.Text = skw.AngleY.ToString
        txtTransX.Text = trs.X.ToString
        txtTransY.Text = trs.Y.ToString
        LoadFinished = True
    End Sub
    Private Sub UpdateTranslate() Handles txtTransX.TextChanged, txtTransY.TextChanged
        Try
            If LoadFinished Then
                With DirectCast(DirectCast(ctlPrev.RenderTransform, TransformGroup).Children(0), TranslateTransform)
                    .X = CDbl(txtTransX.Text)
                    .Y = CDbl(txtTransY.Text)
                End With
            End If

        Catch ex As Exception
            Msg("平移填写错误", LPMessageBox.LPMesageBoxStyle.fault, "错误")
        End Try
    End Sub
    Private Sub UpdateSkew() Handles txtSkewX.TextChanged, txtSkewY.TextChanged
        Try
            If LoadFinished Then

                With DirectCast(DirectCast(ctlPrev.RenderTransform, TransformGroup).Children(2), SkewTransform)
                    .AngleX = CDbl(txtSkewX.Text)
                    .AngleY = CDbl(txtSkewY.Text)
                End With
            End If
        Catch ex As Exception
            Msg("倾斜填写错误", LPMessageBox.LPMesageBoxStyle.fault, "错误")
        End Try
    End Sub
    Private Sub UpdateScale() Handles txtScaleX.TextChanged, txtScaleY.TextChanged
        Try
            If LoadFinished Then
                With DirectCast(DirectCast(ctlPrev.RenderTransform, TransformGroup).Children(3), ScaleTransform)
                    .ScaleX = CDbl(txtScaleX.Text)
                    .ScaleY = CDbl(txtScaleY.Text)
                End With
            End If
        Catch ex As Exception
            Msg("缩放填写错误", LPMessageBox.LPMesageBoxStyle.fault, "错误")
        End Try
    End Sub
    Private Sub UpdateRotate() Handles txtRotate.TextChanged
        Try
            If LoadFinished Then DirectCast(DirectCast(ctlPrev.RenderTransform, TransformGroup).Children(1), RotateTransform).Angle = CDbl(txtRotate.Text)
        Catch ex As Exception
            Msg("旋转填写错误", LPMessageBox.LPMesageBoxStyle.fault, "错误")
        End Try
    End Sub
    Private Sub UpdateCenter() Handles txtCenterX.TextChanged, txtCenterY.TextChanged
        Try
            If LoadFinished Then ctlPrev.RenderTransformOrigin = New Point(CDbl(txtCenterX.Text), CDbl(txtCenterY.Text))
        Catch ex As Exception
            Msg("中心点填写错误", LPMessageBox.LPMesageBoxStyle.fault, "错误")
        End Try
    End Sub

    Private Sub RcDrag_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles RcDrag.PreviewMouseLeftButtonDown
        DragMove()
    End Sub
End Class
 