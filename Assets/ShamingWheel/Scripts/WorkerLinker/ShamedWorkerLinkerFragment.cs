﻿using System;
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

namespace FrostyMods.ShamingWheel.WorkerLinker
{
    public class ShamedWorkerLinkerFragment : IEntityPanelFragment
    {
        protected readonly UIBuilder _builder;
        protected VisualElement _root;
        protected EntityLinker _entityLinker;

        protected static string LinkContainerName = "LinkContainer";
        protected static string NewLinkButtonName = "NewLinkButton";

        protected VisualElement _linksContainer;

        protected ShamedWorkerLinkerButton _startLinkButton;

        protected ShamedWorkerLinkViewFactory _entityLinkViewFactory;
        protected readonly SelectionManager _selectionManager;
        protected readonly ILoc _loc;

        private int _maxLinks = 1;

        private Building _character;

        public ShamedWorkerLinkerFragment(
            UIBuilder builder,
            ShamedWorkerLinkViewFactory entityLinkViewFactory,
            ShamedWorkerLinkerButton startLinkButton,
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

            _startLinkButton.Initialize<Worker>(_root, () => _entityLinker, delegate
            {
                RemoveAllLinkViews();
            });

            _root.ToggleDisplayStyle(false);
            return _root;
        }

        public virtual void ShowFragment(GameObject entity)
        {
            _entityLinker = entity.GetComponent<EntityLinker>();
            _character = entity.GetComponent<Building>();

            if ((bool)_entityLinker && _character != null)
            {
                AddAllLinkViews();
            }
        }

        public virtual void ClearFragment()
        {
            _entityLinker = null;
            _root.ToggleDisplayStyle(false);
            RemoveAllLinkViews();
        }

        public virtual void UpdateFragment()
        {
            if (_entityLinker != null && _character != null)
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
            ReadOnlyCollection<EntityLink> links = (ReadOnlyCollection<EntityLink>)_entityLinker.EntityLinks;
            for (int i = 0; i < links.Count; i++)
            {
                var link = links[i];

                var linkee = link.Linker == _entityLinker
                    ? link.Linkee
                    : link.Linker;

                var linkeeGameObject = (linkee).gameObject;

                var character = linkeeGameObject.GetComponent<Character>();
                //var avatar = character.GetComponent<IEntityBadge>().GetEntityAvatar();

                var view = _entityLinkViewFactory.Create(_loc.T(character.FirstName));
               // var view = _entityLinkViewFactory.Create(linkeeGameObject.);

                var imageContainer = view.Q<VisualElement>("ImageContainer");
                var img = new Image();
                //img.sprite = avatar;
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

            _startLinkButton.UpdateRemainingSlots(links.Count, _maxLinks);
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
