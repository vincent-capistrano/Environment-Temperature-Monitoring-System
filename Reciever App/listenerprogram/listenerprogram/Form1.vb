'Imports System.Data.SqlClient
'Imports System.IO
'Imports System.IO.Ports
'Imports System.Net
'Imports System.Net.Sockets
'Imports System.Text

'Public Class Form1

'    'Dim WithEvents serialPort As New SerialPort(My.Settings.CONSTRING, My.Settings.BOUDRATE) ' Change the port name to match your Arduino
'    Dim csvFilePath As String = "data.csv"



'    Private listener As TcpListener
'    Private client As TcpClient
'    Private stream As NetworkStream
'    '  Dim constring As New SqlConnection("Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MyLocalDB;Integrated Security=True")

'    'Private Sub constate()
'    '    If constring.State = ConnectionState.Open Then
'    '        constring.Close()
'    '    Else
'    '        constring.Open()
'    '    End If
'    'End Sub

'    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        listener = New TcpListener(IPAddress.Any, 8080) ' Port should match with ESP32 code
'        listener.Start()
'        listener.BeginAcceptTcpClient(AddressOf OnClientConnected, Nothing)
'    End Sub

'    Private Sub OnClientConnected(ar As IAsyncResult)
'        client = listener.EndAcceptTcpClient(ar)
'        stream = client.GetStream()
'        Dim buffer(1024) As Byte
'        stream.BeginRead(buffer, 0, buffer.Length, AddressOf OnDataReceived, buffer)
'    End Sub

'    Private Sub OnDataReceived(ar As IAsyncResult)
'        Dim buffer() As Byte = CType(ar.AsyncState, Byte())
'        Dim bytesRead As Integer = stream.EndRead(ar)
'        If bytesRead > 0 Then
'            Dim receivedText As String = Encoding.ASCII.GetString(buffer, 0, bytesRead)
'            Invoke(New Action(Sub() ListBox1.Items.Add(receivedText & Environment.NewLine)))
'            stream.BeginRead(buffer, 0, buffer.Length, AddressOf OnDataReceived, buffer)
'        End If
'    End Sub


'    Private Sub serialPort_DataReceived(sender As Object, e As SerialDataReceivedEventArgs) Handles serialPort.DataReceived
'        Dim receivedData As String = serialPort.ReadLine()
'        AddToListBox(receivedData & " | " & Date.Now)
'        SaveToCSV(receivedData & " | " & Date.Now)
'    End Sub

'    Private Sub AddToListBox(data As String)
'        ListBox1.Invoke(Sub()
'                            ListBox1.Items.Add(data)
'                            ListBox1.TopIndex = ListBox1.Items.Count - 1 ' Scroll to the latest item
'                        End Sub)

'        Try

'            'constring.Open()
'            'If data = "Error opening serial port: The port 'COM3' does not exist." Then
'            '    data = "COM3 does not exist."
'            'End If
'            'Using com As New SqlCommand("INSERT INTO [dbo].[logs_tbl] ([logs]) VALUES ('" & data & "')", constring)
'            '    Dim sa As New SqlDataAdapter(com)
'            '    com.ExecuteNonQuery()
'            '    constring.Close()
'            'End Using

'        Catch ex As Exception
'            ListBox1.Items.Add(ex.ToString & " | " & Date.Now)
'        End Try



'    End Sub

'    Private Sub SaveToCSV(data As String)
'        Try
'            Using writer As New StreamWriter(csvFilePath, True)
'                writer.WriteLine(data)
'            End Using
'        Catch ex As Exception
'            AddToListBox("Error saving data to CSV: " & ex.Message & " | " & Date.Now)
'        End Try
'    End Sub

'    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
'        If serialPort.IsOpen Then
'            serialPort.Close()

'        End If
'        AddToListBox("APPLICATION CLOSED!." & " | " & Date.Now)
'    End Sub

'    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
'        If Not serialPort.IsOpen Then
'            Try
'                serialPort.Open()
'                AddToListBox("SERIAL PORT OPENED." & " | " & Date.Now)
'            Catch ex As Exception
'                AddToListBox("ERROR OPENING SERIAL PORT:" & ex.Message & " | " & Date.Now)
'            End Try
'        End If

'        If ListBox1.Items.Count > 1 Then
'            ListBox1.SelectedItem = ListBox1.Items.Count
'        End If

'    End Sub

'    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
'        If Button1.Text = "STARTED" Then
'            Button1.Text = "PAUSED"
'            Button1.BackColor = Color.Red
'            Button1.ForeColor = Color.White
'            Timer1.Stop()
'            serialPort.Close()
'            AddToListBox("DATA RECIEVING PAUSED!." & " | " & Date.Now)
'        Else
'            Button1.Text = "STARTED"
'            Button1.BackColor = Color.Green
'            Button1.ForeColor = Color.White
'            Timer1.Start()
'            If Not serialPort.IsOpen Then
'                serialPort.Open()
'            Else
'                serialPort.Close()
'            End If
'            AddToListBox("DATA RECIEVING STARTED!." & " | " & Date.Now)
'            End If
'    End Sub
'End Class