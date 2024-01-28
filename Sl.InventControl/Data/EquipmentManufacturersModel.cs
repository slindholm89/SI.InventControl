namespace Sl.InventControl.Data {
    public class EquipmentManufacturersModel : IDbModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }
    }
}
