
using Sl.InventControl.Data;


namespace Sl.InventControl.Pages {
    public partial class Index {

        private List<LoanModel> loans;

        private bool dense = false;
        private bool hover = true;
        private bool striped = false;
        private bool bordered = false;
        private string searchString1 = "";
        private LoanModel selectedItem1 = null;
        private HashSet<LoanModel> selectedItems = new HashSet<LoanModel>();

        private bool FilterFunc1(LoanModel element) => FilterFunc(element, searchString1);

        private bool FilterFunc(LoanModel element, string searchString) {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.BorowingEmployee.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.OwnerEmployee.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        private async Task AddLoan() {

        }

        private async Task Refresh() {
            loans = await dbService.GetDbContent<LoanModel>(CommonNames.LoansFile);
        }

        protected override async Task OnInitializedAsync() {
            loans = await dbService.GetDbContent<LoanModel>(CommonNames.LoansFile);
        }
    }
}
