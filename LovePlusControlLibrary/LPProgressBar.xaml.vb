Imports System.Threading

Public Class LPProgressBar
    ''' <summary>
    ''' 请务必在应用程序退出时把此值设置为True，否则可能会因Flush线程未终止而延迟退出。
    ''' </summary>
    ''' <remarks></remarks>
    Public IsExiting As Boolean = False
    ''' <summary>
    ''' 填充色
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FillBrush As Brush
        Get
            Return ForeBar.Fill
        End Get
        Set(value As Brush)
            ForeBar.Fill = value
        End Set
    End Property
    Dim ProgressValue As Double
    Dim mx As Double = 100
    ''' <summary>
    ''' 最大值
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MaxValue As Double
        Get
            Return mx
        End Get
        Set(value As Double)
            mx = value
        End Set
    End Property
    ''' <summary>
    ''' 最小值
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MinValue As Double = 0
    Private Function IIf(Expression As Boolean, TruePart As Double, FalsePart As Double) As Double
        If Expression Then
            Return TruePart
        Else
            Return FalsePart
        End If
    End Function
    ''' <summary>
    ''' 是否溢出
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property IsOverflow As Boolean
        Get
            Return ProgressValue > MaxValue
        End Get
    End Property
    ''' <summary>
    ''' 溢出了多少，如果没有溢出就返回0
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property OverflowValue As Double
        Get
            Return IIf(IsOverflow, ProgressValue - MaxValue, 0)
        End Get
    End Property
    ''' <summary>
    ''' 正常进度+溢出进度的上限
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MaxOverflow As Double = 100

    Private Function ValueToActualWidth(v As Double) As Double
        Return v * ActualWidth / (MaxValue - MinValue)
    End Function
    ''' <summary>
    ''' 是否需要Flush
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsFlushNeeded As Boolean = False
    Dim IsDecreasing As Boolean = False
    ''' <summary>
    ''' 进度值，不计溢出部分
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Value As Double
        Get
            Return IIf(IsOverflow, MaxValue, ProgressValue)
        End Get
        Set(v As Double)
            If v < MinValue Then Return
            If IsOverflow Then
                If v < MaxOverflow Then
                    RedAndCyan.Fill = New SolidColorBrush(Colors.DarkRed)
                    ForeBar.Width = ValueToActualWidth(v)
                End If
            Else
                If v > ProgressValue Then
                    IsDecreasing = False
                    RedAndCyan.Fill = New SolidColorBrush(Colors.Cyan)
                    RedAndCyan.Width = ValueToActualWidth(v)
                Else
                    IsDecreasing = True
                    RedAndCyan.Fill = New SolidColorBrush(Colors.DarkRed)
                    ForeBar.Width = ValueToActualWidth(v)
                End If
                'Debug.WriteLine(ForeBar.Width)
            End If
            IsFlushNeeded = True
            If v > MaxOverflow Then v = MaxOverflow
            'Debug.WriteLine(v)
            ProgressValue = v
        End Set
    End Property
    Dim IsFlushing As Boolean = False
    ''' <summary>
    ''' 如果进度条已经移动，则播放进度条对齐动画
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Flush()
        Const Speed = 8
        Dim retry As Boolean = False
        'Debug.WriteLine("flush")
        If IsFlushNeeded And Not IsFlushing Then
            IsFlushing = True

            Dim oldValue As Double = ForeBar.ActualWidth
            Dim newValue As Double = ValueToActualWidth(Value)
            Dim forewid As Double = oldValue
            If IsDecreasing Then
                RedAndCyan.Width = newValue
                oldValue = RedAndCyan.ActualWidth
            End If
            ''Debug.WriteLine("oldValue" + oldValue.ToString)
            'Debug.WriteLine("newValue" + newValue.ToString)
            If oldValue > newValue Then
                'Debug.WriteLine("减少了")
                Dim th As New Thread(Sub()
                                         For i As Integer = CInt(forewid) To 0 Step -Speed
                                             Dim j As Integer = i
                                             Dispatcher.BeginInvoke(Sub()
                                                                        'Debug.WriteLine("Flush Up" + j.ToString)
                                                                        ForeBar.Width = j
                                                                    End Sub)
                                             If IsExiting Then Return
                                             Thread.Sleep(10)
                                         Next
                                         For i As Integer = 0 To CInt((newValue)) Step Speed
                                             newValue = ValueToActualWidth(Value)
                                             Dim j As Integer = i
                                             Dispatcher.BeginInvoke(Sub()
                                                                        'Debug.WriteLine("Flush Down" + j.ToString)
                                                                        ForeBar.Width = j
                                                                    End Sub)
                                             If IsExiting Then Return
                                             Thread.Sleep(10)
                                         Next
                                         Dispatcher.BeginInvoke(Sub()
                                                                    newValue = ValueToActualWidth(Value)
                                                                    RedAndCyan.Width = (newValue)
                                                                    ForeBar.Width = (newValue)
                                                                End Sub)
                                         IsFlushing = False
                                     End Sub)
                th.Start()
            ElseIf oldValue < newValue Then
                Dim max As Double = ValueToActualWidth(MaxValue)
                'Debug.WriteLine("增加")
                Dim th As New Thread(Sub()
                                         For i As Integer = CInt(oldValue) To CInt((max)) Step Speed
                                             Dim j As Integer = i
                                             Dispatcher.BeginInvoke(Sub()
                                                                        ForeBar.Width = j
                                                                        'Debug.WriteLine("Flush Up" + j.ToString)
                                                                    End Sub)
                                             If IsExiting Then Return
                                             Thread.Sleep(10)
                                         Next
                                         For i As Integer = CInt((max)) To CInt((newValue)) Step -Speed
                                             newValue = ValueToActualWidth(Value)
                                             'Debug.WriteLine(newValue)
                                             Dim j As Integer = i
                                             Dispatcher.BeginInvoke(Sub()
                                                                        'Debug.WriteLine("Flush Down" + j.ToString)
                                                                        ForeBar.Width = j
                                                                    End Sub)
                                             If IsExiting Then Return
                                             Thread.Sleep(10)
                                         Next
                                         Dispatcher.BeginInvoke(Sub()
                                                                    newValue = ValueToActualWidth(Value)
                                                                    RedAndCyan.Width = (newValue)
                                                                    ForeBar.Width = (newValue)
                                                                End Sub)
                                         IsFlushing = False
                                     End Sub)
                th.Start()
            Else
                IsFlushing = False
                'Debug.WriteLine("flush不变")
            End If
        Else
            'Debug.WriteLine("flush进行中或value没有change")
        End If
        IsFlushNeeded = False
    End Sub

    Private Sub LPProgressBar_Loaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Value += 1
        Value -= 1
    End Sub
End Class


