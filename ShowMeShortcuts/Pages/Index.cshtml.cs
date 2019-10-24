using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using QuickType;

namespace ShowMeShortcuts.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

            using (var webClient = new WebClient())
            {
                String jsonString = webClient.DownloadString("https://roster19fs702420191018063705.azurewebsites.net/Feed");
                JSchema schema = JSchema.Parse(System.IO.File.ReadAllText("WelcomeSchema.json"));
                JArray jsonArray = JArray.Parse(jsonString);
                IList<string> validationEvents = new List<string>();
                if (jsonArray.IsValid(schema, out validationEvents))
                {
                    Welcome[] welcome = Welcome.FromJson(jsonString);
                    ViewData["Welcome"] = welcome;
                } else
                {
                    Console.WriteLine("Not valid.  Fool.");
                    foreach(string evt in validationEvents) {
                        Console.WriteLine(evt);
                    }
                    ViewData["Welcome"] = new Welcome[0];
                }
            }
        }
    }

}
