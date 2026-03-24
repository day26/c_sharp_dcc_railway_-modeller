using System.IO;
using DccController.Models;
using Newtonsoft.Json;
using System.Runtime.Intrinsics.Arm;

namespace DccController.Services
{
    public class RosterService
    {
        // The file will be saved in the same folder as the application
        private readonly string _filePath;

        public RosterService()
        {
            _filePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "locomotive_roster.json");
        }

        // ── Save Roster ──────────────────────────────────────────────
        public void SaveRoster(List<Locomotive> locomotives)
        {
            try
            {
                string json = JsonConvert.SerializeObject(
                    locomotives, Formatting.Indented);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to save roster: {ex.Message}");
            }
        }

        // ── Load Roster ──────────────────────────────────────────────
        public List<Locomotive> LoadRoster()
        {
            try
            {
                // If no file exists yet, return an empty list
                if (!File.Exists(_filePath))
                    return new List<Locomotive>();

                string json = File.ReadAllText(_filePath);

                var locomotives = JsonConvert.DeserializeObject
                    <List<Locomotive>>(json);

                return locomotives ?? new List<Locomotive>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load roster: {ex.Message}");
            }
        }

        // ── Delete a Locomotive ──────────────────────────────────────
        public void DeleteLocomotive(List<Locomotive> locomotives, int id)
        {
            var loco = locomotives.FirstOrDefault(l => l.Id == id);
            if (loco != null)
            {
                locomotives.Remove(loco);
                SaveRoster(locomotives);
            }
        }

        // ── Get Next Available ID ────────────────────────────────────
        public int GetNextId(List<Locomotive> locomotives)
        {
            if (!locomotives.Any())
                return 1;

            return locomotives.Max(l => l.Id) + 1;
        }
    }
}
