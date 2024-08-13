using System;
using System.Collections.Generic;
using System.Linq;

namespace ChipSecuritySystem
{

	class Program
	{
		public static string OrderChips(List<ColorChip> chips)
		{
			List<ColorChip> orderedChips = new List<ColorChip>();
			Color startPoint = Color.Blue;
			Color endPoint = Color.Green;

			//So I want to check if there is even a chip that will fit in the starting marker.
			ColorChip firstChip = chips.Where(c => c.StartColor == startPoint).FirstOrDefault();

			//If the first chip is null that means nothing will fit the starting marker so there is no point in continuing.
			if (firstChip == null)
			{
				//More descriptive error message.
				return $"{Constants.ErrorMessage}. No chip matches starting marker.";
			}

			//We want to keep state of whatever the current end color is. Right now this is the color of the first chip we just found
			Color currentEndColor = firstChip.EndColor;

			//Now we just add that first chip to our collection.
			orderedChips.Add(firstChip);

			//We can remove it since we don't need it.
			chips.Remove(firstChip);

			//So for fast lookups I know hashtables tend to be good so I am going to create one where the key is just the start color.
			//Reason we want the key to be the start color is because we are saving the current state of the end color and 
			Dictionary<Color, ColorChip> chipHashMap = new Dictionary<Color, ColorChip>();

			foreach(ColorChip chip in chips)
			{
				chipHashMap[chip.StartColor] = chip;
			}

			//Now we just want to iterate through this hashmap and just check and see if anything matches from our original collection
			//We would want to loop while a key still exists for the current end color, otherwise, we cannot make another more matches and we dont want infinite loop.
			//Also since we can make the assumption that the end color is always green then that means probably no chip will start with that color (key won't exist).
			while(chipHashMap.ContainsKey(currentEndColor))
			{
				//So if the Start color of a chip matches the end color of the currentEndColor we want to add it to our ordered collection.
				if (chipHashMap.ContainsKey(currentEndColor))
				{
					ColorChip matchedChip = chipHashMap[currentEndColor];

					orderedChips.Add(matchedChip);

					currentEndColor = matchedChip.EndColor;

					//To showcase the unused chips at the end.
					chips.Remove(matchedChip);
				}
			}

			return $"{startPoint} -> {string.Join(" -> ", orderedChips)} -> {endPoint} {Environment.NewLine}Following chips not used: {string.Join(", ", chips)}";
		}

		static void Main(string[] args)
		{
			//Initialize our input
			List<ColorChip> list = new List<ColorChip>()
			{
				new ColorChip(Color.Blue, Color.Yellow),
				new ColorChip(Color.Red, Color.Green),
				new ColorChip(Color.Yellow, Color.Red),
				new ColorChip(Color.Orange, Color.Purple)
			};

			Console.WriteLine(OrderChips(list));
		}
	}
}
