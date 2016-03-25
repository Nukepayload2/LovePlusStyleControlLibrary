Imports System.Windows.Threading
Imports System.Windows.Media.Effects

Public Class LPButton
    Public Property TextPadding As Thickness
        Get
            Return TextDisplay.Margin
        End Get
        Set(value As Thickness)
            TextDisplay.Margin = value
        End Set
    End Property

    'Dim WithEvents dt As New DispatcherTimer With {.Interval = TimeSpan.FromMilliseconds(100), .IsEnabled = True}
    Public Property Text As String
        Get
            Return TextDisplay.Text
        End Get
        Set(value As String)
            TextDisplay.Text = value
        End Set
    End Property
    'Private Sub dt_Tick(sender As Object, e As System.EventArgs) Handles dt.Tick
    '    If Not IsNothing(InputHitTest(Mouse.GetPosition(Me))) AndAlso ((InputHitTest(Mouse.GetPosition(Me)).IsMouseOver AndAlso Mouse.LeftButton = MouseButtonState.Pressed) OrElse InputHitTest(Mouse.GetPosition(Me)).IsStylusDirectlyOver) Then
    '        MouseDownEventTrigger(False)
    '        RenderTransform = Nothing
    '    Else
    '        MouseDownEventTrigger(True)
    '        RenderTransform = New TranslateTransform(0, -2)
    '    End If
    'End Sub
    'Private Sub MouseDownEventTrigger(IsBlue As Boolean)
    '    Static OldIsblue As Boolean = False
    '    If IsBlue <> OldIsblue Then
    '        ChangeColor(IsBlue)
    '        OldIsblue = IsBlue
    '    End If
    'End Sub
    Dim curIsBlue As Boolean = True
    Private Sub ChangeColor(IsBlue As Boolean)
        '<GradientStop Color="Cyan" Offset="0.407" />
        '<GradientStop Color="#FF00C5FF" Offset="0.68" />
        If curIsBlue = IsBlue Then Return
        If IsBlue Then
            OutterBorder.BorderBrush = Brushes.Cyan
            'BackBorder.Background = Brushes.Cyan
            Dim Grad As New GradientStopCollection
            Grad.Add(New GradientStop(Colors.Cyan, 0.68))
            Grad.Add(New GradientStop(Color.FromArgb(&HFF, 0, &HC5, &HFF), 0.407))
            TextBackground.Fill = New LinearGradientBrush(Grad, New Point(0.6, 1), New Point(0.45, -0.4))
            '<DropShadowEffect BlurRadius="10" Direction="300" Color="#FF00A4FF" ShadowDepth="0"></DropShadowEffect>
            TextDisplay.Effect = New DropShadowEffect With {.BlurRadius = 10, .Color = Color.FromArgb(&HFF, 0, &HA4, &HFF), .ShadowDepth = 0}
        Else
            OutterBorder.BorderBrush = Brushes.Orange
            'BackBorder.Background = Brushes.Orange
            Dim Grad As New GradientStopCollection
            Grad.Add(New GradientStop(Colors.Orange, 0.68))
            Grad.Add(New GradientStop(Colors.OrangeRed, 0.407))
            TextBackground.Fill = New LinearGradientBrush(Grad, New Point(0.6, 1), New Point(0.45, -0.4))
            TextDisplay.Effect = New DropShadowEffect With {.BlurRadius = 10, .Color = Colors.OrangeRed, .ShadowDepth = 0}
        End If
        curIsBlue = IsBlue
    End Sub

    Private Sub LPButton_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles Me.PreviewMouseLeftButtonDown
        ChangeColor(False)
    End Sub

    Private Sub LPButton_PreviewMouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles Me.PreviewMouseLeftButtonUp
        ChangeColor(True)
    End Sub

    Private Sub LPButton_IsEnabledChanged(sender As Object, e As DependencyPropertyChangedEventArgs) Handles Me.IsEnabledChanged
        If IsEnabled Then
            Opacity = 1
        Else
            Opacity = 0.5
        End If
    End Sub
End Class
