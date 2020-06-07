Imports System.Configuration
Imports System.IO
Imports System.Reflection

Namespace Classes

    Public Class ApplicationSettings
        Public Shared Function GetLineSplit() As String
            Try
                Return ConfigurationManager.AppSettings("LinesToSplit")
            Catch e1 As Exception
                Return "2000"
            End Try
        End Function
        Public Shared Function GetChunkFolderLocation() As String
            Try
                Return ConfigurationManager.AppSettings("ChunkFolderLocation")
            Catch e1 As Exception
                Throw New Exception("Failed to read chunk folder location")
            End Try
        End Function
        Public Shared Function GetWorkFolderLocation() As String
            Try
                Return ConfigurationManager.AppSettings("WorkFilesLocation")
            Catch e1 As Exception
                Throw New Exception("Failed to read work folder location")
            End Try
        End Function
        Public Shared Function GetChunkFileBaseName() As String
            Try
                Return ConfigurationManager.AppSettings("ChunkFileBaseName")
            Catch e1 As Exception
                Throw New Exception("Failed to read chunk base file name")
            End Try
        End Function
        Public Shared Sub LinesToSplit(ByVal value As String)

            Try
                Dim applicationDirectoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                Dim configFile = Path.Combine(applicationDirectoryName,$"{Assembly.GetExecutingAssembly().GetName().Name}.exe.config")
                Dim configFileMap = New ExeConfigurationFileMap With {.ExeConfigFilename = configFile}
                Dim config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None)

                config.AppSettings.Settings("LinesToSplit").Value = value
                config.Save()

            Catch e1 As Exception
                ' ignored
            End Try
        End Sub
    End Class
End Namespace