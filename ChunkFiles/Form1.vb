Imports System.ComponentModel
Imports System.IO
Imports ChunkFiles.Classes

Public Class Form1
    Private Sub ChunkFileButton_Click(sender As Object, e As EventArgs) Handles ChunkFileButton.Click

        If SplitLinesByTextBox.AsInteger = 0 Then
            Exit Sub
        End If

        Dim fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TextFile1.txt")
        Try
            FileOperations.SplitLargeFile(fileName, SplitLinesByTextBox.AsInteger)
        Catch ex As Exception
            MessageBox.Show($"Ran into{Environment.NewLine}{ex.Message}")
        End Try

        Dim verifyList = FileOperations.VerifyLineCounts(fileName)

        Dim verifyForm As New VerifyForm(verifyList)

        Try
            verifyForm.ShowDialog()
        Finally
            verifyForm.Dispose()
        End Try

    End Sub

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        SplitLinesByTextBox.Text = ApplicationSettings.GetLineSplit()
    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If Not String.IsNullOrWhiteSpace(SplitLinesByTextBox.Text) Then
            ApplicationSettings.LinesToSplit(SplitLinesByTextBox.Text)
        End If
    End Sub
    ''' <summary>
    ''' Conceptual get next file
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub GetNextButton_Click(sender As Object, e As EventArgs) Handles GetNextButton.Click
        Dim fileName = ""

        Try
            fileName = FileOperations.CheckIfNewIncomingFileIsNeeded()
            If String.IsNullOrWhiteSpace(fileName) Then
                MessageBox.Show("Finished")
            Else
                MessageBox.Show(fileName)
            End If
        Catch ex As Exception
            MessageBox.Show($"Failed: {ex.Message}")
        End Try
    End Sub
End Class
