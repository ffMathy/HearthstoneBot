using System.Windows.Forms;
using UnityEngine;

namespace Bot
{
    public class Loader
    {
        public static void Load()
        {
            MessageBox.Show(GameMenu.Get().IsInGameMenu().ToString());
        }

        public static void Unload()
        {
        }
    }
}