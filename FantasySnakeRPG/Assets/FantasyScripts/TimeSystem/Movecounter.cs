public class Movecounter
{
    private int move = 0;

    public void ResetMoveCounter()
    {
        move = 0;
        
    }
    public void IncreaseMoveCounter()
    {
        move++;
    }

    public int GetCurrentMoveAmount()
    {
        return move;
    }
}

public class MonsterDefeatedCounter
{
    private int monsterDefected = 0;

    public void ResetMonsterAmount()
    {
        monsterDefected = 0;
    }

    public void IncreaseMonsterCounter()
    {
        monsterDefected++;
    }

    public int GetCurrentMonsterAmount()
    {
        return monsterDefected;
    }
}

public class PlayerScore
{
    private int currentScore = 0;

    public void ResetScore()
    {
        currentScore = 0;
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }
}
