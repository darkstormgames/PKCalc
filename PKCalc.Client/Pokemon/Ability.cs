using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKCalc.Client.Pokemon
{
    public partial class Ability
    {
        private readonly PokemonService _service;
        
        public AbilityEnum Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ShortDescription { get; private set; }
        public float Rating { get; private set; }
        
        private Ability(PokemonService service)
        {
            this._service = service;
        }
        
        private bool suppressWeather;
        private bool isBreakable;
        private bool isPermanent;

        #region JSEvents
        private int onModifyTypePriority;
        private int onBasePowerPriority;
        private int onDamagingHitOrder;
        private int onResidualOrder;
        private int onResidualSubOrder;
        private int onAllyBasePowerPriority;
        private int onModifyMovePriority;
        private int onAnyBasePowerPriority;
        private int onDamagePriority;
        private int onFoeBasePowerPriority;
        private int onSourceBasePowerPriority;
        private int onModifyWeightPriority;
        private int onTryHitPriority;
        private int onAnyInvulnerabilityPriority;
        private int onFractionalPriorityPriority;
        private int onTryEatItemPriority;
        private int onSourceModifyDamagePriority;
        private int onDragOutPriority;

        private int onModifyAtkPriority;
        private int onModifyDefPriority;
        private int onModifySpAPriority;
        private int onModifyAccuracyPriority;
        private int onAllyModifyAtkPriority;
        private int onAllyModifySpDPriority;
        private int onSourceModifyAtkPriority;
        private int onSourceModifySpAPriority;
        private int onSourceModifyAccuracyPriority;
        private int onAnyModifyAccuracyPriority;
        private int onAnyFaintPriority;

        private string onModifyMove;
        private string onModifyType;
        private string onBasePower;
        private string onDamagingHit;
        private string onSwitchIn;
        private string onStart;
        private string onHit;
        private string onFoeTrapPokemon;
        private string onFoeMaybeTrapPokemon;
        private string onAllyTryAddVolatile;
        private string onPreStart;
        private string onEnd;
        private string onFoeTryEatItem;
        private string onSourceAfterFaint;
        private string onAnyTryPrimaryHit;
        private string onResidual;
        private string onAllyBasePower;
        private string onCriticalHit;
        private string onDamage;
        private string onTryEatItem;
        private string onAfterMoveSecondary;
        private string onBoost;
        private string onModifyAtk;
        private string onModifySpA;
        private string onTryHit;
        private string onEatItem;
        private string onModifySpe;
        private string onSetStatus;
        private string onAfterEachBoost;
        private string onSourceModifyAccuracy;
        private string onAnyTryMove;
        private string onAnyDamage;
        private string onAnyBasePower;
        private string onFoeTryMove;
        private string onAnySetWeather;
        private string onEffectiveness;
        private string onUpdate;
        private string onFoeBasePower;
        private string onWeather;
        private string onEmergencyExit;
        private string onSourceModifyDamage;
        private string onAllyModifyAtk;
        private string onAllyModifySpD;
        private string onAllyBoost;
        private string onAllySetStatus;
        private string onAnyModifyDamage;
        private string onModifyDef;
        private string onModifyPriority;
        private string onBeforeMove;
        private string onDisableMove;
        private string onSourceTryPrimaryHit;
        private string onSourceBasePower;
        private string onModifyWeight;
        private string onImmunity;
        private string onAnyWeatherStart;
        private string onBeforeSwitchIn;
        private string onFaint;
        private string onTryAddVolatile;
        private string onPrepareHit;
        private string onAnyRedirectTarget;
        private string onSourceTryHeal;
        private string onAllyTryHitSide;
        private string onAfterMoveSecondarySelf;
        private string onModifyCritRatio;
        private string onAnyTerrainStart;
        private string onCheckShow;
        private string onSwitchOut;
        private string onAnyInvulnerability;
        private string onAnyAccuracy;
        private string onSourceModifySecondaries;
        private string onAllySwitchIn;
        private string onAllyFaint;
        private string onDeductPP;
        private string onFractionalPriority;
        private string onAfterBoost;
        private string onTryHeal;
        private string onModifyAccuracy;
        private string onModifySecondaries;
        private string onAnyFaint;
        private string onFlinch;
        private string onTakeItem;
        private string onDragOut;
        private string onAllyAfterUseItem;
        private string onAfterSetStatus;
        private string onSourceModifyAtk;
        private string onAnyModifyBoost;
        private string onAfterUseItem;
        private string onAnyModifyAccuracy;
        private string onSourceModifySpA;
        private string onOther;

        private string condition;
        #endregion
        


    }
}
