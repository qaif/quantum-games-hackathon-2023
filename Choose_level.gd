extends Node2D


# Called when the node enters the scene tree for the first time.
func _ready():
	if GlobalVar.level_selected != 2 : 
		$VBoxContainer/ButtonContainer/MiddleBox/Level1.button_pressed = true
	else: 
		$VBoxContainer/ButtonContainer/MiddleBox/Level2.button_pressed = true
		


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass


func _on_level_1_pressed():
	GlobalVar.level_selected = 1
	$VBoxContainer/ButtonContainer/MiddleBox/Level2.button_pressed = false


func _on_level_2_pressed():
	GlobalVar.level_selected = 2
	$VBoxContainer/ButtonContainer/MiddleBox/Level1.button_pressed = false


func _on_back_pressed():
		get_tree().change_scene_to_file("res://menu.tscn")
