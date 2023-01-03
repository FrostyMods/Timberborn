using Bindito.Core.Internal;
using FrostyMods.ShamingWheel.ShameableCharacterPicker;
using FrostyMods.ShamingWheel.ShameableCharacters;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TimberApi.EntityLinkerSystem;
using Timberborn.Buildings;
using Timberborn.Characters;
using Timberborn.ConstructibleSystem;
using Timberborn.EnterableSystem;
using Timberborn.EntitySystem;
using Timberborn.RelationSystem;
using Timberborn.WorkSystem;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace FrostyMods.ShamingWheel.ShamingPlace
{
    public class ShamingPlaceController : MonoBehaviour, IFinishedStateListener, IRegisteredComponent
    {
        public event EventHandler RelationsChanged;
        private Enterable _enterable;

        public ShameableCharacterPickerFragment _pickerFragment;


        public void Awake()
        {
        }

        public void OnEnterFinishedState()
        {
            ((Behaviour)this).enabled = true;
        }

        public void OnExitFinishedState()
        {
            ((Behaviour)this).enabled = false;
        }

        // Start is called before the first frame update
        void Start()
        {
            _enterable = ((Component)this).GetComponent<Enterable>();
            _pickerFragment = ((Component)this).GetComponent<ShameableCharacterPickerFragment>();


        }

        // Update is called once per frame
        void Update()
        {
            /*if (_enterable == null)
            {
                Plugin.Log.LogWarning("Not enterable");
                return;
            }

            if (_pickerFragment == null)
            {
                Plugin.Log.LogWarning("No picker fragment");
                return;
            }

            if (_pickerFragment.Links.Count == 0)
            {
                Plugin.Log.LogWarning("No linked beavers or bots");
                return;
            }

            var link = _pickerFragment.Links.FirstOrDefault();

            if (link != null) { }
                var linkee = link.Linker == _pickerFragment.Linker
                        ? link.Linkee
                        : link.Linker;

                Enterer character = linkee.GetComponent<Enterer>();
                character?.Enter(_enterable);
            }*/
        }
    }
    }
