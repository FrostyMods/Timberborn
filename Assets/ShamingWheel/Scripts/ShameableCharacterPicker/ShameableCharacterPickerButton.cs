using System;
using TimberApi.EntityLinkerSystem;
using Timberborn.EntitySystem;
using Timberborn.Localization;
using Timberborn.PickObjectToolSystem;
using Timberborn.SelectionSystem;
using Timberborn.ToolSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace FrostyMods.ShamingWheel.ShameableCharacterPicker
{
    /// <summary>
    /// Defines the button that does the actual linking
    /// </summary>
    public class ShameableCharacterPickerButton
    {
        protected static readonly string PickShamedBeaverTooltipKey = "frostymods.shamingwheel.pickerbutton.tooltip";
        protected static readonly string PickShamedBeaverTitleKey = "frostymods.shamingwheel.pickerbutton.title";
        protected static readonly string PickShamedBeaverKey = "frostymods.shamingwheel.pickerbutton";

        protected readonly ILoc _loc;
        protected readonly PickObjectTool _pickObjectTool;
        protected readonly SelectionManager _selectionManager;
        protected readonly ToolManager _toolManager;
        protected Button _button;

        public ShameableCharacterPickerButton(
            ILoc loc,
            PickObjectTool pickObjectTool,
            SelectionManager selectionManager,
            ToolManager toolManager)
        {
            _loc = loc;
            _pickObjectTool = pickObjectTool;
            _selectionManager = selectionManager;
            _toolManager = toolManager;
        }

        public virtual void Initialize<T>(VisualElement root,
                                       Func<EntityLinker> linkerProvider,
                                       Action createdLinkCallback)
            where T : MonoBehaviour, IRegisteredComponent
        {
            _button = root.Q<Button>("NewLinkButton");
            _button.clicked += delegate
            {
                StartLinkEntities<T>(linkerProvider(), createdLinkCallback);
            };
        }

        /// <summary>
        /// Fires up the object picker tool to select the linkee.
        /// Called when the button is pressed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="linker"></param>
        /// <param name="createdLinkCallback"></param>
        protected virtual void StartLinkEntities<T>(EntityLinker linker,
                                                    Action createdLinkCallback)
            where T : MonoBehaviour, IRegisteredComponent
        {
            _pickObjectTool.StartPicking<T>(
                _loc.T(PickShamedBeaverTitleKey),
                _loc.T(PickShamedBeaverTooltipKey),
                (GameObject gameobject) => ValidateLinkee(linker, gameobject),
                delegate (GameObject linkee)
                {
                    FinishLinkSelection(linker, linkee, createdLinkCallback);
                });
        }

        /// <summary>
        /// Validation logic for the linkee. Return empty string if valid.
        /// Used for example if the entities need to be connected with a path. 
        /// </summary>
        /// <param name="linker"></param>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        protected virtual string ValidateLinkee(
            EntityLinker linker,
            GameObject gameObject)
        {
            return "";
        }

        /// <summary>
        /// Creates a link between entities after the selection
        /// </summary>
        /// <param name="linker"></param>
        /// <param name="linkee"></param>
        /// <param name="createdLinkCallback"></param>
        protected virtual void FinishLinkSelection(
            EntityLinker linker,
            GameObject linkee,
            Action createdLinkCallback)
        {
            EntityLinker linkeeComponent = linkee.GetComponent<EntityLinker>();
            linker.CreateLink(linkeeComponent);
            createdLinkCallback();
        }

        /// <summary>
        /// Updates the label on the linking button
        /// </summary>
        /// <param name="currentLinks"></param>
        /// <param name="maxLinks"></param>
        public virtual void UpdateRemainingSlots(int currentLinks, int maxLinks)
        {
            _button.text = _loc.T(PickShamedBeaverKey);
            _button.SetEnabled(currentLinks < maxLinks);
        }
    }
}
