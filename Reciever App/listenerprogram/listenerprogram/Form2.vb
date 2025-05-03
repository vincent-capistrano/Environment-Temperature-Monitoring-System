Imports System.IO
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Net.Sockets
Imports System.Reflection.Emit
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms.DataVisualization.Charting

Public Class Form2
    Private listener As TcpListener
    Private listenThread As Thread

    Dim SENSORNAME1 As String
    Dim SENSORNAME2 As String
    Dim SENSORNAME3 As String
    Dim SENSORNAME4 As String
    Dim SENSORNAME5 As String
    Dim SENSORNAME6 As String
    Dim SENSORNAME7 As String
    Dim SENSORNAME8 As String
    Dim SENSORNAME9 As String
    Dim SENSORNAME10 As String

    Dim SENSORIP1 As String
    Dim SENSORIP2 As String
    Dim SENSORIP3 As String
    Dim SENSORIP4 As String
    Dim SENSORIP5 As String
    Dim SENSORIP6 As String
    Dim SENSORIP7 As String
    Dim SENSORIP8 As String
    Dim SENSORIP9 As String
    Dim SENSORIP10 As String

    Dim LOCATION1 As String
    Dim LOCATION2 As String
    Dim LOCATION3 As String
    Dim LOCATION4 As String
    Dim LOCATION5 As String
    Dim LOCATION6 As String
    Dim LOCATION7 As String
    Dim LOCATION8 As String
    Dim LOCATION9 As String
    Dim LOCATION10 As String

    Dim CODE1 As String
    Dim CODE2 As String
    Dim CODE3 As String

    Dim sensorName As String
    Dim temperature As String
    Dim humidity As String
    Dim signalReceived As String

    Dim UPDATEDMSG As String


    Dim TEMPUPPERLIMIT As Integer
    Dim TEMPLOWERLIMIT As Integer
    Dim HUMIDITYUPPERLIMIT As Integer
    Dim HUMIDITYLOWERLIMIT As Integer

    Dim TEMPBENCHMARK As Integer
    Dim HUMIDITYBENCHMARK As Integer



    Dim DAY As Integer = Date.Now.Day
    Dim MONTH As Integer = Date.Now.Month
    Dim YEAR As Integer = Date.Now.Year

    Dim myResizer As New Resizer

    Public Shared Function ReadCSV(filePath As String) As Dictionary(Of String, String)
        Dim configValues As New Dictionary(Of String, String)
        If File.Exists(filePath) Then
            Dim lines As String() = File.ReadAllLines(filePath)
            For Each line As String In lines
                Dim parts As String() = line.Split(","c)
                If parts.Length = 2 Then
                    configValues(parts(0).Trim()) = parts(1).Trim()
                End If
            Next
        Else
            MsgBox("CONFIG FILE DOES NOT EXIST IN: " & filePath)
        End If
        Return configValues
    End Function


    Public Sub load_csv_file()
        Dim filePath As String = Application.StartupPath & "\config.csv"
        Dim configValues As Dictionary(Of String, String) = Form2.ReadCSV(filePath)

        SENSORNAME1 = configValues("SENSOR1")
        SENSORNAME2 = configValues("SENSOR2")
        SENSORNAME3 = configValues("SENSOR3")
        SENSORNAME4 = configValues("SENSOR4")
        SENSORNAME5 = configValues("SENSOR5")
        SENSORNAME6 = configValues("SENSOR6")
        SENSORNAME7 = configValues("SENSOR7")
        SENSORNAME8 = configValues("SENSOR8")
        SENSORNAME9 = configValues("SENSOR9")
        SENSORNAME10 = configValues("SENSOR10")

        SENSORIP1 = configValues("SENSORIP1")
        SENSORIP2 = configValues("SENSORIP2")
        SENSORIP3 = configValues("SENSORIP3")
        SENSORIP4 = configValues("SENSORIP4")
        SENSORIP5 = configValues("SENSORIP5")
        SENSORIP6 = configValues("SENSORIP6")
        SENSORIP7 = configValues("SENSORIP7")
        SENSORIP8 = configValues("SENSORIP8")
        SENSORIP9 = configValues("SENSORIP9")
        SENSORIP10 = configValues("SENSORIP10")

        LOCATION1 = configValues("LOCATION_S1")
        LOCATION2 = configValues("LOCATION_S2")
        LOCATION3 = configValues("LOCATION_S3")
        LOCATION4 = configValues("LOCATION_S4")
        LOCATION5 = configValues("LOCATION_S5")
        LOCATION6 = configValues("LOCATION_S6")
        LOCATION7 = configValues("LOCATION_S7")
        LOCATION8 = configValues("LOCATION_S8")
        LOCATION9 = configValues("LOCATION_S9")
        LOCATION10 = configValues("LOCATION_S10")

        CODE1 = configValues("CODE1")
        CODE2 = configValues("CODE2")
        CODE3 = configValues("CODE3")

        TEMPUPPERLIMIT = configValues("TEMPUPPERLIMIT")
        TEMPLOWERLIMIT = configValues("TEMPLOWERLIMIT")
        HUMIDITYUPPERLIMIT = configValues("HUMIDITYUPPERLIMIT")
        HUMIDITYLOWERLIMIT = configValues("HUMIDITYLOWERLIMIT")

        TEMPBENCHMARK = configValues("TEMPBENCHMARK")
        HUMIDITYBENCHMARK = configValues("HUMIDITYBENCHMARK")

        'LBLTEMP1.Text = TEMPUPPERLIMIT & TEMPBENCHMARK & TEMPLOWERLIMIT
        'LBLTEMP2.Text = TEMPUPPERLIMIT & TEMPBENCHMARK & TEMPLOWERLIMIT
        'LBLTEMP3.Text = TEMPUPPERLIMIT & TEMPBENCHMARK & TEMPLOWERLIMIT
        'LBLTEMP4.Text = TEMPUPPERLIMIT & TEMPBENCHMARK & TEMPLOWERLIMIT
        'LBLTEMP5.Text = TEMPUPPERLIMIT & TEMPBENCHMARK & TEMPLOWERLIMIT
        'LBLTEMP6.Text = TEMPUPPERLIMIT & TEMPBENCHMARK & TEMPLOWERLIMIT
        'LBLTEMP7.Text = TEMPUPPERLIMIT & TEMPBENCHMARK & TEMPLOWERLIMIT
        'LBLTEMP8.Text = TEMPUPPERLIMIT & TEMPBENCHMARK & TEMPLOWERLIMIT
        'LBLTEMP9.Text = TEMPUPPERLIMIT & TEMPBENCHMARK & TEMPLOWERLIMIT
        'LBLTEMP10.Text = TEMPUPPERLIMIT & TEMPBENCHMARK & TEMPLOWERLIMIT


        'LBLHUM1.Text = HUMIDITYUPPERLIMIT & HUMIDITYBENCHMARK & HUMIDITYLOWERLIMIT
        'LBLHUM2.Text = HUMIDITYUPPERLIMIT & HUMIDITYBENCHMARK & HUMIDITYLOWERLIMIT
        'LBLHUM3.Text = HUMIDITYUPPERLIMIT & HUMIDITYBENCHMARK & HUMIDITYLOWERLIMIT
        'LBLHUM4.Text = HUMIDITYUPPERLIMIT & HUMIDITYBENCHMARK & HUMIDITYLOWERLIMIT
        'LBLHUM5.Text = HUMIDITYUPPERLIMIT & HUMIDITYBENCHMARK & HUMIDITYLOWERLIMIT
        'LBLHUM6.Text = HUMIDITYUPPERLIMIT & HUMIDITYBENCHMARK & HUMIDITYLOWERLIMIT
        'LBLHUM7.Text = HUMIDITYUPPERLIMIT & HUMIDITYBENCHMARK & HUMIDITYLOWERLIMIT
        'LBLHUM8.Text = HUMIDITYUPPERLIMIT & HUMIDITYBENCHMARK & HUMIDITYLOWERLIMIT
        'LBLHUM9.Text = HUMIDITYUPPERLIMIT & HUMIDITYBENCHMARK & HUMIDITYLOWERLIMIT
        'LBLHUM10.Text = HUMIDITYUPPERLIMIT & HUMIDITYBENCHMARK & HUMIDITYLOWERLIMIT


    End Sub





    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        myResizer.FindAllControls(Me)
        listener = New TcpListener(System.Net.IPAddress.Any, 8080)
        listenThread = New Thread(AddressOf ListenForClients)
        listenThread.Start()
        load_csv_file()
    End Sub

    Private Sub ListenForClients()
        listener.Start()

        While True
            Dim client As TcpClient = listener.AcceptTcpClient()
            Dim clientThread As New Thread(AddressOf HandleClientComm)
            clientThread.Start(client)
        End While
    End Sub

    'Private Sub HandleClientComm(client As Object)
    '    Dim tcpClient As TcpClient = CType(client, TcpClient)
    '    Dim clientStream As NetworkStream = tcpClient.GetStream()

    '    Dim message As Byte() = New Byte(4095) {}
    '    Dim bytesRead As Integer

    '    While True
    '        bytesRead = 0

    '        Try
    '            bytesRead = clientStream.Read(message, 0, 4096)
    '        Catch ex As Exception

    '            Exit While
    '        End Try

    '        If bytesRead = 0 Then
    '            Exit While
    '        End If

    '        Dim encoder As New ASCIIEncoding()
    '        Dim msg As String = encoder.GetString(message, 0, bytesRead)

    '        ' Only process messages containing "SENSOR #"

    '        If msg.Contains("SENSOR#") And msg.Length = 20 Then


    '            Me.Invoke(Sub()
    '                          UPDATEDMSG = msg & "," & Date.Now
    '                          Dim parts() As String = UPDATEDMSG.Split(","c)

    '                          If parts.Length = 4 Then
    '                              sensorName = parts(0)
    '                              temperature = parts(1)
    '                              humidity = parts(2)
    '                              signalReceived = parts(3)

    '                              SaveToCSV(UPDATEDMSG)
    '                              ListBox1.Items.Add(UPDATEDMSG)
    '                              ListBox1.TopIndex = ListBox1.Items.Count - 1
    '                              DISPLAYTOFORM()
    '                          End If

    '                      End Sub)
    '        End If

    '    End While

    '    tcpClient.Close()

    'End Sub
    Private sensorTimeouts As New Dictionary(Of String, DateTime)()
    Private disconnectedSensors As New List(Of String)()

    Private Sub HandleClientComm(client As Object)
        Try
            Dim tcpClient As TcpClient = CType(client, TcpClient)
            Dim clientStream As NetworkStream = tcpClient.GetStream()

            Dim message As Byte() = New Byte(4095) {}
            Dim bytesRead As Integer

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

                ' Only process messages containing "SENSOR #"
                If msg.Contains("SENSOR#") AndAlso msg.Length = 20 Then
                    Me.Invoke(Sub()
                                  UPDATEDMSG = msg & "," & Date.Now
                                  Dim parts() As String = UPDATEDMSG.Split(","c)

                                  If parts.Length = 4 Then
                                      Dim sensorName As String = parts(0)
                                      Dim temperature As String = parts(1)
                                      Dim humidity As String = parts(2)
                                      Dim signalReceived As String = parts(3)

                                      ' Check if sensor was previously disconnected
                                      If disconnectedSensors.Contains(sensorName) Then
                                          ' Remove from disconnected list
                                          disconnectedSensors.Remove(sensorName)
                                          Console.WriteLine("Sensor " & sensorName & " reconnected.")
                                          For Each ctrl As Control In Me.Controls
                                              If TypeOf ctrl Is GroupBox AndAlso DirectCast(ctrl, GroupBox).Text = sensorName Then
                                                  ctrl.BackColor = Color.Teal
                                              End If
                                          Next
                                      End If

                                      ' Update the last received time for the sensor
                                      sensorTimeouts(sensorName) = DateTime.Now


                                      create_filename()
                                      SaveToCSV(UPDATEDMSG)
                                      ListBox1.Items.Add(UPDATEDMSG)
                                      ListBox1.TopIndex = ListBox1.Items.Count - 1
                                      DISPLAYTOFORM()
                                      loadtograph()
                                  End If
                              End Sub)
                End If
                loadtograph()
            End While

            tcpClient.Close()
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

    Dim csvFilePath As String
    Dim errorcsvFilePath As String
    Private Sub create_filename()
        DAY = Date.Now.Day
        MONTH = Date.Now.Month
        YEAR = Date.Now.Year
        csvFilePath = "LOGS\" & MONTH & DAY & YEAR & "-data.csv"
        errorcsvFilePath = "ERROR_LOGS\" & MONTH & DAY & YEAR & ".csv"
    End Sub

    Private Sub CheckSensorTimeouts()
        Dim currentTime As DateTime = DateTime.Now


        For Each sensorName As String In sensorTimeouts.Keys.ToList()
            Dim lastReceivedTime As DateTime = sensorTimeouts(sensorName)

            ' kapag more than 10 secs na disconnected mark as disconnected
            If (currentTime - lastReceivedTime).TotalSeconds > 10 AndAlso Not disconnectedSensors.Contains(sensorName) Then

                Console.WriteLine("Sensor " & sensorName & " disconnected.")

                For Each ctrl As Control In Me.Controls
                    ' hanapin yung groupbox based on the groupbox.text which is sensor name :> 
                    If TypeOf ctrl Is GroupBox AndAlso DirectCast(ctrl, GroupBox).Text = sensorName Then
                        ' Change the backcolor of the control
                        ctrl.BackColor = Color.Firebrick
                    End If
                Next
                disconnectedSensors.Add(sensorName)


            End If
        Next
    End Sub

    ' Call CheckSensorTimeouts every second using a timer
    Dim a As Integer
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick




        CheckSensorTimeouts()

        a = a + 1
        If a = 60 Then
            'sensorTimeouts.Clear()
            'disconnectedSensors.Clear()
            ListBox1.Items.Clear()
            a = 0
        End If


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

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        If listener IsNot Nothing Then listener.Stop()
        If listenThread IsNot Nothing AndAlso listenThread.IsAlive Then listenThread.Abort()
    End Sub



    Private Sub DISPLAYTOFORM()
        Dim parts() As String = UPDATEDMSG.Split(","c)

        If parts.Length = 4 Then
            sensorName = parts(0)
            temperature = parts(1)
            humidity = parts(2)
            signalReceived = parts(3)
        End If

        If sensorName = SENSORNAME1 Then
            GroupBox1.Text = SENSORNAME1
            Label1.Text = LOCATION1
            Label2.Text = temperature
            Label3.Text = humidity
            Label4.Text = signalReceived
            '  GroupBox1.BackColor = DEVICE_CONNECTED(SENSORNAME1)
        ElseIf sensorName = SENSORNAME2 Then
            GroupBox2.Text = SENSORNAME2
            Label8.Text = LOCATION2
            Label7.Text = temperature
            Label6.Text = humidity
            Label5.Text = signalReceived
            '  GroupBox2.BackColor = DEVICE_CONNECTED(SENSORNAME2)
        ElseIf sensorName = SENSORNAME3 Then
            GroupBox3.Text = SENSORNAME3
            Label12.Text = LOCATION3
            Label11.Text = temperature
            Label10.Text = humidity
            Label9.Text = signalReceived
            '   GroupBox3.BackColor = DEVICE_CONNECTED(SENSORNAME3)
        ElseIf sensorName = SENSORNAME4 Then
            GroupBox4.Text = SENSORNAME4
            Label16.Text = LOCATION4
            Label15.Text = temperature
            Label14.Text = humidity
            Label13.Text = signalReceived
            '  GroupBox4.BackColor = DEVICE_CONNECTED(SENSORNAME4)
        ElseIf sensorName = SENSORNAME5 Then
            GroupBox5.Text = SENSORNAME5
            Label20.Text = LOCATION5
            Label19.Text = temperature
            Label18.Text = humidity
            Label17.Text = signalReceived
            '  GroupBox5.BackColor = DEVICE_CONNECTED(SENSORNAME5)
        ElseIf sensorName = SENSORNAME6 Then
            GroupBox6.Text = SENSORNAME6
            Label24.Text = LOCATION6
            Label23.Text = temperature
            Label22.Text = humidity
            Label21.Text = signalReceived
            '  GroupBox6.BackColor = DEVICE_CONNECTED(SENSORNAME6)
        ElseIf sensorName = SENSORNAME7 Then
            GroupBox7.Text = SENSORNAME7
            Label28.Text = LOCATION7
            Label27.Text = temperature
            Label26.Text = humidity
            Label25.Text = signalReceived
            ' GroupBox7.BackColor = DEVICE_CONNECTED(SENSORNAME7)
        ElseIf sensorName = SENSORNAME8 Then
            GroupBox8.Text = SENSORNAME8
            Label32.Text = LOCATION8
            Label31.Text = temperature
            Label30.Text = humidity
            Label29.Text = signalReceived
            '  GroupBox8.BackColor = DEVICE_CONNECTED(SENSORNAME8)
        ElseIf sensorName = SENSORNAME9 Then
            GroupBox9.Text = SENSORNAME9
            Label36.Text = LOCATION9
            Label35.Text = temperature
            Label34.Text = humidity
            Label33.Text = signalReceived
            '  GroupBox9.BackColor = DEVICE_CONNECTED(SENSORNAME9)
        ElseIf sensorName = SENSORNAME10 Then
            GroupBox10.Text = SENSORNAME10
            Label40.Text = LOCATION10
            Label39.Text = temperature
            Label38.Text = humidity
            Label37.Text = signalReceived
            ' GroupBox10.BackColor = DEVICE_CONNECTED(SENSORNAME10)
        End If
    End Sub

    Private Sub loadtograph()
        Using fs As FileStream = New FileStream("LOGS\" & MONTH & DAY & YEAR & "-data.csv", FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            Using sr As StreamReader = New StreamReader(fs)

                ' Read all lines from the CSV file
                Dim allLines As List(Of String) = New List(Of String)()
                While Not sr.EndOfStream
                    allLines.Add(sr.ReadLine())
                End While

                ' Get the latest 1000 lines from csv :)  --------------------------@@@@@@@@@@@@
                Dim startIndex As Integer = Math.Max(0, allLines.Count - 1000)
                Dim latestLines As String() = allLines.Skip(startIndex).ToArray()

                ' Initialize variables to store sensor data------------------------------------@@@@@@@@@@
                Dim sensor1Temps As New List(Of Double)()
                Dim sensor1Humidities As New List(Of Double)()
                Dim sensor2Temps As New List(Of Double)()
                Dim sensor2Humidities As New List(Of Double)()
                Dim sensor3Temps As New List(Of Double)()
                Dim sensor3Humidities As New List(Of Double)()
                Dim sensor4Temps As New List(Of Double)()
                Dim sensor4Humidities As New List(Of Double)()
                Dim sensor5Temps As New List(Of Double)()
                Dim sensor5Humidities As New List(Of Double)()
                Dim sensor6Temps As New List(Of Double)()
                Dim sensor6Humidities As New List(Of Double)()
                Dim sensor7Temps As New List(Of Double)()
                Dim sensor7Humidities As New List(Of Double)()
                Dim sensor8Temps As New List(Of Double)()
                Dim sensor8Humidities As New List(Of Double)()
                Dim sensor9Temps As New List(Of Double)()
                Dim sensor9Humidities As New List(Of Double)()
                Dim sensor10Temps As New List(Of Double)()
                Dim sensor10Humidities As New List(Of Double)()

                'place data from each line--------------------------------------------------@@@@@@@@@@@@@@@@@@@@@@@
                For Each line As String In latestLines
                    If line = "DEVICENAME,TEMPERATURE,HUMIDITY,DATETIME" Then
                        ' Console.WriteLine("Skip 1st row")
                    Else
                        Dim parts As String() = line.Split(","c)
                        Dim sensorId As String = parts(0)
                        Dim temperature As Double = Double.Parse(parts(1))
                        Dim humidity As Double = Double.Parse(parts(2))

                        Select Case sensorId
                            Case "SENSOR#1"
                                sensor1Temps.Add(temperature)
                                sensor1Humidities.Add(humidity)
                            Case "SENSOR#2"
                                sensor2Temps.Add(temperature)
                                sensor2Humidities.Add(humidity)
                            Case "SENSOR#3"
                                sensor3Temps.Add(temperature)
                                sensor3Humidities.Add(humidity)
                            Case "SENSOR#4"
                                sensor4Temps.Add(temperature)
                                sensor4Humidities.Add(humidity)
                            Case "SENSOR#5"
                                sensor5Temps.Add(temperature)
                                sensor5Humidities.Add(humidity)
                            Case "SENSOR#6"
                                sensor6Temps.Add(temperature)
                                sensor6Humidities.Add(humidity)
                            Case "SENSOR#7"
                                sensor7Temps.Add(temperature)
                                sensor7Humidities.Add(humidity)
                            Case "SENSOR#8"
                                sensor8Temps.Add(temperature)
                                sensor8Humidities.Add(humidity)
                            Case "SENSOR#9"
                                sensor9Temps.Add(temperature)
                                sensor9Humidities.Add(humidity)
                            Case "SENSOR#10"
                                sensor10Temps.Add(temperature)
                                sensor10Humidities.Add(humidity)
                        End Select
                    End If
                Next

                'create charts and add data--------------------------------------------------------------------------
                Me.Invoke(Sub() AddTempDataToChart(sensor1Temps, Chart1))
                Me.Invoke(Sub() AddTempDataToChart(sensor2Temps, Chart2))
                Me.Invoke(Sub() AddTempDataToChart(sensor3Temps, Chart3))
                Me.Invoke(Sub() AddTempDataToChart(sensor4Temps, Chart4))
                Me.Invoke(Sub() AddTempDataToChart(sensor5Temps, Chart5))
                Me.Invoke(Sub() AddTempDataToChart(sensor6Temps, Chart6))
                Me.Invoke(Sub() AddTempDataToChart(sensor7Temps, Chart7))
                Me.Invoke(Sub() AddTempDataToChart(sensor8Temps, Chart8))
                Me.Invoke(Sub() AddTempDataToChart(sensor9Temps, Chart9))
                Me.Invoke(Sub() AddTempDataToChart(sensor10Temps, Chart10))


                Me.Invoke(Sub() AddHumDataToChart(sensor1Humidities, Chart11))
                Me.Invoke(Sub() AddHumDataToChart(sensor2Humidities, Chart12))
                Me.Invoke(Sub() AddHumDataToChart(sensor3Humidities, Chart13))
                Me.Invoke(Sub() AddHumDataToChart(sensor4Humidities, Chart14))
                Me.Invoke(Sub() AddHumDataToChart(sensor5Humidities, Chart15))
                Me.Invoke(Sub() AddHumDataToChart(sensor6Humidities, Chart16))
                Me.Invoke(Sub() AddHumDataToChart(sensor7Humidities, Chart17))
                Me.Invoke(Sub() AddHumDataToChart(sensor8Humidities, Chart18))
                Me.Invoke(Sub() AddHumDataToChart(sensor9Humidities, Chart19))
                Me.Invoke(Sub() AddHumDataToChart(sensor10Humidities, Chart20))

                'lower limits for temperature and humidity-------------------------------------------
                SetLimits(Chart1)
                SetLimits(Chart2)
                SetLimits(Chart3)
                SetLimits(Chart4)
                SetLimits(Chart5)
                SetLimits(Chart6)
                SetLimits(Chart7)
                SetLimits(Chart8)
                SetLimits(Chart9)
                SetLimits(Chart10)

                SetLimits1(Chart11)
                SetLimits1(Chart12)
                SetLimits1(Chart13)
                SetLimits1(Chart14)
                SetLimits1(Chart15)
                SetLimits1(Chart16)
                SetLimits1(Chart17)
                SetLimits1(Chart18)
                SetLimits1(Chart19)
                SetLimits1(Chart20)



            End Using
        End Using

    End Sub

    Private Sub AddTempDataToChart(temps As List(Of Double), chart As Chart)
        chart.Series.Clear()

        Dim tempSeries As New Series("Temperature")
        Dim tempUpperLimitSeries As New Series("Temperature Upper Limit")
        Dim tempLowerLimitSeries As New Series("Temperature Lower Limit")

        For i As Integer = TEMPLOWERLIMIT To temps.Count - 1
            tempSeries.Points.AddXY(i, temps(i))
            tempUpperLimitSeries.Points.AddXY(i, TEMPUPPERLIMIT) ' Upper limit for temperature
            tempLowerLimitSeries.Points.AddXY(i, TEMPLOWERLIMIT) ' Lower limit for temperature
        Next

        tempSeries.ChartType = SeriesChartType.Line
        tempUpperLimitSeries.ChartType = SeriesChartType.Line
        tempLowerLimitSeries.ChartType = SeriesChartType.Line

        tempSeries.Color = Color.Black
        tempSeries.BorderWidth = 1
        tempUpperLimitSeries.Color = Color.Red
        tempLowerLimitSeries.Color = Color.Blue

        chart.Series.Add(tempUpperLimitSeries)
        chart.Series.Add(tempSeries)
        chart.Series.Add(tempLowerLimitSeries)

        chart.Series("Temperature").IsVisibleInLegend = False
        chart.Series("Temperature Upper Limit").LegendText = "Temperature Upper Limit"
        chart.Series("Temperature Lower Limit").LegendText = "Temperature Lower Limit"

    End Sub

    Private Sub AddHumDataToChart(humidities As List(Of Double), chart As Chart)
        chart.Series.Clear()

        Dim humiditySeries As New Series("Temperature")
        Dim humidityUpperLimitSeries As New Series("Temperature Upper Limit")
        Dim humidityLowerLimitSeries As New Series("Temperature Lower Limit")

        For i As Integer = HUMIDITYLOWERLIMIT To humidities.Count - 1
            humiditySeries.Points.AddXY(i, humidities(i))
            humidityUpperLimitSeries.Points.AddXY(i, HUMIDITYUPPERLIMIT) ' Upper limit for temperature
            humidityLowerLimitSeries.Points.AddXY(i, HUMIDITYLOWERLIMIT) ' Lower limit for temperature
        Next

        humiditySeries.ChartType = SeriesChartType.Line
        humidityUpperLimitSeries.ChartType = SeriesChartType.Line
        humidityLowerLimitSeries.ChartType = SeriesChartType.Line

        humiditySeries.Color = Color.Black
        humiditySeries.BorderWidth = 1
        humidityUpperLimitSeries.Color = Color.Red
        humidityLowerLimitSeries.Color = Color.Blue

        chart.Series.Add(humidityUpperLimitSeries)
        chart.Series.Add(humiditySeries)
        chart.Series.Add(humidityLowerLimitSeries)

        chart.Series("Temperature").IsVisibleInLegend = True
        chart.Series("Temperature Upper Limit").LegendText = "Temperature Upper Limit"
        chart.Series("Temperature Lower Limit").LegendText = "Temperature Lower Limit"

    End Sub




    Private Sub SetLimits(chart As Chart)
        Me.Invoke(Sub()
                      chart.ChartAreas(0).AxisY.MajorGrid.Enabled = False
                      chart.ChartAreas(0).AxisY2.MajorGrid.Enabled = False
                      chart.ChartAreas(0).AxisX.MajorGrid.Enabled = False
                      chart.ChartAreas(0).AxisX2.MajorGrid.Enabled = False


                      'chart.ChartAreas(0).AxisY2.Maximum = tem ' Upper limit for humidity
                      chart.ChartAreas(0).AxisY2.Minimum = TEMPLOWERLIMIT ' Lower limit for humidity


                      ' Hide Y-axis labels
                      chart.ChartAreas(0).AxisY.IsLabelAutoFit = True
                      chart.ChartAreas(0).AxisY.LabelStyle.Enabled = True
                      chart.ChartAreas(0).AxisY2.IsLabelAutoFit = False
                      chart.ChartAreas(0).AxisY2.LabelStyle.Enabled = False

                      ' Hide X-axis labels
                      chart.ChartAreas(0).AxisX.IsLabelAutoFit = False
                      chart.ChartAreas(0).AxisX.LabelStyle.Enabled = False
                      chart.ChartAreas(0).AxisX2.IsLabelAutoFit = False
                      chart.ChartAreas(0).AxisX2.LabelStyle.Enabled = False

                      chart.ChartAreas(0).AxisY.Interval = 10
                      chart.ChartAreas(0).AxisY.IntervalOffset = TEMPLOWERLIMIT
                      chart.ChartAreas(0).AxisY.LabelStyle.Font = New Font("Arial", 8, FontStyle.Bold)
                  End Sub)
    End Sub

    Private Sub SetLimits1(chart As Chart)
        Me.Invoke(Sub()
                      chart.ChartAreas(0).AxisY.MajorGrid.Enabled = False
                      chart.ChartAreas(0).AxisY2.MajorGrid.Enabled = False
                      chart.ChartAreas(0).AxisX.MajorGrid.Enabled = False
                      chart.ChartAreas(0).AxisX2.MajorGrid.Enabled = False


                      'chart.ChartAreas(0).AxisY2.Maximum = HUMIDITYUPPERLIMIT ' Upper limit for humidity
                      chart.ChartAreas(0).AxisY2.Minimum = HUMIDITYLOWERLIMIT ' Lower limit for humidity


                      ' Hide Y-axis labels
                      chart.ChartAreas(0).AxisY.IsLabelAutoFit = True
                      chart.ChartAreas(0).AxisY.LabelStyle.Enabled = True
                      chart.ChartAreas(0).AxisY2.IsLabelAutoFit = False
                      chart.ChartAreas(0).AxisY2.LabelStyle.Enabled = False

                      ' Hide X-axis labels
                      chart.ChartAreas(0).AxisX.IsLabelAutoFit = False
                      chart.ChartAreas(0).AxisX.LabelStyle.Enabled = False
                      chart.ChartAreas(0).AxisX2.IsLabelAutoFit = False
                      chart.ChartAreas(0).AxisX2.LabelStyle.Enabled = False

                      chart.ChartAreas(0).AxisY.Interval = 25
                      chart.ChartAreas(0).AxisY.IntervalOffset = HUMIDITYLOWERLIMIT
                      chart.ChartAreas(0).AxisY.LabelStyle.Font = New Font("Arial", 8, FontStyle.Bold)

                  End Sub)
    End Sub

    Private Sub Form2_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        myResizer.ResizeAllControls(Me)
    End Sub

    Private Sub Label28_Click(sender As Object, e As EventArgs) Handles Label28.Click

    End Sub

    Private Sub Label8_Click(sender As Object, e As EventArgs) Handles Label8.Click

    End Sub

    Private Sub Label12_Click(sender As Object, e As EventArgs) Handles Label12.Click

    End Sub

    Private Sub Label40_Click(sender As Object, e As EventArgs) Handles Label40.Click

    End Sub

    Private Sub Label16_Click(sender As Object, e As EventArgs) Handles Label16.Click

    End Sub

    Private Sub Label36_Click(sender As Object, e As EventArgs) Handles Label36.Click

    End Sub

    Private Sub Label24_Click(sender As Object, e As EventArgs) Handles Label24.Click

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Label32_Click(sender As Object, e As EventArgs) Handles Label32.Click

    End Sub
End Class