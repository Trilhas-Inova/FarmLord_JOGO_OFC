﻿using FarmLord.CardModel;
using FarmLord.CardModel.DrawQueue;
using FarmLord.Gamestate;
using FarmLord.Gamestate.Persistence;
using FarmLord.World;
using Outfrost;
using UnityEngine;

namespace FarmLord {

	public class Game : MonoBehaviour {

		private const int _saveInterval = 8;

		public InputDispatcher inputDispatcher;
		public CardBehaviour cardPrefab;
		public Vector3 spawnPosition;
		public Sprite defaultCharacterSprite;
		public bool loadRemoteCollectionFirst;

		public CardStorage CardStorage {
			get { return cardStorage; }
		}

		private CardStorage cardStorage;
		private ProgressStorage progressStorage;
		private float daysPassedPreviously;
		private float daysLastRun;
		private int saveIntervalCounter;
		private CardDrawQueue cardDrawQueue = new CardDrawQueue();

		private void Awake() {
			// Listen for Escape key ('Back' on Android) that suspends the game on Android
			// or ends it on any other platform
			#if UNITY_ANDROID
			inputDispatcher.AddKeyUpHandler(KeyCode.Escape,
					keyCode => {
						AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
							.GetStatic<AndroidJavaObject>("currentActivity");
						activity.Call<bool>("moveTaskToBack", true);
					});
			#else
			inputDispatcher.AddKeyDownHandler(KeyCode.Escape,
					keyCode => Application.Quit());
			#endif

			cardStorage = new CardStorage(defaultCharacterSprite, loadRemoteCollectionFirst);
			progressStorage = new ProgressStorage(cardStorage);

			GameStartOverlay.FadeOutCallback = StartGameplayLoop;
		}

		private void Start() {
			CallbackWhenDoneLoading(StartGame);
		}

		private void StartGame() {
			daysPassedPreviously = progressStorage.Progress.daysPassed;
			GameStartOverlay.StartSequence(progressStorage.Progress.daysPassed, daysLastRun);
		}

		public void RestartGame() {
			progressStorage.Save();
			daysLastRun = progressStorage.Progress.daysPassed - daysPassedPreviously;
			cardDrawQueue.Clear();
			StartGame();
		}

		private void StartGameplayLoop() {
			Stats.ResetStats();
			ProgressDisplay.SetDaysSurvived(0);
			DrawNextCard();
		}

		public void DrawNextCard() {
			if (Stats.Production == 0) {
				SpawnCard(cardStorage.SpecialCard("gameover_production"));
			}
			else if (Stats.Logistics == 0) {
				SpawnCard(cardStorage.SpecialCard("gameover_logistics"));
			}
			else if (Stats.Economy == 0) {
				SpawnCard(cardStorage.SpecialCard("gameover_economy"));
			}
			else if (Stats.Satisfaction == 0) {
				SpawnCard(cardStorage.SpecialCard("gameover_satisfaction"));
			}
			else {
				IFollowup followup = cardDrawQueue.Next();
				ICard card = followup?.Fetch(cardStorage) ?? cardStorage.Random();
				SpawnCard(card);
			}
			saveIntervalCounter = (saveIntervalCounter - 1) % _saveInterval;
			if (saveIntervalCounter == 0) {
				progressStorage.Save();
			}
		}

		public void CardActionPerformed() {
			progressStorage.Progress.AddDays(Random.Range(0.5f, 1.5f),
					daysPassedPreviously);
			ProgressDisplay.SetDaysSurvived(
					(int)(progressStorage.Progress.daysPassed - daysPassedPreviously));
			DrawNextCard();
		}

		public void AddFollowupCard(IFollowup followup) {
			cardDrawQueue.Insert(followup);
		}

		private async void CallbackWhenDoneLoading(Callback callback) {
			await progressStorage.ProgressStorageInit;
			callback();
		}

		private void SpawnCard(ICard card) {
			CardBehaviour cardInstance = Instantiate(cardPrefab, spawnPosition,
					Quaternion.Euler(0.0f, -180.0f, 0.0f));
			cardInstance.Card = card;
			cardInstance.snapPosition.y = spawnPosition.y;
			cardInstance.Controller = this;
		}

	}

}
