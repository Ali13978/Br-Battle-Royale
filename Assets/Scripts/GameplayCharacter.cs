using System.Collections;
using UnityEngine;

public class GameplayCharacter : MonoBehaviour
{
	public enum ANIMATE
	{
		Standby = 1,
		Move,
		Attack
	}

	public delegate void OnBoolFinishedEvent(bool evt);

	private UnityEngine.AI.NavMeshAgent agent;

	private Gameplay gameplay;

	private GameplayStatus status;

	private Animator animator;

	private ANIMATE animate;

	private GameplayCharacter moveTarget;

	private GameplayCharacter attackTarget;

	private Transform cacheT;

	private Transform projectile;

	private Transform area;

	private Transform bar;

	private Transform healthBar;

	private Transform characterSprite;

	private Transform character;

	private Transform bodyCollider;

	private Transform findCollider;

	private Transform attackCollider;

	private Transform movePath;

	private Vector3 movePathV3;

	private bool isPlayer;

	private bool isMoveAbleSpace;

	private bool isHit;

	private bool isMove;

	private bool isAttack;

	private bool isDeadOnce;

	private int countHit;

	private float xBefore;

	private int[] hitImpact = new int[21]
	{
		6,
		1,
		1,
		2,
		4,
		2,
		1,
		1,
		3,
		5,
		1,
		7,
		1,
		1,
		2,
		1,
		4,
		3,
		4,
		4,
		3
	};

	private int[] attackSound = new int[22]
	{
		0,
		1,
		0,
		0,
		3,
		4,
		2,
		0,
		7,
		10,
		11,
		9,
		11,
		0,
		9,
		10,
		9,
		11,
		13,
		11,
		16,
		15
	};

	private float[] attackVolume = new float[22]
	{
		0.71f,
		0.81f,
		0.71f,
		0.61f,
		0.71f,
		0.61f,
		0.71f,
		0.71f,
		0.91f,
		0.91f,
		0.61f,
		0.91f,
		0.61f,
		0.71f,
		0.91f,
		0.91f,
		0.81f,
		0.61f,
		0.81f,
		0.61f,
		0.91f,
		0.81f
	};

	public void InitMoveAbleSpace()
	{
		gameplay = Gameplay.gameplay;
		cacheT = base.transform;
		projectile = cacheT.Find("Projectile");
		area = cacheT.Find("Area");
		characterSprite = cacheT.Find("CharacterSprite");
		character = characterSprite.Find("Character");
		agent = cacheT.GetComponent<UnityEngine.AI.NavMeshAgent>();
		if (agent != null)
		{
			agent.stoppingDistance = 0f;
		}
		bar = characterSprite.Find("Bar");
		if (bar != null)
		{
			bar.gameObject.SetActive(value: false);
			healthBar = bar.Find("HealthBar");
		}
		if (base.gameObject.name.Contains("Player"))
		{
			isPlayer = true;
		}
		else
		{
			isPlayer = false;
		}
		if (isPlayer)
		{
			isMoveAbleSpace = true;
		}
		else
		{
			isMoveAbleSpace = false;
		}
		Transform transform = cacheT;
		Vector3 localPosition = cacheT.localPosition;
		float x = localPosition.x;
		Vector3 localPosition2 = cacheT.localPosition;
		transform.localPosition = new Vector3(x, localPosition2.y, -0.018333f);
		if (area != null)
		{
			area.gameObject.SetActive(value: true);
		}
	}

	public void InitCharacter(GlobalCard card)
	{
		Reset();
		bodyCollider = cacheT.Find("BodyCollider");
		findCollider = cacheT.Find("FindCollider");
		attackCollider = cacheT.Find("AttackCollider");
		isMoveAbleSpace = false;
		Transform transform = cacheT;
		Vector3 localPosition = cacheT.localPosition;
		float x = localPosition.x;
		Vector3 localPosition2 = cacheT.localPosition;
		transform.localPosition = new Vector3(x, localPosition2.y, -0.018333f);
		status = new GameplayStatus(this, card);
		if (findCollider != null)
		{
			findCollider.GetComponent<SphereCollider>().radius = 2f;
		}
		if (attackCollider != null)
		{
			attackCollider.GetComponent<SphereCollider>().radius = status.range;
		}
		if (area != null)
		{
			area.localScale = new Vector3(status.range, status.range, 1f);
		}
		characterSprite = cacheT.Find("CharacterSprite");
		bar = characterSprite.Find("Bar");
		if (bar != null)
		{
			bar.Find("TextLevel").GetComponent<TextMesh>().text = string.Empty + status.level;
		}
		Animate(ANIMATE.Standby);
		StartCoroutine(InitMovePath(isFirstTime: true, delegate
		{
		}));
		isDeadOnce = false;
	}

