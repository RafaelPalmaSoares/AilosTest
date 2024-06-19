using Newtonsoft.Json;
using Questao2;
using Refit;
using System.Text.RegularExpressions;

public class Program
{
    public static async Task Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static async Task<int> getTotalScoredGoals(string team, int year)
    {
        var api = RestService.For<IHackerHankClient>("https://jsonmock.hackerrank.com/api");

        var totalGoals = 0;
        var currentPage = 1;
        var totalPages = 1;

        while (currentPage <= totalPages)
        {
            var apiResponse = await api.GetFootballMatchesAsync(year, team, currentPage);

            if(apiResponse?.Content?.Data is not null)
            {
                totalGoals += apiResponse.Content.Data.Sum(x =>
                    int.TryParse(x.Team1Goals, out int goals) ? goals : 0);

                totalPages = apiResponse.Content.TotalPages;
                currentPage++;
            }
        }

        return totalGoals;

    }

}