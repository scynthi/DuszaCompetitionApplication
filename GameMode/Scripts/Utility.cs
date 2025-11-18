using Godot;
using System;
using System.Collections.Generic;

public static class Utility
{
	public static List<string> WorldObjectListToNameList<T>(List<T> objectList) where T : IWorldObject
    {
		List<string> nameList = new List<string>();
		foreach (T currObj in objectList)
			nameList.Add(currObj.Name);
		return nameList;
    }
}
