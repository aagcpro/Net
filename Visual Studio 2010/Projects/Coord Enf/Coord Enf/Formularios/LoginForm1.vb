Public Class LoginForm1
    Dim user, pw As String
    ' TODO: inserte el código para realizar autenticación personalizada usando el nombre de usuario y la contraseña proporcionada 
    ' (Consulte http://go.microsoft.com/fwlink/?LinkId=35339).  
    ' El objeto principal personalizado se puede adjuntar al objeto principal del subproceso actual como se indica a continuación: 
    '     My.User.CurrentPrincipal = CustomPrincipal
    ' donde CustomPrincipal es la implementación de IPrincipal utilizada para realizar la autenticación. 
    ' Posteriormente, My.User devolverá la información de identidad encapsulada en el objeto CustomPrincipal
    ' como el nombre de usuario, nombre para mostrar, etc.
  


        Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
            Me.Close()
        End Sub

        Private Sub LoginForm1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        user = "ADMIN"
        pw = "SERVER"
    End Sub

    Private Sub UsernameTextBox_TextChanged(sender As System.Object, e As System.EventArgs) Handles UsernameTextBox.TextChanged
        UsernameTextBox.CharacterCasing = CharacterCasing.Upper

    End Sub

    Private Sub OK_Click(sender As System.Object, e As System.EventArgs) Handles OK.Click

   

        If user = UsernameTextBox.Text Then

            FrmRegistroyControldePacientes.Show()
            PasswordTextBox.Text = ""
        Else
            MsgBox("Usuario y/o Contraseña fueron incorrectas, Intenta de nuevo", MsgBoxStyle.Critical, "Error")
            UsernameTextBox.Text = ""
            PasswordTextBox.Text = ""
            UsernameTextBox.Focus()
        End If

    End Sub
End Class
