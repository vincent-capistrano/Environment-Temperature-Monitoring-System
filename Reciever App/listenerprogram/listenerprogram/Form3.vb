Imports System.Data.SqlClient
Imports System.Diagnostics.Eventing.Reader
Imports System.IO
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Net.Sockets
Imports System.Reflection.Emit
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms.DataVisualization.Charting

Public Class Form3

    Private listener As TcpListener
    Private listenThread As Thread

    Dim DEVICENAMECONTAINS As String
    Dim DEVICENAMELENGTH As Integer
    Dim SLEEPINTERVAL As Integer
    Dim DISCONNECTIONTIMEOUT As Integer
    Dim CONNECTIONSTRING As String

    Dim connnnnn As New SqlConnection(My.Settings.CONSTRING)

    Dim myResizer As New Resizer

    Dim UPDATEDMSG As String

    Dim DAY As Integer = Date.Now.Day
    Dim MONTH As Integer = Date.Now.Month
    Dim YEAR As Integer = Date.Now.Year


    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        connnnnn.Open()

        myResizer.FindAllControls(Me)
        listener = New TcpListener(System.Net.IPAddress.Any, 8080)
        listenThread = New Thread(AddressOf ListenForClients)
        listenThread.Start()
        load_csv_file()
    End Sub
    Private sensorTimeouts As New Dictionary(Of String, DateTime)()
    Private disconnectedSensors As New List(Of String)()
    Private Sub ListenForClients()
        listener.Start()

        While True
            Dim client As TcpClient = listener.AcceptTcpClient()
            Dim clientThread As New Thread(AddressOf HandleClientComm)
            clientThread.Start(client)
        End While
    End Sub
    Private Sub HandleClientComm(client As Object)
        Try
            Dim tcpClient As TcpClient = CType(client, TcpClient)
            Dim clientStream As NetworkStream = tcpClient.GetStream()

            Dim message As Byte() = New Byte(4095) {}
            Dim bytesRead As Integer
            Dim receivedAllSignals As Boolean = False

            While True
                bytesRead = 0

                Try
                    bytesRead = clientStream.Read(message, 0, 4096)
                Catch ex As Exception
                    Exit While
                End Try

                If bytesRead = 0 Then
                    Exit While
                End If

                Dim encoder As New ASCIIEncoding()
                Dim msg As String = encoder.GetString(message, 0, bytesRead)

                If msg.Contains(DEVICENAMECONTAINS) AndAlso msg.Length = DEVICENAMELENGTH Then
                    Me.Invoke(Sub()
                                  UPDATEDMSG = msg & "," & Date.Now
                                  Dim parts() As String = UPDATEDMSG.Split(","c)

                                  If parts.Length = 4 Then
                                      Dim sensorName As String = parts(0)
                                      Dim temperature As String = parts(1)
                                      Dim humidity As String = parts(2)
                                      Dim signalReceived As String = parts(3)

                                      If disconnectedSensors.Contains(sensorName) Then

                                          disconnectedSensors.Remove(sensorName)

                                          For Each row As DataGridViewRow In DataGridView3.Rows
                                              If row.Cells("D_Name").Value IsNot Nothing AndAlso row.Cells("D_Name").Value.ToString() = sensorName Then
                                                  DataGridView3.Rows.Remove(row)
                                                  Exit For
                                              End If
                                          Next
                                          Console.WriteLine("Sensor " & sensorName & " reconnected.")

                                      End If

                                      sensorTimeouts(sensorName) = DateTime.Now


                                      DataGridView1.Rows.Add(sensorName, temperature, humidity, signalReceived)
                                      insert_to_db(sensorName, temperature, humidity, signalReceived)


                                      create_filename()
                                      SaveToCSV(UPDATEDMSG)
                                      clear_dgv1()

                                      If CheckIfAllSignalsReceived() Then
                                          receivedAllSignals = True
                                      End If
                                  End If

                              End Sub)
                End If


                If receivedAllSignals Then
                    System.Threading.Thread.Sleep(SLEEPINTERVAL)
                End If
                tcpClient.Close()

            End While

        Catch ex As Exception
            If Not Directory.Exists("ERROR_LOGS") Then
                Directory.CreateDirectory("ERROR_LOGS")
            End If

            If Not File.Exists(errorcsvFilePath) Then

                Using writer As New StreamWriter(errorcsvFilePath, False)
                    writer.WriteLine(Date.Now & " | An error occurred: " & ex.Message)
                End Using
            End If

            Using writer As New StreamWriter(errorcsvFilePath, True)
                writer.WriteLine(Date.Now & " | An error occurred: " & ex.Message)
            End Using
        End Try
    End Sub

    Private Sub insert_to_db(ByVal devicename As String, ByVal temp As String, ByVal hum As String, ByVal datetime As DateTime)

        Using COM As New SqlCommand("dbo.sp_PERMS_INSERT_LOGS", connnnnn)
            COM.CommandType = CommandType.StoredProcedure
            COM.Parameters.Add("@device_name", SqlDbType.NVarChar).Value = devicename
            COM.Parameters.Add("@temperature", SqlDbType.NVarChar).Value = temp
            COM.Parameters.Add("@humidity", SqlDbType.NVarChar).Value = hum
            COM.Parameters.Add("@date_time", SqlDbType.DateTime).Value = datetime
            COM.ExecuteNonQuery()
        End Using

    End Sub

    Private Sub online(ByVal devicename As String, ByVal datetime As DateTime)

        Using COM1 As New SqlCommand("dbo.sp_PERMS_reconnected", connnnnn)
            COM1.CommandType = CommandType.StoredProcedure
            COM1.Parameters.Add("@devicename", SqlDbType.NVarChar).Value = devicename
            COM1.Parameters.Add("@datetime", SqlDbType.DateTime).Value = datetime
            COM1.ExecuteNonQuery()
        End Using

    End Sub

    Private Sub offline(ByVal devicename As String, ByVal datetime As DateTime)

        Using COM2 As New SqlCommand("dbo.sp_PERMS_offline", connnnnn)
            COM2.CommandType = CommandType.StoredProcedure
            COM2.Parameters.Add("@devicename", SqlDbType.NVarChar).Value = devicename
            COM2.Parameters.Add("@datetime", SqlDbType.DateTime).Value = datetime
            COM2.ExecuteNonQuery()
        End Using

    End Sub



    Private Function CheckIfAllSignalsReceived() As Boolean
        ' scan datagridview2
        If DataGridView2.Rows.Count <> 0 Then
            For Each row As DataGridViewRow In DataGridView2.Rows
                If row.Cells(0).Value IsNot Nothing Then
                    Dim deviceName As String = row.Cells(0).Value.ToString()
                    If Not sensorTimeouts.ContainsKey(deviceName) Then
                        Return False
                    End If
                End If
            Next
            Return True
        End If
    End Function


    Private Sub clear_dgv1()
        If DataGridView1.Rows.Count = 25 Then
            DataGridView1.Rows.Clear()
        End If
    End Sub

    Private Sub CheckSensorTimeouts()

        Dim currentTime As DateTime = DateTime.Now

        For Each sensorName As String In sensorTimeouts.Keys.ToList()
            Dim lastReceivedTime As DateTime = sensorTimeouts(sensorName)

            If disconnectedSensors.Contains(sensorName) Then

                'Console.WriteLine("Sensor " & sensorName & " reconnected.")
            Else

                If (currentTime - lastReceivedTime).TotalSeconds <= DISCONNECTIONTIMEOUT Then
                    Dim found As Boolean = False
                    For Each row As DataGridViewRow In DataGridView2.Rows
                        If row.Cells("C_Name").Value IsNot Nothing AndAlso row.Cells("C_Name").Value.ToString() = sensorName Then
                            found = True
                            Exit For
                        End If
                    Next

                    If Not found Then
                        DataGridView2.Rows.Add(sensorName, currentTime)
                        online(sensorName, currentTime)
                    End If
                Else

                    If Not disconnectedSensors.Contains(sensorName) Then
                        Console.WriteLine("Sensor " & sensorName & " disconnected.")
                        disconnectedSensors.Add(sensorName)

                        For Each row As DataGridViewRow In DataGridView2.Rows
                            If row.Cells("C_Name").Value IsNot Nothing AndAlso row.Cells("C_Name").Value.ToString() = sensorName Then
                                DataGridView2.Rows.Remove(row)
                                Exit For
                            End If
                        Next

                        DataGridView3.Rows.Add(sensorName, currentTime)
                        offline(sensorName, currentTime)
                    End If
                End If
            End If
        Next
    End Sub

    Dim csvFilePath As String
    Dim errorcsvFilePath As String

    Private Sub create_filename()
        DAY = Date.Now.Day
        MONTH = Date.Now.Month
        YEAR = Date.Now.Year
        csvFilePath = "LOGS\" & MONTH & DAY & YEAR & "-data.csv"
        errorcsvFilePath = "ERROR_LOGS\" & MONTH & DAY & YEAR & ".csv"
    End Sub
    Public Sub load_csv_file()
        Dim filePath As String = Application.StartupPath & "\config.csv"
        Dim configValues As Dictionary(Of String, String) = Form2.ReadCSV(filePath)

        DEVICENAMECONTAINS = configValues("DEVICENAME")
        DEVICENAMELENGTH = configValues("DEVICENAMELENGTH")
        SLEEPINTERVAL = configValues("SLEEPINTERVAL")
        DISCONNECTIONTIMEOUT = configValues("DISCONNECTIONTIMEOUT")

    End Sub
    Private Sub SaveToCSV(data As String)
        Try

            If Not Directory.Exists("LOGS") Then
                Directory.CreateDirectory("LOGS")
            End If


            If Not File.Exists(csvFilePath) Then

                Using writer As New StreamWriter(csvFilePath, False)
                    writer.WriteLine("DEVICENAME,TEMPERATURE,HUMIDITY,DATETIME")
                End Using
            End If


            Using writer As New StreamWriter(csvFilePath, True)
                writer.WriteLine(data)
            End Using
        Catch ex As Exception

            'Console.WriteLine("An error occurred: " & ex.Message)
            If Not Directory.Exists("ERROR_LOGS") Then
                Directory.CreateDirectory("ERROR_LOGS")
            End If


            If Not File.Exists(errorcsvFilePath) Then

                Using writer As New StreamWriter(errorcsvFilePath, False)
                    writer.WriteLine(Date.Now & " | An error occurred: " & ex.Message)
                End Using
            End If


            Using writer As New StreamWriter(errorcsvFilePath, True)
                writer.WriteLine(Date.Now & " | An error occurred: " & ex.Message)
            End Using

        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        CheckSensorTimeouts()
    End Sub

    Private Sub Form3_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        myResizer.ResizeAllControls(Me)
    End Sub
End Class