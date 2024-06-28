using FarmLord.CardModel.DrawQueue;

namespace FarmLord.CardModel {

	public class ActionOutcome : IActionOutcome {

		private readonly StatsModification statsModification;
		private readonly IFollowup followup;

		public ActionOutcome() {
			statsModification = new StatsModification(0, 0, 0, 0);
		}

		public ActionOutcome(int productionMod, int logisticsMod, int economyMod, int satisfactionMod) {
			statsModification = new StatsModification(productionMod, logisticsMod, economyMod, satisfactionMod);
		}

		public ActionOutcome(int productionMod, int logisticsMod, int economyMod, int satisfactionMod, IFollowup followup) {
			statsModification = new StatsModification(productionMod, logisticsMod, economyMod, satisfactionMod);
			this.followup = followup;
		}

		public ActionOutcome(StatsModification statsModification, IFollowup followup) {
			this.statsModification = statsModification;
			this.followup = followup;
		}

		public void Perform(Game controller) {
			statsModification.Perform();
			if (followup != null) {
				controller.AddFollowupCard(followup);
			}
			controller.CardActionPerformed();
		}

	}

}
