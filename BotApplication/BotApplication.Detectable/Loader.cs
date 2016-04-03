using System;
using System.Linq;
using System.Windows.Forms;
using UnityEngine;

namespace Bot
{
    public class Loader
    {
        public static void Load()
        {
            var cards = GameState.Get().GetCurrentPlayer().GetHandZone().GetCards();
            foreach (var card in cards)
            {
                Alert(card.ToString());
            }

            EndTurnButton.Get().OnEndTurnRequested();
            //SceneMgr.Get().RegisterSceneLoadedEvent(Callback);
        }

        private static void Alert(string alert)
        {
            DialogManager.Get().ShowMessageOfTheDay(alert);
        }

        public static void Unload()
        {
        }

        private static void Callback(SceneMgr.Mode mode, Scene scene, object userdata)
        {
            //Alert(mode.ToString());
        }
    }
}