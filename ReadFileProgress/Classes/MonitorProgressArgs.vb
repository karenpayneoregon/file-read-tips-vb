Namespace Classes
    Public Class MonitorProgressArgs
        Inherits EventArgs

        Public Sub New(amount As Integer, totalLineCount As Integer)

            Current = amount
            Total = totalLineCount
        End Sub
        ''' <summary>
        ''' Current line
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Current() As Integer
        ''' <summary>
        ''' Total lines
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Total() As Integer

    End Class
End Namespace