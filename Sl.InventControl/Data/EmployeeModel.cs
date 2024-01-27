namespace Sl.InventControl.Data {
    public class EmployeeModel : IDbModel {
        public string Id { get; set; } = Guid.NewGuid().ToString(); 
        public string? FirstName { get; set; }
        public string? LastName { get; set; } = "";
        public string? Department { get; set; } = "";
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; } = "";
    }
}
