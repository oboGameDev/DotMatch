using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
namespace _Scripts.Utils
{
	public class RandomHelper
	{
		private List<IndexPercent> _percents;

		public RandomHelper(int count, params IndexPercent[] percents)
		{
			Init(count, percents);
		}

		private void Init(int count, params IndexPercent[] percents)
		{
			_percents = DividePercents(count, percents);
		}


		private List<IndexPercent> DividePercents(int count, IndexPercent[] custom)
		{
			// Calculate the remaining percent for division after considering custom values
			float remainingDivision = 1f / count;
			if (custom.Length > 0)
				foreach (IndexPercent index in custom)
				{
					remainingDivision -= index.Percent;
				}

			// Calculate the base percent for each element
			float basePercent = remainingDivision / (count - custom.Length);

			// Create a list to store the final percentages
			List<IndexPercent> percents = new List<IndexPercent>();

			for (int i = 0; i < count; i++)
			{
				if (custom.Any(p => p.Index == i))
				{
					percents.Add(custom.First(p => p.Index == i));
				}
				else
				{
					percents.Add(new IndexPercent(i, basePercent));
				}
			}

			percents = percents.OrderBy(p => p.Index).ToList();

			float sum = percents.Sum(p => p.Percent);
			if (Math.Abs(sum - 1) > 0.0001f)
			{
				// Adjust the base percent slightly to ensure the sum is 1
				float adjustment = (1 - sum) / (count - custom.Length);
				for (int i = 0; i < percents.Count; i++)
				{
					if (custom.All(p => p.Index != i))
					{
						percents[i].Percent += adjustment;
					}
				}
			}
			float cumulative = 0f;

			for (var i = 0; i < percents.Count; i++)
			{
				cumulative += percents[i].Percent;
				percents[i].Cumulative = cumulative;
			}

			return percents;
		}

		public int GetRandom()
		{
			float random = Random.value;
			return _percents.First(p => p.Cumulative > random).Index;
		}

	}

	[Serializable]
	public class IndexPercent
	{
		public int Index;
		[Range(0f, 1f)] public float Percent;
		public float Cumulative;

		public IndexPercent(int index, float percent)
		{
			Index = index;
			Percent = percent;
		}

		public override string ToString()
		{
			return $"{Index} {Percent}: {Cumulative}";
		}
	}
}