	public void InitCastle(GlobalCard card)
	{
		InitMoveAbleSpace();
		InitCharacter(card);
	}

	private void Update()
	{
		if (isMoveAbleSpace)
		{
			Vector3 vector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			Transform spaceNow = gameplay.global.spaceNow;
			if (spaceNow == null)
			{
				isMoveAbleSpace = false;
				return;
			}
			Vector3 localPosition = cacheT.localPosition;
			float x = localPosition.x;
			Vector3 localPosition2 = cacheT.localPosition;
			float y = localPosition2.y;
			Vector3 localPosition3 = cacheT.localPosition;
			float z = localPosition3.z;
			float x2 = vector.x;
			Vector3 localPosition4 = spaceNow.localPosition;
			float x3 = localPosition4.x;
			Vector3 localScale = spaceNow.localScale;
			if (x2 >= x3 - localScale.x / 2f)
			{
				float x4 = vector.x;
				Vector3 localPosition5 = spaceNow.localPosition;
				float x5 = localPosition5.x;
				Vector3 localScale2 = spaceNow.localScale;
				if (x4 <= x5 + localScale2.x / 2f)
				{
					x = vector.x;
				}
			}
			float z2 = vector.z;
			Vector3 localPosition6 = spaceNow.localPosition;
			float y2 = localPosition6.y;
			Vector3 localScale3 = spaceNow.localScale;
			if (z2 >= y2 - localScale3.y / 2f)
			{
				float z3 = vector.z;
				Vector3 localPosition7 = spaceNow.localPosition;
				float y3 = localPosition7.y;
				Vector3 localScale4 = spaceNow.localScale;
				if (z3 <= y3 + localScale4.y / 2f)
				{
					y = vector.z;
				}
			}
			cacheT.localPosition = new Vector3(x, y, z);
			Transform transform = characterSprite;
			Vector3 localPosition8 = characterSprite.localPosition;
			float x6 = localPosition8.x;
			Vector3 localPosition9 = cacheT.localPosition;
			transform.localPosition = new Vector2(x6, 1f - localPosition9.y / 100f);
		}
		else if (isHit)
		{
			Hit();
		}
		else if (isMove)
		{
			Move();
		}
		else if (isAttack)
		{
			Attack();
		}
	}

	private IEnumerator InitMovePath(bool isFirstTime, OnBoolFinishedEvent evt)
	{
		if (area != null)
		{
			area.gameObject.SetActive(value: false);
		}
		if (bodyCollider != null)
		{
			bodyCollider.gameObject.SetActive(value: true);
		}
		if (bar != null)
		{
			if (isPlayer)
			{
				healthBar.Find("Bar").GetComponent<SpriteRenderer>().color = new Color(0.31f, 1f, 0f, 1f);
				bar.Find("TextLevelUnused").GetComponent<SpriteRenderer>().color = new Color(0.31f, 1f, 0f, 1f);
				bar.Find("TextLevel").GetComponent<TextMesh>().color = new Color(0f, 0f, 0f, 1f);
			}
			else
			{
				healthBar.Find("Bar").GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
				bar.Find("TextLevelUnused").GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
				bar.Find("TextLevel").GetComponent<TextMesh>().color = new Color(1f, 1f, 1f, 1f);
			}
		}
		if (base.gameObject != null)
		{
			if (status.speed > 0f && gameplay.IsCastleAlive(!isPlayer, 1))
			{
				if (this.movePath == null)
				{
					string enemy = "Enemy";
					if (!isPlayer)
					{
						enemy = "Player";
					}
					Transform movePath = gameplay.global.movePath;
					if (Vector2.Distance(cacheT.localPosition, movePath.Find("Castle" + enemy + "2").localPosition) <= Vector2.Distance(cacheT.localPosition, movePath.Find("Castle" + enemy + "3").localPosition))
					{
						if (gameplay.IsCastleAlive(!isPlayer, 2))
						{
							this.movePath = movePath.Find("Castle" + enemy + "2");
						}
						else
						{
							this.movePath = movePath.Find("Castle" + enemy + "1");
						}
					}
					else if (gameplay.IsCastleAlive(!isPlayer, 3))
					{
						this.movePath = movePath.Find("Castle" + enemy + "3");
					}
					else
					{
						this.movePath = movePath.Find("Castle" + enemy + "1");
					}
					float randomX = UnityEngine.Random.Range(-0.8f, 0.8f);
					movePathV3 = this.movePath.position + new Vector3(randomX, 0f, 0f);
				}
				agent.updateRotation = false;
				agent.avoidancePriority = (int)status.priority;
				agent.enabled = true;
				agent.speed = status.speed;
				agent.SetDestination(movePathV3);
				yield return new WaitForEndOfFrame();
				agent.isStopped = false;
				if (isFirstTime)
				{
					yield return new WaitForEndOfFrame();
					yield return new WaitForEndOfFrame();
					agent.isStopped = true;
					yield return new WaitForSeconds(1f + UnityEngine.Random.Range(-0.5f, 1f));
					agent.isStopped = false;
				}
				attackCollider.gameObject.SetActive(value: false);
				findCollider.gameObject.SetActive(value: true);
				isMove = true;
				Vector3 position = character.position;
				xBefore = position.x;
				Animate(ANIMATE.Move);
				evt(evt: true);
			}
			else
			{
				if (attackCollider != null)
				{
					attackCollider.gameObject.SetActive(value: false);
				}
				if (findCollider != null)
				{
					findCollider.gameObject.SetActive(value: true);
				}
				if (status.unitType == GameplayStatus.UNITTYPE.Cast)
				{
					isAttack = true;
				}
				evt(evt: false);
			}
		}
		else
		{
			evt(evt: false);
		}
	}

