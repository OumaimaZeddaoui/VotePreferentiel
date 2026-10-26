namespace VotePreferentiel;

public class VoteCounter : IVoteCounter
{
    public HashSet<string> GetCandidates(VoteModel voteModel)
    {
        HashSet<string> candidates = new();

        foreach (Vote vote in voteModel.Votes)
        {
            foreach (string candidate in vote.Choices)
            {
                candidates.Add(candidate);
            }
        }

        return candidates;
    }

    public List<string> GetAllFirstChoices(VoteModel voteModel)
    {
        List<string> firstChoices = new();

        foreach (Vote vote in voteModel.Votes)
        {
            if (vote.Choices.Count > 0)
            {
                firstChoices.Add(vote.Choices[0]);
            }
        }

        return firstChoices;
    }

    public IDictionary<string, int> CountFirstChoices(List<string> firstChoices)
    {
        Dictionary<string, int> counts = new();

        foreach (string candidate in firstChoices)
        {
            if (counts.ContainsKey(candidate))
            {
                counts[candidate]++;
            }
            else
            {
                counts[candidate] = 1;
            }
        }

        return counts;
    }

    public string[] GetCandidatesWithLeastVotes(
        IDictionary<string, int> firstChoiceCounts)
    {
        int minimum = firstChoiceCounts.Values.Min();

        List<string> candidates = new();

        foreach (var candidate in firstChoiceCounts)
        {
            if (candidate.Value == minimum)
            {
                candidates.Add(candidate.Key);
            }
        }

        return candidates.ToArray();
    }

    public string GetWinner(VoteModel voteModel)
    {
        foreach (Vote vote in voteModel.Votes)
        {
            if (vote.Choices == null || vote.Choices.Count == 0)
            {
                throw new Exception("Bulletin vide");
            }
        }

        HashSet<string> candidates = GetCandidates(voteModel);

        if (candidates.Count == 0)
        {
            throw new Exception("Aucun candidat");
        }

        while (candidates.Count > 1)
        {
            List<string> firstChoices = new();

            foreach (Vote vote in voteModel.Votes)
            {
                foreach (string choice in vote.Choices)
                {
                    if (candidates.Contains(choice))
                    {
                        firstChoices.Add(choice);
                        break;
                    }
                }
            }

            IDictionary<string, int> counts = CountFirstChoices(firstChoices);

            foreach (string candidate in candidates)
            {
                if (!counts.ContainsKey(candidate))
                {
                    counts[candidate] = 0;
                }
            }

            string[] eliminated = GetCandidatesWithLeastVotes(counts);

            foreach (string candidate in eliminated)
            {
                candidates.Remove(candidate);
            }

            if (candidates.Count == 0)
            {
                throw new Exception("Égalité entre tous les candidats");
            }
        }

        return candidates.First();
    }
}