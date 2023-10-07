@tool
extends Node2D

@export var state_filter = "0"

# Called when the node enters the scene tree for the first time.
func _ready():
	pass
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	$PlayerStateSphere.set_state(state_filter)


func allow_passage() -> void:
	#self.queue_free()
	$RigidBody2D.collision_layer = 0
	$RigidBody2D.collision_mask = 0

func deny_passage() -> void:
	$RigidBody2D.collision_layer = 1 << 4
	$RigidBody2D.collision_mask = 1 << 4
	print("denied")
	


func _on_area_2d_body_entered(body):
	if get_node("../../Player").quantum_state == state_filter:
		allow_passage()
	else:
		deny_passage()
