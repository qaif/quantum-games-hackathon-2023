extends Node2D

var cosmic_ray = preload("res://resources/CosmicRay/CosmicRay.tscn")

var map_width = 3000 # TODO : set as parameter
var map_height_detroy_limit = 2000

var spawn_timer = 0  # Timer to control spawning

# TODO: Might cause a memory leak, to fix later
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	spawn_timer += delta  # Increment the timer by the elapsed time
	if spawn_timer >= 0.05:  # Check if 0.1 second has passed
		spawn_timer = 0  # Reset the timer
		spawn_cosmic_ray()  # Spawn a new CosmicRay


func spawn_cosmic_ray():
	var instance = cosmic_ray.instantiate()
	var random_x = randf_range(0, map_width)  # Generate a random X coordinate
	var random_y = randf_range(-100, 0)  # Generate a random Y coordinate
	instance.position = Vector2(random_x, random_y)  # Set the position of the CosmicRay
	add_child(instance)  # Add the CosmicRay to the scene tree

	


func _on_button_pressed():
	# Load the current scene again
	get_tree().change_scene_to_file("res://scenes/level2.tscn")
