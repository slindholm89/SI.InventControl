using MudBlazor;
using Sl.InventControl.Data;
using Sl.InventControl.Dialog;

namespace Sl.InventControl.Pages {
    public partial class Equipment {
        public List<EquipmentModel> Items { get; set; } = new List<EquipmentModel>();

        private string searchString1 = "";
        private EquipmentModel selectedItem1 = null;

        private bool FilterFunc1(EquipmentModel element) => FilterFunc(element, searchString1);

        private bool FilterFunc(EquipmentModel element, string searchString) {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.Make.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Model.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Type.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.SerialNumber.StartsWith(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Location.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;


            return false;
        }

        protected override async Task OnInitializedAsync() {
            Items = (await dbService.GetDbContent<EquipmentModel>(CommonNames.EquipmentFile)).OrderBy(x => x.Type).ToList();
        }

        private async Task EditItem(EquipmentModel item) {
            var parameters = new DialogParameters<ManageEquipmentDialog> {
                { x => x.Caption, $"Edit item" },
                {x => x.SnackbarInfo, $"Item updated successfully" },
                {x => x.Item, item }
            };
            DialogOptions options = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true };

            var dialog = await DialogService.ShowAsync<ManageEquipmentDialog>("Create item", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled) {
                await dbService.UpdateDbContent<EquipmentModel>(CommonNames.EquipmentFile, result?.Data as EquipmentModel);
                await OnInitializedAsync();
            }
        }

        private async Task DeleteItem(EquipmentModel item) {
            var parameters = new DialogParameters<ConfirmDialog> {
                { x => x.Caption, $"Are you sure you want to delete item '{item.Type} - {item.SerialNumber}'" },
                {x => x.SnackbarInfo, $"item '{item.Type} - {item.SerialNumber}' deleted" }
            };
            DialogOptions options = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true };

            var dialog = await DialogService.ShowAsync<ConfirmDialog>("Delete item", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled) {
                await dbService.RemoveDbContent<EquipmentModel>(CommonNames.EquipmentFile, item);
                await OnInitializedAsync();
            }

        }

        private async Task AddEquipmentCategories() {

            var existingCategories = await dbService.GetDbContent<EquipmentCategoryModel>(CommonNames.EquipmentCategoryFile) ?? new List<EquipmentCategoryModel>();

            var parameters = new DialogParameters<ManageEquipmentCategoryDialog> {
                {x => x.SnackbarInfo, $"Categories updated successfully" },
                {x => x.categories, existingCategories.ToList() }
            };
            DialogOptions options = new DialogOptions() { MaxWidth = MaxWidth.Small, FullWidth = true };

            var dialog = await DialogService.ShowAsync<ManageEquipmentCategoryDialog>("manage type", parameters, options);
            var result = await dialog.Result;
            
            if (!result.Canceled) {
                await dbService.ClearDbContent<EquipmentCategoryModel>(CommonNames.EquipmentCategoryFile);
                var items = result?.Data as List<EquipmentCategoryModel>;
                
                foreach(var item in items)
                    await dbService.AddDbContent<EquipmentCategoryModel>(CommonNames.EquipmentCategoryFile, item);
                
                await OnInitializedAsync();
            }
        }

        private async Task AddEquipmentManufacturers()
        {

            var existingManufacturers = await dbService.GetDbContent<EquipmentManufacturerModel>(CommonNames.EquipmentManufacturerFile) ?? new List<EquipmentManufacturerModel>();

            var parameters = new DialogParameters<ManageEquipmentManufacturerDialog> {
                {x => x.SnackbarInfo, $"Manufacturers updated successfully" },
                {x => x.manufacturers, existingManufacturers.ToList() }
            };
            DialogOptions options = new DialogOptions() { MaxWidth = MaxWidth.Small, FullWidth = true };

            var dialog = await DialogService.ShowAsync<ManageEquipmentManufacturerDialog>("manage type", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await dbService.ClearDbContent<EquipmentManufacturerModel>(CommonNames.EquipmentManufacturerFile);
                var items = result?.Data as List<EquipmentManufacturerModel>;

                foreach (var item in items)
                    await dbService.AddDbContent<EquipmentManufacturerModel>(CommonNames.EquipmentManufacturerFile, item);

                await OnInitializedAsync();
            }
        }

        private async Task AddEquipmentModels()
        {

            var existingModels = await dbService.GetDbContent<EquipmentModelModel>(CommonNames.EquipmentModelFile) ?? new List<EquipmentModelModel>();

            var parameters = new DialogParameters<ManageEquipmentModelDialog> {
                {x => x.SnackbarInfo, $"Models updated successfully" },
                { x => x.ManufacturerTypes, await dbService.GetDbContent<EquipmentManufacturerModel>(CommonNames.EquipmentManufacturerFile) ?? new List<EquipmentManufacturerModel>() },
                {x => x.models, existingModels.ToList() }
            };
            DialogOptions options = new DialogOptions() { MaxWidth = MaxWidth.Small, FullWidth = true };

            var dialog = await DialogService.ShowAsync<ManageEquipmentModelDialog>("manage type", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await dbService.ClearDbContent<EquipmentModelModel>(CommonNames.EquipmentModelFile);
                var items = result?.Data as List<EquipmentModelModel>;

                foreach (var item in items)
                    await dbService.AddDbContent<EquipmentModelModel>(CommonNames.EquipmentModelFile, item);

                await OnInitializedAsync();
            }
        }

        private async Task AddEquipmentLocations()
        {

            var existingLocations = await dbService.GetDbContent<EquipmentLocationModel>(CommonNames.EquipmentLocationFile) ?? new List<EquipmentLocationModel>();

            var parameters = new DialogParameters<ManageEquipmentLocationDialog> {
                {x => x.SnackbarInfo, $"Locations updated successfully" },
                {x => x.locations, existingLocations.ToList() }
            };
            DialogOptions options = new DialogOptions() { MaxWidth = MaxWidth.Small, FullWidth = true };

            var dialog = await DialogService.ShowAsync<ManageEquipmentLocationDialog>("manage type", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await dbService.ClearDbContent<EquipmentLocationModel>(CommonNames.EquipmentLocationFile);
                var items = result?.Data as List<EquipmentLocationModel>;

                foreach (var item in items)
                    await dbService.AddDbContent<EquipmentLocationModel>(CommonNames.EquipmentLocationFile, item);

                await OnInitializedAsync();
            }
        }

        private async Task AddEquipment() {

            var parameters = new DialogParameters<ManageEquipmentDialog> {
                { x => x.Caption, $"Create new item" },
                {x => x.SnackbarInfo, $"Item created successfully" },
                { x => x.EquipmentTypes, await dbService.GetDbContent<EquipmentCategoryModel>(CommonNames.EquipmentCategoryFile) ?? new List<EquipmentCategoryModel>() }        
            };
            DialogOptions options = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true };

            var dialog = await DialogService.ShowAsync<ManageEquipmentDialog>("Create item", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled) {
                var item = result?.Data as EquipmentModel;
                await dbService.AddDbContent<EquipmentModel>(CommonNames.EquipmentFile, item);
                await OnInitializedAsync();
            }
        }
    }
}
