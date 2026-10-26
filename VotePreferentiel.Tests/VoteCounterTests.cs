using VotePreferentiel;

namespace VotePreferentiel.Tests;

public class VoteCounterTests
{
    // Test 1 : Cas nominal
    [Test]
    public async Task TestCasNominal()
    {
        VoteModel votes = new VoteModel
        {
            Votes = new List<Vote>
            {
                new Vote { Choices = new() { "Alice", "Bilal", "Chloé" } },
                new Vote { Choices = new() { "Alice", "Chloé", "Bilal" } },
                new Vote { Choices = new() { "Bilal", "Chloé", "Alice" } },
                new Vote { Choices = new() { "Chloé", "Bilal", "Alice" } },
                new Vote { Choices = new() { "Chloé", "Alice", "Bilal" } }
            }
        };

        VoteCounter counter = new VoteCounter();
        string gagnant = counter.GetWinner(votes);

        await Assert.That(gagnant).IsEqualTo("Chloé");
    }

    // Test 2 : Un seul candidat
    [Test]
    public async Task TestUnSeulCandidat()
    {
        VoteModel votes = new VoteModel
        {
            Votes = new List<Vote>
            {
                new Vote { Choices = new() { "Alice" } }
            }
        };

        VoteCounter counter = new VoteCounter();
        string gagnant = counter.GetWinner(votes);

        await Assert.That(gagnant).IsEqualTo("Alice");
    }

    // Test 3 : Report des préférences
    [Test]
    public async Task TestReportPreferences()
    {
        VoteModel votes = new VoteModel();

        for (int i = 0; i < 4; i++)
        {
            votes.Votes.Add(new Vote
            {
                Choices = new() { "Alice", "Bilal", "Chloé", "Diane" }
            });
        }

        for (int i = 0; i < 3; i++)
        {
            votes.Votes.Add(new Vote
            {
                Choices = new() { "Bilal", "Chloé", "Alice", "Diane" }
            });
        }

        for (int i = 0; i < 3; i++)
        {
            votes.Votes.Add(new Vote
            {
                Choices = new() { "Chloé", "Alice", "Bilal", "Diane" }
            });
        }

        votes.Votes.Add(new Vote
        {
            Choices = new() { "Diane", "Chloé", "Bilal", "Alice" }
        });

        VoteCounter counter = new VoteCounter();
        string gagnant = counter.GetWinner(votes);

        await Assert.That(gagnant).IsEqualTo("Chloé");
    }

    // Test 4 : Égalité en dernière position
    [Test]
    public async Task TestEgalite()
    {
        VoteModel votes = new VoteModel
        {
            Votes = new List<Vote>
            {
                new Vote { Choices = new() { "Alice", "Chloé", "Bilal" } },
                new Vote { Choices = new() { "Bilal", "Chloé", "Alice" } },
                new Vote { Choices = new() { "Chloé", "Alice", "Bilal" } },
                new Vote { Choices = new() { "Chloé", "Bilal", "Alice" } }
            }
        };

        VoteCounter counter = new VoteCounter();
        string gagnant = counter.GetWinner(votes);

        await Assert.That(gagnant).IsEqualTo("Chloé");
    }

    // Test 5 : Bulletin vide
    [Test]
    public async Task TestBulletinVide()
    {
        VoteModel votes = new VoteModel
        {
            Votes = new List<Vote>
            {
                new Vote { Choices = new() }
            }
        };

        VoteCounter counter = new VoteCounter();

        await Assert.That(() => counter.GetWinner(votes))
            .Throws<Exception>();
    }
}