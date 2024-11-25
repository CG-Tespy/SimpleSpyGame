using UnityEngine;

public class Task1 : Task 
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
