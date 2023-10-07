extends CharacterBody2D
#Double jump 

var max_jumps = 2  # allow for double jump
var jump_count = 0  # track number of jumps

const SPEED = 300.0
const JUMP_VELOCITY = -400.0

# Get the gravity from the project settings to be synced with RigidBody nodes.
var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")

# Player quantum state
var quantum_state = '0'

@onready var anim = get_node("AnimationPlayer")

func _physics_process(delta):
	# Add the gravity.
	if not is_on_floor():
		velocity.y += gravity * delta
	
	# Reset jump count when on floor
	if  is_on_floor():
		jump_count = 0  

	# Handle Jump.
	if Input.is_action_just_pressed("ui_accept") and (is_on_floor() or jump_count < max_jumps):
		velocity.y = JUMP_VELOCITY
		jump_count += 1  # Increment jump count
		anim.play("JumpUp")

	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	var direction = Input.get_axis("ui_left", "ui_right")
	if direction==-1:
		get_node("AnimatedSprite2D").flip_h = true
		$AnimatedSprite2D.offset.x = -17
	elif direction == 1:
		get_node("AnimatedSprite2D").flip_h = false
		$AnimatedSprite2D.offset.x = 0
	if direction:
		velocity.x = direction * SPEED
		if velocity.y == 0:
			anim.play("Run")
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)
		if velocity.y ==0:
			anim.play("Idle")
	if velocity.y > 0:
		anim.play("JumpDown")

	move_and_slide()


func change_state(gate):
	match gate:
		"X":
			match quantum_state:
				'0': quantum_state = '1'
				'1': quantum_state = '0'
				'i': quantum_state = '-i'
				'-i': quantum_state = 'i'
		
		"Y":
			match quantum_state:
				'0': quantum_state = '1'
				'1': quantum_state = '0'
				'+': quantum_state = '-'
				'-': quantum_state = '+'
				
		"Z":
			match quantum_state:
				'+': quantum_state = '-'
				'-': quantum_state = '+'
				'i': quantum_state = '-i'
				'-i': quantum_state = 'i'
				
		"H":
			match quantum_state:
				'0': quantum_state = '+'
				'1': quantum_state = '-'
				'+': quantum_state = '0'
				'-': quantum_state = '1'
				'i': quantum_state = '-i'
				'-i': quantum_state = 'i'
				
		"P":
			match quantum_state:
				'+': quantum_state = 'i'
				'i': quantum_state = '-'
				'-': quantum_state = '-i'
				'-i': quantum_state = '+'
				
		_:
			print("Invalid gate")
	$"../CanvasLayer/PlayerStateSphere".set_state(quantum_state)
	return quantum_state


func get_hit(damage : float):
	$"../CanvasLayer/thermometer".increase_degree(damage)
