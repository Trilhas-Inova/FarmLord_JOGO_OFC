using System;
﻿using FarmLord.Gamestate;

namespace FarmLord.CardModel {

	[Serializable]
	public class StatsModification {

		public int production;
		public int logistics;
		public int economy;
		public int satisfaction;

		public StatsModification(int production, int logistics, int economy, int satisfaction) {
			this.production = production;
			this.logistics = logistics;
			this.economy = economy;
			this.satisfaction = satisfaction;
		}

		public void Perform() {
			// TODO Pass through status effects
			Stats.ApplyModification(this);
		}

	}

}
