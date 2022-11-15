using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKCalc.Client.Pokemon
{
    public partial class Item
    {
        private readonly PokemonService _service;

        public ItemEnum Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Start { get; private set; }
        public string End { get; private set; }
        public string Heal { get; private set; }
        public string Damage { get; private set; }
        public string Activate { get; private set; }
        public string Block { get; private set; }
        public string Transform { get; private set; }
        public bool IsBattleItem { get; private set; }
        public bool IsBerry { get; private set; }
        public bool IsGem { get; private set; }
        public bool IsChoiceItem { get; private set; }
        public int FlingBasePower { get; private set; }
        public int NaturalGiftBasePower { get; private set; }
        public TypeEnum NaturalGiftType { get; private set; }
        public bool IsZMove { get; private set; }
        public TypeEnum ZMoveType { get; private set; }
        // public MoveEnum ZMove { get; private set; }
        // public MoveEnum ZMoveFrom { get; private set; }
        // public SpeciesEnum MegaEvolves { get; private set; }
        // public List<SpeciesEnum> ItemUser { get; private set; }
        public bool IgnoreKlutz { get; private set; }
        public bool IsDeprecated { get; private set; }
        public int IntroducingGeneration { get; private set; }

        private Item(PokemonService service)
        {
            this._service = service;
        }

        #region JS-Events
        private int onBasePowerPriority;
        private int onBoostPriority;
        private int onModifySpDPriority;
        private int onTryHealPriority;
        private int onResidualOrder;
        private int onResidualSubOrder;
        private int onModifyAccuracyPriority;
        private int onModifyAtkPriority;
        private int onModifySpAPriority;
        private int onFractionalPriorityPriority;
        private int onAttractPriority;
        private int onAfterMoveSecondaryPriority;
        private int onModifyDefPriority;
        private int onDamagePriority;
        private int onModifyMovePriority;
        private int onAfterSetStatusPriority;
        private int onDamagingHitOrder;
        private int onTrapPokemonPriority;
        private int onAfterMoveSecondarySelfPriority;

        private string onTakeItem;
        private string onDamagingHit;
        private string onBasePower;
        private string onBoost;
        private string onAfterBoost;
        private string onUpdate;
        private string onEat;
        private string onStart;
        private string onAfterSubDamage;
        private string onModifySpD;
        private string onDisableMove;
        private string onSourceModifyDamage;
        private string onTryHeal;
        private string onResidual;
        private string onSwitchIn;
        private string onPrimal;
        private string onModifyAccuracy;
        private string onSourceTryPrimaryHit;
        private string onModifyMove;
        private string onModifyAtk;
        private string onModifySpe;
        private string onModifySpA;
        private string onFractionalPriority;
        private string onAttract;
        private string onAfterMoveSecondary;
        private string onAnyTerrainStart;
        private string onHit;
        private string onTryEatItem;
        private string onModifyDef;
        private string onModifyDamage;
        private string onModifyWeight;
        private string onDamage;
        private string onEffectiveness;
        private string onModifyCritRatio;
        private string onTryHit;
        private string onAfterMoveSecondarySelf;
        private string onAfterSetStatus;
        private string onChargeMove;
        private string onImmunity;
        private string onTrapPokemon;

        private string onMemory;
        private string onDrive;
        private string onPlate;
        private string condition;
        private string boosts;

        private string fling;
        #endregion


    }
}
