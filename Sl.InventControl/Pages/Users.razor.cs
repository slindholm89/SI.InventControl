using MudBlazor;
using Sl.InventControl.Data;
using Sl.InventControl.Dialog;

namespace Sl.InventControl.Pages {
    public partial class Users {
        public List<EmployeeModel> Employees { get; set; } = new List<EmployeeModel>();

        private string searchString1 = "";
        private EmployeeModel selectedItem1 = null;

        private bool FilterFunc1(EmployeeModel element) => FilterFunc(element, searchString1);

        private bool FilterFunc(EmployeeModel element, string searchString) {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.LastName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Department.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.PhoneNumber.StartsWith(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;


            return false;
        }

        protected override async Task OnInitializedAsync() {
            Employees = (await dbService.GetDbContent<EmployeeModel>("employees.json")).OrderBy(x => x.LastName).ToList();
        }

        private async Task EditUser(EmployeeModel user) {
            var parameters = new DialogParameters<ManageUserDialog> {
                { x => x.Caption, $"Edit user" },
                {x => x.SnackbarInfo, $"User updated successfully" },
                {x => x.User, user }
            };
            DialogOptions options = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true };

            var dialog = await DialogService.ShowAsync<ManageUserDialog>("Create user", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled) {
                await dbService.UpdateDbContent<EmployeeModel>(CommonNames.EmployeeFile, result?.Data as EmployeeModel);
                await OnInitializedAsync();
            }
        }

        private async Task DeleteUser(EmployeeModel user) {
            var parameters = new DialogParameters<ConfirmDialog> {
                { x => x.Caption, $"Are you sure you want to delete user '{user.FirstName} {user.LastName}'?" },
                {x => x.SnackbarInfo, $"User '{user.FirstName} {user.LastName}' deleted" }
            };
            DialogOptions options = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true };

            var dialog = await DialogService.ShowAsync<ConfirmDialog>("Delete user", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled) {
                await dbService.RemoveDbContent<EmployeeModel>(CommonNames.EmployeeFile, user);
                await OnInitializedAsync();
            }

        }

        private async Task AddUser() {

            var parameters = new DialogParameters<ManageUserDialog> {
                { x => x.Caption, $"Create new user" },
                {x => x.SnackbarInfo, $"User created successfully" }
            };
            DialogOptions options = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true };

            var dialog = await DialogService.ShowAsync<ManageUserDialog>("Create user", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled) {
                var user = result?.Data as EmployeeModel;
                await dbService.AddDbContent<EmployeeModel>(CommonNames.EmployeeFile, user);
                await OnInitializedAsync();
            }
        }
    }
}
