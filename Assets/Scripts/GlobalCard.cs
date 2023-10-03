public class GlobalCard
{
	public enum UNITTYPE
	{
		Castle = 1,
		Ground,
		Fly,
		Cast
	}

	public enum UNITTARGET
	{
		All = 1,
		GroundOnly,
		CastleOnly
	}

	public enum ATTACKTYPE
	{
		Melee = 1,
		Range,
		Magic,
		Cast
	}

	public enum ATTACKTARGET
	{
		Single = 1,
		Area
	}

	public enum RARITY
	{
		Common = 1,
		Rare,
		Epic,
		Legendary
	}

	public UNITTYPE unitType;

	public UNITTARGET unitTarget;

	public ATTACKTYPE attackType;

	public ATTACKTARGET attackTarget;

	public RARITY rarity;

	public int id;

	public int level;

	public int energy;

	public int health;

	public int healthIncrement;

	public int attack;

	public int attackIncrement;

	public int attackRadius;

	public int attackSpeed;

	public int speed;

	public int count;

	public int range;

	public int priority;

	private int[] rarityValue = new int[4]
	{
		100,
		110,
		125,
		150
	};

	public GlobalCard(int id, int level)
	{
		this.id = id;
		this.level = level;
		InitStatus();
	}

	public float GetPower()
	{
		if (id <= 0)
		{
			return 0f;
		}
		return (float)((int)rarity * rarityValue[(int)(rarity - 1)] * (level - 1)) + ((float)health / 10f + (float)attack * 1.5f) * (float)count;
	}

	private void InitStatus()
	{
		if (id == -1)
		{
			unitType = UNITTYPE.Castle;
			unitTarget = UNITTARGET.All;
			attackType = ATTACKTYPE.Range;
			attackTarget = ATTACKTARGET.Single;
			rarity = RARITY.Common;
			healthIncrement = 170;
			attackIncrement = 4;
			energy = 0;
			health = 2400 + healthIncrement * (level - 1);
			attack = 50 + attackIncrement * (level - 1);
			attackRadius = 0;
			attackSpeed = 10;
			speed = 0;
			count = 1;
			range = 7;
			priority = 1;
		}
		else if (id == 0)
		{
			unitType = UNITTYPE.Castle;
			unitTarget = UNITTARGET.All;
			attackType = ATTACKTYPE.Range;
			attackTarget = ATTACKTARGET.Single;
			rarity = RARITY.Common;
			healthIncrement = 115;
			attackIncrement = 4;
			energy = 0;
			health = 1400 + healthIncrement * (level - 1);
			attack = 50 + attackIncrement * (level - 1);
			attackRadius = 0;
			attackSpeed = 4;
			speed = 0;
			count = 1;
			range = 8;
			priority = 1;
		}
		else if (id == 1)
		{
			unitType = UNITTYPE.Cast;
			unitTarget = UNITTARGET.All;
			attackType = ATTACKTYPE.Cast;
			attackTarget = ATTACKTARGET.Area;
			rarity = RARITY.Common;
			healthIncrement = 1;
			attackIncrement = 13;
			energy = 2;
			health = 1 + healthIncrement * (level - 1);
			attack = 250 + attackIncrement * (level - 1);
			attackRadius = 2;
			attackSpeed = 0;
			speed = 0;
			count = 1;
			range = 2;
			priority = 1;
		}
		else if (id == 2)
		{
			unitType = UNITTYPE.Ground;
			unitTarget = UNITTARGET.GroundOnly;
			attackType = ATTACKTYPE.Range;
			attackTarget = ATTACKTARGET.Area;
			rarity = RARITY.Common;
			healthIncrement = 15;
			attackIncrement = 12;
			energy = 3;
			health = 150 + healthIncrement * (level - 1);
			attack = 110 + attackIncrement * (level - 1);
			attackRadius = 2;
			attackSpeed = 19;
			speed = 3;
			count = 1;
			range = 4;
			priority = 75;
		}
		else if (id == 3)
		{
			unitType = UNITTYPE.Ground;
			unitTarget = UNITTARGET.All;
			attackType = ATTACKTYPE.Range;
			attackTarget = ATTACKTARGET.Single;
			rarity = RARITY.Common;
			healthIncrement = 13;
			attackIncrement = 4;
			energy = 3;
			health = 125 + healthIncrement * (level - 1);
			attack = 40 + attackIncrement * (level - 1);
			attackRadius = 0;
			attackSpeed = 12;
			speed = 4;
			count = 2;
			range = 5;
			priority = 85;
		}
		else if (id == 4)
		{
			unitType = UNITTYPE.Ground;
			unitTarget = UNITTARGET.GroundOnly;
			attackType = ATTACKTYPE.Melee;
			attackTarget = ATTACKTARGET.Single;
			rarity = RARITY.Common;
			healthIncrement = 66;
			attackIncrement = 8;
			energy = 3;
			health = 560 + healthIncrement * (level - 1);
			attack = 75 + attackIncrement * (level - 1);
			attackRadius = 0;
			attackSpeed = 11;
			speed = 5;
			count = 1;
			range = 1;
			priority = 50;
		}
		else if (id == 5)
		{
			unitType = UNITTYPE.Cast;
			unitTarget = UNITTARGET.All;
			attackType = ATTACKTYPE.Cast;
			attackTarget = ATTACKTARGET.Area;
			rarity = RARITY.Rare;
			healthIncrement = 1;
			attackIncrement = 32;
			energy = 4;
			health = 1 + healthIncrement * (level - 1);
			attack = 510 + attackIncrement * (level - 1);
			attackRadius = 3;
			attackSpeed = 0;
			speed = 0;
			count = 1;
			range = 0;
			priority = 1;
		}
		else if (id == 6)
		{
			unitType = UNITTYPE.Ground;
			unitTarget = UNITTARGET.GroundOnly;
			attackType = ATTACKTYPE.Melee;
			attackTarget = ATTACKTARGET.Single;
			rarity = RARITY.Rare;
			healthIncrement = 66;
			attackIncrement = 32;
			energy = 4;
			health = 600 + healthIncrement * (level - 1);
			attack = 325 + attackIncrement * (level - 1);
			attackRadius = 0;
			attackSpeed = 18;
			speed = 7;
			count = 1;
			range = 1;
			priority = 30;
		}
		else if (id == 7)
		{
			unitType = UNITTYPE.Ground;
			unitTarget = UNITTARGET.All;
			attackType = ATTACKTYPE.Range;
			attackTarget = ATTACKTARGET.Single;
			rarity = RARITY.Rare;
			healthIncrement = 35;
			attackIncrement = 11;
			energy = 4;
			health = 340 + healthIncrement * (level - 1);
			attack = 100 + attackIncrement * (level - 1);
			attackRadius = 0;
			attackSpeed = 11;
			speed = 5;
			count = 1;
			range = 6;
			priority = 65;
		}
		else if (id == 8)
		{
			unitType = UNITTYPE.Ground;
			unitTarget = UNITTARGET.CastleOnly;
			attackType = ATTACKTYPE.Melee;
			attackTarget = ATTACKTARGET.Single;
			rarity = RARITY.Rare;
			healthIncrement = 210;
			attackIncrement = 13;
			energy = 5;
			health = 2000 + healthIncrement * (level - 1);
			attack = 126 + attackIncrement * (level - 1);
			attackRadius = 0;
			attackSpeed = 15;
			speed = 2;
			count = 1;
			range = 1;
			priority = 20;
		}
		else if (id == 9)
		{
			unitType = UNITTYPE.Ground;
			unitTarget = UNITTARGET.GroundOnly;
			attackType = ATTACKTYPE.Melee;
			attackTarget = ATTACKTARGET.Single;
			rarity = RARITY.Epic;
			healthIncrement = 120;
			attackIncrement = 23;
			energy = 5;
			health = 1210 + healthIncrement * (level - 1);
			attack = 220 + attackIncrement * (level - 1);
			attackRadius = 0;
			attackSpeed = 15;
			speed = 5;
			count = 1;
			range = 1;
			priority = 35;
		}
		else if (id == 10)
		{
			unitType = UNITTYPE.Fly;
			unitTarget = UNITTARGET.All;
			attackType = ATTACKTYPE.Magic;
			attackTarget = ATTACKTARGET.Area;
			rarity = RARITY.Epic;
			healthIncrement = 80;
			attackIncrement = 11;
			energy = 4;
			health = 800 + healthIncrement * (level - 1);
			attack = 100 + attackIncrement * (level - 1);
			attackRadius = 2;
			attackSpeed = 18;
			speed = 7;
			count = 1;
			range = 3;
			priority = 70;
		}
		else if (id == 11)
		{
			unitType = UNITTYPE.Ground;
			unitTarget = UNITTARGET.GroundOnly;
			attackType = ATTACKTYPE.Melee;
			attackTarget = ATTACKTARGET.Single;
			rarity = RARITY.Epic;
			healthIncrement = 4;
			attackIncrement = 4;
			energy = 4;
			health = 32 + healthIncrement * (level - 1);
			attack = 32 + attackIncrement * (level - 1);
			attackRadius = 0;
			attackSpeed = 10;
			speed = 9;
			count = 20;
			range = 1;
			priority = 60;
		}
		else if (id == 12)
		{
			unitType = UNITTYPE.Ground;
			unitTarget = UNITTARGET.All;
			attackType = ATTACKTYPE.Magic;
			attackTarget = ATTACKTARGET.Area;
			rarity = RARITY.Epic;
			healthIncrement = 34;
			attackIncrement = 13;
			energy = 5;
			health = 340 + healthIncrement * (level - 1);
			attack = 130 + attackIncrement * (level - 1);
			attackRadius = 2;
			attackSpeed = 17;
			speed = 5;
			count = 1;
			range = 5;
			priority = 70;
		}
		else if (id == 13)
		{
			unitType = UNITTYPE.Ground;
			unitTarget = UNITTARGET.All;
			attackType = ATTACKTYPE.Range;
			attackTarget = ATTACKTARGET.Single;
			rarity = RARITY.Common;
			healthIncrement = 1;
			attackIncrement = 1;
			energy = 2;
			health = 52 + healthIncrement * (level - 1);
			attack = 24 + attackIncrement * (level - 1);
			attackRadius = 0;
			attackSpeed = 13;
			speed = 8;
			count = 3;
			range = 5;
			priority = 80;
		}
		else if (id == 14)
		{
			unitType = UNITTYPE.Ground;
			unitTarget = UNITTARGET.GroundOnly;
			attackType = ATTACKTYPE.Melee;
			attackTarget = ATTACKTARGET.Single;
			rarity = RARITY.Common;
			healthIncrement = 9;
			attackIncrement = 5;
			energy = 2;
			health = 80 + healthIncrement * (level - 1);
			attack = 50 + attackIncrement * (level - 1);
			attackRadius = 0;
			attackSpeed = 11;
			speed = 8;
			count = 3;
			range = 1;
			priority = 45;
		}
		else if (id == 15)
		{
			unitType = UNITTYPE.Ground;
			unitTarget = UNITTARGET.GroundOnly;
			attackType = ATTACKTYPE.Melee;
			attackTarget = ATTACKTARGET.Area;
			rarity = RARITY.Rare;
			healthIncrement = 90;
			attackIncrement = 13;
			energy = 4;
			health = 880 + healthIncrement * (level - 1);
			attack = 120 + attackIncrement * (level - 1);
			attackRadius = 2;
			attackSpeed = 15;
			speed = 4;
			count = 1;
			range = 1;
			priority = 40;
		}
		else if (id == 16)
		{
			unitType = UNITTYPE.Fly;
			unitTarget = UNITTARGET.All;
			attackType = ATTACKTYPE.Melee;
			attackTarget = ATTACKTARGET.Single;
			rarity = RARITY.Common;
			healthIncrement = 10;
			attackIncrement = 4;
			energy = 3;
			health = 90 + healthIncrement * (level - 1);
			attack = 40 + attackIncrement * (level - 1);
			attackRadius = 0;
			attackSpeed = 10;
			speed = 8;
			count = 3;
			range = 2;
			priority = 55;
		}
		else if (id == 17)
		{
			unitType = UNITTYPE.Castle;
			unitTarget = UNITTARGET.All;
			attackType = ATTACKTYPE.Melee;
			attackTarget = ATTACKTARGET.Area;
			rarity = RARITY.Rare;
			healthIncrement = 90;
			attackIncrement = 10;
			energy = 5;
			health = 900 + healthIncrement * (level - 1);
			attack = 100 + attackIncrement * (level - 1);
			attackRadius = 2;
			attackSpeed = 16;
			speed = 0;
			count = 1;
			range = 6;
			priority = 5;
		}
		else if (id == 18)
		{
			unitType = UNITTYPE.Ground;
			unitTarget = UNITTARGET.GroundOnly;
			attackType = ATTACKTYPE.Melee;
			attackTarget = ATTACKTARGET.Single;
			rarity = RARITY.Epic;
			healthIncrement = 220;
			attackIncrement = 11;
			energy = 6;
			health = 2200 + healthIncrement * (level - 1);
			attack = 110 + attackIncrement * (level - 1);
			attackRadius = 0;
			attackSpeed = 15;
			speed = 3;
			count = 1;
			range = 1;
			priority = 25;
		}
		else if (id == 19)
		{
			unitType = UNITTYPE.Fly;
			unitTarget = UNITTARGET.CastleOnly;
			attackType = ATTACKTYPE.Melee;
			attackTarget = ATTACKTARGET.Single;
			rarity = RARITY.Epic;
			healthIncrement = 105;
			attackIncrement = 60;
			energy = 5;
			health = 1050 + healthIncrement * (level - 1);
			attack = 620 + attackIncrement * (level - 1);
			attackRadius = 0;
			attackSpeed = 30;
			speed = 3;
			count = 1;
			range = 2;
			priority = 20;
		}
		else if (id == 20)
		{
			unitType = UNITTYPE.Fly;
			unitTarget = UNITTARGET.All;
			attackType = ATTACKTYPE.Magic;
			attackTarget = ATTACKTARGET.Area;
			rarity = RARITY.Legendary;
			healthIncrement = 300;
			attackIncrement = 3;
			energy = 5;
			health = 3000 + healthIncrement * (level - 1);
			attack = 10 + attackIncrement * (level - 1);
			attackRadius = 2;
			attackSpeed = 15;
			speed = 1;
			count = 1;
			range = 2;
			priority = 15;
		}
		else if (id == 21)
		{
			unitType = UNITTYPE.Ground;
			unitTarget = UNITTARGET.GroundOnly;
			attackType = ATTACKTYPE.Melee;
			attackTarget = ATTACKTARGET.Area;
			rarity = RARITY.Legendary;
			healthIncrement = 120;
			attackIncrement = 7;
			energy = 4;
			health = 900 + healthIncrement * (level - 1);
			attack = 65 + attackIncrement * (level - 1);
			attackRadius = 5;
			attackSpeed = 15;
			speed = 4;
			count = 1;
			range = 1;
			priority = 10;
		}
	}
}
