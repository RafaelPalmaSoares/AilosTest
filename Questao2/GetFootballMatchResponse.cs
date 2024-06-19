using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Questao2
{
    public class GetFootballMatchResponse
    {
        public int Page { get; set; }

        [JsonPropertyName("per_page")]
        public int PerPage { get; set; }
        public int Total { get; set; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }
        public List<FootballMatch>? Data { get; set; } = new List<FootballMatch>();
    }

    public class FootballMatch
    {
        public string? Competition { get; set; }
        public int Year { get; set; }
        public string? Round { get; set; }
        public string? Team1 { get; set; }
        public string? Team2 { get; set; }
        public string? Team1Goals { get; set; }
        public string? Team2Goals { get; set; }
    }
}
