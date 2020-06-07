Imports System.IO

Namespace Classes

    Public Class FileOperations

        Private Shared _chunkFolderLocation As String
        Private Shared _workFolderLocation As String
        Private Shared _chunkFileBaseName As String

        ''' <summary>
        ''' Location where smaller chunk files are created
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property ChunkFolderLocation() As String
            Get
                If String.IsNullOrWhiteSpace(_chunkFolderLocation) Then
                    _chunkFolderLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                        ApplicationSettings.GetChunkFolderLocation())
                End If

                Return _chunkFolderLocation

            End Get
        End Property
        Public Shared ReadOnly Property WorkFolderLocation() As String
            Get
                If String.IsNullOrWhiteSpace(_workFolderLocation) Then
                    _workFolderLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                       ApplicationSettings.GetWorkFolderLocation())
                End If

                Return _workFolderLocation

            End Get
        End Property
        ''' <summary>
        ''' Base chunk file name which has a _Number appended
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property ChunkFileBaseName() As String
            Get
                If String.IsNullOrWhiteSpace(_chunkFileBaseName) Then
                    _chunkFileBaseName = ApplicationSettings.GetChunkFileBaseName()
                End If

                Return _chunkFileBaseName

            End Get
        End Property
        ''' <summary>
        ''' Split a larger file into smaller files where the smaller files
        ''' line count equal the line count of the larger file
        ''' </summary>
        ''' <param name="fileName">Valid existing file with path</param>
        ''' <param name="splitSize">How many lines to split on</param> 
        Public Shared Sub SplitLargeFile(fileName As String, ByVal splitSize As Integer)

            If Not File.Exists(fileName) Then
                Throw New FileNotFoundException(fileName)
            End If

            Dim files = Directory.GetFiles(ChunkFolderLocation)

            For fileIndex As Integer = 0 To files.Count() - 1
                File.Delete(files(fileIndex))
            Next

            Using lineIterator As IEnumerator(Of String) = File.ReadLines(fileName).GetEnumerator()

                Dim stillGoing = True

                Dim chunkIndex As Integer = 0

                Do While stillGoing
                    stillGoing = WriteChunk(lineIterator, splitSize, chunkIndex)
                    chunkIndex += 1
                Loop

            End Using

            RemoveZeroLengthFiles()

        End Sub
        ''' <summary>
        ''' Create smaller chunk file
        ''' </summary>
        ''' <param name="lineIterator"></param>
        ''' <param name="splitSize"></param>
        ''' <param name="chunk"></param>
        ''' <returns></returns>
        Private Shared Function WriteChunk(lineIterator As IEnumerator(Of String),
                                           splitSize As Integer, chunk As Integer) As Boolean

            Dim fileName = Path.Combine(ChunkFolderLocation, $"{ChunkFileBaseName}{chunk + 1}.txt")

            Using writer As StreamWriter = File.CreateText(fileName)

                For index As Integer = 0 To splitSize - 1

                    If Not lineIterator.MoveNext() Then
                        Return False
                    End If

                    writer.WriteLine(lineIterator.Current)

                Next index
            End Using

            Return True

        End Function

        ''' <summary>
        ''' Verify the chunked files equal line count in the original file
        ''' </summary>
        ''' <param name="incomingFileName">The larger file</param>
        ''' <returns></returns>
        Public Shared Function VerifyLineCounts(incomingFileName As String) As List(Of Verify)
            Dim verifyList = New List(Of Verify)()

            Dim directory = New DirectoryInfo(ChunkFolderLocation)
            Dim files = directory.GetFiles("*.*", SearchOption.AllDirectories)

            Dim lineCount = 0
            Dim totalLines = 0

            For Each fileInfo As FileInfo In files
                Using reader = File.OpenText(Path.Combine(ChunkFolderLocation, fileInfo.Name))
                    Do While reader.ReadLine() IsNot Nothing
                        lineCount += 1
                    Loop
                End Using

                verifyList.Add(New Verify() With {.FileName = fileInfo.Name, .Count = lineCount})

                totalLines += lineCount
                lineCount = 0

            Next fileInfo

            '
            ' IMPORTANT: The chunk method appends _N to each of the smaller files
            ' which this statement expects so if the above method changes this must too.
            '
            verifyList = verifyList.Select(
                Function(verify) New With {
                      Key .Name = verify,
                      Key .Index = Convert.ToInt32(verify.FileName.Split("_"c)(1).
                                                      Replace(".txt", ""))}).
                OrderBy(Function(item) item.Index).
                Select(Function(anonymousItem) anonymousItem.Name).ToList()

            verifyList.Add(New Verify() With {.FileName = "Total", .Count = totalLines})

            lineCount = 0

            Dim baseFile = New FileInfo(incomingFileName)

            Using reader = File.OpenText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, baseFile.Name))
                Do While reader.ReadLine() IsNot Nothing
                    lineCount += 1
                Loop
            End Using

            verifyList.Add(New Verify() With {
                              .FileName = Path.GetFileName(incomingFileName),
                              .Count = lineCount})

            Return verifyList

        End Function
        ''' <summary>
        ''' Use for <see cref="RemoveZeroLengthFiles"/>
        ''' </summary>
        ''' <param name="path"></param>
        ''' <returns></returns>
        Private Shared Function GetFilesWithZeroLengthFiles(path As String) As String()
            Dim directory = New DirectoryInfo(path)
            Dim files = directory.GetFiles("*.*", SearchOption.AllDirectories)

            Return (
                From file In files
                Where file.Length = 0
                Select file.Name).ToArray()

        End Function
        ''' <summary>
        ''' We don't want any empty lines, this ensures there are no
        ''' empty lines
        ''' </summary>
        Public Shared Sub RemoveZeroLengthFiles()


            Dim files = GetFilesWithZeroLengthFiles(ChunkFolderLocation)

            For Each currentFile In files
                File.Delete(Path.Combine(ChunkFolderLocation, currentFile))
            Next

        End Sub
        ''' <summary>
        ''' The idea here is to take files in the chunk folder and one
        ''' by one process them where in this case there is no processing.
        '''
        ''' Processing can take many forms e.g. move to a work folder and parse
        ''' or perhaps move to a monitor folder which a windows service watches
        ''' and processes any file in that location. In this case a fictitious
        ''' Windows service looks for a known file which means any file here
        ''' is moved it always overwrites the current file the service is watching for.
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function CheckIfNewIncomingFileIsNeeded() As String

            Dim availableFiles = Directory.GetFiles(ChunkFolderLocation)

            Dim fileNamesOrdered = availableFiles.Select(
                Function(fName) New With {
                        Key .Name = fName,
                        Key .Index = Convert.ToInt32(fName.Split("_"c)(1).
                                                        Replace(".txt", ""))}).
                    OrderBy(Function(item) item.Index).ToArray()

            If fileNamesOrdered.Length > 0 Then
                Dim chunkFileName = Path.GetFileName(fileNamesOrdered(0).Name)

                '
                ' Process the file or move the file to another folder for processing
                ' then do the delete or not
                '

                Dim destinationFileName = Path.Combine(WorkFolderLocation, "Current.txt")
                If File.Exists(destinationFileName) Then
                    File.Delete(destinationFileName)
                End If

                File.Move(fileNamesOrdered(0).Name, destinationFileName)

                Return chunkFileName
            Else
                ' Signifies all files have been processed
                Return Nothing
            End If
        End Function
    End Class
End Namespace