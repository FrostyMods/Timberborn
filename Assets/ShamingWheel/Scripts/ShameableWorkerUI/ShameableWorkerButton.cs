using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using Timberborn.Localization;
using UnityEngine;
using UnityEngine.UIElements;

namespace FrostyMods.ShamingWheel
{
    public class ShameButtonFragment : IEntityPanelFragment
    {
        private static readonly string ShameLocKey = "frostymods.shamingwheel.Worker.StartShaming";
        private static readonly string StopShamingLocKey = "frostymods.shamingwheel.Worker.StopShaming";

        private readonly VisualElementLoader _visualElementLoader;
        private readonly ILoc _loc;

        private VisualElement _root;
        private Button _button;
        private ShameableWorker _worker;

        public ShameButtonFragment(VisualElementLoader visualElementLoader, ILoc loc)
        {
            _visualElementLoader = visualElementLoader;
            _loc = loc;
        }

        public VisualElement InitializeFragment()
        {
            _root = _visualElementLoader.LoadVisualElement("Master/EntityPanel/DynamiteFragment");
            _button = _root.Q<Button>("Button");
            _button.clicked += ToggleCharacterShame;
            _button.SetEnabled(true);
            _root.ToggleDisplayStyle(visible: false);

            return _root;
        }

        public void ShowFragment(GameObject entity)
        {
            _worker = entity.GetComponent<ShameableWorker>();
        }

        public void UpdateFragment()
        {
            _root.ToggleDisplayStyle(visible: _worker != null);

            if (_worker != null)
            {
                UpdateButton(_worker.IsShamed);
            }
        }

        public void ClearFragment()
        {
            _root.ToggleDisplayStyle(visible: false);
            _worker = null;
        }

        private void UpdateButton(bool isShamed)
        {
            _button.text = _loc.T(isShamed ? StopShamingLocKey : ShameLocKey);
        }

        private void ToggleCharacterShame()
        {
            _worker?.ToggleShame();
        }
    }
}