using System;
using System.Text;
using System.Text.RegularExpressions;
using DG.Tweening;
using UnityEngine;
public static class DoTweenHelper
{
	[HideInCallstack]
	public static Tween Capture(this Tween tween)
	{
		bool _finished = false;

		string objectName = (tween.target as UnityEngine.Object)?.name;
		string stackTrace = Environment.StackTrace;

		string[] lines = stackTrace.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

		StringBuilder builder = new StringBuilder();

		Regex squareBracketsRemover = new Regex("\\[\\w*\\]");
		Regex fileNumberRemover = new Regex(".cs\\:\\d*");
		Regex fileNameFinder = new Regex("\\w\\:\\\\.*.cs:\\d*");
		Regex beforeSystemGetter = new Regex("^.*?(?=System.Environment)");

		foreach (string line in lines)
		{
			if (line.Contains(nameof(DoTweenHelper))) continue;

			string newLine = line;
			newLine = squareBracketsRemover.Replace(newLine, "");

			string lineNumberRaw;

			if (newLine.Contains(".cs:"))
			{
				lineNumberRaw = fileNumberRemover.Match(newLine).Value.Replace(".cs:", "").Trim();
			}
			else
			{
				lineNumberRaw = "0";
			}

			if (newLine.Contains("System.Environment"))
			{
				newLine = beforeSystemGetter.Match(newLine).Value;
			}

			if (newLine.Contains(".cs:"))
			{
				if (newLine.Contains("at") && newLine.Contains(") "))
				{
					newLine = fileNameFinder.Match(newLine).Value;
				}

				if (newLine.Contains(" in "))
				{
					newLine = $"<a href=\"{fileNumberRemover.Replace(newLine.Replace(" in ", "").Trim(), ".cs")}\" line=\"{lineNumberRaw}\">{newLine},</a>";
				}
				else
				{
					newLine = $"<a href=\"{fileNumberRemover.Replace(newLine, ".cs")}\" line=\"{lineNumberRaw}\">{newLine},</a>";
				}

			}
			builder.AppendLine(newLine);
		}
		stackTrace = builder.ToString();

		if (string.IsNullOrEmpty(objectName))
		{
			objectName = "Unknown object";
		}

		TweenCallback onComplete = tween.onComplete;
		TweenCallback onKill = tween.onKill;

		return tween.OnComplete(() =>
		{
			onComplete?.Invoke();
			_finished = true;
		}).OnKill(() =>
		{
			onKill?.Invoke();
			if (!_finished)
			{
				throw new TweenError($"Tween of {objectName} is killed", stackTrace);
			}
		});
	}
}

public class TweenError : Exception
{
	public override string StackTrace { get; }
	public override string Message { get; }

	public TweenError(string message, string stackTrace)
	{
		StackTrace = stackTrace;
		Message = $"<color=#FF0000>{message}</color>";
	}
}