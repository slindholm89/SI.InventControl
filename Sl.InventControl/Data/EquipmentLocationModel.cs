namespace Sl.InventControl.Data {
    public class EquipmentLocationModel : IDbModel {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }
    }
}
