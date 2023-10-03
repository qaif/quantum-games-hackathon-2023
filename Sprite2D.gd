extends Sprite2D


var current_state = 0

# This function is called whenever the timer times out
func _ready():
	set_state(current_state)  # Set initial stat

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
		0:
			texture = state_images[0]
		1:
			texture = state_images[1]
		2:
			texture = state_images[2]
		3:
			texture = state_images[3]
		4:
			texture = state_images[4]
		5:
			texture = state_images[5]



func _on_timer_timeout():
	set_state(current_state)
	current_state = (current_state + 1) % 6  # Cycle through states 0 to 5

