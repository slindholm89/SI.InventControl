namespace Sl.InventControl.Data {
    public class SettingsModel {
        
        public SettingsModel() { }

        public SettingsModel(IConfiguration configuration) {
            configuration.GetSection("InventControl").Bind(this);
        }

        public ManagementModel Management { get; set; } = new ManagementModel();

        public PathsModel Paths { get; set; } = new PathsModel();
    }

    public class ManagementModel {
        public string[] AccessGroups { get; set; }
    }

    public class PathsModel {
        public string WorkingFolder { get; set; } = "";
    }
}
