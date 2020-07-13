using System;

namespace TryToRefactoring
{
	public class Person
	{
		public int Points { get; private set; } = 25;
		public Abilities Abilities { get; private set; } = new Abilities();

		public void Test()
		{
			var age = 0;

			Console.WriteLine("Добро пожаловать в меню выбора создания персонажа!\nУ вас есть 25 очков, которые вы можете распределить по умениям\nНажмите любую клавишу чтобы продолжить...");
			Console.ReadKey();

			while (Points > 0)
			{
				PrintAbilities(age);
				
				Console.WriteLine("Какую характеристику вы хотите изменить?");

				var subject = Console.ReadLine();

				Console.WriteLine(@"Что вы хотите сделать? +\-");

				var operation = Console.ReadLine();

				Console.WriteLine(@"Колличество поинтов которые следует {0}", operation == "+" ? "прибавить" : "отнять");

				int operandPoints;
				string operandPointsRaw;
				
				do
				{
					operandPointsRaw = Console.ReadLine();
				} while (!int.TryParse(operandPointsRaw, out operandPoints));

				switch (subject.ToLower())
				{
					case "сила":
						DistributePoints(operation, "Strength", operandPoints);
						break;
					case "ловкость":
						DistributePoints(operation, "Agility", operandPoints);
						break;
					case "интелект":
						DistributePoints(operation, "Intelligence", operandPoints);
						break;
				}
			}

			Console.WriteLine("Вы распределили все очки. Введите возраст персонажа:");

			string ageRaw;

			do
			{
				ageRaw = Console.ReadLine();
			} while (!int.TryParse(ageRaw, out age));

			PrintAbilities(age);

			Console.ReadKey();
		}

		private void PrintAbilities(int age)
		{
			Console.Clear();

			var strengthVisual = string.Empty;
			var agilityVisual = string.Empty;
			var intelligenceVisual = string.Empty;

			strengthVisual = strengthVisual.PadLeft(Abilities.Strength, '#').PadRight(10, '_');
			agilityVisual = agilityVisual.PadLeft(Abilities.Agility, '#').PadRight(10, '_');
			intelligenceVisual = intelligenceVisual.PadLeft(Abilities.Intelligence, '#').PadRight(10, '_');

			var points = Points == 0 ? string.Empty : $"Поинтов - {Points}\n";

			Console.WriteLine($"{points}Возраст - {age}\nСила - [{strengthVisual}]\nЛовкость - [{agilityVisual}]\nИнтелект - [{intelligenceVisual}]");
		}

		private void DistributePoints(string operation, string abilityName, int operandPoints)
		{
			var type = Abilities.GetType();
			var ability = type.GetProperty(abilityName);
			var abilityValue = (int)ability.GetValue(Abilities);

			if (operation == "+")
			{
				var overhead = operandPoints - (10 - abilityValue);
				overhead = overhead <= 0 ? Points : overhead;
				operandPoints -= overhead;
			}
			else
			{
				var overhead = abilityValue - operandPoints;
				overhead = overhead < 0 ? overhead : 0;
				operandPoints += overhead;
			}

			ability.SetValue(Abilities, operation == "+" ? abilityValue + operandPoints : abilityValue - operandPoints);
			Points = operation == "+" ? Points - operandPoints : Points + operandPoints;
		}
	}
}