using FarmLord.CardModel.DrawQueue;

namespace FarmLord.CardModel {

	public interface IActionOutcome {

		void Perform(Game controller);

	}

}
