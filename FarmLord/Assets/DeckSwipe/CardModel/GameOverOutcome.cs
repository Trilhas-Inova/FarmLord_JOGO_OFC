﻿namespace FarmLord.CardModel {

	public class GameOverOutcome : IActionOutcome {

		public void Perform(Game controller) {
			controller.RestartGame();
		}

	}

}
