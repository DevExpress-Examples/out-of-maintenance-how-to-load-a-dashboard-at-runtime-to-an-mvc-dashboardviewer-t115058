using System.Web.Mvc;
using DevExpress.DashboardWeb.Mvc;
using DevExpress.DataAccess.ConnectionParameters;
using System.IO;

namespace MVCLoadingDashboard.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult DashboardViewerPartial() {
            return PartialView("_DashboardViewerPartial", DashboardViewerSettings.Model);
        }
        public FileStreamResult DashboardViewerPartialExport() {
            return DashboardViewerExtension.Export("DashboardViewer", DashboardViewerSettings.Model);
        }
    }
    class DashboardViewerSettings {
        public static DashboardSourceModel Model {
            get {
                DashboardSourceModel model = new DashboardSourceModel();
                model.DashboardId = "SalesDashboard";

                model.DashboardLoading = (sender, e) => {
                    // Checks the identifier of the required dashboard.
                    if (e.DashboardId == "SalesDashboard") {
                        // Writes the dashboard XML definition from a file to a string.
                        string dashboardDefinition = File.ReadAllText(System.Web.Hosting.
                            HostingEnvironment.MapPath(@"~\App_Data\SalesDashboard.xml"));

                        // Specifies the dashboard XML definition.
                        e.DashboardXml = dashboardDefinition;
                    }
                };

                model.ConfigureDataConnection = (sender, e) => {
                    if (e.DataSourceName == "SQL Data Source 1") {
                        // Gets connection parameters used to establish a connection to the database.
                        Access97ConnectionParameters parameters =
                            (Access97ConnectionParameters)e.ConnectionParameters;
                        string databasePath = System.Web.Hosting.
                            HostingEnvironment.MapPath(@"~\App_Data\nwind.mdb");
                        // Specifies the path to a database file.  
                        parameters.FileName = databasePath;
                    }
                };

                return model;
            }
        }
    }
}