
using Microsoft.Extensions.Configuration;
using Sl.InventControl.Data;
using static MudBlazor.CategoryTypes;


namespace Sl.InventControl.Pages {
    public partial class Index {

        private string searchString1 = "";
        public List<ItemHistoryModel> Items { get; set; } = new List<ItemHistoryModel>();


        private bool FilterFunc1(ItemHistoryModel element) => FilterFunc(element, searchString1);

        private bool FilterFunc(ItemHistoryModel element, string searchString) {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            /*if (element.Make.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Model.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Type.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;*/
            if (element.Item.SerialNumber.StartsWith(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Item.Id.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;


            return false;
        }


        protected override async Task OnInitializedAsync() {
            var workingFolder = new SettingsModel(Configuration).Paths.WorkingFolder;
            foreach(var item in Directory.GetFiles(workingFolder, "*-History.json")) {
                Items.AddRange(await dbService.GetDbContent<ItemHistoryModel>(new FileInfo(item).Name));
            }
            
        }
    }
}
