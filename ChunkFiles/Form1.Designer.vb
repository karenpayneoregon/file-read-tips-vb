<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.SplitLinesByTextBox = New WindowsFormsControls.NumericTextBox()
        Me.GetNextButton = New System.Windows.Forms.Button()
        Me.ChunkFileButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'SplitLinesByTextBox
        '
        Me.SplitLinesByTextBox.Location = New System.Drawing.Point(151, 26)
        Me.SplitLinesByTextBox.Name = "SplitLinesByTextBox"
        Me.SplitLinesByTextBox.Size = New System.Drawing.Size(100, 20)
        Me.SplitLinesByTextBox.TabIndex = 1
        '
        'GetNextButton
        '
        Me.GetNextButton.Image = Global.ChunkFiles.My.Resources.Resources.Writeable_16x
        Me.GetNextButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.GetNextButton.Location = New System.Drawing.Point(25, 52)
        Me.GetNextButton.Name = "GetNextButton"
        Me.GetNextButton.Size = New System.Drawing.Size(120, 23)
        Me.GetNextButton.TabIndex = 2
        Me.GetNextButton.Text = "Get Next"
        Me.GetNextButton.UseVisualStyleBackColor = True
        '
        'ChunkFileButton
        '
        Me.ChunkFileButton.Image = Global.ChunkFiles.My.Resources.Resources.ExportFile_16x
        Me.ChunkFileButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ChunkFileButton.Location = New System.Drawing.Point(25, 23)
        Me.ChunkFileButton.Name = "ChunkFileButton"
        Me.ChunkFileButton.Size = New System.Drawing.Size(120, 23)
        Me.ChunkFileButton.TabIndex = 0
        Me.ChunkFileButton.Text = "Chunk file"
        Me.ChunkFileButton.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(290, 79)
        Me.Controls.Add(Me.GetNextButton)
        Me.Controls.Add(Me.SplitLinesByTextBox)
        Me.Controls.Add(Me.ChunkFileButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Chuk file code sample"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ChunkFileButton As Button
    Friend WithEvents SplitLinesByTextBox As WindowsFormsControls.NumericTextBox
    Friend WithEvents GetNextButton As Button
End Class
