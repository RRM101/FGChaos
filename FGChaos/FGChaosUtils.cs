using FGChaos.Effects;
using FMODUnity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FGChaos
{
    public static class FGChaosUtils
    {
        public static void LoadBank(string bank)
        {
            if (!RuntimeManager.HasBankLoaded(bank))
            {
                RuntimeManager.LoadBank(bank);
                RuntimeManager.LoadBank($"{bank}.assets");
            }
        }

        public static void UnloadBank(string bank)
        {
            if (RuntimeManager.HasBankLoaded(bank))
            {
                RuntimeManager.UnloadBank(bank);
                RuntimeManager.UnloadBank($"{bank}.assets");
            }
        }

        public static Sprite PNGtoSprite(string path)
        {
            byte[] imagedata = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(0, 0, TextureFormat.ARGB32, false);
            ImageConversion.LoadImage(texture, imagedata);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
            return sprite;
        }

        public static Effect GetEffectForID(string id)
        {
            foreach (Effect effect in EffectList.effects)
            {
                if (effect.ID == id)
                {
                    return effect.Create();
                }
            }
            return null;
        }
    }
}
