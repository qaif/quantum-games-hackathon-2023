extends Node2D

@export var speed = 500

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	check_position()

func _physics_process(delta):
	position.y += speed*delta

func check_position():
	var viewport_size = get_viewport_rect().size
	if position.y > viewport_size.y:
		queue_free()  # This will delete the object from the scene

func _on_rigid_body_2d_body_entered(body):
	body.get_hit(1.0)
	print("Hit !")
