Module carga
    Public Sub main()
        Application.EnableVisualStyles()
        Application.EnableVisualStyles()

        ' Mostramos el formulario SplashScreeen
        ' 
        Using frm As New SplashScreen1()
            frm.ShowDialog()
        End Using

        ' Mostramos el formulario principal
        '
        Application.Run(LoginForm1)
    End Sub
End Module
