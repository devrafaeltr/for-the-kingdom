using System.Collections.Generic;
using FTKingdom.Utils;
using UnityEngine;

namespace FTKingdom
{
    public class FloatingTextManager : LocalSingleton<FloatingTextManager>
    {
        [SerializeField] private FloatingText floatingTextPrefab = null;

        // TODO: Create a ScriptableObject(probably) to hold colors
        private readonly Dictionary<TextType, Color> textColors = new()
        {
            { TextType.Damage, Color.red },
            { TextType.Heal, Color.green },
            { TextType.Defense, Color.blue },
            { TextType.Evasion, Color.yellow },
            { TextType.Critical, Color.magenta }
        };

        public void Show(string message, Vector3 position, TextType textType)
        {
            FloatingText floatingText = Instantiate(floatingTextPrefab, position, Quaternion.identity);
            floatingText.Setup(message, position, textColors[textType]);
        }
    }
}