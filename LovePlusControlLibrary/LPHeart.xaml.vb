Public Class LPHeart
    Public Property FontColor As Brush
        Get
            Return t3.Foreground
        End Get
        Set(value As Brush)
            t3.Foreground = value
        End Set
    End Property
    Public Property BackColor As Brush
        Get
            Return HeartBack.Fill
        End Get
        Set(value As Brush)
            HeartWhiteLine.Fill = value
            HeartBack.Fill = value
        End Set
    End Property
    Public Property Text As String
        Get
            Return t1.Text
        End Get
        Set(value As String)
            t1.Text = value
            t2.Text = value
            t3.Text = value
            t4.Text = value
        End Set
    End Property
    Private Sub HeartControl_Loaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        G1.RenderTransform = New ScaleTransform(Width / 100, Height / 100)
        t1.RenderTransform = New ScaleTransform(1, Width / Height)
        t2.RenderTransform = New ScaleTransform(1, Width / Height)
        t3.RenderTransform = New ScaleTransform(1, Width / Height)
        t4.RenderTransform = New ScaleTransform(1, Width / Height)
    End Sub
End Class
