using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKCalc.Client.Pokemon
{
    public partial class Ability
    {
        public static IEnumerable<Ability> GetAbilities(PokemonService service)
        {
            PokemonService.Instance.Logger.Debug("Getting abilities...");
            return new List<Ability>()
            {
                new Ability(service)
                {
                    Id = AbilityEnum.NoAbility,
                    Name = "No Ability",
                    Description = "",
                    ShortDescription = "Does nothing.",
                    Rating = 0.1f,
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Stench,
                    Name = "Stench",
                    Description = "This Pokemon's attacks without a chance to make the target flinch gain a 10% chance to make the target flinch.",
                    ShortDescription = "This Pokemon's attacks without a chance to flinch gain a 10% chance to flinch.",
                    Rating = 0.5f,
                    onModifyMovePriority = -1,
                    onModifyMove = "onModifyMove(move) {\n			if (move.category !== \"Status\") {\n				this.debug('Adding Stench flinch');\n				if (!move.secondaries) move.secondaries = [];\n				for (const secondary of move.secondaries) {\n					if (secondary.volatileStatus === 'flinch') return;\n				}\n				move.secondaries.push({\n					chance: 10,\n					volatileStatus: 'flinch',\n				});\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Drizzle,
                    Name = "Drizzle",
                    Description = "",
                    ShortDescription = "On switch-in, this Pokemon summons Rain Dance.",
                    Rating = 4f,
                    onStart = "onStart(source) {\n			for (const action of this.queue) {\n				if (action.choice === 'runPrimal' && action.pokemon === source && source.species.id === 'kyogre') return;\n				if (action.choice !== 'runSwitch' && action.choice !== 'runPrimal') break;\n			}\n			this.field.setWeather('raindance');\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.SpeedBoost,
                    Name = "Speed Boost",
                    Description = "This Pokemon's Speed is raised by 1 stage at the end of each full turn it has been on the field.",
                    ShortDescription = "This Pokemon's Speed is raised 1 stage at the end of each full turn on the field.",
                    Rating = 4.5f,
                    onResidualOrder = 28,
                    onResidualSubOrder = 2,
                    onResidual = "onResidual(pokemon) {\n			if (pokemon.activeTurns) {\n				this.boost({spe: 1});\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.BattleArmor,
                    Name = "Battle Armor",
                    Description = "",
                    ShortDescription = "This Pokemon cannot be struck by a critical hit.",
                    Rating = 1f,
                    isBreakable = true,
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Sturdy,
                    Name = "Sturdy",
                    Description = "If this Pokemon is at full HP, it survives one hit with at least 1 HP. OHKO moves fail when used against this Pokemon.",
                    ShortDescription = "If this Pokemon is at full HP, it survives one hit with at least 1 HP. Immune to OHKO.",
                    Rating = 3f,
                    isBreakable = true,
                    onDamagePriority = -30,
                    onDamage = "onDamage(damage, target, source, effect) {\n			if (target.hp === target.maxhp && damage >= target.hp && effect && effect.effectType === 'Move') {\n				this.add('-ability', target, 'Sturdy');\n				return target.hp - 1;\n			}\n		}",
                    onTryHit = "onTryHit(pokemon, target, move) {\n			if (move.ohko) {\n				this.add('-immune', pokemon, '[from] ability: Sturdy');\n				return null;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Damp,
                    Name = "Damp",
                    Description = "While this Pokemon is active, Explosion, Mind Blown, Misty Explosion, Self-Destruct, and the Aftermath Ability are prevented from having an effect.",
                    ShortDescription = "Prevents Explosion/Mind Blown/Misty Explosion/Self-Destruct/Aftermath while active.",
                    Rating = 1f,
                    isBreakable = true,
                    onAnyTryMove = "onAnyTryMove(target, source, effect) {\n			if (['explosion', 'mindblown', 'mistyexplosion', 'selfdestruct'].includes(effect.id)) {\n				this.attrLastMove('[still]');\n				this.add('cant', this.effectState.target, 'ability: Damp', effect, '[of] ' + target);\n				return false;\n			}\n		}",
                    onAnyDamage = "onAnyDamage(damage, target, source, effect) {\n			if (effect && effect.id === 'aftermath') {\n				return false;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Limber,
                    Name = "Limber",
                    Description = "",
                    ShortDescription = "This Pokemon cannot be paralyzed. Gaining this Ability while paralyzed cures it.",
                    Rating = 2f,
                    isBreakable = true,
                    onSetStatus = "onSetStatus(status, target, source, effect) {\n			if (status.id !== 'par') return;\n			if (_optionalChain([(effect ), 'optionalAccess', _8 => _8.status])) {\n				this.add('-immune', target, '[from] ability: Limber');\n			}\n			return false;\n		}",
                    onUpdate = "onUpdate(pokemon) {\n			if (pokemon.status === 'par') {\n				this.add('-activate', pokemon, 'ability: Limber');\n				pokemon.cureStatus();\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.SandVeil,
                    Name = "Sand Veil",
                    Description = "If Sandstorm is active, this Pokemon's evasiveness is multiplied by 1.25. This Pokemon takes no damage from Sandstorm.",
                    ShortDescription = "If Sandstorm is active, this Pokemon's evasiveness is 1.25x; immunity to Sandstorm.",
                    Rating = 1.5f,
                    isBreakable = true,
                    onModifyAccuracyPriority = -1,
                    onImmunity = "onImmunity(type, pokemon) {\n			if (type === 'sandstorm') return false;\n		}",
                    onModifyAccuracy = "onModifyAccuracy(accuracy) {\n			if (typeof accuracy !== 'number') return;\n			if (this.field.isWeather('sandstorm')) {\n				this.debug('Sand Veil - decreasing accuracy');\n				return this.chainModify([3277, 4096]);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Static,
                    Name = "Static",
                    Description = "",
                    ShortDescription = "30% chance a Pokemon making contact with this Pokemon will be paralyzed.",
                    Rating = 2f,
                    onDamagingHit = "onDamagingHit(damage, target, source, move) {\n			if (this.checkMoveMakesContact(move, source, target)) {\n				if (this.randomChance(3, 10)) {\n					source.trySetStatus('par', target);\n				}\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.VoltAbsorb,
                    Name = "Volt Absorb",
                    Description = "This Pokemon is immune to Electric-type moves and restores 1/4 of its maximum HP, rounded down, when hit by an Electric-type move.",
                    ShortDescription = "This Pokemon heals 1/4 of its max HP when hit by Electric moves; Electric immunity.",
                    Rating = 3.5f,
                    isBreakable = true,
                    onTryHit = "onTryHit(target, source, move) {\n			if (target !== source && move.type === 'Electric') {\n				if (!this.heal(target.baseMaxhp / 4)) {\n					this.add('-immune', target, '[from] ability: Volt Absorb');\n				}\n				return null;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.WaterAbsorb,
                    Name = "Water Absorb",
                    Description = "This Pokemon is immune to Water-type moves and restores 1/4 of its maximum HP, rounded down, when hit by a Water-type move.",
                    ShortDescription = "This Pokemon heals 1/4 of its max HP when hit by Water moves; Water immunity.",
                    Rating = 3.5f,
                    isBreakable = true,
                    onTryHit = "onTryHit(target, source, move) {\n			if (target !== source && move.type === 'Water') {\n				if (!this.heal(target.baseMaxhp / 4)) {\n					this.add('-immune', target, '[from] ability: Water Absorb');\n				}\n				return null;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Oblivious,
                    Name = "Oblivious",
                    Description = "This Pokemon cannot be infatuated or taunted. Gaining this Ability while affected cures it. Immune to Intimidate.",
                    ShortDescription = "This Pokemon cannot be infatuated or taunted. Immune to Intimidate.",
                    Rating = 1.5f,
                    isBreakable = true,
                    onBoost = "onBoost(boost, target, source, effect) {\n			if (effect.id === 'intimidate') {\n				delete boost.atk;\n				this.add('-fail', target, 'unboost', 'Attack', '[from] ability: Oblivious', '[of] ' + target);\n			}\n		}",
                    onTryHit = "onTryHit(pokemon, target, move) {\n			if (move.id === 'attract' || move.id === 'captivate' || move.id === 'taunt') {\n				this.add('-immune', pokemon, '[from] ability: Oblivious');\n				return null;\n			}\n		}",
                    onUpdate = "onUpdate(pokemon) {\n			if (pokemon.volatiles['attract']) {\n				this.add('-activate', pokemon, 'ability: Oblivious');\n				pokemon.removeVolatile('attract');\n				this.add('-end', pokemon, 'move: Attract', '[from] ability: Oblivious');\n			}\n			if (pokemon.volatiles['taunt']) {\n				this.add('-activate', pokemon, 'ability: Oblivious');\n				pokemon.removeVolatile('taunt');\n				// Taunt's volatile already sends the -end message when removed\n			}\n		}",
                    onImmunity = "onImmunity(type, pokemon) {\n			if (type === 'attract') return false;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.CloudNine,
                    Name = "Cloud Nine",
                    Description = "",
                    ShortDescription = "While this Pokemon is active, the effects of weather conditions are disabled.",
                    Rating = 2f,
                    suppressWeather = true,
                    onSwitchIn = "onSwitchIn(pokemon) {\n			this.effectState.switchingIn = true;\n		}",
                    onStart = "onStart(pokemon) {\n			// Cloud Nine does not activate when Skill Swapped or when Neutralizing Gas leaves the field\n			if (!this.effectState.switchingIn) return;\n			this.add('-ability', pokemon, 'Cloud Nine');\n			this.effectState.switchingIn = false;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.CompoundEyes,
                    Name = "Compound Eyes",
                    Description = "",
                    ShortDescription = "This Pokemon's moves have their accuracy multiplied by 1.3.",
                    Rating = 3f,
                    onSourceModifyAccuracyPriority = -1,
                    onSourceModifyAccuracy = "onSourceModifyAccuracy(accuracy) {\n			if (typeof accuracy !== 'number') return;\n			this.debug('compoundeyes - enhancing accuracy');\n			return this.chainModify([5325, 4096]);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Insomnia,
                    Name = "Insomnia",
                    Description = "",
                    ShortDescription = "This Pokemon cannot fall asleep. Gaining this Ability while asleep cures it.",
                    Rating = 2f,
                    isBreakable = true,
                    onSetStatus = "onSetStatus(status, target, source, effect) {\n			if (status.id !== 'slp') return;\n			if (_optionalChain([(effect ), 'optionalAccess', _6 => _6.status])) {\n				this.add('-immune', target, '[from] ability: Insomnia');\n			}\n			return false;\n		}",
                    onUpdate = "onUpdate(pokemon) {\n			if (pokemon.status === 'slp') {\n				this.add('-activate', pokemon, 'ability: Insomnia');\n				pokemon.cureStatus();\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.ColorChange,
                    Name = "Color Change",
                    Description = "This Pokemon's type changes to match the type of the last move that hit it, unless that type is already one of its types. This effect applies after all hits from a multi-hit move; Sheer Force prevents it from activating if the move has a secondary effect.",
                    ShortDescription = "This Pokemon's type changes to the type of a move it's hit by, unless it has the type.",
                    Rating = 0f,
                    onAfterMoveSecondary = "onAfterMoveSecondary(target, source, move) {\n			if (!target.hp) return;\n			const type = move.type;\n			if (\n				target.isActive && move.effectType === 'Move' && move.category !== 'Status' &&\n				type !== '???' && !target.hasType(type)\n			) {\n				if (!target.setType(type)) return false;\n				this.add('-start', target, 'typechange', type, '[from] ability: Color Change');\n\n				if (target.side.active.length === 2 && target.position === 1) {\n					// Curse Glitch\n					const action = this.queue.willMove(target);\n					if (action && action.move.id === 'curse') {\n						action.targetLoc = -1;\n					}\n				}\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Immunity,
                    Name = "Immunity",
                    Description = "",
                    ShortDescription = "This Pokemon cannot be poisoned. Gaining this Ability while poisoned cures it.",
                    Rating = 2f,
                    isBreakable = true,
                    onSetStatus = "onSetStatus(status, target, source, effect) {\n			if (status.id !== 'psn' && status.id !== 'tox') return;\n			if (_optionalChain([(effect ), 'optionalAccess', _5 => _5.status])) {\n				this.add('-immune', target, '[from] ability: Immunity');\n			}\n			return false;\n		}",
                    onUpdate = "onUpdate(pokemon) {\n			if (pokemon.status === 'psn' || pokemon.status === 'tox') {\n				this.add('-activate', pokemon, 'ability: Immunity');\n				pokemon.cureStatus();\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.FlashFire,
                    Name = "Flash Fire",
                    Description = "This Pokemon is immune to Fire-type moves. The first time it is hit by a Fire-type move, its attacking stat is multiplied by 1.5 while using a Fire-type attack as long as it remains active and has this Ability. If this Pokemon is frozen, it cannot be defrosted by Fire-type attacks.",
                    ShortDescription = "This Pokemon's Fire attacks do 1.5x damage if hit by one Fire move; Fire immunity.",
                    Rating = 3.5f,
                    isBreakable = true,
                    onEnd = "onEnd(pokemon) {\n			pokemon.removeVolatile('flashfire');\n		}",
                    onTryHit = "onTryHit(target, source, move) {\n			if (target !== source && move.type === 'Fire') {\n				move.accuracy = true;\n				if (!target.addVolatile('flashfire')) {\n					this.add('-immune', target, '[from] ability: Flash Fire');\n				}\n				return null;\n			}\n		}",
                    condition = "[object Object]",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.ShieldDust,
                    Name = "Shield Dust",
                    Description = "",
                    ShortDescription = "This Pokemon is not affected by the secondary effect of another Pokemon's attack.",
                    Rating = 2f,
                    isBreakable = true,
                    onModifySecondaries = "onModifySecondaries(secondaries) {\n			this.debug('Shield Dust prevent secondary');\n			return secondaries.filter(effect => !!(effect.self || effect.dustproof));\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.OwnTempo,
                    Name = "Own Tempo",
                    Description = "This Pokemon cannot be confused. Gaining this Ability while confused cures it. Immune to Intimidate.",
                    ShortDescription = "This Pokemon cannot be confused. Immune to Intimidate.",
                    Rating = 1.5f,
                    isBreakable = true,
                    onHit = "onHit(target, source, move) {\n			if (_optionalChain([move, 'optionalAccess', _10 => _10.volatileStatus]) === 'confusion') {\n				this.add('-immune', target, 'confusion', '[from] ability: Own Tempo');\n			}\n		}",
                    onBoost = "onBoost(boost, target, source, effect) {\n			if (effect.id === 'intimidate') {\n				delete boost.atk;\n				this.add('-fail', target, 'unboost', 'Attack', '[from] ability: Own Tempo', '[of] ' + target);\n			}\n		}",
                    onUpdate = "onUpdate(pokemon) {\n			if (pokemon.volatiles['confusion']) {\n				this.add('-activate', pokemon, 'ability: Own Tempo');\n				pokemon.removeVolatile('confusion');\n			}\n		}",
                    onTryAddVolatile = "onTryAddVolatile(status, pokemon) {\n			if (status.id === 'confusion') return null;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.SuctionCups,
                    Name = "Suction Cups",
                    Description = "",
                    ShortDescription = "This Pokemon cannot be forced to switch out by another Pokemon's attack or item.",
                    Rating = 1f,
                    isBreakable = true,
                    onDragOutPriority = 1,
                    onDragOut = "onDragOut(pokemon) {\n			this.add('-activate', pokemon, 'ability: Suction Cups');\n			return null;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Intimidate,
                    Name = "Intimidate",
                    Description = "On switch-in, this Pokemon lowers the Attack of adjacent opposing Pokemon by 1 stage. Inner Focus, Oblivious, Own Tempo, Scrappy, and Pokemon behind a substitute are immune.",
                    ShortDescription = "On switch-in, this Pokemon lowers the Attack of adjacent opponents by 1 stage.",
                    Rating = 3.5f,
                    onStart = "onStart(pokemon) {\n			let activated = false;\n			for (const target of pokemon.adjacentFoes()) {\n				if (!activated) {\n					this.add('-ability', pokemon, 'Intimidate', 'boost');\n					activated = true;\n				}\n				if (target.volatiles['substitute']) {\n					this.add('-immune', target);\n				} else {\n					this.boost({atk: -1}, target, pokemon, null, true);\n				}\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.ShadowTag,
                    Name = "Shadow Tag",
                    Description = "Prevents adjacent opposing Pokemon from choosing to switch out unless they are immune to trapping or also have this Ability.",
                    ShortDescription = "Prevents adjacent foes from choosing to switch unless they also have this Ability.",
                    Rating = 5f,
                    onFoeTrapPokemon = "onFoeTrapPokemon(pokemon) {\n			if (!pokemon.hasAbility('shadowtag') && pokemon.isAdjacent(this.effectState.target)) {\n				pokemon.tryTrap(true);\n			}\n		}",
                    onFoeMaybeTrapPokemon = "onFoeMaybeTrapPokemon(pokemon, source) {\n			if (!source) source = this.effectState.target;\n			if (!source || !pokemon.isAdjacent(source)) return;\n			if (!pokemon.hasAbility('shadowtag')) {\n				pokemon.maybeTrapped = true;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.RoughSkin,
                    Name = "Rough Skin",
                    Description = "Pokemon making contact with this Pokemon lose 1/8 of their maximum HP, rounded down.",
                    ShortDescription = "Pokemon making contact with this Pokemon lose 1/8 of their max HP.",
                    Rating = 2.5f,
                    onDamagingHitOrder = 1,
                    onDamagingHit = "onDamagingHit(damage, target, source, move) {\n			if (this.checkMoveMakesContact(move, source, target, true)) {\n				this.damage(source.baseMaxhp / 8, source, target);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.WonderGuard,
                    Name = "Wonder Guard",
                    Description = "",
                    ShortDescription = "This Pokemon can only be damaged by supereffective moves and indirect damage.",
                    Rating = 5f,
                    isBreakable = true,
                    onTryHit = "onTryHit(target, source, move) {\n			if (target === source || move.category === 'Status' || move.type === '???' || move.id === 'struggle') return;\n			if (move.id === 'skydrop' && !source.volatiles['skydrop']) return;\n			this.debug('Wonder Guard immunity: ' + move.id);\n			if (target.runEffectiveness(move) <= 0) {\n				if (move.smartTarget) {\n					move.smartTarget = false;\n				} else {\n					this.add('-immune', target, '[from] ability: Wonder Guard');\n				}\n				return null;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Levitate,
                    Name = "Levitate",
                    Description = "This Pokemon is immune to Ground-type attacks and the effects of Spikes, Toxic Spikes, Sticky Web, and the Arena Trap Ability. The effects of Gravity, Ingrain, Smack Down, Thousand Arrows, and Iron Ball nullify the immunity.",
                    ShortDescription = "This Pokemon is immune to Ground; Gravity/Ingrain/Smack Down/Iron Ball nullify it.",
                    Rating = 3.5f,
                    isBreakable = true,
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.EffectSpore,
                    Name = "Effect Spore",
                    Description = "30% chance a Pokemon making contact with this Pokemon will be poisoned, paralyzed, or fall asleep.",
                    ShortDescription = "30% chance of poison/paralysis/sleep on others making contact with this Pokemon.",
                    Rating = 2f,
                    onDamagingHit = "onDamagingHit(damage, target, source, move) {\n			if (this.checkMoveMakesContact(move, source, target) && !source.status && source.runStatusImmunity('powder')) {\n				const r = this.random(100);\n				if (r < 11) {\n					source.setStatus('slp', target);\n				} else if (r < 21) {\n					source.setStatus('par', target);\n				} else if (r < 30) {\n					source.setStatus('psn', target);\n				}\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Synchronize,
                    Name = "Synchronize",
                    Description = "If another Pokemon burns, paralyzes, poisons, or badly poisons this Pokemon, that Pokemon receives the same non-volatile status condition.",
                    ShortDescription = "If another Pokemon burns/poisons/paralyzes this Pokemon, it also gets that status.",
                    Rating = 2f,
                    onAfterSetStatus = "onAfterSetStatus(status, target, source, effect) {\n			if (!source || source === target) return;\n			if (effect && effect.id === 'toxicspikes') return;\n			if (status.id === 'slp' || status.id === 'frz') return;\n			this.add('-activate', target, 'ability: Synchronize');\n			// Hack to make status-prevention abilities think Synchronize is a status move\n			// and show messages when activating against it.\n			source.trySetStatus(status, target, {status: status.id, id: 'synchronize'} );\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.ClearBody,
                    Name = "Clear Body",
                    Description = "",
                    ShortDescription = "Prevents other Pokemon from lowering this Pokemon's stat stages.",
                    Rating = 2f,
                    isBreakable = true,
                    onBoost = "onBoost(boost, target, source, effect) {\n			if (source && target === source) return;\n			let showMsg = false;\n			let i;\n			for (i in boost) {\n				if (boost[i] < 0) {\n					delete boost[i];\n					showMsg = true;\n				}\n			}\n			if (showMsg && !(effect ).secondaries && effect.id !== 'octolock') {\n				this.add(\"-fail\", target, \"unboost\", \"[from] ability: Clear Body\", \"[of] \" + target);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.NaturalCure,
                    Name = "Natural Cure",
                    Description = "",
                    ShortDescription = "This Pokemon has its non-volatile status condition cured when it switches out.",
                    Rating = 2.5f,
                    onCheckShow = "onCheckShow(pokemon) {\n			// This is complicated\n			// For the most part, in-game, it's obvious whether or not Natural Cure activated,\n			// since you can see how many of your opponent's pokemon are statused.\n			// The only ambiguous situation happens in Doubles/Triples, where multiple pokemon\n			// that could have Natural Cure switch out, but only some of them get cured.\n			if (pokemon.side.active.length === 1) return;\n			if (pokemon.showCure === true || pokemon.showCure === false) return;\n\n			const cureList = [];\n			let noCureCount = 0;\n			for (const curPoke of pokemon.side.active) {\n				// pokemon not statused\n				if (!_optionalChain([curPoke, 'optionalAccess', _9 => _9.status])) {\n					// this.add('-message', \"\" + curPoke + \" skipped: not statused or doesn't exist\");\n					continue;\n				}\n				if (curPoke.showCure) {\n					// this.add('-message', \"\" + curPoke + \" skipped: Natural Cure already known\");\n					continue;\n				}\n				const species = curPoke.species;\n				// pokemon can't get Natural Cure\n				if (!Object.values(species.abilities).includes('Natural Cure')) {\n					// this.add('-message', \"\" + curPoke + \" skipped: no Natural Cure\");\n					continue;\n				}\n				// pokemon's ability is known to be Natural Cure\n				if (!species.abilities['1'] && !species.abilities['H']) {\n					// this.add('-message', \"\" + curPoke + \" skipped: only one ability\");\n					continue;\n				}\n				// pokemon isn't switching this turn\n				if (curPoke !== pokemon && !this.queue.willSwitch(curPoke)) {\n					// this.add('-message', \"\" + curPoke + \" skipped: not switching\");\n					continue;\n				}\n\n				if (curPoke.hasAbility('naturalcure')) {\n					// this.add('-message', \"\" + curPoke + \" confirmed: could be Natural Cure (and is)\");\n					cureList.push(curPoke);\n				} else {\n					// this.add('-message', \"\" + curPoke + \" confirmed: could be Natural Cure (but isn't)\");\n					noCureCount++;\n				}\n			}\n\n			if (!cureList.length || !noCureCount) {\n				// It's possible to know what pokemon were cured\n				for (const pkmn of cureList) {\n					pkmn.showCure = true;\n				}\n			} else {\n				// It's not possible to know what pokemon were cured\n\n				// Unlike a -hint, this is real information that battlers need, so we use a -message\n				this.add('-message', \"(\" + cureList.length + \" of \" + pokemon.side.name + \"'s pokemon \" + (cureList.length === 1 ? \"was\" : \"were\") + \" cured by Natural Cure.)\");\n\n				for (const pkmn of cureList) {\n					pkmn.showCure = false;\n				}\n			}\n		}",
                    onSwitchOut = "onSwitchOut(pokemon) {\n			if (!pokemon.status) return;\n\n			// if pokemon.showCure is undefined, it was skipped because its ability\n			// is known\n			if (pokemon.showCure === undefined) pokemon.showCure = true;\n\n			if (pokemon.showCure) this.add('-curestatus', pokemon, pokemon.status, '[from] ability: Natural Cure');\n			pokemon.setStatus('');\n\n			// only reset .showCure if it's false\n			// (once you know a Pokemon has Natural Cure, its cures are always known)\n			if (!pokemon.showCure) pokemon.showCure = undefined;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.LightningRod,
                    Name = "Lightning Rod",
                    Description = "This Pokemon is immune to Electric-type moves and raises its Special Attack by 1 stage when hit by an Electric-type move. If this Pokemon is not the target of a single-target Electric-type move used by another Pokemon, this Pokemon redirects that move to itself if it is within the range of that move.",
                    ShortDescription = "This Pokemon draws Electric moves to itself to raise Sp. Atk by 1; Electric immunity.",
                    Rating = 3f,
                    isBreakable = true,
                    onTryHit = "onTryHit(target, source, move) {\n			if (target !== source && move.type === 'Electric') {\n				if (!this.boost({spa: 1})) {\n					this.add('-immune', target, '[from] ability: Lightning Rod');\n				}\n				return null;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.SereneGrace,
                    Name = "Serene Grace",
                    Description = "",
                    ShortDescription = "This Pokemon's moves have their secondary effect chance doubled.",
                    Rating = 3.5f,
                    onModifyMovePriority = -2,
                    onModifyMove = "onModifyMove(move) {\n			if (move.secondaries) {\n				this.debug('doubling secondary chance');\n				for (const secondary of move.secondaries) {\n					if (secondary.chance) secondary.chance *= 2;\n				}\n			}\n			if (_optionalChain([move, 'access', _18 => _18.self, 'optionalAccess', _19 => _19.chance])) move.self.chance *= 2;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.SwiftSwim,
                    Name = "Swift Swim",
                    Description = "If Rain Dance is active and this Pokemon is not holding Utility Umbrella, this Pokemon's Speed is doubled.",
                    ShortDescription = "If Rain Dance is active, this Pokemon's Speed is doubled.",
                    Rating = 3f,
                    onModifySpe = "onModifySpe(spe, pokemon) {\n			if (['raindance', 'primordialsea'].includes(pokemon.effectiveWeather())) {\n				return this.chainModify(2);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Chlorophyll,
                    Name = "Chlorophyll",
                    Description = "If Sunny Day is active and this Pokemon is not holding Utility Umbrella, this Pokemon's Speed is doubled.",
                    ShortDescription = "If Sunny Day is active, this Pokemon's Speed is doubled.",
                    Rating = 3f,
                    onModifySpe = "onModifySpe(spe, pokemon) {\n			if (['sunnyday', 'desolateland'].includes(pokemon.effectiveWeather())) {\n				return this.chainModify(2);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Illuminate,
                    Name = "Illuminate",
                    Description = "",
                    ShortDescription = "No competitive use.",
                    Rating = 0f,
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Trace,
                    Name = "Trace",
                    Description = "On switch-in, or when this Pokemon acquires this ability, this Pokemon copies a random adjacent opposing Pokemon's Ability. However, if one or more adjacent Pokemon has the Ability \"No Ability\", Trace won't copy anything even if there is another valid Ability it could normally copy. Otherwise, if there is no Ability that can be copied at that time, this Ability will activate as soon as an Ability can be copied. Abilities that cannot be copied are the previously mentioned \"No Ability\", as well as As One, Battle Bond, Comatose, Disguise, Flower Gift, Forecast, Gulp Missile, Hunger Switch, Ice Face, Illusion, Imposter, Multitype, Neutralizing Gas, Power Construct, Power of Alchemy, Receiver, RKS System, Schooling, Shields Down, Stance Change, Trace, and Zen Mode.",
                    ShortDescription = "On switch-in, or when it can, this Pokemon copies a random adjacent foe's Ability.",
                    Rating = 2.5f,
                    onStart = "onStart(pokemon) {\n			// n.b. only affects Hackmons\n			// interaction with No Ability is complicated: https://www.smogon.com/forums/threads/pokemon-sun-moon-battle-mechanics-research.3586701/page-76#post-7790209\n			if (pokemon.adjacentFoes().some(foeActive => foeActive.ability === 'noability')) {\n				this.effectState.gaveUp = true;\n			}\n		}",
                    onUpdate = "onUpdate(pokemon) {\n			if (!pokemon.isStarted || this.effectState.gaveUp) return;\n\n			const additionalBannedAbilities = [\n				// Zen Mode included here for compatability with Gen 5-6\n				'noability', 'flowergift', 'forecast', 'hungerswitch', 'illusion', 'imposter', 'neutralizinggas', 'powerofalchemy', 'receiver', 'trace', 'zenmode',\n			];\n			const possibleTargets = pokemon.adjacentFoes().filter(target => (\n				!target.getAbility().isPermanent && !additionalBannedAbilities.includes(target.ability)\n			));\n			if (!possibleTargets.length) return;\n\n			const target = this.sample(possibleTargets);\n			const ability = target.getAbility();\n			this.add('-ability', pokemon, ability, '[from] ability: Trace', '[of] ' + target);\n			pokemon.setAbility(ability);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.HugePower,
                    Name = "Huge Power",
                    Description = "",
                    ShortDescription = "This Pokemon's Attack is doubled.",
                    Rating = 5f,
                    onModifyAtkPriority = 5,
                    onModifyAtk = "onModifyAtk(atk) {\n			return this.chainModify(2);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.PoisonPoint,
                    Name = "Poison Point",
                    Description = "",
                    ShortDescription = "30% chance a Pokemon making contact with this Pokemon will be poisoned.",
                    Rating = 1.5f,
                    onDamagingHit = "onDamagingHit(damage, target, source, move) {\n			if (this.checkMoveMakesContact(move, source, target)) {\n				if (this.randomChance(3, 10)) {\n					source.trySetStatus('psn', target);\n				}\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.InnerFocus,
                    Name = "Inner Focus",
                    Description = "",
                    ShortDescription = "This Pokemon cannot be made to flinch. Immune to Intimidate.",
                    Rating = 1.5f,
                    isBreakable = true,
                    onBoost = "onBoost(boost, target, source, effect) {\n			if (effect.id === 'intimidate') {\n				delete boost.atk;\n				this.add('-fail', target, 'unboost', 'Attack', '[from] ability: Inner Focus', '[of] ' + target);\n			}\n		}",
                    onTryAddVolatile = "onTryAddVolatile(status, pokemon) {\n			if (status.id === 'flinch') return null;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.MagmaArmor,
                    Name = "Magma Armor",
                    Description = "",
                    ShortDescription = "This Pokemon cannot be frozen. Gaining this Ability while frozen cures it.",
                    Rating = 1f,
                    isBreakable = true,
                    onUpdate = "onUpdate(pokemon) {\n			if (pokemon.status === 'frz') {\n				this.add('-activate', pokemon, 'ability: Magma Armor');\n				pokemon.cureStatus();\n			}\n		}",
                    onImmunity = "onImmunity(type, pokemon) {\n			if (type === 'frz') return false;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.WaterVeil,
                    Name = "Water Veil",
                    Description = "",
                    ShortDescription = "This Pokemon cannot be burned. Gaining this Ability while burned cures it.",
                    Rating = 2f,
                    isBreakable = true,
                    onSetStatus = "onSetStatus(status, target, source, effect) {\n			if (status.id !== 'brn') return;\n			if (_optionalChain([(effect ), 'optionalAccess', _27 => _27.status])) {\n				this.add('-immune', target, '[from] ability: Water Veil');\n			}\n			return false;\n		}",
                    onUpdate = "onUpdate(pokemon) {\n			if (pokemon.status === 'brn') {\n				this.add('-activate', pokemon, 'ability: Water Veil');\n				pokemon.cureStatus();\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.MagnetPull,
                    Name = "Magnet Pull",
                    Description = "Prevents adjacent opposing Steel-type Pokemon from choosing to switch out unless they are immune to trapping.",
                    ShortDescription = "Prevents adjacent Steel-type foes from choosing to switch.",
                    Rating = 4f,
                    onFoeTrapPokemon = "onFoeTrapPokemon(pokemon) {\n			if (pokemon.hasType('Steel') && pokemon.isAdjacent(this.effectState.target)) {\n				pokemon.tryTrap(true);\n			}\n		}",
                    onFoeMaybeTrapPokemon = "onFoeMaybeTrapPokemon(pokemon, source) {\n			if (!source) source = this.effectState.target;\n			if (!source || !pokemon.isAdjacent(source)) return;\n			if (!pokemon.knownType || pokemon.hasType('Steel')) {\n				pokemon.maybeTrapped = true;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Soundproof,
                    Name = "Soundproof",
                    Description = "",
                    ShortDescription = "This Pokemon is immune to sound-based moves, including Heal Bell.",
                    Rating = 1.5f,
                    isBreakable = true,
                    onTryHit = "onTryHit(target, source, move) {\n			if (target !== source && move.flags['sound']) {\n				this.add('-immune', target, '[from] ability: Soundproof');\n				return null;\n			}\n		}",
                    onAllyTryHitSide = "onAllyTryHitSide(target, source, move) {\n			if (move.flags['sound']) {\n				this.add('-immune', this.effectState.target, '[from] ability: Soundproof');\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.RainDish,
                    Name = "Rain Dish",
                    Description = "If Rain Dance is active, this Pokemon restores 1/16 of its maximum HP, rounded down, at the end of each turn. If this Pokemon is holding Utility Umbrella, its HP does not get restored.",
                    ShortDescription = "If Rain Dance is active, this Pokemon heals 1/16 of its max HP each turn.",
                    Rating = 1.5f,
                    onWeather = "onWeather(target, source, effect) {\n			if (target.hasItem('utilityumbrella')) return;\n			if (effect.id === 'raindance' || effect.id === 'primordialsea') {\n				this.heal(target.baseMaxhp / 16);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.SandStream,
                    Name = "Sand Stream",
                    Description = "",
                    ShortDescription = "On switch-in, this Pokemon summons Sandstorm.",
                    Rating = 4f,
                    onStart = "onStart(source) {\n			this.field.setWeather('sandstorm');\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Pressure,
                    Name = "Pressure",
                    Description = "If this Pokemon is the target of an opposing Pokemon's move, that move loses one additional PP.",
                    ShortDescription = "If this Pokemon is the target of a foe's move, that move loses one additional PP.",
                    Rating = 2.5f,
                    onStart = "onStart(pokemon) {\n			this.add('-ability', pokemon, 'Pressure');\n		}",
                    onDeductPP = "onDeductPP(target, source) {\n			if (target.isAlly(source)) return;\n			return 1;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.ThickFat,
                    Name = "Thick Fat",
                    Description = "If a Pokemon uses a Fire- or Ice-type attack against this Pokemon, that Pokemon's attacking stat is halved when calculating the damage to this Pokemon.",
                    ShortDescription = "Fire/Ice-type moves against this Pokemon deal damage with a halved attacking stat.",
                    Rating = 3.5f,
                    isBreakable = true,
                    onSourceModifyAtkPriority = 6,
                    onSourceModifySpAPriority = 5,
                    onSourceModifyAtk = "onSourceModifyAtk(atk, attacker, defender, move) {\n			if (move.type === 'Ice' || move.type === 'Fire') {\n				this.debug('Thick Fat weaken');\n				return this.chainModify(0.5);\n			}\n		}",
                    onSourceModifySpA = "onSourceModifySpA(atk, attacker, defender, move) {\n			if (move.type === 'Ice' || move.type === 'Fire') {\n				this.debug('Thick Fat weaken');\n				return this.chainModify(0.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.EarlyBird,
                    Name = "Early Bird",
                    Description = "",
                    ShortDescription = "This Pokemon's sleep counter drops by 2 instead of 1.",
                    Rating = 1.5f,
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.FlameBody,
                    Name = "Flame Body",
                    Description = "",
                    ShortDescription = "30% chance a Pokemon making contact with this Pokemon will be burned.",
                    Rating = 2f,
                    onDamagingHit = "onDamagingHit(damage, target, source, move) {\n			if (this.checkMoveMakesContact(move, source, target)) {\n				if (this.randomChance(3, 10)) {\n					source.trySetStatus('brn', target);\n				}\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.RunAway,
                    Name = "Run Away",
                    Description = "",
                    ShortDescription = "No competitive use.",
                    Rating = 0f,
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.KeenEye,
                    Name = "Keen Eye",
                    Description = "Prevents other Pokemon from lowering this Pokemon's accuracy stat stage. This Pokemon ignores a target's evasiveness stat stage.",
                    ShortDescription = "This Pokemon's accuracy can't be lowered by others; ignores their evasiveness stat.",
                    Rating = 0.5f,
                    isBreakable = true,
                    onModifyMove = "onModifyMove(move) {\n			move.ignoreEvasion = true;\n		}",
                    onBoost = "onBoost(boost, target, source, effect) {\n			if (source && target === source) return;\n			if (boost.accuracy && boost.accuracy < 0) {\n				delete boost.accuracy;\n				if (!(effect ).secondaries) {\n					this.add(\"-fail\", target, \"unboost\", \"accuracy\", \"[from] ability: Keen Eye\", \"[of] \" + target);\n				}\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.HyperCutter,
                    Name = "Hyper Cutter",
                    Description = "",
                    ShortDescription = "Prevents other Pokemon from lowering this Pokemon's Attack stat stage.",
                    Rating = 1.5f,
                    isBreakable = true,
                    onBoost = "onBoost(boost, target, source, effect) {\n			if (source && target === source) return;\n			if (boost.atk && boost.atk < 0) {\n				delete boost.atk;\n				if (!(effect ).secondaries) {\n					this.add(\"-fail\", target, \"unboost\", \"Attack\", \"[from] ability: Hyper Cutter\", \"[of] \" + target);\n				}\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Pickup,
                    Name = "Pickup",
                    Description = "",
                    ShortDescription = "If this Pokemon has no item, it finds one used by an adjacent Pokemon this turn.",
                    Rating = 0.5f,
                    onResidualOrder = 28,
                    onResidualSubOrder = 2,
                    onResidual = "onResidual(pokemon) {\n			if (pokemon.item) return;\n			const pickupTargets = this.getAllActive().filter(target => (\n				target.lastItem && target.usedItemThisTurn && pokemon.isAdjacent(target)\n			));\n			if (!pickupTargets.length) return;\n			const randomTarget = this.sample(pickupTargets);\n			const item = randomTarget.lastItem;\n			randomTarget.lastItem = '';\n			this.add('-item', pokemon, this.dex.items.get(item), '[from] ability: Pickup');\n			pokemon.setItem(item);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Truant,
                    Name = "Truant",
                    Description = "",
                    ShortDescription = "This Pokemon skips every other turn instead of using a move.",
                    Rating = -1f,
                    onStart = "onStart(pokemon) {\n			pokemon.removeVolatile('truant');\n			if (pokemon.activeTurns && (pokemon.moveThisTurnResult !== undefined || !this.queue.willMove(pokemon))) {\n				pokemon.addVolatile('truant');\n			}\n		}",
                    onBeforeMove = "onBeforeMove(pokemon) {\n			if (pokemon.removeVolatile('truant')) {\n				this.add('cant', pokemon, 'ability: Truant');\n				return false;\n			}\n			pokemon.addVolatile('truant');\n		}",
                    condition = "[object Object]",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Hustle,
                    Name = "Hustle",
                    Description = "This Pokemon's Attack is multiplied by 1.5 and the accuracy of its physical attacks is multiplied by 0.8.",
                    ShortDescription = "This Pokemon's Attack is 1.5x and accuracy of its physical attacks is 0.8x.",
                    Rating = 3.5f,
                    onModifyAtkPriority = 5,
                    onSourceModifyAccuracyPriority = -1,
                    onModifyAtk = "onModifyAtk(atk) {\n			return this.modify(atk, 1.5);\n		}",
                    onSourceModifyAccuracy = "onSourceModifyAccuracy(accuracy, target, source, move) {\n			if (move.category === 'Physical' && typeof accuracy === 'number') {\n				return this.chainModify([3277, 4096]);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.CuteCharm,
                    Name = "Cute Charm",
                    Description = "There is a 30% chance a Pokemon making contact with this Pokemon will become infatuated if it is of the opposite gender.",
                    ShortDescription = "30% chance of infatuating Pokemon of the opposite gender if they make contact.",
                    Rating = 0.5f,
                    onDamagingHit = "onDamagingHit(damage, target, source, move) {\n			if (this.checkMoveMakesContact(move, source, target)) {\n				if (this.randomChance(3, 10)) {\n					source.addVolatile('attract', this.effectState.target);\n				}\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Plus,
                    Name = "Plus",
                    Description = "If an active ally has this Ability or the Minus Ability, this Pokemon's Special Attack is multiplied by 1.5.",
                    ShortDescription = "If an active ally has this Ability or the Minus Ability, this Pokemon's Sp. Atk is 1.5x.",
                    Rating = 0f,
                    onModifySpAPriority = 5,
                    onModifySpA = "onModifySpA(spa, pokemon) {\n			for (const allyActive of pokemon.allies()) {\n				if (allyActive.hasAbility(['minus', 'plus'])) {\n					return this.chainModify(1.5);\n				}\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Minus,
                    Name = "Minus",
                    Description = "If an active ally has this Ability or the Plus Ability, this Pokemon's Special Attack is multiplied by 1.5.",
                    ShortDescription = "If an active ally has this Ability or the Plus Ability, this Pokemon's Sp. Atk is 1.5x.",
                    Rating = 0f,
                    onModifySpAPriority = 5,
                    onModifySpA = "onModifySpA(spa, pokemon) {\n			for (const allyActive of pokemon.allies()) {\n				if (allyActive.hasAbility(['minus', 'plus'])) {\n					return this.chainModify(1.5);\n				}\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Forecast,
                    Name = "Forecast",
                    Description = "If this Pokemon is a Castform, its type changes to the current weather condition's type, except Sandstorm. If this Pokemon is holding Utility Umbrella and the weather condition is Sunny Day, Desolate Land, Rain Dance, or Primordial Sea, it will not change types.",
                    ShortDescription = "Castform's type changes to the current weather condition's type, except Sandstorm.",
                    Rating = 2f,
                    onUpdate = "onUpdate(pokemon) {\n			if (pokemon.baseSpecies.baseSpecies !== 'Castform' || pokemon.transformed) return;\n			let forme = null;\n			switch (pokemon.effectiveWeather()) {\n			case 'sunnyday':\n			case 'desolateland':\n				if (pokemon.species.id !== 'castformsunny') forme = 'Castform-Sunny';\n				break;\n			case 'raindance':\n			case 'primordialsea':\n				if (pokemon.species.id !== 'castformrainy') forme = 'Castform-Rainy';\n				break;\n			case 'hail':\n				if (pokemon.species.id !== 'castformsnowy') forme = 'Castform-Snowy';\n				break;\n			default:\n				if (pokemon.species.id !== 'castform') forme = 'Castform';\n				break;\n			}\n			if (pokemon.isActive && forme) {\n				pokemon.formeChange(forme, this.effect, false, '[msg]');\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.StickyHold,
                    Name = "Sticky Hold",
                    Description = "",
                    ShortDescription = "This Pokemon cannot lose its held item due to another Pokemon's attack.",
                    Rating = 2f,
                    isBreakable = true,
                    onTakeItem = "onTakeItem(item, pokemon, source) {\n			if (!this.activeMove) throw new Error(\"Battle.activeMove is null\");\n			if (!pokemon.hp || pokemon.item === 'stickybarb') return;\n			if ((source && source !== pokemon) || this.activeMove.id === 'knockoff') {\n				this.add('-activate', pokemon, 'ability: Sticky Hold');\n				return false;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.ShedSkin,
                    Name = "Shed Skin",
                    Description = "This Pokemon has a 33% chance to have its non-volatile status condition cured at the end of each turn.",
                    ShortDescription = "This Pokemon has a 33% chance to have its status cured at the end of each turn.",
                    Rating = 3f,
                    onResidualOrder = 5,
                    onResidualSubOrder = 3,
                    onResidual = "onResidual(pokemon) {\n			if (pokemon.hp && pokemon.status && this.randomChance(33, 100)) {\n				this.debug('shed skin');\n				this.add('-activate', pokemon, 'ability: Shed Skin');\n				pokemon.cureStatus();\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Guts,
                    Name = "Guts",
                    Description = "If this Pokemon has a non-volatile status condition, its Attack is multiplied by 1.5; burn's physical damage halving is ignored.",
                    ShortDescription = "If this Pokemon is statused, its Attack is 1.5x; ignores burn halving physical damage.",
                    Rating = 3f,
                    onModifyAtkPriority = 5,
                    onModifyAtk = "onModifyAtk(atk, pokemon) {\n			if (pokemon.status) {\n				return this.chainModify(1.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.MarvelScale,
                    Name = "Marvel Scale",
                    Description = "If this Pokemon has a non-volatile status condition, its Defense is multiplied by 1.5.",
                    ShortDescription = "If this Pokemon is statused, its Defense is 1.5x.",
                    Rating = 2.5f,
                    isBreakable = true,
                    onModifyDefPriority = 6,
                    onModifyDef = "onModifyDef(def, pokemon) {\n			if (pokemon.status) {\n				return this.chainModify(1.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.LiquidOoze,
                    Name = "Liquid Ooze",
                    Description = "",
                    ShortDescription = "This Pokemon damages those draining HP from it for as much as they would heal.",
                    Rating = 1.5f,
                    onSourceTryHeal = "onSourceTryHeal(damage, target, source, effect) {\n			this.debug(\"Heal is occurring: \" + target + \" <- \" + source + \" :: \" + effect.id);\n			const canOoze = ['drain', 'leechseed', 'strengthsap'];\n			if (canOoze.includes(effect.id)) {\n				this.damage(damage);\n				return 0;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Overgrow,
                    Name = "Overgrow",
                    Description = "When this Pokemon has 1/3 or less of its maximum HP, rounded down, its attacking stat is multiplied by 1.5 while using a Grass-type attack.",
                    ShortDescription = "At 1/3 or less of its max HP, this Pokemon's attacking stat is 1.5x with Grass attacks.",
                    Rating = 2f,
                    onModifyAtkPriority = 5,
                    onModifySpAPriority = 5,
                    onModifyAtk = "onModifyAtk(atk, attacker, defender, move) {\n			if (move.type === 'Grass' && attacker.hp <= attacker.maxhp / 3) {\n				this.debug('Overgrow boost');\n				return this.chainModify(1.5);\n			}\n		}",
                    onModifySpA = "onModifySpA(atk, attacker, defender, move) {\n			if (move.type === 'Grass' && attacker.hp <= attacker.maxhp / 3) {\n				this.debug('Overgrow boost');\n				return this.chainModify(1.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Blaze,
                    Name = "Blaze",
                    Description = "When this Pokemon has 1/3 or less of its maximum HP, rounded down, its attacking stat is multiplied by 1.5 while using a Fire-type attack.",
                    ShortDescription = "At 1/3 or less of its max HP, this Pokemon's attacking stat is 1.5x with Fire attacks.",
                    Rating = 2f,
                    onModifyAtkPriority = 5,
                    onModifySpAPriority = 5,
                    onModifyAtk = "onModifyAtk(atk, attacker, defender, move) {\n			if (move.type === 'Fire' && attacker.hp <= attacker.maxhp / 3) {\n				this.debug('Blaze boost');\n				return this.chainModify(1.5);\n			}\n		}",
                    onModifySpA = "onModifySpA(atk, attacker, defender, move) {\n			if (move.type === 'Fire' && attacker.hp <= attacker.maxhp / 3) {\n				this.debug('Blaze boost');\n				return this.chainModify(1.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Torrent,
                    Name = "Torrent",
                    Description = "When this Pokemon has 1/3 or less of its maximum HP, rounded down, its attacking stat is multiplied by 1.5 while using a Water-type attack.",
                    ShortDescription = "At 1/3 or less of its max HP, this Pokemon's attacking stat is 1.5x with Water attacks.",
                    Rating = 2f,
                    onModifyAtkPriority = 5,
                    onModifySpAPriority = 5,
                    onModifyAtk = "onModifyAtk(atk, attacker, defender, move) {\n			if (move.type === 'Water' && attacker.hp <= attacker.maxhp / 3) {\n				this.debug('Torrent boost');\n				return this.chainModify(1.5);\n			}\n		}",
                    onModifySpA = "onModifySpA(atk, attacker, defender, move) {\n			if (move.type === 'Water' && attacker.hp <= attacker.maxhp / 3) {\n				this.debug('Torrent boost');\n				return this.chainModify(1.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Swarm,
                    Name = "Swarm",
                    Description = "When this Pokemon has 1/3 or less of its maximum HP, rounded down, its attacking stat is multiplied by 1.5 while using a Bug-type attack.",
                    ShortDescription = "At 1/3 or less of its max HP, this Pokemon's attacking stat is 1.5x with Bug attacks.",
                    Rating = 2f,
                    onModifyAtkPriority = 5,
                    onModifySpAPriority = 5,
                    onModifyAtk = "onModifyAtk(atk, attacker, defender, move) {\n			if (move.type === 'Bug' && attacker.hp <= attacker.maxhp / 3) {\n				this.debug('Swarm boost');\n				return this.chainModify(1.5);\n			}\n		}",
                    onModifySpA = "onModifySpA(atk, attacker, defender, move) {\n			if (move.type === 'Bug' && attacker.hp <= attacker.maxhp / 3) {\n				this.debug('Swarm boost');\n				return this.chainModify(1.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.RockHead,
                    Name = "Rock Head",
                    Description = "This Pokemon does not take recoil damage besides Struggle, Life Orb, and crash damage.",
                    ShortDescription = "This Pokemon does not take recoil damage besides Struggle/Life Orb/crash damage.",
                    Rating = 3f,
                    onDamage = "onDamage(damage, target, source, effect) {\n			if (effect.id === 'recoil') {\n				if (!this.activeMove) throw new Error(\"Battle.activeMove is null\");\n				if (this.activeMove.id !== 'struggle') return null;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Drought,
                    Name = "Drought",
                    Description = "",
                    ShortDescription = "On switch-in, this Pokemon summons Sunny Day.",
                    Rating = 4f,
                    onStart = "onStart(source) {\n			for (const action of this.queue) {\n				if (action.choice === 'runPrimal' && action.pokemon === source && source.species.id === 'groudon') return;\n				if (action.choice !== 'runSwitch' && action.choice !== 'runPrimal') break;\n			}\n			this.field.setWeather('sunnyday');\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.ArenaTrap,
                    Name = "Arena Trap",
                    Description = "Prevents adjacent opposing Pokemon from choosing to switch out unless they are immune to trapping or are airborne.",
                    ShortDescription = "Prevents adjacent foes from choosing to switch unless they are airborne.",
                    Rating = 5f,
                    onFoeTrapPokemon = "onFoeTrapPokemon(pokemon) {\n			if (!pokemon.isAdjacent(this.effectState.target)) return;\n			if (pokemon.isGrounded()) {\n				pokemon.tryTrap(true);\n			}\n		}",
                    onFoeMaybeTrapPokemon = "onFoeMaybeTrapPokemon(pokemon, source) {\n			if (!source) source = this.effectState.target;\n			if (!source || !pokemon.isAdjacent(source)) return;\n			if (pokemon.isGrounded(!pokemon.knownType)) { // Negate immunity if the type is unknown\n				pokemon.maybeTrapped = true;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.VitalSpirit,
                    Name = "Vital Spirit",
                    Description = "",
                    ShortDescription = "This Pokemon cannot fall asleep. Gaining this Ability while asleep cures it.",
                    Rating = 2f,
                    isBreakable = true,
                    onSetStatus = "onSetStatus(status, target, source, effect) {\n			if (status.id !== 'slp') return;\n			if (_optionalChain([(effect ), 'optionalAccess', _25 => _25.status])) {\n				this.add('-immune', target, '[from] ability: Vital Spirit');\n			}\n			return false;\n		}",
                    onUpdate = "onUpdate(pokemon) {\n			if (pokemon.status === 'slp') {\n				this.add('-activate', pokemon, 'ability: Vital Spirit');\n				pokemon.cureStatus();\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.WhiteSmoke,
                    Name = "White Smoke",
                    Description = "",
                    ShortDescription = "Prevents other Pokemon from lowering this Pokemon's stat stages.",
                    Rating = 2f,
                    isBreakable = true,
                    onBoost = "onBoost(boost, target, source, effect) {\n			if (source && target === source) return;\n			let showMsg = false;\n			let i;\n			for (i in boost) {\n				if (boost[i] < 0) {\n					delete boost[i];\n					showMsg = true;\n				}\n			}\n			if (showMsg && !(effect ).secondaries && effect.id !== 'octolock') {\n				this.add(\"-fail\", target, \"unboost\", \"[from] ability: White Smoke\", \"[of] \" + target);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.PurePower,
                    Name = "Pure Power",
                    Description = "",
                    ShortDescription = "This Pokemon's Attack is doubled.",
                    Rating = 5f,
                    onModifyAtkPriority = 5,
                    onModifyAtk = "onModifyAtk(atk) {\n			return this.chainModify(2);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.ShellArmor,
                    Name = "Shell Armor",
                    Description = "",
                    ShortDescription = "This Pokemon cannot be struck by a critical hit.",
                    Rating = 1f,
                    isBreakable = true,
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.AirLock,
                    Name = "Air Lock",
                    Description = "",
                    ShortDescription = "While this Pokemon is active, the effects of weather conditions are disabled.",
                    Rating = 2f,
                    suppressWeather = true,
                    onSwitchIn = "onSwitchIn(pokemon) {\n			this.effectState.switchingIn = true;\n		}",
                    onStart = "onStart(pokemon) {\n			// Air Lock does not activate when Skill Swapped or when Neutralizing Gas leaves the field\n			if (!this.effectState.switchingIn) return;\n			this.add('-ability', pokemon, 'Air Lock');\n			this.effectState.switchingIn = false;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.TangledFeet,
                    Name = "Tangled Feet",
                    Description = "",
                    ShortDescription = "This Pokemon's evasiveness is doubled as long as it is confused.",
                    Rating = 1f,
                    isBreakable = true,
                    onModifyAccuracyPriority = -1,
                    onModifyAccuracy = "onModifyAccuracy(accuracy, target) {\n			if (typeof accuracy !== 'number') return;\n			if (_optionalChain([target, 'optionalAccess', _21 => _21.volatiles, 'access', _22 => _22['confusion']])) {\n				this.debug('Tangled Feet - decreasing accuracy');\n				return this.chainModify(0.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.MotorDrive,
                    Name = "Motor Drive",
                    Description = "This Pokemon is immune to Electric-type moves and raises its Speed by 1 stage when hit by an Electric-type move.",
                    ShortDescription = "This Pokemon's Speed is raised 1 stage if hit by an Electric move; Electric immunity.",
                    Rating = 3f,
                    isBreakable = true,
                    onTryHit = "onTryHit(target, source, move) {\n			if (target !== source && move.type === 'Electric') {\n				if (!this.boost({spe: 1})) {\n					this.add('-immune', target, '[from] ability: Motor Drive');\n				}\n				return null;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Rivalry,
                    Name = "Rivalry",
                    Description = "This Pokemon's attacks have their power multiplied by 1.25 against targets of the same gender or multiplied by 0.75 against targets of the opposite gender. There is no modifier if either this Pokemon or the target is genderless.",
                    ShortDescription = "This Pokemon's attacks do 1.25x on same gender targets; 0.75x on opposite gender.",
                    Rating = 0f,
                    onBasePower = "onBasePower(basePower, attacker, defender, move) {\n			if (attacker.gender && defender.gender) {\n				if (attacker.gender === defender.gender) {\n					this.debug('Rivalry boost');\n					return this.chainModify(1.25);\n				} else {\n					this.debug('Rivalry weaken');\n					return this.chainModify(0.75);\n				}\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Steadfast,
                    Name = "Steadfast",
                    Description = "",
                    ShortDescription = "If this Pokemon flinches, its Speed is raised by 1 stage.",
                    Rating = 1f,
                    onFlinch = "onFlinch(pokemon) {\n			this.boost({spe: 1});\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.SnowCloak,
                    Name = "Snow Cloak",
                    Description = "If Hail is active, this Pokemon's evasiveness is multiplied by 1.25. This Pokemon takes no damage from Hail.",
                    ShortDescription = "If Hail is active, this Pokemon's evasiveness is 1.25x; immunity to Hail.",
                    Rating = 1.5f,
                    isBreakable = true,
                    onModifyAccuracyPriority = -1,
                    onImmunity = "onImmunity(type, pokemon) {\n			if (type === 'hail') return false;\n		}",
                    onModifyAccuracy = "onModifyAccuracy(accuracy) {\n			if (typeof accuracy !== 'number') return;\n			if (this.field.isWeather('hail')) {\n				this.debug('Snow Cloak - decreasing accuracy');\n				return this.chainModify([3277, 4096]);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Gluttony,
                    Name = "Gluttony",
                    Description = "",
                    ShortDescription = "When this Pokemon has 1/2 or less of its maximum HP, it uses certain Berries early.",
                    Rating = 1.5f,
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.AngerPoint,
                    Name = "Anger Point",
                    Description = "If this Pokemon, but not its substitute, is struck by a critical hit, its Attack is raised by 12 stages.",
                    ShortDescription = "If this Pokemon (not its substitute) takes a critical hit, its Attack is raised 12 stages.",
                    Rating = 1.5f,
                    onHit = "onHit(target, source, move) {\n			if (!target.hp) return;\n			if (_optionalChain([move, 'optionalAccess', _ => _.effectType]) === 'Move' && target.getMoveHitData(move).crit) {\n				target.setBoost({atk: 6});\n				this.add('-setboost', target, 'atk', 12, '[from] ability: Anger Point');\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Unburden,
                    Name = "Unburden",
                    Description = "If this Pokemon loses its held item for any reason, its Speed is doubled. This boost is lost if it switches out or gains a new item or Ability.",
                    ShortDescription = "Speed is doubled on held item loss; boost is lost if it switches, gets new item/Ability.",
                    Rating = 3.5f,
                    onEnd = "onEnd(pokemon) {\n			pokemon.removeVolatile('unburden');\n		}",
                    onTakeItem = "onTakeItem(item, pokemon) {\n			pokemon.addVolatile('unburden');\n		}",
                    onAfterUseItem = "onAfterUseItem(item, pokemon) {\n			if (pokemon !== this.effectState.target) return;\n			pokemon.addVolatile('unburden');\n		}",
                    condition = "[object Object]",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Heatproof,
                    Name = "Heatproof",
                    Description = "The power of Fire-type attacks against this Pokemon is halved, and burn damage taken is halved.",
                    ShortDescription = "The power of Fire-type attacks against this Pokemon is halved; burn damage halved.",
                    Rating = 2f,
                    isBreakable = true,
                    onSourceBasePowerPriority = 18,
                    onDamage = "onDamage(damage, target, source, effect) {\n			if (effect && effect.id === 'brn') {\n				return damage / 2;\n			}\n		}",
                    onSourceBasePower = "onSourceBasePower(basePower, attacker, defender, move) {\n			if (move.type === 'Fire') {\n				return this.chainModify(0.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Simple,
                    Name = "Simple",
                    Description = "When this Pokemon's stat stages are raised or lowered, the effect is doubled instead. This Ability does not affect stat stage increases received from Z-Power effects that happen before a Z-Move is used.",
                    ShortDescription = "When this Pokemon's stat stages are raised or lowered, the effect is doubled instead.",
                    Rating = 4f,
                    isBreakable = true,
                    onBoost = "onBoost(boost, target, source, effect) {\n			if (effect && effect.id === 'zpower') return;\n			let i;\n			for (i in boost) {\n				boost[i] *= 2;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.DrySkin,
                    Name = "Dry Skin",
                    Description = "This Pokemon is immune to Water-type moves and restores 1/4 of its maximum HP, rounded down, when hit by a Water-type move. The power of Fire-type moves is multiplied by 1.25 when used on this Pokemon. At the end of each turn, this Pokemon restores 1/8 of its maximum HP, rounded down, if the weather is Rain Dance, and loses 1/8 of its maximum HP, rounded down, if the weather is Sunny Day. If this Pokemon is holding Utility Umbrella, the effects of weather are nullified.",
                    ShortDescription = "This Pokemon is healed 1/4 by Water, 1/8 by Rain; is hurt 1.25x by Fire, 1/8 by Sun.",
                    Rating = 3f,
                    isBreakable = true,
                    onFoeBasePowerPriority = 17,
                    onTryHit = "onTryHit(target, source, move) {\n			if (target !== source && move.type === 'Water') {\n				if (!this.heal(target.baseMaxhp / 4)) {\n					this.add('-immune', target, '[from] ability: Dry Skin');\n				}\n				return null;\n			}\n		}",
                    onFoeBasePower = "onFoeBasePower(basePower, attacker, defender, move) {\n			if (this.effectState.target !== defender) return;\n			if (move.type === 'Fire') {\n				return this.chainModify(1.25);\n			}\n		}",
                    onWeather = "onWeather(target, source, effect) {\n			if (target.hasItem('utilityumbrella')) return;\n			if (effect.id === 'raindance' || effect.id === 'primordialsea') {\n				this.heal(target.baseMaxhp / 8);\n			} else if (effect.id === 'sunnyday' || effect.id === 'desolateland') {\n				this.damage(target.baseMaxhp / 8, target, target);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Download,
                    Name = "Download",
                    Description = "On switch-in, this Pokemon's Attack or Special Attack is raised by 1 stage based on the weaker combined defensive stat of all opposing Pokemon. Attack is raised if their Defense is lower, and Special Attack is raised if their Special Defense is the same or lower.",
                    ShortDescription = "On switch-in, Attack or Sp. Atk is raised 1 stage based on the foes' weaker Defense.",
                    Rating = 3.5f,
                    onStart = "onStart(pokemon) {\n			let totaldef = 0;\n			let totalspd = 0;\n			for (const target of pokemon.foes()) {\n				totaldef += target.getStat('def', false, true);\n				totalspd += target.getStat('spd', false, true);\n			}\n			if (totaldef && totaldef >= totalspd) {\n				this.boost({spa: 1});\n			} else if (totalspd) {\n				this.boost({atk: 1});\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.IronFist,
                    Name = "Iron Fist",
                    Description = "This Pokemon's punch-based attacks have their power multiplied by 1.2.",
                    ShortDescription = "This Pokemon's punch-based attacks have 1.2x power. Sucker Punch is not boosted.",
                    Rating = 3f,
                    onBasePower = "onBasePower(basePower, attacker, defender, move) {\n			if (move.flags['punch']) {\n				this.debug('Iron Fist boost');\n				return this.chainModify([4915, 4096]);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.PoisonHeal,
                    Name = "Poison Heal",
                    Description = "If this Pokemon is poisoned, it restores 1/8 of its maximum HP, rounded down, at the end of each turn instead of losing HP.",
                    ShortDescription = "This Pokemon is healed by 1/8 of its max HP each turn when poisoned; no HP loss.",
                    Rating = 4f,
                    onDamagePriority = 1,
                    onDamage = "onDamage(damage, target, source, effect) {\n			if (effect.id === 'psn' || effect.id === 'tox') {\n				this.heal(target.baseMaxhp / 8);\n				return false;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Adaptability,
                    Name = "Adaptability",
                    Description = "This Pokemon's moves that match one of its types have a same-type attack bonus (STAB) of 2 instead of 1.5.",
                    ShortDescription = "This Pokemon's same-type attack bonus (STAB) is 2 instead of 1.5.",
                    Rating = 4f,
                    onModifyMove = "onModifyMove(move) {\n			move.stab = 2;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.SkillLink,
                    Name = "Skill Link",
                    Description = "",
                    ShortDescription = "This Pokemon's multi-hit attacks always hit the maximum number of times.",
                    Rating = 3f,
                    onModifyMove = "onModifyMove(move) {\n			if (move.multihit && Array.isArray(move.multihit) && move.multihit.length) {\n				move.multihit = move.multihit[1];\n			}\n			if (move.multiaccuracy) {\n				delete move.multiaccuracy;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Hydration,
                    Name = "Hydration",
                    Description = "This Pokemon has its non-volatile status condition cured at the end of each turn if Rain Dance is active. If this Pokemon is holding Utility Umbrella, its non-volatile status condition will not be cured.",
                    ShortDescription = "This Pokemon has its status cured at the end of each turn if Rain Dance is active.",
                    Rating = 1.5f,
                    onResidualOrder = 5,
                    onResidualSubOrder = 3,
                    onResidual = "onResidual(pokemon) {\n			if (pokemon.status && ['raindance', 'primordialsea'].includes(pokemon.effectiveWeather())) {\n				this.debug('hydration');\n				this.add('-activate', pokemon, 'ability: Hydration');\n				pokemon.cureStatus();\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.SolarPower,
                    Name = "Solar Power",
                    Description = "If Sunny Day is active, this Pokemon's Special Attack is multiplied by 1.5 and it loses 1/8 of its maximum HP, rounded down, at the end of each turn. If this Pokemon is holding Utility Umbrella, its Special Attack remains the same and it does not lose any HP.",
                    ShortDescription = "If Sunny Day is active, this Pokemon's Sp. Atk is 1.5x; loses 1/8 max HP per turn.",
                    Rating = 2f,
                    onModifySpAPriority = 5,
                    onModifySpA = "onModifySpA(spa, pokemon) {\n			if (['sunnyday', 'desolateland'].includes(pokemon.effectiveWeather())) {\n				return this.chainModify(1.5);\n			}\n		}",
                    onWeather = "onWeather(target, source, effect) {\n			if (target.hasItem('utilityumbrella')) return;\n			if (effect.id === 'sunnyday' || effect.id === 'desolateland') {\n				this.damage(target.baseMaxhp / 8, target, target);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.QuickFeet,
                    Name = "Quick Feet",
                    Description = "If this Pokemon has a non-volatile status condition, its Speed is multiplied by 1.5; the Speed drop from paralysis is ignored.",
                    ShortDescription = "If this Pokemon is statused, its Speed is 1.5x; ignores Speed drop from paralysis.",
                    Rating = 2.5f,
                    onModifySpe = "onModifySpe(spe, pokemon) {\n			if (pokemon.status) {\n				return this.chainModify(1.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Normalize,
                    Name = "Normalize",
                    Description = "This Pokemon's moves are changed to be Normal type and have their power multiplied by 1.2. This effect comes before other effects that change a move's type.",
                    ShortDescription = "This Pokemon's moves are changed to be Normal type and have 1.2x power.",
                    Rating = 0f,
                    onModifyTypePriority = 1,
                    onModifyType = "onModifyType(move, pokemon) {\n			const noModifyType = [\n				'hiddenpower', 'judgment', 'multiattack', 'naturalgift', 'revelationdance', 'struggle', 'technoblast', 'terrainpulse', 'weatherball',\n			];\n			if (!(move.isZ && move.category !== 'Status') && !noModifyType.includes(move.id)) {\n				move.type = 'Normal';\n				move.normalizeBoosted = true;\n			}\n		}",
                    onBasePower = "onBasePower(basePower, pokemon, target, move) {\n			if (move.normalizeBoosted) return this.chainModify([4915, 4096]);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Sniper,
                    Name = "Sniper",
                    Description = "",
                    ShortDescription = "If this Pokemon strikes with a critical hit, the damage is multiplied by 1.5.",
                    Rating = 2f,
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.MagicGuard,
                    Name = "Magic Guard",
                    Description = "This Pokemon can only be damaged by direct attacks. Curse and Substitute on use, Belly Drum, Pain Split, Struggle recoil, and confusion damage are considered direct damage.",
                    ShortDescription = "This Pokemon can only be damaged by direct attacks.",
                    Rating = 4f,
                    onDamage = "onDamage(damage, target, source, effect) {\n			if (effect.effectType !== 'Move') {\n				if (effect.effectType === 'Ability') this.add('-activate', source, 'ability: ' + effect.name);\n				return false;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.NoGuard,
                    Name = "No Guard",
                    Description = "",
                    ShortDescription = "Every move used by or against this Pokemon will always hit.",
                    Rating = 4f,
                    onAnyInvulnerabilityPriority = 1,
                    onAnyInvulnerability = "onAnyInvulnerability(target, source, move) {\n			if (move && (source === this.effectState.target || target === this.effectState.target)) return 0;\n		}",
                    onAnyAccuracy = "onAnyAccuracy(accuracy, target, source, move) {\n			if (move && (source === this.effectState.target || target === this.effectState.target)) {\n				return true;\n			}\n			return accuracy;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Stall,
                    Name = "Stall",
                    Description = "",
                    ShortDescription = "This Pokemon moves last among Pokemon using the same or greater priority moves.",
                    Rating = -1f,
                    onFractionalPriority = "-0.1",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Technician,
                    Name = "Technician",
                    Description = "This Pokemon's moves of 60 power or less have their power multiplied by 1.5. Does affect Struggle.",
                    ShortDescription = "This Pokemon's moves of 60 power or less have 1.5x power. Includes Struggle.",
                    Rating = 3.5f,
                    onBasePower = "onBasePower(basePower, attacker, defender, move) {\n			const basePowerAfterMultiplier = this.modify(basePower, this.event.modifier);\n			this.debug('Base Power: ' + basePowerAfterMultiplier);\n			if (basePowerAfterMultiplier <= 60) {\n				this.debug('Technician boost');\n				return this.chainModify(1.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.LeafGuard,
                    Name = "Leaf Guard",
                    Description = "If Sunny Day is active and this Pokemon is not holding Utility Umbrella, this Pokemon cannot gain a non-volatile status condition and Rest will fail for it.",
                    ShortDescription = "If Sunny Day is active, this Pokemon cannot be statused and Rest will fail for it.",
                    Rating = 0.5f,
                    isBreakable = true,
                    onSetStatus = "onSetStatus(status, target, source, effect) {\n			if (['sunnyday', 'desolateland'].includes(target.effectiveWeather())) {\n				if (_optionalChain([(effect ), 'optionalAccess', _7 => _7.status])) {\n					this.add('-immune', target, '[from] ability: Leaf Guard');\n				}\n				return false;\n			}\n		}",
                    onTryAddVolatile = "onTryAddVolatile(status, target) {\n			if (status.id === 'yawn' && ['sunnyday', 'desolateland'].includes(target.effectiveWeather())) {\n				this.add('-immune', target, '[from] ability: Leaf Guard');\n				return null;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Klutz,
                    Name = "Klutz",
                    Description = "This Pokemon's held item has no effect. This Pokemon cannot use Fling successfully. Macho Brace, Power Anklet, Power Band, Power Belt, Power Bracer, Power Lens, and Power Weight still have their effects.",
                    ShortDescription = "This Pokemon's held item has no effect, except Macho Brace. Fling cannot be used.",
                    Rating = -1f,
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.MoldBreaker,
                    Name = "Mold Breaker",
                    Description = "This Pokemon's moves and their effects ignore certain Abilities of other Pokemon. The Abilities that can be negated are Aroma Veil, Aura Break, Battle Armor, Big Pecks, Bulletproof, Clear Body, Contrary, Damp, Dark Aura, Dazzling, Disguise, Dry Skin, Fairy Aura, Filter, Flash Fire, Flower Gift, Flower Veil, Fluffy, Friend Guard, Fur Coat, Grass Pelt, Heatproof, Heavy Metal, Hyper Cutter, Ice Face, Ice Scales, Immunity, Inner Focus, Insomnia, Keen Eye, Leaf Guard, Levitate, Light Metal, Lightning Rod, Limber, Magic Bounce, Magma Armor, Marvel Scale, Mirror Armor, Motor Drive, Multiscale, Oblivious, Overcoat, Own Tempo, Pastel Veil, Punk Rock, Queenly Majesty, Sand Veil, Sap Sipper, Shell Armor, Shield Dust, Simple, Snow Cloak, Solid Rock, Soundproof, Sticky Hold, Storm Drain, Sturdy, Suction Cups, Sweet Veil, Tangled Feet, Telepathy, Thick Fat, Unaware, Vital Spirit, Volt Absorb, Water Absorb, Water Bubble, Water Veil, White Smoke, Wonder Guard, and Wonder Skin. This affects every other Pokemon on the field, whether or not it is a target of this Pokemon's move, and whether or not their Ability is beneficial to this Pokemon.",
                    ShortDescription = "This Pokemon's moves and their effects ignore the Abilities of other Pokemon.",
                    Rating = 3.5f,
                    onModifyMove = "onModifyMove(move) {\n			move.ignoreAbility = true;\n		}",
                    onStart = "onStart(pokemon) {\n			this.add('-ability', pokemon, 'Mold Breaker');\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.SuperLuck,
                    Name = "Super Luck",
                    Description = "",
                    ShortDescription = "This Pokemon's critical hit ratio is raised by 1 stage.",
                    Rating = 1.5f,
                    onModifyCritRatio = "onModifyCritRatio(critRatio) {\n			return critRatio + 1;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Aftermath,
                    Name = "Aftermath",
                    Description = "If this Pokemon is knocked out with a contact move, that move's user loses 1/4 of its maximum HP, rounded down. If any active Pokemon has the Damp Ability, this effect is prevented.",
                    ShortDescription = "If this Pokemon is KOed with a contact move, that move's user loses 1/4 its max HP.",
                    Rating = 2.5f,
                    onDamagingHitOrder = 1,
                    onDamagingHit = "onDamagingHit(damage, target, source, move) {\n			if (!target.hp && this.checkMoveMakesContact(move, source, target, true)) {\n				this.damage(source.baseMaxhp / 4, source, target);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Anticipation,
                    Name = "Anticipation",
                    Description = "On switch-in, this Pokemon is alerted if any opposing Pokemon has an attack that is super effective on this Pokemon, or an OHKO move. Counter, Metal Burst, and Mirror Coat count as attacking moves of their respective types, Hidden Power counts as its determined type, and Judgment, Multi-Attack, Natural Gift, Revelation Dance, Techno Blast, and Weather Ball are considered Normal-type moves.",
                    ShortDescription = "On switch-in, this Pokemon shudders if any foe has a supereffective or OHKO move.",
                    Rating = 0.5f,
                    onStart = "onStart(pokemon) {\n			for (const target of pokemon.foes()) {\n				for (const moveSlot of target.moveSlots) {\n					const move = this.dex.moves.get(moveSlot.move);\n					if (move.category === 'Status') continue;\n					const moveType = move.id === 'hiddenpower' ? target.hpType : move.type;\n					if (\n						this.dex.getImmunity(moveType, pokemon) && this.dex.getEffectiveness(moveType, pokemon) > 0 ||\n						move.ohko\n					) {\n						this.add('-ability', pokemon, 'Anticipation');\n						return;\n					}\n				}\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Forewarn,
                    Name = "Forewarn",
                    Description = "On switch-in, this Pokemon is alerted to the move with the highest power, at random, known by an opposing Pokemon.",
                    ShortDescription = "On switch-in, this Pokemon is alerted to the foes' move with the highest power.",
                    Rating = 0.5f,
                    onStart = "onStart(pokemon) {\n			let warnMoves = [];\n			let warnBp = 1;\n			for (const target of pokemon.foes()) {\n				for (const moveSlot of target.moveSlots) {\n					const move = this.dex.moves.get(moveSlot.move);\n					let bp = move.basePower;\n					if (move.ohko) bp = 150;\n					if (move.id === 'counter' || move.id === 'metalburst' || move.id === 'mirrorcoat') bp = 120;\n					if (bp === 1) bp = 80;\n					if (!bp && move.category !== 'Status') bp = 80;\n					if (bp > warnBp) {\n						warnMoves = [[move, target]];\n						warnBp = bp;\n					} else if (bp === warnBp) {\n						warnMoves.push([move, target]);\n					}\n				}\n			}\n			if (!warnMoves.length) return;\n			const [warnMoveName, warnTarget] = this.sample(warnMoves);\n			this.add('-activate', pokemon, 'ability: Forewarn', warnMoveName, '[of] ' + warnTarget);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Unaware,
                    Name = "Unaware",
                    Description = "This Pokemon ignores other Pokemon's Attack, Special Attack, and accuracy stat stages when taking damage, and ignores other Pokemon's Defense, Special Defense, and evasiveness stat stages when dealing damage.",
                    ShortDescription = "This Pokemon ignores other Pokemon's stat stages when taking or doing damage.",
                    Rating = 4f,
                    isBreakable = true,
                    onAnyModifyBoost = "onAnyModifyBoost(boosts, pokemon) {\n			const unawareUser = this.effectState.target;\n			if (unawareUser === pokemon) return;\n			if (unawareUser === this.activePokemon && pokemon === this.activeTarget) {\n				boosts['def'] = 0;\n				boosts['spd'] = 0;\n				boosts['evasion'] = 0;\n			}\n			if (pokemon === this.activePokemon && unawareUser === this.activeTarget) {\n				boosts['atk'] = 0;\n				boosts['def'] = 0;\n				boosts['spa'] = 0;\n				boosts['accuracy'] = 0;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.TintedLens,
                    Name = "Tinted Lens",
                    Description = "",
                    ShortDescription = "This Pokemon's attacks that are not very effective on a target deal double damage.",
                    Rating = 4f,
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Filter,
                    Name = "Filter",
                    Description = "",
                    ShortDescription = "This Pokemon receives 3/4 damage from supereffective attacks.",
                    Rating = 3f,
                    isBreakable = true,
                    onSourceModifyDamage = "onSourceModifyDamage(damage, source, target, move) {\n			if (target.getMoveHitData(move).typeMod > 0) {\n				this.debug('Filter neutralize');\n				return this.chainModify(0.75);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.SlowStart,
                    Name = "Slow Start",
                    Description = "",
                    ShortDescription = "On switch-in, this Pokemon's Attack and Speed are halved for 5 turns.",
                    Rating = -1f,
                    onStart = "onStart(pokemon) {\n			pokemon.addVolatile('slowstart');\n		}",
                    onEnd = "onEnd(pokemon) {\n			delete pokemon.volatiles['slowstart'];\n			this.add('-end', pokemon, 'Slow Start', '[silent]');\n		}",
                    condition = "[object Object]",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Scrappy,
                    Name = "Scrappy",
                    Description = "This Pokemon can hit Ghost types with Normal- and Fighting-type moves. Immune to Intimidate.",
                    ShortDescription = "Fighting, Normal moves hit Ghost. Immune to Intimidate.",
                    Rating = 3f,
                    onModifyMovePriority = -5,
                    onModifyMove = "onModifyMove(move) {\n			if (!move.ignoreImmunity) move.ignoreImmunity = {};\n			if (move.ignoreImmunity !== true) {\n				move.ignoreImmunity['Fighting'] = true;\n				move.ignoreImmunity['Normal'] = true;\n			}\n		}",
                    onBoost = "onBoost(boost, target, source, effect) {\n			if (effect.id === 'intimidate') {\n				delete boost.atk;\n				this.add('-fail', target, 'unboost', 'Attack', '[from] ability: Scrappy', '[of] ' + target);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.StormDrain,
                    Name = "Storm Drain",
                    Description = "This Pokemon is immune to Water-type moves and raises its Special Attack by 1 stage when hit by a Water-type move. If this Pokemon is not the target of a single-target Water-type move used by another Pokemon, this Pokemon redirects that move to itself if it is within the range of that move.",
                    ShortDescription = "This Pokemon draws Water moves to itself to raise Sp. Atk by 1; Water immunity.",
                    Rating = 3f,
                    isBreakable = true,
                    onTryHit = "onTryHit(target, source, move) {\n			if (target !== source && move.type === 'Water') {\n				if (!this.boost({spa: 1})) {\n					this.add('-immune', target, '[from] ability: Storm Drain');\n				}\n				return null;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.IceBody,
                    Name = "Ice Body",
                    Description = "If Hail is active, this Pokemon restores 1/16 of its maximum HP, rounded down, at the end of each turn. This Pokemon takes no damage from Hail.",
                    ShortDescription = "If Hail is active, this Pokemon heals 1/16 of its max HP each turn; immunity to Hail.",
                    Rating = 1f,
                    onWeather = "onWeather(target, source, effect) {\n			if (effect.id === 'hail') {\n				this.heal(target.baseMaxhp / 16);\n			}\n		}",
                    onImmunity = "onImmunity(type, pokemon) {\n			if (type === 'hail') return false;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.SolidRock,
                    Name = "Solid Rock",
                    Description = "",
                    ShortDescription = "This Pokemon receives 3/4 damage from supereffective attacks.",
                    Rating = 3f,
                    isBreakable = true,
                    onSourceModifyDamage = "onSourceModifyDamage(damage, source, target, move) {\n			if (target.getMoveHitData(move).typeMod > 0) {\n				this.debug('Solid Rock neutralize');\n				return this.chainModify(0.75);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.SnowWarning,
                    Name = "Snow Warning",
                    Description = "",
                    ShortDescription = "On switch-in, this Pokemon summons Hail.",
                    Rating = 4f,
                    onStart = "onStart(source) {\n			this.field.setWeather('hail');\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.HoneyGather,
                    Name = "Honey Gather",
                    Description = "",
                    ShortDescription = "No competitive use.",
                    Rating = 0f,
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Frisk,
                    Name = "Frisk",
                    Description = "",
                    ShortDescription = "On switch-in, this Pokemon identifies the held items of all opposing Pokemon.",
                    Rating = 1.5f,
                    onStart = "onStart(pokemon) {\n			for (const target of pokemon.foes()) {\n				if (target.item) {\n					this.add('-item', target, target.getItem().name, '[from] ability: Frisk', '[of] ' + pokemon, '[identify]');\n				}\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Reckless,
                    Name = "Reckless",
                    Description = "This Pokemon's attacks with recoil or crash damage have their power multiplied by 1.2. Does not affect Struggle.",
                    ShortDescription = "This Pokemon's attacks with recoil or crash damage have 1.2x power; not Struggle.",
                    Rating = 3f,
                    onBasePower = "onBasePower(basePower, attacker, defender, move) {\n			if (move.recoil || move.hasCrashDamage) {\n				this.debug('Reckless boost');\n				return this.chainModify([4915, 4096]);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Multitype,
                    Name = "Multitype",
                    Description = "",
                    ShortDescription = "If this Pokemon is an Arceus, its type changes to match its held Plate or Z-Crystal.",
                    Rating = 4f,
                    isPermanent = true,
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.FlowerGift,
                    Name = "Flower Gift",
                    Description = "If this Pokemon is a Cherrim and Sunny Day is active, it changes to Sunshine Form and the Attack and Special Defense of it and its allies are multiplied by 1.5. If this Pokemon is a Cherrim and it is holding Utility Umbrella, it remains in its regular form and the Attack and Special Defense stats of it and its allies are not boosted. If this Pokemon is a Cherrim in its Sunshine form and is given Utility Umbrella, it will immediately switch back to its regular form. If this Pokemon is a Cherrim holding Utility Umbrella and its item is removed while Sunny Day is active, it will transform into its Sunshine Form. If an ally is holding Utility Umbrella while Cherrim is in its Sunshine Form, they will not receive the Attack and Special Defense boosts.",
                    ShortDescription = "If user is Cherrim and Sunny Day is active, it and allies' Attack and Sp. Def are 1.5x.",
                    Rating = 1f,
                    isBreakable = true,
                    onAllyModifyAtkPriority = 3,
                    onAllyModifySpDPriority = 4,
                    onStart = "onStart(pokemon) {\n			delete this.effectState.forme;\n		}",
                    onUpdate = "onUpdate(pokemon) {\n			if (!pokemon.isActive || pokemon.baseSpecies.baseSpecies !== 'Cherrim' || pokemon.transformed) return;\n			if (!pokemon.hp) return;\n			if (['sunnyday', 'desolateland'].includes(pokemon.effectiveWeather())) {\n				if (pokemon.species.id !== 'cherrimsunshine') {\n					pokemon.formeChange('Cherrim-Sunshine', this.effect, false, '[msg]');\n				}\n			} else {\n				if (pokemon.species.id === 'cherrimsunshine') {\n					pokemon.formeChange('Cherrim', this.effect, false, '[msg]');\n				}\n			}\n		}",
                    onAllyModifyAtk = "onAllyModifyAtk(atk, pokemon) {\n			if (this.effectState.target.baseSpecies.baseSpecies !== 'Cherrim') return;\n			if (['sunnyday', 'desolateland'].includes(pokemon.effectiveWeather())) {\n				return this.chainModify(1.5);\n			}\n		}",
                    onAllyModifySpD = "onAllyModifySpD(spd, pokemon) {\n			if (this.effectState.target.baseSpecies.baseSpecies !== 'Cherrim') return;\n			if (['sunnyday', 'desolateland'].includes(pokemon.effectiveWeather())) {\n				return this.chainModify(1.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.BadDreams,
                    Name = "Bad Dreams",
                    Description = "Causes adjacent opposing Pokemon to lose 1/8 of their maximum HP, rounded down, at the end of each turn if they are asleep.",
                    ShortDescription = "Causes sleeping adjacent foes to lose 1/8 of their max HP at the end of each turn.",
                    Rating = 1.5f,
                    onResidualOrder = 28,
                    onResidualSubOrder = 2,
                    onResidual = "onResidual(pokemon) {\n			if (!pokemon.hp) return;\n			for (const target of pokemon.foes()) {\n				if (target.status === 'slp' || target.hasAbility('comatose')) {\n					this.damage(target.baseMaxhp / 8, target, pokemon);\n				}\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Pickpocket,
                    Name = "Pickpocket",
                    Description = "If this Pokemon has no item and is hit by a contact move, it steals the attacker's item. This effect applies after all hits from a multi-hit move; Sheer Force prevents it from activating if the move has a secondary effect.",
                    ShortDescription = "If this Pokemon has no item and is hit by a contact move, it steals the attacker's item.",
                    Rating = 1f,
                    onAfterMoveSecondary = "onAfterMoveSecondary(target, source, move) {\n			if (source && source !== target && _optionalChain([move, 'optionalAccess', _13 => _13.flags, 'access', _14 => _14['contact']])) {\n				if (target.item || target.switchFlag || target.forceSwitchFlag || source.switchFlag === true) {\n					return;\n				}\n				const yourItem = source.takeItem(target);\n				if (!yourItem) {\n					return;\n				}\n				if (!target.setItem(yourItem)) {\n					source.item = yourItem.id;\n					return;\n				}\n				this.add('-enditem', source, yourItem, '[silent]', '[from] ability: Pickpocket', '[of] ' + source);\n				this.add('-item', target, yourItem, '[from] ability: Pickpocket', '[of] ' + source);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.SheerForce,
                    Name = "Sheer Force",
                    Description = "This Pokemon's attacks with secondary effects have their power multiplied by 1.3, but the secondary effects are removed. If a secondary effect was removed, it also removes the user's Life Orb recoil and Shell Bell recovery, and prevents the target's Berserk, Color Change, Emergency Exit, Pickpocket, Wimp Out, Red Card, Eject Button, Kee Berry, and Maranga Berry from activating.",
                    ShortDescription = "This Pokemon's attacks with secondary effects have 1.3x power; nullifies the effects.",
                    Rating = 3.5f,
                    onModifyMove = "onModifyMove(move, pokemon) {\n			if (move.secondaries) {\n				delete move.secondaries;\n				// Technically not a secondary effect, but it is negated\n				delete move.self;\n				if (move.id === 'clangoroussoulblaze') delete move.selfBoost;\n				// Actual negation of `AfterMoveSecondary` effects implemented in scripts.js\n				move.hasSheerForce = true;\n			}\n		}",
                    onBasePower = "onBasePower(basePower, pokemon, target, move) {\n			if (move.hasSheerForce) return this.chainModify([5325, 4096]);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Contrary,
                    Name = "Contrary",
                    Description = "If this Pokemon has a stat stage raised it is lowered instead, and vice versa. This Ability does not affect stat stage increases received from Z-Power effects that happen before a Z-Move is used.",
                    ShortDescription = "If this Pokemon has a stat stage raised it is lowered instead, and vice versa.",
                    Rating = 4.5f,
                    isBreakable = true,
                    onBoost = "onBoost(boost, target, source, effect) {\n			if (effect && effect.id === 'zpower') return;\n			let i;\n			for (i in boost) {\n				boost[i] *= -1;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Unnerve,
                    Name = "Unnerve",
                    Description = "While this Pokemon is active, it prevents opposing Pokemon from using their Berries. Activation message broadcasts before other Abilities regardless of the Pokemon's Speed tiers.",
                    ShortDescription = "While this Pokemon is active, it prevents opposing Pokemon from using their Berries.",
                    Rating = 1.5f,
                    onStart = "onStart(pokemon) {\n			if (this.effectState.unnerved) return;\n			this.add('-ability', pokemon, 'Unnerve');\n			this.effectState.unnerved = true;\n		}",
                    onPreStart = "onPreStart(pokemon) {\n			this.add('-ability', pokemon, 'Unnerve');\n			this.effectState.unnerved = true;\n		}",
                    onEnd = "onEnd() {\n			this.effectState.unnerved = false;\n		}",
                    onFoeTryEatItem = "onFoeTryEatItem() {\n			return !this.effectState.unnerved;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Defiant,
                    Name = "Defiant",
                    Description = "This Pokemon's Attack is raised by 2 stages for each of its stat stages that is lowered by an opposing Pokemon.",
                    ShortDescription = "This Pokemon's Attack is raised by 2 for each of its stats that is lowered by a foe.",
                    Rating = 2.5f,
                    onAfterEachBoost = "onAfterEachBoost(boost, target, source, effect) {\n			if (!source || target.isAlly(source)) {\n				if (effect.id === 'stickyweb') {\n					this.hint(\"Court Change Sticky Web counts as lowering your own Speed, and Defiant only affects stats lowered by foes.\", true, source.side);\n				}\n				return;\n			}\n			let statsLowered = false;\n			let i;\n			for (i in boost) {\n				if (boost[i] < 0) {\n					statsLowered = true;\n				}\n			}\n			if (statsLowered) {\n				this.add('-ability', target, 'Defiant');\n				this.boost({atk: 2}, target, target, null, true);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Defeatist,
                    Name = "Defeatist",
                    Description = "While this Pokemon has 1/2 or less of its maximum HP, its Attack and Special Attack are halved.",
                    ShortDescription = "While this Pokemon has 1/2 or less of its max HP, its Attack and Sp. Atk are halved.",
                    Rating = -1f,
                    onModifyAtkPriority = 5,
                    onModifySpAPriority = 5,
                    onModifyAtk = "onModifyAtk(atk, pokemon) {\n			if (pokemon.hp <= pokemon.maxhp / 2) {\n				return this.chainModify(0.5);\n			}\n		}",
                    onModifySpA = "onModifySpA(atk, pokemon) {\n			if (pokemon.hp <= pokemon.maxhp / 2) {\n				return this.chainModify(0.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.CursedBody,
                    Name = "Cursed Body",
                    Description = "If this Pokemon is hit by an attack, there is a 30% chance that move gets disabled unless one of the attacker's moves is already disabled.",
                    ShortDescription = "If this Pokemon is hit by an attack, there is a 30% chance that move gets disabled.",
                    Rating = 2f,
                    onDamagingHit = "onDamagingHit(damage, target, source, move) {\n			if (source.volatiles['disable']) return;\n			if (!move.isMax && !move.isFutureMove && move.id !== 'struggle') {\n				if (this.randomChance(3, 10)) {\n					source.addVolatile('disable', this.effectState.target);\n				}\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Healer,
                    Name = "Healer",
                    Description = "There is a 30% chance of curing an adjacent ally's non-volatile status condition at the end of each turn.",
                    ShortDescription = "30% chance of curing an adjacent ally's status at the end of each turn.",
                    Rating = 0f,
                    onResidualOrder = 5,
                    onResidualSubOrder = 3,
                    onResidual = "onResidual(pokemon) {\n			for (const allyActive of pokemon.adjacentAllies()) {\n				if (allyActive.status && this.randomChance(3, 10)) {\n					this.add('-activate', pokemon, 'ability: Healer');\n					allyActive.cureStatus();\n				}\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.FriendGuard,
                    Name = "Friend Guard",
                    Description = "",
                    ShortDescription = "This Pokemon's allies receive 3/4 damage from other Pokemon's attacks.",
                    Rating = 0f,
                    isBreakable = true,
                    onAnyModifyDamage = "onAnyModifyDamage(damage, source, target, move) {\n			if (target !== this.effectState.target && target.isAlly(this.effectState.target)) {\n				this.debug('Friend Guard weaken');\n				return this.chainModify(0.75);\n			}\n		}",
                    onOther = "onAnyModifyDamage(damage, source, target, move) {\n			if (target !== this.effectState.target && target.isAlly(this.effectState.target)) {\n				this.debug('Friend Guard weaken');\n				return this.chainModify(0.75);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.WeakArmor,
                    Name = "Weak Armor",
                    Description = "If a physical attack hits this Pokemon, its Defense is lowered by 1 stage and its Speed is raised by 2 stages.",
                    ShortDescription = "If a physical attack hits this Pokemon, Defense is lowered by 1, Speed is raised by 2.",
                    Rating = 1f,
                    onDamagingHit = "onDamagingHit(damage, target, source, move) {\n			if (move.category === 'Physical') {\n				this.boost({def: -1, spe: 2}, target, target);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.HeavyMetal,
                    Name = "Heavy Metal",
                    Description = "",
                    ShortDescription = "This Pokemon's weight is doubled.",
                    Rating = 0f,
                    isBreakable = true,
                    onModifyWeight = "onModifyWeight(weighthg) {\n			return weighthg * 2;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.LightMetal,
                    Name = "Light Metal",
                    Description = "",
                    ShortDescription = "This Pokemon's weight is halved.",
                    Rating = 1f,
                    isBreakable = true,
                    onModifyWeight = "onModifyWeight(weighthg) {\n			return this.trunc(weighthg / 2);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Multiscale,
                    Name = "Multiscale",
                    Description = "",
                    ShortDescription = "If this Pokemon is at full HP, damage taken from attacks is halved.",
                    Rating = 3.5f,
                    isBreakable = true,
                    onSourceModifyDamage = "onSourceModifyDamage(damage, source, target, move) {\n			if (target.hp >= target.maxhp) {\n				this.debug('Multiscale weaken');\n				return this.chainModify(0.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.ToxicBoost,
                    Name = "Toxic Boost",
                    Description = "While this Pokemon is poisoned, the power of its physical attacks is multiplied by 1.5.",
                    ShortDescription = "While this Pokemon is poisoned, its physical attacks have 1.5x power.",
                    Rating = 2.5f,
                    onBasePower = "onBasePower(basePower, attacker, defender, move) {\n			if ((attacker.status === 'psn' || attacker.status === 'tox') && move.category === 'Physical') {\n				return this.chainModify(1.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.FlareBoost,
                    Name = "Flare Boost",
                    Description = "While this Pokemon is burned, the power of its special attacks is multiplied by 1.5.",
                    ShortDescription = "While this Pokemon is burned, its special attacks have 1.5x power.",
                    Rating = 2f,
                    onBasePower = "onBasePower(basePower, attacker, defender, move) {\n			if (attacker.status === 'brn' && move.category === 'Special') {\n				return this.chainModify(1.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Harvest,
                    Name = "Harvest",
                    Description = "If the last item this Pokemon used is a Berry, there is a 50% chance it gets restored at the end of each turn. If Sunny Day is active, this chance is 100%.",
                    ShortDescription = "If last item used is a Berry, 50% chance to restore it each end of turn. 100% in Sun.",
                    Rating = 2.5f,
                    onResidualOrder = 28,
                    onResidualSubOrder = 2,
                    onResidual = "onResidual(pokemon) {\n			if (this.field.isWeather(['sunnyday', 'desolateland']) || this.randomChance(1, 2)) {\n				if (pokemon.hp && !pokemon.item && this.dex.items.get(pokemon.lastItem).isBerry) {\n					pokemon.setItem(pokemon.lastItem);\n					pokemon.lastItem = '';\n					this.add('-item', pokemon, pokemon.getItem(), '[from] ability: Harvest');\n				}\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Telepathy,
                    Name = "Telepathy",
                    Description = "",
                    ShortDescription = "This Pokemon does not take damage from attacks made by its allies.",
                    Rating = 0f,
                    isBreakable = true,
                    onTryHit = "onTryHit(target, source, move) {\n			if (target !== source && target.isAlly(source) && move.category !== 'Status') {\n				this.add('-activate', target, 'ability: Telepathy');\n				return null;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Moody,
                    Name = "Moody",
                    Description = "This Pokemon has a random stat other than accuracy or evasion raised by 2 stages and another stat lowered by 1 stage at the end of each turn.",
                    ShortDescription = "Boosts a random stat (except accuracy/evasion) +2 and another stat -1 every turn.",
                    Rating = 5f,
                    onResidualOrder = 28,
                    onResidualSubOrder = 2,
                    onResidual = "onResidual(pokemon) {\n			let stats = [];\n			const boost = {};\n			let statPlus;\n			for (statPlus in pokemon.boosts) {\n				if (statPlus === 'accuracy' || statPlus === 'evasion') continue;\n				if (pokemon.boosts[statPlus] < 6) {\n					stats.push(statPlus);\n				}\n			}\n			let randomStat = stats.length ? this.sample(stats) : undefined;\n			if (randomStat) boost[randomStat] = 2;\n\n			stats = [];\n			let statMinus;\n			for (statMinus in pokemon.boosts) {\n				if (statMinus === 'accuracy' || statMinus === 'evasion') continue;\n				if (pokemon.boosts[statMinus] > -6 && statMinus !== randomStat) {\n					stats.push(statMinus);\n				}\n			}\n			randomStat = stats.length ? this.sample(stats) : undefined;\n			if (randomStat) boost[randomStat] = -1;\n\n			this.boost(boost);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Overcoat,
                    Name = "Overcoat",
                    Description = "",
                    ShortDescription = "This Pokemon is immune to powder moves and damage from Sandstorm or Hail.",
                    Rating = 2f,
                    isBreakable = true,
                    onTryHitPriority = 1,
                    onTryHit = "onTryHit(target, source, move) {\n			if (move.flags['powder'] && target !== source && this.dex.getImmunity('powder', target)) {\n				this.add('-immune', target, '[from] ability: Overcoat');\n				return null;\n			}\n		}",
                    onImmunity = "onImmunity(type, pokemon) {\n			if (type === 'sandstorm' || type === 'hail' || type === 'powder') return false;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.PoisonTouch,
                    Name = "Poison Touch",
                    Description = "",
                    ShortDescription = "This Pokemon's contact moves have a 30% chance of poisoning.",
                    Rating = 2f,
                    onModifyMove = "onModifyMove(move) {\n			if (!_optionalChain([move, 'optionalAccess', _15 => _15.flags, 'access', _16 => _16['contact']]) || move.target === 'self') return;\n			if (!move.secondaries) {\n				move.secondaries = [];\n			}\n			move.secondaries.push({\n				chance: 30,\n				status: 'psn',\n				ability: this.dex.abilities.get('poisontouch'),\n			});\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Regenerator,
                    Name = "Regenerator",
                    Description = "",
                    ShortDescription = "This Pokemon restores 1/3 of its maximum HP, rounded down, when it switches out.",
                    Rating = 4.5f,
                    onSwitchOut = "onSwitchOut(pokemon) {\n			pokemon.heal(pokemon.baseMaxhp / 3);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.BigPecks,
                    Name = "Big Pecks",
                    Description = "",
                    ShortDescription = "Prevents other Pokemon from lowering this Pokemon's Defense stat stage.",
                    Rating = 0.5f,
                    isBreakable = true,
                    onBoost = "onBoost(boost, target, source, effect) {\n			if (source && target === source) return;\n			if (boost.def && boost.def < 0) {\n				delete boost.def;\n				if (!(effect ).secondaries && effect.id !== 'octolock') {\n					this.add(\"-fail\", target, \"unboost\", \"Defense\", \"[from] ability: Big Pecks\", \"[of] \" + target);\n				}\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.SandRush,
                    Name = "Sand Rush",
                    Description = "If Sandstorm is active, this Pokemon's Speed is doubled. This Pokemon takes no damage from Sandstorm.",
                    ShortDescription = "If Sandstorm is active, this Pokemon's Speed is doubled; immunity to Sandstorm.",
                    Rating = 3f,
                    onModifySpe = "onModifySpe(spe, pokemon) {\n			if (this.field.isWeather('sandstorm')) {\n				return this.chainModify(2);\n			}\n		}",
                    onImmunity = "onImmunity(type, pokemon) {\n			if (type === 'sandstorm') return false;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.WonderSkin,
                    Name = "Wonder Skin",
                    Description = "All non-damaging moves that check accuracy have their accuracy changed to 50% when used on this Pokemon. This change is done before any other accuracy modifying effects.",
                    ShortDescription = "Status moves with accuracy checks are 50% accurate when used on this Pokemon.",
                    Rating = 2f,
                    isBreakable = true,
                    onModifyAccuracyPriority = 10,
                    onModifyAccuracy = "onModifyAccuracy(accuracy, target, source, move) {\n			if (move.category === 'Status' && typeof accuracy === 'number') {\n				this.debug('Wonder Skin - setting accuracy to 50');\n				return 50;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Analytic,
                    Name = "Analytic",
                    Description = "The power of this Pokemon's move is multiplied by 1.3 if it is the last to move in a turn. Does not affect Doom Desire and Future Sight.",
                    ShortDescription = "This Pokemon's attacks have 1.3x power if it is the last to move in a turn.",
                    Rating = 2.5f,
                    onBasePower = "onBasePower(basePower, pokemon) {\n			let boosted = true;\n			for (const target of this.getAllActive()) {\n				if (target === pokemon) continue;\n				if (this.queue.willMove(target)) {\n					boosted = false;\n					break;\n				}\n			}\n			if (boosted) {\n				this.debug('Analytic boost');\n				return this.chainModify([5325, 4096]);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Illusion,
                    Name = "Illusion",
                    Description = "When this Pokemon switches in, it appears as the last unfainted Pokemon in its party until it takes direct damage from another Pokemon's attack. This Pokemon's actual level and HP are displayed instead of those of the mimicked Pokemon.",
                    ShortDescription = "This Pokemon appears as the last Pokemon in the party until it takes direct damage.",
                    Rating = 4.5f,
                    onDamagingHit = "onDamagingHit(damage, target, source, move) {\n			if (target.illusion) {\n				this.singleEvent('End', this.dex.abilities.get('Illusion'), target.abilityState, target, source, move);\n			}\n		}",
                    onEnd = "onEnd(pokemon) {\n			if (pokemon.illusion) {\n				this.debug('illusion cleared');\n				pokemon.illusion = null;\n				const details = pokemon.species.name + (pokemon.level === 100 ? '' : ', L' + pokemon.level) +\n					(pokemon.gender === '' ? '' : ', ' + pokemon.gender) + (pokemon.set.shiny ? ', shiny' : '');\n				this.add('replace', pokemon, details);\n				this.add('-end', pokemon, 'Illusion');\n			}\n		}",
                    onBeforeSwitchIn = "onBeforeSwitchIn(pokemon) {\n			pokemon.illusion = null;\n			// yes, you can Illusion an active pokemon but only if it's to your right\n			for (let i = pokemon.side.pokemon.length - 1; i > pokemon.position; i--) {\n				const possibleTarget = pokemon.side.pokemon[i];\n				if (!possibleTarget.fainted) {\n					pokemon.illusion = possibleTarget;\n					break;\n				}\n			}\n		}",
                    onFaint = "onFaint(pokemon) {\n			pokemon.illusion = null;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Imposter,
                    Name = "Imposter",
                    Description = "On switch-in, this Pokemon Transforms into the opposing Pokemon that is facing it. If there is no Pokemon at that position, this Pokemon does not Transform.",
                    ShortDescription = "On switch-in, this Pokemon Transforms into the opposing Pokemon that is facing it.",
                    Rating = 5f,
                    onSwitchIn = "onSwitchIn(pokemon) {\n			this.effectState.switchingIn = true;\n		}",
                    onStart = "onStart(pokemon) {\n			// Imposter does not activate when Skill Swapped or when Neutralizing Gas leaves the field\n			if (!this.effectState.switchingIn) return;\n			// copies across in multibattle and diagonally in free-for-all\n			// fortunately, side.foe already takes care of all that\n			const target = pokemon.side.foe.active[pokemon.side.foe.active.length - 1 - pokemon.position];\n			if (target) {\n				pokemon.transformInto(target, this.dex.abilities.get('imposter'));\n			}\n			this.effectState.switchingIn = false;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Infiltrator,
                    Name = "Infiltrator",
                    Description = "This Pokemon's moves ignore substitutes and the opposing side's Reflect, Light Screen, Safeguard, Mist and Aurora Veil.",
                    ShortDescription = "Moves ignore substitutes and foe's Reflect/Light Screen/Safeguard/Mist/Aurora Veil.",
                    Rating = 2.5f,
                    onModifyMove = "onModifyMove(move) {\n			move.infiltrates = true;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Mummy,
                    Name = "Mummy",
                    Description = "Pokemon making contact with this Pokemon have their Ability changed to Mummy. Does not affect a Pokemon which already has Mummy or the Abilities As One, Battle Bond, Comatose, Disguise, Gulp Missile, Ice Face, Multitype, Power Construct, RKS System, Schooling, Shields Down, Stance Change, and Zen Mode.",
                    ShortDescription = "Pokemon making contact with this Pokemon have their Ability changed to Mummy.",
                    Rating = 2f,
                    onDamagingHit = "onDamagingHit(damage, target, source, move) {\n			const sourceAbility = source.getAbility();\n			if (sourceAbility.isPermanent || sourceAbility.id === 'mummy') {\n				return;\n			}\n			if (this.checkMoveMakesContact(move, source, target, !source.isAlly(target))) {\n				const oldAbility = source.setAbility('mummy', target);\n				if (oldAbility) {\n					this.add('-activate', target, 'ability: Mummy', this.dex.abilities.get(oldAbility).name, '[of] ' + source);\n				}\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Moxie,
                    Name = "Moxie",
                    Description = "This Pokemon's Attack is raised by 1 stage if it attacks and knocks out another Pokemon.",
                    ShortDescription = "This Pokemon's Attack is raised by 1 stage if it attacks and KOes another Pokemon.",
                    Rating = 3f,
                    onSourceAfterFaint = "onSourceAfterFaint(length, target, source, effect) {\n			if (effect && effect.effectType === 'Move') {\n				this.boost({atk: length}, source);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Justified,
                    Name = "Justified",
                    Description = "",
                    ShortDescription = "This Pokemon's Attack is raised by 1 stage after it is damaged by a Dark-type move.",
                    Rating = 2.5f,
                    onDamagingHit = "onDamagingHit(damage, target, source, move) {\n			if (move.type === 'Dark') {\n				this.boost({atk: 1});\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Rattled,
                    Name = "Rattled",
                    Description = "This Pokemon's Speed is raised by 1 stage if hit by a Bug-, Dark-, or Ghost-type attack, or Intimidate.",
                    ShortDescription = "Speed is raised 1 stage if hit by a Bug-, Dark-, or Ghost-type attack, or Intimidated.",
                    Rating = 1.5f,
                    onDamagingHit = "onDamagingHit(damage, target, source, move) {\n			if (['Dark', 'Bug', 'Ghost'].includes(move.type)) {\n				this.boost({spe: 1});\n			}\n		}",
                    onAfterBoost = "onAfterBoost(boost, target, source, effect) {\n			if (effect && effect.id === 'intimidate') {\n				this.boost({spe: 1});\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.MagicBounce,
                    Name = "Magic Bounce",
                    Description = "This Pokemon blocks certain status moves and instead uses the move against the original user.",
                    ShortDescription = "This Pokemon blocks certain status moves and bounces them back to the user.",
                    Rating = 4f,
                    isBreakable = true,
                    onTryHitPriority = 1,
                    onTryHit = "onTryHit(target, source, move) {\n			if (target === source || move.hasBounced || !move.flags['reflectable']) {\n				return;\n			}\n			const newMove = this.dex.getActiveMove(move.id);\n			newMove.hasBounced = true;\n			newMove.pranksterBoosted = false;\n			this.actions.useMove(newMove, target, source);\n			return null;\n		}",
                    onAllyTryHitSide = "onAllyTryHitSide(target, source, move) {\n			if (target.isAlly(source) || move.hasBounced || !move.flags['reflectable']) {\n				return;\n			}\n			const newMove = this.dex.getActiveMove(move.id);\n			newMove.hasBounced = true;\n			newMove.pranksterBoosted = false;\n			this.actions.useMove(newMove, this.effectState.target, source);\n			return null;\n		}",
                    condition = "[object Object]",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.SapSipper,
                    Name = "Sap Sipper",
                    Description = "This Pokemon is immune to Grass-type moves and raises its Attack by 1 stage when hit by a Grass-type move.",
                    ShortDescription = "This Pokemon's Attack is raised 1 stage if hit by a Grass move; Grass immunity.",
                    Rating = 3f,
                    isBreakable = true,
                    onTryHitPriority = 1,
                    onTryHit = "onTryHit(target, source, move) {\n			if (target !== source && move.type === 'Grass') {\n				if (!this.boost({atk: 1})) {\n					this.add('-immune', target, '[from] ability: Sap Sipper');\n				}\n				return null;\n			}\n		}",
                    onAllyTryHitSide = "onAllyTryHitSide(target, source, move) {\n			if (source === this.effectState.target || !target.isAlly(source)) return;\n			if (move.type === 'Grass') {\n				this.boost({atk: 1}, this.effectState.target);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Prankster,
                    Name = "Prankster",
                    Description = "",
                    ShortDescription = "This Pokemon's Status moves have priority raised by 1, but Dark types are immune.",
                    Rating = 4f,
                    onModifyPriority = "onModifyPriority(priority, pokemon, target, move) {\n			if (_optionalChain([move, 'optionalAccess', _17 => _17.category]) === 'Status') {\n				move.pranksterBoosted = true;\n				return priority + 1;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.SandForce,
                    Name = "Sand Force",
                    Description = "If Sandstorm is active, this Pokemon's Ground-, Rock-, and Steel-type attacks have their power multiplied by 1.3. This Pokemon takes no damage from Sandstorm.",
                    ShortDescription = "This Pokemon's Ground/Rock/Steel attacks do 1.3x in Sandstorm; immunity to it.",
                    Rating = 2f,
                    onBasePower = "onBasePower(basePower, attacker, defender, move) {\n			if (this.field.isWeather('sandstorm')) {\n				if (move.type === 'Rock' || move.type === 'Ground' || move.type === 'Steel') {\n					this.debug('Sand Force boost');\n					return this.chainModify([5325, 4096]);\n				}\n			}\n		}",
                    onImmunity = "onImmunity(type, pokemon) {\n			if (type === 'sandstorm') return false;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.IronBarbs,
                    Name = "Iron Barbs",
                    Description = "Pokemon making contact with this Pokemon lose 1/8 of their maximum HP, rounded down.",
                    ShortDescription = "Pokemon making contact with this Pokemon lose 1/8 of their max HP.",
                    Rating = 2.5f,
                    onDamagingHitOrder = 1,
                    onDamagingHit = "onDamagingHit(damage, target, source, move) {\n			if (this.checkMoveMakesContact(move, source, target, true)) {\n				this.damage(source.baseMaxhp / 8, source, target);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.ZenMode,
                    Name = "Zen Mode",
                    Description = "If this Pokemon is a Darmanitan or Darmanitan-Galar, it changes to Zen Mode if it has 1/2 or less of its maximum HP at the end of a turn. If Darmanitan's HP is above 1/2 of its maximum HP at the end of a turn, it changes back to Standard Mode. This Ability cannot be removed or suppressed.",
                    ShortDescription = "If Darmanitan, at end of turn changes Mode to Standard if > 1/2 max HP, else Zen.",
                    Rating = 0f,
                    isPermanent = true,
                    onResidualOrder = 29,
                    onEnd = "onEnd(pokemon) {\n			if (!pokemon.volatiles['zenmode'] || !pokemon.hp) return;\n			pokemon.transformed = false;\n			delete pokemon.volatiles['zenmode'];\n			if (pokemon.species.baseSpecies === 'Darmanitan' && pokemon.species.battleOnly) {\n				pokemon.formeChange(pokemon.species.battleOnly , this.effect, false, '[silent]');\n			}\n		}",
                    onResidual = "onResidual(pokemon) {\n			if (pokemon.baseSpecies.baseSpecies !== 'Darmanitan' || pokemon.transformed) {\n				return;\n			}\n			if (pokemon.hp <= pokemon.maxhp / 2 && !['Zen', 'Galar-Zen'].includes(pokemon.species.forme)) {\n				pokemon.addVolatile('zenmode');\n			} else if (pokemon.hp > pokemon.maxhp / 2 && ['Zen', 'Galar-Zen'].includes(pokemon.species.forme)) {\n				pokemon.addVolatile('zenmode'); // in case of base Darmanitan-Zen\n				pokemon.removeVolatile('zenmode');\n			}\n		}",
                    condition = "[object Object]",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.VictoryStar,
                    Name = "Victory Star",
                    Description = "",
                    ShortDescription = "This Pokemon and its allies' moves have their accuracy multiplied by 1.1.",
                    Rating = 2f,
                    onAnyModifyAccuracyPriority = -1,
                    onAnyModifyAccuracy = "onAnyModifyAccuracy(accuracy, target, source) {\n			if (source.isAlly(this.effectState.target) && typeof accuracy === 'number') {\n				return this.chainModify([4506, 4096]);\n			}\n		}",
                    onOther = "onAnyModifyAccuracy(accuracy, target, source) {\n			if (source.isAlly(this.effectState.target) && typeof accuracy === 'number') {\n				return this.chainModify([4506, 4096]);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Turboblaze,
                    Name = "Turboblaze",
                    Description = "This Pokemon's moves and their effects ignore certain Abilities of other Pokemon. The Abilities that can be negated are Aroma Veil, Aura Break, Battle Armor, Big Pecks, Bulletproof, Clear Body, Contrary, Damp, Dark Aura, Dazzling, Disguise, Dry Skin, Fairy Aura, Filter, Flash Fire, Flower Gift, Flower Veil, Fluffy, Friend Guard, Fur Coat, Grass Pelt, Heatproof, Heavy Metal, Hyper Cutter, Ice Face, Ice Scales, Immunity, Inner Focus, Insomnia, Keen Eye, Leaf Guard, Levitate, Light Metal, Lightning Rod, Limber, Magic Bounce, Magma Armor, Marvel Scale, Mirror Armor, Motor Drive, Multiscale, Oblivious, Overcoat, Own Tempo, Pastel Veil, Punk Rock, Queenly Majesty, Sand Veil, Sap Sipper, Shell Armor, Shield Dust, Simple, Snow Cloak, Solid Rock, Soundproof, Sticky Hold, Storm Drain, Sturdy, Suction Cups, Sweet Veil, Tangled Feet, Telepathy, Thick Fat, Unaware, Vital Spirit, Volt Absorb, Water Absorb, Water Bubble, Water Veil, White Smoke, Wonder Guard, and Wonder Skin. This affects every other Pokemon on the field, whether or not it is a target of this Pokemon's move, and whether or not their Ability is beneficial to this Pokemon.",
                    ShortDescription = "This Pokemon's moves and their effects ignore the Abilities of other Pokemon.",
                    Rating = 3.5f,
                    onModifyMove = "onModifyMove(move) {\n			move.ignoreAbility = true;\n		}",
                    onStart = "onStart(pokemon) {\n			this.add('-ability', pokemon, 'Turboblaze');\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Teravolt,
                    Name = "Teravolt",
                    Description = "This Pokemon's moves and their effects ignore certain Abilities of other Pokemon. The Abilities that can be negated are Aroma Veil, Aura Break, Battle Armor, Big Pecks, Bulletproof, Clear Body, Contrary, Damp, Dark Aura, Dazzling, Disguise, Dry Skin, Fairy Aura, Filter, Flash Fire, Flower Gift, Flower Veil, Fluffy, Friend Guard, Fur Coat, Grass Pelt, Heatproof, Heavy Metal, Hyper Cutter, Ice Face, Ice Scales, Immunity, Inner Focus, Insomnia, Keen Eye, Leaf Guard, Levitate, Light Metal, Lightning Rod, Limber, Magic Bounce, Magma Armor, Marvel Scale, Mirror Armor, Motor Drive, Multiscale, Oblivious, Overcoat, Own Tempo, Pastel Veil, Punk Rock, Queenly Majesty, Sand Veil, Sap Sipper, Shell Armor, Shield Dust, Simple, Snow Cloak, Solid Rock, Soundproof, Sticky Hold, Storm Drain, Sturdy, Suction Cups, Sweet Veil, Tangled Feet, Telepathy, Thick Fat, Unaware, Vital Spirit, Volt Absorb, Water Absorb, Water Bubble, Water Veil, White Smoke, Wonder Guard, and Wonder Skin. This affects every other Pokemon on the field, whether or not it is a target of this Pokemon's move, and whether or not their Ability is beneficial to this Pokemon.",
                    ShortDescription = "This Pokemon's moves and their effects ignore the Abilities of other Pokemon.",
                    Rating = 3.5f,
                    onModifyMove = "onModifyMove(move) {\n			move.ignoreAbility = true;\n		}",
                    onStart = "onStart(pokemon) {\n			this.add('-ability', pokemon, 'Teravolt');\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.AromaVeil,
                    Name = "Aroma Veil",
                    Description = "This Pokemon and its allies cannot be affected by Attract, Disable, Encore, Heal Block, Taunt, or Torment.",
                    ShortDescription = "Protects user/allies from Attract, Disable, Encore, Heal Block, Taunt, and Torment.",
                    Rating = 2f,
                    isBreakable = true,
                    onAllyTryAddVolatile = "onAllyTryAddVolatile(status, target, source, effect) {\n			if (['attract', 'disable', 'encore', 'healblock', 'taunt', 'torment'].includes(status.id)) {\n				if (effect.effectType === 'Move') {\n					const effectHolder = this.effectState.target;\n					this.add('-block', target, 'ability: Aroma Veil', '[of] ' + effectHolder);\n				}\n				return null;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.FlowerVeil,
                    Name = "Flower Veil",
                    Description = "Grass-type Pokemon on this Pokemon's side cannot have their stat stages lowered by other Pokemon or have a non-volatile status condition inflicted on them by other Pokemon.",
                    ShortDescription = "This side's Grass types can't have stats lowered or status inflicted by other Pokemon.",
                    Rating = 0f,
                    isBreakable = true,
                    onAllyTryAddVolatile = "onAllyTryAddVolatile(status, target) {\n			if (target.hasType('Grass') && status.id === 'yawn') {\n				this.debug('Flower Veil blocking yawn');\n				const effectHolder = this.effectState.target;\n				this.add('-block', target, 'ability: Flower Veil', '[of] ' + effectHolder);\n				return null;\n			}\n		}",
                    onAllyBoost = "onAllyBoost(boost, target, source, effect) {\n			if ((source && target === source) || !target.hasType('Grass')) return;\n			let showMsg = false;\n			let i;\n			for (i in boost) {\n				if (boost[i] < 0) {\n					delete boost[i];\n					showMsg = true;\n				}\n			}\n			if (showMsg && !(effect ).secondaries) {\n				const effectHolder = this.effectState.target;\n				this.add('-block', target, 'ability: Flower Veil', '[of] ' + effectHolder);\n			}\n		}",
                    onAllySetStatus = "onAllySetStatus(status, target, source, effect) {\n			if (target.hasType('Grass') && source && target !== source && effect && effect.id !== 'yawn') {\n				this.debug('interrupting setStatus with Flower Veil');\n				if (effect.id === 'synchronize' || (effect.effectType === 'Move' && !effect.secondaries)) {\n					const effectHolder = this.effectState.target;\n					this.add('-block', target, 'ability: Flower Veil', '[of] ' + effectHolder);\n				}\n				return null;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.CheekPouch,
                    Name = "Cheek Pouch",
                    Description = "If this Pokemon eats a Berry, it restores 1/3 of its maximum HP, rounded down, in addition to the Berry's effect.",
                    ShortDescription = "If this Pokemon eats a Berry, it restores 1/3 of its max HP after the Berry's effect.",
                    Rating = 2f,
                    onEatItem = "onEatItem(item, pokemon) {\n			this.heal(pokemon.baseMaxhp / 3);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Protean,
                    Name = "Protean",
                    Description = "This Pokemon's type changes to match the type of the move it is about to use. This effect comes after all effects that change a move's type.",
                    ShortDescription = "This Pokemon's type changes to match the type of the move it is about to use.",
                    Rating = 4.5f,
                    onPrepareHit = "onPrepareHit(source, target, move) {\n			if (move.hasBounced || move.sourceEffect === 'snatch') return;\n			const type = move.type;\n			if (type && type !== '???' && source.getTypes().join() !== type) {\n				if (!source.setType(type)) return;\n				this.add('-start', source, 'typechange', type, '[from] ability: Protean');\n			}\n		}",
                },
                new Ability(service)
                {
                    Id = AbilityEnum.FurCoat,
                    Name = "Fur Coat",
                    Description = "",
                    ShortDescription = "This Pokemon's Defense is doubled.",
                    Rating = 4f,
                    isBreakable = true,
                    onModifyDefPriority = 6,
                    onModifyDef = "onModifyDef(def) {\n			return this.chainModify(2);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Magician,
                    Name = "Magician",
                    Description = "If this Pokemon has no item, it steals the item off a Pokemon it hits with an attack. Does not affect Doom Desire and Future Sight.",
                    ShortDescription = "If this Pokemon has no item, it steals the item off a Pokemon it hits with an attack.",
                    Rating = 1.5f,
                    onAfterMoveSecondarySelf = "onAfterMoveSecondarySelf(source, target, move) {\n			if (!move || !target) return;\n			if (target !== source && move.category !== 'Status') {\n				if (source.item || source.volatiles['gem'] || move.id === 'fling') return;\n				const yourItem = target.takeItem(source);\n				if (!yourItem) return;\n				if (!source.setItem(yourItem)) {\n					target.item = yourItem.id; // bypass setItem so we don't break choicelock or anything\n					return;\n				}\n				this.add('-item', source, yourItem, '[from] ability: Magician', '[of] ' + target);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Bulletproof,
                    Name = "Bulletproof",
                    Description = "This Pokemon is immune to ballistic moves. Ballistic moves include Bullet Seed, Octazooka, Barrage, Rock Wrecker, Zap Cannon, Acid Spray, Aura Sphere, Focus Blast, and all moves with Ball or Bomb in their name.",
                    ShortDescription = "Makes user immune to ballistic moves (Shadow Ball, Sludge Bomb, Focus Blast, etc).",
                    Rating = 3f,
                    isBreakable = true,
                    onTryHit = "onTryHit(pokemon, target, move) {\n			if (move.flags['bullet']) {\n				this.add('-immune', pokemon, '[from] ability: Bulletproof');\n				return null;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Competitive,
                    Name = "Competitive",
                    Description = "This Pokemon's Special Attack is raised by 2 stages for each of its stat stages that is lowered by an opposing Pokemon.",
                    ShortDescription = "This Pokemon's Sp. Atk is raised by 2 for each of its stats that is lowered by a foe.",
                    Rating = 2.5f,
                    onAfterEachBoost = "onAfterEachBoost(boost, target, source, effect) {\n			if (!source || target.isAlly(source)) {\n				if (effect.id === 'stickyweb') {\n					this.hint(\"Court Change Sticky Web counts as lowering your own Speed, and Competitive only affects stats lowered by foes.\", true, source.side);\n				}\n				return;\n			}\n			let statsLowered = false;\n			let i;\n			for (i in boost) {\n				if (boost[i] < 0) {\n					statsLowered = true;\n				}\n			}\n			if (statsLowered) {\n				this.add('-ability', target, 'Competitive');\n				this.boost({spa: 2}, target, target, null, true);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.StrongJaw,
                    Name = "Strong Jaw",
                    Description = "This Pokemon's bite-based attacks have their power multiplied by 1.5.",
                    ShortDescription = "This Pokemon's bite-based attacks have 1.5x power. Bug Bite is not boosted.",
                    Rating = 3f,
                    onBasePower = "onBasePower(basePower, attacker, defender, move) {\n			if (move.flags['bite']) {\n				return this.chainModify(1.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Refrigerate,
                    Name = "Refrigerate",
                    Description = "This Pokemon's Normal-type moves become Ice-type moves and have their power multiplied by 1.2. This effect comes after other effects that change a move's type, but before Ion Deluge and Electrify's effects.",
                    ShortDescription = "This Pokemon's Normal-type moves become Ice type and have 1.2x power.",
                    Rating = 4f,
                    onModifyTypePriority = -1,
                    onModifyType = "onModifyType(move, pokemon) {\n			const noModifyType = [\n				'judgment', 'multiattack', 'naturalgift', 'revelationdance', 'technoblast', 'terrainpulse', 'weatherball',\n			];\n			if (move.type === 'Normal' && !noModifyType.includes(move.id) && !(move.isZ && move.category !== 'Status')) {\n				move.type = 'Ice';\n				move.refrigerateBoosted = true;\n			}\n		}",
                    onBasePower = "onBasePower(basePower, pokemon, target, move) {\n			if (move.refrigerateBoosted) return this.chainModify([4915, 4096]);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.SweetVeil,
                    Name = "Sweet Veil",
                    Description = "",
                    ShortDescription = "This Pokemon and its allies cannot fall asleep.",
                    Rating = 2f,
                    isBreakable = true,
                    onAllyTryAddVolatile = "onAllyTryAddVolatile(status, target) {\n			if (status.id === 'yawn') {\n				this.debug('Sweet Veil blocking yawn');\n				const effectHolder = this.effectState.target;\n				this.add('-block', target, 'ability: Sweet Veil', '[of] ' + effectHolder);\n				return null;\n			}\n		}",
                    onAllySetStatus = "onAllySetStatus(status, target, source, effect) {\n			if (status.id === 'slp') {\n				this.debug('Sweet Veil interrupts sleep');\n				const effectHolder = this.effectState.target;\n				this.add('-block', target, 'ability: Sweet Veil', '[of] ' + effectHolder);\n				return null;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.StanceChange,
                    Name = "Stance Change",
                    Description = "If this Pokemon is an Aegislash, it changes to Blade Forme before attempting to use an attacking move, and changes to Shield Forme before attempting to use King's Shield.",
                    ShortDescription = "If Aegislash, changes Forme to Blade before attacks and Shield before King's Shield.",
                    Rating = 4f,
                    isPermanent = true,
                    onModifyMovePriority = 1,
                    onModifyMove = "onModifyMove(move, attacker, defender) {\n			if (attacker.species.baseSpecies !== 'Aegislash' || attacker.transformed) return;\n			if (move.category === 'Status' && move.id !== 'kingsshield') return;\n			const targetForme = (move.id === 'kingsshield' ? 'Aegislash' : 'Aegislash-Blade');\n			if (attacker.species.name !== targetForme) attacker.formeChange(targetForme);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.GaleWings,
                    Name = "Gale Wings",
                    Description = "",
                    ShortDescription = "If this Pokemon is at full HP, its Flying-type moves have their priority increased by 1.",
                    Rating = 3f,
                    onModifyPriority = "onModifyPriority(priority, pokemon, target, move) {\n			if (_optionalChain([move, 'optionalAccess', _4 => _4.type]) === 'Flying' && pokemon.hp === pokemon.maxhp) return priority + 1;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.MegaLauncher,
                    Name = "Mega Launcher",
                    Description = "This Pokemon's pulse moves have their power multiplied by 1.5. Heal Pulse restores 3/4 of a target's maximum HP, rounded half down.",
                    ShortDescription = "This Pokemon's pulse moves have 1.5x power. Heal Pulse heals 3/4 target's max HP.",
                    Rating = 3f,
                    onBasePower = "onBasePower(basePower, attacker, defender, move) {\n			if (move.flags['pulse']) {\n				return this.chainModify(1.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.GrassPelt,
                    Name = "Grass Pelt",
                    Description = "",
                    ShortDescription = "If Grassy Terrain is active, this Pokemon's Defense is multiplied by 1.5.",
                    Rating = 0.5f,
                    isBreakable = true,
                    onModifyDefPriority = 6,
                    onModifyDef = "onModifyDef(pokemon) {\n			if (this.field.isTerrain('grassyterrain')) return this.chainModify(1.5);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Symbiosis,
                    Name = "Symbiosis",
                    Description = "If an ally uses its item, this Pokemon gives its item to that ally immediately. Does not activate if the ally's item was stolen or knocked off.",
                    ShortDescription = "If an ally uses its item, this Pokemon gives its item to that ally immediately.",
                    Rating = 0f,
                    onAllyAfterUseItem = "onAllyAfterUseItem(item, pokemon) {\n			if (pokemon.switchFlag) return;\n			const source = this.effectState.target;\n			const myItem = source.takeItem();\n			if (!myItem) return;\n			if (\n				!this.singleEvent('TakeItem', myItem, source.itemState, pokemon, source, this.effect, myItem) ||\n				!pokemon.setItem(myItem)\n			) {\n				source.item = myItem.id;\n				return;\n			}\n			this.add('-activate', source, 'ability: Symbiosis', myItem, '[of] ' + pokemon);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.ToughClaws,
                    Name = "Tough Claws",
                    Description = "",
                    ShortDescription = "This Pokemon's contact moves have their power multiplied by 1.3.",
                    Rating = 3.5f,
                    onBasePower = "onBasePower(basePower, attacker, defender, move) {\n			if (move.flags['contact']) {\n				return this.chainModify([5325, 4096]);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Pixilate,
                    Name = "Pixilate",
                    Description = "This Pokemon's Normal-type moves become Fairy-type moves and have their power multiplied by 1.2. This effect comes after other effects that change a move's type, but before Ion Deluge and Electrify's effects.",
                    ShortDescription = "This Pokemon's Normal-type moves become Fairy type and have 1.2x power.",
                    Rating = 4f,
                    onModifyTypePriority = -1,
                    onModifyType = "onModifyType(move, pokemon) {\n			const noModifyType = [\n				'judgment', 'multiattack', 'naturalgift', 'revelationdance', 'technoblast', 'terrainpulse', 'weatherball',\n			];\n			if (move.type === 'Normal' && !noModifyType.includes(move.id) && !(move.isZ && move.category !== 'Status')) {\n				move.type = 'Fairy';\n				move.pixilateBoosted = true;\n			}\n		}",
                    onBasePower = "onBasePower(basePower, pokemon, target, move) {\n			if (move.pixilateBoosted) return this.chainModify([4915, 4096]);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Gooey,
                    Name = "Gooey",
                    Description = "",
                    ShortDescription = "Pokemon making contact with this Pokemon have their Speed lowered by 1 stage.",
                    Rating = 2f,
                    onDamagingHit = "onDamagingHit(damage, target, source, move) {\n			if (this.checkMoveMakesContact(move, source, target, true)) {\n				this.add('-ability', target, 'Gooey');\n				this.boost({spe: -1}, source, target, null, true);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Aerilate,
                    Name = "Aerilate",
                    Description = "This Pokemon's Normal-type moves become Flying-type moves and have their power multiplied by 1.2. This effect comes after other effects that change a move's type, but before Ion Deluge and Electrify's effects.",
                    ShortDescription = "This Pokemon's Normal-type moves become Flying type and have 1.2x power.",
                    Rating = 4f,
                    onModifyTypePriority = -1,
                    onModifyType = "onModifyType(move, pokemon) {\n			const noModifyType = [\n				'judgment', 'multiattack', 'naturalgift', 'revelationdance', 'technoblast', 'terrainpulse', 'weatherball',\n			];\n			if (move.type === 'Normal' && !noModifyType.includes(move.id) && !(move.isZ && move.category !== 'Status')) {\n				move.type = 'Flying';\n				move.aerilateBoosted = true;\n			}\n		}",
                    onBasePower = "onBasePower(basePower, pokemon, target, move) {\n			if (move.aerilateBoosted) return this.chainModify([4915, 4096]);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.ParentalBond,
                    Name = "Parental Bond",
                    Description = "This Pokemon's damaging moves become multi-hit moves that hit twice. The second hit has its damage quartered. Does not affect multi-hit moves or moves that have multiple targets.",
                    ShortDescription = "This Pokemon's damaging moves hit twice. The second hit has its damage quartered.",
                    Rating = 4.5f,
                    onPrepareHit = "onPrepareHit(source, target, move) {\n			if (move.category === 'Status' || move.selfdestruct || move.multihit) return;\n			if (['endeavor', 'fling', 'iceball', 'rollout'].includes(move.id)) return;\n			if (!move.flags['charge'] && !move.spreadHit && !move.isZ && !move.isMax) {\n				move.multihit = 2;\n				move.multihitType = 'parentalbond';\n			}\n		}",
                    onSourceModifySecondaries = "onSourceModifySecondaries(secondaries, target, source, move) {\n			if (move.multihitType === 'parentalbond' && move.id === 'secretpower' && move.hit < 2) {\n				// hack to prevent accidentally suppressing King's Rock/Razor Fang\n				return secondaries.filter(effect => effect.volatileStatus === 'flinch');\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.DarkAura,
                    Name = "Dark Aura",
                    Description = "While this Pokemon is active, the power of Dark-type moves used by active Pokemon is multiplied by 1.33.",
                    ShortDescription = "While this Pokemon is active, a Dark move used by any Pokemon has 1.33x power.",
                    Rating = 3f,
                    isBreakable = true,
                    onAnyBasePowerPriority = 20,
                    onStart = "onStart(pokemon) {\n			if (this.suppressingAbility(pokemon)) return;\n			this.add('-ability', pokemon, 'Dark Aura');\n		}",
                    onAnyBasePower = "onAnyBasePower(basePower, source, target, move) {\n			if (target === source || move.category === 'Status' || move.type !== 'Dark') return;\n			if (!move.auraBooster) move.auraBooster = this.effectState.target;\n			if (move.auraBooster !== this.effectState.target) return;\n			return this.chainModify([move.hasAuraBreak ? 3072 : 5448, 4096]);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.FairyAura,
                    Name = "Fairy Aura",
                    Description = "While this Pokemon is active, the power of Fairy-type moves used by active Pokemon is multiplied by 1.33.",
                    ShortDescription = "While this Pokemon is active, a Fairy move used by any Pokemon has 1.33x power.",
                    Rating = 3f,
                    isBreakable = true,
                    onAnyBasePowerPriority = 20,
                    onStart = "onStart(pokemon) {\n			if (this.suppressingAbility(pokemon)) return;\n			this.add('-ability', pokemon, 'Fairy Aura');\n		}",
                    onAnyBasePower = "onAnyBasePower(basePower, source, target, move) {\n			if (target === source || move.category === 'Status' || move.type !== 'Fairy') return;\n			if (!move.auraBooster) move.auraBooster = this.effectState.target;\n			if (move.auraBooster !== this.effectState.target) return;\n			return this.chainModify([move.hasAuraBreak ? 3072 : 5448, 4096]);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.AuraBreak,
                    Name = "Aura Break",
                    Description = "While this Pokemon is active, the effects of the Dark Aura and Fairy Aura Abilities are reversed, multiplying the power of Dark- and Fairy-type moves, respectively, by 3/4 instead of 1.33.",
                    ShortDescription = "While this Pokemon is active, the Dark Aura and Fairy Aura power modifier is 0.75x.",
                    Rating = 1f,
                    isBreakable = true,
                    onStart = "onStart(pokemon) {\n			if (this.suppressingAbility(pokemon)) return;\n			this.add('-ability', pokemon, 'Aura Break');\n		}",
                    onAnyTryPrimaryHit = "onAnyTryPrimaryHit(target, source, move) {\n			if (target === source || move.category === 'Status') return;\n			move.hasAuraBreak = true;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.PrimordialSea,
                    Name = "Primordial Sea",
                    Description = "On switch-in, the weather becomes heavy rain that prevents damaging Fire-type moves from executing, in addition to all the effects of Rain Dance. This weather remains in effect until this Ability is no longer active for any Pokemon, or the weather is changed by Delta Stream or Desolate Land.",
                    ShortDescription = "On switch-in, heavy rain begins until this Ability is not active in battle.",
                    Rating = 4.5f,
                    onStart = "onStart(source) {\n			this.field.setWeather('primordialsea');\n		}",
                    onEnd = "onEnd(pokemon) {\n			if (this.field.weatherState.source !== pokemon) return;\n			for (const target of this.getAllActive()) {\n				if (target === pokemon) continue;\n				if (target.hasAbility('primordialsea')) {\n					this.field.weatherState.source = target;\n					return;\n				}\n			}\n			this.field.clearWeather();\n		}",
                    onAnySetWeather = "onAnySetWeather(target, source, weather) {\n			const strongWeathers = ['desolateland', 'primordialsea', 'deltastream'];\n			if (this.field.getWeather().id === 'primordialsea' && !strongWeathers.includes(weather.id)) return false;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.DesolateLand,
                    Name = "Desolate Land",
                    Description = "On switch-in, the weather becomes extremely harsh sunlight that prevents damaging Water-type moves from executing, in addition to all the effects of Sunny Day. This weather remains in effect until this Ability is no longer active for any Pokemon, or the weather is changed by Delta Stream or Primordial Sea.",
                    ShortDescription = "On switch-in, extremely harsh sunlight begins until this Ability is not active in battle.",
                    Rating = 4.5f,
                    onStart = "onStart(source) {\n			this.field.setWeather('desolateland');\n		}",
                    onEnd = "onEnd(pokemon) {\n			if (this.field.weatherState.source !== pokemon) return;\n			for (const target of this.getAllActive()) {\n				if (target === pokemon) continue;\n				if (target.hasAbility('desolateland')) {\n					this.field.weatherState.source = target;\n					return;\n				}\n			}\n			this.field.clearWeather();\n		}",
                    onAnySetWeather = "onAnySetWeather(target, source, weather) {\n			const strongWeathers = ['desolateland', 'primordialsea', 'deltastream'];\n			if (this.field.getWeather().id === 'desolateland' && !strongWeathers.includes(weather.id)) return false;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.DeltaStream,
                    Name = "Delta Stream",
                    Description = "On switch-in, the weather becomes strong winds that remove the weaknesses of the Flying type from Flying-type Pokemon. This weather remains in effect until this Ability is no longer active for any Pokemon, or the weather is changed by Desolate Land or Primordial Sea.",
                    ShortDescription = "On switch-in, strong winds begin until this Ability is not active in battle.",
                    Rating = 4f,
                    onStart = "onStart(source) {\n			this.field.setWeather('deltastream');\n		}",
                    onEnd = "onEnd(pokemon) {\n			if (this.field.weatherState.source !== pokemon) return;\n			for (const target of this.getAllActive()) {\n				if (target === pokemon) continue;\n				if (target.hasAbility('deltastream')) {\n					this.field.weatherState.source = target;\n					return;\n				}\n			}\n			this.field.clearWeather();\n		}",
                    onAnySetWeather = "onAnySetWeather(target, source, weather) {\n			const strongWeathers = ['desolateland', 'primordialsea', 'deltastream'];\n			if (this.field.getWeather().id === 'deltastream' && !strongWeathers.includes(weather.id)) return false;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Stamina,
                    Name = "Stamina",
                    Description = "",
                    ShortDescription = "This Pokemon's Defense is raised by 1 stage after it is damaged by a move.",
                    Rating = 3.5f,
                    onDamagingHit = "onDamagingHit(damage, target, source, effect) {\n			this.boost({def: 1});\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.WimpOut,
                    Name = "Wimp Out",
                    Description = "When this Pokemon has more than 1/2 its maximum HP and takes damage bringing it to 1/2 or less of its maximum HP, it immediately switches out to a chosen ally. This effect applies after all hits from a multi-hit move; Sheer Force prevents it from activating if the move has a secondary effect. This effect applies to both direct and indirect damage, except Curse and Substitute on use, Belly Drum, Pain Split, and confusion damage.",
                    ShortDescription = "This Pokemon switches out when it reaches 1/2 or less of its maximum HP.",
                    Rating = 1f,
                    onEmergencyExit = "onEmergencyExit(target) {\n			if (!this.canSwitch(target.side) || target.forceSwitchFlag || target.switchFlag) return;\n			for (const side of this.sides) {\n				for (const active of side.active) {\n					active.switchFlag = false;\n				}\n			}\n			target.switchFlag = true;\n			this.add('-activate', target, 'ability: Wimp Out');\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.EmergencyExit,
                    Name = "Emergency Exit",
                    Description = "When this Pokemon has more than 1/2 its maximum HP and takes damage bringing it to 1/2 or less of its maximum HP, it immediately switches out to a chosen ally. This effect applies after all hits from a multi-hit move; Sheer Force prevents it from activating if the move has a secondary effect. This effect applies to both direct and indirect damage, except Curse and Substitute on use, Belly Drum, Pain Split, and confusion damage.",
                    ShortDescription = "This Pokemon switches out when it reaches 1/2 or less of its maximum HP.",
                    Rating = 1f,
                    onEmergencyExit = "onEmergencyExit(target) {\n			if (!this.canSwitch(target.side) || target.forceSwitchFlag || target.switchFlag) return;\n			for (const side of this.sides) {\n				for (const active of side.active) {\n					active.switchFlag = false;\n				}\n			}\n			target.switchFlag = true;\n			this.add('-activate', target, 'ability: Emergency Exit');\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.WaterCompaction,
                    Name = "Water Compaction",
                    Description = "",
                    ShortDescription = "This Pokemon's Defense is raised 2 stages after it is damaged by a Water-type move.",
                    Rating = 1.5f,
                    onDamagingHit = "onDamagingHit(damage, target, source, move) {\n			if (move.type === 'Water') {\n				this.boost({def: 2});\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Merciless,
                    Name = "Merciless",
                    Description = "",
                    ShortDescription = "This Pokemon's attacks are critical hits if the target is poisoned.",
                    Rating = 1.5f,
                    onModifyCritRatio = "onModifyCritRatio(critRatio, source, target) {\n			if (target && ['psn', 'tox'].includes(target.status)) return 5;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.ShieldsDown,
                    Name = "Shields Down",
                    Description = "If this Pokemon is a Minior, it changes to its Core forme if it has 1/2 or less of its maximum HP, and changes to Meteor Form if it has more than 1/2 its maximum HP. This check is done on switch-in and at the end of each turn. While in its Meteor Form, it cannot become affected by non-volatile status conditions. Moongeist Beam, Sunsteel Strike, and the Mold Breaker, Teravolt, and Turboblaze Abilities cannot ignore this Ability.",
                    ShortDescription = "If Minior, switch-in/end of turn it changes to Core at 1/2 max HP or less, else Meteor.",
                    Rating = 3f,
                    isPermanent = true,
                    onResidualOrder = 29,
                    onStart = "onStart(pokemon) {\n			if (pokemon.baseSpecies.baseSpecies !== 'Minior' || pokemon.transformed) return;\n			if (pokemon.hp > pokemon.maxhp / 2) {\n				if (pokemon.species.forme !== 'Meteor') {\n					pokemon.formeChange('Minior-Meteor');\n				}\n			} else {\n				if (pokemon.species.forme === 'Meteor') {\n					pokemon.formeChange(pokemon.set.species);\n				}\n			}\n		}",
                    onResidual = "onResidual(pokemon) {\n			if (pokemon.baseSpecies.baseSpecies !== 'Minior' || pokemon.transformed || !pokemon.hp) return;\n			if (pokemon.hp > pokemon.maxhp / 2) {\n				if (pokemon.species.forme !== 'Meteor') {\n					pokemon.formeChange('Minior-Meteor');\n				}\n			} else {\n				if (pokemon.species.forme === 'Meteor') {\n					pokemon.formeChange(pokemon.set.species);\n				}\n			}\n		}",
                    onSetStatus = "onSetStatus(status, target, source, effect) {\n			if (target.species.id !== 'miniormeteor' || target.transformed) return;\n			if (_optionalChain([(effect ), 'optionalAccess', _20 => _20.status])) {\n				this.add('-immune', target, '[from] ability: Shields Down');\n			}\n			return false;\n		}",
                    onTryAddVolatile = "onTryAddVolatile(status, target) {\n			if (target.species.id !== 'miniormeteor' || target.transformed) return;\n			if (status.id !== 'yawn') return;\n			this.add('-immune', target, '[from] ability: Shields Down');\n			return null;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Stakeout,
                    Name = "Stakeout",
                    Description = "",
                    ShortDescription = "This Pokemon's attacking stat is doubled against a target that switched in this turn.",
                    Rating = 4.5f,
                    onModifyAtkPriority = 5,
                    onModifySpAPriority = 5,
                    onModifyAtk = "onModifyAtk(atk, attacker, defender) {\n			if (!defender.activeTurns) {\n				this.debug('Stakeout boost');\n				return this.chainModify(2);\n			}\n		}",
                    onModifySpA = "onModifySpA(atk, attacker, defender) {\n			if (!defender.activeTurns) {\n				this.debug('Stakeout boost');\n				return this.chainModify(2);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.WaterBubble,
                    Name = "Water Bubble",
                    Description = "This Pokemon's attacking stat is doubled while using a Water-type attack. If a Pokemon uses a Fire-type attack against this Pokemon, that Pokemon's attacking stat is halved when calculating the damage to this Pokemon. This Pokemon cannot be burned. Gaining this Ability while burned cures it.",
                    ShortDescription = "This Pokemon's Water power is 2x; it can't be burned; Fire power against it is halved.",
                    Rating = 4.5f,
                    isBreakable = true,
                    onSourceModifyAtkPriority = 5,
                    onSourceModifySpAPriority = 5,
                    onModifyAtk = "onModifyAtk(atk, attacker, defender, move) {\n			if (move.type === 'Water') {\n				return this.chainModify(2);\n			}\n		}",
                    onModifySpA = "onModifySpA(atk, attacker, defender, move) {\n			if (move.type === 'Water') {\n				return this.chainModify(2);\n			}\n		}",
                    onSetStatus = "onSetStatus(status, target, source, effect) {\n			if (status.id !== 'brn') return;\n			if (_optionalChain([(effect ), 'optionalAccess', _26 => _26.status])) {\n				this.add('-immune', target, '[from] ability: Water Bubble');\n			}\n			return false;\n		}",
                    onUpdate = "onUpdate(pokemon) {\n			if (pokemon.status === 'brn') {\n				this.add('-activate', pokemon, 'ability: Water Bubble');\n				pokemon.cureStatus();\n			}\n		}",
                    onSourceModifyAtk = "onSourceModifyAtk(atk, attacker, defender, move) {\n			if (move.type === 'Fire') {\n				return this.chainModify(0.5);\n			}\n		}",
                    onSourceModifySpA = "onSourceModifySpA(atk, attacker, defender, move) {\n			if (move.type === 'Fire') {\n				return this.chainModify(0.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Steelworker,
                    Name = "Steelworker",
                    Description = "",
                    ShortDescription = "This Pokemon's attacking stat is multiplied by 1.5 while using a Steel-type attack.",
                    Rating = 3.5f,
                    onModifyAtkPriority = 5,
                    onModifySpAPriority = 5,
                    onModifyAtk = "onModifyAtk(atk, attacker, defender, move) {\n			if (move.type === 'Steel') {\n				this.debug('Steelworker boost');\n				return this.chainModify(1.5);\n			}\n		}",
                    onModifySpA = "onModifySpA(atk, attacker, defender, move) {\n			if (move.type === 'Steel') {\n				this.debug('Steelworker boost');\n				return this.chainModify(1.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Berserk,
                    Name = "Berserk",
                    Description = "When this Pokemon has more than 1/2 its maximum HP and takes damage from an attack bringing it to 1/2 or less of its maximum HP, its Special Attack is raised by 1 stage. This effect applies after all hits from a multi-hit move; Sheer Force prevents it from activating if the move has a secondary effect.",
                    ShortDescription = "This Pokemon's Sp. Atk is raised by 1 when it reaches 1/2 or less of its max HP.",
                    Rating = 2f,
                    onDamage = "onDamage(damage, target, source, effect) {\n			if (\n				effect.effectType === \"Move\" &&\n				!effect.multihit &&\n				(!effect.negateSecondary && !(effect.hasSheerForce && source.hasAbility('sheerforce')))\n			) {\n				target.abilityState.checkedBerserk = false;\n			} else {\n				target.abilityState.checkedBerserk = true;\n			}\n		}",
                    onTryEatItem = "onTryEatItem(item, pokemon) {\n			const healingItems = [\n				'aguavberry', 'enigmaberry', 'figyberry', 'iapapaberry', 'magoberry', 'sitrusberry', 'wikiberry', 'oranberry', 'berryjuice',\n			];\n			if (healingItems.includes(item.id)) {\n				return pokemon.abilityState.checkedBerserk;\n			}\n			return true;\n		}",
                    onAfterMoveSecondary = "onAfterMoveSecondary(target, source, move) {\n			target.abilityState.checkedBerserk = true;\n			if (!source || source === target || !target.hp || !move.totalDamage) return;\n			const lastAttackedBy = target.getLastAttackedBy();\n			if (!lastAttackedBy) return;\n			const damage = move.multihit ? move.totalDamage : lastAttackedBy.damage;\n			if (target.hp <= target.maxhp / 2 && target.hp + damage > target.maxhp / 2) {\n				this.boost({spa: 1});\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.SlushRush,
                    Name = "Slush Rush",
                    Description = "",
                    ShortDescription = "If Hail is active, this Pokemon's Speed is doubled.",
                    Rating = 3f,
                    onModifySpe = "onModifySpe(spe, pokemon) {\n			if (this.field.isWeather('hail')) {\n				return this.chainModify(2);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.LongReach,
                    Name = "Long Reach",
                    Description = "",
                    ShortDescription = "This Pokemon's attacks do not make contact with the target.",
                    Rating = 1f,
                    onModifyMove = "onModifyMove(move) {\n			delete move.flags['contact'];\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.LiquidVoice,
                    Name = "Liquid Voice",
                    Description = "This Pokemon's sound-based moves become Water-type moves. This effect comes after other effects that change a move's type, but before Ion Deluge and Electrify's effects.",
                    ShortDescription = "This Pokemon's sound-based moves become Water type.",
                    Rating = 1.5f,
                    onModifyTypePriority = -1,
                    onModifyType = "onModifyType(move, pokemon) {\n			if (move.flags['sound'] && !pokemon.volatiles['dynamax']) { // hardcode\n				move.type = 'Water';\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Triage,
                    Name = "Triage",
                    Description = "",
                    ShortDescription = "This Pokemon's healing moves have their priority increased by 3.",
                    Rating = 3.5f,
                    onModifyPriority = "onModifyPriority(priority, pokemon, target, move) {\n			if (_optionalChain([move, 'optionalAccess', _23 => _23.flags, 'access', _24 => _24['heal']])) return priority + 3;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Galvanize,
                    Name = "Galvanize",
                    Description = "This Pokemon's Normal-type moves become Electric-type moves and have their power multiplied by 1.2. This effect comes after other effects that change a move's type, but before Ion Deluge and Electrify's effects.",
                    ShortDescription = "This Pokemon's Normal-type moves become Electric type and have 1.2x power.",
                    Rating = 4f,
                    onModifyTypePriority = -1,
                    onModifyType = "onModifyType(move, pokemon) {\n			const noModifyType = [\n				'judgment', 'multiattack', 'naturalgift', 'revelationdance', 'technoblast', 'terrainpulse', 'weatherball',\n			];\n			if (move.type === 'Normal' && !noModifyType.includes(move.id) && !(move.isZ && move.category !== 'Status')) {\n				move.type = 'Electric';\n				move.galvanizeBoosted = true;\n			}\n		}",
                    onBasePower = "onBasePower(basePower, pokemon, target, move) {\n			if (move.galvanizeBoosted) return this.chainModify([4915, 4096]);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.SurgeSurfer,
                    Name = "Surge Surfer",
                    Description = "",
                    ShortDescription = "If Electric Terrain is active, this Pokemon's Speed is doubled.",
                    Rating = 3f,
                    onModifySpe = "onModifySpe(spe) {\n			if (this.field.isTerrain('electricterrain')) {\n				return this.chainModify(2);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Schooling,
                    Name = "Schooling",
                    Description = "On switch-in, if this Pokemon is a Wishiwashi that is level 20 or above and has more than 1/4 of its maximum HP left, it changes to School Form. If it is in School Form and its HP drops to 1/4 of its maximum HP or less, it changes to Solo Form at the end of the turn. If it is in Solo Form and its HP is greater than 1/4 its maximum HP at the end of the turn, it changes to School Form.",
                    ShortDescription = "If user is Wishiwashi, changes to School Form if it has > 1/4 max HP, else Solo Form.",
                    Rating = 3f,
                    isPermanent = true,
                    onResidualOrder = 29,
                    onStart = "onStart(pokemon) {\n			if (pokemon.baseSpecies.baseSpecies !== 'Wishiwashi' || pokemon.level < 20 || pokemon.transformed) return;\n			if (pokemon.hp > pokemon.maxhp / 4) {\n				if (pokemon.species.id === 'wishiwashi') {\n					pokemon.formeChange('Wishiwashi-School');\n				}\n			} else {\n				if (pokemon.species.id === 'wishiwashischool') {\n					pokemon.formeChange('Wishiwashi');\n				}\n			}\n		}",
                    onResidual = "onResidual(pokemon) {\n			if (\n				pokemon.baseSpecies.baseSpecies !== 'Wishiwashi' || pokemon.level < 20 ||\n				pokemon.transformed || !pokemon.hp\n			) return;\n			if (pokemon.hp > pokemon.maxhp / 4) {\n				if (pokemon.species.id === 'wishiwashi') {\n					pokemon.formeChange('Wishiwashi-School');\n				}\n			} else {\n				if (pokemon.species.id === 'wishiwashischool') {\n					pokemon.formeChange('Wishiwashi');\n				}\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Disguise,
                    Name = "Disguise",
                    Description = "If this Pokemon is a Mimikyu, the first hit it takes in battle deals 0 neutral damage. Its disguise is then broken, it changes to Busted Form, and it loses 1/8 of its max HP. Confusion damage also breaks the disguise.",
                    ShortDescription = "(Mimikyu only) The first hit it takes is blocked, and it takes 1/8 HP damage instead.",
                    Rating = 3.5f,
                    isBreakable = true,
                    isPermanent = true,
                    onDamagePriority = 1,
                    onCriticalHit = "onCriticalHit(target, source, move) {\n			if (!target) return;\n			if (!['mimikyu', 'mimikyutotem'].includes(target.species.id) || target.transformed) {\n				return;\n			}\n			const hitSub = target.volatiles['substitute'] && !move.flags['authentic'] && !(move.infiltrates && this.gen >= 6);\n			if (hitSub) return;\n\n			if (!target.runImmunity(move.type)) return;\n			return false;\n		}",
                    onDamage = "onDamage(damage, target, source, effect) {\n			if (\n				effect && effect.effectType === 'Move' &&\n				['mimikyu', 'mimikyutotem'].includes(target.species.id) && !target.transformed\n			) {\n				this.add('-activate', target, 'ability: Disguise');\n				this.effectState.busted = true;\n				return 0;\n			}\n		}",
                    onEffectiveness = "onEffectiveness(typeMod, target, type, move) {\n			if (!target || move.category === 'Status') return;\n			if (!['mimikyu', 'mimikyutotem'].includes(target.species.id) || target.transformed) {\n				return;\n			}\n\n			const hitSub = target.volatiles['substitute'] && !move.flags['authentic'] && !(move.infiltrates && this.gen >= 6);\n			if (hitSub) return;\n\n			if (!target.runImmunity(move.type)) return;\n			return 0;\n		}",
                    onUpdate = "onUpdate(pokemon) {\n			if (['mimikyu', 'mimikyutotem'].includes(pokemon.species.id) && this.effectState.busted) {\n				const speciesid = pokemon.species.id === 'mimikyutotem' ? 'Mimikyu-Busted-Totem' : 'Mimikyu-Busted';\n				pokemon.formeChange(speciesid, this.effect, true);\n				this.damage(pokemon.baseMaxhp / 8, pokemon, pokemon, this.dex.species.get(speciesid));\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.BattleBond,
                    Name = "Battle Bond",
                    Description = "If this Pokemon is a Greninja, it transforms into Ash-Greninja after knocking out a Pokemon. As Ash-Greninja, its Water Shuriken has 20 base power and always hits 3 times.",
                    ShortDescription = "After KOing a Pokemon: becomes Ash-Greninja, Water Shuriken: 20 power, hits 3x.",
                    Rating = 4f,
                    isPermanent = true,
                    onModifyMovePriority = -1,
                    onModifyMove = "onModifyMove(move, attacker) {\n			if (move.id === 'watershuriken' && attacker.species.name === 'Greninja-Ash') {\n				move.multihit = 3;\n			}\n		}",
                    onSourceAfterFaint = "onSourceAfterFaint(length, target, source, effect) {\n			if (_optionalChain([effect, 'optionalAccess', _2 => _2.effectType]) !== 'Move') {\n				return;\n			}\n			if (source.species.id === 'greninja' && source.hp && !source.transformed && source.side.foePokemonLeft()) {\n				this.add('-activate', source, 'ability: Battle Bond');\n				source.formeChange('Greninja-Ash', this.effect, true);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.PowerConstruct,
                    Name = "Power Construct",
                    Description = "If this Pokemon is a Zygarde in its 10% or 50% Forme, it changes to Complete Forme when it has 1/2 or less of its maximum HP at the end of the turn.",
                    ShortDescription = "If Zygarde 10%/50%, changes to Complete if at 1/2 max HP or less at end of turn.",
                    Rating = 5f,
                    isPermanent = true,
                    onResidualOrder = 29,
                    onResidual = "onResidual(pokemon) {\n			if (pokemon.baseSpecies.baseSpecies !== 'Zygarde' || pokemon.transformed || !pokemon.hp) return;\n			if (pokemon.species.id === 'zygardecomplete' || pokemon.hp > pokemon.maxhp / 2) return;\n			this.add('-activate', pokemon, 'ability: Power Construct');\n			pokemon.formeChange('Zygarde-Complete', this.effect, true);\n			pokemon.baseMaxhp = Math.floor(Math.floor(\n				2 * pokemon.species.baseStats['hp'] + pokemon.set.ivs['hp'] + Math.floor(pokemon.set.evs['hp'] / 4) + 100\n			) * pokemon.level / 100 + 10);\n			const newMaxHP = pokemon.volatiles['dynamax'] ? (2 * pokemon.baseMaxhp) : pokemon.baseMaxhp;\n			pokemon.hp = newMaxHP - (pokemon.maxhp - pokemon.hp);\n			pokemon.maxhp = newMaxHP;\n			this.add('-heal', pokemon, pokemon.getHealth, '[silent]');\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Corrosion,
                    Name = "Corrosion",
                    Description = "",
                    ShortDescription = "This Pokemon can poison or badly poison other Pokemon regardless of their typing.",
                    Rating = 2.5f,
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Comatose,
                    Name = "Comatose",
                    Description = "This Pokemon cannot be statused, and is considered to be asleep. Moongeist Beam, Sunsteel Strike, and the Mold Breaker, Teravolt, and Turboblaze Abilities cannot ignore this Ability.",
                    ShortDescription = "This Pokemon cannot be statused, and is considered to be asleep.",
                    Rating = 4f,
                    isPermanent = true,
                    onStart = "onStart(pokemon) {\n			this.add('-ability', pokemon, 'Comatose');\n		}",
                    onSetStatus = "onSetStatus(status, target, source, effect) {\n			if (_optionalChain([(effect ), 'optionalAccess', _3 => _3.status])) {\n				this.add('-immune', target, '[from] ability: Comatose');\n			}\n			return false;\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.QueenlyMajesty,
                    Name = "Queenly Majesty",
                    Description = "While this Pokemon is active, priority moves from opposing Pokemon targeted at allies are prevented from having an effect.",
                    ShortDescription = "While this Pokemon is active, allies are protected from opposing priority moves.",
                    Rating = 2.5f,
                    isBreakable = true,
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.InnardsOut,
                    Name = "Innards Out",
                    Description = "If this Pokemon is knocked out with a move, that move's user loses HP equal to the amount of damage inflicted on this Pokemon.",
                    ShortDescription = "If this Pokemon is KOed with a move, that move's user loses an equal amount of HP.",
                    Rating = 4f,
                    onDamagingHitOrder = 1,
                    onDamagingHit = "onDamagingHit(damage, target, source, move) {\n			if (!target.hp) {\n				this.damage(target.getUndynamaxedHP(damage), source, target);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Dancer,
                    Name = "Dancer",
                    Description = "After another Pokemon uses a dance move, this Pokemon uses the same move. Moves used by this Ability cannot be copied again.",
                    ShortDescription = "After another Pokemon uses a dance move, this Pokemon uses the same move.",
                    Rating = 1.5f,
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Battery,
                    Name = "Battery",
                    Description = "",
                    ShortDescription = "This Pokemon's allies have the power of their special attacks multiplied by 1.3.",
                    Rating = 0f,
                    onAllyBasePowerPriority = 22,
                    onAllyBasePower = "onAllyBasePower(basePower, attacker, defender, move) {\n			if (attacker !== this.effectState.target && move.category === 'Special') {\n				this.debug('Battery boost');\n				return this.chainModify([5325, 4096]);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Fluffy,
                    Name = "Fluffy",
                    Description = "This Pokemon receives 1/2 damage from contact moves, but double damage from Fire moves.",
                    ShortDescription = "This Pokemon takes 1/2 damage from contact moves, 2x damage from Fire moves.",
                    Rating = 3.5f,
                    isBreakable = true,
                    onSourceModifyDamage = "onSourceModifyDamage(damage, source, target, move) {\n			let mod = 1;\n			if (move.type === 'Fire') mod *= 2;\n			if (move.flags['contact']) mod /= 2;\n			return this.chainModify(mod);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Dazzling,
                    Name = "Dazzling",
                    Description = "While this Pokemon is active, priority moves from opposing Pokemon targeted at allies are prevented from having an effect.",
                    ShortDescription = "While this Pokemon is active, allies are protected from opposing priority moves.",
                    Rating = 2.5f,
                    isBreakable = true,
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.SoulHeart,
                    Name = "Soul-Heart",
                    Description = "This Pokemon's Special Attack is raised by 1 stage when another Pokemon faints.",
                    ShortDescription = "This Pokemon's Sp. Atk is raised by 1 stage when another Pokemon faints.",
                    Rating = 3.5f,
                    onAnyFaintPriority = 1,
                    onAnyFaint = "onAnyFaint() {\n			this.boost({spa: 1}, this.effectState.target);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.TanglingHair,
                    Name = "Tangling Hair",
                    Description = "",
                    ShortDescription = "Pokemon making contact with this Pokemon have their Speed lowered by 1 stage.",
                    Rating = 2f,
                    onDamagingHit = "onDamagingHit(damage, target, source, move) {\n			if (this.checkMoveMakesContact(move, source, target, true)) {\n				this.add('-ability', target, 'Tangling Hair');\n				this.boost({spe: -1}, source, target, null, true);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Receiver,
                    Name = "Receiver",
                    Description = "This Pokemon copies the Ability of an ally that faints. Abilities that cannot be copied are \"No Ability\", As One, Battle Bond, Comatose, Disguise, Flower Gift, Forecast, Gulp Missile, Hunger Switch, Ice Face, Illusion, Imposter, Multitype, Neutralizing Gas, Power Construct, Power of Alchemy, Receiver, RKS System, Schooling, Shields Down, Stance Change, Trace, Wonder Guard, and Zen Mode.",
                    ShortDescription = "This Pokemon copies the Ability of an ally that faints.",
                    Rating = 0f,
                    onAllyFaint = "onAllyFaint(target) {\n			if (!this.effectState.target.hp) return;\n			const ability = target.getAbility();\n			const additionalBannedAbilities = [\n				'noability', 'flowergift', 'forecast', 'hungerswitch', 'illusion', 'imposter', 'neutralizinggas', 'powerofalchemy', 'receiver', 'trace', 'wonderguard',\n			];\n			if (target.getAbility().isPermanent || additionalBannedAbilities.includes(target.ability)) return;\n			this.add('-ability', this.effectState.target, ability, '[from] ability: Receiver', '[of] ' + target);\n			this.effectState.target.setAbility(ability);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.PowerOfAlchemy,
                    Name = "Power of Alchemy",
                    Description = "This Pokemon copies the Ability of an ally that faints. Abilities that cannot be copied are \"No Ability\", As One, Battle Bond, Comatose, Disguise, Flower Gift, Forecast, Gulp Missile, Hunger Switch, Ice Face, Illusion, Imposter, Multitype, Neutralizing Gas, Power Construct, Power of Alchemy, Receiver, RKS System, Schooling, Shields Down, Stance Change, Trace, Wonder Guard, and Zen Mode.",
                    ShortDescription = "This Pokemon copies the Ability of an ally that faints.",
                    Rating = 0f,
                    onAllyFaint = "onAllyFaint(target) {\n			if (!this.effectState.target.hp) return;\n			const ability = target.getAbility();\n			const additionalBannedAbilities = [\n				'noability', 'flowergift', 'forecast', 'hungerswitch', 'illusion', 'imposter', 'neutralizinggas', 'powerofalchemy', 'receiver', 'trace', 'wonderguard',\n			];\n			if (target.getAbility().isPermanent || additionalBannedAbilities.includes(target.ability)) return;\n			this.add('-ability', this.effectState.target, ability, '[from] ability: Power of Alchemy', '[of] ' + target);\n			this.effectState.target.setAbility(ability);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.BeastBoost,
                    Name = "Beast Boost",
                    Description = "This Pokemon's highest stat is raised by 1 stage if it attacks and knocks out another Pokemon.",
                    ShortDescription = "This Pokemon's highest stat is raised by 1 if it attacks and KOes another Pokemon.",
                    Rating = 3.5f,
                    onSourceAfterFaint = "onSourceAfterFaint(length, target, source, effect) {\n			if (effect && effect.effectType === 'Move') {\n				let statName = 'atk';\n				let bestStat = 0;\n				let s;\n				for (s in source.storedStats) {\n					if (source.storedStats[s] > bestStat) {\n						statName = s;\n						bestStat = source.storedStats[s];\n					}\n				}\n				this.boost({[statName]: length}, source);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.RKSSystem,
                    Name = "RKS System",
                    Description = "",
                    ShortDescription = "If this Pokemon is a Silvally, its type changes to match its held Memory.",
                    Rating = 4f,
                    isPermanent = true,
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.ElectricSurge,
                    Name = "Electric Surge",
                    Description = "",
                    ShortDescription = "On switch-in, this Pokemon summons Electric Terrain.",
                    Rating = 4f,
                    onStart = "onStart(source) {\n			this.field.setTerrain('electricterrain');\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.PsychicSurge,
                    Name = "Psychic Surge",
                    Description = "",
                    ShortDescription = "On switch-in, this Pokemon summons Psychic Terrain.",
                    Rating = 4f,
                    onStart = "onStart(source) {\n			this.field.setTerrain('psychicterrain');\n		}",
                },
                new Ability(service)
                {
                    Id = AbilityEnum.MistySurge,
                    Name = "Misty Surge",
                    Description = "",
                    ShortDescription = "On switch-in, this Pokemon summons Misty Terrain.",
                    Rating = 3.5f,
                    onStart = "onStart(source) {\n			this.field.setTerrain('mistyterrain');\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.GrassySurge,
                    Name = "Grassy Surge",
                    Description = "",
                    ShortDescription = "On switch-in, this Pokemon summons Grassy Terrain.",
                    Rating = 4f,
                    onStart = "onStart(source) {\n			this.field.setTerrain('grassyterrain');\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.FullMetalBody,
                    Name = "Full Metal Body",
                    Description = "Prevents other Pokemon from lowering this Pokemon's stat stages. Moongeist Beam, Sunsteel Strike, and the Mold Breaker, Teravolt, and Turboblaze Abilities cannot ignore this Ability.",
                    ShortDescription = "Prevents other Pokemon from lowering this Pokemon's stat stages.",
                    Rating = 2f,
                    onBoost = "onBoost(boost, target, source, effect) {\n			if (source && target === source) return;\n			let showMsg = false;\n			let i;\n			for (i in boost) {\n				if (boost[i] < 0) {\n					delete boost[i];\n					showMsg = true;\n				}\n			}\n			if (showMsg && !(effect ).secondaries && effect.id !== 'octolock') {\n				this.add(\"-fail\", target, \"unboost\", \"[from] ability: Full Metal Body\", \"[of] \" + target);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.ShadowShield,
                    Name = "Shadow Shield",
                    Description = "If this Pokemon is at full HP, damage taken from attacks is halved. Moongeist Beam, Sunsteel Strike, and the Mold Breaker, Teravolt, and Turboblaze Abilities cannot ignore this Ability.",
                    ShortDescription = "If this Pokemon is at full HP, damage taken from attacks is halved.",
                    Rating = 3.5f,
                    onSourceModifyDamage = "onSourceModifyDamage(damage, source, target, move) {\n			if (target.hp >= target.maxhp) {\n				this.debug('Shadow Shield weaken');\n				return this.chainModify(0.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.PrismArmor,
                    Name = "Prism Armor",
                    Description = "This Pokemon receives 3/4 damage from supereffective attacks. Moongeist Beam, Sunsteel Strike, and the Mold Breaker, Teravolt, and Turboblaze Abilities cannot ignore this Ability.",
                    ShortDescription = "This Pokemon receives 3/4 damage from supereffective attacks.",
                    Rating = 3f,
                    onSourceModifyDamage = "onSourceModifyDamage(damage, source, target, move) {\n			if (target.getMoveHitData(move).typeMod > 0) {\n				this.debug('Prism Armor neutralize');\n				return this.chainModify(0.75);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Neuroforce,
                    Name = "Neuroforce",
                    Description = "",
                    ShortDescription = "This Pokemon's attacks that are super effective against the target do 1.25x damage.",
                    Rating = 2.5f,
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.IntrepidSword,
                    Name = "Intrepid Sword",
                    Description = "",
                    ShortDescription = "On switch-in, this Pokemon's Attack is raised by 1 stage.",
                    Rating = 4f,
                    onStart = "onStart(pokemon) {\n			this.boost({atk: 1}, pokemon);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.DauntlessShield,
                    Name = "Dauntless Shield",
                    Description = "",
                    ShortDescription = "On switch-in, this Pokemon's Defense is raised by 1 stage.",
                    Rating = 3.5f,
                    onStart = "onStart(pokemon) {\n			this.boost({def: 1}, pokemon);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Libero,
                    Name = "Libero",
                    Description = "This Pokemon's type changes to match the type of the move it is about to use. This effect comes after all effects that change a move's type.",
                    ShortDescription = "This Pokemon's type changes to match the type of the move it is about to use.",
                    Rating = 4.5f,
                    onPrepareHit = "onPrepareHit(source, target, move) {\n			if (move.hasBounced || move.sourceEffect === 'snatch') return;\n			const type = move.type;\n			if (type && type !== '???' && source.getTypes().join() !== type) {\n				if (!source.setType(type)) return;\n				this.add('-start', source, 'typechange', type, '[from] ability: Libero');\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.BallFetch,
                    Name = "Ball Fetch",
                    Description = "",
                    ShortDescription = "No competitive use.",
                    Rating = 0f,
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.CottonDown,
                    Name = "Cotton Down",
                    Description = "When this Pokemon is hit by an attack, the Speed of all other Pokemon on the field is lowered by 1 stage.",
                    ShortDescription = "If this Pokemon is hit, it lowers the Speed of all other Pokemon on the field 1 stage.",
                    Rating = 2f,
                    onDamagingHit = "onDamagingHit(damage, target, source, move) {\n			let activated = false;\n			for (const pokemon of this.getAllActive()) {\n				if (pokemon === target || pokemon.fainted) continue;\n				if (!activated) {\n					this.add('-ability', target, 'Cotton Down');\n					activated = true;\n				}\n				this.boost({spe: -1}, pokemon, target, null, true);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.PropellerTail,
                    Name = "Propeller Tail",
                    Description = "",
                    ShortDescription = "This Pokemon's moves cannot be redirected to a different target by any effect.",
                    Rating = 0f,
                    onModifyMovePriority = 1,
                    onModifyMove = "onModifyMove(move) {\n			// most of the implementation is in Battle#getTarget\n			move.tracksTarget = move.target !== 'scripted';\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.MirrorArmor,
                    Name = "Mirror Armor",
                    Description = "When one of this Pokemon's stat stages would be lowered by another Pokemon, that Pokemon's stat stage is lowered instead. This effect does not happen if this Pokemon's stat stage was already -6.",
                    ShortDescription = "If this Pokemon's stat stages would be lowered, the attacker's are lowered instead.",
                    Rating = 2f,
                    isBreakable = true,
                    onBoost = "onBoost(boost, target, source, effect) {\n			// Don't bounce self stat changes, or boosts that have already bounced\n			if (target === source || !boost || effect.id === 'mirrorarmor') return;\n			let b;\n			for (b in boost) {\n				if (boost[b] < 0) {\n					if (target.boosts[b] === -6) continue;\n					const negativeBoost = {};\n					negativeBoost[b] = boost[b];\n					delete boost[b];\n					this.add('-ability', target, 'Mirror Armor');\n					this.boost(negativeBoost, source, target, null, true);\n				}\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.GulpMissile,
                    Name = "Gulp Missile",
                    Description = "If this Pokemon is a Cramorant, it changes forme when it hits a target with Surf or uses the first turn of Dive successfully. It becomes Gulping Form with an Arrokuda in its mouth if it has more than 1/2 of its maximum HP remaining, or Gorging Form with a Pikachu in its mouth if it has 1/2 or less of its maximum HP remaining. If Cramorant gets hit in Gulping or Gorging Form, it spits the Arrokuda or Pikachu at its attacker, even if it has no HP remaining. The projectile deals damage equal to 1/4 of the target's maximum HP, rounded down; this damage is blocked by the Magic Guard Ability but not by a substitute. An Arrokuda also lowers the target's Defense by 1 stage, and a Pikachu paralyzes the target. Cramorant will return to normal if it spits out a projectile, switches out, or Dynamaxes.",
                    ShortDescription = "When hit after Surf/Dive, attacker takes 1/4 max HP and -1 Defense or paralysis.",
                    Rating = 2.5f,
                    isPermanent = true,
                    onDamagingHit = "onDamagingHit(damage, target, source, move) {\n			if (!source.hp || !source.isActive || target.transformed || target.isSemiInvulnerable()) return;\n			if (['cramorantgulping', 'cramorantgorging'].includes(target.species.id)) {\n				this.damage(source.baseMaxhp / 4, source, target);\n				if (target.species.id === 'cramorantgulping') {\n					this.boost({def: -1}, source, target, null, true);\n				} else {\n					source.trySetStatus('par', target, move);\n				}\n				target.formeChange('cramorant', move);\n			}\n		}",
                    onSourceTryPrimaryHit = "onSourceTryPrimaryHit(target, source, effect) {\n			if (\n				effect && effect.id === 'surf' && source.hasAbility('gulpmissile') &&\n				source.species.name === 'Cramorant' && !source.transformed\n			) {\n				const forme = source.hp <= source.maxhp / 2 ? 'cramorantgorging' : 'cramorantgulping';\n				source.formeChange(forme, effect);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Stalwart,
                    Name = "Stalwart",
                    Description = "",
                    ShortDescription = "This Pokemon's moves cannot be redirected to a different target by any effect.",
                    Rating = 0f,
                    onModifyMovePriority = 1,
                    onModifyMove = "onModifyMove(move) {\n			// most of the implementation is in Battle#getTarget\n			move.tracksTarget = move.target !== 'scripted';\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.SteamEngine,
                    Name = "Steam Engine",
                    Description = "",
                    ShortDescription = "This Pokemon's Speed is raised by 6 stages after it is damaged by Fire/Water moves.",
                    Rating = 2f,
                    onDamagingHit = "onDamagingHit(damage, target, source, move) {\n			if (['Water', 'Fire'].includes(move.type)) {\n				this.boost({spe: 6});\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.PunkRock,
                    Name = "Punk Rock",
                    Description = "This Pokemon's sound-based moves have their power multiplied by 1.3. This Pokemon takes halved damage from sound-based moves.",
                    ShortDescription = "This Pokemon receives 1/2 damage from sound moves. Its own have 1.3x power.",
                    Rating = 3.5f,
                    isBreakable = true,
                    onBasePower = "onBasePower(basePower, attacker, defender, move) {\n			if (move.flags['sound']) {\n				this.debug('Punk Rock boost');\n				return this.chainModify([5325, 4096]);\n			}\n		}",
                    onSourceModifyDamage = "onSourceModifyDamage(damage, source, target, move) {\n			if (move.flags['sound']) {\n				this.debug('Punk Rock weaken');\n				return this.chainModify(0.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.SandSpit,
                    Name = "Sand Spit",
                    Description = "When this Pokemon is hit by an attack, Sandstorm begins. This effect happens after the effects of Max and G-Max Moves.",
                    ShortDescription = "When this Pokemon is hit, Sandstorm begins.",
                    Rating = 2f,
                    onDamagingHit = "onDamagingHit(damage, target, source, move) {\n			if (this.field.getWeather().id !== 'sandstorm') {\n				this.field.setWeather('sandstorm');\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.IceScales,
                    Name = "Ice Scales",
                    Description = "",
                    ShortDescription = "This Pokemon receives 1/2 damage from special attacks.",
                    Rating = 4f,
                    isBreakable = true,
                    onSourceModifyDamage = "onSourceModifyDamage(damage, source, target, move) {\n			if (move.category === 'Special') {\n				return this.chainModify(0.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Ripen,
                    Name = "Ripen",
                    Description = "",
                    ShortDescription = "When this Pokemon eats a Berry, its effect is doubled.",
                    Rating = 2f,
                    onTryEatItemPriority = -1,
                    onSourceModifyDamagePriority = -1,
                    onTryEatItem = "onTryEatItem(item, pokemon) {\n			this.add('-activate', pokemon, 'ability: Ripen');\n		}",
                    onBoost = "onBoost(boost, target, source, effect) {\n			if (effect && (effect ).isBerry) {\n				let b;\n				for (b in boost) {\n					boost[b] *= 2;\n				}\n			}\n		}",
                    onEatItem = "onEatItem(item, pokemon) {\n			const weakenBerries = [\n				'Babiri Berry', 'Charti Berry', 'Chilan Berry', 'Chople Berry', 'Coba Berry', 'Colbur Berry', 'Haban Berry', 'Kasib Berry', 'Kebia Berry', 'Occa Berry', 'Passho Berry', 'Payapa Berry', 'Rindo Berry', 'Roseli Berry', 'Shuca Berry', 'Tanga Berry', 'Wacan Berry', 'Yache Berry',\n			];\n			// Record if the pokemon ate a berry to resist the attack\n			pokemon.abilityState.berryWeaken = weakenBerries.includes(item.name);\n		}",
                    onSourceModifyDamage = "onSourceModifyDamage(damage, source, target, move) {\n			if (target.abilityState.berryWeaken) {\n				target.abilityState.berryWeaken = false;\n				return this.chainModify(0.5);\n			}\n		}",
                    onTryHeal = "onTryHeal(damage, target, source, effect) {\n			if (!effect) return;\n			if (effect.id === 'berryjuice' || effect.id === 'leftovers') {\n				this.add('-activate', target, 'ability: Ripen');\n			}\n			if ((effect ).isBerry) return this.chainModify(2);\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.IceFace,
                    Name = "Ice Face",
                    Description = "If this Pokemon is an Eiscue, the first physical hit it takes in battle deals 0 neutral damage. Its ice face is then broken and it changes forme to Noice Face. Eiscue regains its Ice Face forme when Hail begins or when Eiscue switches in while Hail is active. Confusion damage also breaks the ice face.",
                    ShortDescription = "If Eiscue, the first physical hit it takes deals 0 damage. This effect is restored in Hail.",
                    Rating = 3f,
                    isBreakable = true,
                    isPermanent = true,
                    onDamagePriority = 1,
                    onStart = "onStart(pokemon) {\n			if (this.field.isWeather('hail') && pokemon.species.id === 'eiscuenoice' && !pokemon.transformed) {\n				this.add('-activate', pokemon, 'ability: Ice Face');\n				this.effectState.busted = false;\n				pokemon.formeChange('Eiscue', this.effect, true);\n			}\n		}",
                    onCriticalHit = "onCriticalHit(target, type, move) {\n			if (!target) return;\n			if (move.category !== 'Physical' || target.species.id !== 'eiscue' || target.transformed) return;\n			if (target.volatiles['substitute'] && !(move.flags['authentic'] || move.infiltrates)) return;\n			if (!target.runImmunity(move.type)) return;\n			return false;\n		}",
                    onDamage = "onDamage(damage, target, source, effect) {\n			if (\n				effect && effect.effectType === 'Move' && effect.category === 'Physical' &&\n				target.species.id === 'eiscue' && !target.transformed\n			) {\n				this.add('-activate', target, 'ability: Ice Face');\n				this.effectState.busted = true;\n				return 0;\n			}\n		}",
                    onEffectiveness = "onEffectiveness(typeMod, target, type, move) {\n			if (!target) return;\n			if (move.category !== 'Physical' || target.species.id !== 'eiscue' || target.transformed) return;\n\n			const hitSub = target.volatiles['substitute'] && !move.flags['authentic'] && !(move.infiltrates && this.gen >= 6);\n			if (hitSub) return;\n\n			if (!target.runImmunity(move.type)) return;\n			return 0;\n		}",
                    onUpdate = "onUpdate(pokemon) {\n			if (pokemon.species.id === 'eiscue' && this.effectState.busted) {\n				pokemon.formeChange('Eiscue-Noice', this.effect, true);\n			}\n		}",
                    onAnyWeatherStart = "onAnyWeatherStart() {\n			const pokemon = this.effectState.target;\n			if (!pokemon.hp) return;\n			if (this.field.isWeather('hail') && pokemon.species.id === 'eiscuenoice' && !pokemon.transformed) {\n				this.add('-activate', pokemon, 'ability: Ice Face');\n				this.effectState.busted = false;\n				pokemon.formeChange('Eiscue', this.effect, true);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.PowerSpot,
                    Name = "Power Spot",
                    Description = "",
                    ShortDescription = "This Pokemon's allies have the power of their moves multiplied by 1.3.",
                    Rating = 1f,
                    onAllyBasePowerPriority = 22,
                    onAllyBasePower = "onAllyBasePower(basePower, attacker, defender, move) {\n			if (attacker !== this.effectState.target) {\n				this.debug('Power Spot boost');\n				return this.chainModify([5325, 4096]);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Mimicry,
                    Name = "Mimicry",
                    Description = "",
                    ShortDescription = "This Pokemon's type changes to match the Terrain. Type reverts when Terrain ends.",
                    Rating = 0.5f,
                    onStart = "onStart(pokemon) {\n			if (this.field.terrain) {\n				pokemon.addVolatile('mimicry');\n			} else {\n				const types = pokemon.baseSpecies.types;\n				if (pokemon.getTypes().join() === types.join() || !pokemon.setType(types)) return;\n				this.add('-start', pokemon, 'typechange', types.join('/'), '[from] ability: Mimicry');\n				this.hint(\"Transform Mimicry changes you to your original un-transformed types.\");\n			}\n		}",
                    onEnd = "onEnd(pokemon) {\n			delete pokemon.volatiles['mimicry'];\n		}",
                    onAnyTerrainStart = "onAnyTerrainStart() {\n			const pokemon = this.effectState.target;\n			delete pokemon.volatiles['mimicry'];\n			pokemon.addVolatile('mimicry');\n		}",
                    condition = "[object Object]",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.ScreenCleaner,
                    Name = "Screen Cleaner",
                    Description = "",
                    ShortDescription = "On switch-in, the effects of Aurora Veil, Light Screen, and Reflect end for both sides.",
                    Rating = 2f,
                    onStart = "onStart(pokemon) {\n			let activated = false;\n			for (const sideCondition of ['reflect', 'lightscreen', 'auroraveil']) {\n				for (const side of [pokemon.side, ...pokemon.side.foeSidesWithConditions()]) {\n					if (side.getSideCondition(sideCondition)) {\n						if (!activated) {\n							this.add('-activate', pokemon, 'ability: Screen Cleaner');\n							activated = true;\n						}\n						side.removeSideCondition(sideCondition);\n					}\n				}\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.SteelySpirit,
                    Name = "Steely Spirit",
                    Description = "",
                    ShortDescription = "This Pokemon and its allies' Steel-type moves have their power multiplied by 1.5.",
                    Rating = 3.5f,
                    onAllyBasePowerPriority = 22,
                    onAllyBasePower = "onAllyBasePower(basePower, attacker, defender, move) {\n			if (move.type === 'Steel') {\n				this.debug('Steely Spirit boost');\n				return this.chainModify(1.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.PerishBody,
                    Name = "Perish Body",
                    Description = "Making contact with this Pokemon starts the Perish Song effect for it and the attacker. This effect does not happen if this Pokemon already has a perish count.",
                    ShortDescription = "Making contact with this Pokemon starts the Perish Song effect for it and the attacker.",
                    Rating = 1f,
                    onDamagingHit = "onDamagingHit(damage, target, source, move) {\n			if (!this.checkMoveMakesContact(move, source, target)) return;\n\n			let announced = false;\n			for (const pokemon of [target, source]) {\n				if (pokemon.volatiles['perishsong']) continue;\n				if (!announced) {\n					this.add('-ability', target, 'Perish Body');\n					announced = true;\n				}\n				pokemon.addVolatile('perishsong');\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.WanderingSpirit,
                    Name = "Wandering Spirit",
                    Description = "Pokemon making contact with this Pokemon have their Ability swapped with this one. Does not affect a Pokemon which has the Ability As One, Battle Bond, Comatose, Disguise, Gulp Missile, Hunger Switch, Ice Face, Illusion, Multitype, Neutralizing Gas, Power Construct, RKS System, Schooling, Shields Down, Stance Change, or Zen Mode.",
                    ShortDescription = "Pokemon making contact with this Pokemon have their Ability swapped with this one.",
                    Rating = 2.5f,
                    onDamagingHit = "onDamagingHit(damage, target, source, move) {\n			const additionalBannedAbilities = ['hungerswitch', 'illusion', 'neutralizinggas', 'wonderguard'];\n			if (source.getAbility().isPermanent || additionalBannedAbilities.includes(source.ability) ||\n				target.volatiles['dynamax']\n			) {\n				return;\n			}\n\n			if (this.checkMoveMakesContact(move, source, target)) {\n				const sourceAbility = source.setAbility('wanderingspirit', target);\n				if (!sourceAbility) return;\n				if (target.isAlly(source)) {\n					this.add('-activate', target, 'Skill Swap', '', '', '[of] ' + source);\n				} else {\n					this.add('-activate', target, 'ability: Wandering Spirit', this.dex.abilities.get(sourceAbility).name, 'Wandering Spirit', '[of] ' + source);\n				}\n				target.setAbility(sourceAbility);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.GorillaTactics,
                    Name = "Gorilla Tactics",
                    Description = "",
                    ShortDescription = "This Pokemon's Attack is 1.5x, but it can only select the first move it executes.",
                    Rating = 4.5f,
                    onModifyAtkPriority = 1,
                    onModifyMove = "onModifyMove(move, pokemon) {\n			if (pokemon.abilityState.choiceLock || move.isZOrMaxPowered || move.id === 'struggle') return;\n			pokemon.abilityState.choiceLock = move.id;\n		}",
                    onStart = "onStart(pokemon) {\n			pokemon.abilityState.choiceLock = \"\";\n		}",
                    onEnd = "onEnd(pokemon) {\n			pokemon.abilityState.choiceLock = \"\";\n		}",
                    onModifyAtk = "onModifyAtk(atk, pokemon) {\n			if (pokemon.volatiles['dynamax']) return;\n			// PLACEHOLDER\n			this.debug('Gorilla Tactics Atk Boost');\n			return this.chainModify(1.5);\n		}",
                    onBeforeMove = "onBeforeMove(pokemon, target, move) {\n			if (move.isZOrMaxPowered || move.id === 'struggle') return;\n			if (pokemon.abilityState.choiceLock && pokemon.abilityState.choiceLock !== move.id) {\n				// Fails unless ability is being ignored (these events will not run), no PP lost.\n				this.addMove('move', pokemon, move.name);\n				this.attrLastMove('[still]');\n				this.debug(\"Disabled by Gorilla Tactics\");\n				this.add('-fail', pokemon);\n				return false;\n			}\n		}",
                    onDisableMove = "onDisableMove(pokemon) {\n			if (!pokemon.abilityState.choiceLock) return;\n			if (pokemon.volatiles['dynamax']) return;\n			for (const moveSlot of pokemon.moveSlots) {\n				if (moveSlot.id !== pokemon.abilityState.choiceLock) {\n					pokemon.disableMove(moveSlot.id, false, this.effectState.sourceEffect);\n				}\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.NeutralizingGas,
                    Name = "Neutralizing Gas",
                    Description = "While this Pokemon is active, Abilities have no effect. Does not affect the Abilities As One, Battle Bond, Comatose, Disguise, Gulp Missile, Ice Face, Multitype, Power Construct, RKS System, Schooling, Shields Down, Stance Change, or Zen Mode.",
                    ShortDescription = "While this Pokemon is active, Abilities have no effect.",
                    Rating = 4f,
                    onPreStart = "onPreStart(pokemon) {\n			this.add('-ability', pokemon, 'Neutralizing Gas');\n			pokemon.abilityState.ending = false;\n			for (const target of this.getAllActive()) {\n				if (target.illusion) {\n					this.singleEvent('End', this.dex.abilities.get('Illusion'), target.abilityState, target, pokemon, 'neutralizinggas');\n				}\n				if (target.volatiles['slowstart']) {\n					delete target.volatiles['slowstart'];\n					this.add('-end', target, 'Slow Start', '[silent]');\n				}\n			}\n		}",
                    onEnd = "onEnd(source) {\n			for (const pokemon of this.getAllActive()) {\n				if (pokemon !== source && pokemon.hasAbility('Neutralizing Gas')) {\n					return;\n				}\n			}\n			this.add('-end', source, 'ability: Neutralizing Gas');\n\n			// FIXME this happens before the pokemon switches out, should be the opposite order.\n			// Not an easy fix since we cant use a supported event. Would need some kind of special event that\n			// gathers events to run after the switch and then runs them when the ability is no longer accessible.\n			// (If you're tackling this, do note extreme weathers have the same issue)\n\n			// Mark this pokemon's ability as ending so Pokemon#ignoringAbility skips it\n			if (source.abilityState.ending) return;\n			source.abilityState.ending = true;\n			const sortedActive = this.getAllActive();\n			this.speedSort(sortedActive);\n			for (const pokemon of sortedActive) {\n				if (pokemon !== source) {\n					// Will be suppressed by Pokemon#ignoringAbility if needed\n					this.singleEvent('Start', pokemon.getAbility(), pokemon.abilityState, pokemon);\n				}\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.PastelVeil,
                    Name = "Pastel Veil",
                    Description = "",
                    ShortDescription = "This Pokemon and its allies cannot be poisoned. On switch-in, cures poisoned allies.",
                    Rating = 2f,
                    isBreakable = true,
                    onStart = "onStart(pokemon) {\n			for (const ally of pokemon.alliesAndSelf()) {\n				if (['psn', 'tox'].includes(ally.status)) {\n					this.add('-activate', pokemon, 'ability: Pastel Veil');\n					ally.cureStatus();\n				}\n			}\n		}",
                    onSetStatus = "onSetStatus(status, target, source, effect) {\n			if (!['psn', 'tox'].includes(status.id)) return;\n			if (_optionalChain([(effect ), 'optionalAccess', _11 => _11.status])) {\n				this.add('-immune', target, '[from] ability: Pastel Veil');\n			}\n			return false;\n		}",
                    onUpdate = "onUpdate(pokemon) {\n			if (['psn', 'tox'].includes(pokemon.status)) {\n				this.add('-activate', pokemon, 'ability: Pastel Veil');\n				pokemon.cureStatus();\n			}\n		}",
                    onAllySetStatus = "onAllySetStatus(status, target, source, effect) {\n			if (!['psn', 'tox'].includes(status.id)) return;\n			if (_optionalChain([(effect ), 'optionalAccess', _12 => _12.status])) {\n				const effectHolder = this.effectState.target;\n				this.add('-block', target, 'ability: Pastel Veil', '[of] ' + effectHolder);\n			}\n			return false;\n		}",
                    onAllySwitchIn = "onAllySwitchIn(pokemon) {\n			if (['psn', 'tox'].includes(pokemon.status)) {\n				this.add('-activate', this.effectState.target, 'ability: Pastel Veil');\n				pokemon.cureStatus();\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.HungerSwitch,
                    Name = "Hunger Switch",
                    Description = "",
                    ShortDescription = "If Morpeko, it changes between Full Belly and Hangry Mode at the end of each turn.",
                    Rating = 1f,
                    onResidualOrder = 29,
                    onResidual = "onResidual(pokemon) {\n			if (pokemon.species.baseSpecies !== 'Morpeko' || pokemon.transformed) return;\n			const targetForme = pokemon.species.name === 'Morpeko' ? 'Morpeko-Hangry' : 'Morpeko';\n			pokemon.formeChange(targetForme);\n		}",
                },
                new Ability(service)
                {
                    Id = AbilityEnum.QuickDraw,
                    Name = "Quick Draw",
                    Description = "",
                    ShortDescription = "This Pokemon has a 30% chance to move first in its priority bracket with attacking moves.",
                    Rating = 2.5f,
                    onFractionalPriorityPriority = -1,
                    onFractionalPriority = "onFractionalPriority(priority, pokemon, target, move) {\n			if (move.category !== \"Status\" && this.randomChance(3, 10)) {\n				this.add('-activate', pokemon, 'ability: Quick Draw');\n				return 0.1;\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.UnseenFist,
                    Name = "Unseen Fist",
                    Description = "All of this Pokemon's moves that make contact bypass protection.",
                    ShortDescription = "All contact moves hit through protection.",
                    Rating = 2f,
                    onModifyMove = "onModifyMove(move) {\n			if (move.flags['contact']) delete move.flags['protect'];\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.CuriousMedicine,
                    Name = "Curious Medicine",
                    Description = "",
                    ShortDescription = "On switch-in, this Pokemon's allies have their stat stages reset to 0.",
                    Rating = 0f,
                    onStart = "onStart(pokemon) {\n			for (const ally of pokemon.adjacentAllies()) {\n				ally.clearBoosts();\n				this.add('-clearboost', ally, '[from] ability: Curious Medicine', '[of] ' + pokemon);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.Transistor,
                    Name = "Transistor",
                    Description = "",
                    ShortDescription = "This Pokemon's attacking stat is multiplied by 1.5 while using an Electric-type attack.",
                    Rating = 3.5f,
                    onModifyAtkPriority = 5,
                    onModifySpAPriority = 5,
                    onModifyAtk = "onModifyAtk(atk, attacker, defender, move) {\n			if (move.type === 'Electric') {\n				this.debug('Transistor boost');\n				return this.chainModify(1.5);\n			}\n		}",
                    onModifySpA = "onModifySpA(atk, attacker, defender, move) {\n			if (move.type === 'Electric') {\n				this.debug('Transistor boost');\n				return this.chainModify(1.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.DragonsMaw,
                    Name = "Dragon's Maw",
                    Description = "",
                    ShortDescription = "This Pokemon's attacking stat is multiplied by 1.5 while using a Dragon-type attack.",
                    Rating = 3.5f,
                    onModifyAtkPriority = 5,
                    onModifySpAPriority = 5,
                    onModifyAtk = "onModifyAtk(atk, attacker, defender, move) {\n			if (move.type === 'Dragon') {\n				this.debug('Dragon\'s Maw boost');\n				return this.chainModify(1.5);\n			}\n		}",
                    onModifySpA = "onModifySpA(atk, attacker, defender, move) {\n			if (move.type === 'Dragon') {\n				this.debug('Dragon\'s Maw boost');\n				return this.chainModify(1.5);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.ChillingNeigh,
                    Name = "Chilling Neigh",
                    Description = "This Pokemon's Attack is raised by 1 stage if it attacks and knocks out another Pokemon.",
                    ShortDescription = "This Pokemon's Attack is raised by 1 stage if it attacks and KOes another Pokemon.",
                    Rating = 3f,
                    onSourceAfterFaint = "onSourceAfterFaint(length, target, source, effect) {\n			if (effect && effect.effectType === 'Move') {\n				this.boost({atk: length}, source);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.GrimNeigh,
                    Name = "Grim Neigh",
                    Description = "This Pokemon's Special Attack is raised by 1 stage if it attacks and knocks out another Pokemon.",
                    ShortDescription = "This Pokemon's Sp. Atk is raised by 1 stage if it attacks and KOes another Pokemon.",
                    Rating = 3f,
                    onSourceAfterFaint = "onSourceAfterFaint(length, target, source, effect) {\n			if (effect && effect.effectType === 'Move') {\n				this.boost({spa: length}, source);\n			}\n		}",
                    },
                new Ability(service)
                {
                    Id = AbilityEnum.AsOneGlastrier,
                    Name = "As One (Glastrier)",
                    Description = "",
                    ShortDescription = "The combination of Unnerve and Chilling Neigh.",
                    Rating = 3.5f,
                    isPermanent = true,
                    onPreStart = "onPreStart(pokemon) {\n			this.add('-ability', pokemon, 'As One');\n			this.add('-ability', pokemon, 'Unnerve');\n			this.effectState.unnerved = true;\n		}",
                    onEnd = "onEnd() {\n			this.effectState.unnerved = false;\n		}",
                    onFoeTryEatItem = "onFoeTryEatItem() {\n			return !this.effectState.unnerved;\n		}",
                    onSourceAfterFaint = "onSourceAfterFaint(length, target, source, effect) {\n			if (effect && effect.effectType === 'Move') {\n				this.boost({atk: length}, source, source, this.dex.abilities.get('chillingneigh'));\n			}\n		}",
                },
                new Ability(service)
                {
                    Id = AbilityEnum.AsOneSpectrier,
                    Name = "As One (Spectrier)",
                    Description = "",
                    ShortDescription = "The combination of Unnerve and Grim Neigh.",
                    Rating = 3.5f,
                    isPermanent = true,
                    onPreStart = "onPreStart(pokemon) {\n			this.add('-ability', pokemon, 'As One');\n			this.add('-ability', pokemon, 'Unnerve');\n			this.effectState.unnerved = true;\n		}",
                    onEnd = "onEnd() {\n			this.effectState.unnerved = false;\n		}",
                    onFoeTryEatItem = "onFoeTryEatItem() {\n			return !this.effectState.unnerved;\n		}",
                    onSourceAfterFaint = "onSourceAfterFaint(length, target, source, effect) {\n			if (effect && effect.effectType === 'Move') {\n				this.boost({spa: length}, source, source, this.dex.abilities.get('grimneigh'));\n			}\n		}",
                },
            };
        }
    }
}
