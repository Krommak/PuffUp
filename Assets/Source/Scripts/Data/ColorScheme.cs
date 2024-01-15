using Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ColorScheme
    {
        [SerializeField]
        private SpecialColor _defaultColor;

        [SerializeField]
        private SpecialColor[] _colors;
        private Dictionary<int, List<SpecialColor>> _colorsByMask;

        public SpecialColor GetColor(LayerMask mask)
        {
            if (_colorsByMask == null)
                Init();

            if (!_colorsByMask.ContainsKey(mask))
            {
                return _defaultColor;
            }
            else
            {
                var count = _colorsByMask[mask].Count;
                if (count > 0)
                {
                    return _colorsByMask[mask][UnityEngine.Random.Range(0, count)];
                }
                else
                {
                    return _colorsByMask[mask].First();
                }
            }
        }

        private void Init()
        {
            _colorsByMask = new Dictionary<int, List<SpecialColor>>();

            foreach (var item in _colors)
            {
                if (_colorsByMask.ContainsKey(item.LayerColor))
                    _colorsByMask[item.LayerColor].Add(item);
                else
                    _colorsByMask.Add(item.LayerColor, new List<SpecialColor>() { item });
            }
        }
    }
}