using System.Collections.Generic;
using FarmLord.CardModel;
using FarmLord.World;
using UnityEngine;

namespace FarmLord.Gamestate {
	
	public static class Stats {
		
		private const int _maxStatValue = 32;
		private const int _startingProduction = 16;
		private const int _startingLogistics = 16;
		private const int _startingEconomy = 16;
		private const int _startingSatisfaction = 16;
		
		private static readonly List<StatsDisplay> _changeListeners = new List<StatsDisplay>();
		
		public static int Production { get; private set; }
		public static int Logistics { get; private set; }
		public static int Economy { get; private set; }
		public static int Satisfaction { get; private set; }
		
		public static float ProductionPercentage => (float) Production / _maxStatValue;
		public static float LogisticsPercentage => (float) Logistics / _maxStatValue;
		public static float EconomyPercentage => (float) Economy / _maxStatValue;
		public static float SatisfactionPercentage => (float) Satisfaction / _maxStatValue;
		
		public static void ApplyModification(StatsModification mod) {
			Production = ClampValue(Production + mod.production);
			Logistics = ClampValue(Logistics + mod.logistics);
			Economy = ClampValue(Economy + mod.economy);
			Satisfaction = ClampValue(Satisfaction + mod.satisfaction);
			TriggerAllListeners();
		}
		
		public static void ResetStats() {
			ApplyStartingValues();
			TriggerAllListeners();
		}
		
		private static void ApplyStartingValues() {
			Production = ClampValue(_startingProduction);
			Logistics = ClampValue(_startingLogistics);
			Economy = ClampValue(_startingEconomy);
			Satisfaction = ClampValue(_startingSatisfaction);
		}
		
		private static void TriggerAllListeners() {
			for (int i = 0; i < _changeListeners.Count; i++) {
				if (_changeListeners[i] == null) {
					_changeListeners.RemoveAt(i);
				}
				else {
					_changeListeners[i].TriggerUpdate();
				}
			}
		}
		
		public static void AddChangeListener(StatsDisplay listener) {
			_changeListeners.Add(listener);
		}
		
		private static int ClampValue(int value) {
			return Mathf.Clamp(value, 0, _maxStatValue);
		}
		
	}
	
}
