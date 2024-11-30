public class Task4 : Task 
{
    public override bool Done()
    {
        // Cancle hiding
        if (tutorial.isTeleportToggled && !tutorial.player.IsHiding)
        {
            return true;
		}
        return false;
	}
}
