extends Node2D

@export var speed = 500

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func _physics_process(delta):
	position.y += speed*delta

func _on_rigid_body_2d_body_entered(body):
	body.get_hit(2.0)
	print("test")
