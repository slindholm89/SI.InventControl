namespace Sl.InventControl.Data {
    public class LoanModel : IDbModel {

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public DateTime? LoanStartDate { get; set; } = DateTime.Now;

        public DateTime? ReturnByDate { get; set; } = DateTime.Now.AddDays(7);

        public EmployeeModel? OwnerEmployee { get; set; }

        public EmployeeModel? BorowingEmployee { get; set; }

        public List<EquipmentModel> Items { get; set; } = new List<EquipmentModel>();

        public bool IsActive { get; set; } = true;

    }
}
