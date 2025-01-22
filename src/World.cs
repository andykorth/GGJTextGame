using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

public class World
{
	// Everything you want to save or serialize is stored on the world.
	public static World instance;
	public const string FILENAME = "world.json";

	public static void CreateOrLoad(){
		Console.WriteLine("Create or load world...");
		if(File.Exists(FILENAME)){
			instance = JSONUtilities.Deserialize<World>(FILENAME);
		}else{
			Console.WriteLine("No world found, writing one.");
			instance = new World();
			SaveWorld();
		}
	}

	public static void SaveWorld(){
		string s = JSONUtilities.SerializeToString(instance);
		
		// serialize JSON directly to a file using stream
		var serializer = JSONUtilities.serializer;
		using (StreamWriter file = File.CreateText(FILENAME + "tmp"))
		{
			serializer.Serialize(file, instance);
		}

		// This ensures interrupted file writes don't destroy data.
		// the previous working backup is retained.
		if(File.Exists(FILENAME + "tmp")){
			if(File.Exists(FILENAME)){
				// retain previous version:
				if(File.Exists(FILENAME +"bk")){
					File.Delete(FILENAME + "bk");
				}
				File.Move(FILENAME, FILENAME + "bk");
			}
			//atomically replace previous:
			File.Move(FILENAME +"tmp", FILENAME);
		}

	}

}