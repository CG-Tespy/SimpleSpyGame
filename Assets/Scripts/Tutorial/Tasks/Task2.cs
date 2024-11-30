using UnityEngine;
public class Task2 : Task 
{
    public override bool Done()
    {
        if (tutorial.isHiding)
        {
            return true;
        }
        return false;
	}
}
