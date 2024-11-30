using UnityEngine;

public class Task1 : Task 
{
    public override bool Done()
    {
        if (tutorial.isThirdEyeToggled)
        {
            return true;
		}
        return false;
	}
}
