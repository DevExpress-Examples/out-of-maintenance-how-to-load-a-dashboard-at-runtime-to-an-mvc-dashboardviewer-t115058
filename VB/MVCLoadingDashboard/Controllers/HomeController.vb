Imports System.Web.Mvc
Imports DevExpress.DashboardWeb.Mvc
Imports DevExpress.DataAccess.ConnectionParameters
Imports System.IO

Namespace MVCLoadingDashboard.Controllers
    Public Class HomeController
        Inherits Controller

        Public Function Index() As ActionResult
            Return View()
        End Function

        <ValidateInput(False)> _
        Public Function DashboardViewerPartial() As ActionResult
            Return PartialView("_DashboardViewerPartial", DashboardViewerSettings.Model)
        End Function
        Public Function DashboardViewerPartialExport() As FileStreamResult
            Return DashboardViewerExtension.Export("DashboardViewer", DashboardViewerSettings.Model)
        End Function
    End Class
    Friend Class DashboardViewerSettings
        Public Shared ReadOnly Property Model() As DashboardSourceModel
            Get

                Dim model_Renamed As New DashboardSourceModel()
                model_Renamed.DashboardId = "SalesDashboard"

                model_Renamed.DashboardLoading = Sub(sender, e)
                    ' Checks the identifier of the required dashboard.
                    If e.DashboardId = "SalesDashboard" Then
                        ' Writes the dashboard XML definition from a file to a string.
                        Dim dashboardDefinition As String = 
                            File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~\App_Data\SalesDashboard.xml"))

                        ' Specifies the dashboard XML definition.
                        e.DashboardXml = dashboardDefinition
                    End If
                End Sub

                model_Renamed.ConfigureDataConnection = Sub(sender, e)
                    If e.DataSourceName = "SQL Data Source 1" Then
                        ' Gets connection parameters used to establish a connection to the database.
                        Dim parameters As Access97ConnectionParameters = 
                            CType(e.ConnectionParameters, Access97ConnectionParameters)
                        Dim databasePath As String = System.Web.Hosting.HostingEnvironment.MapPath("~\App_Data\nwind.mdb")
                        ' Specifies the path to a database file.  
                        parameters.FileName = databasePath
                    End If
                End Sub

                Return model_Renamed
            End Get
        End Property
    End Class
End Namespace