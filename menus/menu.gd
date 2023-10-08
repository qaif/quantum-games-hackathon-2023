extends Node2D

# Called when the node enters the scene tree for the first time.
func _ready():
	var singleton = get_node("/root/inv")
	$VBoxContainer/ButtonContainer/MiddleBox/Level2.disabled = !singleton.level1
		


func _on_quit_button_pressed():
	get_tree().quit()

func _on_play_button_pressed():
	get_tree().change_scene_to_file("res://menus/level1_backstory.tscn")


func _on_option_button_pressed():
	get_tree().change_scene_to_file("res://menus/level2_backstory.tscn")
