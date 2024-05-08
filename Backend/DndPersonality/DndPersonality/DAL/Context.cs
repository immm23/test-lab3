using DndPersonality.Models.DALModels;
using DndPersonality.Models.DALModels.Components;
using DndPersonality.Models.DALModels.Subrace;
using DndPersonality.Models.DALModels.System;
using Microsoft.EntityFrameworkCore;

namespace DndPersonality.DAL
{
    public class Context : DbContext
    {
        public DbSet<Race> Races { get; set; }
        public DbSet<Name> Names { get; set; }
        public DbSet<RaceBonus> RaceBonuses { get; set; }
        public DbSet<Ability> Abilities { get; set; }
        public DbSet<Alignment> Alignments { get; set; }
        public DbSet<AlignmentSide> AlignmentSides { get; set; }
        public DbSet<AlignmentStrength> AlignmentStrengths { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Subrace> Subraces { get; set; }
        public DbSet<WeaponType> WeaponTypes { get; set; }
        public DbSet<ArmorType> ArmorTypes { get; set; }
        public DbSet<HitPoints> HitPoints { get; set; }
        public DbSet<Tool> Tools { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<WeaponSelector> WeaponsSelectors { get; set; }
        public DbSet<CharacterClass> Classes { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<Armor> Armors { get; set; }
        public DbSet<DamageType> DamageTypes { get; set; }

        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlignmentSide>(alignment =>
            {
                alignment.Property(p => p.AlignmentSideId)
                    .HasConversion<int>();
                alignment.HasData(
                    Enum.GetValues(typeof(AlignmentSideId))
                    .Cast<AlignmentSideId>()
                    .Select(p => new AlignmentSide()
                    {
                        AlignmentSideId = p,
                        Name = p.ToString()
                    })
                );
            });

            modelBuilder.Entity<AlignmentStrength>(alignmentStrength =>
            {
                alignmentStrength.Property(p => p.AlignmentStrengthId)
                    .HasConversion<int>();
                alignmentStrength.HasData(
                    Enum.GetValues(typeof(AlignmentStrengthId))
                    .Cast<AlignmentStrengthId>()
                    .Select(p => new AlignmentStrength()
                    {
                        AlignmentStrengthId = p,
                        Name = p.ToString()
                    })
                );
            });

            modelBuilder.Entity<WeaponType>(weaponType =>
            {
                weaponType.Property(p => p.WeaponTypeId)
                    .HasConversion<int>();
                weaponType.HasData(
                    Enum.GetValues(typeof(WeaponTypeId))
                    .Cast<WeaponTypeId>()
                    .Select(p => new WeaponType()
                    {
                        WeaponTypeId = p,
                        Name = p.ToString()
                    })
                );
            });

            modelBuilder.Entity<ArmorType>(armorType =>
            {
                armorType.Property(p => p.ArmorTypeId)
                    .HasConversion<int>();
                armorType.HasData(
                    Enum.GetValues(typeof(ArmorTypeId))
                    .Cast<ArmorTypeId>()
                    .Select(p => new ArmorType()
                    {
                        ArmorTypeId = p,
                        Name = p.ToString()
                    })
                );
            });

            modelBuilder.Entity<DamageType>(damageType =>
            {
                damageType.Property(p => p.DamageTypeId)
                    .HasConversion<int>();
                damageType.HasData(
                    Enum.GetValues(typeof(DamageTypeId))
                    .Cast<DamageTypeId>()
                    .Select(p => new DamageType()
                    {
                        DamageTypeId = p,
                        Name = p.ToString()
                    })
                );
            });

            modelBuilder.Entity<Ability>(ability =>
            {
                ability.Property(p => p.AbilityId)
                    .HasConversion<int>();
                ability.HasData(
                    Enum.GetValues(typeof(AbilityId))
                    .Cast<AbilityId>()
                    .Select(p => new Ability()
                    {
                        AbilityId = p,
                        Name = p.ToString()
                    })
                );
            });

            modelBuilder.Entity<Armor>(armor =>
            {
                armor.Property(p => p.ArmorTypeId)
                    .HasConversion<int>();
                armor.Property(p => p.BonusAbilityId)
                    .HasConversion<int>();
                armor.HasOne(p => p.BonusAbility)
                    .WithMany(p => p.Armors);
                armor.HasOne(p => p.ArmorType)
                    .WithMany(p => p.Armors);
            });

            modelBuilder.Entity<Weapon>(weapon =>
            {
                weapon.Property(p => p.WeaponTypeId)
                    .HasConversion<int>();
                weapon.HasOne(p => p.WeaponType)
                    .WithMany(p => p.Weapons);
                weapon.Property(p => p.DamageTypeId)
                    .HasConversion<int>();
                weapon.HasOne(p => p.DamageType)
                    .WithMany(p => p.Weapons);
            });

            modelBuilder.Entity<Alignment>(alignment =>
            {
                alignment.Property(p => p.AlignmentSideId)
                    .HasConversion<int>();
                alignment.HasIndex(p => p.AlignmentSideId)
                    .IsUnique(false);
                alignment.Property(p => p.AlignmentStrengthId)
                    .HasConversion<int>();
                alignment.HasIndex(p => p.AlignmentStrengthId)
                    .IsUnique(false);
                alignment.HasOne(p => p.AlignmentSide)
                    .WithMany(p => p.Alignments);
                alignment.HasOne(p => p.AlignmentStrength)
                    .WithMany(p => p.Alignments);
            });

            modelBuilder.Entity<RaceBonus>(raceBonus =>
            {
                raceBonus.Property(p => p.AbilityId)
                    .HasConversion<int>();
                raceBonus.HasOne(p => p.Ability)
                    .WithMany(p => p.RaceBonuses);
                raceBonus.HasOne(p => p.Subrace)
                    .WithOne(p => p.RaceBonus)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Race>(race =>
            {
                race.HasKey(p => p.Id);
                race.HasOne(p => p.RaceBonus)
                    .WithOne(p => p.Race)
                    .HasForeignKey<Race>(p => p.RaceBonusId);
                race.HasOne(p => p.Alignment)
                    .WithMany(p => p.Races);
                race.HasMany(p => p.Names)
                    .WithOne(p => p.Race)
                    .OnDelete(DeleteBehavior.Cascade);
                race.HasMany(p => p.NativeLanguages)
                    .WithMany(p => p.Races)
                    .UsingEntity<RaceLanguage>(
                        t => t
                            .HasOne(p => p.Language)
                            .WithMany(p => p.RaceLanguages)
                            .HasForeignKey(p => p.LanguageId),
                        t => t
                            .HasOne(p => p.Race)
                            .WithMany(p => p.RaceLanguages)
                            .HasForeignKey(p => p.RaceId),
                        t =>
                        {
                            t.HasKey(p => new { p.LanguageId, p.RaceId });
                        });
            });

            modelBuilder.Entity<Subrace>(subrace =>
            {
                subrace.HasOne(p => p.RaceBonus)
                    .WithOne(p => p.Subrace)
                    .HasForeignKey<Subrace>(p => p.RaceBonusId)
                    .OnDelete(DeleteBehavior.NoAction);
                subrace.HasOne(p => p.Race)
                    .WithMany(p => p.Subraces)
                    .HasForeignKey(p => p.RaceId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<HitPoints>(hitPoints =>
            {
                hitPoints.Property(p => p.AbilityId)
                    .HasConversion<int>();
            });

            modelBuilder.Entity<CharacterClass>(characterClass =>
            {
                characterClass
                    .Property(p => p.SavingThrowId)
                    .HasConversion<int>();
                characterClass
                    .HasOne(p => p.PrimaryAbility)
                    .WithMany()
                    .HasForeignKey(p => p.PrimaryAbilityId);
                characterClass
                    .HasOne(p => p.SavingThrow)
                    .WithMany()
                    .HasForeignKey(p => p.SavingThrowId)
                    .OnDelete(DeleteBehavior.NoAction);
                characterClass
                    .HasOne(p => p.HitPoints)
                    .WithOne(p => p.Class)
                    .HasForeignKey<CharacterClass>(p => p.Id)
                    .OnDelete(DeleteBehavior.NoAction);
                characterClass
                    .HasMany(p => p.WeaponSelectors)
                    .WithOne(p => p.Class)
                    .OnDelete(DeleteBehavior.Cascade);
                characterClass
                    .HasMany(p => p.ArmorTypes)
                    .WithMany(p => p.Classes);
                characterClass
                    .HasMany(p => p.WeaponTypes)
                    .WithMany(p => p.Classes);
                characterClass
                    .HasMany(p => p.Tools)
                    .WithMany(p => p.Classes)
                    .UsingEntity<ToolClass>(
                        t => t
                            .HasOne(p => p.Tool)
                            .WithMany(p => p.ToolClasses)
                            .HasForeignKey(p => p.ToolId),
                        t => t
                            .HasOne(p => p.CharacterClass)
                            .WithMany(p => p.ToolClasses)
                            .HasForeignKey(p => p.ClassId),
                        t =>
                        {
                            t.HasKey(p => new { p.ToolId, p.ClassId });
                        });
                characterClass.HasMany(p => p.Skills)
                    .WithMany(p => p.Classes)
                    .UsingEntity<SkillClass>
                    (
                            t => t
                            .HasOne(p => p.Skill)
                            .WithMany(p => p.SkillClasses)
                            .HasForeignKey(p => p.SkillId),
                        t => t
                            .HasOne(p => p.CharacterClass)
                            .WithMany(p => p.SkillClasses)
                            .HasForeignKey(p => p.ClassId),
                        t =>
                        {
                            t.HasKey(p => new { p.ClassId, p.SkillId });
                        });
                characterClass.HasMany(p => p.StartItems)
                    .WithMany(p => p.Classes)
                    .UsingEntity<ItemClass>
                    (
                        t => t
                            .HasOne(p => p.Item)
                            .WithMany(p => p.ItemClasses)
                            .HasForeignKey(p => p.ItemId),
                        t => t
                            .HasOne(p => p.CharacterClass)
                            .WithMany(p => p.ItemClasses)
                            .HasForeignKey(p => p.ClassId),
                        t =>
                        {
                            t.HasKey(p => new { p.ItemId, p.ClassId });
                        });
            });

            modelBuilder.Entity<WeaponSelector>(weaponSelector =>
            {
                 weaponSelector.HasMany(p => p.Weapons)
                    .WithMany(p => p.WeaponSelectors)
                    .UsingEntity<WeaponSelectorWeapon>
                    (
                        t => t
                            .HasOne(p => p.Weapon)
                            .WithMany(p => p.WeaponSelectorWeapons)
                            .HasForeignKey(p => p.WeaponId),
                        t => t
                            .HasOne(p => p.WeaponSelector)
                            .WithMany(p => p.WeaponSelectorWeapons)
                            .HasForeignKey(p => p.WeaponSelectorId),
                        t =>
                        {
                            t.HasKey(p => new { p.WeaponId, p.WeaponSelectorId });
                        });
            });   
        }
    }
}
