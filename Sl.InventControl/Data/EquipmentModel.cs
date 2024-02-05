namespace Sl.InventControl.Data {
    public class EquipmentModel : IDbModel {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        /*public string? Make { get; set; }
        public string? Model {  get; set; }
        public EquipmentCategoryModel? Type { get; set; } */
        public EquipmentTypeModel Type { get; set; }
        public string? SerialNumber { get; set; }
        public string? Location { get; set; }
        public string? Remark { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
