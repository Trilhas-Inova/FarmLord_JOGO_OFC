﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmLord.CardModel;
using FarmLord.CardModel.Import;
using FarmLord.CardModel.Import.Resource;
using FarmLord.CardModel.Prerequisite;
using UnityEngine;

namespace FarmLord.Gamestate {

	public class CardStorage {

		private static readonly Character _defaultGameOverCharacter = new Character("", null);

		private readonly Sprite defaultSprite;
		private readonly bool loadRemoteCollectionFirst;

		public Dictionary<int, Card> Cards { get; private set; }
		public Dictionary<string, SpecialCard> SpecialCards { get; private set; }

		public Task CardCollectionImport { get; }

		private List<Card> drawableCards = new List<Card>();

		public CardStorage(Sprite defaultSprite, bool loadRemoteCollectionFirst) {
			this.defaultSprite = defaultSprite;
			this.loadRemoteCollectionFirst = loadRemoteCollectionFirst;
			CardCollectionImport = PopulateCollection();
		}

		public Card Random() {
			return drawableCards[UnityEngine.Random.Range(0, drawableCards.Count)];
		}

		public Card ForId(int id) {
			Card card;
			Cards.TryGetValue(id, out card);
			return card;
		}

		public SpecialCard SpecialCard(string id) {
			SpecialCard card;
			SpecialCards.TryGetValue(id, out card);
			return card;
		}

		public void ResolvePrerequisites() {
			foreach (Card card in Cards.Values) {
				card.ResolvePrerequisites(this);
				if (card.PrerequisitesSatisfied()) {
					AddDrawableCard(card);
				}
			}
		}

		public void AddDrawableCard(Card card) {
			drawableCards.Add(card);
		}

		private async Task PopulateCollection() {
			ImportedCards importedCards =
					await new CollectionImporter(defaultSprite, loadRemoteCollectionFirst).Import();
			Cards = importedCards.cards;
			SpecialCards = importedCards.specialCards;
			if (Cards == null || Cards.Count == 0) {
				PopulateFallback();
			}
			VerifySpecialCards();
		}

		private void PopulateFallback() {
			Cards = new Dictionary<int, Card>();
			Character placeholderPerson = new Character("Placeholder Person", defaultSprite);
			Cards.Add(0, new Card("Placeholder card 1",
					"A",
					"B",
					placeholderPerson,
					new ActionOutcome(-2, 4, -2, 2),
					new ActionOutcome(2, 0, 2, -2),
					new List<ICardPrerequisite>()));
			Cards.Add(1, new Card("Placeholder card 2",
					"A",
					"B",
					placeholderPerson,
					new ActionOutcome(-1, -1, -1, -1),
					new ActionOutcome(2, 2, 2, 2),
					new List<ICardPrerequisite>()));
			Cards.Add(2, new Card("Placeholder card 3",
					"A",
					"B",
					placeholderPerson,
					new ActionOutcome(1, 1, 0, -2),
					new ActionOutcome(2, 2, -2, -4),
					new List<ICardPrerequisite>()));
		}

		private void VerifySpecialCards() {
			if (SpecialCards == null) {
				SpecialCards = new Dictionary<string, SpecialCard>();
			}

			if (!SpecialCards.ContainsKey("gameover_production")) {
				SpecialCards.Add("gameover_production", new SpecialCard("The city runs out of production to run the generator, and freezes over.", "", "",
						_defaultGameOverCharacter,
						new GameOverOutcome(),
						new GameOverOutcome()));
			}
			if (!SpecialCards.ContainsKey("gameover_logistics")) {
				SpecialCards.Add("gameover_logistics", new SpecialCard("Hunger consumes the city, as logistics reserves deplete.", "", "",
						_defaultGameOverCharacter,
						new GameOverOutcome(),
						new GameOverOutcome()));
			}
			if (!SpecialCards.ContainsKey("gameover_economy")) {
				SpecialCards.Add("gameover_economy", new SpecialCard("The city's population succumbs to wounds and spreading diseases.", "", "",
						_defaultGameOverCharacter,
						new GameOverOutcome(),
						new GameOverOutcome()));
			}
			if (!SpecialCards.ContainsKey("gameover_satisfaction")) {
				SpecialCards.Add("gameover_satisfaction", new SpecialCard("All satisfaction among the people is lost.", "", "",
						_defaultGameOverCharacter,
						new GameOverOutcome(),
						new GameOverOutcome()));
			}
		}

	}

}
