using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao2
{
    public interface IHackerHankClient
    {
        [Get("/football_matches?year={year}&team1={team}&page={page}")]
        Task<ApiResponse<GetFootballMatchResponse>> GetFootballMatchesAsync(int year, string team, int page);
    }
}
