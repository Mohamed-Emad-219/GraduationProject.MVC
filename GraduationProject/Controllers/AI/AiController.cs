using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace GraduationProject.Controllers.AI
{
    public class AiController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> TrainScheduler(int level1, int level2, int level3is, int level4is, int level3cs, int level4cs, int? level3ai, int? level4ai, int timeslot)
        {
            try
            {
                // Step 1: Prepare model input by making a POST request to prepare CSVs
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync("http://localhost:6000/prepare-model-input", null);
                    if (!response.IsSuccessStatusCode)
                    {
                        return Json(new { success = false, message = "Failed to prepare model input." });
                    }
                }

                // Step 2: Train the model after preparation
                using (var client = new HttpClient())
                {
                    var data = new
                    {
                        level1,
                        level2,
                        level3is,
                        level4is,
                        level3cs,
                        level4cs,
                        level3ai = level3ai ?? 0, // default to 0 if null
                        level4ai = level4ai ?? 0,
                        timeslot
                    };
                    var json = new StringContent(
                        JsonSerializer.Serialize(data),
                        Encoding.UTF8,
                        "application/json"
                    );
                    var response = await client.PostAsync("http://localhost:6000/train", json);
                    if (!response.IsSuccessStatusCode)
                    {
                        return Json(new { success = false, message = "Failed to train scheduler." });
                    }
                }
                return Json(new { success = true, message = "Scheduler trained and saved to database!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }

        }
    }
}
