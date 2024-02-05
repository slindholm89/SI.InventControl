using Sl.InventControl.Data;

namespace Sl.InventControl.Service {
    public class DbService {

        private string folderPath;
        private object _fileLock = new object();

        public DbService(IConfiguration configuration) {
            folderPath = new SettingsModel(configuration).Paths.WorkingFolder;
        }


        public async Task<List<T>> GetDbContent<T>(string fileName) {
            
            var allItems = new List<T>();
            var filePath = Path.Combine(folderPath, fileName);
            try {
                lock (_fileLock) {
                    var fileContent = File.ReadAllText(filePath);
                    allItems.AddRange(System.Text.Json.JsonSerializer.Deserialize<T[]>(fileContent));
                }
            }
            catch (Exception ex) {

            }
            return allItems;
            
        }

        public async Task AddDbContent<T>(string filename, IDbModel item) {
            var filePath = Path.Combine(folderPath, filename);

            var allItems = await GetDbContent<T>(filePath);
            if (allItems != null && !allItems.Any(i => ((IDbModel)i).Id == item.Id)) {
                allItems?.Add((T)item);
            }

            var content = System.Text.Json.JsonSerializer.Serialize(allItems);
            lock (_fileLock) {
                File.WriteAllText(filePath, content);
            }
        }

        public async Task ClearDbContent<T>(string filename) {
            var filePath = Path.Combine(folderPath, filename);
            var content = System.Text.Json.JsonSerializer.Serialize(new List<T>());
            lock (_fileLock) {
                File.WriteAllText(filePath, content);
            }
        }

        public async Task UpdateDbContent<T>(string filename, IDbModel updateditem) {
            var filePath = Path.Combine(folderPath, filename);

            var allItems = await GetDbContent<T>(filePath);
            var existingItem = allItems.FirstOrDefault(i => ((IDbModel)i).Id == updateditem.Id);
            //existingItem = (T)updateditem;
            allItems.Remove(existingItem);
            allItems.Add((T)updateditem);

            var content = System.Text.Json.JsonSerializer.Serialize(allItems);
            lock (_fileLock) {
                File.WriteAllText(filePath, content);
            }
        }

        public async Task RemoveDbContent<T>(string filename, IDbModel item) {
            var filePath = Path.Combine(folderPath, filename);

            var allItems = await GetDbContent<T>(filePath);
            allItems = allItems.Where(x => ((IDbModel)x).Id != item.Id).ToList();
            
            var content = System.Text.Json.JsonSerializer.Serialize(allItems);
            lock (_fileLock) {
                File.WriteAllText(filePath, content);
            }
        }
    }
}
