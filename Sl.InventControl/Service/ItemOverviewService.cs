using Sl.InventControl.Data;

namespace Sl.InventControl.Service {
    public class ItemOverviewService {

        private const string _usersFile = "";
        protected readonly IConfiguration configuration;

        public ItemOverviewService(IConfiguration configuration) {
            this.configuration = configuration;
        }


        public async Task<LoanModel[]> GetLoanAsync() {
            
            var loans = new List<LoanModel>();
            return loans.ToArray();
        }
    }
}
