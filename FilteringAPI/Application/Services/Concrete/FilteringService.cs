using Application.Services.Abstractions;
using Domain.Options;
using Microsoft.Extensions.Options;

namespace Application.Services.Concrete;

public class FilteringService : IFilteringService
{
    private readonly FilteringOptions _options;

    public FilteringService(IOptions<FilteringOptions> options)
    {
        _options = options.Value;
    }

    public string Filter(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        var filteredText = text;

        switch (_options.Strategy)
        {
            
            case "Levenshtein":
                filteredText = LevenshteinFiltering(filteredText);
                break;
            case "Jaro-Winkler":
                filteredText = JaroWinklerFiltering(filteredText);
                break;
            default:
                filteredText = LevenshteinFiltering(filteredText);
                break;
        }

        return filteredText;
    }
    
    
        private string LevenshteinFiltering(string text)
    {
  
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var filteredWords = new List<string>();

        foreach (var word in words)
        {
            var saveWord = true;
            
            if (_options.StopWords != null)
            {
                foreach (var stopWord in _options.StopWords)
                {
                    var similarity = CalculateLevenshteinSimilarity(word, stopWord);
                    
                    if (similarity > _options.SimilarityThreshold)
                    {
                        saveWord = false;
                        break;
                    }
                }
            }

            if (saveWord)
            {
                filteredWords.Add(word);
            }
        }

        return string.Join(" ", filteredWords);
    }
    
    
    private double CalculateLevenshteinSimilarity(string word, string stopWord)
    {
        if (word == stopWord) return 1.0;
        if (string.IsNullOrEmpty(word) || string.IsNullOrEmpty(stopWord))
            return 0.0;

        int wordLength = word.Length;
        int stopWordLength = stopWord.Length;
        int[,] distance = new int[wordLength + 1, stopWordLength + 1];

        for (int i = 0; i <= wordLength; i++) distance[i, 0] = i;
        for (int j = 0; j <= stopWordLength; j++) distance[0, j] = j;

        for (int i = 1; i <= wordLength; i++)
        {
            for (int j = 1; j <= stopWordLength; j++)
            {
                int cost = (word[i - 1] == stopWord[j - 1]) ? 0 : 1;
                distance[i, j] = Math.Min(
                    Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1),
                    distance[i - 1, j - 1] + cost
                );
            }
        }

        int resultDistance = distance[wordLength, stopWordLength];
        int maxLen = Math.Max(wordLength, stopWordLength);
        return 1.0 - (double)resultDistance / maxLen;
    }


    private string JaroWinklerFiltering(string text)
    {

        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var filteredWords = new List<string>();

        foreach (var word in words)
        {
            var saveWord = true;
            
  
            if (_options.StopWords != null)
            {
                foreach (var stopWord in _options.StopWords)
                {
                    var similarity = CalculateJaroWinklerSimilarity(word, stopWord);
                    
                    if (similarity > _options.SimilarityThreshold)
                    {
                        saveWord = false;
                        break;
                    }
                }
            }

            if (saveWord)
            {
                filteredWords.Add(word);
            }
        }

        return string.Join(" ", filteredWords);
    }




    private double CalculateJaroWinklerSimilarity(string s1, string s2)
    {
        if (s1 == s2) return 1.0;
        if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2)) return 0.0;

        int matchDistance = Math.Max(s1.Length, s2.Length) / 2 - 1;

        bool[] s1Matches = new bool[s1.Length];
        bool[] s2Matches = new bool[s2.Length];

        int matches = 0;
        int transpositions = 0;

        // Find matches
        for (int i = 0; i < s1.Length; i++)
        {
            int start = Math.Max(0, i - matchDistance);
            int end = Math.Min(i + matchDistance + 1, s2.Length);

            for (int j = start; j < end; j++)
            {
                if (s2Matches[j] || s1[i] != s2[j]) continue;
                s1Matches[i] = true;
                s2Matches[j] = true;
                matches++;
                break;
            }
        }

        if (matches == 0) return 0.0;

        // Count transpositions
        int k = 0;
        for (int i = 0; i < s1.Length; i++)
        {
            if (!s1Matches[i]) continue;
            while (!s2Matches[k]) k++;
            if (s1[i] != s2[k]) transpositions++;
            k++;
        }

        double jaro = (matches / (double)s1.Length + matches / (double)s2.Length + 
                      (matches - transpositions / 2.0) / matches) / 3.0;

        // Jaro-Winkler modification
        int prefix = 0;
        for (int i = 0; i < Math.Min(Math.Min(s1.Length, s2.Length), 4); i++)
        {
            if (s1[i] == s2[i]) prefix++;
            else break;
        }

        return jaro + (0.1 * prefix * (1 - jaro));
    }

  

}