using FarmLord.Gamestate;

namespace FarmLord.CardModel.Prerequisite {

	public interface ICardPrerequisite {

		CardStatus Status { get; }

		ICard GetCard(CardStorage cardStorage);
		
	}

}
