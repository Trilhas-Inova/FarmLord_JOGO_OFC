using FarmLord.Gamestate;

namespace FarmLord.CardModel.DrawQueue {

	public interface IFollowup {

		int Delay { get; set; }

		IFollowup Clone();
		ICard Fetch(CardStorage cardStorage);

	}

}
