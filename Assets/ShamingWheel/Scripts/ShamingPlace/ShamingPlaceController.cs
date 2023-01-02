using Bindito.Core.Internal;
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
        public EntityLinker _entityLinker
        {
            get
            {
                return GetComponent<EntityLinker>();
            }
        }

        public ReadOnlyCollection<EntityLinker> _entityLinks {
            get
            {
                return _entityLinker.entityLinks;
            }
        }


        public IEnumerable<GameObject> GetRelations()
        {
            var list = Enumerable.Empty<GameObject>();
            IReadOnlyCollection<EntityLink> links = GetComponent<EntityLinker>()?.EntityLinks;
            
            if (links != null && links.Count > 0) {
                list.AddItem(links.First().Linkee.gameObject);
            }
            
            return list;
        }

        public void Awake()
        {
            _enterable = ((Component)this).GetComponent<Enterable>();
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

            
        }

        // Update is called once per frame
        void Update()
        {
            if (_enterable != null && GetRelations().Count() > 0)
            {
                var shamedBeaver = GetRelations().First();

                if (shamedBeaver != null)
                {
                    Enterer character = shamedBeaver.GetComponent<Enterer>();
                    character?.Enter(_enterable);
                }
            } else
            {
                Plugin.Log.LogWarning("No beavers to shame");
            }
            


        }
    }
}