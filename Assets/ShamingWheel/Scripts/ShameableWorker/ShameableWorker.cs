using Bindito.Core;
using Timberborn.BeaversUI;
using Timberborn.GameDistricts;
using Timberborn.Persistence;
using Timberborn.WorkSystem;
using UnityEngine;
using static FrostyMods.ShamingWheel.Constants;

namespace FrostyMods.ShamingWheel
{
    public class ShameableWorker : MonoBehaviour, IPersistentEntity
    {
        private static readonly ComponentKey ShameableWorkerKey = new("ShameableWorker");

        [SerializeField]
        private string _originalWorkerType;
        private static readonly PropertyKey<string> OriginalWorkerTypeKey = new("OriginalWorkerType");

        private Helpers _helperService;
        private Worker _worker;
        private Citizen _citizen;
        private DistrictCenter _district;

        public bool IsShamed => _worker.WorkerType.ToString() == ShamedWorkerType;

        [Inject]
        public void InjectDependencies(Helpers helperService)
        {
            _helperService = helperService;
        }

        public void Awake()
        {
            _worker = GetComponent<Worker>();
            _citizen = GetComponent<Citizen>();
            _district = _citizen.AssignedDistrict;
            _originalWorkerType = _worker.WorkerType.ToString();
        }

        private void SetWorkerType(string workerType)
        {
            if (workerType != null && _worker.WorkerType.ToString() != workerType)
            {
                if (_worker.Employed)
                {
                    _worker.GetComponent<Workplace>()?.UnassignWorker(_worker);
                }

                if (_citizen.HasAssignedDistrict)
                {
                    _district = _citizen.AssignedDistrict;
                    _citizen.UnassignDistrict();
                }

                _helperService.ChangePrivateField(_worker, "_workerType", workerType);

                if (_district != null)
                {
                    // Re-assigning the district is a kludgy way to make the
                    // workplace assigner aware of our cheeky WorkerType switch-a-roo
                    _citizen.AssignDistrict(_district);
                }
            }
        }

        public void StartShaming()
        {
            SetWorkerType(ShamedWorkerType);
            var beaverSelectionSound = GetComponent<BeaverSelectionSound>();
            beaverSelectionSound?.OnSelect();
        }

        public void StopShaming() => SetWorkerType(_originalWorkerType);

        public void ToggleShame()
        {
            if (!IsShamed)
            {
                StartShaming();
                return;
            }

            StopShaming();
        }

        public void Save(IEntitySaver entitySaver)
        {
            // We only need to save the original worker type if it has actually changed
            if (IsShamed)
            {
                entitySaver.GetComponent(ShameableWorkerKey).Set(OriginalWorkerTypeKey, _originalWorkerType);
            }
        }

        public void Load(IEntityLoader entityLoader)
        {
            if (entityLoader.HasComponent(ShameableWorkerKey))
            {
                IObjectLoader component = entityLoader.GetComponent(ShameableWorkerKey);

                if (component.Has(OriginalWorkerTypeKey))
                {
                    _originalWorkerType = component.Get(OriginalWorkerTypeKey);
                }
            }
        }
    }
}