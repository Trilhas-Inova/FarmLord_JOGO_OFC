using FarmLord.Gamestate;
using Outfrost;
using UnityEngine;
using UnityEngine.UI;

namespace FarmLord.World {
	
	public class StatsDisplay : MonoBehaviour {
		
		public Image productionBar;
		public Image logisticsBar;
		public Image economyBar;
		public Image satisfactionBar;
		public float relativeMargin;
		
		private float minFillAmount;
		private float maxFillAmount;
		
		private void Awake() {
			minFillAmount = Mathf.Clamp01(relativeMargin);
			maxFillAmount = Mathf.Clamp01(1.0f - relativeMargin);
			
			if (!Util.IsPrefab(gameObject)) {
				Stats.AddChangeListener(this);
				TriggerUpdate();
			}
		}
		
		public void TriggerUpdate() {
			productionBar.fillAmount = Mathf.Lerp(minFillAmount, maxFillAmount, Stats.ProductionPercentage);
			logisticsBar.fillAmount = Mathf.Lerp(minFillAmount, maxFillAmount, Stats.LogisticsPercentage);
			economyBar.fillAmount = Mathf.Lerp(minFillAmount, maxFillAmount, Stats.EconomyPercentage);
			satisfactionBar.fillAmount = Mathf.Lerp(minFillAmount, maxFillAmount, Stats.SatisfactionPercentage);
		}
		
	}
	
}
