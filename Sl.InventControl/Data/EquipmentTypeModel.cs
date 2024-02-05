namespace Sl.InventControl.Data {
    public class EquipmentTypeModel : IDbModel {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Make {  get; set; }
        public string? Model { get; set; }
        public string? Type {  get; set; }
    }
}
