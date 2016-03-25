Public Class LPFlower
    Dim curCol As Color = Colors.Pink
    Public Property OutterColor As Color
        Get
            Return curCol
        End Get
        Set(value As Color)
            curCol = value
            Flower.Fill = New RadialGradientBrush With {.RadiusY = 0.25,
                                                      .GradientStops = New GradientStopCollection From {
                                                          New GradientStop(Colors.White, 0),
                                                          New GradientStop(value, 0.442)
                                                      }
                                                     }
        End Set
    End Property
End Class
