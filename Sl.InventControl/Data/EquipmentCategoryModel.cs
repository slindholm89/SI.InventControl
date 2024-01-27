namespace Sl.InventControl.Data {
    public class EquipmentCategoryModel : IDbModel {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }
    }
}
