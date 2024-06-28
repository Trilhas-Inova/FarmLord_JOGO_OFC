using System.Collections.Generic;
using FarmLord.CardModel.Prerequisite;
using FarmLord.Gamestate;
using UnityEngine;

namespace FarmLord.CardModel {

	public interface ICard {

		string CardText { get; }
		string LeftSwipeText { get; }
		string RightSwipeText { get; }
		string CharacterName { get; }
		Sprite CardSprite { get; }
		ICardProgress Progress { get; }

		void CardShown(Game controller);
		void PerformLeftDecision(Game controller);
		void PerformRightDecision(Game controller);
		void AddDependentCard(Card card);
		void RemoveDependentCard(Card card);

	}

}
