namespace Sl.InventControl.Data {
    public class EquipmentCategoryModel : IDbModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Manufacturer { get; set; }
        public List<ManufacturerCategoryModel> Categories { get; set; } = new List<ManufacturerCategoryModel>();
    }

    public class  ManufacturerCategoryModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Type { get; set; }
        public List<ManufacturerModelModel> Models { get; set; } = new List<ManufacturerModelModel>();
    }

    public class ManufacturerModelModel 
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Model { get; set; }
    }
}
