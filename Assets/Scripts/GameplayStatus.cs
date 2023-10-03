public class GameplayStatus
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

	public UNITTYPE unitType;

	public UNITTARGET unitTarget;

	public ATTACKTYPE attackType;

	public ATTACKTARGET attackTarget;

	public int level;

	public int id;

	public float healthReal;

	public float health;

	public float attackReal;

	public float attack;

	public float attackRadiusReal;

	public float attackRadius;

	public float attackSpeedReal;

	public float attackSpeed;

	public float speedReal;

	public float speed;

	public float range;

	public float priority;

	public GameplayCharacter character;

	public GameplayStatus(GameplayCharacter character, GlobalCard card)
	{
		this.character = character;
		InitStatus(card);
		ResetAllStatus();
	}

	public void ResetAllStatus()
	{
		health = healthReal;
		attack = attackReal;
		attackRadius = attackRadiusReal;
		attackSpeed = attackSpeedReal;
		speed = speedReal;
	}

	public void Damage(float damage)
	{
		health -= damage;
		if (health < 0f)
		{
			health = 0f;
		}
		else if (health > healthReal)
		{
			health = healthReal;
		}
	}

	private void InitStatus(GlobalCard card)
	{
		unitType = (UNITTYPE)card.unitType;
		unitTarget = (UNITTARGET)card.unitTarget;
		attackType = (ATTACKTYPE)card.attackType;
		attackTarget = (ATTACKTARGET)card.attackTarget;
		level = card.level;
		id = card.id;
		healthReal = card.health;
		attackReal = card.attack;
		attackRadiusReal = (float)card.attackRadius / 3f;
		attackSpeedReal = (float)card.attackSpeed / 10f;
		speedReal = (float)card.speed / 15f;
		range = (float)card.range / 3f;
		priority = card.priority;
	}
}
