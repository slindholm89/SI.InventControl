namespace Sl.InventControl.Data {
    public class EquipmentManufacturerModel : IDbModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }
    }
}
