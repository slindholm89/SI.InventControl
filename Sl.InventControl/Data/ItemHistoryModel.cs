namespace Sl.InventControl.Data {
    public class ItemHistoryModel : IDbModel{
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public EquipmentModel? Item { get; set; }
        public DateTime? TimeStamp { get; set; } = DateTime.Now;
        public string? Action { get; set; }
        public string? User { get; set; }
    }
}
