Module FuncionesDB
    Public conn As New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Enfemeria.accdb;Persist Security Info=false")
    Public cmd As OleDb.OleDbCommand
    Public dr As OleDb.OleDbDataReader

    Public Sub conectarse()
        Try
            conn.Open()
            MsgBox("Bienvenido")

        Catch ex As Exception
            MsgBox(ex.ToString)

        End Try
    End Sub

    Public Sub consultar(ByRef identificacion As String)
        cmd.Connection = conn
        cmd.CommandType = CommandType.Text
        If identificacion <> "" Then
            cmd.CommandText = "select USUARIO, TURNO, PUESTO, APPATERNO, APMATERNO, NOMBRE FROM USERS WHERE ID=" + identificacion
        Else
            cmd.CommandText = "select USUARIO, TURNO, PUESTO, APPATERNO, APMATERNO, NOMBRE FROM USERS "
        End If
        Try
            dr = cmd.ExecuteReader

            If dr.HasRows Then
                While dr.Read()
                    MsgBox(dr(0).ToString + "" + dr(1).ToString + "" + dr(2).ToString + "" + dr(3).ToString + "" + dr(4).ToString + "" + dr(5).ToString + "" + dr(6).ToString)


                End While
            Else
                MsgBox(" No existen registros para la consulta")
            End If
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
End Module
