using System.Text;

namespace CSVViewer.Models
{
    /// <summary>
    /// Extension class for IFromFile
    /// </summary>
    public static class FromFileExtender
    {
        public async static Task<List<string>> ReadAsList(this IFormFile file)
        {
            var result = new List<string>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader?.Peek() >= 0)
                {
                    var item = await reader.ReadLineAsync();
                    result.Add(item ?? string.Empty);
                }
            }

            return result;
        }
    }
}
