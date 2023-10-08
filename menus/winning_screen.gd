extends Node2D


# Called when the node enters the scene tree for the first time.
func _ready():
	var singleton = get_node("/root/inv")
	singleton.level1 = true


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass


func _on_back_button_pressed():
	get_tree().change_scene_to_file("res://menus/winning_screen.tscn")
