using MudBlazor;
using Sl.InventControl.Data;
using Sl.InventControl.Dialog;

namespace Sl.InventControl.Pages {
    public partial class Loans {
        public List<LoanModel> ActiveLoans { get; set; } = new List<LoanModel>();

        private string searchString1 = "";
        private LoanModel selectedItem1 = null;

        private bool FilterFunc1(LoanModel element) => FilterFunc(element, searchString1);

        private bool FilterFunc(LoanModel element, string searchString) {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.BorowingEmployee.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.BorowingEmployee.LastName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.BorowingEmployee.PhoneNumber.StartsWith(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.BorowingEmployee.Email.StartsWith(searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (element.Items.Any(x => x.SerialNumber.StartsWith(searchString, StringComparison.OrdinalIgnoreCase)))
                return true;

            return false;
        }

        protected override async Task OnInitializedAsync() {
            ActiveLoans = (await dbService.GetDbContent<LoanModel>(CommonNames.LoansFile)).Where(x => x.IsActive).OrderBy(x => x.ReturnByDate).ToList();
        }

        private async Task AddLoanAgreement() {

            var parameters = new DialogParameters<ManageLoansDialog> {
                { x => x.Caption, $"Create new lending agreement" },
                { x => x.SnackbarInfo, $"lending agreement created successfully" }
            };
            DialogOptions options = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true };

            var dialog = await DialogService.ShowAsync<ManageLoansDialog>("Create loaning agreement", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled) {
                var item = result?.Data as LoanModel;
                await dbService.AddDbContent<LoanModel>(CommonNames.LoansFile, item);

                foreach(var equipmentItem in item.Items) {
                    equipmentItem.IsAvailable = false;
                    await dbService.UpdateDbContent<EquipmentModel>(CommonNames.EquipmentFile, equipmentItem);
                }

                await OnInitializedAsync();
            }
        }

        private async Task EditLoansAgreement(LoanModel agreement) {
            var existingItems = new EquipmentModel[agreement.Items.Count()];
            agreement.Items.CopyTo(existingItems);
            
            var parameters = new DialogParameters<ManageLoansDialog> {
                { x => x.Caption, $"edit lending agreement" },
                { x => x.SnackbarInfo, $"lending agreement changed successfully" },
                { x => x.Loan, agreement }
            };
            DialogOptions options = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true };

            var dialog = await DialogService.ShowAsync<ManageLoansDialog>("Edit loan agreement", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled) {
                var item = result?.Data as LoanModel;
                await dbService.UpdateDbContent<LoanModel>(CommonNames.LoansFile, item);

                foreach(var existingItem in existingItems) {
                    if(!item.Items.Any(i => i.Id == existingItem.Id)) {
                        existingItem.IsAvailable = true;
                        await dbService.UpdateDbContent<EquipmentModel>(CommonNames.EquipmentFile, existingItem);
                    }
                        
                }

                foreach (var equipmentItem in item.Items) {
                    equipmentItem.IsAvailable = false;
                    await dbService.UpdateDbContent<EquipmentModel>(CommonNames.EquipmentFile, equipmentItem);
                }

                await OnInitializedAsync();
            }
        }

        private async Task ReturnLoansAgreement(LoanModel agreement) {

            var parameters = new DialogParameters<ConfirmDialog> {
                { x => x.Caption, $"Confirm return of all items?" },
                { x => x.SnackbarInfo, $"lending agreement returned successfully" }
            };
            DialogOptions options = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true };

            var dialog = await DialogService.ShowAsync<ConfirmDialog>("Return lending agreement", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled) {
                var items = agreement.Items;
                agreement.IsActive = false;

                await dbService.UpdateDbContent<LoanModel>(CommonNames.LoansFile, agreement);
                foreach (var item in items) {
                    item.IsAvailable = true;
                    await dbService.UpdateDbContent<EquipmentModel>(CommonNames.EquipmentFile, item);
                }

                await OnInitializedAsync();
            }

                
        }
    }
}
