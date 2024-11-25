using UnityEngine;

public class Task3 : Task 
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
