using Bindito.Core;
using System;
using System.Linq;
using TimberApi.EntityLinkerSystem;
using Timberborn.BeaversUI;
using Timberborn.BotsUI;
using Timberborn.EnterableSystem;
using Timberborn.EntitySystem;
using Timberborn.GameDistricts;
using Timberborn.Localization;
using Timberborn.NeedSystem;
using Timberborn.Persistence;
using Timberborn.TickSystem;
using Timberborn.TimeSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FrostyMods.ShamingWheel.ShameableCharacters
{

    public class ShameableCharacter : TickableComponent, IPersistentEntity, IRegisteredComponent
    {
        private static readonly ComponentKey ShameableCharacterKey = new(nameof(ShameableCharacter));
        private static readonly PropertyKey<bool> IsShamedKey = new("IsShamed");

        // private static readonly PropertyKey<float> CurrentShameKey = new("CurrentShame");

        [SerializeField]
        private string _shameNeedId;
        private Citizen _citizen;
        private NeedManager _needManager;
        private float _currentShame = 0;
        public float ShameProgress => 0;

        public bool IsShamed { get; private set; }

        private IDayNightCycle _dayNightCycle;
        private ILoc _loc;


        [Inject]
        public void InjectDependencies(IDayNightCycle dayNightCycle, ILoc loc)
        {
            _dayNightCycle = dayNightCycle;
            _loc = loc;
        }


        public void Awake()
        {
            _citizen = GetComponent<Citizen>();
            _needManager = GetComponent<NeedManager>();
        }

        public override void Tick()
        {
            /*var enterer = GetComponent<Enterer>();
            var enterable = GetComponent<EntityLink>()?.Linker.GetComponent<Enterable>();
            
            if (enterable != null && _dayNightCycle.IsDaytime)
            {
                enterer?.Enter(enterable);
            } else
            {
                enterer?.Exit();
            }*/
        }

        public float OperationalHoursLeft()
        {
            float val = _currentShame * 24f;
            float val2 = _needManager.NeedHoursLeftToMinimumValue(_shameNeedId);
            return Math.Min(val, val2);
        }

        public void StartShaming()
        {
            IsShamed = true;
        }

        public void StopShaming()
        {
            IsShamed = false;
        }

        public void ToggleShame()
        {
            IsShamed = !IsShamed;

           /* BeaverSelectionSound beaverSelectionSound = _citizen.GetComponent<BeaverSelectionSound>();
            BotSelectionSound botSelectionSound = _citizen.GetComponent<BotSelectionSound>();

            if ((bool)(Object)(object)beaverSelectionSound)
            {
                beaverSelectionSound.PlaySound();
            }
            else if ((bool)(Object)(object)botSelectionSound)
            {
                botSelectionSound.PlaySound();
            }*/
        }

        public Enterable ShamingPlace
        {
            get
            {
                var entityLink = GetComponent<EntityLink>();

                if (entityLink == null)
                {
                    return null;
                }

                return entityLink.Linker.GetComponent<Enterable>();
            }
        }

        public void ExitShamingPlace()
        {
            GetComponent<Enterer>()?.Exit();
        }

        public void Save(IEntitySaver entitySaver)
        {
            entitySaver.GetComponent(ShameableCharacterKey).Set(IsShamedKey, IsShamed);
        }

        public void Load(IEntityLoader entityLoader)
        {
            if (entityLoader.HasComponent(ShameableCharacterKey))
            {
                IObjectLoader component = entityLoader.GetComponent(ShameableCharacterKey);
                IsShamed = component.Get(IsShamedKey);
            }
        }
    }
}