public class Task3 : Task 
{
    public override bool Done()
    {
        if (tutorial.isTeleportToggled)
        {
            return true;
		}
        return false;
	}
}
