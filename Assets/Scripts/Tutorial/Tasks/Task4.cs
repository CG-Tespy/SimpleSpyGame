using UnityEngine;

public class Task4 : Task 
{
    public override bool Done()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            return true;
		}
        return false;
	}
}
