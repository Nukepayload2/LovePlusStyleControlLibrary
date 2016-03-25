Imports System.Windows.Media.Effects

Public Class LPEffectDialog
    Sub New()
        InitializeComponent()
    End Sub
    Dim defEff As Effect
    Sub New(DefaultEffect As Effect)
        InitializeComponent()
        defEff = DefaultEffect
        
    End Sub


    Public Overloads Function ShowDialog() As Effect
        MyBase.ShowDialog()
        Return tblPrev.Effect
    End Function

    Private Sub btnClearEffects_PreviewMouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles btnClearEffects.PreviewMouseLeftButtonUp
        tblPrev.ClearValue(TextBlock.EffectProperty)
    End Sub

    Private Sub btnOK_PreviewMouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles btnOK.PreviewMouseLeftButtonUp
        Close()
    End Sub
     
    Private Sub radSmooth_Checked(sender As Object, e As RoutedEventArgs) Handles radSmooth.Checked
        Try
            If IsInitialized Then
                tblPrev.Effect = New BlurEffect With {.Radius = CDbl(txtBlurRadius.Text)}

            End If
        Catch ex As Exception
            ErrorBox(Me, ex)
        End Try
    End Sub

    Private Sub txtBlueRadius_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtBlurRadius.TextChanged
        Try
            If IsInitialized AndAlso TypeOf tblPrev.Effect Is BlurEffect Then
                DirectCast(tblPrev.Effect, BlurEffect).Radius = CDbl(txtBlurRadius.Text)
            End If
        Catch ex As Exception
            ErrorBox(Me, ex)
        End Try
    End Sub

    Private Sub UpdateShadowEffect() Handles radDropShadow.Checked, txtDirection.TextChanged, txtShadowBlurRadius.TextChanged, txtOpecity.TextChanged, txtShadowDepth.TextChanged
        Try
            If FinishLoad AndAlso radDropShadow.IsChecked Then
                tblPrev.Effect = New DropShadowEffect With {.BlurRadius = CDbl(txtShadowBlurRadius.Text),
                                                                        .Color = DirectCast(rectPrev.Fill, SolidColorBrush).Color,
                                                                        .Direction = CDbl(txtDirection.Text),
                                                                        .ShadowDepth = CDbl(txtShadowDepth.Text),
                                                                        .Opacity = CDbl(txtOpecity.Text)}

            End If

        Catch ex As Exception
            ErrorBox(Me, ex)
        End Try
    End Sub

    Dim FinishLoad As Boolean = False
    Private Sub LPEffectDialog_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim defaulteffect = defEff
        If Not IsNothing(DefaultEffect) Then
            tblPrev.Effect = DefaultEffect
            Select Case DefaultEffect.GetType
                Case GetType(BlurEffect)
                    radSmooth.IsChecked = True
                    txtBlurRadius.Text = DirectCast(DefaultEffect, BlurEffect).Radius.ToString
                Case GetType(DropShadowEffect)
                    radDropShadow.IsChecked = True
                    radSmooth.IsChecked = False
                    Dim ef = DirectCast(DefaultEffect, DropShadowEffect)
                    txtDirection.Text = ef.Direction.ToString
                    txtShadowBlurRadius.Text = ef.BlurRadius.ToString
                    txtShadowDepth.Text = ef.ShadowDepth.ToString
                    txtOpecity.Text = CStr(CDbl(ef.Color.A) / 255)
                    rectPrev.Fill = New SolidColorBrush(ef.Color)
                    FinishLoad = True
                    UpdateShadowEffect()
            End Select
        End If
        FinishLoad = True
    End Sub

    Private Sub Rectangle_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        DragMove()
    End Sub

    Private Sub btnSelColor_PreviewMouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles btnSelColor.PreviewMouseLeftButtonUp
        rectPrev.Fill = New LPPenDialog(New Pen(rectPrev.Fill, 1)).ShowDialog.Brush
        txtOpecity.Text = CStr(CDbl(DirectCast(rectPrev.Fill, SolidColorBrush).Color.A) / 255)
        UpdateShadowEffect()
    End Sub
End Class
