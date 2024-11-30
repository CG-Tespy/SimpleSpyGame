public class Task2 : Task 
{
    public override bool Done()
    {
        if (tutorial.player.IsHiding)
        {
            return true;
        }
        return false;
	}
}
