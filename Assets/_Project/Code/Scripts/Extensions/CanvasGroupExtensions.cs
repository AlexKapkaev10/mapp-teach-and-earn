using System;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace Project.Infrastructure.Extensions
{
	[Preserve]
	public static class CanvasGroupExtensions
	{
		public static void Display(this CanvasGroup self, bool show)
		{
			self.alpha = Convert.ToInt32(show);
			self.interactable = show;
			self.blocksRaycasts = show;
		}
		
		public static void AnimationDisplay(this CanvasGroup self, 
			bool show,
			float duration = 0.25f,
			Ease ease = Ease.Linear,
			Action onComplete = null)
		{
			self.DOFade(Convert.ToInt32(show), duration)
				.SetEase(ease)
				.OnComplete(() =>
				{
					onComplete?.Invoke();
				});
			
			self.interactable = show;
			self.blocksRaycasts = show;
		}
		
		public static void AnimationDisplayFrom(this CanvasGroup self, 
			bool show,
			float from,
			float duration = 0.25f,
			Ease ease = Ease.Linear,
			Action onComplete = null)
		{
			self.DOFade(Convert.ToInt32(show), duration)
				.From(from)
				.SetEase(ease)
				.OnComplete(() =>
				{
					onComplete?.Invoke();
				});
			
			self.interactable = show;
			self.blocksRaycasts = show;
		}
	}
}