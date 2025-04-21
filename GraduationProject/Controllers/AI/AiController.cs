using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Controllers.AI
{
    public class AiController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> TrainScheduler()
        {
            try
            {
                // Step 1: Prepare model input by making a POST request to prepare CSVs
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync("http://localhost:8000/prepare-model-input", null);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Model input prepared successfully!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to prepare model input.";
                        return RedirectToAction("SemesterInfoPage", "Advisor"); // Redirect if input preparation fails
                    }
                }

                // Step 2: Train the model after preparation
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync("http://localhost:8000/train", null);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Scheduler trained and saved to database!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to train scheduler.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            // Redirect to Home (or any page you want)
            return RedirectToAction("SemesterInfoPage", "Advisor");
        }
    }
}
