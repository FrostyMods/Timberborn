using System;
using System.Collections.ObjectModel;
using TimberApi.EntityLinkerSystem;
using TimberApi.UiBuilderSystem;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using Timberborn.EntitySystem;
using Timberborn.Localization;
using Timberborn.SelectionSystem;
using Timberborn.Buildings;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.LengthUnit;
using Timberborn.PrefabSystem;
using Timberborn.WorkSystem;
using Timberborn.Characters;
using FrostyMods.ShamingWheel.ShameableCharacters;
using FrostyMods.ShamingWheel.ShamingPlace;

namespace FrostyMods.ShamingWheel.ShameableCharacterPicker
{
    public class ShameableCharacterPickerFragment : IEntityPanelFragment
    {
        protected readonly UIBuilder _builder;
        protected VisualElement _root;
        public EntityLinker Linker;
        public ReadOnlyCollection<EntityLink> Links => (ReadOnlyCollection<EntityLink>)Linker.EntityLinks;


        protected static string LinkContainerName = "LinkContainer";
        protected static string NewLinkButtonName = "NewLinkButton";

        protected VisualElement _linksContainer;

        protected ShameableCharacterPickerButton _startLinkButton;

        protected ShameableCharacterPickerViewFactory _entityLinkViewFactory;
        protected readonly SelectionManager _selectionManager;
        protected readonly ILoc _loc;

        private int _maxLinks = 1;

        private ShamingPlaceController _character;

        public ShameableCharacterPickerFragment(
            UIBuilder builder,
            ShameableCharacterPickerViewFactory entityLinkViewFactory,
            ShameableCharacterPickerButton startLinkButton,
            SelectionManager selectionManager,
            ILoc loc)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _entityLinkViewFactory = entityLinkViewFactory;
            _startLinkButton = startLinkButton;
            _selectionManager = selectionManager;
            _loc = loc;
        }

        public virtual VisualElement InitializeFragment()
        {
            _root = _builder.CreateFragmentBuilder()
                            .ModifyWrapper(builder => builder.SetFlexDirection(FlexDirection.Row)
                                                             .SetFlexWrap(Wrap.Wrap)
                                                             .SetJustifyContent(Justify.Center))
                            .AddComponent(
                                _builder.CreateComponentBuilder()
                                        .CreateVisualElement()
                                        .SetName(LinkContainerName)
                                        .BuildAndInitialize())
                            .AddComponent(
                                _builder.CreateComponentBuilder()
                                        .CreateButton()
                                        .AddClass("entity-fragment__button--green")
                                        .SetName(NewLinkButtonName)
                                        .SetColor(new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)))
                                        .SetFontSize(new Length(13, Pixel))
                                        .SetFontStyle(FontStyle.Normal)
                                        .SetHeight(new Length(29, Pixel))
                                        .SetWidth(new Length(290, Pixel))
                                        .Build())
                            .BuildAndInitialize();



            _linksContainer = _root.Q<VisualElement>(LinkContainerName);

            _startLinkButton.Initialize<ShameableCharacter>(_root, () => Linker, delegate
            {
                RemoveAllLinkViews();
            });

            _root.ToggleDisplayStyle(false);
            return _root;
        }

        public virtual void ShowFragment(GameObject entity)
        {
            Linker = entity.GetComponent<EntityLinker>();
            _character = entity.GetComponent<ShamingPlaceController>();

            if ((bool)Linker && _character != null)
            {
                AddAllLinkViews();
            }
        }

        public virtual void ClearFragment()
        {
            Linker = null;
            _root.ToggleDisplayStyle(false);
            RemoveAllLinkViews();
        }

        public virtual void UpdateFragment()
        {
            if (Linker != null && _character != null)
            {
                _root.ToggleDisplayStyle(true);
            }
            else
            {
                _root.ToggleDisplayStyle(false);
            }
        }

        /// <summary>
        /// Loops through and adds a view for all existing Links
        /// </summary>
        public virtual void AddAllLinkViews()
        {
            for (int i = 0; i < Links.Count; i++)
            {
                var link = Links[i];

                var linkee = link.Linker == Linker
                    ? link.Linkee
                    : link.Linker;

                var linkeeGameObject = (linkee).gameObject;

                var character = linkeeGameObject.GetComponent<Character>();
                var worker = linkeeGameObject.GetComponent<Worker>();
                var workplace = Linker.gameObject.GetComponent<Workplace>();

                if (workplace != null)
                {
                    if (!workplace.AssignedWorkers.Contains(worker))
                    {
                        var assignedWorkers = workplace.AssignedWorkers;
                        workplace.UnassignAllWorkers();
                        foreach (var assignedWorker in assignedWorkers)
                        {
                           assignedWorker.Unemploy();
                         }

                        worker.EmployAt(workplace);
                        workplace.AssignWorker(worker);

                    }
                }

                var avatar = character.GetComponent<IEntityBadge>().GetEntityAvatar();

                var view = _entityLinkViewFactory.Create(_loc.T(character.FirstName));

                var imageContainer = view.Q<VisualElement>("ImageContainer");
                var img = new Image();
                img.sprite = avatar;
                imageContainer.Add(img);

                var targetButton = view.Q<Button>("Target");
                
                targetButton.clicked += delegate
                {
                    _selectionManager.FocusOn(linkeeGameObject);
                };

                view.Q<Button>("RemoveLinkButton").clicked += delegate
                {
                    link.Linker.DeleteLink(link);
                    ResetLinks();
                };

                _linksContainer.Add(view);
            }

            _startLinkButton.UpdateRemainingSlots(Links.Count, _maxLinks);
        }

        /// <summary>
        /// Resets the link views. 
        /// Used for example when a new Link is added
        /// </summary>
        public virtual void ResetLinks()
        {
            RemoveAllLinkViews();
            AddAllLinkViews();
            UpdateFragment();
        }

        /// <summary>
        /// Removes all existing Link from an entity.
        /// Used for example when the entity is destroyed
        /// </summary>
        public virtual void RemoveAllLinkViews()
        {
            _linksContainer.Clear();
        }
    }
}