	private bool IsDestinationReached()
	{
		if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f))
		{
			return true;
		}
		return false;
	}

	private void Hit()
	{
		countHit++;
		if (countHit == 1)
		{
			character.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
			return;
		}
		if (countHit == 2)
		{
			character.GetComponent<SpriteRenderer>().color = new Color(1f, 0.3f, 0.3f, 1f);
			return;
		}
		if (countHit == 3)
		{
			character.GetComponent<SpriteRenderer>().color = new Color(1f, 0.6f, 0.6f, 1f);
			return;
		}
		countHit = 0;
		isHit = false;
		character.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
	}

	private void Move()
	{
		if (moveTarget != null && moveTarget.IsTargetAble(this))
		{
			agent.SetDestination(moveTarget.GetPosition());
		}
		else
		{
			moveTarget = null;
		}
		float num = xBefore;
		Vector3 nextPosition = agent.nextPosition;
		if (num >= nextPosition.x)
		{
			cacheT.localScale = new Vector3(1f, 1f, 1f);
		}
		else
		{
			cacheT.localScale = new Vector3(-1f, 1f, 1f);
		}
		if (bar != null)
		{
			Transform transform = bar;
			Vector3 localScale = cacheT.localScale;
			transform.localScale = new Vector3(localScale.x, 1f, 1f);
		}
		Transform transform2 = characterSprite;
		Vector3 localPosition = characterSprite.localPosition;
		float x = localPosition.x;
		Vector3 localPosition2 = cacheT.localPosition;
		transform2.localPosition = new Vector2(x, 1f - localPosition2.y / 100f);
		if (IsDestinationReached() && moveTarget == null)
		{
			agent.isStopped = true;
			movePath = null;
			StartCoroutine(InitMovePath(isFirstTime: false, delegate(bool evt)
			{
				if (!evt)
				{
					isMove = false;
					agent.avoidancePriority = (int)(status.priority - 1f);
					Animate(ANIMATE.Standby);
				}
			}));
		}
		Vector3 position = cacheT.position;
		xBefore = position.x;
	}

	private void Attack()
	{
		isAttack = false;
		StartCoroutine(AttackCoroutine());
	}

	private IEnumerator AttackCoroutine()
	{
		yield return new WaitForSeconds(status.attackSpeed);
		if ((attackTarget != null && attackTarget.IsTargetAble(this)) || status.attackType == GameplayStatus.ATTACKTYPE.Cast)
		{
			if (status.unitType != GameplayStatus.UNITTYPE.Castle && status.unitType != GameplayStatus.UNITTYPE.Cast)
			{
				Vector3 position = cacheT.position;
				float x = position.x;
				Vector3 position2 = attackTarget.GetPosition();
				if (x >= position2.x)
				{
					cacheT.localScale = new Vector3(1f, 1f, 1f);
				}
				else
				{
					cacheT.localScale = new Vector3(-1f, 1f, 1f);
				}
				if (bar != null)
				{
					Transform transform = bar;
					Vector3 localScale = cacheT.localScale;
					transform.localScale = new Vector3(localScale.x, 1f, 1f);
				}
			}
			Animate(ANIMATE.Attack);
		}
		else
		{
			NoAttackTarget();
		}
	}

	private void NoAttackTarget()
	{
		attackTarget = null;
		if (moveTarget != null && !moveTarget.IsTargetAble(this))
		{
			moveTarget = null;
		}
		StartCoroutine(InitMovePath(isFirstTime: false, delegate
		{
		}));
	}

	private void Animate(ANIMATE animate)
	{
		if (animator == null)
		{
			animator = cacheT.GetComponent<Animator>();
		}
		if (animate == ANIMATE.Attack || this.animate != animate)
		{
			this.animate = animate;
			animator.Play(this.animate.ToString());
		}
	}

	private void Reset()
	{
		if (bodyCollider != null)
		{
			bodyCollider.gameObject.SetActive(value: false);
		}
		if (attackCollider != null)
		{
			attackCollider.gameObject.SetActive(value: false);
		}
		if (findCollider != null)
		{
			findCollider.gameObject.SetActive(value: false);
		}
		movePath = null;
		attackTarget = null;
		moveTarget = null;
		isMoveAbleSpace = false;
		isHit = false;
		isMove = false;
		isAttack = false;
	}

	private void Dead()
	{
		if (isDeadOnce)
		{
			return;
		}
		isDeadOnce = true;
		Reset();
		Transform transform = null;
		if (status.unitType != GameplayStatus.UNITTYPE.Cast)
		{
			transform = gameplay.global.gameplayGraveyard.GetPoof();
			transform.parent = cacheT.parent;
			transform.localPosition = cacheT.localPosition;
			Transform transform2 = transform;
			Vector3 localPosition = transform2.localPosition;
			Vector3 localPosition2 = characterSprite.localPosition;
			transform2.localPosition = localPosition - new Vector3(0f, 0f, localPosition2.y);
			transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
			transform.GetComponent<Animator>().Play("Poof");
		}
		if (base.gameObject.name.Contains("Castle"))
		{
			gameplay.global.gameplayCamera.InitShake(0.3f);
			if (gameplay.global.isStart)
			{
				gameplay.global.globalSound.AudioOnce(gameplay.global.gameplayGraveyard.resSound[12], 0.51f);
			}
			gameplay.DestroyCastle(isPlayer, int.Parse(string.Empty + base.gameObject.name[base.gameObject.name.Length - 1]));
			if (transform != null)
			{
				transform.localScale = new Vector3(2f, 2f, 2f);
			}
		}
		else if (transform != null)
		{
			transform.localScale = new Vector3(1f, 1f, 1f);
		}
		if (isPlayer)
		{
			gameplay.global.playerList.Remove(this);
		}
		else
		{
			gameplay.global.enemyList.Remove(this);
		}
		if (status.id <= 0)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			gameplay.global.gameplayGraveyard.AddCharacter(status.id, base.transform);
		}
	}

	private bool IsCollide(Transform t)
	{
		if (t.gameObject.name == "Area")
		{
			return false;
		}
		if (t != null && t.gameObject.activeInHierarchy)
		{
			GameplayStatus gameplayStatus = t.GetComponent<GameplayCharacter>().status;
			if (gameplayStatus == null)
			{
				return false;
			}
			if (IsPlayer() == gameplayStatus.character.IsPlayer())
			{
				return false;
			}
			if (status.unitTarget == GameplayStatus.UNITTARGET.All || (status.unitTarget == GameplayStatus.UNITTARGET.GroundOnly && gameplayStatus.unitType == GameplayStatus.UNITTYPE.Ground) || gameplayStatus.unitType == GameplayStatus.UNITTYPE.Castle)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public void EventAnimation(ANIMATE animate)
	{
		if (animate != ANIMATE.Attack)
		{
			return;
		}
		AudioOnce();
		if (status.attackType == GameplayStatus.ATTACKTYPE.Melee || status.attackType == GameplayStatus.ATTACKTYPE.Magic)
		{
			if (status.attackTarget == GameplayStatus.ATTACKTARGET.Single)
			{
				if (attackTarget != null)
				{
					attackTarget.GetComponent<GameplayCharacter>().ApplyDamage(this);
				}
				return;
			}
			Vector3 position = bodyCollider.position;
			if (attackTarget != null && status.attackType == GameplayStatus.ATTACKTYPE.Magic)
			{
				position = attackTarget.GetBodyCollider().position;
			}
			AreaDamage(position, status.attackRadius);
		}
		else if (status.attackType == GameplayStatus.ATTACKTYPE.Range)
		{
			if (attackTarget != null)
			{
				Transform arrow = gameplay.global.gameplayGraveyard.GetArrow();
				arrow.gameObject.SetActive(value: true);
				arrow.gameObject.name = "Projectile";
				arrow.SetParent(cacheT.parent);
				arrow.position = projectile.position;
				arrow.localPosition += new Vector3(0f, 0f, -2f);
				arrow.Find("DamageArea").GetComponent<SphereCollider>().radius = status.attackRadius;
				arrow.GetComponent<GameplayProjectile>().Init(this, attackTarget.GetComponent<GameplayCharacter>(), attackTarget.GetLocalPosition());
			}
		}
		else if (status.attackType == GameplayStatus.ATTACKTYPE.Cast)
		{
			Vector3 position2 = cacheT.position;
			AreaDamage(position2, status.attackRadius);
			gameplay.global.gameplayCamera.InitShake(0.125f);
		}
	}

	public void EndAnimation(ANIMATE animate)
	{
		if (animate != ANIMATE.Attack)
		{
			return;
		}
		if (status.attackType == GameplayStatus.ATTACKTYPE.Cast)
		{
			Dead();
		}
		else if (attackTarget != null && attackTarget.IsTargetAble(this))
		{
			bool flag = true;
			Transform transform = attackTarget.transform;
			Collider[] array = Physics.OverlapSphere(cacheT.position, status.range);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].gameObject.name == "BodyCollider" && array[i].transform.parent == transform)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				NoAttackTarget();
			}
			else
			{
				isAttack = true;
			}
		}
		else
		{
			NoAttackTarget();
		}
	}

	public void AreaDamage(Vector3 location, float radius)
	{
		Collider[] array = Physics.OverlapSphere(location, radius);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].gameObject.name == "BodyCollider")
			{
				Transform parent = array[i].transform.parent;
				if (IsCollide(parent))
				{
					parent.GetComponent<GameplayCharacter>().ApplyDamage(this);
				}
			}
		}
	}

	public void ApplyDamage(GameplayCharacter from)
	{
		if (from.status.id >= 1)
		{
			Transform hit = gameplay.global.gameplayGraveyard.GetHit();
			hit.gameObject.SetActive(value: true);
			hit.SetParent(gameplay.global.game);
			hit.localPosition = cacheT.localPosition + new Vector3(0f, 0f, -1.02f);
			hit.localRotation = Quaternion.Euler(0f, 0f, 0f);
			hit.GetComponent<Animator>().Play("Hit" + hitImpact[from.status.id - 1]);
		}
		if (from.status.attackType == GameplayStatus.ATTACKTYPE.Range && gameplay.global.isStart)
		{
			gameplay.global.globalSound.AudioOnce(gameplay.global.gameplayGraveyard.resSound[5], 0.31f);
		}
		float num = from.status.attack;
		if (from.status.unitType == GameplayStatus.UNITTYPE.Cast)
		{
			num /= 3f;
		}
		status.Damage(num);
		RefreshHealthBar();
		if (!isHit)
		{
			isHit = true;
		}
		if (status.health <= 0f)
		{
			Dead();
		}
	}

	public bool CollideFind(Transform t)
	{
		if (IsCollide(t))
		{
			GameplayCharacter component = t.GetComponent<GameplayCharacter>();
			GameplayStatus gameplayStatus = component.status;
			if (gameplayStatus.unitType != GameplayStatus.UNITTYPE.Castle)
			{
				moveTarget = component;
			}
			attackCollider.gameObject.SetActive(value: true);
			return true;
		}
		return false;
	}

	public bool CollideAttack(Transform t)
	{
		if (IsCollide(t))
		{
			isMove = false;
			if (agent != null)
			{
				agent.avoidancePriority = (int)(status.priority - 1f);
				agent.isStopped = true;
			}
			attackTarget = t.GetComponent<GameplayCharacter>();
			isAttack = true;
			Animate(ANIMATE.Standby);
			return true;
		}
		return false;
	}

	public bool IsPlayer()
	{
		return isPlayer;
	}

	public GameplayStatus GetStatus()
	{
		return status;
	}

	public void RefreshHealthBar()
	{
		if (bar != null)
		{
			healthBar.localScale = new Vector3(status.health / status.healthReal, 1f, 1f);
			bar.gameObject.SetActive(value: true);
		}
	}

	public Vector3 GetLocalPosition()
	{
		return cacheT.localPosition;
	}

	public Vector3 GetPosition()
	{
		return cacheT.position;
	}

	public Transform GetBodyCollider()
	{
		return bodyCollider;
	}

	public bool IsTargetAble(GameplayCharacter from)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return false;
		}
		if (from.IsPlayer() != IsPlayer())
		{
			if (status.health > 0f)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	private void AudioOnce()
	{
		if (status.id >= 0 && gameplay.global.isStart)
		{
			gameplay.global.globalSound.AudioOnce(gameplay.global.gameplayGraveyard.resSound[attackSound[status.id]], attackVolume[status.id]);
		}
	}
}
