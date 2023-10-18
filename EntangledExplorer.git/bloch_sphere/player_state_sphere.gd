@tool
extends Node2D



# Called when the node enters the scene tree for the first time.
func _ready():
	pass

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

var state_images = [
	preload("res://resources/qubit_state/moinsi_state.png"),
	preload("res://resources/qubit_state/minus_state.png"),
	preload("res://resources/qubit_state/one_state.png"),
	preload("res://resources/qubit_state/plusi_state.png"),
	preload("res://resources/qubit_state/plus_state.png"),
	preload("res://resources/qubit_state/zero_state.png")
]

func set_state(state):
	match state:
		"-i":
			$Sprite2D.texture = state_images[0]
		"-":
			$Sprite2D.texture = state_images[1]
		"1":
			$Sprite2D.texture = state_images[2]
		"i":
			$Sprite2D.texture = state_images[3]
		"+":
			$Sprite2D.texture = state_images[4]
		"0":
			$Sprite2D.texture = state_images[5]
