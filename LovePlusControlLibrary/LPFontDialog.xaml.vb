Public Class LPFontDialog
    Public Structure FontInfo
        Dim decorations As TextDecorationCollection
        Dim family As FontFamily
        Dim fsytle As FontStyle
        Dim fweight As FontWeight
        Dim size As Double
        Dim isgdi As Boolean
    End Structure
    Private Sub chkIsGDI_Click(sender As Object, e As RoutedEventArgs) Handles chkIsGDI.Click, Me.Loaded
        If chkIsGDI.IsChecked Then
            TextOptions.SetTextFormattingMode(tblPrev, TextFormattingMode.Display)
        Else
            TextOptions.SetTextFormattingMode(tblPrev, TextFormattingMode.Ideal)
        End If
    End Sub
    Public Overloads Function ShowDialog(Optional DefaultControl As TextBlock = Nothing) As FontInfo
        If DefaultControl IsNot Nothing Then
            tblPrev.FontFamily = DefaultControl.FontFamily
            tblPrev.FontWeight = DefaultControl.FontWeight
            If DefaultControl.FontWeight = FontWeights.Bold Then
                chkBold.IsChecked = True
            End If
            If DefaultControl.FontStyle = FontStyles.Italic Then
                chkIt.IsChecked = True
            End If
            tblPrev.FontStyle = DefaultControl.FontStyle
            tblPrev.FontSize = DefaultControl.FontSize
            For Each dec In DefaultControl.TextDecorations
                If dec.Location = TextDecorationLocation.Underline Then
                    chkUnd.IsChecked = True
                ElseIf dec.Location = TextDecorationLocation.Strikethrough Then
                    chkDel.IsChecked = True
                End If
            Next
            If TextOptions.GetTextFormattingMode(DefaultControl) = TextFormattingMode.Display Then chkIsGDI.IsChecked = True
        End If

        MyBase.ShowDialog()

        Dim inf As New FontInfo With {.decorations = tblPrev.TextDecorations,
                                      .family = tblPrev.FontFamily,
                                      .fsytle = tblPrev.FontStyle,
                                      .fweight = tblPrev.FontWeight,
                                      .isgdi = chkIsGDI.IsChecked.Value,
                                      .size = tblPrev.FontSize
                                     }
        Return inf
    End Function
    Private Sub chkBold_Click(sender As Object, e As RoutedEventArgs) Handles chkBold.Click, chkIt.Click
        If chkBold.IsChecked Then
            tblPrev.FontWeight = FontWeights.Bold
        Else
            tblPrev.FontWeight = FontWeights.Normal
        End If
        If chkIt.IsChecked Then
            tblPrev.FontStyle = FontStyles.Italic
        Else
            tblPrev.FontStyle = FontStyles.Normal
        End If
    End Sub

    Private Sub chkUnd_Click(sender As Object, e As RoutedEventArgs) Handles chkUnd.Click, chkDel.Click, Me.Loaded
        With tblPrev
            .TextDecorations.Clear()
            If chkUnd.IsChecked Then .TextDecorations.Add(New TextDecoration(TextDecorationLocation.Underline, New Pen(Brushes.Black, 1), 0, TextDecorationUnit.Pixel, TextDecorationUnit.Pixel))
            If chkDel.IsChecked Then .TextDecorations.Add(New TextDecoration(TextDecorationLocation.Strikethrough, New Pen(Brushes.Black, 1), 0, TextDecorationUnit.Pixel, TextDecorationUnit.Pixel))
        End With
    End Sub

    Private Sub btnOK_PreviewMouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles btnOK.PreviewMouseLeftButtonUp
        Close()
    End Sub

    Private Sub RactDrag_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles RactDrag.PreviewMouseLeftButtonDown
        DragMove()
    End Sub
End Class
